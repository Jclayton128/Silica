using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorLibrary : MonoBehaviour
{
    public static ColorLibrary Instance { get; private set; }

    [SerializeField] Color[] _playerColors = null;
    public Color[] PlayerColors => _playerColors;


    [SerializeField] Color _usedColor = Color.gray;
    public Color UsedColor => _usedColor;


    [SerializeField] Color _availableColor = Color.green;
    public Color AvailableColor => _availableColor;

    [SerializeField] Color _bugColor = Color.red;
    public Color BugColor => _bugColor;

    private void Awake()
    {
        Instance = this;
    }

    
}
