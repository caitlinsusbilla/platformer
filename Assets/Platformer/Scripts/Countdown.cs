using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Countdown : MonoBehaviour
{
    public float countdownTime = 60f; // Initial countdown time in seconds
    public TextMeshProUGUI countdownText;

    void Start()
    {
        StartCountdown();
    }

    void StartCountdown()
    {
        UpdateCountdownText();

        // Start a coroutine to countdown every second
        StartCoroutine(CountdownCoroutine());
    }

    IEnumerator CountdownCoroutine()
    {
        while (countdownTime > 0)
        {
            // Decrease countdown time
            countdownTime -= Time.deltaTime; // Use Time.deltaTime for real-time countdown

            UpdateCountdownText();

            // Wait for the next frame
            yield return null;
        }

        Debug.Log("Countdown finished");
    }

    void UpdateCountdownText()
    {
        // Update the UI text with the current countdown time
        countdownText.text = "Time: " + Mathf.RoundToInt(countdownTime).ToString();
    }
}