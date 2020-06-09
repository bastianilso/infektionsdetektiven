using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpreadPanel : MonoBehaviour
{

    private GameStats stats;

    private EventLogger eventLog;

    [SerializeField]
    private GameObject spreadUI;

    [SerializeField]
    private CurveRender infectionCurve;

    [SerializeField]
    private CurveRender gameOverCurve;

    [SerializeField]
    private CurveRender isolationCurve;

    [SerializeField]
    private CurveRender timeCurve;

    [SerializeField]
    private AnnotationRender gameOverAnnotation;

    [SerializeField]
    private AnnotationRender infectionAnnotation;

    [SerializeField]
    private AnnotationRender isolationAnnotation;

    [SerializeField]
    private AnnotationRender timeAnnotation;

    [SerializeField]
    private Text timeAnnotationText;
    private string timeAnnotationTextTemplate;

    [SerializeField]
    private Text spreadText;
    private string spreadTextTemplate;

    [SerializeField]
    private Text isolationEffectiveText;
    private string isolationEffectiveTemplate;

    [SerializeField]
    private Text xMaxLabel;
    private string xMaxLabelTemplate;

    [SerializeField]
    private Text yMaxLabel;
    private string yMaxLabelTemplate;

    Dictionary<string, Dictionary<int, object>> gameLogs;
    int logCount;

    // Start is called before the first frame update
    void Awake()
    {
        xMaxLabelTemplate = xMaxLabel.text;
        yMaxLabelTemplate = yMaxLabel.text;
        spreadTextTemplate = spreadText.text;
        isolationEffectiveTemplate = isolationEffectiveText.text;
        timeAnnotationTextTemplate = timeAnnotationText.text;
        eventLog = GameObject.Find("Logging").GetComponent<EventLogger>();
    }

    public void OnGameOver(GameStats gameStats, GameState gameState) {
        stats = gameStats;
        gameLogs = eventLog.GetGameLogs();
        logCount = (gameLogs["GameTime"].Keys.Count-1);
    }

    public void Activate() {
        spreadUI.SetActive(true);
        Dictionary<string, object> newEvent = new Dictionary<string, object>() {
            {"Event", "ViewingStatistics"},
            {"EventType", "StatsEvent"},
        };
        eventLog.AddToEventLog(newEvent);
        Vector2 annotationPos;
        CurveCanvasSettings settings = new CurveCanvasSettings();
        settings.xMin = 0f;
        settings.xMax = (float) gameLogs["GameTime"][logCount];
        settings.yMin = 0f;
        settings.yMax = stats.numberOfSubjects;
        xMaxLabel.text = string.Format(xMaxLabelTemplate, stats.daysToWin.ToString());
        yMaxLabel.text = string.Format(yMaxLabelTemplate, stats.numberOfSubjects.ToString());

        UpdateText(0.5f);

        infectionCurve.SetUpCurveCanvas(settings);
        infectionCurve.GenerateCurveFromGameLogs(gameLogs["GameTime"], gameLogs["NumberOfInfected"]);

        infectionAnnotation.SetUpCurveCanvas(settings);
        annotationPos = new Vector2(stats.gameTime, System.Convert.ToSingle(gameLogs["NumberOfInfected"][logCount]) );
        infectionAnnotation.SetPositionByVector2(annotationPos);

        isolationCurve.SetUpCurveCanvas(settings);
        isolationCurve.GenerateCurveFromGameLogs(gameLogs["GameTime"], gameLogs["NumberOfIsolated"]);

        isolationAnnotation.SetUpCurveCanvas(settings);
        annotationPos = new Vector2(stats.gameTime, System.Convert.ToSingle(gameLogs["NumberOfIsolated"][logCount]));
        isolationAnnotation.SetPositionByVector2(annotationPos);

        CurveCanvasSettings timeSettings = new CurveCanvasSettings();
        timeSettings.xMin = 0f;
        timeSettings.xMax = 1f;
        timeSettings.yMin = 0f;
        timeSettings.yMax = 1f;
        timeCurve.SetUpCurveCanvas(timeSettings);
        timeAnnotation.SetUpCurveCanvas(timeSettings);

        CurveCanvasSettings gameOverSettings = new CurveCanvasSettings();
        gameOverSettings.xMin = 0f;
        gameOverSettings.xMax = 1f;
        gameOverSettings.yMin = 0f;
        gameOverSettings.yMax = stats.numberOfSubjects;
        gameOverCurve.SetUpCurveCanvas(gameOverSettings);

        Vector2[] gameOverData = new Vector2[2];
        gameOverData[0] = new Vector2(0f, (float) (stats.numberOfSubjects - stats.gameOverScore) );
        gameOverData[1] = new Vector2(1f, (float) (stats.numberOfSubjects - stats.gameOverScore) );
        gameOverCurve.GenerateCurveFromVector2(gameOverData);

        gameOverAnnotation.SetUpCurveCanvas(gameOverSettings);
        annotationPos = new Vector2(0f, (float) (stats.numberOfSubjects - stats.gameOverScore));
        gameOverAnnotation.SetPositionByVector2(annotationPos);
    }

    private void UpdateText(float timeVal) {
        timeVal = timeVal > 1f ? 1f : timeVal;
        timeVal = timeVal < 0f ? 0f : timeVal;

        int chosenDay = Mathf.RoundToInt((float) stats.daysToWin * timeVal);
        int logCount = Mathf.RoundToInt((float) (gameLogs["GameTime"].Keys.Count-1) * timeVal);

        int numInfected = (int) gameLogs["NumberOfInfected"][logCount];
        int numIsolated = (int) gameLogs["NumberOfIsolated"][logCount];

        float isolationEffective = (float) numIsolated / (float) numInfected * 100f;
        Debug.Log("numIsolated: " + numIsolated + ", numInfected: " + numInfected + ", isolationEff: " + isolationEffective + ", logCount: " + logCount + ", timeVal: " + timeVal);
        isolationEffectiveText.text = string.Format(isolationEffectiveTemplate, isolationEffective.ToString("F0"));
        if (numIsolated == 0) {
            isolationEffectiveText.text = "Du isolerede endnu ikke nogen syge.";
        } 

        timeAnnotationText.text = string.Format(timeAnnotationTextTemplate, chosenDay.ToString());
        spreadText.text = string.Format(spreadTextTemplate, chosenDay.ToString(), numInfected.ToString());
    }

    public void OnSliderChanged(float val) {
        Vector2[] timeCurveData = new Vector2[2];
        timeCurveData[0] = new Vector2(val, 0f);
        timeCurveData[1] = new Vector2(val, 1f);
        timeCurve.GenerateCurveFromVector2(timeCurveData);
        timeAnnotation.SetPositionByVector2(timeCurveData[0]);
        UpdateText(val);
    }

}
