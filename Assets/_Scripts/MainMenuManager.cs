using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        SceneLoader.instance.LoadScene("MainScene");
    }

    public void ExitGame()
    {
        SceneLoader.instance.QuitGame();
    }
}
