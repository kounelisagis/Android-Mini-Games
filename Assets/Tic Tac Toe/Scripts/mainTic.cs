using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class mainTic : MonoBehaviour
{
    public GameObject[] GOs;
    public Text StatusText;
    public BoardTic myBoard;

    private AudioSource audioSource;
    public Button ButtonNext;

    private const TileTic.TileType firstPlayerType = TileTic.TileType.X;
    private const TileTic.TileType secondPlayerType = TileTic.TileType.O;
    public TileTic.TileType currType = TileTic.TileType.X;
    public TileTic.TileType notCurrType = TileTic.TileType.O;

    private const int SIZE = 9;
    

    // Use this for initialization
    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;

        StatusText.text = "";

        audioSource = GetComponent<AudioSource>();

        ButtonNext.onClick.AddListener(() => SceneManager.LoadScene((SceneManager.GetActiveScene().name))); // loads current scene

        for(int i=0;i<SIZE; i++)
        {
            int x = i;
            GOs[i].GetComponent<Button>().onClick.AddListener(delegate { onClick(x); });
            myBoard.getTile(i).setButtonAndText(GOs[i].GetComponent<Button>(), GOs[i].GetComponentInChildren<Text>());
        }

        randomColors();

    }

    public void onClick(int num)
    {
        myBoard.getTile(num).setType(currType);

        if (isThereWinner())
        {
            StatusText.text = "WIN!!!";
            ButtonNext.gameObject.SetActive(true);
            myBoard.changeState(false);
        }
        else if (myBoard.allFilled())
        {
            StatusText.text = "TIE";
            ButtonNext.gameObject.SetActive(true);
        }
        else
        {
            playerSwitch();

            if (menuTic.mode > 0)
            {
                StartCoroutine("makePCmove");

            }
        }

    }


    IEnumerator makePCmove()
    {
        myBoard.changeState(false);
        int move = ΑΙmove.getAImove(currType, notCurrType, myBoard);
        yield return new WaitForSeconds(0.5f);
        myBoard.getTile(move).setType(currType);

        if (isThereWinner())
        {
            StatusText.text = "WIN!!!";
            ButtonNext.gameObject.SetActive(true);
            myBoard.changeState(false);
        }
        else if (myBoard.allFilled())
        {
            StatusText.text = "TIE";
            ButtonNext.gameObject.SetActive(true);
        }
        else
        {
            playerSwitch();
            myBoard.changeState(true);
        }
    }


    private bool isThereWinner()
    {
        int[,] winnerCombinations = new int[8, 3] { { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 }, { 0, 3, 6 }, { 1, 4, 7 }, { 2, 5, 8 }, { 0, 4, 8 }, { 2, 4, 6 } };

        for (int i = 0; i < 8; ++i)
            if (!myBoard.getTile(winnerCombinations[i, 0]).isEmpty()
                && myBoard.getTile(winnerCombinations[i, 0]).getType() == myBoard.getTile(winnerCombinations[i, 1]).getType()
                && myBoard.getTile(winnerCombinations[i, 1]).getType() == myBoard.getTile(winnerCombinations[i, 2]).getType())
                return true;

        return false;
    }

    public void playerSwitch()
    {
        currType = currType == firstPlayerType ? secondPlayerType : firstPlayerType;
        notCurrType = notCurrType == firstPlayerType ? secondPlayerType : firstPlayerType;
    }

    public void playSound()
    {
        audioSource.Play();
    }

    public void changeNextButtonState(bool state)
    {
        ButtonNext.gameObject.SetActive(state);
    }

    private void randomColors()
    {
        float f1 = Random.Range(0f, 1f);

        float f2 = Random.Range(0f, 1f);
        while (Mathf.Abs(f2 - f1) < 0.2) f2 = Random.Range(0f, 1f);

        float f3 = Random.Range(0f, 1f);
        while (Mathf.Abs(f3 - f1) < 0.2 || Mathf.Abs(f3 - f2) < 0.2) f3 = Random.Range(0f, 1f);

        Camera.main.GetComponent<Camera>().backgroundColor = Random.ColorHSV(f1, f1, 1f, 1f, 1f, 1f, 1f, 1f);
        ButtonNext.GetComponent<Image>().color = Random.ColorHSV(f2, f2, 1f, 1f, 1f, 1f, 1f, 1f);

        Color color = Random.ColorHSV(f3, f3, 1f, 1f, 1f, 1f, 1f, 1f);

        myBoard.setColor(color);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            SceneManager.LoadScene(Strings.menuTic);
    }

}
