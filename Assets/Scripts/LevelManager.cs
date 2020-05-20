﻿using System.Collections;
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
            gameOverScore = 10,
            gameWonScore = 28f,
            daysToWin = 20,
            numberOfSubjects = 40,
            newInfectionSeconds = 4f,
            numberOfInfectedOnStart = 1,
        };
        levelDict[2] = new GameSettings() {
            levelNo = 2,
            gameOverScore = 15,
            gameWonScore = 35f,
            daysToWin = 25,
            numberOfSubjects = 50,
            newInfectionSeconds = 3f,
            numberOfInfectedOnStart = 1,
        };
        levelDict[3] = new GameSettings() {
            levelNo = 3,
            gameOverScore = 15,
            gameWonScore = 42f,
            daysToWin = 30,
            numberOfSubjects = 80,
            newInfectionSeconds = 5f,
            numberOfInfectedOnStart = 1
        };
        levelDict[4] = new GameSettings() {
            levelNo = 4,
            gameOverScore = 20,
            gameWonScore = 49f,
            daysToWin = 35,
            numberOfSubjects = 150,
            newInfectionSeconds = 5f,
            numberOfInfectedOnStart = 1
        };
        levelDict[5] = new GameSettings() {
            levelNo = 5,
            gameOverScore = 20,
            gameWonScore = 84f,
            daysToWin = 60,
            numberOfSubjects = 180,
            newInfectionSeconds = 5f,
            numberOfInfectedOnStart = 3
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
