using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] float explorableDistance = 5.0f;
    public ArrayList explorableWaypoints = new ArrayList();

    [HideInInspector] public Waypoint exploredFrom;

    void Start()
    {
        findExplorableWaypoints();
    }

    private void findExplorableWaypoints()
    {
        Collider[] aux = Physics.OverlapSphere(transform.position, explorableDistance, LayerMask.GetMask("Waypoint"));
        LayerMask staticSolidMask = LayerMask.GetMask("staticSolid");

        foreach(Collider collider in aux)
        {
            Vector3 rayCastDirection = (collider.transform.position - transform.position).normalized;
            float rayCastMaxDistance = (collider.transform.position - transform.position).magnitude;

            if( collider.gameObject.transform != gameObject.transform 
                &&
                !Physics.Raycast(transform.position, rayCastDirection, rayCastMaxDistance,  staticSolidMask) 
                )
                explorableWaypoints.Add(collider.gameObject.GetComponent<Waypoint>());
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
        Gizmos.DrawSphere(transform.position, explorableDistance);

        foreach(Waypoint waypoint in explorableWaypoints)
        {
            Gizmos.color = new Color(0f, 1f, 0f, 1f);
            Gizmos.DrawLine(transform.position, waypoint.transform.position);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "Waypoint.png", true);
    }
}
