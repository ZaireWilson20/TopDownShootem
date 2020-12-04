using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;

public class LobbyJoin : MonoBehaviour
{

    [SerializeField] private ShootNetworkManager networkManager = null;

    [SerializeField] private Button joinButton = null;
    [SerializeField] private TMP_InputField ipAddressInput = null;

    public GameObject landingPage; 
    public GameObject joinPanel; 


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        ShootNetworkManager.OnClientConnected += HandleClientConnected;
        ShootNetworkManager.OnClientDisconnected += HandleClientDisconnected;
    }

    private void OnDisable()
    {
        ShootNetworkManager.OnClientConnected -= HandleClientConnected;
        ShootNetworkManager.OnClientDisconnected -= HandleClientDisconnected;
    }
    public void JoinLobby()
    {
        networkManager.networkAddress = ipAddressInput.text;
        networkManager.StartClient();

        joinButton.interactable = false;

        joinPanel.SetActive(false);
        
    }

    private void HandleClientConnected()
    {
        joinButton.interactable = true;

        //gameObject.SetActive(false);
        landingPage.SetActive(false);
    }

    private void HandleClientDisconnected()
    {
        joinButton.interactable = true;
    }
}
