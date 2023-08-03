using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private DeathManagerGrapple DeathManagerGrapple;
    private DeathManagerHammer DeathManagerHammer;

    //CheckpointText(Grapple and Hammer) has to be on every checkpoint-object!
    [SerializeField] GameObject CheckpointUITextGrapple;
    [SerializeField] GameObject CheckpointUITextHammer;

    private bool CheckpointUIGrapple = false;
    private bool CheckpointUIHammer = false;
    private float CountdownTime = 3f;
    private float CurrentCountdownTimeGrapple = 0f;
    private float CurrentCountdownTimeHammer = 0f;

    /// <summary>
    /// Access Death Manager (Grapple and Hammer) scripts from the players` bodies.
    /// </summary>
    private void Start()
    {
        DeathManagerGrapple = GameObject.Find("PlayerGrapple").GetComponent<DeathManagerGrapple>();
        DeathManagerHammer = GameObject.Find("PlayerHammer").GetComponent<DeathManagerHammer>();
    }

    /// <summary>
    /// Sets Checkpoint-UI timer.
    /// </summary>
    private void Update()
    {
        //Timer 
        if (CheckpointUIGrapple)
        {
            CurrentCountdownTimeGrapple += Time.deltaTime;
            if (CurrentCountdownTimeGrapple > CountdownTime)
            {
                CurrentCountdownTimeGrapple = 0;
                CheckpointUIGrapple = false;
                CheckpointUITextGrapple.SetActive(false);
            }
        }

        if (CheckpointUIHammer)
        {
            CurrentCountdownTimeHammer += Time.deltaTime;
            if (CurrentCountdownTimeHammer > CountdownTime)
            {
                CurrentCountdownTimeHammer = 0;
                CheckpointUIHammer = false;
                CheckpointUITextHammer.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Detects collision with players. Sets current checkpoint to latest checkpoint variable from each player.
    /// </summary>
    /// <param name="other"> Collider which collides with the checkpoint. </param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //PlayerGrapple
            if(other.name == GameObject.Find("PlayerGrapple").name)
            {
                if(DeathManagerGrapple.LatestCheckpoint != transform.position)
                {
                    DeathManagerGrapple.LatestCheckpoint = transform.position;
                    CheckpointUIGrapple = true;
                    CheckpointUITextGrapple.SetActive(true);
                }
            }
            //PlayerHammer
            else if (other.name == GameObject.Find("PlayerHammer").name)
            {
                if (DeathManagerHammer.LatestCheckpoint != transform.position)
                {
                    DeathManagerHammer.LatestCheckpoint = transform.position;
                    CheckpointUIHammer = true;
                    CheckpointUITextHammer.SetActive(true);
                }
            }
        }
    }
}
