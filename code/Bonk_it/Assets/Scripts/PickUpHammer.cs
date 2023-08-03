using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class PickUpHammer : MonoBehaviour
{
    private GameObject PickUpCube;
    private GameObject tempCube;

    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Camera;
    [SerializeField] private float shootpower = 3000;

    public bool PickUpCubeInRadius = false;
    public bool isHoldingHammer = false;

    private float distance;
    private Vector3 objectPos;
    private float yPos;
    private float maxdistance = 5.0f;

    //Cube y-position
    private float startCoordinateY;
    private float currentCoordinateY;
    private float difference;

    //Audio
    [Header("Sound")]
    [SerializeField] private AudioSource explosionSchuss;

    //Raycast method
    [Header("Raycast")]
    [SerializeField]private Camera cam;
    private GameObject rayobject;
    private Vector3 origin;
    private Vector3 direction;
    private Color tintColor = Color.green;
    private float maxraycast=3.5f;
    private bool colourchange;
    //Materials
    [SerializeField] private Material glow;
    [SerializeField] private Material glowTransparent;
    [SerializeField] private Material standard;

    [SerializeField] GameObject hammerHitRadius;
    [SerializeField] GameObject hammerAnimationScriptPlace;

    [Header("UI abilities")]
    //Interact UI
    [SerializeField] GameObject interactUIOn;
    [SerializeField] GameObject interactUIOff;
    [SerializeField] GameObject interactUIActive;
    //Hammer Shoot UI
    [SerializeField] GameObject HammerShootUIOn;
    [SerializeField] GameObject HammerShootUIOff;

    [SerializeField] GameObject DoorButton;

    /// <summary>
    /// Adjusts UI Interact ability; Raycast method for picking up small cubes; calls CheckInputHammer() method.
    /// </summary>
    void Update()
    {
        //Interact UI
        if (!isHoldingHammer)
        {
            if (PickUpCubeInRadius)
            {
                AbilityInteractUI(0);
            }
            else if (!PickUpCubeInRadius)
            {
                if (!DoorButton.GetComponent<DoorButton>().playerHammerInRadius)
                {
                    AbilityInteractUI(1);
                }
            }
        }
        else if (isHoldingHammer)
        {
            AbilityInteractUI(2);
        }

        //HammerShootUI
        if (isHoldingHammer == true)
        {
            AbilityHammerShootUI(true);
        }
        else
        {
            AbilityHammerShootUI(false);
        }

        //removes highlight when not looking at cube
        if (colourchange)
        {
            rayobject.GetComponent<Renderer>().material = standard;
            colourchange = false;
        }

        //Raycast method
        if (!isHoldingHammer)
        {
            origin = cam.transform.position;
            direction = cam.transform.forward;
            Ray ray = new Ray(origin, direction);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxraycast))
            {
                if (hit.collider.gameObject.CompareTag("PickUpAble"))
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

        CheckInputHammer();
    }

    /// <summary>
    /// Cube's y-position is adjusted when player jumps; Freezes cube's velocity/ angularvelocity to avoid cube floating around;
    /// Drops cube under certain conditions
    /// </summary>
    private void FixedUpdate()
    {
        if (isHoldingHammer == true)
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
                if (plane.GetDistanceToPoint(point) < -0)
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
    /// Checks input of the hammer-player; Pick-up cube: fixates cube's position with player's position, sets gravity usage to false;
    /// Drops cube with interact input button; Shoots cube with hammer ability input
    /// </summary>
    private void CheckInputHammer()
    {
        //Pick-up cube
        if (Input.GetButtonDown("InteractHammer") && PickUpCubeInRadius && isHoldingHammer == false)
        {
            PickUpCube = tempCube;
            isHoldingHammer = true;
            PickUpCube.transform.SetParent(Camera.transform);
            PickUpCube.GetComponent<Rigidbody>().useGravity = false;
            PickUpCube.GetComponent<Rigidbody>().detectCollisions = true;
            startCoordinateY = Player.transform.position.y;
        }

        //Drop cube (interact button input)
        else if (Input.GetButtonDown("InteractHammer") && isHoldingHammer == true)
        {
            loslassen();
        }
        
        //shoot cube (hammer ability input)
        if (Input.GetButtonDown("LinksklickHammer") && isHoldingHammer == true)
        {
            hammerAnimationScriptPlace.GetComponent<HammerAnimation>().HammerShoot();
            StartCoroutine(WaitHammerShoot());
        }
    }

    /// <summary>
    /// Plays audio and adds force after a certain amount of time
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitHammerShoot()
    {
       yield return new WaitForSeconds(0.12f);
       explosionSchuss.Play();
       PickUpCube.GetComponent<Rigidbody>().AddForce(Player.transform.GetChild(0).forward*shootpower);
       loslassen();
    }

    /// <summary>
    /// Drop cube method, reverts all pick-up changes and adjusts cube's colour
    /// </summary>
    private void loslassen()
    {
        isHoldingHammer = false;
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
    public void AbilityInteractUI(int number)
    {
        if (number == 0)
        {
            interactUIOn.SetActive(true);
            interactUIOff.SetActive(false);
            interactUIActive.SetActive(false);
        }
        else if (number == 1)
        {
            interactUIOn.SetActive(false);
            interactUIOff.SetActive(true);
            interactUIActive.SetActive(false);
        }
        else if (number == 2)
        {
            interactUIOn.SetActive(false);
            interactUIOff.SetActive(false);
            interactUIActive.SetActive(true);
        }
    }

    /// <summary>
    /// Changes hammer shoot ability UI state according to the bool.
    /// </summary>
    /// <param name="on">Boolean.</param>
    public void AbilityHammerShootUI(bool on)
    {
        if (on)
        {
            HammerShootUIOn.SetActive(true);
            HammerShootUIOff.SetActive(false);
        }
        else if (!on)
        {
            HammerShootUIOn.SetActive(false);
            HammerShootUIOff.SetActive(true);
        }
    }
}