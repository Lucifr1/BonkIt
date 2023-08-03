using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHint : MonoBehaviour
{
    [SerializeField] GameObject hint;

    /// <summary>
    /// Activates hint, when grapple-player enters activation zone
    /// </summary>
    /// <param name="other">Collider.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "PlayerGrapple")
        {
            hint.SetActive(true);
        }
    }

    /// <summary>
    /// Deactivates hint, when grapple-player leaves activation zone
    /// </summary>
    /// <param name="other">Collider.</param>
    private void OnTriggerExit(Collider other)
    {
        if(other.name == "PlayerGrapple")
        {
            hint.SetActive(false);
        }
    }
}