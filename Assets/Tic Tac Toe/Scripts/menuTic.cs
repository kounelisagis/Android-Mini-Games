using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class menuTic : MonoBehaviour {

	public Button B0;
	public Button B1;
	public Button B2;
	public Button B3;
	public Button B4;

	public static int mode;


    void Start () {
        Screen.orientation = ScreenOrientation.Portrait;

        B0.onClick.AddListener( () => TaskOnClick(0));
		B1.onClick.AddListener( () => TaskOnClick(1));
		B2.onClick.AddListener( () => TaskOnClick(2));
		B3.onClick.AddListener( () => TaskOnClick(3));
		B4.onClick.AddListener( () => TaskOnClick(4));
    }

	public void TaskOnClick(int value){

		mode = value;
		SceneManager.LoadScene(Strings.mainTic);
	}

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            SceneManager.LoadScene(Strings.main);
    }


}
