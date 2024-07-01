using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeLibrary : MonoBehaviour
{
    public static NodeLibrary Instance { get; private set; }

    [SerializeField] NodeHandler _nodePrefab = null;

    [SerializeField] Sprite _activeNodeSprite = null;
    [SerializeField] Sprite _availableNodeSprite = null;



    private void Awake()
    {
        Instance = this;
    }

    public NodeHandler GetNodePrefab()
    {
        return _nodePrefab;
    }

    public Sprite GetCurrentNodeSprite()
    {
        return _activeNodeSprite;
    }

    public Sprite GetAvailableNodeSprite()
    {
        return _availableNodeSprite;
    }


}
