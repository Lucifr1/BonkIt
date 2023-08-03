using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TypewritingUI : MonoBehaviour
{
	Text text;
	TMP_Text tmpProText;
	string writer;

	[SerializeField] float delayBeforeStart = 0f;
	[SerializeField] float timeBtwChars = 0.1f;
	[SerializeField] string leadingChar = "";
	[SerializeField] bool leadingCharBeforeDelay = false;

	/// <summary>
	/// Initialises the typewriter by starting coroutines
	/// </summary>
	void Start()
	{
		text = GetComponent<Text>()!;
		tmpProText = GetComponent<TMP_Text>()!;

		if(text != null)
        {
			writer = text.text;
			text.text = "";

			StartCoroutine("TypeWriterText");
		}

		if (tmpProText != null)
		{
			writer = tmpProText.text;
			tmpProText.text = "";

			StartCoroutine("TypeWriterTMP");
		}
	}

	/// <summary>
	/// Adds single characters in foreach-cycle, and waits 'timeBtwChars' before the next cycle is entered; For normal text fields
	/// </summary>
	/// <returns>WaitForSeconds</returns>
	IEnumerator TypeWriterText()
	{
		text.text = leadingCharBeforeDelay ? leadingChar : "";

		yield return new WaitForSeconds(delayBeforeStart);

		foreach (char c in writer)
		{
			if (text.text.Length > 0)
			{
				text.text = text.text.Substring(0, text.text.Length - leadingChar.Length);
			}
			text.text += c;
			text.text += leadingChar;
			yield return new WaitForSeconds(timeBtwChars);
		}

		if(leadingChar != "")
        {
			text.text = text.text.Substring(0, text.text.Length - leadingChar.Length);
		}
	}

	/// <summary>
	/// Adds single characters in foreach-cycle, and waits 'timeBtwChars' before the next cycle is entered; For text mesh pro
	/// </summary>
	/// <returns>WaitForSeconds</returns>
	IEnumerator TypeWriterTMP()
    {
        tmpProText.text = leadingCharBeforeDelay ? leadingChar : "";

        yield return new WaitForSeconds(delayBeforeStart);

		foreach (char c in writer)
		{
			if (tmpProText.text.Length > 0)
			{
				tmpProText.text = tmpProText.text.Substring(0, tmpProText.text.Length - leadingChar.Length);
			}
			tmpProText.text += c;
			tmpProText.text += leadingChar;
			yield return new WaitForSeconds(timeBtwChars);
		}

		if (leadingChar != "")
		{
			tmpProText.text = tmpProText.text.Substring(0, tmpProText.text.Length - leadingChar.Length);
		}
	}
}