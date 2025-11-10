using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;




public class SpeedrunTimer : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI timerText;
    public GameObject leaderboardPanel;
    public TextMeshProUGUI leaderboardText;
    
    [Header("Settings")]
    public bool startOnTutorialComplete = true;
    public int maxLeaderboardEntries = 10;
    
    private float currentTime = 0f;
    private bool isRunning = false;
    private bool hasCompleted = false;

    void Start()
    {
        if (leaderboardPanel != null)
        {
            leaderboardPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (isRunning)
        {
            currentTime += Time.deltaTime;
            UpdateTimerDisplay();
        }

        if (leaderboardPanel != null && leaderboardPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return))
            {
                HideLeaderboard();
            }
        }
    }

    public void StartTimer()
    {
        if (!isRunning && !hasCompleted)
        {
            isRunning = true;
            currentTime = 0f;
            Debug.Log("[SpeedrunTimer] Timer started!");
        }
    }

    public void StopTimer()
    {
        if (isRunning)
        {
            isRunning = false;
            Debug.Log($"[SpeedrunTimer] Timer stopped at {FormatTime(currentTime)}");
        }
    }

    public void CompleteRun()
    {
        if (!hasCompleted)
        {
            StopTimer();
            hasCompleted = true;
            
            Debug.Log($"[SpeedrunTimer] Run completed in {FormatTime(currentTime)}!");

            SaveTime(currentTime);

            ShowLeaderboard();
        }
    }

    void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            timerText.text = FormatTime(currentTime);
        }
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int milliseconds = Mathf.FloorToInt((time * 100f) % 100f);
        return $"{minutes:00}:{seconds:00}.{milliseconds:00}";
    }

    void SaveTime(float time)
    {

        List<float> times = GetLeaderboardTimes();

        times.Add(time);

        times.Sort();

        if (times.Count > maxLeaderboardEntries)
        {
            times = times.Take(maxLeaderboardEntries).ToList();
        }

        for (int i = 0; i < times.Count; i++)
        {
            PlayerPrefs.SetFloat($"Speedrun_Time_{i}", times[i]);
        }
        PlayerPrefs.SetInt("Speedrun_Count", times.Count);
        PlayerPrefs.Save();
        
        Debug.Log($"[SpeedrunTimer] Saved time to leaderboard. Rank: {times.IndexOf(time) + 1}/{times.Count}");
    }

    List<float> GetLeaderboardTimes()
    {
        List<float> times = new List<float>();
        int count = PlayerPrefs.GetInt("Speedrun_Count", 0);
        
        for (int i = 0; i < count; i++)
        {
            float time = PlayerPrefs.GetFloat($"Speedrun_Time_{i}", 0f);
            if (time > 0)
            {
                times.Add(time);
            }
        }
        
        return times;
    }

    void ShowLeaderboard()
    {
        if (leaderboardPanel == null || leaderboardText == null) return;
        
        leaderboardPanel.SetActive(true);
        
        List<float> times = GetLeaderboardTimes();
        
        string leaderboardString = "=== SPEEDRUN LEADERBOARD ===\n\n";
        
        for (int i = 0; i < times.Count; i++)
        {
            string rank = (i + 1).ToString();
            string medal = "";
            
            if (i == 0) medal = "[1ST]";
            else if (i == 1) medal = "[2ND]";
            else if (i == 2) medal = "[3RD]";
            else medal = $"[{rank}]";
            
            bool isCurrentRun = Mathf.Abs(times[i] - currentTime) < 0.01f;
            string highlight = isCurrentRun ? ">>> " : "    ";
            
            leaderboardString += $"{highlight}{medal} {FormatTime(times[i])}\n";
        }
        
        leaderboardString += "\n\nPress ESC to continue";
        
        
        leaderboardText.text = leaderboardString;
    }

    public void HideLeaderboard()
    {
        if (leaderboardPanel != null)
        {
            leaderboardPanel.SetActive(false);
        }
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }

    public bool IsRunning()
    {
        return isRunning;
    }

    public void OnTutorialComplete()
    {
        if (startOnTutorialComplete)
        {
            StartTimer();
        }
    }

    public void OnGameComplete()
    {
        CompleteRun();
    }
}
