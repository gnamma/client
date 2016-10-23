using UnityEngine;
using System.Collections;
using Protocol;
using GUtils;

public class TrackedNode : MonoBehaviour {
    public int type;
    public string label;

    private Node node;
    private bool ready;

    private UpdateNode update1;
    private UpdateNode update2;

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

        return false;
    }

    public void UpdateNode(UpdateNode un) {
        update2 = update1;
        update1 = un;
    }

    void Update() {
        if (update1 == null || update2 == null) {
            return;
        }

        var dt = (update1.sent_at - update2.sent_at) * 1e-9;
        var md = Vector3.Distance(update1.position.Vector3(), update2.position.Vector3());

        transform.position = Vector3.MoveTowards(transform.position, update2.position.Vector3(), (float) (md * dt));
    }
}
