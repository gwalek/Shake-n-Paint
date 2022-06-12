using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class DrawingControl : MonoBehaviour
{
    // Grash's Notes: 
    // This is Drawing in 2D only on the Canvas. 
    // We are drawing by creating a prefab that has a 2d Image.
    // This prefab is set up and placed in the "DrawArea" on the Canvas.  
    // How this performs is easily extrapolated into 3D, like for VR. 


    // Controls if we're drawing or not. 
    // When a Menu is open, this is false
    public bool DrawingOn = true;

    // Grash's Notes:
    // We have two Booleans here because Multi-line and Single line was added late. 
    // And I only need to control 1 line of code for it. 

    // Okay This variable needs to be true for multi-line or Single line mode. 
    public bool IsLinesModeOn = false;
    // This varible controls if you're in Multi-line or single line mode
    public bool DrawOnlyALine = false; 

    
    [Space(10)]
    // Current drawing info 
    public Color currentColor = Color.red;
    public float currentWidth = 10;
    public Sprite currentBrush;

    [Space(10)]
    // Drawing object prefab and Where to place them
    public GameObject drawingObject;
    public Transform DrawArea;
    public List<GameObject> drawingObjectList;

    // Privates, Drawing variables 
    GameObject currentObject;
    Vector3 MousePosition = Vector3.zero;
    Vector3 PreviousMouse = Vector3.zero;
    Vector3 StartingPosition = Vector3.zero;


    void Start()
    {
        // This object is set up in the inspector
        // Or gets what it needs during Update
    }

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
            return; 
        }

        if (Input.GetMouseButton(0))
        {
            if (IsLinesModeOn)
            {
                OnMove();
            }
            return; 
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (IsLinesModeOn)
            {
                OnEnd();
            }
            return; 
        }
    }

    public void TouchControls()
    {
        Touch[] touches = Input.touches; 
        if (touches.Length > 0)
        {
            if (touches[0].phase == TouchPhase.Began)
            {
                OnStart(); 
            }


            if (touches[0].phase == TouchPhase.Moved)
            {
                if (IsLinesModeOn)
                {
                    OnMove();
                }

            }

            if (touches[0].phase == TouchPhase.Ended)
            {
                if (IsLinesModeOn)
                {
                    OnEnd();
                }
            }

            if (touches[0].phase == TouchPhase.Canceled)
            {
                RemoveLastLine(); 
            }
        }
        
    }

    // Grash's Notes: 
    // This is needed by the menu.
    // Otherwise we get an extra drawing object on Screen when we click the button 
    // This Fixes a bug from the legacy app! 
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

    // Grash's Notes: 
    // When we start, we create a drawing object
    // This gets used by OnMove to create the next object drawn in Multi-Line Mode (aka "Draw Lines")
    void OnStart()
    {
        currentObject = Instantiate(drawingObject, DrawArea);
        Image image = currentObject.GetComponent<Image>();
        image.color = currentColor;
        image.sprite = currentBrush;
        image.rectTransform.sizeDelta = new Vector2(currentWidth, currentWidth); 
        drawingObjectList.Add(currentObject); 
        currentObject.transform.position = MousePosition;
        StartingPosition = MousePosition; 
    }

    // Grash's Notes: 
    // Only Called if IsLinesModeOn is true
    void OnMove()
    {
        Image image = currentObject.GetComponent<Image>();
        Vector3 dMouse = (MousePosition - StartingPosition);

        float angle = Mathf.Atan2(dMouse.y, dMouse.x) * Mathf.Rad2Deg;
        //Debug.Log("angle: " + angle);
        image.rectTransform.rotation = Quaternion.Euler(0, 0, angle); 
        image.rectTransform.sizeDelta = new Vector2(dMouse.magnitude + .5f, currentWidth);

        if (!DrawOnlyALine)
        {
            OnStart(); 
        }
    }

   void OnEnd()
    {
        // Grash's Notes: 
        // How the application works, this doesn't matter... We end, We end. 
        // Start and Move are going to handle everythign else anyways. 
    }

}
