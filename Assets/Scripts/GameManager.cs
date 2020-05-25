using System.Collections;
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
    
    private float countDownTime = 5f;
    private float countDownTimer = 0f;
    private int prevTime;

    private LevelManager levelManager;
    private GameSettings currentLevel;

    // Start is called before the first frame update
    void Start()
    {
        populationManager = this.GetComponent<PopulationManager>();
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        currentLevel = levelManager.GetCurrentLevelSettings();
        populationManager.SetNumberOfInfectedOnStart(currentLevel.numberOfInfectedOnStart);
        PrepareGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == GameState.Preparation) {
        } else if (gameState == GameState.CountDown) {
            countDownTimer -= Time.deltaTime;

            if (countDownTimer < 1) {
                gameState = GameState.Playing;
                populationManager.StartInfection();
                onGameCountDown.Invoke((int) countDownTimer);
                onGameStateChanged.Invoke(gameTime, gameState);
            } else if ((int) countDownTimer != prevTime) {
                onGameCountDown.Invoke((int) countDownTimer);
                prevTime = (int) countDownTimer;
            }
        } else if (gameState == GameState.Playing) {
            gameTime += Time.deltaTime;
            newInfectionTimer += Time.deltaTime;
            if (newInfectionTimer > currentLevel.newInfectionSeconds) {
                populationManager.AddNewInfected();
                newInfectionTimer = 0f;
            }

            if (subjectsInfectedScore - subjectsIsolationScore < 1) {
                Debug.Log("There are no infected subjects left, create new infections.");
                populationManager.StartInfection();
            }

            if (populationScore > 0) {
                //Debug.Log("Gametime: " + gameTime.ToString());
                if (populationScore < currentLevel.gameOverScore) {
                        gameState = GameState.GameLost;
                }
                bool didWin = gameTime > currentLevel.gameWonScore;
                //Debug.Log("gameTime: " + gameTime + " gameWonScore: " + gameWonScore + "won: " + didWin );
                if (gameTime > currentLevel.gameWonScore) {
                        Debug.Log("Game Won!");
                        gameState = GameState.GameWon;
                }
            }
            
        } else if (gameState == GameState.GameLost) {
            onGameStateChanged.Invoke(gameTime, gameState);
            onGameOver.Invoke(GetGameStats(), gameState);
        } else if (gameState == GameState.GameWon) {
            onGameStateChanged.Invoke(gameTime, gameState);
            onGameOver.Invoke(GetGameStats(), gameState);
        } else if (gameState == GameState.Stopped) {
            onGameStateChanged.Invoke(gameTime, gameState);
            ResetGame();
        }
    }

    private GameStats GetGameStats() {
            GameStats gameStats = new GameStats();
            gameStats.gameTime = gameTime;
            gameStats.numberOfSubjects = currentLevel.numberOfSubjects;
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
        onGameStateChanged.Invoke(gameTime, gameState);
        onGamePreparation.Invoke(GetGameSettings());
        countDownTimer = countDownTime;
        prevTime = (int) countDownTime;
    }

    public void StartGame() {
        gameState = GameState.CountDown;
        onGameStateChanged.Invoke(gameTime, gameState);
    }

    public void AbortGame() {
        gameState = GameState.Stopped;
        onGameStateChanged.Invoke(gameTime, gameState);
        SceneManager.LoadScene("MagnifyGlassMainMenu");
        //Application.Quit();
    }

    public void NextLevel() {
        levelManager.IncrementLevel();
        levelManager.LoadNextLevel();
    }

    public void ReloadLevel() {
        levelManager.LoadNextLevel();
    }

    public void GameOver() {

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
        onPopulationChange.Invoke(GetGameStats());
    }

    public void DecreasePopulationCount(int id, SubjectStatus subjectStatus) {
        if (subjectStatus == SubjectStatus.Infected) {
            populationScore--;
            dangerImage.TriggerDangerImage();
            if (gameState == GameState.Playing) {
                onPopulationChange.Invoke(GetGameStats());
            }
        }
    }

    public void IncreaseSubjectInfectedCount(int id, SubjectStatus subjectStatus) {
        if (subjectStatus == SubjectStatus.Infected) {
            subjectsInfectedScore++;
            if (gameState == GameState.Playing) {
                onPopulationChange.Invoke(GetGameStats());
                EvaluateTooMuchSpread();
            }
        }
    }

    public void IncreaseNumberOfTestsCount() {
        subjectsTestedScore++;
    }

    public void IncreaseSubjectsInIsolationCount(int id, SubjectStatus subjectStatus) {
        subjectsIsolationScore++;
        if (gameState == GameState.Playing) {
            onPopulationChange.Invoke(GetGameStats());
        }
    }
}
