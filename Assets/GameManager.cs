using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public enum GameState {
    Stopped,
    Preparation,
    Playing,
    GameWon,
    GameLost,
    Paused
}


public class GameManager : MonoBehaviour
{

    [Serializable]
    public class OnGameStateChanged : UnityEvent <float, GameState> {}
    public OnGameStateChanged onGameStateChanged;

    [Serializable]
    public class OnGamePreparation : UnityEvent {}
    public OnGamePreparation onGamePreparation;

    [Serializable]
    public class OnGameWon : UnityEvent {}
    public OnGameWon onGameWon;

    [Serializable]
    public class OnGameLost : UnityEvent {}
    public OnGameLost onGameLost;

    // in-game variables - will be reset to their default values when game stops.
    private float gameTime = 0.0f;
    public float newInfectionSeconds = 10;
    private float newInfectionTimer = 0f;

    public int numberOfSubjects = 150;
    private int subjectsInfectedScore = 0;
    private int subjectsTestedScore = 0;
    private int populationScore = -1;
    private int subjectsIsolationScore = 0;
    
    public int gameOverScore = 20;
    public float gameWonScore = 30f;

    private GameState gameState = GameState.Stopped;

    private PopulationManager populationManager;

    [SerializeField]
    private InfectionStats infectStats;

    [SerializeField]
    private DangerImage dangerImage;
    
    private float countDownTime =3f;
    private float countDownTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        populationManager = this.GetComponent<PopulationManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == GameState.Preparation) {
            onGameStateChanged.Invoke(gameTime, gameState);
            onGamePreparation.Invoke();
        } else if (gameState == GameState.Playing) {
            gameTime += Time.deltaTime;
            newInfectionTimer += Time.deltaTime;
            if (newInfectionTimer > newInfectionSeconds) {
                populationManager.AddNewInfected();
                newInfectionTimer = 0f;
            }

            if (subjectsInfectedScore - subjectsIsolationScore < 1) {
                Debug.Log("There are no infected subjects left, create new infections.");
                populationManager.StartInfection();
            }

            if (populationScore > 0) {
                Debug.Log("Gametime: " + gameTime.ToString());
                if (populationScore < gameOverScore) {
                        gameState = GameState.GameLost;
                }
                bool didWin = gameTime > gameWonScore;
                Debug.Log("gameTime: " + gameTime + " gameWonScore: " + gameWonScore + "won: " + didWin );
                if (gameTime > gameWonScore) {
                        Debug.Log("Game Won!");
                        gameState = GameState.GameWon;
                }
            }
            
        } else if (gameState == GameState.GameLost) {
            onGameStateChanged.Invoke(gameTime, gameState);
            onGameLost.Invoke();
        } else if (gameState == GameState.GameWon) {
            onGameStateChanged.Invoke(gameTime, gameState);
            onGameWon.Invoke();
        } else if (gameState == GameState.Stopped) {
            onGameStateChanged.Invoke(gameTime, gameState);
            ResetGame();
        }
    }

    void ResetGame() {
        gameTime = 0.0f;
    }

    public void PrepareGame() {
        populationManager.StartPopulation(numberOfSubjects);
        gameState = GameState.Preparation;
        onGameStateChanged.Invoke(gameTime, gameState);
    }

    public void StartGame() {
        populationManager.StartInfection();
        gameState = GameState.Playing;
        onGameStateChanged.Invoke(gameTime, gameState);
    }

    public void AbortGame() {
        gameState = GameState.Stopped;
        onGameStateChanged.Invoke(gameTime, gameState);
        Application.Quit();
    }

    public void GameOver() {

    }

    public void SetOneInfected() {
        populationManager.SetNumberOfInfectedOnStart(1);
    }

    public void SetThreeInfected() {
        populationManager.SetNumberOfInfectedOnStart(3);
    }

    public void SetFiveInfected() {
        populationManager.SetNumberOfInfectedOnStart(5);
    }

    public void IncreasePopulationCount(SubjectManager subjectManager) {
        populationScore++;
        infectStats.UpdatePopulationScore(populationScore);
    }

    public void DecreasePopulationCount(int id, SubjectStatus subjectStatus) {
        if (subjectStatus == SubjectStatus.Infected) {
            populationScore--;
            dangerImage.TriggerDangerImage();
            infectStats.UpdatePopulationScore(populationScore);
        }
    }

    public void IncreaseSubjectInfectedCount(int id, SubjectStatus subjectStatus) {
        if (subjectStatus == SubjectStatus.Infected) {
            subjectsInfectedScore++;
        }
    }

    public void IncreaseNumberOfTestsCount() {
        subjectsTestedScore++;
    }

    public void IncreaseSubjectsInIsolationCount(int id, SubjectStatus subjectStatus) {
        subjectsIsolationScore++;
        infectStats.UpdateIsolationScore(subjectsIsolationScore);
    }
}
