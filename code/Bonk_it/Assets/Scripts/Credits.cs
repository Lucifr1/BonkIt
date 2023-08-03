using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Credits : MonoBehaviour
{
    [SerializeField] GameObject CreditsText;
    [SerializeField] float yMoveFactor;

    [SerializeField] Vector3 CreditsStartingPoint;
    [SerializeField] Vector3 CreditsStopPoint;
    private bool CreditsActive = false;
    
    /// <summary>
    /// Corrects the starting point position for the credits.
    /// </summary>
    public void Start()
    {
        CreditsStartingPoint += new Vector3(960, 540, 0);
        CreditsStopPoint += new Vector3(960, 540, 0);
    }

    /// <summary>
    /// Activates/Stops the credits using a bool.
    /// </summary>
    /// <param name="on">Boolean</param>
    public void CreditsTextController(bool on)
    {
        if (on)
        {
            CreditsActive = true;
        }

        if (!on)
        {
            CreditsText.transform.position = CreditsStartingPoint;
            CreditsActive = false;
        }
    }

    /// <summary>
    /// Moves the credits according to the state of the boolean.
    /// </summary>
    private void FixedUpdate()
    {
        if (CreditsActive && (CreditsText.transform.position.y <= CreditsStopPoint.y))
        {
            CreditsText.transform.position += new Vector3(0f, yMoveFactor, 0f);
        }
    }
}
