using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using XInputDotNetPure;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject pauseMenuInnen;
    public GameObject optionsMenu;
    [SerializeField] GameObject pauseFirstButton;
    [SerializeField] GameObject optionsFirstButton;

    //bool so you can't open pauseMenu during goal in tutorial and level
    public bool pauseMenuNotAvailable = false;

    //Audio stop
    public AudioSource walking;

    /// <summary>
    /// Opening and closing of pauseMenu
    /// </summary>
    private void Update()
    {
        if (!pauseMenuNotAvailable)
        {
            if (Input.GetButtonDown("EscapeGrapple") || Input.GetButtonDown("EscapeHammer"))
            {
                if (pauseMenu.activeSelf)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(pauseFirstButton);
                    optionsMenu.SetActive(false);
                    pauseMenuInnen.SetActive(true);
                    pauseMenu.SetActive(false);
                    Cursor.lockState = CursorLockMode.Locked;
                    Time.timeScale = 1f;

                }
                else
                {
                    Time.timeScale = 0f;
                    pauseMenu.SetActive(true);
                    Cursor.lockState = CursorLockMode.Locked;
                    walking.Stop();

                    //Stop Controller Vibration
                    GamePad.SetVibration(PlayerIndex.One, 0f, 0f);

                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(pauseFirstButton);
                }
            }
        }
    }

    /// <summary>
    /// Loads MainMenu scene. Accessed through UI-button in canvas.
    /// </summary>
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Resumes the game, unfreezes game. Accessed through UI-button in canvas.
    /// </summary>
    public void Resume()
    {
        optionsMenu.SetActive(false);
        pauseMenuInnen.SetActive(true);
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Restarts the level/ reloads scene. Accessed through UI-button in canvas.
    /// </summary>
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Selects correct first UI-button for optionsMenu.
    /// </summary>
    public void Options()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsFirstButton);
    }

    /// <summary>
    /// Selects correct first UI-button for pauseMenu.
    /// </summary>
    public void BackToPauseMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
    }

    /// <summary>
    /// Closes the application. Accessed through UI-button in canvas.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}