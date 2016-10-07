using UnityEngine;
using System.Collections;
using Protocol;
using System.Threading;

public class Client : MonoBehaviour {
    public string alias = "parzival";
    public string environment = "pillars.gsml";

    private GNSClient gnsNet;
    private AssetsClient asNet;
    private Builder builder;

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

        asNet.SendRawString(environment);

        string resp = asNet.ReadRaw();
        Debug.Log(resp);

        builder.LoadFromString(resp);
        builder.Build();

        Debug.Log("Done!");
    }
}
