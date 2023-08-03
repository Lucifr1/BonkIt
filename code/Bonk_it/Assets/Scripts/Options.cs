using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class Options : MonoBehaviour
{
    //Audio
    [Header ("Audio")]
    public AudioMixer audioMixer;
    public float currentVolume;
    public Slider volumeSlider;

    [Header("Sensitivities")]
    //Grapple Sensitivity
    public Slider grappleSlider;
    public GameObject mouseLookGrapple;
    public float currentGrappleSensitivity;
    
    //Hammer Sensitivity
    public Slider hammerSlider;
    public GameObject mouseLookHammer;
    public float currentHammerSensitivity;
    
    /// <summary>
    /// Set voulume and sensitivities with PlayerPrefs.
    /// </summary>
    public void Awake()
    {
        if (PlayerPrefs.HasKey("volume"))
        {
            currentVolume = PlayerPrefs.GetFloat("volume");
            audioMixer.SetFloat("volume", currentVolume);
            volumeSlider.value = currentVolume;
        }
        
        //Grapple Sensitivity
        if (PlayerPrefs.HasKey("GrappleSensitivity"))
        {
            currentGrappleSensitivity = PlayerPrefs.GetFloat("GrappleSensitivity");
            grappleSlider.value = currentGrappleSensitivity;
            mouseLookGrapple.GetComponent<MouseLookGrapple>().SetSensitivity(grappleSlider.value);
            
        }
        //Hammer Sensitivity
        if (PlayerPrefs.HasKey("HammerSensitivity"))
        {
            currentHammerSensitivity = PlayerPrefs.GetFloat("HammerSensitivity");
            hammerSlider.value = currentHammerSensitivity;
            mouseLookHammer.GetComponent<MouseLookHammer>().SetSensitivity(hammerSlider.value);
            
        }
    }

    /// <summary>
    /// Set volume and sensitivities with PlayerPrefs.
    /// </summary>
    public void Start()
    {
        //Audio
        currentVolume = PlayerPrefs.GetFloat("volume");
        audioMixer.SetFloat("volume", currentVolume);
        volumeSlider.value = currentVolume;
        
        //Grapple Sensitivity
        currentGrappleSensitivity = PlayerPrefs.GetFloat("GrappleSensitivity");
        grappleSlider.value = currentGrappleSensitivity;
        mouseLookGrapple.GetComponent<MouseLookGrapple>().SetSensitivity(grappleSlider.value);
        
        //Hammer Sensitivity
        currentHammerSensitivity = PlayerPrefs.GetFloat("HammerSensitivity");
        hammerSlider.value = currentHammerSensitivity;
        mouseLookHammer.GetComponent<MouseLookHammer>().SetSensitivity(hammerSlider.value);
    }

    /// <summary>
    /// Sets volume of the active audioMixer using the respective slider in the options menu.
    /// </summary>
    /// <param name="volume">float volume</param>
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        PlayerPrefs.SetFloat("volume", volume);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Sets sensitivity for the grapple-player using the respective slider in the options menu.
    /// </summary>
    /// <param name="sensitivityGrapple">float sensitivity of the grapple-player</param>
    public void SetSensitivityGrapple(float sensitivityGrapple)
    {
        mouseLookGrapple.GetComponent<MouseLookGrapple>().SetSensitivity(grappleSlider.value);
        PlayerPrefs.SetFloat("GrappleSensitivity", sensitivityGrapple);
        PlayerPrefs.Save();
    }
    /// <summary>
    /// Sets sensitivity for the hammer-player using the respective slider in the options menu.
    /// </summary>
    /// <param name="sensitivityHammer">float sensitivity of the hammer-player</param>
    public void SetSensitivityHammer(float sensitivityHammer)
    {
        mouseLookHammer.GetComponent<MouseLookHammer>().SetSensitivity(hammerSlider.value);
        PlayerPrefs.SetFloat("HammerSensitivity", sensitivityHammer);
        PlayerPrefs.Save();
    }
}