using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private GameManager gameManager;
    private AudioManager audioManager;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        audioManager = FindFirstObjectByType<AudioManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        gameManager.InitiateEndScreen(false);
        audioManager.StopSinging();
        audioManager.PlayReaction(false);
    }
}
