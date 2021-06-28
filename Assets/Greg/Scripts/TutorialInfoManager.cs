using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class TutorialInfoManager : MonoBehaviour
{
    public Text textDisplay;
    private Queue<string> stringQueue;
    public static TutorialInfoManager instance;
    private void Awake()
    {
        instance = this;
        stringQueue = new Queue<string>();
        TextAsset test = Resources.Load<TextAsset>("TextFiles/TutorialDialog");
        InitTextQueue(test.text);
        PlayNextLine();
    }

    public void PlayNextLine()
    {
        if (stringQueue.Count > 0)
        {
            textDisplay.text = stringQueue.Dequeue();
        }
    }
    void InitTextQueue(string textToConvert)
    {
        string[] tutorialTxt = textToConvert.Split('\n');
        foreach (string sentence in tutorialTxt)
        {
            Debug.Log(sentence);
            stringQueue.Enqueue(sentence);
        }
    }
}