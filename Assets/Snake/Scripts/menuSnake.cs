using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class menuSnake : MonoBehaviour
{
    public Button playButton;
    public Text bestScoreTxt;


    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;

        int bestscore = PlayerPrefs.GetInt(Strings.bestScoreSnake, 0);
        bestScoreTxt.text = "" + bestscore;

        playButton.onClick.AddListener(() => TaskOnClick());
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            SceneManager.LoadScene(Strings.main);
    }

    public void TaskOnClick()
    {
        SceneManager.LoadScene(Strings.mainSnake);
    }

}
