using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class main : MonoBehaviour {

    public Button ButtonSimon;
    public Button ButtonSnake;
    public Button ButtonSurrounded;
    public Button ButtonUnlock;
    public Button ButtonTic;
    public Button ButtonTower;


    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;

        ButtonSimon.onClick.AddListener(() => SceneManager.LoadScene(Strings.menuSimon));
        ButtonSnake.onClick.AddListener(() => SceneManager.LoadScene(Strings.menuSnake));
        ButtonSurrounded.onClick.AddListener(() => SceneManager.LoadScene(Strings.menuSurrounded));
        ButtonUnlock.onClick.AddListener(() => SceneManager.LoadScene(Strings.menuUnlock));
        ButtonTic.onClick.AddListener(() => SceneManager.LoadScene(Strings.menuTic));
        ButtonTower.onClick.AddListener(() => SceneManager.LoadScene(Strings.menuTower));
    }

}
