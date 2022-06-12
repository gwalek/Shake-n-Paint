using System.Collections;
using System.IO; 
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using NativeGalleryNamespace;
using TMPro; 

// Grash's Comment: 
// Admittedly InterfaceControl is the Kitchen sink of the application. 
// Every Button and interface element code is here
// Saving the Image is here
// As well as how to handle the shakes... 

public class InterfaceControl : MonoBehaviour
{
    public DrawingControl drawingControl;
    public bool AddSavedImagesFolderName = false;

    public bool MoreCowbell = false;
    

    [Space(10)]
    // Co-Routines 
    public bool IsClearing = false;
    public float ClearAfterShakeReenabledTime = .2f;
    public bool HitingCowbell = false;
    public float CowbellReenabledTime = .2f;
    

    [Space(10)]
    public Camera mainCamera;
    public Image menuBackground;
    public Image menuBrush;
    public TextMeshProUGUI WidthField;
    public Scrollbar widthScrollbar; 
    public TextMeshProUGUI DrawModeField;

    [Space(10)]
    public GameObject menuButton;
    public GameObject menuPannel;
    public GameObject aboutPannel;
    public GameObject cowbellPannel;
    public GameObject exitPannel;
    public GameObject CameraFlash; 

    [Space(10)]
    public int DrawMode = 0;
    public int currentColor = 0;
    public int currentBrush = 0;
    public int currentBackground = 10;
    public int currentWidth = 10; 
    public int MAXWIDTH = 100;
    
    [Space(10)]
    public AudioSource audioSource;
    public AudioClip clickSound;
    public AudioClip shakeSound;
    public AudioClip cameraSource;
    public AudioClip MoarCowbell;
 
    [Space(10)]
    public string fileName;

    [Space(10)]
    public List<Color> colorList;
    public List<Sprite> brushList;

    public void Start()
    {
        // The App Defaults to a drawing state. 
        drawingControl.DrawingOn = true;
        MoreCowbell = false;
        menuButton.SetActive(true);
        menuPannel.SetActive(false);
        aboutPannel.SetActive(false);
        cowbellPannel.SetActive(false);
        exitPannel.SetActive(false);
    }

    // Self Promotion, Ya'll!
    public void OpenURL(string URL)
    {
        Application.OpenURL(URL); 
    }

    // Grash's Notes:
    // Why do you see alot of repeated code in the OnOpen--- Methods?
    // The app can be losely looked at in terms of states. 
    // We can think of Drawing as a state, Each pannel as a state.  
    // The drawing it self, saving, shakes, all the buttons as "actions" aka doing something... 
    // One of the things that I picked from using an abstract state modeling language, 
    // When you change a state, you make sure you set every variable, not just the ones that changed. 
    // This makes state changes cleaner when you do them! 

    public void OnOpenMenu()
    {
        Debug.Log("Open Menu");
        
        menuButton.SetActive(false);
        menuPannel.SetActive(true);
        aboutPannel.SetActive(false); 
        cowbellPannel.SetActive(false);
        exitPannel.SetActive(false);

        audioSource.PlayOneShot(clickSound);

        if (drawingControl.DrawingOn)
        {
            drawingControl.RemoveLastLine();
        }
        drawingControl.DrawingOn = false;
        MoreCowbell = false;
    }

    public void OnCloseMenu()
    {
        Debug.Log("Close Menu");
  
        menuButton.SetActive(true);
        menuPannel.SetActive(false);
        aboutPannel.SetActive(false);
        cowbellPannel.SetActive(false);
        exitPannel.SetActive(false);

        audioSource.PlayOneShot(clickSound);

        drawingControl.DrawingOn = true;
        MoreCowbell = false;
    }

    public void OnOpenAbout()
    {
        Debug.Log("Open About");

        menuButton.SetActive(true);
        menuPannel.SetActive(false);
        aboutPannel.SetActive(true);
        cowbellPannel.SetActive(false);
        exitPannel.SetActive(false);

        audioSource.PlayOneShot(clickSound);

        drawingControl.DrawingOn = false;
        MoreCowbell = false;
    }

    public void OnOpenCowbell()
    {
        Debug.Log("Open Cowbell");

        menuButton.SetActive(true);
        menuPannel.SetActive(false);
        aboutPannel.SetActive(false);
        cowbellPannel.SetActive(true);
        exitPannel.SetActive(false);

        audioSource.PlayOneShot(clickSound);

        drawingControl.DrawingOn = false;
        MoreCowbell = true;
    }

    public void OnOpenExit()
    {
        Debug.Log("Open Exit");

        menuButton.SetActive(true);
        menuPannel.SetActive(false);
        aboutPannel.SetActive(false);
        cowbellPannel.SetActive(false);
        exitPannel.SetActive(true);

        audioSource.PlayOneShot(clickSound);

        drawingControl.DrawingOn = false;
    }

    public void OnExitApplication()
    {
        audioSource.PlayOneShot(clickSound);
        Application.Quit(); 
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
        if (IsClearing)
        {
            return; 
        }
        IsClearing = true;
        audioSource.PlayOneShot(shakeSound);
        ClearImage();
        StartCoroutine(WaitToClearAgain());
    }

    public IEnumerator WaitToClearAgain()
    {
        yield return new WaitForSeconds(ClearAfterShakeReenabledTime);
        IsClearing = false; 
    }

    public void OnShake()
    {
        Debug.Log("On Shake");
        
        if (MoreCowbell)
        {
            OnCowbell(); 
        }

        if (drawingControl.DrawingOn)
        {
            OnClearImage(); 
        }
    }

    public void OnCowbell()
    {
        if (HitingCowbell)
        {
            return; 
        }
        audioSource.PlayOneShot(MoarCowbell);
        HitingCowbell = true;
        StartCoroutine(ReenableCowbell()); 
    }

    public IEnumerator ReenableCowbell()
    {
        yield return new WaitForSeconds(CowbellReenabledTime);
        HitingCowbell = false;
    }


    public void SaveImage()
    {
        System.DateTime now = System.DateTime.Now;

        fileName = "SNP." +
            now.Year + "." + now.Month + "." + now.Day + "." +
            now.Hour + "." + now.Minute + "." + now.Second + ".png"; 
        
        // This is a build for platform setting really... 
        if(AddSavedImagesFolderName)
        {
            fileName = "SavedImages//" + fileName; 
        }
        // Hide Menu Pannel... 
        menuPannel.SetActive(false);

        // Start Co-Routine
        Debug.Log("Star Co-Routine");
        StartCoroutine(PerformScreenCapture()); 
        

    }
    public IEnumerator PerformScreenCapture()
    {
       
        yield return new WaitForSeconds(.1f);
        ScreenCapture.CaptureScreenshot(fileName);

        // This needs to be .2 in order the file to be there. 
        yield return new WaitForSeconds(.2f);
        CameraFlash.SetActive(true);

        // Need to wrap this correctly. 
        MoveScreenShotToGallery();

        yield return new WaitForSeconds(.3f);
        CameraFlash.SetActive(false);
        yield return new WaitForSeconds(.1f);
        
        // Reopen the Menu, it was closed to take a screen shot. 
        OnOpenMenu();
    }

 

    public void MoveScreenShotToGallery()
    {
        string wherefrom = Application.persistentDataPath + "/" + fileName;
        NativeGallery.SaveImageToGallery(wherefrom, "Downloads", fileName);
    }

    public void ClearImage()
    {
        foreach (GameObject go in drawingControl.drawingObjectList)
        {
            Destroy(go); 
        }
        drawingControl.drawingObjectList.Clear(); 
    }

    public void NextBrush()
    {
        currentBrush++; 
        if (currentBrush >= brushList.Count) { currentBrush = 0;  }
        menuBrush.sprite = brushList[currentBrush]; 
        drawingControl.currentBrush = brushList[currentBrush]; 
    }
    public void PrevBrush()
    {
        currentBrush--;
        if (currentBrush <= -1 ) { currentBrush = brushList.Count-1; }
        menuBrush.sprite = brushList[currentBrush];
        drawingControl.currentBrush = brushList[currentBrush];
    }

    public void NextColor()
    {
        currentColor++;
        if (currentColor >= colorList.Count) { currentColor = 0; }
        menuBrush.color = colorList[currentColor];
        drawingControl.currentColor = colorList[currentColor];
    }

    public void PrevColor()
    {
        currentColor--;
        if (currentColor <= -1) { currentColor = colorList.Count - 1; }
        menuBrush.color = colorList[currentColor];
        drawingControl.currentColor = colorList[currentColor];
    }

    public void NextBackground()
    {
        currentBackground++;
        if (currentBackground >= colorList.Count) { currentBackground = 0; }
        menuBackground.color = colorList[currentBackground];
        mainCamera.backgroundColor = colorList[currentBackground];
    }

    public void PrevBackground()
    {
        currentBackground--;
        if (currentBackground <= -1) { currentBackground = colorList.Count - 1; }
        menuBackground.color = colorList[currentBackground];
        mainCamera.backgroundColor = colorList[currentBackground];
    }

    public void AddWidth()
    {
        currentWidth++; 
        if (currentWidth >= MAXWIDTH) { currentWidth = MAXWIDTH; }
        drawingControl.currentWidth = currentWidth;
        WidthField.text = "" + currentWidth;
    }

    public void SubWidth()
    {
        currentWidth--;
        if (currentWidth <= 0) { currentWidth = 0; }
        drawingControl.currentWidth = currentWidth;
        WidthField.text = "" + currentWidth; 
    }

    public void UpdateWidth()
    {
        currentWidth = (int)(widthScrollbar.value * 100);
        drawingControl.currentWidth = currentWidth;
        WidthField.text = "" + currentWidth;
    }

    public void ToggleDrawMode()
    {
        DrawMode++;
        if (DrawMode > 3) { DrawMode = 0; }

        if (DrawMode == 0) { OnPointMode(); return; }
        if (DrawMode == 1) { OnLineMode(); return; }
        if (DrawMode == 2) { OnLinesMode(); return; }
    }

    public void OnPointMode()
    {
        drawingControl.IsLinesModeOn = false;
        drawingControl.DrawOnlyALine = false;
        DrawModeField.text = "Point Mode";
    }

    public void OnLineMode()
    {
        drawingControl.IsLinesModeOn = true;
        drawingControl.DrawOnlyALine = true;
        DrawModeField.text = "Line Mode";
    }

    public void OnLinesMode()
    {
        drawingControl.IsLinesModeOn = true;
        drawingControl.DrawOnlyALine = false;
        DrawModeField.text = "Lines Mode";
    }


}
