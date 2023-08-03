using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class DoorButton : MonoBehaviour
{
    public bool playerGrappleInRadius = false;
    public bool playerHammerInRadius = false;

    bool OpeningDoor;
    private GameObject DoorMoveableBottom;
    private GameObject DoorMoveableTop;
    private Vector3 DoorScaleChange = new Vector3(0.0f, 0.02f, 0.0f);
    private Vector3 DoorPositionScaleChange = new Vector3(0.0f, 0.01f, 0.0f);

    [SerializeField] private GameObject pickUpGrapple;
    [SerializeField] private GameObject pickUpHammer;
    private GameObject Button;
    private GameObject ButtonHammer;
    private GameObject ButtonGrapple;
    private Vector3 ButtonScaleChange = new Vector3(0.0f, 0.1f, 0.0f);
    private bool ButtonIsPressed;
    private float CurrentCountdownTime;
    private float CountdownTime = 1.5f;

    //Raycast method
    [Header("Raycast")] [SerializeField] private Camera camHammer;
    [SerializeField] private Camera camGrapple;
    private GameObject RayobjectHammer;
    private GameObject RayobjectGrapple;
    private Vector3 origin;
    private Vector3 camdirection;
    private float maxraycast = 3f;

    //Door-Sound
    [Header("Sound")] [SerializeField] private AudioSource doorOpen;
    [SerializeField] private AudioSource button;

    [SerializeField] GameObject GrappleInteractUIon;
    [SerializeField] GameObject GrappleInteractUIoff;

    [SerializeField] GameObject HammerInteractUIon;
    [SerializeField] GameObject HammerInteractUIoff;

    /// <summary>
    /// Gets the first button-game object to avoid NullReferenceException.
    /// </summary>
    private void Start()
    {
        Button = transform.GetChild(0).GetChild(0).GetChild(1).gameObject;
    }

    /// <summary>
    /// Raycast method to able to interact with a button for both players. Changing UI interact ability state.
    /// </summary>
    void Update()
    {
        playerGrappleInRadius = false;
        playerHammerInRadius = false;

        //Hammer Raycast method
        if (!pickUpHammer.GetComponent<PickUpHammer>().isHoldingHammer)
        {
            origin = camHammer.transform.position;
            camdirection = camHammer.transform.forward;
            Ray ray = new Ray(origin, camdirection);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxraycast))
            {
                if (hit.collider.gameObject.CompareTag("DoorButton"))
                {
                    RayobjectHammer = hit.collider.gameObject;
                    ButtonHammer = hit.collider.gameObject;
                    playerHammerInRadius = true;
                    HammerAbilityUI(true);
                }
                else
                {
                    if (!pickUpHammer.GetComponent<PickUpHammer>().PickUpCubeInRadius)
                    {
                        HammerAbilityUI(false);
                    }
                }
            }
        }

        //Grapple Raycast method
        if (!pickUpGrapple.GetComponent<PickUpGrapple>().isHoldingGrapple)
        {
            origin = camGrapple.transform.position;
            camdirection = camGrapple.transform.forward;
            Ray ray = new Ray(origin, camdirection);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxraycast))
            {
                if (hit.collider.gameObject.CompareTag("DoorButton"))
                {
                    RayobjectGrapple = hit.collider.gameObject;
                    ButtonGrapple = hit.collider.gameObject;
                    playerGrappleInRadius = true;
                    GrappleAbilityUI(true);
                }
                else
                {
                    if (!pickUpGrapple.GetComponent<PickUpGrapple>().PickUpCubeInRadius)
                    {
                        GrappleAbilityUI(false);
                    }
                }
            }
        }

        CheckInput();
    }

    /// <summary>
    /// Button animation timer.
    /// </summary>
    private void FixedUpdate()
    {
        if (ButtonIsPressed)
        {
            CurrentCountdownTime += Time.deltaTime;
            ChangeDoor(true);
            if (CurrentCountdownTime > CountdownTime)
            {
                CurrentCountdownTime = 0;
                PressButton(false);
                ChangeDoor(false);
            }
        }
    }

    /// <summary>
    /// Checks the interact input of both players and opens the respective door.
    /// </summary>
    private void CheckInput()
    {
        //Grapple 
        if (playerGrappleInRadius && Input.GetButtonDown("InteractGrapple") && !ButtonIsPressed)
        {
            doorOpen.Play();
            button.Play();
            DoorMoveableBottom = RayobjectGrapple.transform.parent.parent.GetChild(1).GetChild(0).gameObject;
            DoorMoveableTop = RayobjectGrapple.transform.parent.parent.GetChild(1).GetChild(1).gameObject;
            Button = ButtonGrapple;
            PressButton(true);
            ChangeDoor(true);
            if (DoorMoveableBottom.transform.localScale.y > 0)
            {
                OpeningDoor = true;
            }
            else
            {
                OpeningDoor = false;
            }
        }

        //Hammer
        if (playerHammerInRadius && Input.GetButtonDown("InteractHammer") && !ButtonIsPressed)
        {
            doorOpen.Play();
            button.Play();
            DoorMoveableBottom = RayobjectHammer.transform.parent.parent.GetChild(1).GetChild(0).gameObject;
            DoorMoveableTop = RayobjectHammer.transform.parent.parent.GetChild(1).GetChild(1).gameObject;
            Button = ButtonHammer;
            PressButton(true);
            ChangeDoor(true);
            if (DoorMoveableBottom.transform.localScale.y > 0)
            {
                OpeningDoor = true;
            }
            else
            {
                OpeningDoor = false;
            }
        }
    }

    /// <summary>
    /// Opens door by changing its position and local scale.
    /// </summary>
    /// <param name="change">A boolean for opening/closing.</param>
    private void ChangeDoor(bool change)
    {
        if (OpeningDoor)
        {
            if (DoorMoveableBottom.transform.localScale.y > 0)
            {
                DoorMoveableBottom.transform.localScale -= DoorScaleChange;
                DoorMoveableBottom.transform.position -= DoorPositionScaleChange;

                DoorMoveableTop.transform.localScale -= DoorScaleChange;
                DoorMoveableTop.transform.position += DoorPositionScaleChange;
            }
        }

        if (!OpeningDoor)
        {
            if (DoorMoveableBottom.transform.localScale.y <= 1)
            {
                DoorMoveableBottom.transform.position += DoorPositionScaleChange;
                DoorMoveableBottom.transform.localScale += DoorScaleChange;

                DoorMoveableTop.transform.position -= DoorPositionScaleChange;
                DoorMoveableTop.transform.localScale += DoorScaleChange;
            }
        }
    }

    /// <summary>
    /// Button animation.
    /// </summary>
    /// <param name="up">Boolean for button going down/up.</param>
    private void PressButton(bool up)
    {
        if (up)
        {
            if (!ButtonIsPressed)
            {
                Button.transform.localScale -= ButtonScaleChange;
                ButtonIsPressed = true;
            }
        }

        if (!up)
        {
            Button.transform.localScale += ButtonScaleChange;
            ButtonIsPressed = false;
        }
    }

    /// <summary>
    /// Changes state of the grapple interact UI.
    /// </summary>
    /// <param name="on">Boolean for on/off.</param>
    private void GrappleAbilityUI(bool on)
    {
        if (on)
        {
            GrappleInteractUIon.SetActive(true);
            GrappleInteractUIoff.SetActive(false);
        }
        else if (!on)
        {
            GrappleInteractUIon.SetActive(false);
            GrappleInteractUIoff.SetActive(true);
        }
    }

    /// <summary>
    /// Changes state of the hammer interact UI.
    /// </summary>
    /// <param name="on">Boolean for on/off.</param>
    private void HammerAbilityUI(bool on)
    {
        if (on)
        {
            HammerInteractUIon.SetActive(true);
            HammerInteractUIoff.SetActive(false);
        }
        else if (!on)
        {
            HammerInteractUIon.SetActive(false);
            HammerInteractUIoff.SetActive(true);
        }
    }
}