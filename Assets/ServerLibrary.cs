using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerLibrary : MonoBehaviour
{
    public static ServerLibrary Instance { get; private set; }

    [SerializeField] Sprite _unvisitedIcon = null;
    [SerializeField] Sprite _visitedIcon = null;
    [SerializeField] Sprite _beatenIcon = null;
    [SerializeField] Sprite _currentIcon = null;
    [SerializeField] Sprite _homeServer = null;

    public Sprite VisitedIcon => _visitedIcon;
    public Sprite BeatenIcon => _beatenIcon;
    public Sprite CurrentIcon => _currentIcon;
    public Sprite UnvisitedIcon => _unvisitedIcon;
    public Sprite HomeServerIcon => _homeServer;


    private void Awake()
    {
        Instance = this;
    }


}
