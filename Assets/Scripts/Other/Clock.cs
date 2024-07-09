using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    public static Clock instance;

    [SerializeField]
    private GameObject blackClock, whiteClock, blackClock3D, whiteClock3D;
    Text blackClockTxt, whiteClockTxt, blackClock3DTxt, whiteClock3DTxt;
    float whiteTime, blackTime, time3D;

    private bool isStopped;

    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    void Start()
    {
        blackClockTxt = blackClock.GetComponent<Text>();
        whiteClockTxt = whiteClock.GetComponent<Text>();

        if(blackClock3D != null)
        {
            blackClock3DTxt = blackClock3D.GetComponent<Text>();
            whiteClock3DTxt = whiteClock3D.GetComponent<Text>();
        }

        whiteTime = Settings.instance.timeForGame;
        blackTime = Settings.instance.timeForGame;

        whiteClockTxt.text = ToString(whiteTime);
        blackClockTxt.text = ToString(blackTime);
    }

    void Update()
    {
        if (isStopped)
        {
            if (GameManager.instance.is3DActive)
            {
                Clock3DMode();
            }

            return;
        }

        if (GameManager.instance.playing && GameManager.instance.whiteTurn && GameManager.instance.turnNumber != 0)
        {
            whiteTime -= Time.deltaTime;
            if (whiteTime < 0)
            {
                whiteClockTxt.text = ToString(0f);
                GameManager.instance.GameOver(-2);
                return;
            }
            whiteClockTxt.text = ToString(whiteTime);
        }
        else if(GameManager.instance.playing && GameManager.instance.turnNumber != 0)
        {
            blackTime -= Time.deltaTime;
            if (blackTime < 0)
            {
                blackClockTxt.text = ToString(0f);
                GameManager.instance.GameOver(2);
                return;
            }
            blackClockTxt.text = ToString(blackTime);
        }
    }

    private string ToString(float time)
    {
        if(time >= 59)
        {
            string seconds = ((int)time % 60).ToString("00");
            string minutes = ((int)time / 60).ToString("00");

            return minutes + ":" + seconds;
        }
        else
        {
            return time.ToString("00.00");
        }
    }

    public void Pause()
    {
        isStopped = true;
    }

    public void Unpause()
    {
        isStopped = false;
    }

    public void Set3DClock()
    {
        Pause();
        time3D = Settings.instance.timeFor3DMove;

        whiteClock3DTxt.text = ToString(whiteTime);
        blackClock3DTxt.text = ToString(blackTime);
    }

    public void Set2DClock()
    {
        Unpause();
        whiteClockTxt.text = ToString(whiteTime);
        blackClockTxt.text = ToString(blackTime);
    }

    public void Clock3DMode()
    {
        time3D -= Time.deltaTime;

        if (time3D < 0)
        {
            GameManager.instance.ChangeTo2D();
        }
        else if (GameManager.instance.whiteTurn)
        {
            whiteClock3DTxt.text = ToString(time3D);
        }
        else
        {
            blackClock3DTxt.text = ToString(time3D);
        }
        
    }
}
