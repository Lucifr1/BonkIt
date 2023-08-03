using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialSynchGoal : MonoBehaviour
{
    private bool SynchGrapple = false;
    private bool SynchHammer = false;

    [SerializeField] GameObject TutorialDoneUI;
    [SerializeField] GameObject TutorialGoalFirstButton;

    private PauseMenu pauseMenu;

    /// <summary>
    /// Assigns pauseMenu to the correct gameObject
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
                TutorialFinished();
            }

            if (other.name == GameObject.Find("PlayerHammer").name)
            {
                SynchHammer = true;
                TutorialFinished();
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
    /// Activates tutorial finish UI screen; Timescale adjusted; Sets pauseMenuNotAvailable = true
    /// </summary>
    private void TutorialFinished()
    {
        if (SynchGrapple == true && SynchHammer == true)
        {
            pauseMenu.pauseMenuNotAvailable = true;
            TutorialDoneUI.SetActive(true);
            Time.timeScale = 0f;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(TutorialGoalFirstButton);
        }
    }
}