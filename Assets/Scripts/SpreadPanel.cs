using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpreadPanel : MonoBehaviour
{

    private GameStats stats;

    [SerializeField]
    private EventLogger eventLog;

    [SerializeField]
    private GameObject spreadUI;

    [SerializeField]
    private CurveRender infectionCurve;

    [SerializeField]
    private CurveRender gameOverCurve;

    [SerializeField]
    private AnnotationRender gameOverAnnotation;

    [SerializeField]
    private AnnotationRender infectionAnnotation;

    [SerializeField]
    private CurveRender isolationCurve;

    [SerializeField]
    private AnnotationRender isolationAnnotation;

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

    // Start is called before the first frame update
    void Awake()
    {
        xMaxLabelTemplate = xMaxLabel.text;
        yMaxLabelTemplate = yMaxLabel.text;
        spreadTextTemplate = spreadText.text;
        isolationEffectiveTemplate = isolationEffectiveText.text;
        
    }

    public void OnGameOver(GameStats gameStats, GameState gameState) {
        stats = gameStats;
        gameLogs = eventLog.GetGameLogs();
    }

    public void Activate() {
        spreadUI.SetActive(true);
        Vector2 annotationPos;
        CurveCanvasSettings settings = new CurveCanvasSettings();
        settings.xMin = 0f;
        settings.xMax = stats.gameWonScore;
        settings.yMin = 0f;
        settings.yMax = stats.numberOfSubjects;
        settings.xLabel = "Dage";
        settings.yLabel = "Inficerede";
        infectionCurve.SetUpCurveCanvas(settings);

        xMaxLabel.text = string.Format(xMaxLabelTemplate, stats.daysToWin.ToString());
        yMaxLabel.text = string.Format(yMaxLabelTemplate, stats.numberOfSubjects.ToString());

        int chosenDay = stats.daysToWin / 2;
        int logCount = gameLogs["GameTime"].Keys.Count;
        UpdateText(chosenDay, logCount / 2);

        infectionCurve.GenerateCurveFromGameLogs(gameLogs["GameTime"], gameLogs["NumberOfInfected"]);

        infectionAnnotation.SetUpCurveCanvas(settings);
        annotationPos = new Vector2(stats.gameTime, (float) stats.subjectsInfectedScore);
        infectionAnnotation.SetPositionByVector2(annotationPos);

        isolationCurve.SetUpCurveCanvas(settings);
        isolationCurve.GenerateCurveFromGameLogs(gameLogs["GameTime"], gameLogs["NumberOfIsolated"]);

        isolationAnnotation.SetUpCurveCanvas(settings);
        annotationPos = new Vector2(stats.gameTime, (float) stats.subjectsIsolationScore);
        isolationAnnotation.SetPositionByVector2(annotationPos);

        settings.xMin = 0f;
        settings.xMax = 1f;
        gameOverCurve.SetUpCurveCanvas(settings);
        Vector2[] gameOverData = new Vector2[2];
        gameOverData[0] = new Vector2(0f, (float) (stats.numberOfSubjects - stats.gameOverScore) );
        gameOverData[1] = new Vector2(1f, (float) (stats.numberOfSubjects - stats.gameOverScore) );
        gameOverCurve.GenerateCurveFromVector2(gameOverData);

        gameOverAnnotation.SetUpCurveCanvas(settings);
        annotationPos = new Vector2(0f, (float) (stats.numberOfSubjects - stats.gameOverScore));
        gameOverAnnotation.SetPositionByVector2(annotationPos);
    }

    private void UpdateText(int chosenDay, int logCount) {
        float isolationEffective = (float) stats.subjectsIsolationScore / (float) stats.subjectsInfectedScore * 100f;
        isolationEffectiveText.text = string.Format(isolationEffectiveTemplate, isolationEffective.ToString("00"));

        int numInfected = (int) gameLogs["NumberOfInfected"][logCount];
        spreadText.text = string.Format(spreadTextTemplate, chosenDay.ToString(), numInfected.ToString());
    }

}
