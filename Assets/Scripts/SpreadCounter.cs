﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpreadCounter : MonoBehaviour
{
    [SerializeField]
    private Text infectionCounter;
    private Vector4 textColor;
    [SerializeField]
    private Image infectionCounterImg;
    [SerializeField]
    private Image infectionIcon;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private AudioSource audioSource;

    private float infectionVisibilitySpeed = 2f;
    private bool fadeout = false;
    private float infectionVisT = 0f;

    // Start is called before the first frame update
    void Awake()
    {
        textColor = new Vector4(infectionCounter.color.r,infectionCounter.color.g,infectionCounter.color.b,infectionCounter.color.a);
        infectionCounter.color = new Vector4(1f, 1f, 1f, 0f);
        infectionCounterImg.color = new Vector4(1f, 1f, 1f, 0f);
        infectionIcon.color = new Vector4(1f, 1f, 1f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (infectionVisT > 0 && fadeout) {
            infectionVisT -= infectionVisibilitySpeed * Time.deltaTime;
            infectionCounter.color = Vector4.Lerp(new Vector4(1f, 1f, 1f, 0f), textColor, infectionVisT);
            infectionCounterImg.color = Vector4.Lerp(new Vector4(1f, 1f, 1f, 0f), new Vector4(1f, 1f, 1f, 1f), infectionVisT);
            infectionIcon.color = Vector4.Lerp(new Vector4(1f, 1f, 1f, 0f), new Vector4(1f, 1f, 1f, 1f), infectionVisT);
            if (infectionVisT < 0 ) {
                fadeout = false;
            }
        }
    }

    //public void onPopulationUpdate(GameStats gameStats) {
    //    if (subjectsInfectedScore < gameStats.subjectsInfectedScore) {
    //        subjectsInfectedScore = gameStats.subjectsInfectedScore;
    //        subjectsIsolationScore = gameStats.subjectsIsolationScore;
    //        int subjectsLeft = subjectsInfectedScore - subjectsIsolationScore;
    //        if ( subjectsLeft > scoreList[index]) {
    //
    //            index++;
    //        }
    //    }
    //}

    public void onTooMuchSpread(int scoreThreshold, int subjectsLeftInfected) {
        infectionVisT = 2.5f;
        fadeout = true;
        infectionCounter.text = subjectsLeftInfected.ToString();
        animator.Play("spreadcount-scaler");
        audioSource.Play();
    }
}
