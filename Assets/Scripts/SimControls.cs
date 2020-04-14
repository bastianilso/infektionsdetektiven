using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimControls : MonoBehaviour
{
    public bool pauseSim = false;

    public bool endSim = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.GetComponent<InfectionCounter>().numInfected == 0 && transform.GetComponent<InfectionCounter>().numHealed != 0)
        {
            endSim = true;
        }
    }
}
