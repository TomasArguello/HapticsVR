using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonTrigger : MonoBehaviour
{
    int timesTouched;
    public enum ButtonTypes
    {
        Next,
        Fetus,
        Video,
        Quit
    }

    public ButtonTypes currentButtonType ;

    private void Awake()
    {
        timesTouched = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(timesTouched >= 1)
        {
            return;
        }
        else
        {
            timesTouched += 1;
            if(currentButtonType == ButtonTypes.Next)
            {
                UIEventSystem.current.nextButtonTriggerEnter();
            }
            else if(currentButtonType == ButtonTypes.Fetus)
            {
                UIEventSystem.current.fetusButtonTriggerEnter();
            }
            else if(currentButtonType == ButtonTypes.Video)
            {
                UIEventSystem.current.videoButtonTriggerEnter();
            }
            else if(currentButtonType == ButtonTypes.Quit)
            {
                UIEventSystem.current.quitButtonTriggerEnter();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        timesTouched = 0;
    }
}
