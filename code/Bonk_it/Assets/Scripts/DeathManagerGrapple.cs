using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathManagerGrapple : MonoBehaviour
{
    public Vector3 LatestCheckpoint = new Vector3(0, 0, 0);

    Rigidbody rb;
    private int random;
    private bool cr_running;

    [Header("Audio")] 
    [SerializeField] private AudioSource wilhelmScream;
    [SerializeField] private AudioSource deathSound;
    
    /// <summary>
    /// Access player's rigidbody.
    /// </summary>
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Teleports player to the latest checkpoint if he dies.
    /// </summary>
    void Update()
    {
        if (IsDead() && !cr_running)
        {
            TpToLatestCheckPoint();
        }
    }

    /// <summary>
    /// Starts a coroutine for the death audio.
    /// </summary>
    void TpToLatestCheckPoint()
    {
        StartCoroutine(DeathSound());
    }

    /// <summary>
    /// Plays the death sound, delays the teleport for the length of the sound. Adding a randomizer to play a special sound effect.
    /// </summary>
    /// <returns> WaitsForSeconds </returns>
    private IEnumerator DeathSound()
    {
        cr_running = true;
        random = Random.Range(0, 10);
        Debug.Log(random);
        if (random == 7)
        {
            wilhelmScream.Play();
            yield return new WaitForSeconds(0.6f);
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            transform.position = LatestCheckpoint;
            cr_running = false;
        }
        else
        {
            deathSound.Play();
            yield return new WaitForSeconds(0.6f);
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            transform.position = LatestCheckpoint;
            cr_running = false;
        }
    }
    
    /// <summary>
    /// Checks if the player is dead by inspecting the y-postion.
    /// </summary>
    /// <returns>Boolean for player death status. </returns>
    bool IsDead()
    {
        if (transform.position.y <= -25)
        {
            return true;
        }
        return false;
    }
}
