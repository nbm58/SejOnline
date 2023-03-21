using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUI : MonoBehaviour
{
    public GameModel gameModel;

    public void ThrowWands()
    {
        // Throw the wands
        gameModel.ThrowWands();
    }

    public void ThrowDice()
    {
        // Clear the previous dice
        gameModel.ClearDiceDisplay();
        // Throw the dice
        gameModel.ThrowDice();
    }

    public void Decline()
    {
        gameModel.DeclineHand();
    }

    public void Pass()
    {
        gameModel.Pass();
    }

    public void ToggleScore()
    {
        gameModel.ToggleScoreDisplay();
    }

    public void QuitSej()
    {
        gameModel.QuitSej();
    }

    public void AboutSej()
    {
        gameModel.AboutSej();
    }

    public void Instructions()
    {
        gameModel.Instructions();
    }
}
