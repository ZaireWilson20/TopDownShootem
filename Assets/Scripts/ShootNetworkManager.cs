using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using Mirror;
using System;
using System.Linq;

public class ShootNetworkManager : NetworkManager
{
    NetworkConnection con;
    [SerializeField] private RoomPlayer roomPlayerPref = null;
    [SerializeField] private GamePlayer gamePlayerPref = null;
    [SerializeField] private GameObject spawnPref = null; 


    [Scene][SerializeField] private string menuScene = string.Empty;
    [Scene] [SerializeField] private string gameScene = string.Empty;
    [SerializeField] private int minPlayers = 2;

    public List<RoomPlayer> roomPlayers = new List<RoomPlayer>();
    public List<GamePlayer> gamePlayers = new List<GamePlayer>();
    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;
    public static event Action<NetworkConnection> OnServerReadied;
    public static event Action OnServerStopped;


    public override void OnStartServer() => spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();

    public override void OnStartClient()
    {
        var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");

        foreach (var prefab in spawnablePrefabs)
        {
            //ClientScene.RegisterPrefab(prefab);
        }
    }
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        OnClientConnected?.Invoke();
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);

        OnClientDisconnected?.Invoke();
    }
    
    public override void OnServerConnect(NetworkConnection conn)
    {
        if(numPlayers >= maxConnections)
        {
            conn.Disconnect();
            return; 
        }

        if(SceneManager.GetActiveScene().path != menuScene)
        {
            conn.Disconnect();
            return; 
        }
    }
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        if(conn.identity != null)
        {
            var player = conn.identity.GetComponent<RoomPlayer>();
            roomPlayers.Remove(player);
            NotifyReadyState();
        }
        base.OnServerDisconnect(conn);
    }
    public override void OnServerAddPlayer(NetworkConnection conn)
    {


        if(SceneManager.GetActiveScene().path == menuScene)
        {
            Debug.Log("heyoo");
            bool isLeader = roomPlayers.Count == 0; 
            RoomPlayer roomPlayerInstance = Instantiate(roomPlayerPref);

            roomPlayerInstance.IsLeader = isLeader; 
            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
        }
    }

    public void NotifyReadyState()
    {
        foreach(var player in roomPlayers)
        {
            player.HandleReadyStart(IsReadyStart());
        }
    }

    private bool IsReadyStart()
    {
        if(numPlayers < minPlayers) { return false; }

        foreach(var player in roomPlayers)
        {
            if (!player.IsReady) { return false; }
        }

        return true; 
    }


    public override void OnStopServer()
    {
        roomPlayers.Clear(); 
    }

    public void StartGame()
    {
        if(SceneManager.GetActiveScene().path == menuScene)
        {
            if (!IsReadyStart()) { return; }
            ServerChangeScene(gameScene);
        }
    }

    public override void ServerChangeScene(string newSceneName)
    {
        // From menu to game
        if (SceneManager.GetActiveScene().path == menuScene)
        {
            for (int i = roomPlayers.Count - 1; i >= 0; i--)
            {
                var conn = roomPlayers[i].connectionToClient;
                var gameplayerInstance = Instantiate(gamePlayerPref);
                gameplayerInstance.SetDisplayName(roomPlayers[i].displayName);

                NetworkServer.Destroy(conn.identity.gameObject);

                NetworkServer.ReplacePlayerForConnection(conn, gameplayerInstance.gameObject);
            }
        }

        base.ServerChangeScene(newSceneName);
    }

    public override void OnServerSceneChanged(string sceneName)
    {

            GameObject playerSpawnSystemInstance = Instantiate(spawnPref);
            NetworkServer.Spawn(playerSpawnSystemInstance);

          //  GameObject roundSystemInstance = Instantiate(roundSystem);
          //  NetworkServer.Spawn(roundSystemInstance);

    }

    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);

        OnServerReadied?.Invoke(conn);
    }




}
