using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public CanvasGroup fadeGroup;
    public GameObject creditsUI;
    public GameObject next;
    public GameObject previous;
    public GameObject howToPlayUI;
    public GameObject[] pages;
    public float fadeDuration = 1f;
    public string firstSceneName;

    int page = 0;

    public void StartGame() //start the game!
    {
        StartCoroutine(FadeAndLoad());
    }

    public void Credits() //display the credits
    {
        creditsUI.SetActive(true);
    }

    public void Back() //go back to main menu
    {
        if (creditsUI.activeSelf){
        creditsUI.SetActive(false);
        }

        if (howToPlayUI.activeSelf)
        {
            howToPlayUI.SetActive(false);
        }
    }

    public void HowToPlay() //open how to play menu
    {
        howToPlayUI.SetActive(true);
        page = 0;
        UpdatePages();
    }

    public void Next() //go to next page
    {
        if (page < pages.Length - 1)
        {
            page++;
            UpdatePages();
        }
    }

    public void Prev() //go to previous page
    {
        if (page > 0)
        {
            page--;
            UpdatePages();
        }
    }

    void UpdatePages() //show current page
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == page);
        }

        previous.SetActive(page > 0);
        next.SetActive(page < pages.Length - 1);
    }

    IEnumerator FadeAndLoad() //fades the main menu to start game
    {
        float t = 0;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeGroup.alpha = t / fadeDuration;
            yield return null;
        }

        SceneManager.LoadScene(firstSceneName);
    }
}