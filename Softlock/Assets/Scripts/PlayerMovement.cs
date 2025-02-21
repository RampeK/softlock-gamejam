using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Animator animator;
    [SerializeField] private float jumpForce = 5f;    // Hypyn voimakkuus
    [SerializeField] private float gravity = -9.81f;   // Painovoima
    [SerializeField] private LayerMask groundLayer;    // Maakerros tarkistusta varten
    [SerializeField] private Transform groundCheckPosition; // Transform josta tehdään ground check
    
    private Vector3 forward = new Vector3(1f, 0f, 0.5f).normalized;
    private Vector3 right = new Vector3(1f, 0f, -0.5f).normalized;
    
    private GameObject currentBox;
    private bool isHoldingBox;
    private Rigidbody rb;
    public bool isGrounded;
    private Vector3 velocity;
    private bool jumped = false;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = false; // Käytämme omaa painovoimaa
    }
    
    private void Update()
    {
        CheckGrounded();
        HandleJump();
        
        // Tartu laatikkoon tai päästä irti
        //if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F) || Input.GetMouseButtonDown(0))
        //{
        //    if (!isHoldingBox)
        //    {
        //        TryGrabBox();
        //    }
        //}
        
        //if (Input.GetKeyUp(KeyCode.E) || Input.GetKeyUp(KeyCode.F) || Input.GetMouseButtonUp(0))
        //{
        //    ReleaseBox();
        //}
        
        HandleMovement();
    }
    
    private void CheckGrounded()
    {
        // Tarkista onko hahmo maassa raycastilla
        float rayLength = 0.1075f;
        isGrounded = Physics.Raycast(groundCheckPosition.position, Vector3.down, rayLength, groundLayer);
        
        // Päivitä animaattori
        // animator.SetBool("Jump", !isGrounded);
    }
    
    private void HandleJump()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            animator.SetTrigger("Jump");

            // puolen sekunnin viiveellä flagi päälle, jotta painovoima toimii oikein

            jumped = true;
            StartCoroutine(SetJumpedToFalse());
        }
        
        // Jos maassa, nollaa pystysuuntainen nopeus
        if (!isGrounded)
        {
            // velocity.y = -2f;
            // Lisää painovoima
            velocity.y += gravity * Time.deltaTime;
        }
        else
        {
            if (!jumped)
            {
                velocity.y = 0;
            }
        }

        Debug.Log("Y Velocity = " + velocity.y);
    }
    
    private IEnumerator SetJumpedToFalse()
    {
        yield return new WaitForSeconds(0.5f);

        jumped = false;
    }

    private void HandleMovement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        
        Vector3 movement = (forward * verticalInput + right * horizontalInput).normalized;
        
        // Yhdistä vaaka- ja pystyliike
        Vector3 moveVelocity = movement * moveSpeed;
        rb.linearVelocity = new Vector3(moveVelocity.x, velocity.y, moveVelocity.z);
        
        // Liikuta laatikkoa pelaajan mukana
        if (currentBox != null && isHoldingBox && movement != Vector3.zero)
        {
            currentBox.transform.position = transform.position + movement.normalized;
        }
        
        // Päivitä kävelyanimaatio vain jos hahmo on maassa
        if (isGrounded)
        {
            if (horizontalInput == 0 && verticalInput == 0)
            {
                animator.SetBool("Walk", false);
            }
            else
            {
                animator.SetBool("Walk", true);
            }
        }
    }
    
    //private void TryGrabBox()
    //{
    //    Collider[] colliders = Physics.OverlapSphere(transform.position, interactionDistance);
    //    foreach (Collider collider in colliders)
    //    {
    //        if (collider.CompareTag("Box"))
    //        {
    //            currentBox = collider.gameObject;
    //            isHoldingBox = true;
    //            break;
    //        }
    //    }
    //}
    
    //private void ReleaseBox()
    //{
    //    isHoldingBox = false;
    //    currentBox = null;
    //}
} 