using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementGrapple : MonoBehaviour
{
    [SerializeField] private float speed = 12f;
    
    [SerializeField] private float groundDistance = 0.1f;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;

    private bool isGrounded;
    private Rigidbody rb;

    private bool jumpNextFrame;
    private Vector3 MovementNextFrame;
    public float jumpHeight = 6f;

    /*
     * Coyote time. Theoretically fully functional, but we chose to disable it for the moment, as we didn't like the effect.
     * private float coyoteTime = 0f;
     * private float currentCountdownTime = 0f;
    */

    Animator animator;

    [Header("Audio")] 
    [SerializeField] private AudioSource walking;
    [SerializeField] private GameObject movementHammer;
    public bool walkingSound = false;
    private bool playingSound = false;

    /// <summary>
    /// Awakes rigidbody and animator of the grapple-player
    /// </summary>
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        animator = transform.GetChild(1).GetChild(1).GetComponent<Animator>();
    }

    /// <summary>
    /// Includes movement Input; Movement Animations; Movement Sound; Landing Animation
    /// </summary>
    private void Update()
    {
        if (Input.GetButtonDown("JumpGrapple") && isGrounded && Time.timeScale > 0) jumpNextFrame = true;

        MovementNextFrame = new Vector3(Input.GetAxis("MovementHorizontalGrapple"), 0f, Input.GetAxis("MovementVerticalGrapple"));

        /*
         * Coyote time.
         * if (isGrounded) currentCountdownTime = coyoteTime;
         * if(!isGrounded) currentCountdownTime -= Time.deltaTime;
        */

        if ((Input.GetAxis("MovementHorizontalGrapple") != 0) || Input.GetAxis("MovementVerticalGrapple") != 0)
        {
            animator.SetBool("isWalking", true);
            if (isGrounded)
            {
                walkingSound = true;
            }
        }
        else
        {
            animator.SetBool("isWalking", false);
            walkingSound = false;
        }

        if (isGrounded)
        {
            animator.SetTrigger("Landing");
        }

        WalkingSound();
    }

    /// <summary>
    /// Includes movement Physics; Jump condition (groundcheck); Falling Animation
    /// </summary>
    void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        transform.Translate((MovementNextFrame) * Time.deltaTime * speed);

        if (!isGrounded)
        {
            animator.SetBool("Falling", true);
            walkingSound = false;
        }
        else
        {
            animator.SetBool("Falling", false);
        }

        if (jumpNextFrame)
        {
            animator.SetTrigger("Jumping");
            jumpNextFrame = false;
            rb.AddForce(new Vector3(0, jumpHeight, 0), ForceMode.Impulse);
            isGrounded = false;
        }
    }

    /// <summary>
    /// Walking Sound plays if sound is not already playing (e.g. through other player).
    /// </summary>
    private void WalkingSound()
    {
        if (walkingSound && !playingSound)
        {
            playingSound = true;
            StartCoroutine(WalkingSoundWait());
        }
        else if (!walkingSound)
        {
            playingSound = false;
            if (!movementHammer.GetComponent<PlayerMovementHammer>().walkingSound)
            {
                walking.Stop();
            }
        }
    }

    /// <summary>
    /// Delays the walking sound, to avoid small, short instances of the sound (e.g. in jump 'n' run parts)
    /// </summary>
    /// <returns>WaitforSeconds</returns>
    private IEnumerator WalkingSoundWait()
    {
        yield return new WaitForSeconds(0.04f);
        walking.Play();
    }
}