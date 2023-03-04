using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayWandSide : MonoBehaviour
{
    public GameObject side = null;
    public GameObject ship = null;
    public GameObject star = null;
    public CanvasRenderer rend;
    public CanvasRenderer rendi;
    public float rollDelay;
    public int sideUp;
    public GameModel game;

    // event for signaling wand roll stopped
    public delegate void RollStopEvent();
    public event RollStopEvent WandStop;

    //private float count;

    // Start is called before the first frame update
    void Start()
    {
        rend = side.GetComponent<CanvasRenderer>();
        rendi = ship.GetComponent<CanvasRenderer>();
        star.SetActive(false);
        ship.SetActive(false);
        rend.SetColor(Color.white);
        sideUp = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int GetSide()
    {
        return sideUp;
    }

    public void ThrowWand()
    {
        game.DisableButtons();
        StartCoroutine(ThrowWandLoop(rollDelay));
    }

    IEnumerator ThrowWandLoop(float delayTime)
    { 
        for (int count = 0; count < 10; count++)
        {
            sideUp = Random.Range(0, 4);
            switch (sideUp)
            {
                case 0: // white
                    star.SetActive(false);
                    ship.SetActive(false);
                    rend.SetColor(Color.white);
                    //sideUp = Random.Range(0, 4);
                    break;
                case 1: // star
                    star.SetActive(true);
                    ship.SetActive(false);
                    rend.SetColor(Color.yellow);
                    //sideUp = Random.Range(0, 4);
                    break;
                case 2: // black
                    star.SetActive(false);
                    ship.SetActive(false);
                    rend.SetColor(Color.black);
                    //sideUp = Random.Range(0, 4);
                    break;
                case 3: // ship
                    star.SetActive(false);
                    ship.SetActive(true);
                    rend.SetColor(new Color(0.2206f, 0.8902f, 0.8902f, 1.0f));
                    rendi.SetColor(new Color(0.2206f, 0.8902f, 0.8902f, 1.0f));
                    //sideUp = Random.Range(0, 4);
                    break;
            }

            yield return new WaitForSeconds(delayTime + Random.Range(0.0f, 0.5f));
        }

        this.WandStop();
        yield return null;
    }
}
