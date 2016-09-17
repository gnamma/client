using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

public class NetworkListener : MonoBehaviour {
    public string host;
    public int port;

    private TcpClient client;
    private NetworkStream stream;

    private byte[] datalen = new byte[4];

	void Start () {
        client = new TcpClient();
        client.Connect(host, port);

        Protocol.ConnectRequest conreq = new Protocol.ConnectRequest();
        conreq.command = "connect_request";
        conreq.username = "paked";
        conreq.sent_at = 22;

        Send(conreq);
	}

    private void Send(object cmd) {
        stream = client.GetStream();

        string toSend = JsonUtility.ToJson(cmd);

        byte[] data;
        data = Encoding.Default.GetBytes(toSend);
        Debug.Log(data.ToString());
        stream.Write(data, 0, data.Length);
    }
}
