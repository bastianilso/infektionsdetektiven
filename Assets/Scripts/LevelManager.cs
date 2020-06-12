using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSettings {
    public int levelNo;
    public float newInfectionSeconds;
    public int gameOverScore;
    public float gameWonScore;
    public int daysToWin;
    public int numberOfSubjects;
    public int numberOfInfectedOnStart;
}

public class LevelManager : MonoBehaviour
{
    
    private static LevelManager instance;
    public int currentLevel = 1;
    private Dictionary<int, GameSettings> levelDict;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(this);
        }
        levelDict = new Dictionary<int, GameSettings>();   
        GenerateLevels();
    }

    private void GenerateLevels() {
        levelDict[1] = new GameSettings() {
            levelNo = 1,
            gameOverScore = 2,
            gameWonScore = 20f,
            daysToWin = 20,
            numberOfSubjects = 20,
            newInfectionSeconds = 30f,
            numberOfInfectedOnStart = 5,
        };
        levelDict[2] = new GameSettings() {
            levelNo = 2,
            gameOverScore = 10,
            gameWonScore = 24f,
            daysToWin = 20,
            numberOfSubjects = 40,
            newInfectionSeconds = 6f,
            numberOfInfectedOnStart = 1,
        };
        levelDict[3] = new GameSettings() {
            levelNo = 3,
            gameOverScore = 15,
            gameWonScore = 28f,
            daysToWin = 20,
            numberOfSubjects = 40,
            newInfectionSeconds = 4f,
            numberOfInfectedOnStart = 1,
        };
        levelDict[4] = new GameSettings() {
            levelNo = 4,
            gameOverScore = 15,
            gameWonScore = 10f,
            daysToWin = 25,
            numberOfSubjects = 50,
            newInfectionSeconds = 2f,
            numberOfInfectedOnStart = 1,
        };
        levelDict[5] = new GameSettings() {
            levelNo = 5,
            gameOverScore = 15,
            gameWonScore = 42f,
            daysToWin = 30,
            numberOfSubjects = 80,
            newInfectionSeconds = 5f,
            numberOfInfectedOnStart = 1
        };
        levelDict[6] = new GameSettings() {
            levelNo = 6,
            gameOverScore = 20,
            gameWonScore = 49f,
            daysToWin = 35,
            numberOfSubjects = 150,
            newInfectionSeconds = 5f,
            numberOfInfectedOnStart = 1
        };
        levelDict[7] = new GameSettings() {
            levelNo = 7,
            gameOverScore = 20,
            gameWonScore = 84f,
            daysToWin = 60,
            numberOfSubjects = 180,
            newInfectionSeconds = 5f,
            numberOfInfectedOnStart = 3
        };
        levelDict[8] = new GameSettings() {
            levelNo = 8,
            gameOverScore = 40,
            gameWonScore = 84f,
            daysToWin = 60,
            numberOfSubjects = 200,
            newInfectionSeconds = 3f,
            numberOfInfectedOnStart = 5
        };
    }

    public void SetLevel(int lvl) {
        currentLevel = lvl;
    }

    public void IncrementLevel() {
        currentLevel++;
    }

    public GameSettings GetCurrentLevelSettings() {
        return levelDict[currentLevel];
    }

    public void LoadNextLevel() {
        SceneManager.LoadScene("MagnifyGlassLevel");
    }

}
