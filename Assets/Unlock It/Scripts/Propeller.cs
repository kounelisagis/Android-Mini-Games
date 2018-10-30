using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Propeller : MonoBehaviour {

    private bool myState = true;
    private bool isThisClicked = false;

    public void setPosition(Vector2 myVec)
    {
        this.transform.localPosition = myVec;
    }

    public bool getState()
    {
        return myState;
    }

    public bool isClicked()
    {
        return isThisClicked;
    }

    public void toggleClicked()
    {
        isThisClicked = !isThisClicked;
    }
	
    public void toggleState()
    {
        myState = !myState;

        this.gameObject.transform.GetChild(0).localEulerAngles += new Vector3(0, 0, 90);
    }
    
}
