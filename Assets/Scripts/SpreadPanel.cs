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
    private Text xMaxLabel;
    private string xMaxLabelTemplate;

    [SerializeField]
    private Text yMaxLabel;
    private string yMaxLabelTemplate;

    // Start is called before the first frame update
    void Awake()
    {
        xMaxLabelTemplate = xMaxLabel.text;
        yMaxLabelTemplate = yMaxLabel.text;
        
    }

    public void OnGameOver(GameStats gameStats, GameState gameState) {
        stats = gameStats;
    }

    public void Activate() {
        spreadUI.SetActive(true);
        CurveCanvasSettings settings = new CurveCanvasSettings();
        settings.xMin = 0f;
        settings.xMax = stats.gameWonScore;
        settings.yMin = 0f;
        settings.yMax = stats.numberOfSubjects;
        settings.xLabel = "Dage";
        settings.yLabel = "Inficerede";
        infectionCurve.SetUpCurveCanvas(settings);

        Dictionary<string, Dictionary<int, object>> gameLogs = eventLog.GetGameLogs();
        infectionCurve.GenerateCurveFromGameLogs(gameLogs["GameTime"], gameLogs["NumberOfInfected"]);

        settings.xMin = 0f;
        settings.xMax = 1f;
        gameOverCurve.SetUpCurveCanvas(settings);
        Vector2[] gameOverData = new Vector2[2];
        gameOverData[0] = new Vector2(0f, (float) (stats.numberOfSubjects - stats.gameOverScore) );
        gameOverData[1] = new Vector2(1f, (float) (stats.numberOfSubjects - stats.gameOverScore) );
        gameOverCurve.GenerateCurveFromVector2(gameOverData);

        xMaxLabel.text = string.Format(xMaxLabelTemplate, stats.daysToWin.ToString());
        yMaxLabel.text = string.Format(yMaxLabelTemplate, stats.numberOfSubjects.ToString());

        gameOverAnnotation.SetUpCurveCanvas(settings);
        Vector2 annotationPos = new Vector2(0f, (float) (stats.numberOfSubjects - stats.gameOverScore));
        gameOverAnnotation.SetPositionByVector2(annotationPos);
    }

}
