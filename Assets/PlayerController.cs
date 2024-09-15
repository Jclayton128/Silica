using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    [SerializeField] PlayerHandler _playerPrefab;
    [SerializeField] AvatarUIDriver _avatarUIPrefab = null;

    //state
    [SerializeField] List<PlayerHandler> _playerHandlers = new List<PlayerHandler>();
    public PlayerHandler CurrentPlayer { get; private set; }

    public ArenaHandler CurrentArena => CurrentPlayer.CurrentServer.Arena;
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

    public void DeregisterPlayer(PlayerHandler unneededPlayerHandler)
    {
        _playerHandlers.Remove(unneededPlayerHandler);
    }

    public PlayerHandler GetPlayer(int ownerIndex)
    {
       return _playerHandlers[ownerIndex-1];
    }

    public void InitializeNewPlayer()
    {
        if (CurrentPlayer == null)
        {
            CurrentPlayer = Instantiate(_playerPrefab);
        }
        else
        {
            Debug.LogWarning("Player already existed; deleted old player.");
            //PlayerController.Instance.DeregisterPlayer(Player);
            CurrentPlayer.ForcePlayerDeath();

            CurrentPlayer = Instantiate(_playerPrefab);
        }
    }

}
