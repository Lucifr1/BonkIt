using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class MouseLookHammer : MonoBehaviour
{
    private float mouseSensitivity = 200f;
    [SerializeField] private Transform playerBody;
    [SerializeField] private Transform playerGrapple;
    [SerializeField] private GameObject options;
    private float xRotation = 0f;
    
    /// <summary>
    /// Locks the cursor and calls the 'Options' script.
    /// </summary>
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        options.GetComponent<Options>().Awake();
    }

    /// <summary>
    /// Rotates the camera according to the input.
    /// </summary>
    void Update()
    {
        float mouseX = Input.GetAxis("CameraHorizontalHammer") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("CameraVerticalHammer") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
        
        //LookAt (other player) ability
        if (Input.GetButtonDown("CameraLockHammer") && Time.timeScale > 0)
        {
            Vector3 targetPosition = new Vector3(playerGrapple.position.x, transform.parent.position.y, playerGrapple.position.z);
            transform.parent.LookAt(targetPosition);
        }
    }
    
    /// <summary>
    /// Sets the hammer controller sensitivity.
    /// </summary>
    /// <param name="sensitivity">Hammer controller sensitivity.</param>
    public void SetSensitivity(float sensitivity)
    {
        mouseSensitivity = sensitivity;
    }
}