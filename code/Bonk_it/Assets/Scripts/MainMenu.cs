using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainmenuFirstButton;
    [SerializeField] GameObject optionsFirstButton;
    [SerializeField] GameObject scoreboardFirstButton;
    [SerializeField] GameObject creditsFirstButton;
    [SerializeField] GameObject enterNameFirstButton;
    
    
    //Options Settings
    //Audio
    public AudioMixer audioMixer;
    public float currentVolume;
    public Slider volumeSlider;
    
    //Grapple Sensitivity
    public Slider grappleSlider;
    public GameObject mouseLookGrapple;
    public float currentGrappleSensitivity;
    
    //Hammer Sensitivity
    public Slider hammerSlider;
    public GameObject mouseLookHammer;
    public float currentHammerSensitivity;

    /// <summary>
    /// Sets the option's values.
    /// </summary>
    public void Awake()
    {
        Time.timeScale = 0f;
        //Audio
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
            mouseLookGrapple.GetComponent<MouseLookGrapple>().SetSensitivity(currentGrappleSensitivity);
            grappleSlider.value = currentGrappleSensitivity;
        }
        //Hammer Sensitivity
        if (PlayerPrefs.HasKey("HammerSensitivity"))
        {
            currentHammerSensitivity = PlayerPrefs.GetFloat("HammerSensitivity");
            mouseLookHammer.GetComponent<MouseLookHammer>().SetSensitivity(currentHammerSensitivity);
            hammerSlider.value = currentHammerSensitivity;
        }
    }
    
    /// <summary>
    /// Sets the option's values.
    /// </summary>
    public void Start()
    {
        Time.timeScale = 0f;
        //Audio
        currentVolume = PlayerPrefs.GetFloat("volume");
        audioMixer.SetFloat("volume", currentVolume);
        volumeSlider.value = currentVolume;
        
        //Grapple Sensitivity
        currentGrappleSensitivity = PlayerPrefs.GetFloat("GrappleSensitivity");
        mouseLookGrapple.GetComponent<MouseLookGrapple>().SetSensitivity(currentGrappleSensitivity);
        grappleSlider.value = currentGrappleSensitivity;
        
        //Hammer Sensitivity
        currentHammerSensitivity = PlayerPrefs.GetFloat("HammerSensitivity");
        mouseLookHammer.GetComponent<MouseLookHammer>().SetSensitivity(currentHammerSensitivity);
        hammerSlider.value = currentHammerSensitivity;
    }
    
    /// <summary>
    /// Auto selects enter name text field.
    /// </summary>
    public void PlayGame()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(enterNameFirstButton);
    }

    /// <summary>
    /// Loads tutorial scene.
    /// </summary>
    public void PlayTutorial()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Awake();
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Auto selects option's first button.
    /// </summary>
    public void Options()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsFirstButton);
    }

    /// <summary>
    /// Auto selects scoreboard's first button.
    /// </summary>
    public void Scoreboard()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(scoreboardFirstButton);
    }

    /// <summary>
    /// Auto selects main menu's first button.
    /// </summary>
    public void BackToMainMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(mainmenuFirstButton);
    }

    /// <summary>
    /// Quits the game.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
    
    /// <summary>
    /// Auto selects credit's first button.
    /// </summary>
    public void Credits()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(creditsFirstButton);
    }
}
