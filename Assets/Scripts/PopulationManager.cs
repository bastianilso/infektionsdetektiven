using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class PopulationManager : MonoBehaviour
{

    [SerializeField]
    private GameObject subjectTemplate;

    [SerializeField]
    private SpawnZone spawnZone;

    private Dictionary<int, SubjectManager> allSubjects;

    //private Dictionary<int, MetaSubject> healthySubjects;
    private Dictionary<int, SubjectManager> infectedSubjects;

    [Serializable]
    public class OnSubjectSpawned : UnityEvent <SubjectManager> {}
    public OnSubjectSpawned onSubjectSpawned;

    private int lastId = 0;

    // Start is called before the first frame update
    void Awake()
    {
        allSubjects = new Dictionary<int, SubjectManager>();
        infectedSubjects = new Dictionary<int, SubjectManager>();
    }

    public void StartPopulation(int numberOfSubjects) {
        StartCoroutine("SpawnSubjects", numberOfSubjects);
    }

    public void GetNextInfectedPosition() {
        
    }

    private IEnumerator SpawnSubjects(int num) {
        for (int i = 0; i < num; i++) {          
            var obj = Instantiate(subjectTemplate);
            Vector3 newSpawnPoint = spawnZone.SpawnPoint;
            obj.transform.localPosition = new Vector3(newSpawnPoint.x, 0f, newSpawnPoint.z);
            obj.SetActive(true);

            allSubjects[lastId] = obj.GetComponent<SubjectManager>();
            allSubjects[lastId].id = lastId;
            onSubjectSpawned.Invoke(allSubjects[lastId]);
            lastId++;
            yield return new WaitForSeconds(0.00005f);
        }
        yield return null;

    }

    public void StartInfection(int numberOfInfected) {
        InfectRandomSubject(numberOfInfected);
    }

    public void InfectSubject(int id) {
        allSubjects[id].SetSubjectStatus(SubjectStatus.Infected);
    }

    public void InfectRandomSubject(int count) {
        for (int i = 0; i < count; i++) {
            int randomID = UnityEngine.Random.Range(0,allSubjects.Count-1);
            allSubjects[randomID].SetSubjectStatus(SubjectStatus.Infected);
        }
    }

    public void OnSubjectStatusChanged(int id, SubjectStatus subjectStatus) {
        if (subjectStatus == SubjectStatus.Infected) {
            infectedSubjects[id] = allSubjects[id];
        }
    }

    public void AddNewInfected() {
        List<SubjectManager> spreadingSubjects = new List<SubjectManager>();

        foreach(var entry in infectedSubjects)
        {
            spreadingSubjects.Add(entry.Value);
        }

        StartCoroutine("SpreadInfection", spreadingSubjects);
    }

    private IEnumerator SpreadInfection(List<SubjectManager> spreadingSubjects) {

        foreach(var subject in spreadingSubjects) {
            Debug.Log(subject.id + " spreads the infection.");
            subject.InfectLocally();
            yield return new WaitForSeconds(0.005f);
        }

    }

}
