using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerController : MonoBehaviour
{
    public static ServerController Instance { get; private set; }

    //settings
    [SerializeField] ServerHandler _serverPrefab = null;
    [SerializeField] ServerHandler _start = null;

    //state
    ServerHandler _startingServer;
    public ServerHandler StartingServer => _startingServer;
    List<ServerHandler> _allServers = new List<ServerHandler>();


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CameraController.Instance.ZoomedMicro += HandleZoomMicroCompleted;
    }

    public void RegisterServer(ServerHandler server)
    {
        _allServers.Add(server);
    }

    public void SetupNewGame()
    {
        //create starting server

        RegisterServer(_start);
        _startingServer = _start;
        _start.Initialize();
        _start.SetupServer(ServerHandler.ServerStates.Current, ServerHandler.ServerTypes.Type1, true);
        //ServerHandler sh = Instantiate(_serverPrefab, Vector3.zero, Quaternion.identity);
        //RegisterServer(sh);
        //_startingServer = sh;
        //sh.Initialize();
        //sh.SetupServer(ServerHandler.ServerStates.Current, ServerHandler.ServerTypes.Type1, true);

        //populate with other servers
        //for (int i = 1; i < 4; i++)
        //{
        //    ServerHandler sh1 = Instantiate(_serverPrefab, new Vector3(2* i, 2 * i, 0), Quaternion.identity);
        //    RegisterServer(sh1);
        //    sh1.Initialize();
        //    sh1.SetupServer(ServerHandler.ServerStates.Unvisited, ServerHandler.ServerTypes.Type1, false);
        //}
    }

    public void EnterServerToArena()
    {
        HideAllServersWhenEnteringAnArena();

        CameraController.Instance.ZoomMicro();

    }

    private void HandleZoomMicroCompleted()
    {
        PlayerController.Instance.CurrentPlayer.CurrentServer.EnterArena();


        //ArenaController.Instance.CreateNewCurrentArena();
        //Find the starting node for the arena and make it the player's current node
        //NodeController.Instance.SpawnStartingNode(1);
    }

    public void ExitServerFromArena()
    {
        Debug.Log("Exiting Arena");

        //Close the arena borders
        //Disable all nodes
        //reveal all servers

        CameraController.Instance.ZoomIn();
        NodeController.Instance.DespawnAllNodes();
        PlayerController.Instance.CurrentPlayer.CurrentServer.ExitArena();
        //ArenaController.Instance.CloseDownArena();
        ShowAllServersWhenExitingAnArena();

    }

    private void HideAllServersWhenEnteringAnArena()
    {
        foreach(ServerHandler server in _allServers)
        {
            server.HideServer();
        }
    }

    private void ShowAllServersWhenExitingAnArena()
    {
        foreach (ServerHandler server in _allServers)
        {
            server.ShowServer();
        }
    }




}
