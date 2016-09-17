using UnityEngine;
using System.Collections;
using System.Xml;
using System;

public class RoomBuilder : MonoBehaviour {
    public GameObject box;
    public GameObject plane;

    private XmlDocument doc;

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
                XmlAttribute model = node.Attributes["model"];

                if (model == null) {
                    throw new Exception("Node 'el' needs a 'model' attribute");
                }

                if (model.Value == "box") {
                    MakeBox(node);
                } else if (model.Value == "plane") {
                    MakePlane(node);
                }

                continue;
            }

            if (node.Name == "box") {
                MakeBox(node);

                continue;
            }

            if (node.Name == "plane") {
                MakePlane(node);

                continue;
            }

            if (node.Name == "room") {
                ProcessRoom(node.ChildNodes);

                continue;
            }

            throw new Exception("Unknown GWML element " + node.Name);
        }
    }

    private void MapAttributes(XmlNode node, GameObject go) {
        XmlAttribute name = node.Attributes["name"];
        if (name != null) {
            go.name = name.Value;
        }

        Vector3 scale = Vector3.one;
        XmlAttribute scaleX = node.Attributes["scale-x"];
        XmlAttribute scaleY = node.Attributes["scale-y"];
        XmlAttribute scaleZ = node.Attributes["scale-z"];

        if (scaleX != null) {
            scale.x = float.Parse(scaleX.Value);
        }

        if (scaleY != null) {
            scale.y = float.Parse(scaleY.Value);
        }

        if (scaleZ != null) {
            scale.z = float.Parse(scaleZ.Value);
        }

        go.transform.localScale = scale;

        Vector3 pos = Vector3.zero;
        XmlAttribute posX = node.Attributes["x"];
        XmlAttribute posY = node.Attributes["y"];
        XmlAttribute posZ = node.Attributes["z"];

        if (posX != null) {
            pos.x = float.Parse(posX.Value);
        }

        if (posY != null) {
            pos.y = float.Parse(posY.Value);
        }

        if (posZ != null) {
            pos.z = float.Parse(posZ.Value);
        }

        go.transform.position = pos;
    }

    private void MakeBox(XmlNode node) {
        GameObject go = Instantiate(box);

        MapAttributes(node, go);
    }

    private void MakePlane(XmlNode node) {
        GameObject go = Instantiate(plane);

        MapAttributes(node, go);
    }
}