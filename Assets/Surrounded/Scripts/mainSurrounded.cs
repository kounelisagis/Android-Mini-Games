using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class mainSurrounded : MonoBehaviour {

    public Checker White;
    public Checker Black;
    public Board myBoard;
    public Text whiteText;
    public Text blackText;

    private AudioSource audiosrc;
    public AudioClip winClip;

    public GameObject winPanel;

    private enum Phase { Moving, Deleting };

    private int round = 1;

    private Phase myPhase = Phase.Moving;
    private Checker currChecker;
    private bool AI;
    private const string EMPTY = "";

    void Start ()
    {
        Screen.orientation = ScreenOrientation.Portrait;

        audiosrc = GetComponent<AudioSource>();

        randomColors();

        AI = menuSurrounded.AI;

        currChecker = White;

        White.placeChecker(6, 3);
        Black.placeChecker(0, 3);

        refreshColorAndText();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            SceneManager.LoadScene(Strings.menuSurrounded);
    }


    public bool playerMove(int i, int j)
    {
        if (myPhase == Phase.Moving)
        {
            int[] checkerCoordinates = currChecker.getCoordinates();
            if (Mathf.Abs(checkerCoordinates[0] - i) <= 1 && Mathf.Abs(checkerCoordinates[1] - j) <= 1 && !isThereChecker(i, j))
            {
                currChecker.placeChecker(i, j);
                myPhase = Phase.Deleting;
                refreshColorAndText();
            }
            else
                audiosrc.Play();

        }
        else // Delete Phase
        {
            if (!isThereChecker(i, j))
            {
                myBoard.deleteTile(i, j);

                if (areAdjacentAvailableForBoth())
                {
                    if (currChecker == Black)
                        round++;
                    myPhase = Phase.Moving;
                    currChecker = getOpponent();
                    refreshColorAndText();
                    return true;
                }
                else
                    StartCoroutine(showWinPanel());
            }
        }

        return false;

    }

    private IEnumerator movePC()
    {
        myBoard.activateClicks(false);

        yield return new WaitForSeconds(1f);
        //moving
        Tile moveTile = getMoveTile(currChecker);
        int[] moveCoo = moveTile.getCoordinates();
        currChecker.placeChecker(moveCoo[0], moveCoo[1]);
        myPhase = Phase.Deleting;
        refreshColorAndText();

        yield return new WaitForSeconds(1f);
        //deleting
        Tile deleteTile = getMoveTile(getOpponent());
        int[] deleteCoo = deleteTile.getCoordinates();
        myBoard.deleteTile(deleteCoo[0], deleteCoo[1]);

        if (areAdjacentAvailableForBoth())
        {
            if (currChecker == Black)
                round++;
            myPhase = Phase.Moving;
            currChecker = getOpponent();
            refreshColorAndText();
        }
        else
            StartCoroutine(showWinPanel());

        yield return new WaitForSeconds(.3f);
        myBoard.activateClicks(true);
    }


    private Tile getMoveTile(Checker myChecker)
    {
        int[] coo = myChecker.getCoordinates();
        Tile myTile = myBoard.getTile(coo[0], coo[1]);

        List<Tile> myList = myBoard.getAdjacentTiles(myTile);
        int max = 0;
        Tile tileWithMax = null;

        foreach (Tile t in myList)
        {
            if (myBoard.getAdjacentTiles(t).Count >= max)
            {
                max = myBoard.getAdjacentTiles(t).Count;
                tileWithMax = t;
            }

        }

        return tileWithMax;
    }
    

    public void buttonClick(int i, int j)
    {
        if(playerMove(i, j) && AI)
            StartCoroutine(movePC());
    }

    public Checker getOpponent()
    {
        return currChecker == White ? Black : White;
    }

    public bool areAdjacentAvailableForBoth()
    {
        return areAdjacentAvailable(White) && areAdjacentAvailable(Black);
    }

    private bool areAdjacentAvailable(Checker ch)
    {
        int[] coo = ch.getCoordinates();
        Tile t = myBoard.getTile(coo[0], coo[1]);

        return myBoard.areThereAdjacent(t);
    }

    public bool isThereChecker(int i, int j)
    {
        int[] white = White.getCoordinates();
        int[] black = Black.getCoordinates();

        return (i == black[0] && j == black[1]) || (i == white[0] && j == white[1]);
    }

    public void refreshColorAndText()
    {
        if(currChecker == White)
        {
            whiteText.text = getMessage();
            blackText.text = EMPTY;
        }
        else if(!AI)
        {
            blackText.text = getMessage();
            whiteText.text = EMPTY;
        }
        else // currChecker == Black && AI
        {
            blackText.text = EMPTY;
            whiteText.text = EMPTY;
        }
    }

    public string getMessage()
    {
        string command;
        if (myPhase == Phase.Moving)
            command = "Make your move\nRound: " + round;
        else
            command = "Delete a tile\nRound: " + round;

        return command;
    }

    private void randomColors()
    {
        float random1 = Random.Range(0f, 0.9f);
        float random2 = Random.Range(0f, 0.9f);
        while (Mathf.Abs(random1 - random2) < 0.15) random2 = Random.Range(0f, 0.9f);

        Camera.main.backgroundColor = Color.HSVToRGB(random1, 1f, 1f);
        myBoard.setTileColor(Color.HSVToRGB(random2, 1f, 1f));
    }

    private IEnumerator showWinPanel()
    {
        string saveString;
        if (AI)
            saveString = Strings.bestScoreAISurrounded;
        else
            saveString = Strings.bestScoreNoAISurrounded;

        int roundMin = PlayerPrefs.GetInt(saveString, 0);
        if (roundMin == 0 || round < roundMin) PlayerPrefs.SetInt(saveString, round);


        audiosrc.PlayOneShot(winClip);
        myBoard.activateClicks(false);
        winPanel.SetActive(true);
        Checker winner = areAdjacentAvailable(White) ? White : Black;
        winPanel.GetComponent<Image>().color = winner == White ? Color.white : Color.black;
        Text txt = winPanel.GetComponentInChildren<Text>();
        txt.color = winner == Black ? Color.white : Color.black;
        txt.text = winner == Black ? "BLACK WON" : "WHITE WON";
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
