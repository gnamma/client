using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.IO;

public class NetworkListener : MonoBehaviour {
    public string host;
    public int port;

    private TcpClient client;

    private byte[] datalen = new byte[4];

	void Start () {
        client = new TcpClient();
        client.Connect(host, port);

        NetworkStream stream = client.GetStream();

        Protocol.ConnectRequest cr = new Protocol.ConnectRequest();
        cr.command = "connect_request";
        cr.username = "paked";
        cr.sent_at = 22;

        Protocol.ConnectVerdict cv = new Protocol.ConnectVerdict();

        Send(cr, stream);
        Receive(ref cv);
    }

    private void Send(object cmd, NetworkStream stream) {
        string toSend = JsonUtility.ToJson(cmd);
        Debug.Log(toSend);

        byte[] data;
        data = Encoding.Default.GetBytes(toSend + "\n");
        stream.Write(data, 0, data.Length);
    }

    private void Receive<T>(ref T blob) {
        new Thread(() => {
            Protocol.ConnectVerdict conver = new Protocol.ConnectVerdict();
            StreamReader reader = new StreamReader(client.GetStream());

            string rec = reader.ReadLine();

            try {
                conver = JsonUtility.FromJson<Protocol.ConnectVerdict>(rec);

                Debug.Log(conver.message);
            } catch (Exception e) {
                Debug.Log("Error umarshalling JSON");
                Debug.Log(e);
            }

        }).Start();
    }
}
