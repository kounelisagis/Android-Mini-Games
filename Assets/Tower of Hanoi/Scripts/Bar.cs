using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;


public class Bar : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    private static mainTower myManager;

    private void Start()
    {
        myManager = GameObject.Find("mainActivity").GetComponent<mainTower>();
    }

    public void setColor(Color clr)
    {
        GetComponent<Image>().color = clr;
    }

    public void setSize(Vector2 vctr)
    {
        this.GetComponent<RectTransform>().sizeDelta = vctr;
    }

    public float getSizeX()
    {
        return this.GetComponent<RectTransform>().sizeDelta.x;
    }

    public void setPosition(Vector2 vctr)
    {
        this.transform.localPosition = vctr;
    }

    private Stack<Bar> myStack;

    public void setStack(Stack<Bar> newStack)
    {
        myStack = newStack;
    }

    public Stack<Bar> getStack()
    {
        return myStack;
    }


    public static Bar itemDragged = null;
    public static Vector2 prevPosition;
    private static bool clickIsOk = false;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(!clickIsOk && myManager.isTop(this))
        {
            clickIsOk = true;
            itemDragged = this;
            prevPosition = this.transform.localPosition;
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if(clickIsOk)
        {
            Canvas myCan = GetComponentInParent<Canvas>();
            Vector2 click = myCan.transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(eventData.position));

            this.transform.localPosition = click;
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if(clickIsOk)
        {
            myManager.clickIsUp(itemDragged, prevPosition);
            itemDragged = null;
            clickIsOk = false;
        }
    }

}