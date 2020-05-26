using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public enum SubjectStatus {
    Infected,
    Healthy,
    Null
}

public enum SubjectVisibility {
    Hidden,
    Visible,
    GoingToIsolation
}

public enum SubjectRoaming {
    Walking,
    Standstill
}

public class RevealInfo {
    public SubjectStatus subjectStatus;
}

public enum FlashState {
    FlashUp,
    FlashDown
}

public class SubjectManager : MonoBehaviour
{

    public int id = -1;

    [Serializable]
    public class OnSubjectStatusChanged : UnityEvent <int, SubjectStatus> {}
    public OnSubjectStatusChanged onSubjectStatusChanged;

    public int spreadRatio = 1;

    [Serializable]
    public class OnRevealEvent : UnityEvent <RevealInfo> {}
    public OnRevealEvent onRevealEvent;

    [Serializable]
    public class OnSubjectIsolated : UnityEvent <int, SubjectStatus> {}
    public OnSubjectIsolated onSubjectIsolated;

    private SubjectStatus subjectStatus = SubjectStatus.Healthy;
    private SubjectVisibility subjectVisibility = SubjectVisibility.Hidden;

    [SerializeField]
    private GameObject infected;
    [SerializeField]
    private GameObject healthy;

    [SerializeField]
    private GameObject neutral;

    [SerializeField]
    private SpawnZone isolationZone;
    public float isolationSpeed = 0.05f;
    private float isolationT = 0f;
    public float upwardsAmount = 5f;
    private float upwardsT = 0f; 
    private Vector3 curPos;
    private Vector3 destPos;

    [SerializeField]
    private SpawnZone spawnZone;

    [SerializeField]
    private GameObject neutralSymptoms;
    [SerializeField]
    private Material neutralSymptomsMaterial;
    [SerializeField]
    private Color symptomColor;
    [SerializeField]
    private Color defaultColor;
    public float flashSpeed = 2f;
    private FlashState flashState = FlashState.FlashUp;

    [SerializeField]
    private SpreadContains spreadContains;

    [SerializeField]
    private LODGroup lodGroup;
    [SerializeField]
    private GameObject subjectInfectedZoom;
    [SerializeField]
    private GameObject subjectInfectedBall;

    [SerializeField]
    private Collider roamingBounds;
    private Vector3 roamingDirection;
    private SubjectRoaming subjectRoaming = SubjectRoaming.Walking; 
    private float roamingTimer = 0.1f;
    public float walkingTime = 7f;
    public float pauseTime = 1f;
    public float roamSpeed = 25f;
    private float pauseTimer = 0f;
    private float roamRadius = 150f;

    public float hideTime = 2f;
    public bool shouldWalk = true;
    private float timer = 0.1f;
    private bool isFlashing = false;
    // Start is called before the first frame update

    [SerializeField]
    private AudioSource audioSource;

    void Start()
    {
        //SetSubjectStatus(SubjectStatus.Infected);
        roamingTimer = walkingTime;
        pauseTimer = pauseTime;
        roamingDirection = spawnZone.SpawnPoint;
        neutralSymptomsMaterial.color = defaultColor;

    }

    // Update is called once per frame
    void Update()
    {
        if (subjectVisibility == SubjectVisibility.Hidden) {
            if (shouldWalk) {
                Roam();
            }
        }
        else if (subjectVisibility == SubjectVisibility.Visible) {
            timer -= Time.deltaTime;
            if (timer < 0f) {
                HideSubjectStatus();
                timer = hideTime;
            }
        } else if (subjectVisibility == SubjectVisibility.GoingToIsolation) {
            isolationT += isolationSpeed * Time.deltaTime;
            Vector3 slerpPos = Vector3.Slerp(curPos, destPos, isolationT);
            if (isolationT < 0.5f) {
                upwardsT += isolationSpeed*2 * Time.deltaTime;
            } else {
                upwardsT -= isolationSpeed*2 * Time.deltaTime;
            }
            this.transform.position = new Vector3(slerpPos.x, Mathf.Lerp(slerpPos.y, slerpPos.y+upwardsAmount, upwardsT), slerpPos.z);
        }
    }

    public void onTooMuchSpread() {
        StartCoroutine(Flash());
    }

    private void Roam() {
        if (subjectRoaming == SubjectRoaming.Walking) {
            roamingTimer -= Time.deltaTime;

            Vector3 newPos = Vector3.MoveTowards(this.transform.position, roamingDirection, roamSpeed * Time.deltaTime);
            if(!roamingBounds.bounds.Contains(newPos)){
                // if next point outside boundary, do a 180
                roamingDirection = -roamingDirection;
                newPos = Vector3.MoveTowards(this.transform.position, roamingDirection, roamSpeed * Time.deltaTime);
            }
            this.transform.position = new Vector3(newPos.x, this.transform.position.y, newPos.z);

            if (roamingTimer < 0f) {
                subjectRoaming = SubjectRoaming.Standstill;
                
                roamingTimer = walkingTime + UnityEngine.Random.Range(-3f,4f);
                roamingDirection = spawnZone.SpawnPoint * 100f;
            }
        } else if (subjectRoaming == SubjectRoaming.Standstill) {
            pauseTimer -= Time.deltaTime;
            if (pauseTimer < 0f) {
                pauseTimer = pauseTime + UnityEngine.Random.Range(0f,4f);
                subjectRoaming = SubjectRoaming.Walking;
            }
        }
    }

    private IEnumerator Flash() {
        if (isFlashing) {
            yield return null;
        } else {
            isFlashing = true;
            float flashTimer = 0f;
            while (flashTimer < 1f) {
                flashTimer += flashSpeed * Time.deltaTime;
                neutralSymptomsMaterial.color = Vector4.Lerp(defaultColor, symptomColor, flashTimer);
                yield return new WaitForSeconds(Time.deltaTime);
            }
            while (flashTimer > 0f) {
                flashTimer -= flashSpeed * Time.deltaTime;
                neutralSymptomsMaterial.color = Vector4.Lerp(defaultColor, symptomColor, flashTimer);
                yield return new WaitForSeconds(Time.deltaTime);
            }
            isFlashing = false;
            yield return null;
        }
    }

    public void SetSubjectStatus(SubjectStatus status) {
        if (subjectStatus == status) {
            // this subject is already infected, return.
            return;
        } else {
            subjectStatus = status;
            onSubjectStatusChanged.Invoke(id, subjectStatus);
            if (subjectStatus == SubjectStatus.Infected) {
                infected.SetActive(false);
                neutralSymptoms.SetActive(true);
                healthy.SetActive(false);
                neutral.SetActive(false);
            } else {
                infected.SetActive(false);
                neutralSymptoms.SetActive(false);
                healthy.SetActive(false);
                neutral.SetActive(true);
            }
        }
    }

    public void RevealSubjectStatus() {
        timer = hideTime;
        if (subjectVisibility == SubjectVisibility.Hidden) {
            subjectVisibility = SubjectVisibility.Visible;

            if (subjectStatus == SubjectStatus.Infected) {
                infected.SetActive(true);
                neutralSymptoms.SetActive(false);
                healthy.SetActive(false);
                neutral.SetActive(false);
            } else if (subjectStatus == SubjectStatus.Healthy) {
                infected.SetActive(false);
                neutral.SetActive(false);
                healthy.SetActive(true);
                neutralSymptoms.SetActive(false);
            }
            RevealInfo revealInfo = new RevealInfo();
            revealInfo.subjectStatus = subjectStatus;
            onRevealEvent.Invoke(revealInfo);
        }
    }

    public void HideSubjectStatus() {
        if (subjectVisibility == SubjectVisibility.Visible) {
            subjectVisibility = SubjectVisibility.Hidden;
                infected.SetActive(false);
                healthy.SetActive(false);
                if (subjectStatus == SubjectStatus.Infected) {
                    neutralSymptoms.SetActive(true);
                } else {
                    neutral.SetActive(true);
                }
        }
    }

    public void IsolateInfectedSubject() {
        if (subjectStatus == SubjectStatus.Infected) {
            infected.SetActive(true);
            neutralSymptoms.SetActive(false);
            curPos = this.transform.position;
            destPos = isolationZone.SpawnPoint;
            lodGroup.enabled = false;
            subjectInfectedBall.SetActive(false);
            subjectInfectedZoom.SetActive(true);
            subjectInfectedZoom.transform.localScale = new Vector3(4f, 4f, 4f);
            subjectInfectedZoom.transform.eulerAngles = new Vector3(-60f, UnityEngine.Random.Range(0f,230f), 0f);
            this.transform.localScale = new Vector3(2f, 2f, 2f);
            subjectVisibility = SubjectVisibility.GoingToIsolation;
            onSubjectIsolated.Invoke(id, subjectStatus);
        }
    }

    public void InfectLocally() {
        if (subjectVisibility != SubjectVisibility.GoingToIsolation) {
            spreadContains.SpreadInfection(spreadRatio);
            audioSource.Play();
        }
    }

}
