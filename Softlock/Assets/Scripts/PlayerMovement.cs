using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float boxMoveSpeed = 3f;
    [SerializeField] private float interactionDistance = 2f;
    
    private Vector3 forward = new Vector3(1f, 0f, 1f).normalized;
    private Vector3 right = new Vector3(1f, 0f, -1f).normalized;
    
    private GameObject currentBox;
    private bool isHoldingBox;
    
    private void Update()
    {
        // Tartu laatikkoon tai päästä irti
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F) || Input.GetMouseButtonDown(0))
        {
            if (!isHoldingBox)
            {
                TryGrabBox();
            }
        }
        
        if (Input.GetKeyUp(KeyCode.E) || Input.GetKeyUp(KeyCode.F) || Input.GetMouseButtonUp(0))
        {
            ReleaseBox();
        }
        
        HandleMovement();
    }
    
    private void HandleMovement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        
        Vector3 movement = (forward * verticalInput + right * horizontalInput).normalized;
        float currentSpeed = isHoldingBox ? boxMoveSpeed : moveSpeed;
        
        transform.position += movement * currentSpeed * Time.deltaTime;
        
        // Liikuta laatikkoa pelaajan mukana
        if (currentBox != null && isHoldingBox && movement != Vector3.zero)
        {
            currentBox.transform.position = transform.position + movement;
        }
    }
    
    private void TryGrabBox()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionDistance);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Box"))
            {
                currentBox = collider.gameObject;
                isHoldingBox = true;
                break;
            }
        }
    }
    
    private void ReleaseBox()
    {
        isHoldingBox = false;
        currentBox = null;
    }
} 