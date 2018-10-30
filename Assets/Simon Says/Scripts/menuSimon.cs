using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class menuSimon : MonoBehaviour {

    public Button normalBtn;
    public Button reverseBtn;
    public Text normalTxt;
    public Text reverseTxt;

    public static bool normal;

    void Start () {
        Screen.orientation = ScreenOrientation.Portrait;

        normalTxt.text = "Best: " + PlayerPrefs.GetInt(Strings.bestScoreNormalSimon, 0);
        reverseTxt.text = "Best: " + PlayerPrefs.GetInt(Strings.bestScoreReverseSimon, 0);

        normalBtn.onClick.AddListener(() => TaskOnClick(true));
        reverseBtn.onClick.AddListener(() => TaskOnClick(false));
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            SceneManager.LoadScene(Strings.main);
    }

    public void TaskOnClick(bool normal)
    {
        menuSimon.normal = normal;
		SceneManager.LoadScene(Strings.mainSimon);
	}
    
}
