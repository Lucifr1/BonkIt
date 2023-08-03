using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTextSynchBorder : MonoBehaviour
{
    //Specialized TutorialText script for a text, where both players have to be present, for it to show

    [TextArea] public string Text;

    [SerializeField] GameObject arrows;

    [SerializeField] GameObject textFollowArrows;
    [SerializeField] GameObject textPressurePlate;

    private bool hammerIsHere = false;
    private bool grappleIsHere = false;

    private TutorialTextController tutorialTextController;

    /// <summary>
    /// Assigns tutorialTextController to the correct gameObject
    /// </summary>
    void Start()
    {
        tutorialTextController = GameObject.Find("GameController").GetComponent<TutorialTextController>();
    }

    /// <summary>
    /// Change textfield of both players to text, if both players enter activation zone
    /// </summary>
    void Update()
    {
        if (hammerIsHere && grappleIsHere)
        {
            Destroy(textFollowArrows);
            Destroy(textPressurePlate);
            tutorialTextController.SetTextGrapple(Text);
            tutorialTextController.SetTextHammer(Text);
            arrows.SetActive(false);
        }
    }

    /// <summary>
    /// Checks if players enter activation zone and changes booleans accordingly
    /// </summary>
    /// <param name="other">Collider.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.name == GameObject.Find("PlayerGrapple").name)
            {
                grappleIsHere = true;
            }

            if (other.name == GameObject.Find("PlayerHammer").name)
            {
                hammerIsHere = true;
            }
        }
    }

    /// <summary>
    /// Checks if players leave activation zone and changes booleans accordingly
    /// </summary>
    /// <param name="other">Collider.</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.name == GameObject.Find("PlayerGrapple").name)
            {
                grappleIsHere = false;
                if (tutorialTextController.GrappleTextBox.text == Text)
                {
                    tutorialTextController.SetTextGrapple("");
                }
            }

            if (other.name == GameObject.Find("PlayerHammer").name)
            {
                hammerIsHere = false;
                if (tutorialTextController.HammerTextBox.text == Text)
                {
                    tutorialTextController.SetTextHammer("");
                }
            }
        }
    }
}