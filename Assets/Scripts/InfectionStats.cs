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

    [SerializeField]
    private Text populationScoreCounter;

    private string populationScoreCounterTextTemplate;

    [SerializeField]
    private Font boldFont;
    private Font defaultFont;

    [SerializeField]
    private Color problematicColor;
    private Color defaultColor;

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
        
    }

    public void UpdateIsolationScore(int score) {
            isolationScoreCounter.text = string.Format(isolationScoreCounterTemplate, score);
    }

    public void UpdatePopulationScore(int score) {
        populationScoreCounter.text = string.Format(populationScoreCounterTextTemplate, score);
        if (score < 50) {
                populationScoreCounter.font = boldFont;
                populationScoreCounter.color = problematicColor;
        } else {
                populationScoreCounter.font = defaultFont;
                populationScoreCounter.color = defaultColor;
        }
    }
}
