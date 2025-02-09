using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StatsDriver : MonoBehaviour
{
    [SerializeField] Image[] _pips = null;

    //settings
    [SerializeField] Color _color_On = Color.green;
    [SerializeField] Color _color_Off = Color.white;

    private void Start()
    {
        SetPips(0);
    }

    public void SetPips(int numberToSet)
    {
        foreach (var pip in _pips)
        {
            pip.color = _color_Off;
        }
        for (int i = 0; i < numberToSet; i++)
        {
            if (i >= _pips.Length)
            {
                Debug.LogWarning("Can't set anymore");
                return;
            }
            _pips[i].color = _color_On; 
        }
    }
}
