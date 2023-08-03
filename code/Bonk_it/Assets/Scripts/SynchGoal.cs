using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynchGoal : MonoBehaviour
{
    private bool SynchGrapple = false;
    private bool SynchHammer = false;

    public GameObject scoreboard;

    private PauseMenu pauseMenu;

    /// <summary>
    /// Assigns pauseMenu to correct gameObject
    /// </summary>
    private void Start()
    {
        pauseMenu = GameObject.Find("GameController").GetComponent<PauseMenu>();
    }

    /// <summary>
    /// Checks if players enter activation zone
    /// </summary>
    /// <param name="other">Collider.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.name == GameObject.Find("PlayerGrapple").name)
            {
                SynchGrapple = true;
                SetTime();
            }

            if (other.name == GameObject.Find("PlayerHammer").name)
            {
                SynchHammer = true;
                SetTime();
            }
        }
    }

    /// <summary>
    /// Checks if players leave activation zone
    /// </summary>
    /// <param name="other">Collider.</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.name == GameObject.Find("PlayerGrapple").name)
            {
                SynchGrapple = false;
            }

            if (other.name == GameObject.Find("PlayerHammer").name)
            {
                SynchHammer = false;
            }
        }
    }

    /// <summary>
    /// Calls SetFinishTime() method in Scoreboard script
    /// </summary>
    private void SetTime()
    {
        if (SynchGrapple == true && SynchHammer == true)
        {
            pauseMenu.pauseMenuNotAvailable = true;
            scoreboard.GetComponent<Scoreboard>().SetFinishTime();
        }
    }
}