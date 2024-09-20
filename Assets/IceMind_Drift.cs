using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceMind_Drift : ICEHandler, IDestroyable
{

    //settings
    [SerializeField] MineBrain _driftMinePrefab = null;
    [SerializeField] float _timeBetweenSalvos = 5;
    [SerializeField] float _timeBetweenIndividualMines = 1f;
    [SerializeField] float _spinRate = 30f;
    [SerializeField] int _salvoSize = 3;
    [SerializeField] float _velocityAtRelease = 2f;

    //state
    [SerializeField] float _timeUntilNextSalvo;
    [SerializeField] float _timeUntilNextMine;
    [SerializeField] int _minesRemainingInSalvo = 3;
    List<MineBrain> _activeMines = new List<MineBrain>();

    protected override void Start()
    {
        base.Start();
        _timeUntilNextMine = 0;
        _timeUntilNextSalvo = _timeBetweenSalvos / 2f;

        //choose random rotation
        transform.up = UnityEngine.Random.insideUnitCircle.normalized;
        _arena.ArenaShuttingDown += HandleArenaShuttingDown;
    }


    private void Update()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * _spinRate);

        if (_minesRemainingInSalvo > 0 && _timeUntilNextSalvo <= 0)
        {
            _timeUntilNextMine -= Time.deltaTime;
            if (_timeUntilNextMine < 0)
            {
                ReleaseMine();
                _minesRemainingInSalvo--;
                _timeUntilNextMine = _timeBetweenIndividualMines;
            }
        }

        if (_minesRemainingInSalvo <= 0)
        {
            _timeUntilNextSalvo = _timeBetweenSalvos;
            _minesRemainingInSalvo = _salvoSize;
        }

        _timeUntilNextSalvo -= Time.deltaTime;

    }

    private void ReleaseMine()
    {
        MineBrain newMine = Instantiate(_driftMinePrefab, transform.position, transform.rotation);
        _activeMines.Add(newMine);
        newMine.Initialize(transform.up * 1f, this);

    }


    private void DestroyAllMines(bool shouldDestroyInstantly)
    {
        for (int i = _activeMines.Count-1; i >= 0; i--)
        {
            _activeMines[i].DestroyMine(shouldDestroyInstantly);
        }
    }

    public void HandleZeroHealth()
    {
        DestroyAllMines(false);
        Destroy(gameObject);
    }

    public void DeregisterMineUponItsHealthZero(MineBrain mine)
    {
        _activeMines.Remove(mine);
    }

    public void HandleHealthDrop(float factorRemaining)
    {
        //TODO
    }

    private void HandleArenaShuttingDown()
    {
        DestroyAllMines(true);
    }

}
