using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Protocol;
using System.Threading;

public class Client : MonoBehaviour {
    public string alias = "parzival";
    public string environment = "pillars.gsml";

    public NetPlayer otherPlayer;
    public TrackedNode nodeAsset;

    public TrackedNode[] nodes;

    private GNSClient gnsNet;
    private AssetsClient asNet;
    private Builder builder;

    private uint pid;

    private Dictionary<uint, NetPlayer> otherPlayers = new Dictionary<uint, NetPlayer>();

    private int registeredNodes = 0;

    void Awake() {
        gnsNet = GetComponent<GNSClient>();
        asNet = GetComponent<AssetsClient>();

        builder = GetComponent<Builder>();
    }

    void Start() {
        gnsNet.Connect();
        asNet.Connect();

        StartCoroutine(StartSession());

        asNet.SendRawString(environment);
        string resp = asNet.ReadRaw();
        Debug.Log(resp);

        builder.LoadFromString(resp);
        builder.Build();
    }

    public IEnumerator StartSession() {
        Debug.Log("Starting session");
        ConnectRequest cr = new ConnectRequest(alias);
        gnsNet.Send(cr);

        ConnectVerdict cv = new ConnectVerdict();
        yield return new WaitForCom(ref gnsNet, cv.command);

        cv = gnsNet.Read(cv);

        GeneratePlayers(cv.players);

        pid = cv.player_id;

        foreach (var tn in nodes) {
            StartCoroutine(RegisterNode(tn));
        }

        StartCoroutine(PollJoins());
        StartCoroutine(PollUpdates());

        yield return new WaitUntil(() => {
            if (registeredNodes == nodes.Length) {
                return true;
            }

            return false;
        });

        Debug.Log("Registered all nodes... Ready to start experience!");
        gnsNet.Send(new RegisteredAllNodes(pid));
    }

    public IEnumerator RegisterNode(TrackedNode tn) {
        var rn = new RegisterNode(tn.Node, pid);
        var ren = new RegisteredNode();

        gnsNet.Send(rn);
        yield return new WaitForCom(ref gnsNet, ren.command);
        ren = gnsNet.Read(ren);

        tn.ID = ren.nid;
        tn.PID = pid;

        registeredNodes++;

        StartCoroutine(UpdateNode(tn));
    }

    public IEnumerator UpdateNode(TrackedNode tn) {
        while (true) {
            if (tn.Ready()) {
                break;
            }

            yield return new WaitForSeconds(0.1f);
        }

        while (true) {
            var un = new UpdateNode(tn.Node);
            gnsNet.Send(un);

            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator PollJoins() {
        Debug.Log("Starting join poll...");
        while (true) {
            var jr = new JoinRoom();

            yield return new WaitForCom(ref gnsNet, jr.command);
            Debug.Log("Got join com");
            jr = gnsNet.Read(jr);

            GeneratePlayer(jr.player);
        }
    }

    public void GeneratePlayers(Player[] players) {
        Debug.Log("Generating players...");
        foreach (var player in players) {
            GeneratePlayer(player);
        }
    }

    public void GeneratePlayer(Player player) {
        var op = (NetPlayer)Instantiate(otherPlayer);

        Debug.Log(player);

        op.username = player.username;
        op.id = player.id;

        foreach (var node in player.nodes) {
            nodeAsset.label = node.label;
            nodeAsset.type = node.type;

            var n = (TrackedNode)Instantiate(nodeAsset, node.position.Vector3(), new Quaternion(), op.transform);
            Debug.Log(player.id);
            n.PID = node.pid;
            n.ID = node.id;
        }

        op.InitNodes();

        otherPlayers.Add(op.id, op);
    }

    public IEnumerator PollUpdates() {
        while (true) {
            var un = new UpdateNode();
            yield return new WaitForCom(ref gnsNet, un.command);
            un = gnsNet.Read(un);

            if (un.pid == pid || !otherPlayers.ContainsKey(un.pid)) {
                continue; // We don't care about our own movement or players not yet registered;
            }

            var op = otherPlayers[un.pid];
            op.UpdateNode(un);
        }
    }
}
