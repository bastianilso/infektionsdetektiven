using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeIn : MonoBehaviour
{
        public enum FadeState {
            FadeIn,
            FadeOut
        }
        
        public Image img;
        public float speed = 0.1f;
        public FadeState f = FadeState.FadeIn;
       
        public void OnEnable()
        {
            if (f == FadeState.FadeIn) {
                StartCoroutine(FadeImage(false));
            } else {
                StartCoroutine(FadeImage(true));
            }
        }
     
        IEnumerator FadeImage(bool fadeAway)
        {
            // fade from opaque to transparent
            if (fadeAway)
            {
                // loop over 1 second backwards
                for (float i = 1; i >= 0; i -= speed)
                {
                    // set color with i as alpha
                    img.color = new Color(1, 1, 1, i);
                    yield return null;
                }
            }
            // fade from transparent to opaque
            else
            {
                // loop over 1 second
                for (float i = 0; i <= 1; i += Time.deltaTime)
                {
                    // set color with i as alpha
                    img.color = new Color(1, 1, 1, i);
                    yield return null;
                }
            }
        }     

}
