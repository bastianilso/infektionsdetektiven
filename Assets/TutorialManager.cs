using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class TutorialManager : MonoBehaviour
{

    public int numberOfSubjects = 50;
    public int numberOfInfections = 1;

    private PopulationManager populationManager;

    private float flashTimer = 0f;

    [Serializable]
    public class OnFlash : UnityEvent {}
    public OnFlash onFlash;

    // Start is called before the first frame update
    void Start()
    {
        populationManager = this.GetComponent<PopulationManager>();
        PrepareTutorial();
    }

    // Update is called once per frame
    void Update()
    {
        flashTimer += Time.deltaTime;
        if (flashTimer > 1f) {
            flashTimer = 0f;
            onFlash.Invoke();
        }
        
    }

    public void PrepareTutorial() {
        populationManager.StartPopulation(numberOfSubjects);
        populationManager.SetNumberOfInfectedOnStart(numberOfInfections);
        populationManager.StartInfection();
    }

}
