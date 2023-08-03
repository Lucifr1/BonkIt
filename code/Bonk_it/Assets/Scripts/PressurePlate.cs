using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    private GameObject PressurePlateVisable;

    private Vector3 PressurePlateScaleChange = new Vector3(0.0f, 0.1f, 0.0f);
    private Vector3 PressurePlateTransformScaleChange = new Vector3(0.0f, 0.05f, 0.0f);

    private float CollissionCounter;

    private bool PressurePlateActivated;
    private bool OpeningDoor;

    private GameObject DoorMoveableBottom;
    private GameObject DoorMoveableTop;

    private Vector3 DoorScaleChange = new Vector3(0.0f, 0.02f, 0.0f);
    private Vector3 DoorPositionScaleChange = new Vector3(0.0f, 0.01f, 0.0f);

    //Door-Sound
    [Header("Sound")] 
    [SerializeField] private AudioSource doorOpen;
    [SerializeField] private AudioSource button;
    
    /// <summary>
    /// Awakes the two parts of the moveable door.
    /// </summary>
    void Start()
    {
        DoorMoveableBottom = transform.parent.parent.GetChild(1).GetChild(0).gameObject;
        DoorMoveableTop = transform.parent.parent.GetChild(1).GetChild(1).gameObject;
    }

    /// <summary>
    /// Door Opens/ Closes by transforming the local scale and position of both moveable door parts.
    /// </summary>
    void FixedUpdate()
    {
        if (OpeningDoor)
        {
            if(DoorMoveableBottom.transform.localScale.y > 0)
            {
                DoorMoveableBottom.transform.localScale -= DoorScaleChange;
                DoorMoveableBottom.transform.position -= DoorPositionScaleChange;

                DoorMoveableTop.transform.localScale -= DoorScaleChange;
                DoorMoveableTop.transform.position += DoorPositionScaleChange;
            }
        }

        if (!OpeningDoor)
        {
            if(DoorMoveableBottom.transform.localScale.y <= 1)
            {
                DoorMoveableBottom.transform.position += DoorPositionScaleChange;
                DoorMoveableBottom.transform.localScale += DoorScaleChange;

                DoorMoveableTop.transform.position -= DoorPositionScaleChange;
                DoorMoveableTop.transform.localScale += DoorScaleChange;
            }
        }
    }

    /// <summary>
    /// Pressure plate detects player/cube; Calls pressure plate activation method
    /// </summary>
    /// <param name="other">Collider.</param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("PickUpAble"))
        {
            button.Play();
            CollissionCounter++;
            DruckplatteAktiviert();
        }
    }

    /// <summary>
    /// Pressure plate detects player/cube leaving; Calls pressure plate deactivation method
    /// </summary>
    /// <param name="other">Collider.</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("PickUpAble"))
        {
            CollissionCounter--; 
            DruckplatteDeaktiviert();
            Debug.Log(CollissionCounter);
        }
    }

    /// <summary>
    /// Pressure plate activation method; Calls OpenDoor() method; Plays Audio; Pressure plate animation
    /// </summary>
    private void DruckplatteAktiviert()
    {
        if (!PressurePlateActivated)
        {
            OpenDoor();
            doorOpen.Play();
            PressurePlatteAnimation(true);
            PressurePlateActivated = true;
        }
    }

    /// <summary>
    /// Pressure plate deactivation method; Calls CloseDoor() method; Plays Audio; Pressure plate animation
    /// </summary>
    private void DruckplatteDeaktiviert()
    {
        if (PressurePlateActivated && (CollissionCounter == 0))
        {
            CloseDoor();
            doorOpen.Play();
            PressurePlatteAnimation(false);
            PressurePlateActivated = false;
        }
    }

    /// <summary>
    /// OpenDoor() method, sets boolean to true to open door in FixedUpdate
    /// </summary>
    private void OpenDoor()
    {
        OpeningDoor = true;
    }

    /// <summary>
    /// CloseDoor() method, sets boolean to false to close door in FixedUpdate
    /// </summary>
    private void CloseDoor()
    {
        OpeningDoor = false;
    }

    /// <summary>
    /// Pressure plate animation, adjusts local scale and position of the pressure plate
    /// </summary>
    /// <param name="up">Boolean.</param>
    private void PressurePlatteAnimation(bool up)
    {
        //Pressure Plate is unchanged, gets smaller and pressed down
        if (up)
        {
            PressurePlateVisable = transform.parent.GetChild(1).gameObject;
            PressurePlateVisable.transform.localScale -= PressurePlateScaleChange;
            PressurePlateVisable.transform.position -= PressurePlateTransformScaleChange;
        }
        //Pressure Plate is small and pressed down, changes get reverted
        if (!up)
        {
            PressurePlateVisable = transform.parent.GetChild(1).gameObject;
            PressurePlateVisable.transform.position += PressurePlateTransformScaleChange;
            PressurePlateVisable.transform.localScale += PressurePlateScaleChange;
        }
    }
}