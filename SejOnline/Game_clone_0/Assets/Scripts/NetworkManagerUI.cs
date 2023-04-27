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
    [SerializeField] private Button throwWandsButton;
    [SerializeField] private Button throwDiceButton;
    [SerializeField] private Button passButton;
    [SerializeField] private Button declineButton;

    [SerializeField] private GameObject wand1;
    [SerializeField] private GameObject wand2;
    [SerializeField] private GameObject wand3;

    [SerializeField] private GameObject dice1;
    [SerializeField] private GameObject dice2;

    GameObject wandSpawn1;
    GameObject wandSpawn2;
    GameObject wandSpawn3;

    GameObject diceSpawn1;
    GameObject diceSpawn2;

    [SerializeField] private TMP_Text HostScoreDisplay;
    [SerializeField] private TMP_Text ClientScoreDisplay;
    [SerializeField] private TMP_Text TurnDisplay;

    [SerializeField] private TMP_Text Dice1Display;
    [SerializeField] private TMP_Text Dice2Display;

    [SerializeField] private TMP_Text Wand1Display;
    [SerializeField] private TMP_Text Wand2Display;
    [SerializeField] private TMP_Text Wand3Display;

    [SerializeField] private TMP_Text GameLogDisplay;
    [SerializeField] private Hide GameLogHideScript;

    public NetworkVariable<int> Dice1Value = new NetworkVariable<int>(0);
    public NetworkVariable<int> Dice2Value = new NetworkVariable<int>(0);
    private NetworkVariable<int> DiceSum = new NetworkVariable<int>(0);

    public NetworkVariable<FixedString32Bytes> Wand1Value = new NetworkVariable<FixedString32Bytes>("");
    public NetworkVariable<FixedString32Bytes> Wand2Value = new NetworkVariable<FixedString32Bytes>("");
    public NetworkVariable<FixedString32Bytes> Wand3Value = new NetworkVariable<FixedString32Bytes>("");

    private NetworkVariable<int> HostScore = new NetworkVariable<int>(0);
    private NetworkVariable<int> ClientScore = new NetworkVariable<int>(0);

    private NetworkVariable<bool> hostWandControl = new NetworkVariable<bool>(true);
    private NetworkVariable<bool> hostDieControl = new NetworkVariable<bool>(true);
    private NetworkVariable<bool> isHostTurn = new NetworkVariable<bool>(true);
    private NetworkVariable<bool> hand = new NetworkVariable<bool>(false);
    private NetworkVariable<bool> pass1 = new NetworkVariable<bool>(false);
    private NetworkVariable<bool> pass2 = new NetworkVariable<bool>(false);
    private NetworkVariable<int> hostRollValue = new NetworkVariable<int>(0);
    private NetworkVariable<int> clientRollValue = new NetworkVariable<int>(0);

    private NetworkVariable<FixedString4096Bytes> GameLog = new NetworkVariable<FixedString4096Bytes>("");

    private PlayerData playerData;
    private Relay relayScript;
    private ButtonController buttonControllerScript;

    private List<int> playList;
    private int playIndex;

    public override void OnNetworkSpawn()
    {
        playerData = GameObject.Find("DBManager").GetComponent<PlayerData>();
        relayScript = GameObject.Find("RelaySystem").GetComponent<Relay>();
        buttonControllerScript = GameObject.Find("ButtonController").GetComponent<ButtonController>();
        
        if (IsServer)
        {
            spawnWandsServerRpc();
            spawnDiceServerRpc();
        }
        
        throwWandsButton.onClick.AddListener(() =>
        {
            ThrowWands();
        });

        throwDiceButton.onClick.AddListener(() =>
        {
            ThrowDice();
        });

        passButton.onClick.AddListener(() =>
        {
            PassServerRpc();
        });

        declineButton.onClick.AddListener(() =>
        {
            DeclineHandServerRpc();
        });

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
            HostScoreDisplay.text = "Host Score\n" + newValue.ToString() + " points";
        };

        ClientScore.OnValueChanged += (oldValue, newValue) =>
        {
            ClientScoreDisplay.text = "Client Score\n" + newValue.ToString() + " points";
        };

        GameLog.OnValueChanged += (oldValue, newValue) =>
        {
            GameLogDisplay.text = newValue.ToString();
            GameLogHideScript.ShowGameLog();
        };

        isHostTurn.OnValueChanged += (oldValue, newValue) =>
        {
            if (newValue)
            {
                TurnDisplay.text = "Host's Turn";
            }
            else
            {
                TurnDisplay.text = "Client's Turn";
            }
        };

        if (IsHost)
        {
            throwWandsButton.interactable = true;
            throwDiceButton.interactable = false;
            passButton.interactable = false;
            declineButton.interactable = false;
        }
        else
        {
            disableButtons();
        }

        GameLog.Value = "Match has begun...\nHost should begin by throwing the wands...\n";
    }

    void Update()
    {
        if (hostWandControl.Value)
        {
            setIsHostTurn(true);
        }
        else
        {
            if (hostDieControl.Value)
            {
                setIsHostTurn(true);
            }
            else
            {
                setIsHostTurn(false);
            }
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

    public void ThrowWands()
    {
        playRollSoundServerRpc();
        disableButtons();
        throwWandsServerRpc();
        StartCoroutine(SortWands());
    }

    [ServerRpc(RequireOwnership = false)]
    private void throwWandsServerRpc(ServerRpcParams serverRpcParams = default)
    {
        try
        {
            wandSpawn1.GetComponent<NetworkObject>().Despawn();
            wandSpawn2.GetComponent<NetworkObject>().Despawn();
            wandSpawn3.GetComponent<NetworkObject>().Despawn();
        }
        catch
        {
            Debug.Log("Wands not spawned.");
        }

        try
        {
            diceSpawn1.GetComponent<NetworkObject>().Despawn();
            diceSpawn2.GetComponent<NetworkObject>().Despawn();
        }
        catch
        {
            Debug.Log("Dice not spawned.");
        }

        GameLog.Value += "Throwing wands...\n";

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

    IEnumerator SortWands()
    {
        yield return new WaitForSeconds(7);

        SortWandsServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SortWandsServerRpc(ServerRpcParams serverRpcParams = default)
    {
         // First get wands in order: white, star, ship, black
        List<int> wandList = new List<int>();
        playList = new List<int>();
        if (Wand1Value.Value == "White")
            wandList.Add(0);
        if (Wand2Value.Value == "White")
            wandList.Add(0);
        if (Wand3Value.Value == "White")
            wandList.Add(0);
        if (Wand1Value.Value == "Star")
            wandList.Add(1);
        if (Wand2Value.Value == "Star")
            wandList.Add(1);
        if (Wand3Value.Value == "Star")
            wandList.Add(1);
        if (Wand1Value.Value == "Ship")
            wandList.Add(3);
        if (Wand2Value.Value == "Ship")
            wandList.Add(3);
        if (Wand3Value.Value == "Ship")
            wandList.Add(3);
        if (Wand1Value.Value == "Black")
            wandList.Add(2);
        if (Wand2Value.Value == "Black")
            wandList.Add(2);
        if (Wand3Value.Value == "Black")
            wandList.Add(2);

        // next, set the plays in order
        if (wandList[0] == 0) // if first wand is white
        {
            if (wandList[1] == 0) // if second wand is white
            {
                if (wandList[2] == 0) // all wands white
                {
                    GameLog.Value += "Playing for the win...\n";
                    playList.Add(0);
                }
                else if (wandList[2] == 1)  // two white, one star
                {
                    GameLog.Value += "Whites take value of star, playing for 36...\n";
                    playList.Add(1);
                }
                else if (wandList[2] == 3)  // two white, one ship
                {
                    GameLog.Value += "Whites take value of ship, playing for 30...\n";
                    playList.Add(2);
                }
                else if (wandList[2] == 2)  // two white, one black
                {
                    GameLog.Value += "Whites pair for 5, then black...\n";
                    playList.Add(4);
                    playList.Add(9);
                }
                else // error
                {
                    GameLog.Value += "There has been an error reading the wands!\n";
                }
            }
            else if (wandList[1] == 1)  // if second wand is Star
            {
                if (wandList[2] == 1)  // one white, two star
                {
                    GameLog.Value += "White takes value of stars, playing for 36...\n";
                    playList.Add(1);
                }
                else if (wandList[2] == 3)  // one white, one star, one ship
                {
                    GameLog.Value += "White takes value of star, playing for 24,\nthen playing for ship at 10 pts...\n";
                    playList.Add(5);
                    playList.Add(8);
                }
                else if (wandList[2] == 2)  // one white, one star, one black
                {
                    GameLog.Value += "White takes value of star, playing for 24,\nthen playing for black...\n";
                    playList.Add(5);
                    playList.Add(9);
                }
                else // error
                {
                    GameLog.Value += "There has been an error reading the wands!\n";
                }
            }
            else if (wandList[1] == 3)  // if second wand is Ship
            {
                if (wandList[2] == 3)  // one white, two ship
                {
                    GameLog.Value += "White takes value of ships, playing for 30...\n";
                    playList.Add(2);
                }
                else if (wandList[2] == 2)  // one white, one ship, one black
                {
                    GameLog.Value += "White takes value of ship, playing for 20,\nthen playing for black...\n";
                    playList.Add(6);
                    playList.Add(9);
                }
                else // error
                {
                    GameLog.Value += "There has been an error reading the wands!\n";
                }
            }
            else if (wandList[1] == 2)  // one white, two black
            {
                GameLog.Value += "White takes value of black, playing for loss...\n";
                playList.Add(3);
            }
            else // error
            {
                GameLog.Value += "There has been an error reading the wands!\n";
            }
        }
        else if (wandList[0] == 1)  // if first wand is Star
        {
            if (wandList[1] == 1) // if second wand is Star
            {
                if (wandList[2] == 1)  // three star
                {
                    GameLog.Value += "Three stars, playing for 36...\n";
                    playList.Add(1);
                }
                else if (wandList[2] == 3)  // two star, one ship
                {
                    GameLog.Value += "Two stars and one ship, playing for 24 and then 10...\n";
                    playList.Add(5);
                    playList.Add(8);
                }
                else if (wandList[2] == 2)  // two star, one black
                {
                    GameLog.Value += "Two stars, playing for 24 and then black...\n";
                    playList.Add(5);
                    playList.Add(9);
                }
                else // error
                {
                    GameLog.Value += "There has been an error reading the wands!\n";
                }
            }
            else if (wandList[1] == 3) // if second wand is Ship
            {
                if (wandList[2] == 3)  // one star, two ship
                {
                    GameLog.Value += "One star, playing for 12, and then\n two ships, playing for 20...\n";
                    playList.Add(7);
                    playList.Add(6);
                }
                else if (wandList[2] == 2)  // one star, one ship, one black
                {
                    GameLog.Value += "Playing one star for 12, one ship for 10, \nand then black...\n";
                    playList.Add(7);
                    playList.Add(8);
                    playList.Add(9);
                }
                else // error
                {
                    GameLog.Value += "There has been an error reading the wands!\n";
                }
            }
            else if (wandList[1] == 2) // one star and two black
            {
                GameLog.Value += "One star, playing for 12, and then two black...\n";
                playList.Add(7);
                playList.Add(9);
                playList.Add(9);
            }
            else // error
            {
                GameLog.Value += "There has been an error reading the wands!\n";
            }
        }
        else if (wandList[0] == 3)  // if first wand is Ship
        {
            if (wandList[1] == 3) // if second wand is Ship
            {
                if (wandList[2] == 3)  // three ship
                {
                    GameLog.Value += "Three ships, playing for 30...\n";
                    playList.Add(2);
                }
                else if (wandList[2] == 2)  // two ship, one black
                {
                    GameLog.Value += "Two ships, playing for 20 and then black...\n";
                    playList.Add(6);
                    playList.Add(9);
                }
                else // error
                {
                    GameLog.Value += "There has been an error reading the wands!\n";
                }
            }
            else if (wandList[1] == 2) // one ship and two black
            {
                GameLog.Value += "One ship, playing for 10 and then two black...\n";
                playList.Add(8);
                playList.Add(9);
                playList.Add(9);

            }
            else // error
            {
                GameLog.Value += "There has been an error reading the wands!\n";
            }
        }
        else if (wandList[0] == 2)  // all wands are black
        {
            GameLog.Value += "Three black, playing for loss...\n";
            playList.Add(3);
        }
        else // error
        {
            GameLog.Value += "There has been an error reading the wands!\n";
        }

        playIndex = 0;

        // Set the UI appropriately
        if (hostWandControl.Value)
        {
            setUIClientRpc(true, false, true, true, true);
        }
        else
        {
            setUIClientRpc(false, false, true, true, true);
        }

        hand.Value = true;  // we're now playing the hand...
        pass1.Value = false;
        pass2.Value = false;
    }

    public void ThrowDice()
    {
        playRollSoundServerRpc();
        disableButtons();
        throwDiceServerRpc();
        StartCoroutine(TallyDice());
    }

    [ServerRpc(RequireOwnership = false)]
    private void throwDiceServerRpc(ServerRpcParams serverRpcParams = default)
    {
        try
        {
            wandSpawn1.GetComponent<NetworkObject>().Despawn();
            wandSpawn2.GetComponent<NetworkObject>().Despawn();
            wandSpawn3.GetComponent<NetworkObject>().Despawn();
        }
        catch
        {
            Debug.Log("Wands not spawned.");
        }

        try
        {
            diceSpawn1.GetComponent<NetworkObject>().Despawn();
            diceSpawn2.GetComponent<NetworkObject>().Despawn();
        }
        catch
        {
            Debug.Log("Dice not spawned.");
        }

        GameLog.Value += "Throwing dice...\n";

        diceSpawn1 = Instantiate(dice1, new Vector3(-18.0f, 2.0f, 0.0f), Quaternion.identity);
        diceSpawn2 = Instantiate(dice2, new Vector3(-19.0f, 2.0f, 0.0f), Quaternion.identity);

        diceSpawn1.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
        diceSpawn2.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);

        diceSpawn1.GetComponent<DiceScript1>().Roll();
        diceSpawn2.GetComponent<DiceScript2>().Roll();
    }

    IEnumerator TallyDice()
    {
        yield return new WaitForSeconds(7);

        TallyDiceServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void TallyDiceServerRpc(ServerRpcParams serverRpcParams = default)
    {
        pass1.Value = false;
        pass2.Value = false;
        int tally = Dice1Value.Value + Dice2Value.Value;

        if (hostDieControl.Value)
        {
            hostDieControl.Value = false;
            hostRollValue.Value = tally;
            GameLog.Value += "Host rolled " + tally.ToString() + "\n";
            if (clientRollValue.Value == 0)
            {
                GameLog.Value += "Client must roll the dice...\n";
                
                setUIClientRpc(false, false, true, false, false);
            }
            else
            {
                ScorePlayServerRpc();
            }
        }
        else
        {
            hostDieControl.Value = true;
            clientRollValue.Value = tally;
            GameLog.Value += "Client rolled " + tally.ToString() + "\n";
            if (hostRollValue.Value == 0)
            {
                GameLog.Value += "Host must roll the dice...\n";
                
                setUIClientRpc(true, false, true, false, false);
            }
            else
            {
                ScorePlayServerRpc();
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void DeclineHandServerRpc(ServerRpcParams serverRpcParams = default)
    {
        if (hostWandControl.Value)
        {
            hostWandControl.Value = false;
            hostDieControl.Value = false;

            setUIClientRpc(false, true, false, true, false);

            GameLog.Value += "Hand declined. Passing wands to client...\n";
        }
        else
        {
            hostWandControl.Value = true;
            hostDieControl.Value = true;

            setUIClientRpc(true, true, false, true, false);

            GameLog.Value += "Hand declined. Passing wands to host...\n";
        }

        hand.Value = false;
        pass1.Value = false;
        pass2.Value = false;
    }

    [ServerRpc(RequireOwnership = false)]
    public void PassServerRpc(ServerRpcParams serverRpcParams = default)
    {
        if (!hand.Value)
        {
            PassWandsServerRpc();
        }
        else
        {
            PassDiceServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void PassWandsServerRpc(ServerRpcParams serverRpcParams = default)
    {
        if (!pass1.Value)
            pass1.Value = true;
        else if (!pass2.Value)
            pass2.Value = true;
        
        
        if (hostWandControl.Value)
        {
            hostWandControl.Value = false;
            hostDieControl.Value = false;

            if (!pass2.Value)
            {
                setUIClientRpc(false, true, false, true, false);
            }
            else
            {
                setUIClientRpc(false, true, false, false, false);
            }

            GameLog.Value += "Passing wands to client...\n";
        }
        else
        {
            hostWandControl.Value = true;
            hostDieControl.Value = true;

            if (!pass2.Value)
            {
                setUIClientRpc(true, true, false, true, false);
            }
            else
            {
                setUIClientRpc(true, true, false, false, false);
            }

            GameLog.Value += "Passing wands to host...\n";
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void PassDiceServerRpc(ServerRpcParams serverRpcParams = default)
    {
        if (!pass1.Value)
        {
            pass1.Value = true;
            if (hostDieControl.Value)
            {
                hostDieControl.Value = false;
                
                setUIClientRpc(false, false, true, true, false);
                
                GameLog.Value += "Passing dice to client...\n";
            }
            else
            {
                hostDieControl.Value = true;
                
                setUIClientRpc(true, false, true, true, false);
                
                GameLog.Value += "Passing dice to host...\n";
            }
        }
        else
        {
            hostRollValue.Value = 0;
            clientRollValue.Value = 0;
            ScorePlayServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void ScorePlayServerRpc(ServerRpcParams serverRpcParams = default)
    {
        if (hostRollValue.Value == clientRollValue.Value)
        {
            if (hostRollValue.Value == 0)
            {
                GameLog.Value += "Both players passed, hand declined.\n";
            }
            else
            {
                GameLog.Value += "Draw! This play has no effect.\n";
            }
        }
        else if (hostRollValue.Value > clientRollValue.Value)
        {
            GameLog.Value += "Host wins the roll!\n";
            switch (playList[playIndex])
            {
                case 0:
                    HostScore.Value = 100;
                    break;
                case 1:
                    HostScore.Value += 36;
                    break;
                case 2:
                    HostScore.Value += 30;
                    break;
                case 3:
                    HostScore.Value = 0;
                    ClientScore.Value = 100;
                    break;
                case 4:
                    HostScore.Value += 5;
                    break;
                case 5:
                    HostScore.Value += 24;
                    break;
                case 6:
                    HostScore.Value += 20;
                    break;
                case 7:
                    HostScore.Value += 12;
                    break;
                case 8:
                    HostScore.Value += 10;
                    break;
                case 9:
                    HostScore.Value = 0;
                    break;
            }
        }
        else
        {
            GameLog.Value += "Client wins the roll!\n";
            switch (playList[playIndex])
            {
                case 0:
                    ClientScore.Value = 100;
                    break;
                case 1:
                    ClientScore.Value += 36;
                    break;
                case 2:
                    ClientScore.Value += 30;
                    break;
                case 3:
                    ClientScore.Value = 0;
                    HostScore.Value = 100;
                    break;
                case 4:
                    ClientScore.Value += 5;
                    break;
                case 5:
                    ClientScore.Value += 24;
                    break;
                case 6:
                    ClientScore.Value += 20;
                    break;
                case 7:
                    ClientScore.Value += 12;
                    break;
                case 8:
                    ClientScore.Value += 10;
                    break;
                case 9:
                    ClientScore.Value = 0;
                    break;
            }
        }

        playIndex++;

        checkForGameOverServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void checkForGameOverServerRpc(ServerRpcParams serverRpcParams = default)
    {
        if (HostScore.Value >= 100)
        {
            GameLog.Value += "Host wins!\n";
            GameLog.Value += "Game over!\n";
            GameLog.Value += "Press the QUIT button to return to the menu...\n";
            
            HandleStatsClientRpc();
        }
        else if (ClientScore.Value >= 100)
        {
            GameLog.Value += "Client wins!\n";
            GameLog.Value += "Game over!\n";
            GameLog.Value += "Press the QUIT button to return to the menu...\n";
            
            HandleStatsClientRpc();
        }
        else if (playIndex >= playList.Count)  // if out of plays, back to wands
        {
            hand.Value = false;
            pass1.Value = false;
            pass2.Value = false;

            if (hostWandControl.Value)
            {
                hostWandControl.Value = false;
                hostDieControl.Value = false;

                setUIClientRpc(false, true, false, true, false);

                GameLog.Value += "Next hand starting with client...\n";
            }
            else
            {
                hostWandControl.Value = true;
                hostDieControl.Value = true;

                setUIClientRpc(true, true, false, true, false);

                GameLog.Value += "Next hand starting with host...\n";
            }
        }
        else if ((playList[playIndex] == 9) && (HostScore.Value == 0) && (HostScore.Value == 0))
        {
            hand.Value = false;
            pass1.Value = false;
            pass2.Value = false;
            GameLog.Value += "Both players have 0 points. Skipping play for black wand...\n";

            if (hostWandControl.Value)
            {
                hostWandControl.Value = false;
                hostDieControl.Value = false;

                setUIClientRpc(false, true, false, true, false);

                GameLog.Value += "Next hand starting with client...\n"; 
            }
            else
            {
                hostWandControl.Value = true;
                hostDieControl.Value = true;

                setUIClientRpc(true, true, false, true, false);

                GameLog.Value += "Next hand starting with host...\n";
            }
        }
        else // next play in hand
        {
            switch (playList[playIndex])
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    GameLog.Value += "What are you doing here? This is impossible!\n";
                    break;
                case 6:
                    GameLog.Value += "Playing for two Ships, 20 points...\n";
                    break;
                case 7:
                    GameLog.Value += "Playing for Star, 12 points...\n";
                    break;
                case 8:
                    GameLog.Value += "Playing for Ship, 10 points...\n";
                    break;
                case 9:
                    GameLog.Value += "Playing for Black, loss of points...\n";
                    break;
            }
            pass1.Value = false;
            pass2.Value = false;

            if (hostDieControl.Value)
            {
                GameLog.Value += "Next play starting with host...\n";

                setUIClientRpc(true, false, true, true, false);
            }
            else
            {
                GameLog.Value += "Next play starting with client...\n";

                setUIClientRpc(false, false, true, true, false);
            }
        }

        hostRollValue.Value = 0;
        clientRollValue.Value = 0;
    }

    IEnumerator DisableButtons()
    {
        disableButtons();

        yield return new WaitForSeconds(5);

        enableButtons();
    }

    private void disableButtons()
    {
        throwWandsButton.interactable = false;
        throwDiceButton.interactable = false;
        passButton.interactable = false;
        declineButton.interactable = false;
    }

    private void enableButtons()
    {
        throwWandsButton.interactable = true;
        throwDiceButton.interactable = true;
        passButton.interactable = true;
        declineButton.interactable = true;
    }

    [ServerRpc(RequireOwnership = false)]
    private void playRollSoundServerRpc(ServerRpcParams serverRpcParams = default)
    {
        playRollSoundClientRpc();
    }

    [ClientRpc]
    private void playRollSoundClientRpc()
    {
        buttonControllerScript.PlayRoll();
    }

    [ClientRpc]
    private void setUIClientRpc(bool hostTurn, bool wands, bool dice, bool pass, bool decline)
    {
        if (hostTurn)
        {
            if (IsHost)
            {
                throwWandsButton.interactable = wands;
                throwDiceButton.interactable = dice;
                passButton.interactable = pass;
                declineButton.interactable = decline;
            }
            else
            {
                throwWandsButton.interactable = false;
                throwDiceButton.interactable = false;
                passButton.interactable = false;
                declineButton.interactable = false;
            }
        }
        else
        {
            if (IsHost)
            {
                throwWandsButton.interactable = false;
                throwDiceButton.interactable = false;
                passButton.interactable = false;
                declineButton.interactable = false;
            }
            else
            {
                throwWandsButton.interactable = wands;
                throwDiceButton.interactable = dice;
                passButton.interactable = pass;
                declineButton.interactable = decline;
            }
        }
    }

    [ClientRpc]
    private void HandleStatsClientRpc()
    {
        if (HostScore.Value >= 100)
        {
            if (IsHost)
            {
                playerData.incrementGamesWon();
                playerData.incrementGamesPlayed();
            }
            else
            {
                playerData.incrementGamesLost();
                playerData.incrementGamesPlayed();
            }
        }
        else
        {
            if (IsHost)
            {
                playerData.incrementGamesLost();
                playerData.incrementGamesPlayed();
            }
            else
            {
                playerData.incrementGamesWon();
                playerData.incrementGamesPlayed();
            }
        }
    }

    public void handleQuitGame()
    {
        try
        {
            relayScript.CloseConnection();
        }
        catch (Exception e)
        {
            Debug.Log("Relay script not found or failed to close connection: " + e);
        }

        GameLog.Value = "";
        HostScore.Value = 0;
        ClientScore.Value = 0;
        Wand1Value.Value = "";
        Wand2Value.Value = "";
        Wand3Value.Value = "";
        Dice1Value.Value = 0;
        Dice2Value.Value = 0;
        DiceSum.Value = 0;
        hostRollValue.Value = 0;
        clientRollValue.Value = 0;
        hostWandControl.Value = true;
        hostDieControl.Value = true;
        pass1.Value = false;
        pass2.Value = false;
        hand.Value = false;

        throwWandsButton.onClick.RemoveAllListeners();
        throwDiceButton.onClick.RemoveAllListeners();
        passButton.onClick.RemoveAllListeners();
        declineButton.onClick.RemoveAllListeners();
    }

    public void setWand1Value(string value)
    {
        if (IsHost)
        {
            Wand1Value.Value = value;
        }
    }

    public void setWand2Value(string value)
    {
        if (IsHost)
        {
            Wand2Value.Value = value;
        }
    }

    public void setWand3Value(string value)
    {
        if (IsHost)
        {
            Wand3Value.Value = value;
        }
    }

    public void setDice1Value(int value)
    {
        if (IsHost)
        {
            Dice1Value.Value = value;
        }
    }

    public void setDice2Value(int value)
    {
        if (IsHost)
        {
            Dice2Value.Value = value;
        }
    }

    public void setIsHostTurn(bool value)
    {
        if (IsHost)
        {
            isHostTurn.Value = value;
        }
    }
}