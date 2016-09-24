using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.IO;

public class Net : MonoBehaviour {
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

    public void Send(object cmd) {
        string toSend = JsonUtility.ToJson(cmd);
        Debug.Log(toSend);

        byte[] data;
        data = Encoding.Default.GetBytes(toSend + "\n");
        client.GetStream().Write(data, 0, data.Length);
    }

    public void Read<T>(ref T blob) {
        string rec = ReadRaw();

        try {
            blob = JsonUtility.FromJson<T>(rec);

            Debug.Log(blob);
        } catch (Exception e) {
            Debug.Log("Error umarshalling JSON");
            Debug.Log(e);
        }
    }

    public string ReadRaw() {
        StreamReader reader = new StreamReader(client.GetStream());

        return reader.ReadLine();
    }
}
