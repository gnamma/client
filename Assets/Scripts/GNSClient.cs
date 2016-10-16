using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

using Protocol;

public class GNSClient : NetworkClient {
    private List<Communication> toSend = new List<Communication>();
    private Dictionary<string, Queue<string>> received = new Dictionary<string, Queue<string>>();
    private Dictionary<string, AutoResetEvent> receivedEvents = new Dictionary<string, AutoResetEvent>();

    void Update() {
        // Sending
        foreach (var c in toSend) {
            Debug.Log(c.command);
            base.Send(c);
        }

        toSend.Clear();

        // Reading
        if (!DataAvailable()) {
            return;
        }

        string cmd = "";
        var comString = Peek(ref cmd);

        checkReceievedDefaults(cmd);

        Debug.Log("queued: " + cmd);

        received[cmd].Enqueue(comString);
        receivedEvents[cmd].Set();

        Debug.Log("Dispatched!");
    }

    public string Peek(ref string cmd) {
        string rec = ReadRaw();
        var com = new Communication();

        FromString(rec, ref com);

        cmd = com.command;

        return rec;
    }

    public void Send(Communication com) {
        toSend.Add(com);
    }

    public bool ComAvailable(string cmd) {
        if (!received.ContainsKey(cmd)) {
            return false;
        }

        if (received[cmd].Count == 0) {
            return false;
        }

        Debug.Log(received[cmd].Count);

        return true;
    }

    public T Read<T>(T blob) where T :Communication, new() {
        checkReceievedDefaults(blob.command);
        Debug.Log(received[blob.command].Count);
        var comString = received[blob.command].Dequeue();

        var com = new T();

        FromString(comString, ref com);

        Debug.Log("here we go: " + com.command);

        Debug.Log(com is ConnectVerdict);

        return (T) com;
    }

    private void checkReceievedDefaults(string cmd) {
        if (!receivedEvents.ContainsKey(cmd)) {
            receivedEvents.Add(cmd, new AutoResetEvent(false));
        }

        if (!received.ContainsKey(cmd)) {
            received.Add(cmd, new Queue<string>());
        }
    }
}