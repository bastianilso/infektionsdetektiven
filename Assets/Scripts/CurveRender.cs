using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class CurveCanvasSettings {
    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;
    public string xLabel;
    public string yLabel;
}

public class CurveRender : MonoBehaviour
{

    [SerializeField]
    private UILineRenderer curve;

    public string xLabel = "x";
    public string yLabel = "y";
    public float xMin = 0f;
    public float xMax = 1f;
    public float yMin = 0f;
    public float yMax = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private float GetCurveCoordinate(float num, float numMax) {
        return ((float) num / (float) numMax);
    }

    public void SetUpCurveCanvas(CurveCanvasSettings settings) {
        xLabel = settings.xLabel;
        yLabel = settings.yLabel;
        xMin = settings.xMin;
        xMax = settings.xMax;
        yMin = settings.yMin;
        yMax = settings.yMax;
    }

    public void GenerateCurveFromGameLogs(Dictionary<int, object> xCoords, Dictionary<int, object> yCoords) {
        int logCount = xCoords.Keys.Count;
        Vector2[] points = new Vector2[logCount];
        for (int i = 0; i < logCount; i++)
        {
            float xVal;
            float yVal;

            if (xCoords[i] is float) {
                xVal = GetCurveCoordinate((float) xCoords[i],xMax);
            } else if (xCoords[i] is int) {
                float f = System.Convert.ToSingle(yCoords[i]);
                xVal = GetCurveCoordinate(f,xMax);
            } else {
                xVal = 0;
            }

            if (yCoords[i] is float) {
                yVal = GetCurveCoordinate((float) yCoords[i], yMax);
            } else if (yCoords[i] is int) {
                float f = System.Convert.ToSingle(yCoords[i]);
                yVal = GetCurveCoordinate(f, yMax);
            } else {
                yVal = 0;
            }
            points[i] = new Vector2(xVal,yVal);
        }
        curve.Points = points;
    }

    public void GenerateCurveFromVector2(Vector2[] v) {
        Vector2[] points = new Vector2[v.Length];
        for (int i = 0; i < v.Length; i++) {
            points[i] = new Vector2(GetCurveCoordinate(v[i].x, xMax), GetCurveCoordinate(v[i].y, yMax));
        }
        curve.Points = points;
    }
}
