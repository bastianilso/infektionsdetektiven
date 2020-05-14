using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    [SerializeField]
    private GameObject countDown;

    private Text countDownText;

    private bool visible = true;
    private float visibilityTimer;
    public float visibilityTime = 2f;

    // Start is called before the first frame update
    void Awake()
    {
        countDownText = countDown.GetComponent<Text>();
        visibilityTimer = visibilityTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!visible) {
            visibilityTimer -= Time.deltaTime;
            if (visibilityTimer < 0f) {
                visibilityTimer = visibilityTime;
                this.gameObject.SetActive(false);
            }
        }
        
    }

    public void onGameCountDown(int count) {
        if (count == 0) {
            countDownText.text = "Nu!";
            visible = false;
        } else {
            countDownText.text = count.ToString();
        }
    }
}
