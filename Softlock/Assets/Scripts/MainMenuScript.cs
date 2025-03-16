using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    private AudioManager audioManager;
    public Button playButton;
    void Start()
    {
        audioManager = FindFirstObjectByType<AudioManager>();
        playButton.onClick.AddListener(StartGame);

        audioManager.StopSinging();
        audioManager.PlaySinging(true);
    }

    void StartGame()
    {
        audioManager.PlayButtonSound();
        SceneManager.LoadScene(sceneName: "MainScene");
    }
}
