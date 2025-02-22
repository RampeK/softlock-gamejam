using UnityEngine;

public class BoxDestroy : MonoBehaviour
{
    [SerializeField] private float destroyY = -10f;    // Y-koordinaatti jonka alla laatikko tuhoutuu
    [SerializeField] private float softLockTime = 1f;  // Aika jonka laatikon pitää olla nurkassa
    
    private float softLockTimer = 0f;
    private int wallContactCount = 0;  // Montaa seinää laatikko koskee
    
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
            
            if (softLockTimer >= softLockTime)
            {
                Debug.Log("Box is softlocked in corner!");
                Destroy(gameObject); //TODO: Korvaa end screenillä
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
            //TODO: Tähän voidaan myöhemmin lisätä end screen kutsu           
            Destroy(gameObject);
        }
    }
} 