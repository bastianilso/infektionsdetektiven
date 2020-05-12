using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnify : MonoBehaviour
{

    [SerializeField]
    private Camera sceneCamera;

    bool m_HitDetect;
    RaycastHit m_Hit;
    float m_MaxDistance;

    Ray boxCenter;
    Vector3 boxSize;
    Ray boxMin;
    Ray boxMax;



    // Start is called before the first frame update
    void Start()
    {
        m_MaxDistance = 300.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // test each world-corner.
        //RaycastHit hit;
        //Ray ray = sceneCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        //Test to see if there is a hit using a BoxCast
        //Calculate using the center of the GameObject's Collider(could also just use the GameObject's position), half the GameObject's size, the direction, the GameObject's rotation, and the maximum distance as variables.
        //Also fetch the hit data
        RaycastHit hit;
        Bounds r_bounds = GetRectTransformBounds(this.GetComponent<RectTransform>());
        boxCenter = sceneCamera.ScreenPointToRay(r_bounds.center);
        
        if (Physics.Raycast(boxCenter, out hit)) {
            GameObject objectHit = hit.transform.gameObject;
            if (objectHit.tag == "canMagnify") {

            } else {
            }
        }

        //boxSize = r_bounds.max - r_bounds.min;
        //boxMin = sceneCamera.ScreenPointToRay(r_bounds.min);
        //boxMax = sceneCamera.ScreenPointToRay(r_bounds.max);
        //Debug.Log(boxCenter);
        //Debug.Log(boxSize);


        //m_HitDetect = Physics.BoxCast(r_bounds.center, transform.localScale, transform.forward, out m_Hit, transform.rotation, m_MaxDistance);
        //if (m_HitDetect)
        //{
            //Output the name of the Collider your Box hit
        //    Debug.Log("Hit : " + m_Hit.collider.name);
        //}

        //var selectionRect = this.GetComponent<RectTransform>();
 
        // Get BoxCast center and size by converting the rect's center, width, and height from screen to world space

        //boxWidth = ((selectionRect.width * canvas.scaleFactor) / 2);
        //boxHeight = ((selectionRect.height * canvas.scaleFactor) / 2);
        //boxSize = new Vector2(boxWidth, boxHeight);
 
        //Debug.Log(boxCenter + " ; " + boxSize);

    }

    public Bounds GetRectTransformBounds(RectTransform transform)
    {
        Vector3[] WorldCorners = new Vector3[4];
        transform.GetWorldCorners(WorldCorners);
        Bounds bounds = new Bounds(WorldCorners[0], Vector3.zero);
        for(int i = 1; i < 4; ++i)
        {
            bounds.Encapsulate(WorldCorners[i]);
        }
        return bounds;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        //Draw a Ray forward from GameObject toward the maximum distance
        Gizmos.DrawLine(boxCenter.origin, boxCenter.direction * m_MaxDistance);
        Gizmos.DrawLine(boxMin.origin, boxMin.direction * m_MaxDistance);
        Gizmos.DrawLine(boxMax.origin, boxMax.direction * m_MaxDistance);
        //Draw a cube at the maximum distance
        Gizmos.DrawWireCube(boxCenter.direction * m_MaxDistance, boxSize);

    }

}
