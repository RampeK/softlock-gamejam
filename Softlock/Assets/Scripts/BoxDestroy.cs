using System.Collections;
using UnityEngine;

public class BoxDestroy : MonoBehaviour
{
    [SerializeField] private float destroyY = -10f;    // Y-koordinaatti jonka alla laatikko tuhoutuu
    [SerializeField] private float softLockTime = 1f;  // Aika jonka laatikon pitää olla nurkassa
    private GameManager gameManager;
    private AudioManager audioManager;
    
    private float softLockTimer = 0f;
    private int wallContactCount = 0;  // Montaa seinää laatikko koskee
    private bool check = false;

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        audioManager = FindFirstObjectByType<AudioManager>();
    }

    private void Update()
    {
        CheckDestroy();
        CheckSoftLock();
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            wallContactCount++;
        }
    }
    
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            wallContactCount--;
        }
    }
    
    private void CheckSoftLock()
    {
        // Soft lock tapahtuu vain kun laatikko koskee kahta tai useampaa seinää (nurkka)
        bool isInCorner = wallContactCount >= 2;
        
        if (isInCorner)
        {
            softLockTimer += Time.deltaTime;

            if (!check)
            {
                audioManager.StopSinging();
                audioManager.PlayReaction(true);
                check = true;
            }
            
            if (softLockTimer >= softLockTime)
            {
                Debug.Log("Box is softlocked in corner!");
                gameManager.InitiateEndScreen(true);
            }
        }
        else
        {
            softLockTimer = 0f;
        }
    }
    
    private void CheckDestroy()
    {
        if (transform.position.y < destroyY)
        {
            if (!check)
            {
                audioManager.StopSinging();
                audioManager.PlayReaction(true);
                check = true;

                StartCoroutine(DestroyBox());
            }
        }
    }

    private IEnumerator DestroyBox()
    {
        yield return new WaitForSeconds(1f);

        Debug.Log("Box is destroyed!");
        gameManager.InitiateEndScreen(true);
        Destroy(gameObject);
    }
} 