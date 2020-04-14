﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject actor;

    public GameObject colliderSpace;

    public int numberOfActors = 10;

    public Material[] possibleStatesMat;
    public int[] possibleStates = { 1, 2, 3 };

    public int numberOfInfected = 1;

    public float percentShutdown = 0.5f;

    void Start()
    {

        Vector3 centerPos = colliderSpace.transform.position;
        Vector3 sizeSpawn = colliderSpace.transform.localScale;
        centerPos.y = -1f;

        GameObject actorHolder = GameObject.Find("actorHolder");

        int spawnInfected_counter = 0;

        int quarantine_max = (int)(numberOfActors * percentShutdown);
        int quarantine_counter = 0;

        for (int i = 0; i < numberOfActors; i++)
        {
            Vector3 spawnPos = centerPos + new Vector3(Random.Range(-sizeSpawn.x / 2, sizeSpawn.x / 2), 0, Random.Range(-sizeSpawn.z / 2, sizeSpawn.z / 2));

            GameObject currActor = Instantiate(actor, spawnPos, Quaternion.identity);

            actorHolder.transform.parent = currActor.transform;

            if (spawnInfected_counter < numberOfInfected)
            {
                currActor.transform.GetComponent<BallBounce>().currState = 2;
                currActor.transform.GetComponent<Renderer>().material = possibleStatesMat[1];
                spawnInfected_counter++;
                transform.GetComponent<InfectionCounter>().numInfected += 1f;
            }
            else
            {
                currActor.transform.GetComponent<BallBounce>().currState = 1;
                currActor.transform.GetComponent<Renderer>().material = possibleStatesMat[0];

                transform.GetComponent<InfectionCounter>().numHealthy += 1f;

                if (quarantine_counter <= quarantine_max)
                {
                    currActor.transform.GetComponent<BallBounce>().isMoving = false;
                    quarantine_counter++;
                }
            }
            

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }





}
