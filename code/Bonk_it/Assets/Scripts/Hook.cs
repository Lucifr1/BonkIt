using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    [SerializeField] float hookForce = 5f;

    Grapple grapple;
    Rigidbody rb;
    LineRenderer lineRenderer;
    
    /// <summary>
    /// Initialize hook being the object in the front and a line renderer as the visualisation of the hook.
    /// </summary>
    /// <param name="grapple">Grapple object.</param>
    /// <param name="shootTransform">Shoot transform position where the hook is spawned.</param>
    public void Initialize(Grapple grapple, Transform shootTransform)
    {
        transform.forward = shootTransform.forward;
        this.grapple = grapple;
        rb = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();
        rb.AddForce(transform.forward * hookForce, ForceMode.Impulse);

    }

    /// <summary>
    /// Adjusting necessary attributes of the hook's rigidbody.
    /// </summary>
    private void Start()
    {
        rb.useGravity = false;
        rb.detectCollisions = true;
    }

    /// <summary>
    /// Adjusting line renderer in between the hook and the player.
    /// </summary>
    void Update()
    {
        Vector3[] positions = new Vector3[]
           {
                transform.position,
                grapple.transform.position
           };
           
        lineRenderer.SetPositions(positions);
    }

    /// <summary>
    /// When the hook collides with hookable object: accesses grapple script to start the pull.
    /// </summary>
    /// <param name="other">Collider which collides with the hook's hitbox.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Grapple") || (other.gameObject.layer == 8))
        {
            transform.position = other.transform.position;
            rb.useGravity = false;
            rb.isKinematic = true;

            grapple.StartPull();
        }
    }
}
