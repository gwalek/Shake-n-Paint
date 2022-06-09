using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class DrawingControl : MonoBehaviour
{

    public bool DrawingOn = true;
    public bool DrawLines = false; 
    public Color currentColor = Color.red;
    public float currentWidth = 10;
    public Sprite currentBrush; 
    public GameObject drawingObject;
    public Transform DrawArea; 
    public Vector3 MousePosition;
    public Vector3 PreviousMouse = Vector3.zero;

    public List<GameObject> drawingObjectList; 

    GameObject currentObject; 




    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PreviousMouse = MousePosition; 
        MousePosition = Input.mousePosition;
        if (DrawingOn)
        {
            
           if (Input.touchSupported)
            {
                TouchControls();
            }
           else
            {
                MouseControls(); 
            }


            

        }
    }

    public void MouseControls()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnStart();
        }
    }

    public void TouchControls()
    {
        Touch[] touches = Input.touches; 
        if (touches.Length > 0)
        {
            if (touches[0].phase == TouchPhase.Began)
            {

            }


            if (touches[0].phase == TouchPhase.Moved)
            {

            }

            if (touches[0].phase == TouchPhase.Ended)
            {

            }

            if (touches[0].phase == TouchPhase.Canceled)
            {
                RemoveLastLine(); 
            }
        }
        
    }




    public void RemoveLastLine()
    {
        int count = drawingObjectList.Count;
        if (count > 0)
        {
            Destroy(drawingObjectList[count - 1]);
            drawingObjectList.RemoveAt(count - 1);
            currentObject = null; 
        }
    }

    void OnStart()
    {
        currentObject = Instantiate(drawingObject, DrawArea);
        Image image = currentObject.GetComponent<Image>();
        image.color = currentColor;
        image.sprite = currentBrush;
        image.rectTransform.sizeDelta = new Vector2(currentWidth, currentWidth); 
        drawingObjectList.Add(currentObject); 
        currentObject.transform.position = MousePosition; 
    }

    void OnMove()
    {
        
    }

   void OnEnd()
    {

    }

}
