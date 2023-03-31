using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayDieSide : MonoBehaviour
{
    public TextMeshProUGUI side;
    public float rollDelay;
    public int sideUp;
    public GameModel game;

    // event for signaling die roll stopped
    public delegate void DieRollStopEvent();
    public event DieRollStopEvent DieStop;

    // Start is called before the first frame update
    void Start()
    {
        sideUp = 1;
        side.text = "1";
     }

    // Update is called once per frame
    void Update()
    {

    }

    public int GetSide()
    {
        return sideUp;
    }

    public void ThrowDie()
    {
        game.DisableButtons();
        StartCoroutine(ThrowDieLoop(rollDelay));
    }

    IEnumerator ThrowDieLoop(float delayTime)
    { 
        for (int count = 0; count < 10; count++)
        {
            sideUp = Random.Range(1, 7);
            side.text = sideUp.ToString();
            yield return new WaitForSeconds(delayTime + Random.Range(0.0f, 0.5f));
        }
        this.DieStop();
        yield return null;
    }
}
