using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ShowDangerImage {
    Fadeout,
    Hidden
}

public class DangerImage : MonoBehaviour
{

    private Image dangerImage;

    private ShowDangerImage showDangerImage = ShowDangerImage.Hidden;
    private float dangerImageT = 0f;
    public float dangerImageSpeed = 0.02f;

    // Start is called before the first frame update
    void Awake()
    {
        dangerImage = this.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dangerImageT > 0 && showDangerImage == ShowDangerImage.Fadeout) {
            dangerImageT -= dangerImageSpeed;
            dangerImage.color = Vector4.Lerp(new Vector4(1f, 1f, 1f, 0f), new Vector4(1f, 1f, 1f, 1f), dangerImageT);
            if (dangerImageT < 0 ) {
                showDangerImage = ShowDangerImage.Hidden;
            }
        }
    }

    public void TriggerDangerImage() {
        showDangerImage = ShowDangerImage.Fadeout;
        dangerImageT = 1;
    }
}
