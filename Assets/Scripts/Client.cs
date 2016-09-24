using UnityEngine;
using System.Collections;
using Protocol;
using System.Threading;

public class Client : MonoBehaviour {
    public string alias = "parzival";

    private Net net;
    private Builder builder;

	void Awake() {
        net = GetComponent<Net>();
        builder = GetComponent<Builder>();
	}

    void Start() {
        net.Connect();

        ConnectRequest cr = new ConnectRequest(alias);
        net.Send(cr);

        ConnectVerdict cv = new ConnectVerdict();
        net.Read(ref cv);

        if (!cv.can_proceed) {
            Debug.Log("Not allowed to proceed: " + cv.message);
            return;
        }

        AssetRequest ar = new AssetRequest("room.gsml");
        net.Send(ar);

        string resp = net.ReadRaw();
        Debug.Log(resp);

        builder.LoadFromString(resp);
        builder.Build();
    }
}
