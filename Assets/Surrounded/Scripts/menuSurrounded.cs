using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class menuSurrounded : MonoBehaviour
{
    public Button ButtonAI;
    public Button ButtonNoAI;
    public Text BestScoresText;

    public static bool AI;

    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;

        ButtonAI.onClick.AddListener(delegate { TaskOnClick(true); });
        ButtonNoAI.onClick.AddListener(delegate { TaskOnClick(false); });

        int AI_best = PlayerPrefs.GetInt(Strings.bestScoreAISurrounded, 0);
        int NO_AI_best = PlayerPrefs.GetInt(Strings.bestScoreNoAISurrounded, 0);

        BestScoresText.text = "One Player Best: " + AI_best + "\nTwo Players Best: " + NO_AI_best;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            SceneManager.LoadScene(Strings.main);
    }

    public void TaskOnClick(bool AI)
    {
        menuSurrounded.AI = AI;
        SceneManager.LoadScene(Strings.mainSurrounded);
    }

}
