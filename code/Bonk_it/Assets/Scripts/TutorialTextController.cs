using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialTextController : MonoBehaviour
{
    public TextMeshProUGUI GrappleTextBox;
    public TextMeshProUGUI HammerTextBox;

    /// <summary>
    /// Fills the Tutorial-text-field of the grapple-player with the string parameter
    /// </summary>
    /// <param name="text">contains respective strings of the hitboxes in tutorial.</param>
    public void SetTextGrapple(string text)
    {
        GrappleTextBox.text = text;
    }

    /// <summary>
    /// Fills the Tutorial-text-field of the hammer-player with the string parameter
    /// </summary>
    /// <param name="text">contains respective strings of the hitboxes in tutorial.</param>
    public void SetTextHammer(string text)
    {
        HammerTextBox.text = text;
    }
}