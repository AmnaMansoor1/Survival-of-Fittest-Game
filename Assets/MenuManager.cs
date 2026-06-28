using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    public void StartGame()
    {
        SceneManager.LoadScene("DarkForest");
    }
    public void ExitGame()
    {
        Debug.Log("Game Exit Pressed");
        Application.Quit();
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("DarkForest"); // reload current game scene
    }
    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartMenu"); // exact name of your menu scene
    }
}
