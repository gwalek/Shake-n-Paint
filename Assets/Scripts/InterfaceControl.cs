using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceControl : MonoBehaviour
{
    public DrawingControl drawingControl;

    public GameObject menuButton;
    public GameObject menuPannel;
    public AudioClip clickSound;
    public AudioClip shakeSound;
    public AudioClip cameraSource;
    public AudioSource audioSource;

    public string debugFilename;

    public void Start()
    {
        drawingControl.DrawingOn = true;
        menuButton.SetActive(true);
        menuPannel.SetActive(false);
    }

    public void OnOpenMenu()
    {
        Debug.Log("Open Menu");
      
        drawingControl.DrawingOn = false;
        menuButton.SetActive(false);
        menuPannel.SetActive(true);
        audioSource.PlayOneShot(clickSound);
        drawingControl.RemoveLastLine(); 
    }

    public void OnCloseMenu()
    {
        Debug.Log("Close Menu");
        audioSource.PlayOneShot(clickSound);
        drawingControl.DrawingOn = true;
        menuButton.SetActive(true);
        menuPannel.SetActive(false);
    }

    public void OnSaveImage()
    {
        Debug.Log("Save Image");
        audioSource.PlayOneShot(cameraSource);
        SaveImage(); 
    }

    public void OnClearImage()
    {
        Debug.Log("Clear Image");
        audioSource.PlayOneShot(shakeSound);
        ClearImage();
    }

    public void OnShake()
    {
        ClearImage();
    }

    public void ShakeOn()
    {

    }

    public void ShakeOff()
    {

    }

    public void SaveImage()
    {
        System.DateTime now = System.DateTime.Now;

        string filename = "SavedImages\\SNP." +
            now.Year + "." + now.Month + "." + now.Day + "." +
            now.Hour + "." + now.Minute + "." + now.Second + ".png"; 
        debugFilename = filename; 

        ScreenCapture.CaptureScreenshot(filename);
    }

    public void ClearImage()
    {
        foreach (GameObject go in drawingControl.drawingObjectList)
        {
            Destroy(go); 
        }
        drawingControl.drawingObjectList.Clear(); 
    }

}
