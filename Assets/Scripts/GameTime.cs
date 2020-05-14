using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameTimer {
    Running,
    Paused
}

public class GameTime : MonoBehaviour
{

    [SerializeField]
    private Text dayText;

    [SerializeField]
    private Text hourText;

    private string dayTextTemplate;
    private string hourTextTemplate;

    private int currentDay;

    [SerializeField]
    private float timeForHourPass = 0.05f;
    private float hourTimer = 0f;
    private int numOfHours = 12;
    private int currentHour = 0;

    private Dictionary<int, string> hourMap;

    private GameTimer gameTimer = GameTimer.Paused;

    // Start is called before the first frame update
    void Awake()
    {
        hourMap = new Dictionary<int, string>()
		{
			{ 0, "8.00" },
			{ 1, "9.00" },
			{ 2, "10.00" },
			{ 3, "11.00" },
			{ 4, "12.00" },
			{ 5, "13.00" },
			{ 6, "14.00" },
			{ 7, "15.00" },
			{ 8, "16.00" },
			{ 9, "17.00" },
			{ 10, "18.00" },
            { 11, "19.00" },
            { 12, "20.00" }
		}; 

        dayTextTemplate = dayText.text;
        hourTextTemplate = hourText.text;

        dayText.text = string.Format(dayTextTemplate, currentDay.ToString());
        hourText.text = string.Format(hourTextTemplate, hourMap[currentHour]);
        gameTimer = GameTimer.Paused;
    }

    public void onGameStateChanged(float gameTime, GameState gameState) {
        if (gameState == GameState.Playing) {
            gameTimer = GameTimer.Running;
        } else if (gameState == GameState.Paused) {
            gameTimer = GameTimer.Paused;
        } else if (gameState == GameState.GameWon) {
            gameTimer = GameTimer.Paused;
        } else if (gameState == GameState.GameLost) {
            gameTimer = GameTimer.Paused;
        } else if (gameState == GameState.Stopped) {
            gameTimer = GameTimer.Paused;
        }
    }

    void Update() {
        if (gameTimer == GameTimer.Running) {
            hourTimer += Time.deltaTime;
            if (hourTimer > timeForHourPass) {
                hourTimer = 0;

                if (currentHour < numOfHours) {
                    currentHour++;
                } else {
                    currentHour = 0;
                    currentDay++;
                    dayText.text = string.Format(dayTextTemplate, currentDay);
                }
                hourText.text = string.Format(hourTextTemplate, hourMap[currentHour]);
            }
        }

    }
}
