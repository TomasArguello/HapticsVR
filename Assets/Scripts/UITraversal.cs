using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITraversal : MonoBehaviour
{
    GameObject previousPanel; //The last panel selected, will become invisible
    
    GameObject currentPanel; //The current UI panel that is selected

    GameObject nextPanel; //The next UI panel to be selected

    GameObject[] listOfPanels; //An array containing the panels to be viewed

    int currentPanelNum; //Is used to iterate through the array of panels

    GameObject nextButton; //The buttons
    GameObject fetusButton;
    GameObject videoButton;
    GameObject quitButton;

    GameObject fetus;

    GameObject videoScreen;
    UnityEngine.Video.VideoPlayer videoPlayer;
    bool isVideoPlaying;

    GameObject fadeOut;
    private void Awake()
    {
        /////Initializes the list of panels in order to be able to scroll through them/////
        listOfPanels = new GameObject[gameObject.transform.childCount]; //Initializes the array of UI panels

        for(int i=0; i < 13; i++) //gameObject.transform.childCount will count ALL children, so we are just counting to 13 to get the first 13 kids and not the buttons
        {
            currentPanel = gameObject.transform.GetChild(i).gameObject;
            listOfPanels[i] = gameObject.transform.GetChild(i).gameObject; //Loops through the children within the UI canvas and adds the panels to the array listOfPanels
            listOfPanels[i].SetActive(false);
        }

        currentPanel = listOfPanels[0]; //Assigns the first panel to the currentPanel at the beginning of the scene
        currentPanel.SetActive(true);
        nextPanel = listOfPanels[1]; //Assigns the second panel to the nextPanel at the beginning of the scene

        currentPanelNum = 0; //Assigns the iterator to zero, which will be used in the nextButtonActivated() function
        /////End of initializiation of the list of panels/////

        nextButton = gameObject.transform.Find("next_button").gameObject;
        fetusButton = gameObject.transform.Find("fetus_button").gameObject;
        videoButton = gameObject.transform.Find("video_button").gameObject;
        quitButton = gameObject.transform.Find("quit_button").gameObject;

        fetus = GameObject.Find("patient");

        /////Initializes the video screen that will show the video of the pregnant lady/////
        videoScreen = GameObject.Find("VideoScreen"); //videoScreen is the physical GameObject that the video will be played on 
        videoPlayer = videoScreen.gameObject.GetComponent<UnityEngine.Video.VideoPlayer>(); //videoPlayer is the actual Video Player component of the screen that controls the video
        //Debug.Log("The videoPlayer's videoScreen is called " + videoPlayer.gameObject.name);
        if(videoPlayer  == null)
        {
            videoPlayer = videoScreen.gameObject.AddComponent<UnityEngine.Video.VideoPlayer>(); //If for some reason the Video Player isn't attached to the screen, a new Video Player will be added instead
            videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.RenderTexture;

        }
        videoScreen.gameObject.SetActive(false); //Turns off the video screen so it doesn't play at the very beginning
        isVideoPlaying = false;
        /////End of video screen configuration and initialzation/////
        /////Initialize the fadeout for when the user quits/////
        fadeOut = GameObject.Find("QuitFadeOut");
        fadeOut.GetComponent<Image>().color = new Color(fadeOut.GetComponent<Image>().color.r, fadeOut.GetComponent<Image>().color.g, fadeOut.GetComponent<Image>().color.b, 0f);
        Debug.Log("The color of the fadeOut Image is " + fadeOut.GetComponent<Image>().color);
    }

    // Start is called before the first frame update
    void Start()
    {
        UIEventSystem.current.onNextButtonTriggerEnter += nextButtonActivated; //Adds the nextButtonActivated() function to the UIEventSystem Action onNextButtonTriggerEnter
        UIEventSystem.current.onFetusButtonTriggerEnter += fetusButtonActivated; //Adds the fetusButtonActivated() function to the UIEventSystem Action onFetusButtonTriggerEnter
        UIEventSystem.current.onVideoButtonTriggerEnter += videoButtonActivated; //Adds the videoButtonActivated() function to the UIEventSystem Action onVideoButtonTriggerEnter
        UIEventSystem.current.onQuitButtonTriggerEnter += quitButtonActivated;
    }

    void nextButtonActivated()
    {
        /////If the videoScreen is currently pulled up, it will stop the video from playing, deactivate the videoScreen and resume from the last UI panel the user was on/////
        if (isVideoPlaying)
        {
            videoPlayer.Stop();
            isVideoPlaying = false;
            videoScreen.SetActive(false);
            currentPanel.SetActive(true);
            return;
        }
        /////If the videoScreen isn't pulled up, then the nextButton works as normal/////
        previousPanel = currentPanel; 
        previousPanel.SetActive(false);

        if (currentPanelNum >= 12)
        {
            currentPanelNum = 0;
            currentPanel = listOfPanels[currentPanelNum];
            nextPanel = listOfPanels[currentPanelNum + 1];
            currentPanel.SetActive(true);
            return;
        }

        else
        {
            currentPanelNum += 1;
            currentPanel = listOfPanels[currentPanelNum];
            currentPanel.SetActive(true);
            nextPanel = listOfPanels[currentPanelNum + 1];
            return;
        }

    }

    void fetusButtonActivated()
    {
        /////Pretty basic, turns off the GameObject labeled "patient"/////
        if (fetus.activeSelf)
        {
            fetus.SetActive(false);
        }
        else
        {
            fetus.SetActive(true);
        }
        /////End of fetusButtonActivated/////
    }

    void videoButtonActivated()
    {
        /////Controls how the videoPlayer works. If the videoScreen was turned off and the videoButton is pressed, then the videoScreen appears, (The previous panel disappears) and plays the video./////
        /////IMPORTANT! If for some reason the videoScreen gets messed up, you will need to make a basic 3D GameObject plane and attach a Video Player component to it./////
        /////You will BOTH the Render Texture labeled VideoTexture AND the material labeled VideoTexture. They're both in Assets>Video/////
        /////Attach the video clip to the VideoClip slot in the Video Player, change the RenderMode to RenderTexture, and attach the Render Texture VideoTexture to the corresponding slot in Video Player/////
        /////Finally, attach the material VideoTexture to the videoScreen GameObject. Make sure the shader of the material is Unlit/Texture!/////
        if (videoScreen.activeSelf)
        {
            if (videoPlayer.isPaused)
            {
                videoPlayer.Play();
            }
            else
            {
                videoPlayer.Pause();
            }
        }
        else
        {
            currentPanel.SetActive(false);
            videoScreen.SetActive(true);
            videoPlayer.Play();
            isVideoPlaying = true;
        }
    }

    void quitButtonActivated()
    {
        if (isVideoPlaying)
        {
            videoPlayer.Stop();
        }
        StartCoroutine(QuitFadeOut());
    }

    public IEnumerator QuitFadeOut()
    {
        Color fadeOutColor = fadeOut.GetComponent<Image>().color;
        float fadeAmount;
        while(fadeOut.GetComponent<Image>().color.a < 1)
        {
            fadeAmount = fadeOutColor.a + (5f * Time.deltaTime);
            fadeOutColor = new Color(fadeOutColor.r, fadeOutColor.g, fadeOutColor.b, fadeAmount);
            fadeOut.GetComponent<Image>().color = fadeOutColor;
            //Debug.Log("The color of the fadeOut is currently " + fadeOut.GetComponent<Image>().color);
            yield return null;
        }
#if UNITY_EDITOR
        Debug.Log("The coroutine started, hopefully...");
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Debug.Log("The Coroutine QuitFadeOut should have started by now...");
        Application.Quit();
    }
}
