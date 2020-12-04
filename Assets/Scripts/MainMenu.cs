using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private ShootNetworkManager networkManager = null;
    [SerializeField] private GameObject landingPage = null; 

    public void HostLobby()
    {
        networkManager.StartHost();
        landingPage.SetActive(false);

    }
}
