using UnityEngine;
using UnityEngine.UI;


public class TileTic: MonoBehaviour
{
    public enum TileType { X, O, EMPTY };

    private Button btn;
    private Text txt;
    private TileType myType = TileType.EMPTY;

    
    public void setButtonAndText(Button btn, Text txt)
    {
        this.btn = btn;
        this.txt = txt;
    }


    public void changeState(bool state)
    {
        btn.interactable = state;
    }

    public TileType getType()
    {
        return myType;
    }

    public void setType(TileType type) // and change color
    {
        myType = type;
        if (myType == TileType.X)
        {
            txt.text = "X";
            txt.color = Color.blue;
        }
        else if (myType == TileType.O)
        {
            txt.text = "O";
            txt.color = Color.red;
        }

        changeState(false);
    }

    public bool isEmpty()
    {
        return myType == TileType.EMPTY;
    }
    
    public void setColor(Color myColor)
    {
        btn.gameObject.GetComponent<Image>().color = myColor;
    }

}
