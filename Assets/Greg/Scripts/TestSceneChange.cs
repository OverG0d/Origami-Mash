using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestSceneChange : MonoBehaviour
{
    public void changeScene() {
        SceneManager.LoadScene("Arda/Scenes/Tutorial");
    }
}
