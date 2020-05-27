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

    [SerializeField]
    private Image lookingGlass;

    public float flashTime = 0.2f;
    private float t = 0f;
    private ClickFlashState flashState = ClickFlashState.None;
    private bool click = false;
    public bool clickWithMouse = true;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        defaultSprite = this.GetComponent<Image>().sprite;
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (flashState == ClickFlashState.None) {
            if (clickWithMouse && Input.GetMouseButtonDown(0)) {
                click = true;
            }
            if (click) {
                lookingGlass.sprite = clickSprite; 
                if (animator != null) {
                animator.Play("looking-glass-click");
                }
                flashState = ClickFlashState.Flashing;
                click = false;
            }
        } else if (flashState == ClickFlashState.Flashing) {
            t += Time.deltaTime;
            if (t > flashTime) {
                lookingGlass.sprite = defaultSprite;
                flashState = ClickFlashState.None;
                t = 0f;
            }
        }
    }

    public void ClickIt() {
        click = true;
    }


}
