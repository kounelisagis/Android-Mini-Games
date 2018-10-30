using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class mainUnlock : MonoBehaviour
{
    public GameObject PropellerPrefab;

    public Text bestMovesText;
    public Text currMovesText;
    public Canvas myCanvas;

    private Propeller[, ] propellerArray;
    private const int DIMENSION = 8;
    private int level = menuUnlock.level;
    private const int STEP = 95;
    private float startX = DIMENSION % 2 == 0 ? -DIMENSION / 2 + 1f / 2 : -DIMENSION / 2;
    private int movesCounter = 0;

    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;

        bestMovesText.text = "Best Moves: " + level;
        currMovesText.text = "Moves: " + movesCounter;

        int nowY = STEP;

        propellerArray = new Propeller[DIMENSION, DIMENSION];
        for(int myLine = 0; myLine < DIMENSION; myLine++)
        {
            for (int i = 0; i < DIMENSION; i++)
            {
                Propeller newPropeller = Instantiate(PropellerPrefab, myCanvas.transform).GetComponent<Propeller>();
                Vector2 myPos = new Vector2((startX + i) * STEP, nowY);
                newPropeller.setPosition(myPos);
                int first = myLine, second = i;
                newPropeller.GetComponent<Button>().onClick.AddListener(delegate { onClick(first, second); });
                propellerArray[myLine, i] = newPropeller;
            }
            nowY -= STEP;
        }

        randomToggles();
    }

    private int hashFunction(int move)  // , level
    {
        return ((move + level) * (move + level + 1) / 2 + level) % DIMENSION;
    }

    private int hashFunction2(int move)  // , level
    {
        move = ((move >> 16) ^ move) * 0x45d9f3b;
        move = ((move >> 16) ^ level) * 0x45d9f3b;
        move = (move >> 16) ^ move;
        return move % DIMENSION;
    }
    
    private void randomToggles()
    {
        for (int i = 0; i < level; i++)
        {
            int x = i;
            int move, move2;
            do
            {
                move = hashFunction(x);
                move2 = hashFunction2(x);

                x *= 2;

            } while (propellerArray[move, move2].GetComponent<Propeller>().isClicked());

            toggleThisAndAdjacent(move, move2);
        }
    }

    private void onClick(int i, int j)
    {
        movesCounter++;
        currMovesText.text = "Moves: " + movesCounter;
        toggleThisAndAdjacent(i, j);

        if (isTheEnd())
            StartCoroutine(whenWin());
    }

    IEnumerator whenWin()
    {
        foreach (Propeller box in propellerArray)
            box.GetComponent<Button>().interactable = false;

        GetComponent<AudioSource>().Play();

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(Strings.menuUnlock);
    }


    private void toggleThisAndAdjacent(int i, int j)
    {
        List<Propeller> toToggle = getThisAndAdjacent(i, j);
        propellerArray[i, j].toggleClicked();

        foreach (Propeller p in toToggle)
            p.toggleState();
    }

    private bool isTheEnd()
    {
        foreach (Propeller p in propellerArray)
            if (!p.getState())
                return false;

        return true;
    }

    private List<Propeller> getThisAndAdjacent(int i, int j)
    {
        List<Propeller> myList = new List<Propeller>();

        myList.Add(propellerArray[i, j]);
        if (i - 1 >= 0) myList.Add(propellerArray[i - 1, j]);
        if (i + 1 < DIMENSION) myList.Add(propellerArray[i + 1, j]);
        if (j - 1 >= 0) myList.Add(propellerArray[i, j - 1]);
        if (j + 1 < DIMENSION) myList.Add(propellerArray[i, j + 1]);

        return myList;
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            SceneManager.LoadScene(Strings.menuUnlock);
    }


}
