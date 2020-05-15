using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class InfectionStats : MonoBehaviour
{

    [SerializeField]
    private Text isolationScoreCounter;

    private string isolationScoreCounterTemplate;
    private GameState gameState;

    [SerializeField]
    private Text populationScoreCounter;

    private string populationScoreCounterTextTemplate;

    [SerializeField]
    private Font boldFont;
    private Font defaultFont;

    [SerializeField]
    private Color problematicColor;
    private Color defaultColor;

    [SerializeField]
    private GameObject infectionIndicator;
    private ProgressIndicator infProgBar;

    [SerializeField]
    private GameObject vaccineIndicator;
    private ProgressIndicator vacProgBar;

    private float gameTime = 0f;
    private float gameTargetTime = -1f;
    private int gameOverScore = -1;
    private int numberOfSubjects = -1;
    // One stats should be the number of infected you identify
    
    // The other stats should be the number of healthy people in the population

    // Start is called before the first frame update
    void Awake()
    {
        isolationScoreCounterTemplate = isolationScoreCounter.text;
        populationScoreCounterTextTemplate = populationScoreCounter.text;

        isolationScoreCounter.text = string.Format(isolationScoreCounterTemplate, 0);
        populationScoreCounter.text = string.Format(populationScoreCounterTextTemplate, 0);
        defaultFont = populationScoreCounter.font;
        defaultColor = populationScoreCounter.color;
        infProgBar = infectionIndicator.GetComponent<ProgressIndicator>();
        vacProgBar = vaccineIndicator.GetComponent<ProgressIndicator>();
        infectionIndicator.SetActive(false);
        vaccineIndicator.SetActive(false);
    }

    public void onGamePrep(GameSettings gameSettings) {
        gameTargetTime = gameSettings.gameWonScore;
        gameOverScore = gameSettings.gameOverScore;
        numberOfSubjects = gameSettings.numberOfSubjects;
    }

    public void onGameStateChanged(float time, GameState gs) {
        gameState = gs;
        if (gameState == GameState.Preparation) {
            gameTime = time;
        } else if (gameState == GameState.Playing) {
            vaccineIndicator.SetActive(true);
            infectionIndicator.SetActive(true);
            gameTime = time;
        }
    }

    void Update() {
        if (gameState == GameState.Playing) {
            gameTime += Time.deltaTime;
            vacProgBar.SetProgressBarValue(gameTime/gameTargetTime);
        }
    }

    //public void UpdateIsolationScore(int score) {
    //    isolationScoreCounter.text = string.Format(isolationScoreCounterTemplate, score);
    //}

    public void OnPopulationChange(GameStats gameStats) {
        float currentScore = 1f - ( ((float) gameStats.populationScore - gameOverScore) / (float) numberOfSubjects);
        infProgBar.SetProgressBarValue(currentScore);
    }

    //public void UpdatePopulationScore(int score) {
    //    populationScoreCounter.text = string.Format(populationScoreCounterTextTemplate, score);
    //    if (score < 50) {
    //            populationScoreCounter.font = boldFont;
    //            populationScoreCounter.color = problematicColor;
    //    } else {
    //            populationScoreCounter.font = defaultFont;
    //            populationScoreCounter.color = defaultColor;
    //    }
    //}
}
