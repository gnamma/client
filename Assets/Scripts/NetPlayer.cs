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
        Debug.Log("Updating: " + un.nid);

        foreach (var node in nodes) {
            Debug.Log("Going through... " + node.Node.id);
            if (node.Node.id == un.nid) {
                Debug.Log("Found Node with ID");
                node.UpdateNode(un);
                break;
            }
        }
    }
}