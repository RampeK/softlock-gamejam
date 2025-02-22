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
    
    // Isometrisen näkymän liikkumisvektorit
    private Vector3 forward = new Vector3(1f, 0f, 0.5f).normalized;  // Määrittää "ylös" suunnan isometrisessä näkymässä
    private Vector3 right = new Vector3(1f, 0f, -0.5f).normalized;   // Määrittää "oikea" suunnan isometrisessä näkymässä
    
    private GameObject currentBox;
    private bool isHoldingBox;
    private Rigidbody rb;
    public bool isGrounded;
    private Vector3 velocity;
    private bool jumped = false;
    public bool isEnded = false;
    
    [SerializeField] private float deathY = -10f;        // Y-koordinaatti jonka alla hahmo kuolee
    [SerializeField] private float respawnHeight = 2f;   // Kuinka korkealta hahmo spawnataan
    private Vector3 startPosition;                        // Aloituspisteen tallennus
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = false; // Käytämme omaa painovoimaa
        
        // Tallenna aloituspiste
        startPosition = transform.position;
    }
    
    private void Update()
    {
        if (!isEnded)
        {
            CheckGrounded();
            HandleJump();
            CheckRespawn();  // Lisätään tarkistus
            HandleMovement();
        }
        
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
    }
    
    private IEnumerator SetJumpedToFalse()
    {
        yield return new WaitForSeconds(0.5f);

        jumped = false;
    }

    private void HandleMovement()
    {
        // Lue pelaajan syötteet (-1 to 1)
        float horizontalInput = Input.GetAxisRaw("Horizontal");  // A/D tai nuolinäppäimet
        float verticalInput = Input.GetAxisRaw("Vertical");      // W/S tai nuolinäppäimet
        
        // Laske liikkumissuunta isometrisessä näkymässä yhdistämällä forward ja right vektorit
        Vector3 movement = (forward * verticalInput + right * horizontalInput).normalized;
        
        // Laske lopullinen nopeus ja aseta se Rigidbodylle
        Vector3 moveVelocity = movement * moveSpeed;
        rb.linearVelocity = new Vector3(moveVelocity.x, velocity.y, moveVelocity.z);  // Säilytä y-nopeus hyppyä varten
        
        // Käännä hahmo liikkumissuuntaan, 90 astetta käännettynä isometrisessä näkymässä
        if (movement != Vector3.zero)
        {
            // Käännetään movement-vektori 90 astetta, jotta hahmo katsoo oikeaan suuntaan
            Vector3 lookDirection = new Vector3(-movement.z, 0, movement.x);
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }
        
        // Liikuta laatikkoa pelaajan mukana jos sellainen on tartuttu
        if (currentBox != null && isHoldingBox && movement != Vector3.zero)
        {
            currentBox.transform.position = transform.position + movement.normalized;
        }
        
        // Päivitä kävelyanimaatio vain jos hahmo on maassa
        if (isGrounded)
        {
            // Vaihda idle/walk animaatiota liikkumistilan mukaan
            if (horizontalInput == 0 && verticalInput == 0)
            {
                animator.SetBool("Walk", false);  // Paikallaan -> idle animaatio
            }
            else
            {
                animator.SetBool("Walk", true);   // Liikkeessä -> walk animaatio
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

    // Tarkista pitääkö hahmo palauttaa alkuun
    private void CheckRespawn()
    {
        if (transform.position.y < deathY)
        {
            Respawn();
        }
    }
    
    // Palauta hahmo alkupisteeseen
    private void Respawn()
    {
        // Nollaa nopeudet
        rb.linearVelocity = Vector3.zero;
        velocity = Vector3.zero;
        
        // Aseta hahmo alkupisteeseen hieman ilmaan
        Vector3 respawnPosition = startPosition + Vector3.up * respawnHeight;
        transform.position = respawnPosition;
        
        // Nollaa muut tarvittavat tilat
        jumped = false;       
    }
} 