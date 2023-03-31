using Unity.Services.Authentication;
using System.Threading.Tasks;
using System.Collections.Generic;
using Unity.Services.Core;
using Unity.Services.CloudSave;
using UnityEngine;
using TMPro;

public class CloudSave : MonoBehaviour
{
    private List<string> keys;
    internal async Task Awake()
    {
        await UnityServices.InitializeAsync();
        await SignInAnonymously();
        keys = await CloudSaveService.Instance.Data.RetrieveAllKeysAsync();

        if (keys.Count == 0)
        {
            Debug.Log("No keys found, new player detected");
            
            initUsername();
            initGamesPlayed();
            initGamesWon();
            initGamesLost();

            keys = await CloudSaveService.Instance.Data.RetrieveAllKeysAsync();
        }
    }
    
    private async Task SignInAnonymously()
    {
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in as: " + AuthenticationService.Instance.PlayerId);
        };
        AuthenticationService.Instance.SignInFailed += s =>
        {
            // Take some action here...
            Debug.Log(s);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void RetrieveKeys()
    {
        List<string> keys = await CloudSaveService.Instance.Data.RetrieveAllKeysAsync();

        for (int i = 0; i < keys.Count; i++)
        {
            Debug.Log(keys[i]);
        }
    }

    public async void initUsername()
    {
        var data = new Dictionary<string, object>{{"username", AuthenticationService.Instance.PlayerId}};
        await CloudSaveService.Instance.Data.ForceSaveAsync(data);
    }

    public async void initGamesPlayed()
    {
        var data = new Dictionary<string, object>{{"gamesPlayed", 0}};
        await CloudSaveService.Instance.Data.ForceSaveAsync(data);
    }

    public async void initGamesWon()
    {
        var data = new Dictionary<string, object>{{"gamesWon", 0}};
        await CloudSaveService.Instance.Data.ForceSaveAsync(data);
    }

    public async void initGamesLost()
    {
        var data = new Dictionary<string, object>{{"gamesLost", 0}};
        await CloudSaveService.Instance.Data.ForceSaveAsync(data);
    }
}