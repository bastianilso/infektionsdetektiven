using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class TutorialManager : MonoBehaviour
{

    public int numberOfSubjects = 50;
    public int numberOfInfections = 4;

    private PopulationManager populationManager;

    private float flashTimer = 0f;
    private float restartTime = 20f;
    private float restartTimer = 0f;

    [Serializable]
    public class OnFlash : UnityEvent {}
    public OnFlash onFlash;

    // Start is called before the first frame update
    void Start()
    {
        populationManager = this.GetComponent<PopulationManager>();
        StartCoroutine(PrepareTutorial());
    }

    // Update is called once per frame
    void Update()
    {
        flashTimer += Time.deltaTime;
        if (flashTimer > 5f) {
            flashTimer = 0f;
            onFlash.Invoke();
        }

        restartTimer += Time.deltaTime;
        if (restartTimer > restartTime) {
            populationManager.InfectRandomSubject(1);        
            restartTimer = 0f;

        }
        
    }

    public IEnumerator PrepareTutorial() {
        populationManager.StartPopulation(numberOfSubjects);
        yield return new WaitForSeconds(2f);
        populationManager.InfectRandomSubject(2);
    }

}
