using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;

public class RoomPlayer : NetworkBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject lobbyUI = null;
    [SerializeField] private TMP_Text[] playerNameTexts = new TMP_Text[4];
    [SerializeField] private TMP_Text[] playerReadyTexts = new TMP_Text[4];
    [SerializeField] private Button startGameButton = null;
    [SerializeField] private ShootNetworkManager netMan; 




    [SyncVar(hook = nameof(HandleDisplayNameChange))]
    public string displayName = "Loading...";

    [SyncVar(hook = nameof(HandleReadyStatusChange))]
    public bool IsReady = false;

    private bool isLeader;

    private ShootNetworkManager room; 

    private ShootNetworkManager Room
    {
        get
        {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as ShootNetworkManager;
        }
    }

    public bool IsLeader
    {
        set
        {
            isLeader = value;
            startGameButton.gameObject.SetActive(value);
        }
    }


    public override void OnStartAuthority()
    {
        Debug.Log(PlayerNameInput.displayName);
        
        CmdSetDisplayName(PlayerNameInput.displayName);
        lobbyUI.SetActive(true);



    }

    public override void OnStartClient()
    {
        Debug.Log("ayo");
        Room.roomPlayers.Add(this);
        UpdateDisplay(); 
    }

    [Command]
    private void CmdSetDisplayName(string displayNm)
    {
        
        Debug.Log(displayName);

        displayName = displayNm;
    }

    [Command]
    public void CmdReadyUp()
    {
        IsReady = !IsReady;
        Room.NotifyReadyState(); 
    }

    [Command]
    public void CmdStarGame()
    {
        if (Room.roomPlayers[0].connectionToClient != connectionToClient) { return;  }

        Room.StartGame();
    }
    public void HandleReadyStatusChange(bool oldValue, bool newValue) => UpdateDisplay();

    public void HandleDisplayNameChange(string oldValue, string newValue) => UpdateDisplay();


    private void UpdateDisplay()
    {
        Debug.Log("updating display for " + displayName);
        if (!hasAuthority)
        {
            foreach (var player in Room.roomPlayers)
            {
                if (player.hasAuthority)
                {
                    player.UpdateDisplay();
                    break;
                }
            }
            return;
        }


        for (int i = 0; i < Room.roomPlayers.Count; i++)
        {

            playerNameTexts[i].text = Room.roomPlayers[i].displayName;
            playerReadyTexts[i].text = Room.roomPlayers[i].IsReady ? "<color=green>Ready</color>" : "<color=red>Not Ready</color>";
            Debug.Log(playerNameTexts[i].text + " " + i);

        }
    }

    public void HandleReadyStart(bool readyToStart)
    {
        if (!isLeader) { return;  }
        startGameButton.interactable = readyToStart;
    }
}
