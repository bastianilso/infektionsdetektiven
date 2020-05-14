using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePrep : MonoBehaviour
{

    [SerializeField]
    private Text headerText;
    private string headerTextTemplate;

    [SerializeField]
    private Text descriptionText;
    private string descriptionTextTemplate;

    // Start is called before the first frame update
    void Awake()
    {
        descriptionTextTemplate = descriptionText.text;
        headerTextTemplate = headerText.text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onGamePrep(GameSettings gameSettings) {
        headerText.text = string.Format(headerTextTemplate, gameSettings.levelNo.ToString());
        descriptionText.text = string.Format(descriptionTextTemplate, gameSettings.daysToWin.ToString());
    }
}
