using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameOver : MonoBehaviour
{

    //[SerializeField]
    //private Text numberOfSubjects;
    //private string numberOfSubjectsTextTemplate;

    [SerializeField]
    private Color infectionColor;
    [SerializeField]
    private Color isolationColor;
    [SerializeField]
    private Color vaccineColor;

    [SerializeField]
    private Image infectionStar;
    [SerializeField]
    private Image isolationStar;
    [SerializeField]
    private Image vaccineStar;
    [SerializeField]
    private Image NoinfectionStar;
    [SerializeField]
    private Image NoisolationStar;
    [SerializeField]
    private Image NovaccineStar;


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

    [SerializeField]
    private LoggingManager eventLogger;

    // Start is called before the first frame update
    void Awake()
    {
        //numberOfSubjectsTextTemplate = numberOfSubjects.text;
        numberOfSubjectsInfectedTemplate = numberOfSubjectsInfected.text;
        numberOfSubjectsInIsolationTemplate = numberOfSubjectsInIsolation.text;
        numberOfSubjectsVacTemplate = numberOfSubjectsVac.text;
        eventLogger = GameObject.Find("Logging").GetComponent<LoggingManager>();
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
            int subjectsVacScore = gameStats.numberOfSubjects - gameStats.subjectsInfectedScore;
            numberOfSubjectsVac.text = string.Format(numberOfSubjectsVacTemplate, subjectsVacScore.ToString());
            StartCoroutine(EvaluateHighScore(gameStats));
        } else if (gameState == GameState.GameLost) {
            GameLostPanel.SetActive(true);
            NextLevelButton.SetActive(false);
            RetryButton.SetActive(true); 
            GameWonPanel.SetActive(false);
            numberOfSubjectsVac.text = string.Format(numberOfSubjectsVacTemplate, "Ingen");
            NoinfectionStar.gameObject.SetActive(false);
            NoisolationStar.gameObject.SetActive(false);
            NovaccineStar.gameObject.SetActive(false);

        }

        //numberOfSubjects.text = string.Format(numberOfSubjectsTextTemplate, gameStats.numberOfSubjects.ToString());
        numberOfSubjectsInfected.text = string.Format(numberOfSubjectsInfectedTemplate, gameStats.subjectsInfectedScore.ToString());
        numberOfSubjectsInIsolation.text = string.Format(numberOfSubjectsInIsolationTemplate, gameStats.subjectsIsolationScore.ToString());
    }

    private IEnumerator EvaluateHighScore(GameStats gameStats) {
        yield return new WaitForSeconds(1f);
        // If less than 30% received the infection, award.
        float popGotInf = ((float) gameStats.numberOfSubjects) * 0.35f;
        if (popGotInf >= gameStats.subjectsInfectedScore) {
            infectionStar.gameObject.SetActive(true);
            infectionStar.color = infectionColor;
            NoinfectionStar.color = new Color(1f,1f,1f,1f);
            Dictionary<string, object> eventLog = new Dictionary<string, object>() {
                {"Event", "GotInfectionStarAward"},
                {"EventType", "StatsEvent"},
            };
            eventLogger.Log("Event", eventLog);
        }
        yield return new WaitForSeconds(0.5f);

        // if all infected except 3 (or fewer) were isolated, award.
        int popGotIso = gameStats.subjectsInfectedScore - 5;
        if (popGotIso <= gameStats.subjectsIsolationScore) {
            isolationStar.gameObject.SetActive(true);
            isolationStar.color = isolationColor;
            NoisolationStar.color = new Color(1f,1f,1f,1f);
            Dictionary<string, object> eventLog = new Dictionary<string, object>() {
                {"Event", "GotIsolationStarAward"},
                {"EventType", "StatsEvent"},
            };
            eventLogger.Log("Event", eventLog);
        }
        yield return new WaitForSeconds(0.5f);

        // If more than 80% received the vaccine, award.
        float popNeedVac = ((float) gameStats.numberOfSubjects) * 0.75f;
        int subjectsVacScore = gameStats.numberOfSubjects - gameStats.subjectsInfectedScore;
        if (popNeedVac <= subjectsVacScore) {
            vaccineStar.gameObject.SetActive(true);
            vaccineStar.color = vaccineColor;
            NovaccineStar.color = new Color(1f,1f,1f,1f);
            Dictionary<string, object> eventLog = new Dictionary<string, object>() {
                {"Event", "GotVaccinationStarAward"},
                {"EventType", "StatsEvent"},
            };
            eventLogger.Log("Event", eventLog);
        }

        yield return null;

    }
}
