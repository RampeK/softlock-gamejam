using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        gameManager.InitiateEndScreen(false);
    }
}
