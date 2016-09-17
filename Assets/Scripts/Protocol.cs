using UnityEngine;
using System.Collections;

public class Protocol {
    public class Communication {
        public string command;
        public uint sent_at;
    }

    public class ConnectRequest: Communication {
        public string username;
    }
}
