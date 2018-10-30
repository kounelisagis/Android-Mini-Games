using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour {

    public mainSurrounded myManager;
    public GameObject ButtonPrefab;

    private const int BOARD_SIZE = 7;
    private const int STEP = 50;
    private const int startX = -150;
    private const int startY = 150;

    private Tile[,] board;


    void Start () {

        board = new Tile[BOARD_SIZE, BOARD_SIZE];

        int posY = startY;

        for(int i=0;i<BOARD_SIZE;i++)
        {
            int posX = startX;
            for (int j = 0; j < BOARD_SIZE; j++)
            {
                board[i, j] = Instantiate(ButtonPrefab, this.transform).GetComponent<Tile>();
                board[i, j].transform.localPosition = new Vector2(posX, posY);
                board[i, j].setCoordinates(i, j);
                int tempI = i;
                int tempJ = j;
                
                board[i, j].GetComponent<Button>().onClick.AddListener(delegate { myManager.buttonClick(tempI, tempJ); });

                posX += STEP;
            }
            posY -= STEP;
        }
	}

    public Tile getTile(int i, int j)
    {
        return board[i, j];
    }

    public void setTileColor(Color myColor)
    {
        ButtonPrefab.GetComponent<Image>().color = myColor;
    }

    public Vector2 getPosition(int i, int j)
    {
        return new Vector2(startX + j * STEP, startY - i * STEP); ;
    }

    public void deleteTile(int i, int j)
    {
        board[i, j].disable();
    }

    public List<Tile> getAdjacentTiles(Tile t)
    {
        int[] coo = t.getCoordinates();
        int i = coo[0], j = coo[1];

        List<Tile> myList = new List<Tile>();
        //NORMAL
        if (isActiveAndEmptyTile(i + 1, j)) myList.Add(board[i + 1, j]);
        if (isActiveAndEmptyTile(i - 1, j)) myList.Add(board[i - 1, j]);
        if (isActiveAndEmptyTile(i, j + 1)) myList.Add(board[i, j + 1]);
        if (isActiveAndEmptyTile(i, j - 1)) myList.Add(board[i, j - 1]);

        //DIAG
        if (isActiveAndEmptyTile(i + 1, j + 1)) myList.Add(board[i + 1, j + 1]);
        if (isActiveAndEmptyTile(i - 1, j - 1)) myList.Add(board[i - 1, j - 1]);
        if (isActiveAndEmptyTile(i + 1, j - 1)) myList.Add(board[i + 1, j - 1]);
        if (isActiveAndEmptyTile(i - 1, j + 1)) myList.Add(board[i - 1, j + 1]);

        return myList;
    }

    public bool isActiveAndEmptyTile(int i, int j)
    {
        return i >= 0 && i < BOARD_SIZE && j >= 0 && j < BOARD_SIZE && !board[i, j].isDisabled() && !myManager.isThereChecker(i, j);
    }

    public bool areThereAdjacent(Tile t)
    {
        return getAdjacentTiles(t).Count > 0;
    }

    public void activateClicks(bool a)
    {
        for (int i = 0; i < BOARD_SIZE; i++)
            for (int j = 0; j < BOARD_SIZE; j++)
                board[i, j].activateInteract(a);
    }

}
