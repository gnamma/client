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

        // Begin handshake with server
        ConnectRequest cr = new ConnectRequest(alias);
        net.Send(cr);

        new Thread(() => {
            ConnectVerdict cv = new ConnectVerdict();
            net.Read(ref cv);

            Debug.Log(cv.message);
        }).Start();
        
    }
}
