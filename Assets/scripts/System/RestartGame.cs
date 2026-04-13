using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public void Restart() //restart the game from main menu
    {
        AudioListener.pause = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }
}