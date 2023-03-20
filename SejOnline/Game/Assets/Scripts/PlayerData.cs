using Unity.Services.Authentication;
using System.Threading.Tasks;
using System.Collections.Generic;
using Unity.Services.Core;
using Unity.Services.CloudSave;
using UnityEngine;
using TMPro;

public class PlayerData : MonoBehaviour
{
    public TMP_Text usernameDisplay;
    public TMP_InputField changeUsernameField;

    public TMP_Text gamesPlayedDisplay;
    public TMP_Text gamesWonDisplay;
    public TMP_Text gamesLostDisplay;
    public TMP_Text winRateDisplay;
    
    public async void changeUsername()
    {
        var data = new Dictionary<string, object>{{"username", changeUsernameField.text}};
        await CloudSaveService.Instance.Data.ForceSaveAsync(data);

        Debug.Log("Username changed to: " + changeUsernameField.text);

        fetchUsername();
    }

    public void fetchUserData()
    {
        fetchUsername();
        fetchGamesPlayed();
        fetchGamesWon();
        fetchGamesLost();
        calculateWinRate();
    }
    
    public async void fetchUsername()
    {
        Dictionary<string, string> playerUsernameDict = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string>{"username"});

        if (playerUsernameDict.ContainsKey("username"))
        {
            var playerUsername = playerUsernameDict["username"];

            usernameDisplay.text = "Player: " + playerUsername;

            return;
        }

        Debug.Log("No username found");
        
        usernameDisplay.text = "Player: " + AuthenticationService.Instance.PlayerId;
    }

    public async void fetchGamesPlayed()
    {
        Dictionary<string, string> gamesPlayedDict = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string>{"gamesPlayed"});

        if (gamesPlayedDict.ContainsKey("gamesPlayed"))
        {
            var gamesPlayed = gamesPlayedDict["gamesPlayed"];

            gamesPlayedDisplay.text = "Games:\n" + gamesPlayed;

            return;
        }

        Debug.Log("No games played found");
    }

    public async void fetchGamesWon()
    {
        Dictionary<string, string> gamesWonDict = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string>{"gamesWon"});

        if (gamesWonDict.ContainsKey("gamesWon"))
        {
            var gamesWon = gamesWonDict["gamesWon"];

            gamesWonDisplay.text = "Wins:\n" + gamesWon;

            return;
        }

        Debug.Log("No games won found");
    }

    public async void fetchGamesLost()
    {
        Dictionary<string, string> gamesLostDict = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string>{"gamesLost"});

        if (gamesLostDict.ContainsKey("gamesLost"))
        {
            var gamesLost = gamesLostDict["gamesLost"];

            gamesLostDisplay.text = "Losses:\n" + gamesLost;

            return;
        }

        Debug.Log("No games lost found");
    }

    public async void calculateWinRate()
    {
        Dictionary<string, string> gamesPlayedDict = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string>{"gamesPlayed"});
        Dictionary<string, string> gamesWonDict = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string>{"gamesWon"});

        if (gamesPlayedDict.ContainsKey("gamesPlayed") && gamesWonDict.ContainsKey("gamesWon"))
        {
            var gamesPlayed = gamesPlayedDict["gamesPlayed"];
            var gamesWon = gamesWonDict["gamesWon"];

            var winRate = (float.Parse(gamesWon) / float.Parse(gamesPlayed)) * 100;

            if (float.IsNaN(winRate))
            {
                winRateDisplay.text = "Win Rate:\n" + "N/A";
                return;
            }
            
            winRateDisplay.text = "Win Rate:\n" + winRate + "%";

            return;
        }

        Debug.Log("No games played or games won found");
    }
}
