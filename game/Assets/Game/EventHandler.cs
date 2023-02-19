using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;

public class EventHandler : MonoBehaviour
{
    public DisplayWandSide displayWand1Side;
    public DisplayWandSide displayWand2Side;
    public DisplayWandSide displayWand3Side;
    public DisplayDieSide displayDie1Side;
    public DisplayDieSide displayDie2Side;
    public GameObject GameModel;

    private bool lock1;
    private bool lock2;
    private bool dlock;

    void onEnable()
    {

    }

    public void HandleWandStop()
    {
        if (lock1)
        {
            lock1 = false;
        }
        else if (lock2)
        {
            lock2 = false;
        }
        else
        {
            lock1 = true;
            lock2 = true;
            GameModel.GetComponent<GameModel>().SortWands();
        }
    }

    public void HandleDieStop()
    {
        if (dlock)
        {
            dlock = false;
        }
        else
        {
            dlock = true;
            GameModel.GetComponent<GameModel>().TallyDice();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        lock1 = true;
        lock2 = true;
        dlock = true;
        GameObject wand1 = GameObject.Find("Wand1");
        GameObject wand2 = GameObject.Find("Wand2");
        GameObject wand3 = GameObject.Find("Wand3");
        GameObject die1 = GameObject.Find("Die1");
        GameObject die2 = GameObject.Find("Die2");
        displayWand1Side = wand1.GetComponent<DisplayWandSide>();
        displayWand2Side = wand2.GetComponent<DisplayWandSide>();
        displayWand3Side = wand3.GetComponent<DisplayWandSide>();
        displayDie1Side = die1.GetComponent<DisplayDieSide>();
        displayDie2Side = die2.GetComponent<DisplayDieSide>();
        displayWand1Side.WandStop += HandleWandStop;
        displayWand2Side.WandStop += HandleWandStop;
        displayWand3Side.WandStop += HandleWandStop;
        displayDie1Side.DieStop += HandleDieStop;
        displayDie2Side.DieStop += HandleDieStop;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
