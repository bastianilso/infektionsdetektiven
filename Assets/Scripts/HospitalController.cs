using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HospitalController : MonoBehaviour
{
    [SerializeField]
    private Animator hospitalAnimator;

    [SerializeField]
    private ParticleSystem particleSystem;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayHospitalFull() {
        particleSystem.Play();
        hospitalAnimator.SetBool("HospitalFull", true);
    }
}
