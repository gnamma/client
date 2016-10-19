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

        received[cmd].Enqueue(comString);
        receivedEvents[cmd].Set();
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

        return true;
    }

    public T Read<T>(T blob) where T :Communication, new() {
        checkReceievedDefaults(blob.command);

        var comString = received[blob.command].Dequeue();

        var com = new T();

        FromString(comString, ref com);

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