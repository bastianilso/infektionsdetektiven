using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;
using UnityEngine.Events;
using System;

public enum MagnifyMode {
    OnClickSingle,
    OnClickMultiple,
    OnHover,
    OnHoverDemo
}

public enum ChargingState {
    Charging,
    Charged
}

public class ChargeInfo {
    public ChargingState chargeState;
    public float currentCharge;
}

public class Cursor : MonoBehaviour
{

    public LayerMask m_LayerMask;

    public MagnifyMode magnifyMode = MagnifyMode.OnClickSingle;

    // Charging Related
    private float maxCharge = 1f;
    private ChargingState chargingState = ChargingState.Charged;
    private float currentCharge = 1f; // goes from 0 to 1?

    [SerializeField]
    private float chargingAmount = 0.02f;

    [Serializable]
    public class OnChargeEvent : UnityEvent <ChargeInfo> {}
    public OnChargeEvent onChargeEvent;

    [Serializable]
    public class OnMagnifyUsed : UnityEvent<Vector3> {}
    public OnMagnifyUsed onMagnifyUsed;

    // Raycast related
    [SerializeField]
    private Camera sceneCamera;

    [SerializeField]
    private GameObject magnifyArea;

    private Vector2 defaultPosition;

    public bool magnifyAllowed = false;
    public bool useMouse = true;

    [SerializeField]
    private Image cursorImage;
    private clickFlash clickFlash;
    [SerializeField]
    private clickFlash mouseclickFlash;

    void Awake() {
        clickFlash = this.GetComponent<clickFlash>();
    }

    // Start is called before the first frame update
    void Start()
    {
        defaultPosition = this.transform.position;
    }

    void EvaluateChargingState() {
        if (chargingState == ChargingState.Charging) {
            if (currentCharge >= maxCharge) {
                currentCharge = maxCharge;
                chargingState = ChargingState.Charged;
            } else {
                currentCharge = currentCharge + chargingAmount;
                Debug.Log("new charge: " + (currentCharge) );
            }

            ChargeInfo chargeInfo = new ChargeInfo();
            chargeInfo.chargeState = chargingState;
            chargeInfo.currentCharge = currentCharge;
            //onChargeEvent.Invoke(chargeInfo);
        }
    }

    public void OnGameStateChanged(float gameTime, GameState gameState) {
        if (gameState == GameState.Playing || gameState == GameState.CountDown) {
            magnifyAllowed = true;
            UnityEngine.Cursor.visible = false;
            this.GetComponent<Image>().enabled = true;
        } else {
            magnifyAllowed = false;
            UnityEngine.Cursor.visible = true;
            this.GetComponent<Image>().enabled = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown("space")) {
        if (magnifyAllowed) {

            EvaluateChargingState();

            RaycastHit hit;
            Ray ray;
            if (useMouse) {
                 ray = sceneCamera.ScreenPointToRay(Input.mousePosition);
            } else {
                 ray = sceneCamera.ScreenPointToRay(this.transform.position);
            }
            var layerMask = 1 << 0;
            
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) {
                GameObject objectHit = hit.transform.gameObject;
                if (objectHit.tag == "dontMagnify") {
                    if (cursorImage.enabled) {
                        //cursorImage.enabled = false;
                        this.transform.position = new Vector2(-10000f, -10000f);
                        UnityEngine.Cursor.visible = true;
                    }
                } else {
                    if (!cursorImage.enabled) {
                        UnityEngine.Cursor.visible = false;
                        cursorImage.enabled = true;
                    }
                    if (useMouse) {
                        this.transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                    }
                    Magnify(hit.point);
                }
            }
        }
        //}
    }

    private void Magnify(Vector3 pos) {
        if (magnifyMode == MagnifyMode.OnClickSingle) {
            if (chargingState == ChargingState.Charged) {
                if (Input.GetMouseButtonDown(0) ) {
                    magnifyArea.transform.position = pos;
                    Collider[] hitColliders = Physics.OverlapBox(magnifyArea.transform.position, magnifyArea.transform.localScale / 2, Quaternion.identity, m_LayerMask);
                    foreach (var coll in hitColliders) {
                        //Debug.Log("Hit : " + coll.name);
                        SubjectManager subject = coll.gameObject.GetComponent<SubjectManager>();
                        subject.RevealSubjectStatus();
                        subject.IsolateInfectedSubject();
                        break;
                    }
                    onMagnifyUsed.Invoke(pos);
                    currentCharge = 0.0f;
                    chargingState = ChargingState.Charging;
                }
            }
        } else if (magnifyMode == MagnifyMode.OnClickMultiple) {
            if (chargingState == ChargingState.Charged) {
                if (Input.GetMouseButtonDown(0) ) {
                    magnifyArea.transform.position = pos;
                    Collider[] hitColliders = Physics.OverlapBox(magnifyArea.transform.position, magnifyArea.transform.localScale / 2, Quaternion.identity, m_LayerMask);
                    foreach (var coll in hitColliders) {
                        //Debug.Log("Hit : " + coll.name);
                        SubjectManager subject = coll.gameObject.GetComponent<SubjectManager>();
                        subject.RevealSubjectStatus();
                        subject.IsolateInfectedSubject();
                    }
                    onMagnifyUsed.Invoke(pos);
                    currentCharge = 0.0f;
                    chargingState = ChargingState.Charging;
                }
            }
        } else if (magnifyMode == MagnifyMode.OnHover || magnifyMode == MagnifyMode.OnHoverDemo) {
            bool isolateSubject = false;
            if (chargingState == ChargingState.Charged) {
                if (Input.GetMouseButtonDown(0) || magnifyMode == MagnifyMode.OnHoverDemo) {
                    Debug.Log("isolate!");
                    isolateSubject = true;
                    onMagnifyUsed.Invoke(pos);
                    currentCharge = 0.0f;
                    chargingState = ChargingState.Charging;
                }
            }
            magnifyArea.transform.position = pos;
            Collider[] hitColliders = Physics.OverlapBox(magnifyArea.transform.position, magnifyArea.transform.localScale / 2, Quaternion.identity, m_LayerMask);
            foreach (var coll in hitColliders) {
                Debug.Log("Hit : " + coll.name);
                SubjectManager subject = coll.gameObject.GetComponent<SubjectManager>();
                subject.RevealSubjectStatus();
                if (isolateSubject && subject.GetSubjectStatus() == SubjectStatus.Infected) {
                    if (magnifyMode == MagnifyMode.OnHoverDemo) {
                        clickFlash.ClickIt();
                        mouseclickFlash.ClickIt();
                    }
                    subject.IsolateInfectedSubject();
                }
            }
        }
    }

    public void SetCursorPosition(Vector2 screenPosition) {
        this.transform.position = screenPosition;
    }

    public void ResetPosition() {
        this.transform.position = defaultPosition;
    }

    public Vector2 GetScreenPosition() {
        Debug.Log(sceneCamera.WorldToScreenPoint(this.transform.position));
        return sceneCamera.WorldToScreenPoint(this.transform.position);
    }
}
