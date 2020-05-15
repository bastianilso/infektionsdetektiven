﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;



public class ProgressIndicator : MonoBehaviour
{

    [SerializeField]
    private RectTransform progressBar;

    private float progressBarSize;

    // Start is called before the first frame update
    void Awake()
    {   
        progressBarSize = progressBar.sizeDelta.x;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void onChargeUpdate(ChargeInfo chargeInfo) {
        SetProgressBarValue(chargeInfo.currentCharge);

    }

    public void SetProgressBarValue(float pgValue) {
        // do the math on how to set the size of the bar here.
        float newPosition = pgValue * progressBarSize;
        progressBar.sizeDelta = new Vector2(newPosition, progressBar.sizeDelta.y);
    }
}
