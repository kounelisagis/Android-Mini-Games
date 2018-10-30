using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour {

    private Image myImage;
    private Button myButton;
    private int i, j;

    void Start()
    {
        myImage = GetComponent<Image>();
        myButton = GetComponent<Button>();
    }

    public void setCoordinates(int i, int j)
    {
        this.i = i;
        this.j = j;
    }

    public int[] getCoordinates()
    {
        return new int[] { i, j };
    }

    public void disable()
    {
        myImage.enabled = false;
    }

    public bool isDisabled()
    {
        return !myImage.enabled;
    }

    public void activateInteract(bool a)
    {
        myButton.interactable = a;
    }


    public enum State { visited, unvisited };
    private State myState = State.unvisited;

    public void setState(State newState)
    {
        myState = newState;
    }

    public State getState()
    {
        return myState;
    }

}
