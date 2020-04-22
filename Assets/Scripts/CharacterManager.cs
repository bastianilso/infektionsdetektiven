﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Walk() {
        animator.SetBool("Moving", true);
    }

    public void Idle() {
        animator.SetBool("Moving", false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}