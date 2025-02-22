using UnityEngine;

public class BoxDestroy : MonoBehaviour
{
    [SerializeField] private float destroyY = -10f;    // Y-koordinaatti jonka alla laatikko tuhoutuu
    
    private void Update()
    {
        CheckDestroy();
    }
    
    // Tarkista pitääkö laatikko tuhota
    private void CheckDestroy()
    {
        if (transform.position.y < destroyY)
        {
            //TODO: Tähän voidaan myöhemmin lisätä end screen kutsu           
            Destroy(gameObject);
        }
    }
} 