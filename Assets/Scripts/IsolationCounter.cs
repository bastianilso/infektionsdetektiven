using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IsolationCounter : MonoBehaviour
{

    [SerializeField]
    private Text isolationCounter;
    private Vector4 textColor;
    [SerializeField]
    private Image isolationCounterImg;
    [SerializeField]
    private Text isolationText;
    private string isoCounterTemplate;

    private float isolationVisibilitySpeed = 0.02f;
    private bool fadeout = false;
    private float isolationVisT = 0f;
    private int subjectsIsolationScore = 0;

    // Start is called before the first frame update
    void Awake()
    {
        isoCounterTemplate = isolationCounter.text;
        textColor = new Vector4(isolationCounter.color.r,isolationCounter.color.g,isolationCounter.color.b,isolationCounter.color.a);
        isolationCounter.color = new Vector4(1f, 1f, 1f, 0f);
        isolationCounterImg.color = new Vector4(1f, 1f, 1f, 0f);
        isolationText.color = new Vector4(1f, 1f, 1f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isolationVisT > 0 && fadeout) {
            isolationVisT -= isolationVisibilitySpeed;
            isolationCounter.color = Vector4.Lerp(new Vector4(1f, 1f, 1f, 0f), textColor, isolationVisT);
            isolationCounterImg.color = Vector4.Lerp(new Vector4(1f, 1f, 1f, 0f), new Vector4(1f, 1f, 1f, 1f), isolationVisT);
            isolationText.color = Vector4.Lerp(new Vector4(1f, 1f, 1f, 0f), textColor, isolationVisT);
            if (isolationVisT < 0 ) {
                fadeout = false;
            }
        }
    }

    public void onPopulationUpdate(GameStats gameStats) {
        if (subjectsIsolationScore < gameStats.subjectsIsolationScore) {
            subjectsIsolationScore = gameStats.subjectsIsolationScore;
            isolationVisT = 2f;
            fadeout = true;
            isolationCounter.text = string.Format(isoCounterTemplate, gameStats.subjectsIsolationScore.ToString());
        }
    }
}
