using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    //Placed on individual trigger boxes

    [TextArea] public string Text;

    public bool OnlyGrapple;
    public bool OnlyHammer;

    private TutorialTextController tutorialTextController;

    /// <summary>
    /// Assigns tutorialTextController to the correct gameObject
    /// </summary>
    private void Start()
    {
        tutorialTextController = GameObject.Find("GameController").GetComponent<TutorialTextController>();
    }

    /// <summary>
    /// Checks if players enter activation zone and displays the respective text in the respective text fields, through SetTextGrapple() and SetTextHammer() methods
    /// </summary>
    /// <param name="other">Collider.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.name == GameObject.Find("PlayerGrapple").name)
            {
                if (!OnlyHammer)
                {
                    tutorialTextController.SetTextGrapple(Text);
                }
            }
        }

        if (other.name == GameObject.Find("PlayerHammer").name)
        {
            if (!OnlyGrapple)
            {
                 tutorialTextController.SetTextHammer(Text);
            }
        }
    }

    /// <summary>
    /// Checks if players leave activation zone and empties the respective textfield, through SetTextGrapple() and SetTextHammer() methods
    /// </summary>
    /// <param name="other">Collider.</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.name == GameObject.Find("PlayerGrapple").name)
            {
                if (tutorialTextController.GrappleTextBox.text == Text)
                {
                    tutorialTextController.SetTextGrapple("");
                }
            }

            if (other.name == GameObject.Find("PlayerHammer").name)
            {
                if (tutorialTextController.HammerTextBox.text == Text)
                {
                    tutorialTextController.SetTextHammer("");
                }
            }
        }
    }
}