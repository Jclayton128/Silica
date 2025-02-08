using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AlertRingHandler : MonoBehaviour
{
    [SerializeField] SpriteRenderer _alertRingSR = null;

    //refs
    Collider2D _coll;

    //settings
    [SerializeField] float _maxAlertRingScale = 40f;
    [SerializeField] float _timeForMaxAlertScale = 10f;

    //state
    Color _col;
    Tween _ringGrowthTween;
    Tween _ringColorTween;

    private void Awake()
    {
        _alertRingSR = GetComponent<SpriteRenderer>();
        _coll = GetComponent<Collider2D>();
    }

    public void AlertOthers()
    {

        _ringColorTween.Kill();
        _ringGrowthTween.Kill();
        _coll.enabled = true;
        _alertRingSR.transform.localScale = Vector3.zero;
        _ringGrowthTween = _alertRingSR.transform.DOScale(_maxAlertRingScale, _timeForMaxAlertScale).OnComplete(HandleRingTweenCompleted);

        _col = _alertRingSR.color;
        _col.a = 1;        
        _alertRingSR.color = _col;
        //_ringColorTween = _alertRingSR.DOFade(0, _timeForMaxAlertScale);
    }

    private void HandleRingTweenCompleted()
    {
        //_alertRingSR.transform.localScale = Vector3.zero;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //check if new thing has an IAlertable
        //if yes, set max alert
        IAlertable alertableThing;
        if (collision.TryGetComponent<IAlertable>(out alertableThing))
        {
            alertableThing.SetMaxAlert();
        }
    }

    public void StopAlerting()
    {
        _ringGrowthTween.Kill();
        _ringGrowthTween = _alertRingSR.transform.DOScale(Vector3.zero, _timeForMaxAlertScale/10f).OnComplete(HandleRingTweenCompleted); 
        //_ringColorTween = _alertRingSR.DOFade(0, _timeForMaxAlertScale/10f);
        _coll.enabled = false;
    }

    private void OnDestroy()
    {
        _ringGrowthTween.Kill();
    }
}
