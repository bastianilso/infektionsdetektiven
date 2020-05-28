using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadPanel : MonoBehaviour
{

    private GameStats stats;

    [SerializeField]
    private EventLogger eventLog;

    [SerializeField]
    private GameObject spreadUI;

    [SerializeField]
    private InfectionCurveRender infectionCurve;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnGameOver(GameStats gameStats, GameState gameState) {
        stats = gameStats;
    }

    public void Activate() {
        spreadUI.SetActive(true);
        CurveCanvasSettings settings = new CurveCanvasSettings();
        settings.xMin = 0f;
        settings.xMax = stats.gameTime;
        settings.yMin = 0f;
        settings.yMax = stats.numberOfSubjects;
        settings.xLabel = "Dage";
        settings.yLabel = "Inficerede";
        infectionCurve.SetUpCurveCanvas(settings);

        Dictionary<string, Dictionary<int, object>> gameLogs = eventLog.GetGameLogs();
        infectionCurve.GenerateCurve(gameLogs["GameTime"], gameLogs["NumberOfInfected"]);
    }

}
