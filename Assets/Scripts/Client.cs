using UnityEngine;
using System.Collections;
using Protocol;
using System.Threading;

public class Client : MonoBehaviour {
    public string alias = "parzival";
    public string environment = "pillars.gsml";

    public TrackedNode[] nodes;

    private GNSClient gnsNet;
    private AssetsClient asNet;
    private Builder builder;

    private uint pid;

	void Awake() {
        gnsNet = GetComponent<GNSClient>();
        asNet = GetComponent<AssetsClient>();

        builder = GetComponent<Builder>();
	}

    void Start() {
        gnsNet.Connect();
        asNet.Connect();

        ConnectRequest cr = new ConnectRequest(alias);
        gnsNet.Send(cr);

        ConnectVerdict cv = new ConnectVerdict();
        gnsNet.Read(ref cv);

        if (!cv.can_proceed) {
            Debug.Log("Not allowed to proceed: " + cv.message);
            return;
        }

        pid = cv.player_id;

        RegisterNodes();

        asNet.SendRawString(environment);
        string resp = asNet.ReadRaw();
        Debug.Log(resp);

        builder.LoadFromString(resp);
        builder.Build();

        Debug.Log("Done!");
    }

    public void RegisterNodes() {
        foreach (var node in nodes) {
            Debug.Log(node);
            var rn = new RegisterNode(node.Node, pid);
            var ren = new RegisteredNode();

            gnsNet.Send(rn);

            gnsNet.Read(ref ren);
            if (ren.command == null) {
                Debug.LogError("Could not register a node: " + node.Node.label);
            }

            node.ID = ren.nid;
            node.PID = pid;
        }
    }
}
