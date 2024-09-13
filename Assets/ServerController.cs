using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerController : MonoBehaviour
{
    public static ServerController Instance { get; private set; }

    //settings
    [SerializeField] ServerHandler _serverPrefab = null;


    //state
    ServerHandler _startingServer;
    public ServerHandler StartingServer => _startingServer;
    List<ServerHandler> _allServers = new List<ServerHandler>();


    private void Awake()
    {
        Instance = this;
    }

    public void RegisterServer(ServerHandler server)
    {
        _allServers.Add(server);
    }

    public void SetupNewGame()
    {
        //create starting server
        ServerHandler sh = Instantiate(_serverPrefab, Vector3.zero, Quaternion.identity);
        RegisterServer(sh);
        _startingServer = sh;
        sh.Initialize();
        sh.SetupServer(ServerHandler.ServerStates.Current, ServerHandler.ServerTypes.Type1, true);

        //populate with other servers
        for (int i = 1; i < 4; i++)
        {
            ServerHandler sh1 = Instantiate(_serverPrefab, new Vector3(2* i, 2 * i, 0), Quaternion.identity);
            RegisterServer(sh1);
            sh1.Initialize();
            sh1.SetupServer(ServerHandler.ServerStates.Unvisited, ServerHandler.ServerTypes.Type1, false);
        }
    }




}
