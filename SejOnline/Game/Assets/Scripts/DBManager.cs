using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//Database Manager will hold values that are pulled/saved to the Database.
//Basically acts as the data structure for the currently running instance of 
//the client
public static class DBManager
{
 public static string username;
 public static int score;

 public static bool LoggedIn { get { return username != null; } }

 public static void LogOut()
 {
  username = null;
 }   
}
