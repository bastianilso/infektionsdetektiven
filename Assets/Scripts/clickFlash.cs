using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ClickFlashState {
    Flashing,
    None
}

public class clickFlash : MonoBehaviour
{

    [SerializeField]
    private Sprite clickSprite;
    private Sprite defaultSprite;

    public float flashTime = 0.2f;
    private float t = 0f;
    private ClickFlashState flashState = ClickFlashState.None;

    // Start is called before the first frame update
    void Start()
    {
        defaultSprite = this.GetComponent<Image>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (flashState == ClickFlashState.None) {
            if (Input.GetMouseButtonDown(0) ) {
                this.GetComponent<Image>().sprite = clickSprite; 
                flashState = ClickFlashState.Flashing;
            }
        } else if (flashState == ClickFlashState.Flashing) {
            t += Time.deltaTime;
            if (t > flashTime) {
                this.GetComponent<Image>().sprite = defaultSprite;
                flashState = ClickFlashState.None;
                t = 0f;
            }
        }
    }
}
