using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CurrentFindState {
    ShowingFind,
    Invisible
}

public enum CurrentFindType {
    Infected,
    Healthy,
    None
}

public class CurrentFind : MonoBehaviour
{

    [SerializeField]
    private GameObject SusceptibleAvatar;

    [SerializeField]
    private GameObject InfectedAvatar;

    [SerializeField]
    private CharacterManager SusceptibleManager;

    [SerializeField]
    private CharacterManager InfectedManager;

    [SerializeField]
    private GameObject currentFindObject;

    [SerializeField]
    private Text currentFindText;

    private CurrentFindState currentFindState = CurrentFindState.Invisible;
    private SubjectStatus currentFindType = SubjectStatus.Null;

    public float hideTime = 2f;
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        timer = hideTime;
        //SusceptibleManager.Walk();
        //InfectedManager.Walk();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentFindState == CurrentFindState.ShowingFind) {
            timer -= Time.deltaTime;
            if (timer < 0f) {
                currentFindObject.SetActive(false);
                currentFindState = CurrentFindState.Invisible;
                currentFindType = SubjectStatus.Null;  
            }
        } else if (currentFindState == CurrentFindState.Invisible) {
            timer = hideTime;
        }
    }

    public void OnRevealInfo(RevealInfo revealInfo) {
        if (currentFindType != revealInfo.subjectStatus) {
            currentFindObject.SetActive(true);
            if (revealInfo.subjectStatus == SubjectStatus.Infected) {
                ShowInfectedAvatar();
                currentFindType = SubjectStatus.Infected;
                timer = hideTime;
            } else if (revealInfo.subjectStatus == SubjectStatus.Healthy) {
                ShowSusceptibleAvatar();
                currentFindType = SubjectStatus.Healthy;
                timer = hideTime;
            }
            currentFindState = CurrentFindState.ShowingFind;
        }
    }

    void ShowSusceptibleAvatar() {
        SusceptibleAvatar.SetActive(true);
        InfectedAvatar.SetActive(false);
        SusceptibleManager.Walk();
        currentFindText.text = "HEALTHY";
    }

    void ShowInfectedAvatar() {
        InfectedAvatar.SetActive(true);
        SusceptibleAvatar.SetActive(false);
        InfectedManager.Walk();
        currentFindText.text = "INFECTED";
    }
}
