using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class loginUser : MonoBehaviour
{
  public TMP_InputField nameInputField;
  public TMP_InputField passwordInputField;
  public Button submitButton;

    //Occurs on Login button press
    public void CallLogin()
    {
     StartCoroutine(Login());
    }


    //Wats for yield result
    IEnumerator Login()
    {
     //create form data to be sent to php script, this will have any info
     //that needs to be uploaded
     WWWForm form = new WWWForm();
     form.AddField("username", nameInputField.text);
     form.AddField("password",passwordInputField.text);
     
     string uri = "nickolasmaxwell.com/login.php";
     //create connection @ uri, with form data
     using(UnityWebRequest request = UnityWebRequest.Post(uri,form))
     {
      yield return request.SendWebRequest();

      //when request results come back...
      if(request.result == UnityWebRequest.Result.ConnectionError
                   || request.result == UnityWebRequest.Result.ProtocolError)
      {
       Debug.Log("User Login Failed, Error #");
       Debug.Log(request.error);
      }
      else
      {
        //if the returned code = 0, user login info is correct/valid
        //set current instance of "user" with relative information
        if(request.downloadHandler.text.StartsWith("0"))
        {
         DBManager.username = nameInputField.text;
         //parses the returned string for the user score
         DBManager.score = int.Parse(request.downloadHandler.text.Split('\t')[1]);
        }
        else
        {
         Debug.Log(request.downloadHandler.text);
         
        }
      }
     }

    }

    

    //Atleast 8 characters
    public void VerifyInputs()
    {
     submitButton.interactable = (nameInputField.text.Length >= 8 
                                         && passwordInputField.text.Length >= 8);
    }
}
