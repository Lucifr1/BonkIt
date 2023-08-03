using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Story : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI currentText;
    [TextArea] [SerializeField] string fullText;
    public float elapsedTime;

    [SerializeField] GameObject TextField;

    //Sounds
    [SerializeField] GameObject Beep;
    [SerializeField] GameObject Typing;

    /// <summary>
    /// Displays text and plays sound according to the elapsed time
    /// </summary>
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if(elapsedTime > 3)
        {
            TextField.SetActive(true);
            Typing.SetActive(true);
            Beep.SetActive(true);
        }

        if(elapsedTime > 4)
        {
            if(currentText.text == fullText)
            {
                Typing.SetActive(false);
            }
        }

        if(elapsedTime > 30)
        {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                Time.timeScale = 1f;
        }
        
        if(Input.GetKeyDown(KeyCode.K))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Time.timeScale = 1f;
        }
    }
}