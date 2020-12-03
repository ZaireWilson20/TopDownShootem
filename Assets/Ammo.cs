using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ammo : MonoBehaviour
{
    public Player player;
    public Text ammoText;

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Ammo:",player.ammo.ToString());
        ammoText.text = player.ammo.ToString();
    }
}
