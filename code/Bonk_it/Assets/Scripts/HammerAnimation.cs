using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using XInputDotNetPure;

public class HammerAnimation : MonoBehaviour
{
    private Animator anim;
    //Controller Vibration
    private GamePadState state;
    private GamePadState prevState;
    private bool canShake;


    [SerializeField] GameObject pickUpScriptPlace;

    /// <summary>
    /// Hammer animation and controller vibration.
    /// </summary>
    void Update()
    {
        anim = GetComponent<Animator>();
        
        if (Input.GetButtonDown("LinksklickHammer"))
        {
            if (Time.timeScale > 0)
            {
                canShake = true;
            }
        }

        if (canShake)
        {
            GamePad.SetVibration(PlayerIndex.One, 0.4f, 0.4f);
            canShake = false;
            StartCoroutine(ShakeTimer());
        }
    }

    /// <summary>
    /// Hammer strike animation.
    /// </summary>
    public void HammerStrike()
    {
        GetComponent<Animator>().Play("Hammer_Strike");
    }

    /// <summary>
    /// Hammer shoot animation.
    /// </summary>
    public void HammerShoot()
    {
        GetComponent<Animator>().Play("Hammer_Shoot");
    }

    /// <summary>
    /// Controller vibration timer.
    /// </summary>
    /// <returns>WaitForSeconds</returns>
    private IEnumerator ShakeTimer()
    {
        //Controller Vibration duration
        yield return new WaitForSeconds(0.4f);
        GamePad.SetVibration(PlayerIndex.One, 0f, 0f);
    }
    
    /// <summary>
    /// Hammer animation stops. Hammer in still state.
    /// </summary>
    public void HammerAnimationStop(){
        //Accessed in the hammer animation window through event at the end of hammer animations.
        GetComponent<Animator>().Play("Hammer_Still");
    }
}
