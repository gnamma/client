using UnityEngine;
using System.Collections;
using System.Xml;
using System;

public class RoomBuilder : MonoBehaviour {
    private XmlDocument doc;

	// Use this for initialization
	void Start () {
        TextAsset text = Resources.Load("world") as TextAsset;
        if (!text) {
            Debug.Log("Couldn't find world[.xml]");
            return;
        }

        doc = new XmlDocument();
        doc.LoadXml(text.ToString());

        ProcessRoom(doc.SelectNodes("room"));
	}

    private void ProcessRoom(XmlNodeList nodes) {
        foreach (XmlNode node in nodes) {
            if (node.Name == "el") {
                if (node.Attributes["model"].Value == "box") {
                    GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);

                    go.name = node.Attributes["name"].Value;
                }

                continue;
            }

            if (node.Name == "room") {
                ProcessRoom(node.ChildNodes);
                continue;
            }

            throw new Exception("Unknown GWML element " + node.Name);
        }
    }
}