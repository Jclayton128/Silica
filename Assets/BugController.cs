using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugController : MonoBehaviour
{
    public static BugController Instance { get; private set; }

    [SerializeField] List<BugHandler> _bugHandlerPrefabs = new List<BugHandler>();

    Dictionary<BugHandler.BugTypes, BugHandler> _bugHandlerMenu = new Dictionary<BugHandler.BugTypes, BugHandler>();
    Dictionary<BugHandler.BugTypes, List<BugHandler>> _activeBugs = new Dictionary<BugHandler.BugTypes, List<BugHandler>>();
    Dictionary<BugHandler.BugTypes, Queue<BugHandler>> _inactiveBugs = new Dictionary<BugHandler.BugTypes, Queue<BugHandler>>();

    private void Awake()
    {
        Instance = this;
        CreateMenu();
    }

    private void CreateMenu()
    {
        foreach (var ph in _bugHandlerPrefabs)
        {
            if (!_bugHandlerMenu.ContainsKey(ph.BugType))
            {
                _bugHandlerMenu.Add(ph.BugType, ph);

                List<BugHandler> activePoolObjects = new List<BugHandler>();
                _activeBugs.Add(ph.BugType, activePoolObjects);

                Queue<BugHandler> inactivePoolObjects = new Queue<BugHandler>();
                _inactiveBugs.Add(ph.BugType, inactivePoolObjects);
            }

        }
    }

    private BugHandler CheckoutBug(BugHandler.BugTypes bugType)
    {
        BugHandler bh;

        if (_inactiveBugs[bugType].Count > 0)
        {
            bh = _inactiveBugs[bugType].Dequeue();
            bh.gameObject.SetActive(true);
        }
        else
        {
            GameObject go = Instantiate(_bugHandlerMenu[bugType].gameObject);
            bh = go.GetComponent<BugHandler>();
        }

        return bh;
    }

    public void CheckinDeactivatedBug(BugHandler.BugTypes poolType, BugHandler poolObject)
    {
        _inactiveBugs[poolType].Enqueue(poolObject);
        poolObject.gameObject.SetActive(false);
    }

    public void SpawnBug(BugHandler.BugTypes bugType)
    {
        BugHandler bug = CheckoutBug(bugType);

        Vector2 pos = Vector2.zero;
        pos.x = UnityEngine.Random.Range(-NodeController.Instance.XSpan, NodeController.Instance.XSpan);
        pos.y = NodeController.Instance.YOffScreen_Up;
        bug.transform.position = pos;

        bug.ActivateBug();
    }
}
