using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolController : MonoBehaviour
{
    public static PoolController Instance { get; private set; }

    [SerializeField] List<PoolHandler> _poolHandlerPrefabs = new List<PoolHandler>();

    Dictionary<PoolHandler.PoolTypes, PoolHandler> _poolHandlerMenu = new Dictionary<PoolHandler.PoolTypes, PoolHandler>();
    Dictionary<PoolHandler.PoolTypes, List<PoolHandler>> _activePoolObjects = new Dictionary<PoolHandler.PoolTypes, List<PoolHandler>>();
    Dictionary<PoolHandler.PoolTypes, Queue<PoolHandler>> _inactivePoolObjects = new Dictionary<PoolHandler.PoolTypes, Queue<PoolHandler>>();

    private void Awake()
    {
        Instance = this;
        CreateMenu();
    }

    private void CreateMenu()
    {
        foreach (var ph in _poolHandlerPrefabs)
        {
            if (!_poolHandlerMenu.ContainsKey(ph.PoolType))
            {
                _poolHandlerMenu.Add(ph.PoolType, ph);

                List<PoolHandler> activePoolObjects = new List<PoolHandler>();
                _activePoolObjects.Add(ph.PoolType, activePoolObjects);

                Queue<PoolHandler> inactivePoolObjects = new Queue<PoolHandler>();
                _inactivePoolObjects.Add(ph.PoolType, inactivePoolObjects);
            }

        }
    }

    public PoolHandler CheckoutPoolObject(PoolHandler.PoolTypes poolType)
    {
        PoolHandler ph;

        if (_inactivePoolObjects[poolType].Count > 0)
        {
            ph = _inactivePoolObjects[poolType].Dequeue();
        }
        else
        {
            GameObject go = Instantiate(_poolHandlerMenu[poolType].gameObject);
            ph = go.GetComponent<PoolHandler>();
        }

        return ph;
    }

    public void CheckinDeactivatedPoolObject(PoolHandler.PoolTypes poolType, PoolHandler poolObject)
    {
        _inactivePoolObjects[poolType].Enqueue(poolObject);
        //Debug.Log($"checking in a {poolType}. Queue contains {_inactivePoolObjects[poolType].Count}");
    }

}
