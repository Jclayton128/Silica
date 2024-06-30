using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeLibrary : MonoBehaviour
{
    public static NodeLibrary Instance { get; private set; }

    [SerializeField] NodeHandler _homeNodePrefab = null;
    [SerializeField] NodeHandler _commercialNodePrefab = null;

    [SerializeField] Sprite _activeNodeSprite = null;
    [SerializeField] Sprite _availableNodeSprite = null;



    private void Awake()
    {
        Instance = this;
    }

    public NodeHandler GetHomeNodePrefab()
    {
        return _homeNodePrefab;
    }

    public NodeHandler GetCommercialNodePrefab()
    {
        return _commercialNodePrefab;
    }

    public Sprite GetActiveNodeSprite()
    {
        return _activeNodeSprite;
    }

    public Sprite GetAvailableNodeSprite()
    {
        return _availableNodeSprite;
    }


}
