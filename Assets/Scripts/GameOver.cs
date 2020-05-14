using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{

    [SerializeField]
    private Text numberOfSubjects;
    private string numberOfSubjectsTextTemplate;

    [SerializeField]
    private Text numberOfSubjectsInfected;
    private string numberOfSubjectsInfectedTemplate;

    [SerializeField]
    private Text numberOfSubjectsTested;
    private string numberOfSubjectsTestedTemplate;

    [SerializeField]
    private Text numberOfSubjectsInIsolation;
    private string numberOfSubjectsInIsolationTemplate;

    [SerializeField]
    private GameObject GameWonPanel;

    [SerializeField]
    private GameObject GameLostPanel;

    // Start is called before the first frame update
    void Awake()
    {
        numberOfSubjectsTextTemplate = numberOfSubjects.text;
        numberOfSubjectsInfectedTemplate = numberOfSubjectsInfected.text;
        numberOfSubjectsInIsolationTemplate = numberOfSubjectsInIsolation.text;
        numberOfSubjectsTestedTemplate = numberOfSubjectsTested.text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnGameOver(GameStats gameStats, GameState gameState) {
        if (gameState == GameState.GameWon) {
            GameWonPanel.SetActive(true); 
            GameLostPanel.SetActive(false);   
        } else if (gameState == GameState.GameLost) {
            GameLostPanel.SetActive(true);
            GameWonPanel.SetActive(false);
        }

        numberOfSubjects.text = string.Format(numberOfSubjectsTextTemplate, gameStats.numberOfSubjects.ToString());
        numberOfSubjectsInfected.text = string.Format(numberOfSubjectsInfectedTemplate, gameStats.subjectsInfectedScore.ToString());
        numberOfSubjectsInIsolation.text = string.Format(numberOfSubjectsInIsolationTemplate, gameStats.subjectsIsolationScore.ToString());
        numberOfSubjectsTested.text = string.Format(numberOfSubjectsTestedTemplate, gameStats.subjectsTestedScore.ToString());

    }
}
