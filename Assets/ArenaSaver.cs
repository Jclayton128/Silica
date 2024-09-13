using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaSaver : MonoBehaviour
{
    [ContextMenu("Gather Nodes")]
    public List<NodeHandler> GatherNodes()
    {
        List<NodeHandler> nodes = new List<NodeHandler>();

        List<NodeHandler> mainframes = new List<NodeHandler>();

        foreach (var node in gameObject.GetComponentsInChildren<NodeHandler>())
        {
            nodes.Add(node);
            if (node.NodeType == NodeHandler.NodeTypes.Mainframe)
            {
                mainframes.Add(node);
            }
        }
        Debug.Log($"Found {nodes.Count} nodes");
        if (mainframes.Count == 0) Debug.LogWarning($"Hey, I found no mainframes to exit!");

        return nodes;
    }

}
