using UnityEngine;
using System.Collections;

namespace Protocol {
    public class Communication {
        public string command;
        public uint sent_at;
    }

    public class ConnectRequest: Communication {
        public string username;
    }

    public class ConnectVerdict: Communication {
        public bool can_proceed;
        public string message;
    }
}