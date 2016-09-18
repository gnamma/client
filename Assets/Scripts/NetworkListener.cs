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

    public void Connect() {
        Connect(host, port);
    }

    public void Connect(string host, int port) {
        client = new TcpClient();
        client.Connect(host, port);
    }

    private void Send(object cmd) {
        string toSend = JsonUtility.ToJson(cmd);
        Debug.Log(toSend);

        byte[] data;
        data = Encoding.Default.GetBytes(toSend + "\n");
        client.GetStream().Write(data, 0, data.Length);
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
