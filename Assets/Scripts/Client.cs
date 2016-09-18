using UnityEngine;
using System.Collections;
using Protocol;

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
        
        ConnectVerdict cv = new ConnectVerdict();
        net.Receive(ref cv);

        Debug.Log(cv.message);
    }
}
