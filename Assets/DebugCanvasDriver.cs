using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DebugCanvasDriver : MonoBehaviour
{
    //scene references
    [SerializeField] TextMeshProUGUI _limiterSpeedTMP = null;
    [SerializeField] TextMeshProUGUI _levelCountTMP = null;


    // Start is called before the first frame update
    void Start()
    {
        LimiterController.Instance.LimiterSpeedChanged += HandleLimiterSpeedChanged;
        LevelController.Instance.LevelCountIncrease += HandleLevelCountChanged;
        HandleLimiterSpeedChanged();
        HandleLevelCountChanged();
    }

    private void HandleLimiterSpeedChanged()
    {
        _limiterSpeedTMP.text = $"Speed: {LimiterController.Instance.LimiterSpeedCurrent.ToString()}";
    }

    private void HandleLevelCountChanged()
    {
        _levelCountTMP.text = $"Level: {LevelController.Instance.LevelCount.ToString()}";
    }
}
