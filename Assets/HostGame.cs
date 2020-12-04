using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class HostGame : MonoBehaviour
{
    int roomSize = 6;
    string roomName = "";
    public ShootNetworkManager manager;

    public void SetRoomName(string s)
    {
        roomName = s; 
    }

    public void CreateRoom()
    {
        if(roomName != "" && roomName != null)
        {
            Debug.Log("Hosting game");
            manager.StartHost();
            
        }

            
    }
}
