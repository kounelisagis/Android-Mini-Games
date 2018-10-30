using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class menuUnlock : MonoBehaviour {

	public Button playButton;
	public Slider slider;
	public Text numberText;


    public static int level = 1;

    void Start () {
        Screen.orientation = ScreenOrientation.Portrait;

        slider.value = level;
        numberText.text = "Level " + level;

        playButton.onClick.AddListener(TaskOnClick);
        slider.onValueChanged.AddListener(delegate { valueChanged(); });
    }

    public void valueChanged() {
        numberText.text = "Level " + slider.value;
    }

    void Update () {
        if (Input.GetKey(KeyCode.Escape))
            SceneManager.LoadScene(Strings.main);
    }

    public void TaskOnClick() {

        level = (int)slider.value;
        SceneManager.LoadScene(Strings.mainUnlock);
	}


}
