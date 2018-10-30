using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checker : MonoBehaviour {

    public Board myBoard;
    private int i, j;

    public void placeChecker(int i, int j)
    {
        this.i = i;
        this.j = j;
        Vector2 newPos = myBoard.getPosition(i, j);
        this.transform.localPosition = newPos;
    }

    public int[] getCoordinates()
    {
        return new int[] {i, j};
    }
    
}
