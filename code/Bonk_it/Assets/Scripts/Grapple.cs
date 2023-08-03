using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    [SerializeField] float pullSpeed = 0.5f;
    [SerializeField] float stopDistance = 8f;
    [SerializeField] float NoHitDistance= 20f;
    [SerializeField] GameObject hookPrefab;
    [SerializeField] Transform shootTransform;
    [SerializeField] float maxForce = 1f;
    [SerializeField] float minForce = 0f;

    Hook hook;
    bool pulling;
    Rigidbody rb;
    
    private bool addForce;
    private bool pullingHammer;

    Rigidbody rbHammer;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] GameObject playerHammer;

    [Header("Audio")] 
    [SerializeField] private AudioSource grapplingHook;
    [SerializeField] private AudioSource grapplingHit;

    //Ability UI
    [Header("UI")]
    [SerializeField] GameObject grappleUIOn;
    [SerializeField] GameObject grappleUIOff;
    [SerializeField] GameObject grappleUIActive;

    [SerializeField] GameObject grappleCancelUIOn;
    [SerializeField] GameObject grappleCancelUIOff;

    //Raycast method
    [Header("Raycast")]
    [SerializeField] private Camera cam;
    private Vector3 origin;
    private Vector3 direction;
    private float maxraycast = 30f;

    /// <summary>
    /// Access both players' rigidbodies.
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rbHammer = GameObject.Find("PlayerHammer").GetComponent<Rigidbody>();
        pulling = false;
    }

    /// <summary>
    /// Grapple ability and grapple ability UI.
    /// </summary>
    void Update()
    {
        //Grapple UI using raycast
        origin = cam.transform.position;
        direction = cam.transform.forward;
        Ray ray = new Ray(origin, direction);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxraycast))
        {
            if (hit.collider.gameObject.CompareTag("Grapple") || (hit.collider.gameObject.layer == 8))
            {
                GrappleUI(0);
            }
            else
            {
                GrappleUI(1);
            }
        }
        else
        {
            GrappleUI(1);
        }

        if(hook != null)
        {
            GrappleUI(2);
        }

        //Cancel Grapple UI
        if (hook != null)
        {
            GrappleCancelUI(true);
        }
        else
        {
            GrappleCancelUI(false);
        }

        addForce = false;
        pullingHammer = false;
        //Spawning hook on shootTransform position (camera)
        if (hook == null && Input.GetButtonDown("LinksklickGrapple") && Time.timeScale > 0)
        {
            StopAllCoroutines();
            HookSound();
            pulling = false;
            hook = Instantiate(hookPrefab, shootTransform.position, Quaternion.identity).GetComponent<Hook>();
            hook.Initialize(this, shootTransform);
            StartCoroutine(DestroyHookAfterLifetime());
        }
        //After surpassing NoHitDistance hook gets destroyed.
        else if(hook != null && Vector3.Distance(transform.position, hook.transform.position) > NoHitDistance)
        {
            DestroyHook();
        }
        //Cancel hook using input.
        else if(hook != null && Input.GetButtonDown("RechtsklickGrapple"))
        {
            DestroyHook();
        }

        if (!pulling || hook == null) return;

        //Cancels hook when the player is too close to the hook.
        if(Vector3.Distance(transform.position, hook.transform.position) <= stopDistance)
        {
            DestroyHook();
        }

        //Pulling hammer-player towards grapple-player if the hook connects with the hammer-player.
        else
        {
            if (hook != null && Physics.CheckSphere(hook.transform.position, 0.1f, playerMask))
            {
                hook.transform.position = playerHammer.transform.position;
            }

            if (Physics.CheckSphere(hook.transform.position, 0.1f, playerMask))
            {
                pullingHammer = true;
            }
            else
            {
                addForce = true;
            }
        }
    }
    
    /// <summary>
    /// Applies force onto the grapple or hammer-player.
    /// </summary>
    private void FixedUpdate()
    {
        if (pullingHammer)
        {
            rbHammer.AddForce((transform.GetChild(0).GetChild(1).position - hook.transform.position).normalized * (6.9f) * Mathf.Clamp(pullSpeed, minForce, maxForce), ForceMode.Impulse);
        }
        if (addForce)
        {
            rb.AddForce((hook.transform.position - transform.position).normalized* 3 * Mathf.Clamp(pullSpeed, minForce, maxForce), ForceMode.Impulse);
        }
        
    }

    /// <summary>
    /// Starts the pull and plays grapple hit audio effect.
    /// </summary>
    public void StartPull()
    {
        grapplingHit.Play();
        pulling = true;
    }

    /// <summary>
    /// Destroys the hook. 
    /// </summary>
    public void DestroyHook()
    {
        if (hook == null) return;

        grapplingHook.Stop();
        pulling = false;
        Destroy(hook.gameObject);
        hook = null;
    }
    
    /// <summary>
    /// Destroys hook after a certain amount of time.
    /// </summary>
    /// <returns>WaitForSeconds</returns>
    private IEnumerator DestroyHookAfterLifetime()
    {
        yield return new WaitForSeconds(8f);

        DestroyHook();
    }

    /// <summary>
    /// Plays the grapple audio effect.
    /// </summary>
    private void HookSound()
    {
        grapplingHook.Play();
    }

    /// <summary>
    /// Changes grapple cancel ability UI according to the boolean.
    /// </summary>
    /// <param name="on">Boolean</param>
    private void GrappleCancelUI(bool on)
    {
        if (on)
        {
            grappleCancelUIOn.SetActive(true);
            grappleCancelUIOff.SetActive(false);
        }
        else if (!on)
        {
            grappleCancelUIOn.SetActive(false);
            grappleCancelUIOff.SetActive(true);
        }
    }

    /// <summary>
    /// Changes grapple ability UI state according to the integer.
    /// </summary>
    /// <param name="number">Integer between 0 and 2 for three different states.</param>
    private void GrappleUI(int number)
    {
        if (number == 0)
        {
            grappleUIOn.SetActive(true);
            grappleUIOff.SetActive(false);
            grappleUIActive.SetActive(false);
        }
        else if (number == 1)
        {
            grappleUIOn.SetActive(false);
            grappleUIOff.SetActive(true);
            grappleUIActive.SetActive(false);
        }
        else if(number == 2)
        {
            grappleUIOn.SetActive(false);
            grappleUIOff.SetActive(false);
            grappleUIActive.SetActive(true);
        }
    }
}