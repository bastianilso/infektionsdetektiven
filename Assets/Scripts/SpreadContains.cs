using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpreadContains : MonoBehaviour
{

    public LayerMask m_LayerMask;
    [SerializeField]
    private float spreadSpeed = 0.5f;

    [SerializeField]
    private GameObject spreadTemplate;

    void Start()
    {

    }

    void Update()
    {

    }

    public void SpreadInfection(int spreadRatio) {
        Debug.Log("attempting to spread infection");
        //this.transform.position = pos;

        //Use the OverlapBox to detect if there are any other colliders within this box area.
        //Use the GameObject's centre, half the size (as a radius) and rotation. This creates an invisible box around your GameObject.
        int spread = 0;
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, m_LayerMask);
        foreach (var coll in hitColliders) {
            Debug.Log("infecting subject " + coll.gameObject.GetComponent<SubjectManager>().id);
            //Debug.Log("Hit : " + coll.name);
            if (spread < spreadRatio) {
                coll.gameObject.GetComponent<SubjectManager>().SetSubjectStatus(SubjectStatus.Infected);

                var obj = Instantiate(spreadTemplate);
                obj.transform.localPosition = this.transform.position;
                obj.SetActive(true);
                StartCoroutine(VisualizeSpread(obj, coll.gameObject.transform.position));
            } else {
                break;
            }
            spread++;
        }
        //int i = 0;
        //Check when there is a new collider coming into contact with the box
        //while (i < hitColliders.Length)
        //{
            //Output all of the collider names
        //    Debug.Log("Hit : " + hitColliders[i].name + i);

            //Increase the number of Colliders in the array
        //    i++;
        //}
    }

    private IEnumerator VisualizeSpread(GameObject obj, Vector3 targetPos) {
        float t = 0f;

        while (t < 1f) {
            t += spreadSpeed;
            obj.transform.position = Vector3.Slerp(this.transform.position, targetPos, t);
            yield return new WaitForSeconds(0.005f);
        }
        yield return new WaitForSeconds(0.100f);
        Destroy(obj);
        yield return null;
    }

    //Draw the Box Overlap as a gizmo to show where it currently is testing. Click the Gizmos button to see this
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        //if (m_Started)
        //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
