using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float boxMoveSpeed = 3f;
    [SerializeField] private float interactionDistance = 2f;
    [SerializeField] private Animator animator;
    
    private Vector3 forward = new Vector3(1f, 0f, 0.5f).normalized;
    private Vector3 right = new Vector3(1f, 0f, -0.5f).normalized;
    
    private GameObject currentBox;
    private bool isHoldingBox;
    private Rigidbody rb;
    private bool isWalking = false;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Lukitaan rotaatio, jotta hahmo ei kaadu
        rb.freezeRotation = true;
        // Lukitaan Y-akseli, jotta hahmo pysyy samalla korkeudella
        rb.constraints = RigidbodyConstraints.FreezePositionY;
    }
    
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
        
        rb.linearVelocity = movement * currentSpeed;
        
        // Liikuta laatikkoa pelaajan mukana
        if (currentBox != null && isHoldingBox && movement != Vector3.zero)
        {
            currentBox.transform.position = transform.position + movement.normalized;
        }

        // Tarkista onko nykyinen animaatio vielä kesken
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        
        if (horizontalInput == 0 && verticalInput == 0)
        {
            // Vaihda idle-animaatioon vain jos walking-animaatio on loppunut
            if (stateInfo.IsName("Walking") && stateInfo.normalizedTime >= 1.0f)
            {
                animator.SetBool("isIdle", true);
                animator.SetBool("isWalking", false);
                isWalking = false;
            }
        }
        else
        {
            // Käsittele walking-animaatio
            if (!isWalking)
            {
                animator.SetBool("isWalking", true);
                animator.SetBool("isIdle", false);
                isWalking = true;
            }
            else if (stateInfo.IsName("Walking") && stateInfo.normalizedTime >= 1.0f)
            {
                // Käynnistä walking-animaatio uudelleen vain kun se on mennyt loppuun
                animator.SetTrigger("WalkAgain");
            }
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