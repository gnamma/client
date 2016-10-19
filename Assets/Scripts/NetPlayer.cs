using UnityEngine;
using System.Collections;

using Protocol;

public class NetPlayer : MonoBehaviour {
    public TrackedNode[] nodes;
    public uint id;
    public string username;

	void Start () {
        InitNodes();
	}

    public void InitNodes() {
        nodes = GetComponentsInChildren<TrackedNode>();
    }

    public void UpdateNode(UpdateNode un) {
        foreach (var node in nodes) {
            if (node.Node.id == un.nid) {
                node.UpdateNode(un);
                break;
            }
        }
    }
}