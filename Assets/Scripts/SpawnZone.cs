using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZone : MonoBehaviour
{
	public Vector3 SpawnPoint {
		get {
			return transform.TransformPoint(Random.insideUnitSphere);
		}
	}

    void OnDrawGizmos () {
		Gizmos.color = Color.cyan;
		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.DrawWireSphere(Vector3.zero, 1f);
	}

}
