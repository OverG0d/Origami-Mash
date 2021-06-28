using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionManager : MonoBehaviour
{
    public Dropdown dropdown_res;
    public Dropdown dropdown_fullscreen;
    public Canvas canvas;
    public bool fullScreen;
    List<string> dropdownOptions = new List<string>();
    Resolution[] resolutions;
    ScreenPrefObject myScreenPrefs = new ScreenPrefObject();
    //initialize the screen prefs
    private void Start()
    {
        LoadScreenData();
        ChangeResolution();
        FullScreen();
    }

    public void ChangeResolution()
    {
        switch (dropdown_res.value)
        {
            case 0:
                Screen.SetResolution(1366, 768, fullScreen);
                //canvas.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1366, 768);
                break;
            case 1:
                Screen.SetResolution(1920, 1080, fullScreen);
                //canvas.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1920, 1080);
                break;
            case 2:
                Screen.SetResolution(1280, 720, fullScreen);
                //canvas.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1280, 720);
                break;
        }
        SaveScreenData();
    }

    public void FullScreen()
    {
        switch (dropdown_fullscreen.value)
        {
            case 0:
                fullScreen = true;
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 1:
                fullScreen = false;
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
        }
        SaveScreenData();
    }

    private void SaveScreenData()
    {
        string filepath = Path.Combine(Application.streamingAssetsPath, "screendata.json");
        if (!File.Exists(filepath))
        {
            File.CreateText(filepath);
        }
        myScreenPrefs.resolutionPref = dropdown_res.value;
        myScreenPrefs.screenPref = dropdown_fullscreen.value;
        string screenPrefsSerial = JsonUtility.ToJson(myScreenPrefs);
        File.WriteAllText(filepath, screenPrefsSerial);
    }

    private void LoadScreenData()
    {
        string filepath = Path.Combine(Application.streamingAssetsPath, "screendata.json");
        string serialdata = File.ReadAllText(filepath);
        ScreenPrefObject screenPrefs = JsonUtility.FromJson<ScreenPrefObject>(serialdata);
        //Debug.Log(screenPrefs.resolutionPref);
        dropdown_res.value = screenPrefs.resolutionPref;
        dropdown_fullscreen.value = screenPrefs.screenPref;
    }

    private class ScreenPrefObject
    {
        public int screenPref, resolutionPref;
    }
}

