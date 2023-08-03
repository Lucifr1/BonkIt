using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] TextMeshProUGUI timer;

    public float elapsedTime;
    private TimeSpan timeSpan;

    /// <summary>
    /// calls UpdateTime(), when pauseMenu is not active
    /// </summary>
    void Update()
    {
        if (!pauseMenu.activeSelf)
        {
            UpdateTimer();
        }
    }

    /// <summary>
    /// Updates the Timer with the elapsed time.
    /// </summary>
    private void UpdateTimer()
    {
        elapsedTime += Time.deltaTime;
        timeSpan = TimeSpan.FromSeconds(elapsedTime);
        timer.text = timeSpan.ToString("mm':'ss'.'ff");
    }
}