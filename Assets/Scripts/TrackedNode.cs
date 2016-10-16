using UnityEngine;
using System.Collections;
using Protocol;
using GUtils;

public class TrackedNode : MonoBehaviour {
    public int type;
    public string label;

    private Node node;
    private bool ready;

    public Node Node {
        get {
            if (node == null) {
                node = new Node {
                    type = type,
                    label = label,
                    position = Position,
                    rotation = Rotation
                };
            } else {
                node.position = Position;
                node.rotation = Rotation;
            }

            return node;
        }
    }

    public uint PID {
        set {
            node.pid = value;
        }
    }

    public uint ID {
        set {
            node.id = value;
        }
    }

    public Point Position {
        get {
            return new Point(transform.position);
        }
    }

    public Point Rotation {
        get {
            return new Point(transform.rotation.eulerAngles);
        }
    }

    public bool Ready() {
        if (node.pid > 0 && node.id > 0) {
            Debug.Log("Ready to rumble!");
            return true;
        }

        Debug.Log("Need to hit the weights mroe bro: " + node.pid + ", " + node.id);

        return false;
    }
}
