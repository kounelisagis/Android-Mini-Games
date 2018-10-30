using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class mainSimon : MonoBehaviour
{
    public Button[] myButtons;
    public Image[] dots;
    public Image circle;

    public AudioClip[] mySounds;
    public AudioClip wrong;
    private AudioSource audiosrc;

    public Text scoreTxt;
    public Text bestScoreTxt;

    private List<Button> myList;

    private int currIndex = 0;
    private int highScore = 0;
    private int lives = 3;


    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;

        audiosrc = GetComponent<AudioSource>();

        foreach (Button a in myButtons)
            a.onClick.AddListener(() => click(a));

        myList = new List<Button>();
        myList.Add(newRand());

        if(menuSimon.normal)
            highScore = PlayerPrefs.GetInt(Strings.bestScoreNormalSimon, 0);
        else
            highScore = PlayerPrefs.GetInt(Strings.bestScoreReverseSimon, 0);


        scoreTxt.text = "" + currIndex;
        bestScoreTxt.text = "" + highScore;

        StartCoroutine(PlayButtons());

    }

    private void click(Button myClickButton)
    {
        if (myClickButton == myList[currIndex])
        {
            audiosrc.clip = mySounds[System.Int32.Parse(myClickButton.tag)];
            audiosrc.Play();

            if (menuSimon.normal)
            {
                currIndex++;
                if (currIndex == myList.Count)
                {
                    currIndex = 0;
                    scoreTxt.text = "" + myList.Count;

                    if(myList.Count > highScore)
                    {
                        highScore = myList.Count;
                        PlayerPrefs.SetInt(Strings.bestScoreNormalSimon, highScore);
                        bestScoreTxt.text = "" + highScore;
                    }

                    myList.Add(newRand());


                    StartCoroutine(PlayButtons());
                }
            }

            else
            {
                currIndex--;
                if (currIndex == -1)
                {
                    currIndex = myList.Count;

                    scoreTxt.text = "" + currIndex;

                    if (myList.Count > highScore)
                    {
                        highScore = myList.Count;
                        PlayerPrefs.SetInt(Strings.bestScoreReverseSimon, highScore);
                        bestScoreTxt.text = "" + highScore;
                    }

                    myList.Add(newRand());

                    StartCoroutine(PlayButtons());
                }
            }
            
        }

        else
        {
            lives--;
            dots[lives].enabled = false;

            if(menuSimon.normal)
                currIndex = 0;
            else
                currIndex = myList.Count - 1;

            audiosrc.clip = wrong;
            audiosrc.Play();

            if (lives == 0)
            {
                changeButtonsState(false);
                StartCoroutine(LoadAgain());
            }
            else
                StartCoroutine(PlayButtons());
        }

    }
    
    private IEnumerator PlayButtons()
    {
        changeButtonsState(false);
        circle.color = Color.HSVToRGB(0, 0, 0.3f);

        yield return new WaitForSeconds(1);


        foreach (Button a in myList)
        {
            yield return new WaitForSeconds(0.2f);

            var colors = a.colors;
            var temp = colors;
            colors.disabledColor = colors.pressedColor;
            a.colors = colors;

            audiosrc.clip = mySounds[System.Int32.Parse(a.tag)];
            audiosrc.Play();

            yield return new WaitForSeconds(0.3f);

            a.colors = temp;

        }

        yield return new WaitForSeconds(0.2f);

        changeButtonsState(true);
        circle.color = Color.HSVToRGB(0, 0, 0.6f);

    }

    private IEnumerator LoadAgain()
    {
        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene(Strings.mainSimon);
    }

    private void changeButtonsState(bool state)
    {
        foreach (Button a in myButtons)
            a.interactable = state;
    }

    private Button newRand()
    {
        int x = Random.Range(0, 99999) % 4;
        return myButtons[x];
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            SceneManager.LoadScene(Strings.menuSimon);
    }

}