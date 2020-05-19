using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    [SerializeField]
    private GameObject countDown;

    private Text countDownText;

    // Start is called before the first frame update
    void Awake()
    {
        countDownText = countDown.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onGameCountDown(int count) {
        if (count == 1) {
            countDownText.text = "Nu!";
        } else if (count == 0) {
            this.gameObject.SetActive(false);
        } else {
            countDownText.text = (count-1).ToString();
        }
    }
}
