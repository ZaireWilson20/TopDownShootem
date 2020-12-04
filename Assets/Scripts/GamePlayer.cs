using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GamePlayer : NetworkBehaviour
{
    [SyncVar]
    private string displayName = "Loading...";

    
    public int kills;

    public int playerNum = 0; 

    private ShootNetworkManager room;
    private ShootNetworkManager Room
    {
        get
        {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as ShootNetworkManager;
        }
    }

    public override void OnStartClient()
    {
        DontDestroyOnLoad(gameObject);

        Room.gamePlayers.Add(this);
        GetPlayerNum(); 
    }

    public override void OnStopClient()
    {
        Room.gamePlayers.Remove(this);
    }

    [Server]
    public void SetDisplayName(string displayName)
    {
        this.displayName = displayName;
    }

    public string GetDisplayName()
    {
        return displayName; 
    }


    private void GetPlayerNum()
    {
        int count = 0; 
        Debug.Log("updating display for " + displayName);
        if (!hasAuthority)
        {
            foreach (var player in Room.gamePlayers)
            {
                if (player.hasAuthority)
                {
                    playerNum = count; 
                    break;
                }
                count++; 
            }
            return;
        }

    }
}

