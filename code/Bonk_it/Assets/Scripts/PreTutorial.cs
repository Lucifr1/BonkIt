using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PreTutorial : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI currentText;
    [TextArea] [SerializeField] string fullText;
    public float elapsedTime;

    [SerializeField] GameObject TextField;
    [SerializeField] GameObject Typing;

    /// <summary>
    /// Displays text in PreTutorial Scene.
    /// </summary>
    void Update()
    {
        elapsedTime += Time.deltaTime;
        TextField.SetActive(true);
        Typing.SetActive(true);

        if (elapsedTime > 2)
        {
            if (currentText.text == fullText)
            {
                Typing.SetActive(false);
            }
        }

        if(elapsedTime > 13)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Time.timeScale = 1f;
        }
    }
}
