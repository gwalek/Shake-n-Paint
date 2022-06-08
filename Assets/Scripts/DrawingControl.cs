using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class DrawingControl : MonoBehaviour
{
    public bool DrawingOn = true;
    public Color currentColor = Color.red; 
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
            
            if(Input.GetMouseButtonDown(0))
            {
                OnStart();
            }


            //drawingObject.transform.position = Input.mousePosition; 

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
