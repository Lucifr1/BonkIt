using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioListener : MonoBehaviour
{
    //Positionen der Kameras
    [SerializeField] private GameObject camGrapple;
    [SerializeField] private GameObject camHammer;

    /// <summary>
    /// Sets audio listener position in between the two players. 
    /// </summary>
    void Update()
    {
        transform.position = (camGrapple.transform.position + camHammer.transform.position) / 2;
    }
}
