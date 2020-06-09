﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.SceneManagement;

public enum GameState {
    Stopped,
    Preparation,
    CountDown,
    Playing,
    GameWon,
    GameLost,
    Paused
}

public class GameStats {
    public float gameTime;
    public int daysToWin;
    public int numberOfSubjects;
    public int subjectsInfectedScore;
    public int subjectsTestedScore;
    public int populationScore;
    public int subjectsIsolationScore;
    public int gameOverScore;
    public float gameWonScore;
}

public class GameManager : MonoBehaviour
{

    [Serializable]
    public class OnGameStateChanged : UnityEvent <float, GameState> {}
    public OnGameStateChanged onGameStateChanged;

    [Serializable]
    public class OnGamePreparation : UnityEvent <GameSettings> {}
    public OnGamePreparation onGamePreparation;

    [Serializable]
    public class OnGameCountDown : UnityEvent <int> {}
    public OnGameCountDown onGameCountDown;

    [Serializable]
    public class OnGameOver : UnityEvent <GameStats, GameState> {}
    public OnGameOver onGameOver;

    [Serializable]
    public class OnPopulationChange : UnityEvent <GameStats> {}
    public OnPopulationChange onPopulationChange;

    [Serializable]
    public class OnTooMuchSpread : UnityEvent <int, int> {}
    public OnTooMuchSpread onTooMuchSpread;

    [SerializeField]
    private int[] scoreList;
    private int scoreIndex = 0;

    // in-game variables - will be reset to their default values when game stops.
    private float gameTime = 0.0f;
    private float newInfectionTimer = 0f;


    private int subjectsInfectedScore = 0;
    private int subjectsTestedScore = 0;
    private int populationScore = -1;
    private int subjectsIsolationScore = 0;

    private GameState gameState = GameState.Stopped;

    private PopulationManager populationManager;

    [SerializeField]
    private DangerImage dangerImage;
    private EventLogger eventLogger;

    private Dictionary<string, object> gameLog = new Dictionary<string, object>();

    private float countDownTime = 5f;
    private float countDownTimer = 0f;
    public float samplingFrequency = 1f;
    private int prevTime;

    private LevelManager levelManager;
    private GameSettings currentLevel;
    

    // Start is called before the first frame update
    void Start()
    {
        populationManager = this.GetComponent<PopulationManager>();
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        eventLogger = GameObject.Find("Logging").GetComponent<EventLogger>();
        currentLevel = levelManager.GetCurrentLevelSettings();
        PrepareGame();
        Dictionary<string, object> stateLog = new Dictionary<string, object>() {
            {"NumberOfHealthy", populationScore},
            {"NumberOfIsolated", subjectsIsolationScore},
            {"NumberOfTested", subjectsTestedScore},
            {"NumberOfInfected", subjectsInfectedScore},
            {"GameState", System.Enum.GetName(typeof(GameState), gameState)},
            {"SubjectsOnStart", currentLevel.numberOfSubjects},
            {"InfectedOnStart", currentLevel.numberOfInfectedOnStart},
            {"GameOverScore", currentLevel.gameOverScore},
            {"GameWonScore", currentLevel.gameWonScore},
            {"NewInfectionSeconds", currentLevel.newInfectionSeconds},
            {"LevelNo", currentLevel.levelNo},
            {"GameResolutionX", Screen.width},
            {"GameResolutionY", Screen.height}
        };
        eventLogger.UpdateStateLog(stateLog);
        Dictionary<string, object> eventLog = new Dictionary<string, object>() {
            {"Event", "GameInit"},
            {"EventType", "GameEvent"},
            {"GameTime", gameTime},
        };
        eventLogger.AddToEventLog(eventLog);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == GameState.Preparation) {
        } else if (gameState == GameState.CountDown) {
            countDownTimer -= Time.deltaTime;

            if (countDownTimer < 1) {
                gameState = GameState.Playing;
                StartCoroutine(SampleLogger());
                populationManager.StartInfection(currentLevel.numberOfInfectedOnStart);
                onGameCountDown.Invoke((int) countDownTimer);
                onGameStateChanged.Invoke(gameTime, gameState);
                eventLogger.UpdateStateLog(new Dictionary<string, object>() {{"GameState", System.Enum.GetName(typeof(GameState), gameState)}});
                Dictionary<string, object> eventLog = new Dictionary<string, object>() {
                    {"Event", "Game" + System.Enum.GetName(typeof(GameState), gameState)},
                    {"EventType", "GameEvent"},
                    {"GameTime", gameTime},
                };
                eventLogger.AddToEventLog(eventLog);
            } else if ((int) countDownTimer != prevTime) {
                Dictionary<string, object> eventLog = new Dictionary<string, object>() {
                    {"Event", "Countdown" + ((int)countDownTimer-1).ToString()},
                    {"EventType", "GameEvent"},
                    {"GameTime", gameTime},
                };
                eventLogger.AddToEventLog(eventLog);
                onGameCountDown.Invoke((int) countDownTimer);
                prevTime = (int) countDownTimer;
            }
        } else if (gameState == GameState.Playing) {
            gameTime += Time.deltaTime;
            newInfectionTimer += Time.deltaTime;
            if (newInfectionTimer > currentLevel.newInfectionSeconds) {
                populationManager.AddNewInfected();
                newInfectionTimer = 0f;
                Dictionary<string, object> eventLog = new Dictionary<string, object>() {
                    {"Event", "AddNewInfected"},
                    {"EventType", "SubjectEvent"},
                    {"GameTime", gameTime},
                };
                eventLogger.AddToEventLog(eventLog);
            }

            if (subjectsInfectedScore - subjectsIsolationScore < 1) {
                Debug.Log("There are no infected subjects left, create new infections and people.");
                Dictionary<string, object> eventLog = new Dictionary<string, object>() {
                    {"Event", "OutOfInfected"},
                    {"EventType", "SubjectEvent"},
                    {"GameTime", gameTime},
                };
                eventLogger.AddToEventLog(eventLog);
                populationManager.StartInfection(1);
                populationManager.StartPopulation(1);
                currentLevel.numberOfSubjects++;
                eventLog = new Dictionary<string, object>() {
                    {"Event", "AddNewInfected"},
                    {"EventType", "SubjectEvent"},
                    {"GameTime", gameTime},
                };
                eventLogger.AddToEventLog(eventLog);
            }

            if (populationScore > 0) {
                //Debug.Log("Gametime: " + gameTime.ToString());
                if (populationScore < currentLevel.gameOverScore) {
                        gameState = GameState.GameLost;
                        eventLogger.UpdateStateLog(new Dictionary<string, object>() {{"GameState", System.Enum.GetName(typeof(GameState), gameState)}});
                        Dictionary<string, object> eventLog = new Dictionary<string, object>() {
                            {"Event", System.Enum.GetName(typeof(GameState), gameState)},
                            {"EventType", "GameEvent"},
                            {"GameTime", gameTime},
                        };
                        eventLogger.AddToEventLog(eventLog);
                        eventLogger.SaveLogs();
                        eventLogger.ClearLogs();
                        onGameOver.Invoke(GetGameStats(), gameState);
                }
                bool didWin = gameTime > currentLevel.gameWonScore;
                //Debug.Log("gameTime: " + gameTime + " gameWonScore: " + gameWonScore + "won: " + didWin );
                if (gameTime > currentLevel.gameWonScore) {
                        Debug.Log("Game Won!");
                        gameState = GameState.GameWon;
                        eventLogger.UpdateStateLog(new Dictionary<string, object>() {{"GameState", System.Enum.GetName(typeof(GameState), gameState)}});
                        Dictionary<string, object> eventLog = new Dictionary<string, object>() {
                            {"Event", System.Enum.GetName(typeof(GameState), gameState)},
                            {"EventType", "GameEvent"},
                            {"GameTime", gameTime},
                        };
                        eventLogger.AddToEventLog(eventLog);
                        eventLogger.SaveLogs();
                        eventLogger.ClearLogs();
                        onGameOver.Invoke(GetGameStats(), gameState);
                }
            }
            
        } else if (gameState == GameState.GameLost) {
        } else if (gameState == GameState.GameWon) {
        } else if (gameState == GameState.Stopped) {
            //onGameStateChanged.Invoke(gameTime, gameState);
            //ResetGame();
        }
    }

    private IEnumerator SampleLogger() {
        while (gameState == GameState.Playing) {
            gameLog = new Dictionary<string, object>();
            gameLog["GameTime"] = gameTime;
            gameLog["NumberOfInfected"] = subjectsInfectedScore;
            gameLog["NumberOfIsolated"] = subjectsIsolationScore;
            gameLog["NumberOfHealthy"] = populationScore;
            gameLog["NumberOfTested"] = subjectsTestedScore;
            eventLogger.AddToGameLog(gameLog);
            yield return new WaitForSeconds(samplingFrequency);
        }
        yield return null;
    }

    private GameStats GetGameStats() {
            GameStats gameStats = new GameStats();
            gameStats.gameTime = gameTime;
            gameStats.numberOfSubjects = currentLevel.numberOfSubjects;
            gameStats.daysToWin = currentLevel.daysToWin;
            gameStats.subjectsInfectedScore = subjectsInfectedScore;
            gameStats.subjectsTestedScore = subjectsTestedScore;
            gameStats.populationScore = populationScore;
            gameStats.subjectsIsolationScore = subjectsIsolationScore;
            gameStats.gameOverScore = currentLevel.gameOverScore;
            gameStats.gameWonScore = currentLevel.gameWonScore;
            return gameStats;
    }

    private GameSettings GetGameSettings() {
            return currentLevel;
    }

    private void EvaluateTooMuchSpread() {
        int subjectsLeftInfected = subjectsInfectedScore - subjectsIsolationScore;
        if (subjectsLeftInfected > scoreList[scoreIndex]) {
            scoreIndex++;
            onTooMuchSpread.Invoke(scoreList[scoreIndex], subjectsLeftInfected);
        }
    }

    void ResetGame() {
        gameTime = 0.0f;
    }

    public void PrepareGame() {
        populationManager.StartPopulation(currentLevel.numberOfSubjects);
        gameState = GameState.Preparation;
        eventLogger.UpdateStateLog(new Dictionary<string, object>() {{"GameState", System.Enum.GetName(typeof(GameState), gameState)}});
        onGameStateChanged.Invoke(gameTime, gameState);
        onGamePreparation.Invoke(GetGameSettings());
        countDownTimer = countDownTime;
        prevTime = (int) countDownTime;
    }

    public void StartGame() {
        gameState = GameState.CountDown;
        onGameStateChanged.Invoke(gameTime, gameState);
        eventLogger.UpdateStateLog(new Dictionary<string, object>() {{"GameState", System.Enum.GetName(typeof(GameState), gameState)}});
    }

    public void AbortGame() {
        gameState = GameState.Stopped;
        eventLogger.UpdateStateLog(new Dictionary<string, object>() {{"GameState", System.Enum.GetName(typeof(GameState), gameState)}});
        onGameStateChanged.Invoke(gameTime, gameState);
        Dictionary<string, object> eventLog = new Dictionary<string, object>() {
            {"Event", System.Enum.GetName(typeof(GameState), gameState)},
            {"EventType", "GameEvent"},
            {"GameTime", gameTime},
        };
        eventLogger.AddToEventLog(eventLog);
        eventLogger.SaveLogs();
        eventLogger.ClearLogs();
        SceneManager.LoadScene("MagnifyGlassMainMenu");
        //Application.Quit();
    }

    public void NextLevel() {
        Dictionary<string, object> eventLog = new Dictionary<string, object>() {
            {"Event", "NextLevel"},
            {"EventType", "GameEvent"},
            {"GameTime", gameTime},
        };
        eventLogger.AddToEventLog(eventLog);
        eventLogger.SaveLogs();
        eventLogger.ClearLogs();
        eventLogger.ClearGameLogs();
        levelManager.IncrementLevel();
        levelManager.LoadNextLevel();
    }

    public void ReloadLevel() {
        Dictionary<string, object> eventLog = new Dictionary<string, object>() {
            {"Event", "ReloadingLevel"},
            {"EventType", "GameEvent"},
            {"GameTime", gameTime},
        };
        eventLogger.AddToEventLog(eventLog);
        eventLogger.SaveLogs();
        eventLogger.ClearLogs();
        eventLogger.ClearGameLogs();
        levelManager.LoadNextLevel();
    }

    public void GameOver() {

    }

    public void OnViewStats() {
        Dictionary<string, object> eventLog = new Dictionary<string, object>() {
            {"Event", "ViewingStatistics"},
            {"EventType", "GameEvent"},
            {"GameTime", gameTime},
        };
        eventLogger.AddToEventLog(eventLog);
    }

    public void OnShowingSummary() {
        Dictionary<string, object> eventLog = new Dictionary<string, object>() {
            {"Event", "ShowingSummary"},
            {"EventType", "GameEvent"},
            {"GameTime", gameTime},
        };
        eventLogger.AddToEventLog(eventLog);
    }

    //public void SetOneInfected() {
    //    populationManager.SetNumberOfInfectedOnStart(1);
    //}

    //public void SetThreeInfected() {
    //    populationManager.SetNumberOfInfectedOnStart(3);
    //}

    //public void SetFiveInfected() {
    //    populationManager.SetNumberOfInfectedOnStart(5);
    //}

    public void IncreasePopulationCount(SubjectManager subjectManager) {
        populationScore++;
        eventLogger.UpdateStateLog(new Dictionary<string, object>() {{"NumberOfHealthy", populationScore}});
        onPopulationChange.Invoke(GetGameStats());
    }

    public void DecreasePopulationCount(int id, SubjectStatus subjectStatus) {
        if (subjectStatus == SubjectStatus.Infected) {
            populationScore--;
            dangerImage.TriggerDangerImage();
            eventLogger.UpdateStateLog(new Dictionary<string, object>() {{"NumberOfHealthy", populationScore}});
            if (gameState == GameState.Playing) {
                onPopulationChange.Invoke(GetGameStats());
            }
        }
    }

    public void IncreaseSubjectInfectedCount(int id, SubjectStatus subjectStatus) {
        if (subjectStatus == SubjectStatus.Infected) {
            subjectsInfectedScore++;
            eventLogger.UpdateStateLog(new Dictionary<string, object>() {{"NumberOfInfected", subjectsInfectedScore}});
            if (gameState == GameState.Playing) {
                onPopulationChange.Invoke(GetGameStats());
                EvaluateTooMuchSpread();
            }
        }
    }

    public void IncreaseNumberOfTestsCount() {
        subjectsTestedScore++;
        eventLogger.UpdateStateLog(new Dictionary<string, object>() {{"NumberOfTested", subjectsTestedScore}});
    }

    public void IncreaseSubjectsInIsolationCount(int id, SubjectStatus subjectStatus) {
        subjectsIsolationScore++;
        eventLogger.UpdateStateLog(new Dictionary<string, object>() {{"NumberOfIsolated", subjectsIsolationScore}});
        if (gameState == GameState.Playing) {
            onPopulationChange.Invoke(GetGameStats());
        }
    }
}
