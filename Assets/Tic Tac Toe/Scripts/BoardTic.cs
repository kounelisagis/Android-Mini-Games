using UnityEngine;

public class BoardTic : MonoBehaviour {

    public TileTic[] board;


    public TileTic getTile(int i)
    {
        return board[i];
    }

    public char getChar(int i) //used for minimax
    {
        if (board[i].getType() == TileTic.TileType.X)
            return 'X';
        else if (board[i].getType() == TileTic.TileType.O)
            return 'O';

        return ' ';
    }

    public void setColor(Color myColor)
    {
        foreach (TileTic t in board)
            t.setColor(myColor);
    }

    public bool allFilled()
    {
        foreach (TileTic t in board)
            if (t.isEmpty())
                return false;

        return true;
    }

    public void changeState(bool state)
    {
        foreach(TileTic t in board)
            if (t.isEmpty())
                t.changeState(state);
    }

}
