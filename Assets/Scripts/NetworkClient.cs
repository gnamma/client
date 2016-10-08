using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.IO;

public class NetworkClient : MonoBehaviour {
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
        string jsonString = JsonUtility.ToJson(cmd);
        byte[] jsonBytes = Encoding.Default.GetBytes(jsonString);

        Debug.Log(jsonString);

        SendRaw(jsonBytes);
    }

    public void SendRawString(string s) {
        SendRaw(Encoding.Default.GetBytes(s));
    }

    public void SendRaw(byte[] data) {
        NetworkStream stream = client.GetStream();

        byte[] lenBytes = Encoding.Default.GetBytes(data.Length.ToString() + "\n");

        stream.Write(lenBytes, 0, lenBytes.Length);
        stream.Write(data, 0, data.Length);
    }

    public void Read<T>(ref T blob) where T : Protocol.Communication {
        string rec = ReadRaw();
        Debug.Log(rec);

        try {
            blob = JsonUtility.FromJson<T>(rec);

            Debug.Log(blob);
        } catch (Exception e) {
            Debug.Log("Error umarshalling JSON");
            Debug.Log(e);
        }
    }

    public string ReadRaw() {
        NetworkStream stream = client.GetStream();
        StreamReader reader = new StreamReader(stream);

        string lenStr = reader.ReadLine();
        int len = Int32.Parse(lenStr);

        byte[] data = new byte[len];
        stream.Read(data, 0, len);

        Debug.Log("read");

        return Encoding.Default.GetString(data);
    }
}