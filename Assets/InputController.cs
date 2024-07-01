using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public static InputController Instance { get; private set; }

    public Vector3 MousePosition;// { get; private set; } 
    public Action<bool> MouseChanged_LMB;
    public Action<bool> MouseChanged_RMB;

    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (NodeController.Instance.CurrentNode == null) return;

        UpdateMousePosition();

        UpdateMouseClicks();
        
    }

    private void UpdateMouseClicks()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            MouseChanged_LMB?.Invoke(true);
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            MouseChanged_RMB?.Invoke(true);
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            MouseChanged_LMB?.Invoke(false);
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            MouseChanged_RMB?.Invoke(false);
        }
    }

    private void UpdateMousePosition()
    {
        MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }



}
