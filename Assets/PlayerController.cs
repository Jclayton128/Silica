using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    //state
    [SerializeField] List<PlayerHandler> _playerHandlers = new List<PlayerHandler>();
    [SerializeField] AvatarUIDriver _avatarUIPrefab = null;
    private void Awake()
    {
        Instance = this;
    }

    public int RegisterNewPlayer(PlayerHandler newPlayerHandler)
    {
        _playerHandlers.Add(newPlayerHandler);
        AvatarUIDriver newAvatar = Instantiate(_avatarUIPrefab);
        newAvatar.AttachUIToAvatar(newPlayerHandler);
        return _playerHandlers.Count;
    }

    public PlayerHandler GetPlayer(int ownerIndex)
    {
       return _playerHandlers[ownerIndex-1];
    }

}
