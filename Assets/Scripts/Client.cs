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
        pid = cv.player_id;

        foreach(var tn in nodes) {
            StartCoroutine(RegisterNode(tn));
        }
    }

    public IEnumerator RegisterNode(TrackedNode tn) {
        var rn = new RegisterNode(tn.Node, pid);
        var ren = new RegisteredNode();

        gnsNet.Send(rn);
        yield return new WaitForCom(ref gnsNet, ren.command);
        ren = gnsNet.Read(ren);

        tn.ID = ren.nid;
        tn.PID = pid;

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
}
