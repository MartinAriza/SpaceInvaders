using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] float explorableDistance = 5.0f;
    public ArrayList explorableWaypoints = new ArrayList();

    [HideInInspector] public bool isExplored = false;
    [HideInInspector] public Waypoint exploredFrom;

    void Start()
    {
        findExplorableWaypoints();
    }

    private void findExplorableWaypoints()
    {
        Collider[] aux = Physics.OverlapSphere(transform.position, explorableDistance, LayerMask.GetMask("Waypoint"));

        foreach(Collider collider in aux)
        {
            if(collider.gameObject.transform != gameObject.transform)
                explorableWaypoints.Add(collider.gameObject.GetComponent<Waypoint>());
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.2f);
        Gizmos.DrawSphere(transform.position, explorableDistance);
    }
}
