using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEventSystem : MonoBehaviour
{

    //For any reference to the event system created, check out https://www.youtube.com/watch?v=gx0Lt4tCDE0, very helpful

    public static UIEventSystem current;

    private void Awake()
    {
        current = this;
    }

    public event Action onNextButtonTriggerEnter;

    public event Action onFetusButtonTriggerEnter;

    public event Action onVideoButtonTriggerEnter;

    public event Action onQuitButtonTriggerEnter;

    public void nextButtonTriggerEnter()
    {
        if(onNextButtonTriggerEnter != null)
        {
            onNextButtonTriggerEnter();
        }
    }

    public void fetusButtonTriggerEnter()
    {
        if(onFetusButtonTriggerEnter != null)
        {
            onFetusButtonTriggerEnter();
        }
    }

    public void videoButtonTriggerEnter()
    {
        if(onVideoButtonTriggerEnter != null)
        {
            onVideoButtonTriggerEnter();
        }
    }

    public void quitButtonTriggerEnter()
    {
        if(onQuitButtonTriggerEnter != null)
        {
            onQuitButtonTriggerEnter();
        }
    }
}
