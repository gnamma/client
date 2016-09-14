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

        ProcessRoom(doc.SelectNodes("room/el"));
	}

    private void ProcessRoom(XmlNodeList nodes) {
        foreach (XmlNode node in nodes) {
            if (node.Name != "el") {
                throw new Exception("Unknown GWML element" + node.Name);
            }

            if (node.Attributes["model"].Value == "box") {
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);

                go.name = node.Attributes["name"].Value;
            }
        }
    }
}