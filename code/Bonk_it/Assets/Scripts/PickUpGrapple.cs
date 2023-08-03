using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpGrapple : MonoBehaviour
{
    private GameObject PickUpCube;
    private GameObject tempCube;

    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Camera;
    [SerializeField] private GameObject hammerScriptPlace;

    public bool PickUpCubeInRadius = false;
    public bool isHoldingGrapple = false;

    private float distance;
    private Vector3 objectPos;
    private float yPos;
    private float maxdistance = 5.0f;

    //Cube y-position
    private float startCoordinateY;
    private float currentCoordinateY;
    private float difference;
    
    //Raycast method
    [Header ("Raycast")]
    [SerializeField]private Camera cam;
    private GameObject rayobject;
    private Vector3 origin;
    private Vector3 direction;
    private float maxraycast = 3.5f;
    private bool colourchange;
    //Materials
    [SerializeField] private Material glow;
    [SerializeField] private Material glowTransparent;
    [SerializeField] private Material standard;

    //Interact UI
    [Header("Interact UI")]
    [SerializeField] GameObject interactUIOn;
    [SerializeField] GameObject interactUIOff;
    [SerializeField] GameObject InteractUIActive;

    [SerializeField] GameObject DoorButton;

    /// <summary>
    /// Adjusts UI Interact ability; Raycast method for picking up small cubes; calls CheckInputGrapple() method.
    /// </summary>
    void Update()
    {
        //Interact UI
        if (!isHoldingGrapple)
        {
            if (PickUpCubeInRadius)
            {
                AbilityUI(0);
            }
            else if(!PickUpCubeInRadius)
            {
                if (!DoorButton.GetComponent<DoorButton>().playerGrappleInRadius)
                {
                    AbilityUI(1);
                }
            }
        }
        else if (isHoldingGrapple)
        {
            AbilityUI(2);
        }
        
        //removes highlight when not looking at cube
        if (colourchange)
        {
            rayobject.GetComponent<Renderer>().material = standard;
            colourchange = false;
        }

        //Raycast method
        if (!isHoldingGrapple)
        {
            origin = cam.transform.position;
            direction = cam.transform.forward;
            Ray ray = new Ray(origin, direction);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxraycast))
            {
                if (hit.collider.gameObject.CompareTag("PickUpAble") && !hammerScriptPlace.GetComponent<Hammer>().PlayerIsSmall)
                {
                    rayobject = hit.collider.gameObject;
                    rayobject.GetComponent<Renderer>().material = glow;
                    tempCube = rayobject;
                    PickUpCubeInRadius = true;
                    colourchange = true;
                }
                else
                {
                    PickUpCubeInRadius = false;
                }
            }
            else
            {
                PickUpCubeInRadius = false;
            }
        }

        CheckInputGrapple();
    }

    /// <summary>
    /// Cube's y-position is adjusted when player jumps; Freezes cube's velocity/ angularvelocity to avoid cube floating around;
    /// Drops cube under certain conditions
    /// </summary>
    private void FixedUpdate()
    {
        if (isHoldingGrapple == true)
        {
            //Adjusting of cube's y-position
            currentCoordinateY = Player.transform.position.y;
            difference = currentCoordinateY - startCoordinateY;
            PickUpCube.transform.position += new Vector3(0, difference, 0);
            startCoordinateY = Player.transform.position.y;

            //Freeze cube's velocity/ angularvelocity
            PickUpCube.GetComponent<Rigidbody>().velocity = Vector3.zero;
            PickUpCube.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            PickUpCube.GetComponent<Renderer>().material = glowTransparent;

            //Checks if cube is in field of view and drops it otherwise
            var planes = GeometryUtility.CalculateFrustumPlanes(cam);
            var point = PickUpCube.transform.position;
            foreach (var plane in planes)
            {
                if (plane.GetDistanceToPoint(point) < 0)
                {
                    loslassen();
                }
            }

            //calculates distance beetween player and cube and drops the cube if distance is too great
            distance = Vector3.Distance(PickUpCube.transform.position, Player.transform.position);

            if (distance > maxdistance)
            {
                loslassen();
            }
        }
    }

    /// <summary>
    /// Checks input of the grapple-player; Pick-up cube: fixates cube's position with player's position, sets gravity usage to false;
    /// Drops cube with interact and grapple ability input buttons 
    /// </summary>
    private void CheckInputGrapple()
    {
        //Pick-up cube
        if (Input.GetButtonDown("InteractGrapple") && PickUpCubeInRadius && isHoldingGrapple == false && !hammerScriptPlace.GetComponent<Hammer>().PlayerIsSmall)
        {
            PickUpCube = tempCube;
            isHoldingGrapple = true;
            PickUpCube.transform.SetParent(Camera.transform);
            PickUpCube.GetComponent<Rigidbody>().useGravity = false;
            PickUpCube.GetComponent<Rigidbody>().detectCollisions = true;
            startCoordinateY = Player.transform.position.y;
        }
        
        //Drop cube (interact button input)
        else if (Input.GetButtonDown("InteractGrapple") && isHoldingGrapple == true)
        {
            loslassen();
        }

        //Drop cube (grapple ability input)
        if (Input.GetButtonDown("LinksklickGrapple") && isHoldingGrapple == true)
        {
            loslassen();
        }
    }

    /// <summary>
    /// Drop cube method, reverts all pick-up changes and adjusts cube's colour
    /// </summary>
    public void loslassen()
    {
        isHoldingGrapple = false;
        objectPos = PickUpCube.transform.position;
        PickUpCube.transform.SetParent(null);
        PickUpCube.GetComponent<Rigidbody>().useGravity = true;
        PickUpCube.transform.position = objectPos;
        PickUpCube.GetComponent<Renderer>().material = standard;
    }

    /// <summary>
    /// Changes interact ability UI state according to the integer.
    /// </summary>
    /// <param name="number">Integer between 0 and 2 for three different states.</param>
    public void AbilityUI(int number)
    {
        if (number == 0)
        {
            interactUIOn.SetActive(true);
            interactUIOff.SetActive(false);
            InteractUIActive.SetActive(false);
        }
        else if (number == 1)
        {
            interactUIOn.SetActive(false);
            interactUIOff.SetActive(true);
            InteractUIActive.SetActive(false);
        }
        else if (number == 2)
        {
            interactUIOn.SetActive(false);
            interactUIOff.SetActive(false);
            InteractUIActive.SetActive(true);
        }
    }
}
