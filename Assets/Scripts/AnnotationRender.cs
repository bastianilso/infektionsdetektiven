using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnnotationRender : MonoBehaviour
{

    [SerializeField]
    private RectTransform annotationObject;

    [SerializeField]
    private RectTransform canvasRect;

    public float xMin = 0f;
    public float xMax = 1f;
    public float yMin = 0f;
    public float yMax = 1f;

    private float xMinRect;
    private float xMaxRect;
    private float yMinRect;
    private float yMaxRect;

    public bool useX = false;
    public bool useY = false;
    public float xOffset = 0f;
    public float yOffset = 0f;

    // Start is called before the first frame update
    void Awake()
    {
        float xMinRect = canvasRect.rect.xMin;
        float xMaxRect = canvasRect.rect.xMax;
        float yMinRect = canvasRect.rect.yMin;
        float yMaxRect = canvasRect.rect.yMax;
    }

    //private float GetPointCoordinate(float num, float numMax, float numMinRect, float numMaxRect) {
        //float dist = numMaxRect - numMinRect;
        //float normNum = num / numMax;
        //float distPortion = dist * normNum;
        //float coordinate = numMinRect + distPortion;
        
    //}

    public void SetUpCurveCanvas(CurveCanvasSettings settings) {
        xMin = settings.xMin;
        xMax = settings.xMax;
        yMin = settings.yMin;
        yMax = settings.yMax;
    }

    public void SetPositionByVector2(Vector2 position) {
        Vector2 normVec = new Vector2( (position.x / xMax), (position.y / yMax) );
        Vector2 point = Rect.NormalizedToPoint(canvasRect.rect, normVec);
        //Debug.Log("positionX / xMax: " + (position.x / xMax).ToString());
        //Debug.Log("positionY / yMax: " + (position.y / yMax).ToString());
        //Debug.Log("point: " + point.ToString());
        if (useX) {
            //float newX = GetPointCoordinate(position.x, xMax, xMinRect, xMaxRect);
            annotationObject.anchoredPosition = new Vector2(point.x + xOffset, annotationObject.anchoredPosition.y + yOffset);
        }
        if (useY) {
            //float newY = GetPointCoordinate(position.y, yMax, yMinRect, yMaxRect);
            annotationObject.anchoredPosition = new Vector2(annotationObject.anchoredPosition.x + xOffset, point.y + yOffset);
        }
    }

}
