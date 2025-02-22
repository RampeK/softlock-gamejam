using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private PlayerMovement playerMovement;

    public Image background;
    public TMP_Text title;
    public TMP_Text subtitle;
    public TMP_Text buttonText;
    public Button endButton;

    private bool ended = false;

    void Start()
    {
        background.gameObject.SetActive(false);
        title.gameObject.SetActive(false);
        subtitle.gameObject.SetActive(false);
        endButton.gameObject.SetActive(false);
    }

    public void InitiateEndScreen(bool softlocked)
    {
        if (!ended)
        {
            FreezeControls();
            StartCoroutine(ShowEndScreen(softlocked));
        }

        ended = true;
    }

    private void FreezeControls()
    {
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        playerMovement.isEnded = true;
    }

    private IEnumerator ShowEndScreen(bool softlocked)
    {
        if (softlocked) { yield return new WaitForSeconds(0.6f); }
        background.gameObject.SetActive(true);

        background.color = new Color(background.color.r, background.color.g, background.color.b, 0);

        while (background.color.a < 0.65f)
        {
            background.color = new Color(background.color.r, background.color.g, background.color.b, background.color.a + (Time.deltaTime / 1));
            yield return null;
        }

        title.gameObject.SetActive(true);
        if (softlocked) { title.text = "Oops! Something went wrong!"; }
        yield return new WaitForSeconds(0.7f);

        subtitle.gameObject.SetActive(true);
        if (softlocked ) { subtitle.text = "You softlocked the level... Congratulations!"; }
        yield return new WaitForSeconds(0.7f);

        endButton.gameObject.SetActive(true);
        if (softlocked) { buttonText.text = "Menu"; }
        endButton.onClick.AddListener(() => EndLevel(softlocked));
    }

    private void EndLevel(bool softlocked)
    {
        if (softlocked)
        {
            SceneManager.LoadScene(sceneName: "MainMenu");
        }
        else
        {
            SceneManager.LoadScene(sceneName: "MainScene");
        }
    }
}
