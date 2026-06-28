using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public float timeRemaining = 120f;
    public TMP_Text timerText;
    public MorningTransition morningTransition;

    private bool timerRunning = true;

    void Start()
    {
        Time.timeScale = 1f;
        UpdateTimerUI();
    }

    void Update()
    {
        if (!timerRunning) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            timerRunning = false;
            UpdateTimerUI();
            EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
            if (spawner != null)
            {
                spawner.enabled = false;
            }
            if (morningTransition != null)
            {
                morningTransition.StartMorningTransition();
            }
            else
            {
                Debug.LogWarning("MorningTransition is not assigned.");
            }

            return;
        }

        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);

        if (timerText != null)
        {
            timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        }
    }
}