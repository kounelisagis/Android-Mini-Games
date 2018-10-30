using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class menuTower : MonoBehaviour {

	public Button playButton;
	public Slider slider;
	public Text numberText;


    public static int bars = 2;

    void Start () {
        Screen.orientation = ScreenOrientation.Landscape;

        slider.value = bars;
        numberText.text = "Number of bars: " + bars;

        playButton.onClick.AddListener(TaskOnClick);
        slider.onValueChanged.AddListener(delegate { valueChanged(); });
    }

    public void valueChanged() {
        numberText.text = "Number of bars: " + slider.value;
    }

    void Update () {
        if (Input.GetKey(KeyCode.Escape))
            SceneManager.LoadScene(Strings.main);
    }

    public void TaskOnClick() {

        bars = (int)slider.value;
        SceneManager.LoadScene(Strings.mainTower);
	}


}
