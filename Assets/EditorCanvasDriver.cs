using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorCanvasDriver : MonoBehaviour
{
    public void HidePanel()
    {
        gameObject.SetActive(false);
    }

    public void ShowPanel()
    {
        gameObject.SetActive(true);
    }
}
