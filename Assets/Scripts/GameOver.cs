using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{

    //[SerializeField]
    //private Text numberOfSubjects;
    //private string numberOfSubjectsTextTemplate;

    [SerializeField]
    private Text numberOfSubjectsInfected;
    private string numberOfSubjectsInfectedTemplate;

    [SerializeField]
    private Text numberOfSubjectsVac;
    private string numberOfSubjectsVacTemplate;

    [SerializeField]
    private Text numberOfSubjectsInIsolation;
    private string numberOfSubjectsInIsolationTemplate;

    [SerializeField]
    private GameObject GameWonPanel;

    [SerializeField]
    private GameObject GameLostPanel;

    [SerializeField]
    private GameObject NextLevelButton;

    [SerializeField]
    private GameObject RetryButton;

    // Start is called before the first frame update
    void Awake()
    {
        //numberOfSubjectsTextTemplate = numberOfSubjects.text;
        numberOfSubjectsInfectedTemplate = numberOfSubjectsInfected.text;
        numberOfSubjectsInIsolationTemplate = numberOfSubjectsInIsolation.text;
        numberOfSubjectsVacTemplate = numberOfSubjectsVac.text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnGameOver(GameStats gameStats, GameState gameState) {
        if (gameState == GameState.GameWon) {
            GameWonPanel.SetActive(true);
            NextLevelButton.SetActive(true);
            RetryButton.SetActive(false); 
            GameLostPanel.SetActive(false);   
        } else if (gameState == GameState.GameLost) {
            GameLostPanel.SetActive(true);
            NextLevelButton.SetActive(false);
            RetryButton.SetActive(true); 
            GameWonPanel.SetActive(false);
        }

        //numberOfSubjects.text = string.Format(numberOfSubjectsTextTemplate, gameStats.numberOfSubjects.ToString());
        numberOfSubjectsInfected.text = string.Format(numberOfSubjectsInfectedTemplate, gameStats.subjectsInfectedScore.ToString());
        numberOfSubjectsInIsolation.text = string.Format(numberOfSubjectsInIsolationTemplate, gameStats.subjectsIsolationScore.ToString());
        int subjectsVacScore = gameStats.numberOfSubjects - gameStats.subjectsInfectedScore;
        numberOfSubjectsVac.text = string.Format(numberOfSubjectsVacTemplate, subjectsVacScore.ToString());

    }
}
