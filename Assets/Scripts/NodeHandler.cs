using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeHandler : MonoBehaviour
{
    [SerializeField] bool _isHomeNode = false;
    public bool IsHomeNode => _isHomeNode;

    //references
    SpriteRenderer _sr;

    //state
    private bool _isInitialized = false;
    Vector3 _angs = Vector3.zero;

    public void SetupNodeInstance(bool isActiveNode)
    {
        if (!_isInitialized)
        {
            Initialize(isActiveNode);
        }

        //Setup other nuances for this node later in this method
    }

    private void Initialize(bool isActiveNode)
    {
        _sr = GetComponent<SpriteRenderer>();
        if (isActiveNode)
        {
            _sr.sprite = NodeLibrary.Instance.GetActiveNodeSprite();
        }
        else
        {
            _sr.sprite = NodeLibrary.Instance.GetAvailableNodeSprite();
        }

        _isInitialized = true;
    }

    public void TeardownAsActiveNode()
    {
        _sr.sprite = NodeLibrary.Instance.GetAvailableNodeSprite();
        transform.right = Vector2.up;
    }

    public void AdjustRotation(Vector2 facingDir)
    {
        transform.up = facingDir;
    }
   
}
