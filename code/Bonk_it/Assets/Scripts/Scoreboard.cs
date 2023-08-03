using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Scoreboard : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject timer;

    //Textfield
    public TMP_InputField profileInputField;

    //Names
    public TMP_Text scoreText1;
    public TMP_Text scoreText2;
    public TMP_Text scoreText3;
    public TMP_Text scoreText4;
    public TMP_Text scoreText5;

    //Times
    public TMP_Text scoreTime1;
    public TMP_Text scoreTime2;
    public TMP_Text scoreTime3;
    public TMP_Text scoreTime4;
    public TMP_Text scoreTime5;

    //Boxes
    public GameObject scoreBox1;
    public GameObject scoreBox2;
    public GameObject scoreBox3;
    public GameObject scoreBox4;
    public GameObject scoreBox5;

    private float currentTime;
    private float compareTime;
    private Scene currentScene;
    private String sceneName;
    private TimeSpan compareTimeSpan;

    //Level Goal UI
    [SerializeField] private GameObject goalUI;
    [SerializeField] private GameObject goalFirstButton;
    [SerializeField] private GameObject sorryMessage;
    [SerializeField] private TMP_Text profileTime;
    
    //Audio stop
    public AudioSource walking;

    /// <summary>
    /// Set Player Prefs when starting game for the first time
    /// </summary>
    private void Start()
    { 
        if (!PlayerPrefs.HasKey("FirstScoreName"))
        {
            SetPlayerPrefs();
        }
        
        ScoreboardSchreiben();
    }

    /// <summary>
    /// ResetScoreboard-Keybind
    /// </summary>
    private void Update()
    {
        if (Input.GetKey(KeyCode.K) && Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.R))
        {
            ResetPlayerPrefs();
        }
    }
    
    /// <summary>
    /// Loads level scene after entering name
    /// </summary>
    public void PlayGame()
    {
        PlayerPrefs.SetString("PlayerProfile", profileInputField.text);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
        mainMenu.GetComponent<MainMenu>().Awake();
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Sets finish time; Calls DisplayOnScoreboard(); Calls ScoreboardSchreiben()
    /// </summary>
    public void SetFinishTime()
    {
        currentTime = timer.GetComponent<Timer>().elapsedTime;
        PlayerPrefs.SetFloat("ProfileTime", currentTime);
        walking.Stop();
        goalUI.SetActive(true);
        profileTime.text = "Your time: " + TimeSpan.FromSeconds(currentTime).ToString("mm':'ss'.'ff");
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(goalFirstButton);
        Time.timeScale = 0f;
        DisplayOnScoreboard();
        ScoreboardSchreiben();
        Cursor.lockState = CursorLockMode.None;
    }
    
    /// <summary>
    /// method gets executed at the end of the level; adjusts scoreboard placements based on the times; highlights own score (if in top5)
    /// </summary>
    private void DisplayOnScoreboard()
    {
        currentScene = SceneManager.GetActiveScene(); 
        sceneName = currentScene.name; 
        compareTime = PlayerPrefs.GetFloat("ProfileTime");
        compareTimeSpan = TimeSpan.FromSeconds(compareTime);

        //adjusts scoreboard placements
        if (compareTime > 0f)
        {
            if (compareTime < PlayerPrefs.GetFloat("FirstScoreTimeFloat") || PlayerPrefs.GetFloat("FirstScoreTimeFloat") == 0)
            {
                VerschiebungScore1();
                PlayerPrefs.SetString("FirstScoreName", PlayerPrefs.GetString("PlayerProfile"));
                PlayerPrefs.SetString("FirstScoreTime", compareTimeSpan.ToString("mm':'ss'.'ff"));
                PlayerPrefs.SetFloat("FirstScoreTimeFloat", PlayerPrefs.GetFloat("ProfileTime"));
                scoreText2.text = " " + PlayerPrefs.GetString("FirstScoreName");
                scoreTime2.text = compareTimeSpan.ToString("mm':'ss'.'ff");
                PlayerPrefs.SetString("PlayerProfile", null);
                PlayerPrefs.SetFloat("ProfileTime", 0f);
                scoreBox1.transform.localScale = new Vector3(1.1f,1.1f,1f);
                VerschiebungScoreSave();
                
            }
            else if (compareTime < PlayerPrefs.GetFloat("SecondScoreTimeFloat")|| PlayerPrefs.GetFloat("SecondScoreTimeFloat") == 0)
            {
                VerschiebungScore2();
                PlayerPrefs.SetString("SecondScoreName", PlayerPrefs.GetString("PlayerProfile"));
                PlayerPrefs.SetString("SecondScoreTime", compareTimeSpan.ToString("mm':'ss'.'ff"));
                PlayerPrefs.SetFloat("SecondScoreTimeFloat", PlayerPrefs.GetFloat("ProfileTime"));
                scoreText2.text = " " + PlayerPrefs.GetString("SecondScoreName");
                scoreTime2.text = compareTimeSpan.ToString("mm':'ss'.'ff");
                PlayerPrefs.SetString("PlayerProfile", null);
                PlayerPrefs.SetFloat("ProfileTime", 0f);
                scoreBox2.transform.localScale = new Vector3(1.1f,1.1f,1f);
                VerschiebungScoreSave();
            }
            else if (compareTime < PlayerPrefs.GetFloat("ThirdScoreTimeFloat")|| PlayerPrefs.GetFloat("ThirdScoreTimeFloat") == 0)
            {
                VerschiebungScore3();
                PlayerPrefs.SetString("ThirdScoreName", PlayerPrefs.GetString("PlayerProfile"));
                PlayerPrefs.SetString("ThirdScoreTime", compareTimeSpan.ToString("mm':'ss'.'ff"));
                PlayerPrefs.SetFloat("ThirdScoreTimeFloat", PlayerPrefs.GetFloat("ProfileTime"));
                scoreText3.text = " " + PlayerPrefs.GetString("ThirdScoreName");
                scoreTime3.text = compareTimeSpan.ToString("mm':'ss'.'ff");
                PlayerPrefs.SetString("PlayerProfile", null);
                PlayerPrefs.SetFloat("ProfileTime", 0f);
                scoreBox3.transform.localScale = new Vector3(1.1f,1.1f,1f);
                VerschiebungScoreSave();
            }
            else if (compareTime < PlayerPrefs.GetFloat("FourthScoreTimeFloat")|| PlayerPrefs.GetFloat("FourthScoreTimeFloat") == 0)
            {
                VerschiebungScore4();
                PlayerPrefs.SetString("FourthScoreName", PlayerPrefs.GetString("PlayerProfile"));
                PlayerPrefs.SetString("FourthScoreTime", compareTimeSpan.ToString("mm':'ss'.'ff"));
                PlayerPrefs.SetFloat("FourthScoreTimeFloat", PlayerPrefs.GetFloat("ProfileTime"));
                scoreText4.text = " " + PlayerPrefs.GetString("FourthScoreName");
                scoreTime4.text = compareTimeSpan.ToString("mm':'ss'.'ff");
                PlayerPrefs.SetString("PlayerProfile", null);
                PlayerPrefs.SetFloat("ProfileTime", 0f);
                scoreBox4.transform.localScale = new Vector3(1.1f,1.1f,1f);
                VerschiebungScoreSave();
            }
            else if (compareTime < PlayerPrefs.GetFloat("FifthScoreTimeFloat")|| PlayerPrefs.GetFloat("FifthScoreTimeFloat") == 0)
            {
                PlayerPrefs.SetString("FifthScoreName", PlayerPrefs.GetString("PlayerProfile"));
                PlayerPrefs.SetString("FifthScoreTime", compareTimeSpan.ToString("mm':'ss'.'ff"));
                PlayerPrefs.SetFloat("FifthScoreTimeFloat", PlayerPrefs.GetFloat("ProfileTime"));
                scoreText5.text = " " + PlayerPrefs.GetString("FifthScoreName");
                scoreTime5.text = compareTimeSpan.ToString("mm':'ss'.'ff");
                PlayerPrefs.SetString("PlayerProfile", null);
                PlayerPrefs.SetFloat("ProfileTime", 0f);
                scoreBox5.transform.localScale = new Vector3(1.1f,1.1f,1f);
            }
            else if (sceneName == "Level")
            {
                sorryMessage.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Moves scoreboard placements, if currentTime should be first place
    /// </summary>
    private void VerschiebungScore1()
    {
        //First to second
        PlayerPrefs.SetString("SecondScoreName", PlayerPrefs.GetString("FirstScoreName"));
        PlayerPrefs.SetString("SecondScoreTime", PlayerPrefs.GetString("FirstScoreTime"));
        PlayerPrefs.SetFloat("SecondScoreTimeFloat", PlayerPrefs.GetFloat("FirstScoreTimeFloat"));
        //Second to third
        PlayerPrefs.SetString("ThirdScoreName", PlayerPrefs.GetString("SecondScoreNameSave"));
        PlayerPrefs.SetString("ThirdScoreTime", PlayerPrefs.GetString("SecondScoreTimeSave"));
        PlayerPrefs.SetFloat("ThirdScoreTimeFloat", PlayerPrefs.GetFloat("SecondScoreTimeFloatSave"));
        //Third to fourth
        PlayerPrefs.SetString("FourthScoreName", PlayerPrefs.GetString("ThirdScoreNameSave"));
        PlayerPrefs.SetString("FourthScoreTime", PlayerPrefs.GetString("ThirdScoreTimeSave"));
        PlayerPrefs.SetFloat("FourthScoreTimeFloat", PlayerPrefs.GetFloat("ThirdScoreTimeFloatSave"));
        //Fourth to fifth
        PlayerPrefs.SetString("FifthScoreName", PlayerPrefs.GetString("FourthScoreNameSave"));
        PlayerPrefs.SetString("FifthScoreTime", PlayerPrefs.GetString("FourthScoreTimeSave"));
        PlayerPrefs.SetFloat("FifthScoreTimeFloat", PlayerPrefs.GetFloat("FourthScoreTimeFloatSave"));
    }

    /// <summary>
    /// Moves scoreboard placements, if currentTime should be second place
    /// </summary>
    private void VerschiebungScore2()
    {
        //Second to third
        PlayerPrefs.SetString("ThirdScoreName", PlayerPrefs.GetString("SecondScoreNameSave"));
        PlayerPrefs.SetString("ThirdScoreTime", PlayerPrefs.GetString("SecondScoreTimeSave"));
        PlayerPrefs.SetFloat("ThirdScoreTimeFloat", PlayerPrefs.GetFloat("SecondScoreTimeFloatSave"));
        //Third to fourth
        PlayerPrefs.SetString("FourthScoreName", PlayerPrefs.GetString("ThirdScoreNameSave"));
        PlayerPrefs.SetString("FourthScoreTime", PlayerPrefs.GetString("ThirdScoreTimeSave"));
        PlayerPrefs.SetFloat("FourthScoreTimeFloat", PlayerPrefs.GetFloat("ThirdScoreTimeFloatSave"));
        //Fourth to fifth
        PlayerPrefs.SetString("FifthScoreName", PlayerPrefs.GetString("FourthScoreNameSave"));
        PlayerPrefs.SetString("FifthScoreTime", PlayerPrefs.GetString("FourthScoreTimeSave"));
        PlayerPrefs.SetFloat("FifthScoreTimeFloat", PlayerPrefs.GetFloat("FourthScoreTimeFloatSave"));
    }

    /// <summary>
    /// Moves scoreboard placements, if currentTime should be third place
    /// </summary>
    private void VerschiebungScore3()
    {
        //Third to fourth
        PlayerPrefs.SetString("FourthScoreName", PlayerPrefs.GetString("ThirdScoreNameSave"));
        PlayerPrefs.SetString("FourthScoreTime", PlayerPrefs.GetString("ThirdScoreTimeSave"));
        PlayerPrefs.SetFloat("FourthScoreTimeFloat", PlayerPrefs.GetFloat("ThirdScoreTimeFloatSave"));
        //Fourth to fifth
        PlayerPrefs.SetString("FifthScoreName", PlayerPrefs.GetString("FourthScoreNameSave"));
        PlayerPrefs.SetString("FifthScoreTime", PlayerPrefs.GetString("FourthScoreTimeSave"));
        PlayerPrefs.SetFloat("FifthScoreTimeFloat", PlayerPrefs.GetFloat("FourthScoreTimeFloatSave"));
    }

    /// <summary>
    /// Moves scoreboard placements, if currentTime should be fourth place
    /// </summary>
    private void VerschiebungScore4()
    {
        //Fourth to fifth
        PlayerPrefs.SetString("FifthScoreName", PlayerPrefs.GetString("FourthScoreNameSave"));
        PlayerPrefs.SetString("FifthScoreTime", PlayerPrefs.GetString("FourthScoreTimeSave"));
        PlayerPrefs.SetFloat("FifthScoreTimeFloat", PlayerPrefs.GetFloat("FourthScoreTimeFloatSave"));
    }

    /// <summary>
    /// help-method to move scores
    /// </summary>
    private void VerschiebungScoreSave()
    {
        //Store scores temporarily
        PlayerPrefs.SetString("SecondScoreNameSave", PlayerPrefs.GetString("SecondScoreName"));
        PlayerPrefs.SetString("SecondScoreTimeSave", PlayerPrefs.GetString("SecondScoreTime"));
        PlayerPrefs.SetFloat("SecondScoreTimeFloatSave", PlayerPrefs.GetFloat("SecondScoreTimeFloat"));
        PlayerPrefs.SetString("ThirdScoreNameSave", PlayerPrefs.GetString("ThirdScoreName"));
        PlayerPrefs.SetString("ThirdScoreTimeSave", PlayerPrefs.GetString("ThirdScoreTime"));
        PlayerPrefs.SetFloat("ThirdScoreTimeFloatSave", PlayerPrefs.GetFloat("ThirdScoreTimeFloat"));
        PlayerPrefs.SetString("FourthScoreNameSave", PlayerPrefs.GetString("FourthScoreName"));
        PlayerPrefs.SetString("FourthScoreTimeSave", PlayerPrefs.GetString("FourthScoreTime"));
        PlayerPrefs.SetFloat("FourthScoreTimeFloatSave", PlayerPrefs.GetFloat("FourthScoreTimeFloat"));
        PlayerPrefs.SetString("FifthScoreNameSave", PlayerPrefs.GetString("FifthScoreName"));
        PlayerPrefs.SetString("FifthScoreTimeSave", PlayerPrefs.GetString("FifthScoreTime"));
        PlayerPrefs.SetFloat("FifthScoreTimeFloatSave", PlayerPrefs.GetFloat("FifthScoreTimeFloat"));
    }

    /// <summary>
    /// Method to fill scoreboard's text fields.
    /// </summary>
    private void ScoreboardSchreiben()
    {
        scoreText1.text = " " + PlayerPrefs.GetString("FirstScoreName");
        scoreText2.text = " " + PlayerPrefs.GetString("SecondScoreName");
        scoreText3.text = " " + PlayerPrefs.GetString("ThirdScoreName");
        scoreText4.text = " " + PlayerPrefs.GetString("FourthScoreName");
        scoreText5.text = " " + PlayerPrefs.GetString("FifthScoreName");
        scoreTime1.text = " " + PlayerPrefs.GetString("FirstScoreTime");
        scoreTime2.text = " " + PlayerPrefs.GetString("SecondScoreTime");
        scoreTime3.text = " " + PlayerPrefs.GetString("ThirdScoreTime");
        scoreTime4.text = " " + PlayerPrefs.GetString("FourthScoreTime");
        scoreTime5.text = " " + PlayerPrefs.GetString("FifthScoreTime");
        VerschiebungScoreSave();
    }


    /// /// <summary>
    /// SetPlayerPrefs-method when starting game for the first time
    /// </summary>
    private void SetPlayerPrefs()
    {
        PlayerPrefs.SetString("FirstScoreName", scoreText1.text);
        PlayerPrefs.SetString("FirstScoreTime", scoreTime1.text);
        PlayerPrefs.SetString("SecondScoreName", scoreText2.text);
        PlayerPrefs.SetString("SecondScoreTime", scoreTime2.text);
        PlayerPrefs.SetString("ThirdScoreName", scoreText3.text);
        PlayerPrefs.SetString("ThirdScoreTime", scoreTime3.text);
        PlayerPrefs.SetString("FourthScoreName", scoreText4.text);
        PlayerPrefs.SetString("FourthScoreTime", scoreTime4.text);
        PlayerPrefs.SetString("FifthScoreName", scoreText5.text);
        PlayerPrefs.SetString("FifthScoreTime", scoreTime5.text);
        PlayerPrefs.SetFloat("FirstScoreTimeFloat", 0f);
        PlayerPrefs.SetFloat("SecondScoreTimeFloat", 0f);
        PlayerPrefs.SetFloat("ThirdScoreTimeFloat", 0f);
        PlayerPrefs.SetFloat("FourthScoreTimeFloat", 0f);
        PlayerPrefs.SetFloat("FifthScoreTimeFloat", 0f);
    }
    
    /// <summary>
    /// ResetsScores-method
    /// </summary>
    private void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteKey("FirstScoreName");
        PlayerPrefs.DeleteKey("SecondScoreName");
        PlayerPrefs.DeleteKey("ThirdScoreName");
        PlayerPrefs.DeleteKey("FourthScoreName");
        PlayerPrefs.DeleteKey("FifthScoreName");
        PlayerPrefs.DeleteKey("FirstScoreTime");
        PlayerPrefs.DeleteKey("SecondScoreTime");
        PlayerPrefs.DeleteKey("ThirdScoreTime");
        PlayerPrefs.DeleteKey("FourthScoreTime");
        PlayerPrefs.DeleteKey("FifthScoreTime");
        PlayerPrefs.DeleteKey("FirstScoreTimeFloat");
        PlayerPrefs.DeleteKey("SecondScoreTimeFloat");
        PlayerPrefs.DeleteKey("ThirdScoreTimeFloat");
        PlayerPrefs.DeleteKey("FourthScoreTimeFloat");
        PlayerPrefs.DeleteKey("FifthScoreTimeFloat");
        PlayerPrefs.DeleteKey("SecondScoreNameSave");
        PlayerPrefs.DeleteKey("ThirdScoreNameSave");
        PlayerPrefs.DeleteKey("FourthScoreNameSave");
        PlayerPrefs.DeleteKey("FifthScoreNameSave");
        PlayerPrefs.DeleteKey("SecondScoreTimeSave");
        PlayerPrefs.DeleteKey("ThirdScoreTimeSave");
        PlayerPrefs.DeleteKey("FourthScoreTimeSave");
        PlayerPrefs.DeleteKey("FifthScoreTimeSave");
        PlayerPrefs.DeleteKey("SecondScoreTimeFloatSave");
        PlayerPrefs.DeleteKey("ThirdScoreTimeFloatSave");
        PlayerPrefs.DeleteKey("FourthScoreTimeFloatSave");
        PlayerPrefs.DeleteKey("FifthScoreTimeFloatSave");
    }
}