using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class SynchBorder : MonoBehaviour
{
    [SerializeField] private GameObject hammerPlayer;
    [SerializeField] private GameObject grapplePlayer;

    private Vector3 closestPointHammer;
    private Vector3 closestPointGrapple;
    private float distanceHammer;
    private float distanceGrapple;

    private GameObject borderwall;

    private bool SynchGrapple = false;
    private bool SynchHammer = false;
    
    //Synchro text field "1/2" Players here
    [SerializeField] private GameObject playerCount;
    
    //Audio
    [SerializeField] private AudioSource energyField;
    
    /// <summary>
    /// Assigns borderwall to correct gameobject
    /// </summary>
    private void Start()
    {
        borderwall = transform.parent.parent.GetChild(0).gameObject;
    }

    /// <summary>
    /// Adjusts playerCount; calls CheckDistance()
    /// </summary>
    private void Update()
    {
        if (SynchHammer && !SynchGrapple)
        {
            playerCount.SetActive(true);
        }
        else if (!SynchHammer && SynchGrapple)
        {
            playerCount.SetActive(true);
        }
        else
        {
            playerCount.SetActive(false);
        }

        CheckDistance();
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
                DestroyWall();
            }

            if (other.name == GameObject.Find("PlayerHammer").name)
            {
                SynchHammer = true;
                DestroyWall();
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
    /// Destroys wall; Stops audio; Deactivates playerCount
    /// </summary>
    private void DestroyWall()
    {
        if (SynchGrapple == true && SynchHammer == true)
        {
            energyField.Stop();
            playerCount.SetActive(false);
            Destroy(transform.parent.parent.GetChild(0).gameObject);
            Destroy(transform.parent.gameObject);
        }
    }

    /// <summary>
    /// Calculates the distance of both players to the border; Enables border on certain range
    /// </summary>
    private void CheckDistance()
    {
        closestPointHammer = borderwall.GetComponent<Collider>().ClosestPoint(hammerPlayer.transform.position);
        closestPointGrapple = borderwall.GetComponent<Collider>().ClosestPoint(grapplePlayer.transform.position);

        distanceHammer = Vector3.Distance(hammerPlayer.transform.position, closestPointHammer);
        distanceGrapple = Vector3.Distance(grapplePlayer.transform.position, closestPointGrapple);
        
        if (distanceGrapple < 20 || distanceHammer < 20)
        {
            borderwall.GetComponent<MeshRenderer>().enabled = true;
            energyField.Play();
        }
        else
        {
            borderwall.GetComponent<MeshRenderer>().enabled = false;
            energyField.Stop();
        }
    }
}