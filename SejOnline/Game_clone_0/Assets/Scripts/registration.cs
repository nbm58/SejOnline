using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class registration : MonoBehaviour
{
 public TMP_InputField nameInputField;
 public TMP_InputField passwordInputField;
 public Button submitButton;
 
 public void CallRegister()
 {
  StartCoroutine(Register());
 }

 IEnumerator Register()
 {
  //create form with data for upload
  WWWForm form = new WWWForm();
  form.AddField("username", nameInputField.text);
  form.AddField("password",passwordInputField.text);
     
  //server location
  string uri = "nickolasmaxwell.com/register.php";

  //"Create a socket on easymode"
  // connect to the server, and pass it the form, return failure
  using(UnityWebRequest request = UnityWebRequest.Post(uri,form))
  {
  yield return request.SendWebRequest();
  if(request.result == UnityWebRequest.Result.ConnectionError
                   || request.result == UnityWebRequest.Result.ProtocolError)
  {
    //bad news
    Debug.Log(request.error);
    Debug.Log("User creation failed. Error #" + request.error);
  }
  else
  {
    //Server returned success code
    Debug.Log(request.downloadHandler.text);
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
