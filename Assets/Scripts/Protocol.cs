using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GUtils;
using System;

namespace Protocol {
    public class Communication {
        public string command;
        public uint sent_at;
    }

    public class ConnectRequest : Communication {
        public string username;

        public ConnectRequest(string u) {
            username = u;
            command = "connect_request";
        }
    }

    public class ConnectVerdict : Communication {
        public bool can_proceed;
        public string message;
        public uint player_id;
        public Player[] players;

        public ConnectVerdict() {
            command = "connect_verdict";
        }
    }

    public class RegisterNode : Communication {
        public Node node;
        public uint pid;

        public RegisterNode(Node n, uint p) {
            node = n;
            pid = p;

            command = "register_node";
        }
    }

    public class RegisteredNode : Communication {
        public uint nid;

        public RegisteredNode() {
            command = "registered_node";
        }
    }

    public class RegisteredAllNodes : Communication {
        public uint pid;
        
        public RegisteredAllNodes(uint p) {
            pid = p;

            command = "registered_all_nodes";
        }
    }

    public class UpdateNode : Communication {
        public uint pid;
        public uint nid;
        public Point position;
        public Point rotation;

        public UpdateNode(Node n) {
            pid = n.pid;
            nid = n.id;
            position = n.position;
            rotation = n.rotation;

            command = "update_node";
        }

        public UpdateNode() {
            command = "update_node";
        }
    }

    public class JoinRoom : Communication {
        public Player player;

        public JoinRoom() {
            command = "join_room";
        }
    }

    [Serializable]
    public class Node {
        public uint id;
        public int type;
        public uint pid;
        public Point position;
        public Point rotation;
        public string asset;
        public string label;
    }

    [Serializable]
    public class Player {
        public uint id;
        public string username;

        public Node[] nodes;
    }
}