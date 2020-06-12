using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartRater : MonoBehaviour
{

    [SerializeField]
    private Image heartRating1;

    [SerializeField]
    private Image heartRating2;

    [SerializeField]
    private Image heartRating3;    

    [SerializeField]
    private Image heartRating4;

    [SerializeField]
    private Image heartRating5;

    [SerializeField]
    private Sprite heartFull;

    [SerializeField]
    private Sprite heartHalf;

    [SerializeField]
    private GameObject nextButton;

    private int currentRating = -1;

    private LoggingManager eventLogger;

    // Start is called before the first frame update
    void Start()
    {
        eventLogger = GameObject.Find("Logging").GetComponent<LoggingManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Heart1Click() {
        currentRating = 1;
        heartRating1.sprite = heartFull;
        heartRating2.sprite = heartHalf;
        heartRating3.sprite = heartHalf;
        heartRating4.sprite = heartHalf;
        heartRating5.sprite = heartHalf;
        nextButton.SetActive(true);
    }

    public void Heart2Click() {
        currentRating = 2;
        heartRating1.sprite = heartFull;
        heartRating2.sprite = heartFull;
        heartRating3.sprite = heartHalf;
        heartRating4.sprite = heartHalf;
        heartRating5.sprite = heartHalf;
        nextButton.SetActive(true);
    }

    public void Heart3Click() {
        currentRating = 3;
        heartRating1.sprite = heartFull;
        heartRating2.sprite = heartFull;
        heartRating3.sprite = heartFull;
        heartRating4.sprite = heartHalf;
        heartRating5.sprite = heartHalf;
        nextButton.SetActive(true);
    }

    public void Heart4Click() {
        currentRating = 4;
        heartRating1.sprite = heartFull;
        heartRating2.sprite = heartFull;
        heartRating3.sprite = heartFull;
        heartRating4.sprite = heartFull;
        heartRating5.sprite = heartHalf;
        nextButton.SetActive(true);
    }

    public void Heart5Click() {
        currentRating = 5;
        heartRating1.sprite = heartFull;
        heartRating2.sprite = heartFull;
        heartRating3.sprite = heartFull;
        heartRating4.sprite = heartFull;
        heartRating5.sprite = heartFull;
        nextButton.SetActive(true);
    }

    public void onNextButtonClick() {
        Dictionary<string, object> eventLog = new Dictionary<string, object>() {
            {"Event", "Evaluation"},
            {"EventType", "EvalEvent"},
        };
        eventLogger.Log("Event", eventLog);
        Dictionary<string, object> metaLog = new Dictionary<string, object>() {
            {"HowMuchDoYouLikeGame", currentRating},
        };
        eventLogger.Log("Meta", metaLog, LogMode.Overwrite);
        eventLogger.SaveLog("Meta");
    }
}
