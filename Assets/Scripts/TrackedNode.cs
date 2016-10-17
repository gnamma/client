using UnityEngine;
using System.Collections;
using Protocol;
using GUtils;

public class TrackedNode : MonoBehaviour {
    public int type;
    public string label;

    private Node node;
    private bool ready;

    public TrackedNode() {
        node = new Node();
    }

    public Node Node {
        get {
            node.type = type;
            node.label = label;
            node.position = Position;
            node.rotation = Rotation;

            return node;
        }
    }

    public uint PID {
        set {
            Debug.Log(node);
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

    public void UpdateNode(UpdateNode un) {
        transform.position = un.position.Vector3();
        Debug.Log("Updating node: " + un.position.z);
        // transform.rotation = un.rotation.Vector3();
    }
}
