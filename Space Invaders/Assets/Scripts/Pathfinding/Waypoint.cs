using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] bool showIcon = true;
    [Space(20)]
    [SerializeField] float explorableDistance = 5.0f;
    public List<Waypoint> explorableWaypoints = new List<Waypoint>();

     public Waypoint exploredFrom;

    void Start()
    {
        findExplorableWaypoints();
    }

    //Comprobamos con radio explorableDistance que waypoints están alrededor
    //Con raycast comprobamos si hay alguna pared, suelo etc entre los waypoints
    //Solo serán explorables aquellos waypoints donde el raycast no de colisión
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
            {
                explorableWaypoints.Add(collider.gameObject.GetComponent<Waypoint>());
                //if (!collider.gameObject.GetComponent<Waypoint>().explorableWaypoints.Contains(this))
                  //collider.gameObject.GetComponent<Waypoint>().explorableWaypoints.Add(this);
            }   
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.1f);
        Gizmos.DrawSphere(transform.position, explorableDistance);

        foreach(Waypoint waypoint in explorableWaypoints)
        {
            Gizmos.color = new Color(0f, 1f, 0f, 1f);
            Gizmos.DrawLine(transform.position, waypoint.transform.position);
        }
    }

    private void OnDrawGizmos()
    {
        if(showIcon)
        Gizmos.DrawIcon(transform.position, "Waypoint.png", false);
    }
}
