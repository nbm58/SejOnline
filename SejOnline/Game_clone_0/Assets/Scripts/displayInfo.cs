using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class displayInfo : MonoBehaviour
{

    public TMP_Text usernameField;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     //if a user has been logged in, it's most recent data will be stored
     //locally in DBManager
     if(DBManager.LoggedIn)
     {
      usernameField.text = "Player: " + DBManager.username;
     }   
    }
    
}
