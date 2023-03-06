using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameModel : MonoBehaviour
{
    // the tally of games they've won
    private int player1Wins;
    private int player2Wins;
    // the tally of points they've won
    private int player1Score;
    private int player2Score;
    // which to show (controlled by toggle button)
    private bool displayGames;
    // for passing purposes, in game or hand mode?
    private bool hand;

    // their score in current game
    private int player1GameScore;
    private int player2GameScore;

    // their dice total
    private int player1Roll;
    private int player2Roll;

    private bool player1WandControl;
    private bool player1DieControl;

    private bool pass1;
    private bool pass2;

    // UI hooks
    public TextMeshProUGUI P1Score;
    public TextMeshProUGUI P2Score;
    public TextMeshProUGUI P1GameScore;
    public TextMeshProUGUI P2GameScore;
    public TextMeshProUGUI P1Roll;
    //public TextMeshProUGUI P2Roll;
    public GameObject ButtonP1ThrowWand;
    //public GameObject ButtonP2ThrowWand;
    public GameObject ButtonP1Decline;
    //public GameObject ButtonP2Decline;
    public GameObject ButtonP1ThrowDie;
    //public GameObject ButtonP2ThrowDie;
    public GameObject ButtonP1Pass;
    //public GameObject ButtonP2Pass;
    public TextMeshProUGUI Message;
    public GameObject Wand1;
    public GameObject Wand2;
    public GameObject Wand3;
    public GameObject Die1;
    public GameObject Die2;
    public GameObject ButtonInstructions;
    public GameObject ButtonAbout;
    public GameObject ButtonToggleScore;
    public GameObject ButtonQuit;
    public GameObject LongMessage;

    // list of plays in hand
    private List<int> playList;
    private int playIndex;

    // save the message to restore after displaying instructions or about
    private string saveMessage;
    private int paragraph;

    // also save the button state
    private List<bool> buttonState;

    // Start is called before the first frame update
    void Start()
    {
        player1Wins = 0;
        player2Wins = 0;

        player1Score = 0;
        player2Score = 0;

        displayGames = true;
        hand = false;

        player1GameScore = 0;
        player2GameScore = 0;

        player1Roll = 0;
        player2Roll = 0;

        player1WandControl = true;
        player1DieControl = true;
        pass1 = false;
        pass2 = false;

        ButtonP1ThrowDie.GetComponent<Button>().interactable = false;
        //ButtonP2ThrowDie.GetComponent<Button>().interactable = false;

        ButtonP1Decline.GetComponent<Button>().interactable = false;
        //ButtonP2Decline.GetComponent<Button>().interactable = false;

        Message.text = "Welcome!  To begin playing, Player 1 should either throw the wands or pass.";
        paragraph = 0;
        LongMessage.SetActive(false);
    }

    /* Target of the Throw Wands buttons; this kicks off the hand, using 
     * coroutines to randomize the wands and an event handler to resume
     * after the wands have stopped rolling. */
    public void ThrowWands()
    {
        Message.text = "Throwing the Wands...";

        //Throw the wands
        Wand1.GetComponent<DisplayWandSide>().ThrowWand();
        Wand2.GetComponent<DisplayWandSide>().ThrowWand();
        Wand3.GetComponent<DisplayWandSide>().ThrowWand();
    }

    /* When the event handler determines that the wands have all stopped, this
     * is the routine it returns to.  A bit complicated, it sorts the visible
     * sides in order (white, star, ship, black) and then determines the
     * plays for the hand based on the combinations and the rules for Sej. At
     * the end, it will reset the buttons appropriately so play continues. */
    public void SortWands()
    {
        int wand1 = Wand1.GetComponent<DisplayWandSide>().GetSide();
        int wand2 = Wand2.GetComponent<DisplayWandSide>().GetSide();
        int wand3 = Wand3.GetComponent<DisplayWandSide>().GetSide();

        // Adjust the game state
        // First get wands in order: white, star, ship, black
        List<int> wandList = new List<int>();
        playList = new List<int>();
        if (wand1 == 0)
            wandList.Add(wand1);
        if (wand2 == 0)
            wandList.Add(wand2);
        if (wand3 == 0)
            wandList.Add(wand3);
        if (wand1 == 1)
            wandList.Add(wand1);
        if (wand2 == 1)
            wandList.Add(wand2);
        if (wand3 == 1)
            wandList.Add(wand3);
        if (wand1 == 3)
            wandList.Add(wand1);
        if (wand2 == 3)
            wandList.Add(wand2);
        if (wand3 == 3)
            wandList.Add(wand3);
        if (wand1 == 2)
            wandList.Add(wand1);
        if (wand2 == 2)
            wandList.Add(wand2);
        if (wand3 == 2)
            wandList.Add(wand3);

        // next, set the plays in order
        if (wandList[0] == 0) // if first wand is white
        {
            if (wandList[1] == 0) // if second wand is white
            {
                if (wandList[2] == 0) // all wands white
                {
                    Message.text = "Playing for the win...";
                    playList.Add(0);
                }
                else if (wandList[2] == 1)  // two white, one star
                {
                    Message.text = "Whites take value of star, playing for 36...";
                    playList.Add(1);
                }
                else if (wandList[2] == 3)  // two white, one ship
                {
                    Message.text = "Whites take value of ship, playing for 30...";
                    playList.Add(2);
                }
                else if (wandList[2] == 2)  // two white, one black
                {
                    Message.text = "Whites pair for 5, then black...";
                    playList.Add(4);
                    playList.Add(9);
                }
                else // error
                {
                    Message.text = "There has been an error reading the wands!";
                }
            }
            else if (wandList[1] == 1)  // if second wand is Star
            {
                if (wandList[2] == 1)  // one white, two star
                {
                    Message.text = "White takes value of stars, playing for 36...";
                    playList.Add(1);
                }
                else if (wandList[2] == 3)  // one white, one star, one ship
                {
                    Message.text = "White takes value of star, playing for 24,\nthen playing for ship at 10 pts...";
                    playList.Add(5);
                    playList.Add(8);
                }
                else if (wandList[2] == 2)  // one white, one star, one black
                {
                    Message.text = "White takes value of star, playing for 24,\nthen playing for black...";
                    playList.Add(5);
                    playList.Add(9);
                }
                else // error
                {
                    Message.text = "There has been an error reading the wands!";
                }
            }
            else if (wandList[1] == 3)  // if second wand is Ship
            {
                if (wandList[2] == 3)  // one white, two ship
                {
                    Message.text = "White takes value of ships, playing for 30...";
                    playList.Add(2);
                }
                else if (wandList[2] == 2)  // one white, one ship, one black
                {
                    Message.text = "White takes value of ship, playing for 20,\nthen playing for black...";
                    playList.Add(6);
                    playList.Add(9);
                }
                else // error
                {
                    Message.text = "There has been an error reading the wands!";
                }
            }
            else if (wandList[1] == 2)  // one white, two black
            {
                Message.text = "White takes value of black, playing for loss...";
                playList.Add(3);
            }
            else // error
            {
                Message.text = "There has been an error reading the wands!";
            }
        }
        else if (wandList[0] == 1)  // if first wand is Star
        {
            if (wandList[1] == 1) // if second wand is Star
            {
                if (wandList[2] == 1)  // three star
                {
                    Message.text = "Three stars, playing for 36...";
                    playList.Add(1);
                }
                else if (wandList[2] == 3)  // two star, one ship
                {
                    Message.text = "Two stars and one ship, playing for 24 and then 10...";
                    playList.Add(5);
                    playList.Add(8);
                }
                else if (wandList[2] == 2)  // two star, one black
                {
                    Message.text = "Two stars, playing for 24 and then black...";
                    playList.Add(5);
                    playList.Add(9);
                }
                else // error
                {
                    Message.text = "There has been an error reading the wands!";
                }
            }
            else if (wandList[1] == 3) // if second wand is Ship
            {
                if (wandList[2] == 3)  // one star, two ship
                {
                    Message.text = "One star, playing for 12, and then\n two ships, playing for 20...";
                    playList.Add(7);
                    playList.Add(6);
                }
                else if (wandList[2] == 2)  // one star, one ship, one black
                {
                    Message.text = "Playing one star for 12, one ship for 10, \nand then black...";
                    playList.Add(7);
                    playList.Add(8);
                    playList.Add(9);
                }
                else // error
                {
                    Message.text = "There has been an error reading the wands!";
                }
            }
            else if (wandList[1] == 2) // one star and two black
            {
                Message.text = "One star, playing for 12, and then two black...";
                playList.Add(7);
                playList.Add(9);
                playList.Add(9);
            }
            else // error
            {
                Message.text = "There has been an error reading the wands!";
            }
        }
        else if (wandList[0] == 3)  // if first wand is Ship
        {
            if (wandList[1] == 3) // if second wand is Ship
            {
                if (wandList[2] == 3)  // three ship
                {
                    Message.text = "Three ships, playing for 30...";
                    playList.Add(2);
                }
                else if (wandList[2] == 2)  // two ship, one black
                {
                    Message.text = "Two ships, playing for 20 and then black...";
                    playList.Add(6);
                    playList.Add(9);
                }
                else // error
                {
                    Message.text = "There has been an error reading the wands!";
                }
            }
            else if (wandList[1] == 2) // one ship and two black
            {
                Message.text = "One ship, playing for 10 and then two black...";
                playList.Add(8);
                playList.Add(9);
                playList.Add(9);

            }
            else // error
            {
                Message.text = "There has been an error reading the wands!";
            }
        }
        else if (wandList[0] == 2)  // all wands are black
        {
            Message.text = "Three black, playing for loss...";
            playList.Add(3);
        }
        else // error
        {
            Message.text = "There has been an error reading the wands!";
        }

        playIndex = 0;  // start hand with first play

        // Set the UI appropriately
        if (player1WandControl)
        {
            ButtonP1Decline.GetComponent<Button>().interactable = true;
            ButtonP1ThrowDie.GetComponent<Button>().interactable = true;
            ButtonP1Pass.GetComponent<Button>().interactable = true;
        }
        else
        {
            //ButtonP2Decline.GetComponent<Button>().interactable = true;
            //ButtonP2ThrowDie.GetComponent<Button>().interactable = true;
            //ButtonP2Pass.GetComponent<Button>().interactable = true;
        }
        ButtonInstructions.GetComponent<Button>().interactable = true;
        ButtonAbout.GetComponent<Button>().interactable = true;
        ButtonToggleScore.GetComponent<Button>().interactable = true;
        ButtonQuit.GetComponent<Button>().interactable = true;
        hand = true;  // we're now playing the hand...
        pass1 = false;
        pass2 = false;
    }

    /* Clear the display fields of the last dice rolls to make the 
     * current throws more visible. */
    public void ClearDiceDisplay()
    {
        P1Roll.text = "Roll";
        //P2Roll.text = "Roll";
    }

    /* Target of the Throw Dice buttons; this kicks off a play, using 
     * coroutines to randomize the dice and an event handler to resume
     * after the dice have stopped rolling. */
    public void ThrowDice()
    {
        Message.text += "\nThrowing the Dice...";

        // Throw the wands
        Die1.GetComponent<DisplayDieSide>().ThrowDie();
        Die2.GetComponent<DisplayDieSide>().ThrowDie();
    }

    /* When the event handler determines that the dice have all stopped, this
     * is the routine it returns to.  It tallies the dice roll and stores it
     * to be compared with the other player, determines the winner of the play
     * if needed, and sets the UI for continued play. */
    public void TallyDice()
    {
        pass1 = false;
        pass2 = false;
        int tally = Die1.GetComponent<DisplayDieSide>().GetSide() + Die2.GetComponent<DisplayDieSide>().GetSide();

        if (player1DieControl)
        {
            player1DieControl = false;
            player1Roll = tally;
            P1Roll.text = "Roll\n " + tally.ToString();
            if (player2Roll == 0)
            {
                Message.text = "Player 2 must roll...";
//                ButtonP2ThrowDie.GetComponent<Button>().interactable = true;
                ThrowDice();
            }
            else
                ScorePlay();
        }
        else
        {
            player1DieControl = true;
            player2Roll = tally;
            //P2Roll.text = "Roll\n " + tally.ToString();
            if (player1Roll == 0)
            {
                Message.text = "Player 1 must roll...";
//                ButtonP1ThrowDie.GetComponent<Button>().interactable = true;
                ThrowDice();
            }
            else
                ScorePlay();
        }
        ButtonInstructions.GetComponent<Button>().interactable = true;
        ButtonAbout.GetComponent<Button>().interactable = true;
        ButtonToggleScore.GetComponent<Button>().interactable = true;
        ButtonQuit.GetComponent<Button>().interactable = true;
    }

    /* When an action is in progress, the buttons are disabled, so that 
     * commands do not inadvertently stack. */
    public void DisableButtons()
    {
        //Debug.Log("Disabling Buttons?");
        ButtonP1ThrowWand.GetComponent<Button>().interactable = false;
        //ButtonP2ThrowWand.GetComponent<Button>().interactable = false;
        ButtonP1Decline.GetComponent<Button>().interactable = false;
        //ButtonP2Decline.GetComponent<Button>().interactable = false;
        ButtonP1ThrowDie.GetComponent<Button>().interactable = false;
        //ButtonP2ThrowDie.GetComponent<Button>().interactable = false;
        ButtonP1Pass.GetComponent<Button>().interactable = false;
        //ButtonP2Pass.GetComponent<Button>().interactable = false;
        ButtonInstructions.GetComponent<Button>().interactable = false;
        ButtonAbout.GetComponent<Button>().interactable = false;
        ButtonToggleScore.GetComponent<Button>().interactable = false;
        ButtonQuit.GetComponent<Button>().interactable = false;
    }

    /* On declining the hand, return to game mode and pass control to
     * the next player. */
    public void DeclineHand()
    {
        DisableButtons();
        if (player1WandControl)
        {
            player1WandControl = false;
            player1DieControl = false;
            //ButtonP2ThrowWand.GetComponent<Button>().interactable = true;
            //ButtonP2Pass.GetComponent<Button>().interactable = true;
            Message.text = "Hand Declined! Passing wands to Player2...";
        }
        else
        {
            player1WandControl = true;
            player1DieControl = true;
            ButtonP1ThrowWand.GetComponent<Button>().interactable = true;
            ButtonP1Pass.GetComponent<Button>().interactable = true;
            Message.text = "Hand Declined! Passing wands to Player 1...";
        }
        ButtonInstructions.GetComponent<Button>().interactable = true;
        ButtonAbout.GetComponent<Button>().interactable = true;
        ButtonToggleScore.GetComponent<Button>().interactable = true;
        ButtonQuit.GetComponent<Button>().interactable = true;
        hand = false;  // we're now playing the game...
        pass1 = false;
        pass2 = false;
    }

    /* This is the target of the Pass buttons, but the behavior is different
     * depending on whether you are passing wands or dice, so it calls two
     * subroutines. */
    public void Pass()
    {
        if (!hand)
            PassWands();
        else
            PassDice();
    }

    /* Passing Wands keeps track of the passes and forces play if both players
     * have already passed. */
    public void PassWands()
    {
        DisableButtons();
        if (!pass1)
            pass1 = true;
        else if (!pass2)
            pass2 = true;

        // Set the UI appropriately
        if (player1WandControl)
        {
            player1WandControl = false;
            player1DieControl = false;
            //ButtonP2ThrowWand.GetComponent<Button>().interactable = true;
            //if (!pass2)
                //ButtonP2Pass.GetComponent<Button>().interactable = true;
            Message.text = "Passing wands to Player 2...";
        }
        else
        {
            player1WandControl = true;
            player1DieControl = true;
            ButtonP1ThrowWand.GetComponent<Button>().interactable = true;
            if (!pass2)
                ButtonP1Pass.GetComponent<Button>().interactable = true;
            Message.text = "Passing wands to Player 1...";
        }
        ButtonInstructions.GetComponent<Button>().interactable = true;
        ButtonAbout.GetComponent<Button>().interactable = true;
        ButtonToggleScore.GetComponent<Button>().interactable = true;
        ButtonQuit.GetComponent<Button>().interactable = true;
    }

    /* If in a hand and both players have passed the dice, then the play is
     * discarded.  However, if either player rolls, then the other player is
     * forced to also roll. */
    public void PassDice()
    {
        DisableButtons();
        if (!pass1)
        {
            pass1 = true;
            if (player1DieControl)
            {
                player1DieControl = false;
                // Update the UI for continued play
                //ButtonP2ThrowDie.GetComponent<Button>().interactable = true;
                //ButtonP2Pass.GetComponent<Button>().interactable = true;
                Message.text += "\nPassing to Player 2...";
            }
            else
            {
                player1DieControl = true;
                // Update the UI for continued play
                ButtonP1ThrowDie.GetComponent<Button>().interactable = true;
                ButtonP1Pass.GetComponent<Button>().interactable = true;
                Message.text += "\nPassing to Player 1...";
            }
        }
        else
        {
            // fake a draw
            player1Roll = 0;
            player2Roll = 0;
            ScorePlay();
        }
    }

    /* Both die have been rolled, score the play. The play possibilities in
     * the playList are as follows:
     * 
     *      0 = 3 white                 (game win)
     *      1 = 3 star                  (36)
     *      2 = 3 ship                  (30)
     *      3 = 3 black                 (game loss)
     *      4 = 2 white (with black)    (5)
     *      5 = 2 star                  (24)
     *      6 = 2 ship                  (20)
     *      7 = 1 star                  (12)
     *      8 = 1 ship                  (10)
     *      9 = 1 black                 (point loss)
     *      
     * There can be up to three plays in playList; there will always be at
     * least one play. After scoring, the score display is updated and then
     * the UI is set appropriately for continued play.
     *
     * Special circumstance: Game enders will start a new game.  plays 0 and 3
     * or a score over 100 ends the current game. */
    public void ScorePlay()
    {
        if (player1Roll == player2Roll)
        {
            if (player1Roll == 0)
            {
                // from faking a draw for passing...
                Message.text = "Both players pass, play declined...";
            }
            else
            {
                // C J Cherryh did not include this possibility in the rules.
                // This is simpler than a series of sudden-death rolls.
                Message.text = "Draw! This play has no effect.";
            }
        }
        else if (player1Roll > player2Roll)
        {
            Message.text = "Player 1 wins the roll!";
            switch (playList[playIndex])
            {
                case 0:
                    player1GameScore = 100;
                    break;
                case 1:
                    player1GameScore += 36;
                    break;
                case 2:
                    player1GameScore += 30;
                    break;
                case 3:
                    player1GameScore = 0;
                    player2GameScore = 100;
                    break;
                case 4:
                    player1GameScore += 5;
                    break;
                case 5:
                    player1GameScore += 24;
                    break;
                case 6:
                    player1GameScore += 20;
                    break;
                case 7:
                    player1GameScore += 12;
                    break;
                case 8:
                    player1GameScore += 10;
                    break;
                case 9:
                    player1GameScore = 0;
                    break;
            }
        }
        else
        {
            Message.text = "Player 2 wins the roll!";
            switch (playList[playIndex])
            {
                case 0:
                    player2GameScore = 100;
                    break;
                case 1:
                    player2GameScore += 36;
                    break;
                case 2:
                    player2GameScore += 30;
                    break;
                case 3:
                    player2GameScore = 0;
                    player1GameScore = 100;
                    break;
                case 4:
                    player2GameScore += 5;
                    break;
                case 5:
                    player2GameScore += 24;
                    break;
                case 6:
                    player2GameScore += 20;
                    break;
                case 7:
                    player2GameScore += 12;
                    break;
                case 8:
                    player2GameScore += 10;
                    break;
                case 9:
                    player2GameScore = 0;
                    break;
            }
        }

        // point to next play (maybe)
        playIndex++;

        // Update the score display
        UpdateScoreDisplay();

        // Check for game enders
        if (player1GameScore >= 100)
        {
            Message.text = "Player 1 wins!";
            player1Wins++;
            player1Score += player1GameScore;
            player1GameScore = 0;
            player2Score += player2GameScore;
            player2GameScore = 0;
            hand = false;  // back to wands
            player1WandControl = false; // courtesy; hand's loser gets the wands
            player1DieControl = false;
            pass1 = false;
            pass2 = false;

            // Update the UI for continued play
            //ButtonP2ThrowWand.GetComponent<Button>().interactable = true;
            //ButtonP2Pass.GetComponent<Button>().interactable = true;
            Message.text += "\nNew game starting with Player 2...";

            ButtonInstructions.GetComponent<Button>().interactable = true;
            ButtonAbout.GetComponent<Button>().interactable = true;
            ButtonToggleScore.GetComponent<Button>().interactable = true;
            ButtonQuit.GetComponent<Button>().interactable = true;
        }
        else if (player2GameScore >= 100)
        {
            Message.text = "Player 2 wins!";
            player2Wins++;
            player1Score += player1GameScore;
            player1GameScore = 0;
            player2Score += player2GameScore;
            player2GameScore = 0;
            hand = false;  // back to wands
            player1WandControl = true; // courtesy; hand's loser gets the wands
            player1DieControl = true;
            pass1 = false;
            pass2 = false;

            // Update the UI for continued play
            ButtonP1ThrowWand.GetComponent<Button>().interactable = true;
            ButtonP1Pass.GetComponent<Button>().interactable = true;
            Message.text += "\nNew game starting with Player 1...";
        }
        else if (playIndex >= playList.Count) // if out of plays, back to wands
        {
            hand = false;  // back to wands
            pass1 = false;
            pass2 = false;

            if (player1WandControl)
            {
                player1WandControl = false;
                player1DieControl = false;

                // Update the UI for continued play
                //ButtonP2ThrowWand.GetComponent<Button>().interactable = true;
                //ButtonP2Pass.GetComponent<Button>().interactable = true;
                Message.text += "\nNext hand starting with Player 2...";
            }
            else
            {
                player1WandControl = true;
                player1DieControl = true;

                // Update the UI for continued play
                ButtonP1ThrowWand.GetComponent<Button>().interactable = true;
                ButtonP1Pass.GetComponent<Button>().interactable = true;
                Message.text += "\nNext hand starting with Player 1...";
            }
        } 
        else if ((playList[playIndex] == 9) && (player1GameScore == 0) && (player2GameScore == 0))
        {
            // no point in playing black if both have 0.
            // This can only happen on second black in hand.  If one black in hand, points have
            // already been won by a player.  Same if two blacks in hand, when the first black
            // is reached. And with three blacks, the play is for instant loss.
            // So, just end the hand with a message explaining the skipped play.
            hand = false;  // back to wands
            pass1 = false;
            pass2 = false;
            Message.text += "\nSkipping play for black since both players are scoreless...";

            if (player1WandControl)
            {
                player1WandControl = false;
                player1DieControl = false;

                // Update the UI for continued play
                //ButtonP2ThrowWand.GetComponent<Button>().interactable = true;
                //ButtonP2Pass.GetComponent<Button>().interactable = true;
                Message.text += "\nNext hand starting with Player 2...";
            }
            else
            {
                player1WandControl = true;
                player1DieControl = true;

                // Update the UI for continued play
                ButtonP1ThrowWand.GetComponent<Button>().interactable = true;
                ButtonP1Pass.GetComponent<Button>().interactable = true;
                Message.text += "\nNext hand starting with Player 1...";
            }
        }
        else // next play in hand
        {
            switch(playList[playIndex])
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    Message.text += "\nWhat are you doing here? This is impossible!";
                    break;
                case 6:
                    Message.text += "\nPlaying for two Ships, 20 pts...";
                    break;
                case 7:
                    Message.text += "\nPlaying for Star, 12 pts...";
                    break;
                case 8:
                    Message.text += "\nPlaying for Ship, 10 pts...";
                    break;
                case 9:
                    Message.text += "\nPlaying for Black, loss of points...";
                    break;
            }
            pass1 = false;
            pass2 = false;

            if (!player1DieControl)
            {
                // Update the UI for continued play
                //ButtonP2ThrowDie.GetComponent<Button>().interactable = true;
                //ButtonP2Pass.GetComponent<Button>().interactable = true;
                Message.text += "\nNext play starting with Player 2...";
            }
            else
            {
                // Update the UI for continued play
                ButtonP1ThrowDie.GetComponent<Button>().interactable = true;
                ButtonP1Pass.GetComponent<Button>().interactable = true;
                Message.text += "\nNext play starting with Player 1...";
            }
        }

        UpdateScoreDisplay();
        ButtonInstructions.GetComponent<Button>().interactable = true;
        ButtonAbout.GetComponent<Button>().interactable = true;
        ButtonToggleScore.GetComponent<Button>().interactable = true;
        ButtonQuit.GetComponent<Button>().interactable = true;

        // Reset the rolls
        player1Roll = 0;
        player2Roll = 0;
    }

    /* Updates the UI score fields */
    public void UpdateScoreDisplay()
    {
        // Display games or cumulative score based on the displayGames flag
        if (displayGames)
        {
            P1Score.text = "Player 1 Score\n" + player1Wins.ToString() + " Games";
            P2Score.text = "Player 2 Score\n" + player2Wins.ToString() + " Games";
        }
        else
        {
            int display1Score = player1Score + player1GameScore;
            if (display1Score < 1000)
                P1Score.text = "Player 1 Score\n" + display1Score.ToString() + " pts";
            else if (display1Score < 100000)
                P1Score.text = "Player 1 Score\n" + display1Score.ToString() + " pts";
            else
                P1Score.text = "Player 1 Score\n" + display1Score.ToString() + " pts";

            int display2Score = player2Score + player2GameScore;
            if (display2Score < 1000)
                P2Score.text = "Player 2 Score\n" + display2Score.ToString() + " pts";
            else if (display2Score < 100000)
                P2Score.text = "Player 2 Score\n" + display2Score.ToString() + " pts";
            else
                P2Score.text = "Player 2 Score\n" + display2Score.ToString() + " pts";
        }

        // and now update the current game scores
        if (player1GameScore < 10)
            P1GameScore.text = "This Game\n" + player1GameScore.ToString();
        else
            P1GameScore.text = "This Game\n" + player1GameScore.ToString();

        if (player2GameScore < 10)
            P2GameScore.text = "This Game\n" + player2GameScore.ToString();
        else
            P2GameScore.text = "This Game\n" + player2GameScore.ToString();
    }

    /* sets/resets the flag to display games won or cumulative score */
    public void ToggleScoreDisplay()
    {
        if (displayGames)
            displayGames = false;
        else
            displayGames = true;
        UpdateScoreDisplay();
    }

    /* no frills exit; may save progress in later version */
    public void QuitSej()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        Debug.Log("Main Menu Scene Loaded");
    }

    public void AboutSej()
    {
        if (paragraph == 0)
        {
            SaveButtonState();
            DisableButtons();
            ButtonAbout.GetComponent<Button>().interactable = true;
            ButtonQuit.GetComponent<Button>().interactable = true;
            saveMessage = Message.text;
            paragraph = 1;
            Message.text = "Sej is the creation of author C.J. Cherryh in the book \"Serpent's Reach\", 1980";
            Message.text += "\nThis adaptation is by Patrick E. Kelley, copyright 2022";
            Message.text += "\nVersion 1.0, May 21, 2022";
            Message.text += "\n";
            Message.text += "\nThe code is available for fair use and intended for a class in Unity Programming";
            Message.text += "\nThe progam may not be used for commercial purposes.";
            Message.text += "\n\t\t\tClick the 'About Sej' button again to return to the game.";
        }
        else
        {
            RestoreButtonState();
            paragraph = 0;
            Message.text = saveMessage;
        }
    }

        public void Instructions()
    {
        if (paragraph == 0)
        {
            SaveButtonState();
            DisableButtons();
            saveMessage = Message.text;
            Message.text = "";
            ButtonInstructions.GetComponent<Button>().interactable = true;
            ButtonQuit.GetComponent<Button>().interactable = true;
            LongMessage.SetActive(true);
            paragraph = 1;
        }
        else
        {
            RestoreButtonState();
            paragraph = 0;
            LongMessage.SetActive(false);
            Message.text = saveMessage;
        }
    }

    /* Saves the states of the player buttons in order. */
    public void SaveButtonState()
    {
        buttonState = new List<bool>();
        buttonState.Add(ButtonP1ThrowWand.GetComponent<Button>().interactable);
        //buttonState.Add(ButtonP2ThrowWand.GetComponent<Button>().interactable);
        buttonState.Add(ButtonP1ThrowDie.GetComponent<Button>().interactable);
        //buttonState.Add(ButtonP2ThrowDie.GetComponent<Button>().interactable);
        buttonState.Add(ButtonP1Decline.GetComponent<Button>().interactable);
        //buttonState.Add(ButtonP2Decline.GetComponent<Button>().interactable);
        buttonState.Add(ButtonP1Pass.GetComponent<Button>().interactable);
        //buttonState.Add(ButtonP2Pass.GetComponent<Button>().interactable);
    }

    /* Restores the saved button states */
    public void RestoreButtonState()
    {
        ButtonP1ThrowWand.GetComponent<Button>().interactable = buttonState[0];
        //ButtonP2ThrowWand.GetComponent<Button>().interactable = buttonState[1];
        ButtonP1ThrowDie.GetComponent<Button>().interactable = buttonState[2];
        //ButtonP2ThrowDie.GetComponent<Button>().interactable = buttonState[3];
        ButtonP1Decline.GetComponent<Button>().interactable = buttonState[4];
        //ButtonP2Decline.GetComponent<Button>().interactable = buttonState[5];
        ButtonP1Pass.GetComponent<Button>().interactable = buttonState[6];
        //ButtonP2Pass.GetComponent<Button>().interactable = buttonState[7];

        // always except during About and Instructions
        ButtonInstructions.GetComponent<Button>().interactable = true;
        ButtonAbout.GetComponent<Button>().interactable = true;
        ButtonToggleScore.GetComponent<Button>().interactable = true;
        ButtonQuit.GetComponent<Button>().interactable = true;
    }
}
