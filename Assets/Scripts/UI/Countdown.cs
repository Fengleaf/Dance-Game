using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    private Text number;
    private Outline boundingBoxOuter;
    public int timeLeft = 0;
    public state countdownState;
    public enum state
    {
        PLAY,
        PAUSE,
        STOP
    }
    // Start is called before the first frame update
    void Start()
    {
        timeLeft = 3 * 50;
        countdownState = state.PLAY;
        number = GetComponentInChildren<Text>();
        boundingBoxOuter = GetComponent<Outline>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (countdownState == state.PLAY)
        {
            Color nowC = boundingBoxOuter.effectColor;
            nowC.a = 1;
            boundingBoxOuter.effectColor = nowC;
            timeLeft -= 1;
            number.text = Mathf.Ceil(timeLeft / 50).ToString();
        }
        else if (countdownState == state.PAUSE)
        {
            Color nowC = boundingBoxOuter.effectColor;
            nowC.a = 0;
            boundingBoxOuter.effectColor = nowC;
        }
        else if (countdownState == state.STOP)
        {
            Destroy(gameObject);
        }
    }

    public bool IsEnd()
    {
        if (timeLeft <= 0)
            return true;
        return false;
    }
}
