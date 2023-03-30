using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine.SceneManagement;
using Unity.Collections;
using TMPro;

public class NetworkManagerUI : NetworkBehaviour
{
    public Button throwWandsButton;
    public Button throwDiceButton;
    public Button passButton;
    public Button declineButton;

    public GameObject wand1;
    public GameObject wand2;
    public GameObject wand3;

    public GameObject dice1;
    public GameObject dice2;

    GameObject wandSpawn1;
    GameObject wandSpawn2;
    GameObject wandSpawn3;

    GameObject diceSpawn1;
    GameObject diceSpawn2;

    [SerializeField] private TMP_Text HostScoreDisplay;
    [SerializeField] private TMP_Text ClientScoreDisplay;

    [SerializeField] private TMP_Text Dice1Display;
    [SerializeField] private TMP_Text Dice2Display;

    [SerializeField] private TMP_Text Wand1Display;
    [SerializeField] private TMP_Text Wand2Display;
    [SerializeField] private TMP_Text Wand3Display;

    [SerializeField] private TMP_Text GameLogDisplay;

    private NetworkVariable<int> Dice1Value = new NetworkVariable<int>(0);
    private NetworkVariable<int> Dice2Value = new NetworkVariable<int>(0);
    private NetworkVariable<int> DiceSum = new NetworkVariable<int>(0);

    private NetworkVariable<int> Wand1Value = new NetworkVariable<int>(0);
    private NetworkVariable<int> Wand2Value = new NetworkVariable<int>(0);
    private NetworkVariable<int> Wand3Value = new NetworkVariable<int>(0);
    private NetworkVariable<int> WandSum = new NetworkVariable<int>(0);

    private NetworkVariable<int> HostScore = new NetworkVariable<int>(0);
    private NetworkVariable<int> ClientScore = new NetworkVariable<int>(0);

    private NetworkVariable<bool> isHostTurn = new NetworkVariable<bool>(true);

    private NetworkVariable<FixedString4096Bytes> GameLog = new NetworkVariable<FixedString4096Bytes>("");

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            spawnWandsServerRpc();
            spawnDiceServerRpc();
        }
        
        throwWandsButton.onClick.AddListener(() =>
        {
            throwWandsServerRpc();
        });

        throwDiceButton.onClick.AddListener(() =>
        {
            throwDiceServerRpc();
        });

        passButton.onClick.AddListener(() =>
        {
            if (isHostTurn.Value)
            {
                isHostTurn.Value = false;
                GameLog.Value += "Host passed. It is now Client's turn. ";
            }
            else
            {
                isHostTurn.Value = true;
                GameLog.Value += "Client passed. It is now Host's turn. ";
            }
        });

        declineButton.onClick.AddListener(() =>
        {
            if (isHostTurn.Value)
            {
                isHostTurn.Value = false;
                GameLog.Value += "Host declined. It is now Client's turn. ";
            }
            else
            {
                isHostTurn.Value = true;
                GameLog.Value += "Client declined. It is now Host's turn. ";
            }
        });

        if (IsServer)
        {
            // on value changed
            Dice1Value.OnValueChanged += (oldValue, newValue) =>
            {
                Dice1Display.text = newValue.ToString();
            };

            Dice2Value.OnValueChanged += (oldValue, newValue) =>
            {
                Dice2Display.text = newValue.ToString();
            };

            Wand1Value.OnValueChanged += (oldValue, newValue) =>
            {
                Wand1Display.text = newValue.ToString();
            };

            Wand2Value.OnValueChanged += (oldValue, newValue) =>
            {
                Wand2Display.text = newValue.ToString();
            };

            Wand3Value.OnValueChanged += (oldValue, newValue) =>
            {
                Wand3Display.text = newValue.ToString();
            };

            HostScore.OnValueChanged += (oldValue, newValue) =>
            {
                HostScoreDisplay.text = newValue.ToString();
            };

            ClientScore.OnValueChanged += (oldValue, newValue) =>
            {
                ClientScoreDisplay.text = newValue.ToString();
            };

            GameLog.OnValueChanged += (oldValue, newValue) =>
            {
                GameLogDisplay.text = newValue.ToString();
            };
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void spawnWandsServerRpc()
    {
        wandSpawn1 = Instantiate(wand1, new Vector3(3.0f, 2.0f, 0.0f), Quaternion.identity);
        wandSpawn2 = Instantiate(wand2, new Vector3(4.0f, 2.0f, 0.0f), Quaternion.identity);
        wandSpawn3 = Instantiate(wand3, new Vector3(5.0f, 2.0f, 0.0f), Quaternion.identity);

        wandSpawn1.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
        wandSpawn2.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
        wandSpawn3.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
    }

    [ServerRpc(RequireOwnership = false)]
    public void spawnDiceServerRpc()
    {
        diceSpawn1 = Instantiate(dice1, new Vector3(3.0f, 2.0f, 0.0f), Quaternion.identity);
        diceSpawn2 = Instantiate(dice2, new Vector3(4.0f, 2.0f, 0.0f), Quaternion.identity);

        diceSpawn1.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
        diceSpawn2.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void throwWandsServerRpc(ServerRpcParams serverRpcParams = default)
    {
        wandSpawn1.GetComponent<NetworkObject>().Despawn();
        wandSpawn2.GetComponent<NetworkObject>().Despawn();
        wandSpawn3.GetComponent<NetworkObject>().Despawn();

        wandSpawn1 = Instantiate(wand1, new Vector3(-18.0f, 2.0f, 0.0f), Quaternion.identity);
        wandSpawn2 = Instantiate(wand2, new Vector3(-19.0f, 2.0f, 0.0f), Quaternion.identity);
        wandSpawn3 = Instantiate(wand3, new Vector3(-17.0f, 2.0f, 0.0f), Quaternion.identity);

        wandSpawn1.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
        wandSpawn2.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
        wandSpawn3.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);

        wandSpawn1.GetComponent<WandScript1>().Roll();
        wandSpawn2.GetComponent<WandScript2>().Roll();
        wandSpawn3.GetComponent<WandScript3>().Roll();
    }

    [ServerRpc(RequireOwnership = false)]
    private void throwDiceServerRpc(ServerRpcParams serverRpcParams = default)
    {
        diceSpawn1.GetComponent<NetworkObject>().Despawn();
        diceSpawn2.GetComponent<NetworkObject>().Despawn();

        diceSpawn1 = Instantiate(dice1, new Vector3(-18.0f, 2.0f, 0.0f), Quaternion.identity);
        diceSpawn2 = Instantiate(dice2, new Vector3(-19.0f, 2.0f, 0.0f), Quaternion.identity);

        diceSpawn1.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
        diceSpawn2.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);

        diceSpawn1.GetComponent<DiceScript1>().Roll();
        diceSpawn2.GetComponent<DiceScript2>().Roll();
    }
}