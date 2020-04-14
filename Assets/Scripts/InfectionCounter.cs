using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectionCounter : MonoBehaviour
{
    public float numHealthy = 0f;
    public float numInfected = 0f;
    public float numHealed = 0f;

    float heightNormalizerMax = 5f;

    GameObject barHealthy;
    GameObject barInfected;
    GameObject barHealed;

    TextMesh textHealthy;
    TextMesh textInfected;
    TextMesh textHealed;


    // Start is called before the first frame update
    void Start()
    {
        barHealthy = GameObject.Find("barHealthy");
        barInfected = GameObject.Find("barInfected");
        barHealed = GameObject.Find("barHealed");

        textHealthy = GameObject.Find("textHealthy").GetComponent<TextMesh>();
        textInfected = GameObject.Find("textInfected").GetComponent<TextMesh>();
        textHealed = GameObject.Find("textHealed").GetComponent<TextMesh>();

        //Vector3 barHealed_currScale = barHealed.transform.localScale;
        //barHealed_currScale.y = 0;
        //barHealed.transform.localScale = barHealed_currScale;
        changeScale(barHealed, 0, heightNormalizerMax);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        changeScale(barHealthy, numHealthy, heightNormalizerMax);
        changeScale(barInfected, numInfected, heightNormalizerMax);
        changeScale(barHealed, numHealed, heightNormalizerMax);
        Debug.Log(numHealed);

        textHealthy.text = numHealthy.ToString();
        textInfected.text = numInfected.ToString();
        textHealed.text = numHealed.ToString();
    }

    void changeScale(GameObject currScaleObj, float changeAmount, float normalizerMax)
    {
        Vector3 currScale = currScaleObj.transform.localScale;
        currScale.y = changeAmount/ normalizerMax;
        currScaleObj.transform.localScale = currScale;
    }
}
