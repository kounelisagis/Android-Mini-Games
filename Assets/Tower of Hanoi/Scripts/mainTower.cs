using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using Random = UnityEngine.Random;


public class mainTower : MonoBehaviour {

    public GameObject BarPrefab;
    public Transform ButtonsLayer;
    public Canvas myCanvas;

    private Camera cam;

    public GameObject winPanel;
    private AudioSource audiosrc;
    public AudioClip win;

    public Text currMoves;
    public Text minMoves;


    private Stack<Bar> A, B, C;

    private const int STEP_Y = 40;
    private const int LEFT_X = -267, MIDDLE_X = 0, RIGHT_X = 267;
    private int start_Y;
    
    private int moves = 0;

    private int barsNum;
    private Color barColor;


    void Start()
    {
        Screen.orientation = ScreenOrientation.Landscape;

        barsNum = menuTower.bars;
        start_Y = -(int)myCanvas.GetComponent<RectTransform>().rect.height / 2 + 20;

        cam = Camera.main;
        audiosrc = GetComponent<AudioSource>();
        randomColors();


        minMoves.text = "Minimum Moves: " + (Mathf.Pow(2, barsNum) - 1);

        A = new Stack<Bar>();
        B = new Stack<Bar>();
        C = new Stack<Bar>();

        for(int i=0;i<barsNum;i++)
        {
            Bar newBar = Instantiate(BarPrefab, ButtonsLayer.transform).GetComponent<Bar>();
            newBar.setPosition(new Vector2(-268, start_Y + i * STEP_Y));
            newBar.setSize(new Vector2(10 - i*0.8f, 2.5f));
            newBar.setColor(barColor);
            newBar.setStack(A);
            A.Push(newBar);
        }

    }


    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            SceneManager.LoadScene(Strings.menuTower);
    }


    public void clickIsUp(Bar myBar, Vector2 prevPos)
    {
        float min = Mathf.Abs(myBar.transform.localPosition.x - LEFT_X);
        float newX = LEFT_X;
        Stack<Bar> newStack = A;

        if (Mathf.Abs(myBar.transform.localPosition.x - MIDDLE_X) < min)
        {
            min = Mathf.Abs(myBar.transform.localPosition.x - MIDDLE_X);
            newStack = B;
            newX = MIDDLE_X;
        }
        if (Mathf.Abs(myBar.transform.localPosition.x - RIGHT_X) < min)
        {
            newStack = C;
            newX = RIGHT_X;
        }


        if(myBar.getStack() == newStack)
            myBar.setPosition(prevPos);
        else if (newStack.Count != 0 && myBar.getSizeX() > newStack.Peek().getSizeX())
        {
            myBar.setPosition(prevPos);
            audiosrc.Play();
        }
        else
        {
            myBar.getStack().Pop();
            myBar.setStack(newStack);
            newStack.Push(myBar);
            myBar.setPosition(new Vector2(newX, start_Y + STEP_Y * (newStack.Count - 1)));
            moves++;
            showCurrMoves();

            if (C.Count == barsNum)
                whenWin();
        }

    }


    public void loadLevels()
    {
        SceneManager.LoadScene(Strings.menuTower);
    }

    public void restartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    private void whenWin()
    {
        audiosrc.PlayOneShot(win);
        winPanel.SetActive(true);
        winPanel.GetComponent<Image>().color = winColor;
    }


    public void showCurrMoves()
    {
        currMoves.text = "Moves: " + moves;
    }


    public bool isTop(Bar myBar)
    {
        return myBar.getStack().Peek() == myBar;
    }


    public void randomColors()
    {
        float f1 = Random.Range(0f, 1f);
        float f2 = Random.Range(0f, 1f);
        while (Mathf.Abs(f2 - f1) < 0.15)
            f2 = Random.Range(0f, 1f);

        float f3 = Random.Range(0f, 1f);
        while (Mathf.Abs(f3 - f1) < 0.15 || Mathf.Abs(f3 - f2) < 0.15) f3 = Random.Range(0f, 1f);

        cam.GetComponent<Camera>().backgroundColor = Color.HSVToRGB(f1, 1f, 1f);
        winColor = Color.HSVToRGB(f2, 1f, 1f);
        barColor = Color.HSVToRGB(f3, 1f, 1f);
    }

    private Color winColor;

}
