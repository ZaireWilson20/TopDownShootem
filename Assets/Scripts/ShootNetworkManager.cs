using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ShootNetworkManager : NetworkManager
{
    NetworkConnection con; 
    public override void OnClientConnect(NetworkConnection conn)
    {
        con = conn;
        foreach(NetworkIdentity c in conn.clientOwnedObjects)
        {
            Debug.Log(c.gameObject.tag + " " + c.netId);
        }
        Debug.Log("Poopu");
    }

    public override void OnStartClient()
    {
        foreach (NetworkIdentity c in con.clientOwnedObjects)
        {
            Debug.Log(c.gameObject.tag + " " + c.netId);
        }
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
    }
}
