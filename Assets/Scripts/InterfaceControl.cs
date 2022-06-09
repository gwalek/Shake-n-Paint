using System.Collections;
using System.IO; 
using System.Collections.Generic;
using UnityEngine;
using NativeGalleryNamespace; 

public class InterfaceControl : MonoBehaviour
{
    public bool AddSavedImages = false; 
    public DrawingControl drawingControl;

    public GameObject menuButton;
    public GameObject menuPannel;
    public AudioClip clickSound;
    public AudioClip shakeSound;
    public AudioClip cameraSource;
    public AudioSource audioSource;

    public string fileName;

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

        fileName = "SNP." +
            now.Year + "." + now.Month + "." + now.Day + "." +
            now.Hour + "." + now.Minute + "." + now.Second + ".png"; 
        
        if(AddSavedImages)
        {
            fileName = "SavedImages//" + fileName; 
        }

        ScreenCapture.CaptureScreenshot(fileName);

    }

    public void CheckExists()
    {
        string wherefrom = Application.persistentDataPath + "/" + fileName;

        //Environment.getExternalStoragePublicDirectory(Environment.DIRECTORY_DOWNLOADS);

        string whereto = "DCIM/Screenshots" + "/" + fileName;
        
        Debug.Log("wherefrom: " + wherefrom);
        
        if (File.Exists(wherefrom))
        {
            Debug.Log("Found File in App Area");

        }
        
        Debug.Log("whereto: " + whereto);

        NativeGallery.SaveImageToGallery(wherefrom, "Downloads", fileName); 
        //File.Move(wherefrom, whereto); 

        if (File.Exists(whereto))
        {
            Debug.Log("Found File in Gallery");
        }

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
