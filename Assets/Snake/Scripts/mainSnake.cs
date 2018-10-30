using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class mainSnake : MonoBehaviour {

    public Button upBtn;
    public Button downBtn;
    public Button leftBtn;
    public Button rightBtn;

    private Camera cam;
    public Image prototypeBlock;

    public Image bg;
    public Transform panel;
    public Image food;

    public AudioSource audiosrc;
    public AudioSource apples;
    public AudioClip lose;
    public AudioClip upClip;
    public AudioClip downClip;
    public AudioClip rightClip;
    public AudioClip leftClip;

    public Text scoreTxt;
    private int score = 0;
    private int bestScore;


    private const int startSize = 4;
    private const int boardSize = 15;
    private const int blockSize = 10;
    private const float refreshTime = .1f;

    private enum Move { LEFT, RIGHT, UP, DOWN, EMPTY };
    private Move prevMove;
    private Move next1 = Move.EMPTY, next2 = Move.EMPTY;

    private List<Image> parts;
    private Vector3 prevPositionOfLast;


    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;

        cam = Camera.main;
        audiosrc = GetComponent<AudioSource>();
        parts = new List<Image>();

        prevMove = Move.LEFT;

        for (int i = 0; i < startSize; i++)
        {
            parts.Add(Instantiate(prototypeBlock, panel));
            parts[parts.Count - 1].transform.localPosition = new Vector3(i * blockSize, 0, 0);
        }

        upBtn.onClick.AddListener(delegate {
            buttonClicked(Move.UP);
        });
        downBtn.onClick.AddListener(delegate {
            buttonClicked(Move.DOWN);
        });
        leftBtn.onClick.AddListener(delegate {
            buttonClicked(Move.LEFT);
        });
        rightBtn.onClick.AddListener(delegate {
            buttonClicked(Move.RIGHT);
        });


        randomColors();

        scaleSnake();

        bestScore = PlayerPrefs.GetInt(Strings.bestScoreSnake, 0);
        updateScoreText();

        InvokeRepeating("makeMove", 0f, refreshTime);
    }

    void updateScoreText()
    {
        if (bestScore < score)
            bestScore = score;

        scoreTxt.text = score + "\n/ " + bestScore;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            saveBestScore();
            SceneManager.LoadScene(Strings.menuSnake);
        }
    }


    bool isMoveGood(Move before, Move after)
    {
        return !(before == Move.UP && after == Move.DOWN ||
                before == Move.DOWN && after == Move.UP ||
                before == Move.RIGHT && after == Move.LEFT ||
                before == Move.LEFT && after == Move.RIGHT);
    }

    void buttonClicked(Move newMove)
    {
        if(next1==Move.EMPTY && isMoveGood(prevMove, newMove))
        {
            next1 = newMove;
        }
        else if(next1 != Move.EMPTY && next2 == Move.EMPTY && isMoveGood(next1, newMove))
        {
            next2 = newMove;
        }
        else if(next1 != Move.EMPTY && next2 != Move.EMPTY && isMoveGood(prevMove, next2) && isMoveGood(next2, newMove))
        {
            next1 = next2;
            next2 = newMove;
        }
    }


    void makeMove()
    {
        Move myMove;
        if (next1 != Move.EMPTY)
        {
            myMove = next1;
            next1 = next2;
            next2 = Move.EMPTY;
        }
        else
            myMove = prevMove;


        if (myMove != prevMove)
            playAudio(myMove);

        prevPositionOfLast = parts[parts.Count - 1].transform.localPosition;

        for (int i = parts.Count - 1; i > 0; i--)
            parts[i].transform.localPosition = parts[i - 1].transform.localPosition;

        parts[0].transform.localPosition += getFirstPosition(myMove);


        //OUT OF BOARD---
        int up = boardSize / 2;
        int right = boardSize / 2;
        int down = -boardSize / 2;
        int left = -boardSize / 2;


        if (parts[0].transform.localPosition.x == (right + 1) * blockSize)          // RIGHT
            parts[0].transform.localPosition = new Vector3(left * blockSize, parts[0].transform.localPosition.y, 0);
        else if (parts[0].transform.localPosition.x == (left - 1) * blockSize)      // LEFT
            parts[0].transform.localPosition = new Vector3(right * blockSize, parts[0].transform.localPosition.y, 0);
        else if (parts[0].transform.localPosition.y == (up + 1) * blockSize)        // UP
            parts[0].transform.localPosition = new Vector3(parts[0].transform.localPosition.x, down * blockSize, 0);
        else if (parts[0].transform.localPosition.y == (down - 1) * blockSize)      // DOWN
            parts[0].transform.localPosition = new Vector3(parts[0].transform.localPosition.x, up * blockSize, 0);
        //---------------


        if(isTheEnd())
        {
            StartCoroutine(restart());
            CancelInvoke();
        }


        checkForFood();

        prevMove = myMove;
    }

    void playAudio(Move myMove)
    {
        if (myMove == Move.DOWN)
            audiosrc.clip = downClip;
        else if (myMove == Move.UP)
            audiosrc.clip = upClip;
        else if (myMove == Move.LEFT)
            audiosrc.clip = leftClip;
        else //myMove == Move.RIGHT
            audiosrc.clip = rightClip;

        audiosrc.Play();
    }

    void checkForFood()
    {
        if (parts[0].transform.localPosition == food.transform.localPosition)
        {
            apples.Play();

            parts.Add(Instantiate(prototypeBlock, panel));
            parts[parts.Count - 1].transform.position = prevPositionOfLast;

            scaleSnake();


            do
            {
                food.transform.localPosition = new Vector3(Random.Range(-boardSize / 2, boardSize / 2) * blockSize, Random.Range(-boardSize / 2, boardSize / 2) * blockSize, 0);
            } while (isThereSnake(food.transform.localPosition, true));


            score++;
            updateScoreText();
        }
    }

    bool isThereSnake(Vector3 myPosition, bool head)
    {
        if (head)   // take into account the head
        {
            for (int j = 0; j < parts.Count; j++)
                if (myPosition == parts[j].transform.localPosition)
                    return true;
        }
        else
        {
            for (int i = 1; i < parts.Count; i++)
                if (myPosition == parts[i].transform.localPosition)
                    return true;
        }

        return false;
    }

    bool isTheEnd()
    {
        return isThereSnake(parts[0].transform.localPosition, false);
    }

    void saveBestScore()
    {
         PlayerPrefs.SetInt(Strings.bestScoreSnake, bestScore);
    }

    IEnumerator restart()
    {
        audiosrc.PlayOneShot(lose);
        Handheld.Vibrate();

        saveBestScore();

        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(Strings.mainSnake);
    }

    Vector3 getFirstPosition(Move myMove)
    {
        if (myMove == Move.UP)
            return new Vector3(0, blockSize, 0);
        else if (myMove == Move.DOWN)
            return new Vector3(0, -blockSize, 0);
        else if (myMove == Move.RIGHT)
            return new Vector3(blockSize, 0, 0);
        else//Move == Move.LEFT
            return new Vector3(-blockSize, 0, 0);
    }

    void scaleSnake()
    {
        float step = 5.0f / (parts.Count - 1);

        parts[parts.Count - 1].rectTransform.sizeDelta = new Vector2(4, 4);

        for(int i=parts.Count - 2; i>=0; i--)
            parts[i].rectTransform.sizeDelta = parts[i + 1].rectTransform.sizeDelta + new Vector2(step, step);

        float colorStep = 1.0f / (parts.Count - 1);

        for (int i = 1; i < parts.Count - 1; i++)
        {
            float h, s, v;
            Color.RGBToHSV(parts[i - 1].color, out h, out s, out v);
            parts[i].color = Color.HSVToRGB(h, s - colorStep, v);
        }

        parts[parts.Count - 1].color = Color.white;

    }

    void randomColors()
    {
        float f1 = Random.Range(0f, 0.9f);
        float f2 = Random.Range(0f, 0.9f);
        while (Mathf.Abs(f2 - f1) < 0.15) f2 = Random.Range(0f, 0.9f);

        float f3 = Random.Range(0f, 1f);
        while (Mathf.Abs(f3 - f1) < 0.15 || Mathf.Abs(f3 - f2) < 0.15) f3 = Random.Range(0f, 0.9f);

        cam.GetComponent<Camera>().backgroundColor = Color.HSVToRGB(f1, 1f, 1f);
        parts[0].color = Color.HSVToRGB(f1, 1f, 1f);
        bg.color = Color.HSVToRGB(f2, 1f, 0.4f);
        food.color = Color.HSVToRGB(f3, 1f, 1f);
    }

}
