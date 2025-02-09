using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsPanelDriver : MonoBehaviour
{
    [SerializeField] StatsDriver _speedStat = null;
    [SerializeField] StatsDriver _mightStat = null;
    [SerializeField] StatsDriver _intelStat = null;
    [SerializeField] StatsDriver _contitutionStat = null;
    [SerializeField] StatsDriver _wisdomStat = null;


    //state
    PlayerDataHolder _pdh;

    void Start()
    {
        GameController.Instance.RunStarted += HandleRunStarted;
    }

    private void HandleRunStarted()
    {
        _pdh = PlayerController.Instance.CurrentPlayer.GetComponent<PlayerDataHolder>();
        _pdh.StatsChanged += HandleStatChanged;
    }

    private void HandleStatChanged()
    {
        _speedStat.SetPips(_pdh.Stat_Speed);
        _mightStat.SetPips(_pdh.Stat_Might);
        _contitutionStat.SetPips(_pdh.Stat_Constitution);
        _intelStat.SetPips(_pdh.Stat_Intelligence);
        _wisdomStat.SetPips(_pdh.Stat_Wisdom);

    }
}
