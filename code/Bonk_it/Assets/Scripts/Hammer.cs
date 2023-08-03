using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class Hammer : MonoBehaviour
{
    [SerializeField] GameObject hammerPlayer;
    [SerializeField] GameObject playerSmallUIActive;
    [SerializeField] private GameObject pickUpGrapple;

    [SerializeField] Vector3 scaleChange;


    //create small objects
    public GameObject smallDestroyedPrefab;
    private Quaternion smallDestroyedRotation;

    private Vector3 smallDestroyedPosition;

    //Door Hammer
    private GameObject EmptyDoor;
    Vector3 direction = new Vector3(-0.2f, 0, 0);

    [SerializeField] GameObject grapplePlayer;
    private float CountdownTime = 10f;
    private float CurrentCountdownTime = 0f;
    public bool PlayerIsSmall;

    [Header("Sound")] [SerializeField] private AudioSource bonkSound;
    [SerializeField] private AudioSource hammerSwing;

    private GameObject DestructableObject;
    private GameObject DestructableDoor;
    private bool DestructableInRadius;
    private bool DestructableDoorInRadius;
    private bool PlayerInRadius;
    private float CollissionCounter = 0;

    [SerializeField] GameObject AbilityUIOn;
    [SerializeField] GameObject AbilityUIOff;

    [SerializeField] GameObject pickUpHammer;

    [SerializeField] GameObject hammerAnimationScriptPlace;

    //Timer
    [Header("GrapplePlayerSmall Timer")] [SerializeField]
    TextMeshProUGUI TimerGrapplePlayerSmall;

    private float currentTimertime;

    /// <summary>
    /// Timer for grapple-player being small and call CheckInput-method.
    /// </summary>
    void Update()
    {
        if (PlayerIsSmall)
        {
            CurrentCountdownTime += Time.deltaTime;
            if (CurrentCountdownTime > CountdownTime)
            {
                CurrentCountdownTime = 0;
                PlayerIsSmall = false;
                ShrinkPlayer();
                playerSmallUIActive.SetActive(false);
                if (CollissionCounter == 1)
                {
                    AbilityUI(true);
                }
            }
        }

        if (PlayerIsSmall)
        {
            TimerGrapplePlayerSmall.gameObject.SetActive(true);
            currentTimertime -= Time.deltaTime;

            if (currentTimertime > 0)
            {
                TimerGrapplePlayerSmall.text = TimeSpan.FromSeconds(currentTimertime).ToString("ss");
            }
            else if (currentTimertime <= 0)
            {
                TimerGrapplePlayerSmall.gameObject.SetActive(false);
            }
        }

        CheckInput();
    }

    /// <summary>
    /// OnTriggerEnter: Checks if the colliding object is interactable for the hammer-player.
    /// </summary>
    /// <param name="other">Collider which collides with the hammer's hitbox.</param>
    void OnTriggerEnter(Collider other)
    {
        //Detect Destructable Object
        if (other.tag == "Destructable")
        {
            DestructableInRadius = true;
            DestructableObject = other.gameObject;
            AbilityUI(true);
        }

        //Detect Destructable Door
        if (other.tag == "DestructableDoor")
        {
            DestructableDoorInRadius = true;
            DestructableDoor = other.gameObject;
            EmptyDoor = other.transform.parent.gameObject;
            AbilityUI(true);
        }

        //Detect Player
        if (other.name == "PlayerGrapple")
        {
            CollissionCounter++;
            if (!PlayerIsSmall)
            {
                AbilityUI(true);
            }
        }
    }

    /// <summary>
    /// OnTriggerExit: Checks if the colliding object is not interactable anymore for the hammer-player.
    /// </summary>
    /// <param name="other">Collider which collides with the hammer's hitbox.</param>
    void OnTriggerExit(Collider other)
    {
        //Detect Destructable Object
        if (other.tag == "Destructable")
        {
            DestructableInRadius = false;
            AbilityUI(false);
        }

        //Detect Destructable Door
        if (other.tag == "DestructableDoor")
        {
            DestructableDoorInRadius = false;
            AbilityUI(false);
        }

        //Detect Player
        if (other.name == "PlayerGrapple")
        {
            CollissionCounter--;
            AbilityUI(false);
        }
    }

    /// <summary>
    /// Creates small cubes.
    /// </summary>
    public void CreateSmallGameDestroyed()
    {
        int quantity = (int) (((DestructableObject.transform.localScale.x) *
                               (DestructableObject.transform.localScale.y) *
                               (DestructableObject.transform.localScale.z) * 5));

        for (int i = 0; i < quantity; i++)
        {
            float xVariance = UnityEngine.Random.Range(
                DestructableObject.transform.position.x - (DestructableObject.transform.localScale.x) / 2,
                DestructableObject.transform.position.x + (DestructableObject.transform.localScale.x) / 2);
            float zVariance = UnityEngine.Random.Range(
                DestructableObject.transform.position.z - (DestructableObject.transform.localScale.z) / 2,
                DestructableObject.transform.position.z + (DestructableObject.transform.localScale.z) / 2);
            smallDestroyedPosition = new Vector3(xVariance, DestructableObject.transform.position.y, zVariance);
            smallDestroyedRotation = DestructableObject.transform.rotation;
            GameObject newSmallDestroyed =
                Instantiate(smallDestroyedPrefab, smallDestroyedPosition, smallDestroyedRotation);
        }
    }

    /// <summary>
    /// Checks Input and performs different actions according to the collider object in range. Playing sound effects (Swing + Bonk)
    /// </summary>
    public void CheckInput()
    {
        if (Input.GetButtonDown("LinksklickHammer"))
        {
            hammerSwing.Play();
        }

        if (!pickUpHammer.GetComponent<PickUpHammer>().isHoldingHammer)
        {
            if (Input.GetButtonDown("LinksklickHammer"))
            {
                hammerAnimationScriptPlace.GetComponent<HammerAnimation>().HammerStrike();
            }

            //Action: Destructable Object: create small objects
            if (Input.GetButtonDown("LinksklickHammer") && DestructableInRadius)
            {
                print("Linksklick & DestructableInRadius & create small objects");
                Destroy(DestructableObject);
                CreateSmallGameDestroyed();
                DestructableInRadius = false;
                AbilityUI(false);
            }

            //Action: Destructable Door
            //Door opens only in one direction, so correct placement is needed.
            if (Input.GetButtonDown("LinksklickHammer") && DestructableDoorInRadius)
            {
                direction = new Vector3(-0.2f, 0, 0);
                DestructableDoor.tag = "DestroyedDoor";
                DestructableDoorInRadius = false;
                direction = Quaternion.Euler(0, DestructableDoor.transform.rotation.eulerAngles.y, 0) * direction;
                Rigidbody rbDoor = DestructableDoor.GetComponent<Rigidbody>();
                rbDoor.isKinematic = false;
                rbDoor.AddForce(direction.normalized * 30, ForceMode.Impulse);
                AbilityUI(true);
            }

            //Action: Shrink Player
            if (Input.GetButtonDown("LinksklickHammer") && (CollissionCounter == 1) && !PlayerIsSmall)
            {
                bonkSound.time = 0.5f;
                bonkSound.Play();
                PlayerIsSmall = true;
                ShrinkPlayer();
                playerSmallUIActive.SetActive(true);
                AbilityUI(false);
                currentTimertime = 10;
            }
        }
    }

    /// <summary>
    /// Shrinks player, adjusting local scale, collider positioning and jump height.
    /// </summary>
    private void ShrinkPlayer()
    {
        if (PlayerIsSmall)
        {
            if (pickUpGrapple.GetComponent<PickUpGrapple>().isHoldingGrapple)
            {
                pickUpGrapple.GetComponent<PickUpGrapple>().loslassen();
            }

            grapplePlayer.transform.localScale += scaleChange;
            grapplePlayer.GetComponent<CapsuleCollider>().center = new Vector3(0, 1.37f, 0.16f);
            grapplePlayer.GetComponent<CapsuleCollider>().radius = 0.7f;
            grapplePlayer.GetComponent<CapsuleCollider>().direction = 2;
            grapplePlayer.GetComponent<PlayerMovementGrapple>().jumpHeight = 8f;
        }
        else if (!PlayerIsSmall)
        {
            grapplePlayer.transform.localScale -= scaleChange;
            grapplePlayer.GetComponent<CapsuleCollider>().center = new Vector3(0, 0.46f, 0);
            grapplePlayer.GetComponent<CapsuleCollider>().radius = 1.12f;
            grapplePlayer.GetComponent<CapsuleCollider>().direction = 1;
            grapplePlayer.GetComponent<PlayerMovementGrapple>().jumpHeight = 6f;
        }
    }

    /// <summary>
    /// Changing hammer ability UI according to the boolean.
    /// </summary>
    /// <param name="on">Boolean.</param>
    private void AbilityUI(bool on)
    {
        if (on)
        {
            AbilityUIOn.SetActive(true);
            AbilityUIOff.SetActive(false);
        }
        else if (!on)
        {
            AbilityUIOn.SetActive(false);
            AbilityUIOff.SetActive(true);
        }
    }
}