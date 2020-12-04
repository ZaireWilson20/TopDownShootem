using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class PlayerNameInput : MonoBehaviour
{

    [SerializeField]
    public static string displayName;

    [SerializeField]
    private TMP_InputField nameInputField; 


    [SerializeField]
    private Button continueButton; 
    // Start is called before the first frame update

    public void SetPlayerName(string name)
    {
        //Disables continue if display name is empty
        continueButton.interactable = !string.IsNullOrEmpty(name);
    }

    public void SavePlayerName()
    {
        displayName = nameInputField.text;  
    }


}
