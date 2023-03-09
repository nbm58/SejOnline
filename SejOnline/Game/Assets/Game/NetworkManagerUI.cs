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

public class NetworkManagerUI : NetworkBehaviour
{
    public Button hostButton;
    public Button clientButton; // TODO: make these serialized fields?
    public Button throwButton;
    public GameObject wand1;
    public GameObject wand2;
    public GameObject wand3;
    public InputField input;

    GameObject wandSpawn1;
    GameObject wandSpawn2;
    GameObject wandSpawn3;

    private void Awake()
    {
        hostButton.onClick.AddListener(() =>
        {
            GameObject testRelay = GameObject.Find("RelayServer");
            testRelay.GetComponent<RelayServer>().CreateRelay();
        });

        clientButton.onClick.AddListener(() =>
        {
            GameObject testRelay = GameObject.Find("RelayServer");
            testRelay.GetComponent<RelayServer>().JoinRelay(input.text);
        });

        throwButton.onClick.AddListener(() =>
        {
            throwWandsServerRpc();
        });
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
}