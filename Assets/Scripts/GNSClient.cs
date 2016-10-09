using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

using Protocol;

public class GNSClient : NetworkClient {
    private Dictionary<string, List<Communication>> received = new Dictionary<string, List<Communication>>();
    private List<Communication> toSend = new List<Communication>();

    void Awake() {
        InvokeRepeating("SendComs", 0, 0.2f);
        InvokeRepeating("ReadComs", 0, 0.1f);
    }

    void ReadComs() {
        new Thread(() => {
            var com = new Communication();
            Read(ref com);

            if (!received.ContainsKey(com.command)) {
                received.Add(com.command, new List<Communication>());
            }

            received[com.command].Add(com);
        }).Start();
    }

    public T Read<T>(T blob) where T : Communication {
        for(;;) {
            if (received[blob.command].Count > 0) {
                var b = received[blob.command][0];

                received[blob.command].RemoveAt(0);
                return b as T;
            }
        }
   } 

    void SendComs() {
        foreach (var obj in toSend) {
            base.Send(obj);
        }

        toSend.Clear();
    }

    public new void Send(Communication cmd) {
        toSend.Add(cmd);
    }
}