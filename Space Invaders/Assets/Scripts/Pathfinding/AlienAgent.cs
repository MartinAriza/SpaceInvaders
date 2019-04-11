using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienAgent : MonoBehaviour
{
    [SerializeField] Vector3 destination;

    Pathfinder pathfinder;
    Rigidbody rigidbody;

    List<Vector3> path = null;
    int waypointIndex = 0;
    [SerializeField] List<Waypoint> patrolPath;

    [SerializeField] bool previewPath = true;
    [ColorUsageAttribute(true, true)]
    [SerializeField] Color pathColor = new Color(1f, 0f, 0f, 1f);

    [SerializeField] float speed = 100.0f;
    [SerializeField][Tooltip("Cuando está a esta distancia del waypoint pasa a dirigirse al siguiente del camino")] float directionChangeThreshold = 1f;
    Vector3 movementDirection;

    void Start()
    {
        pathfinder = GetComponent<Pathfinder>();
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            path = pathfinder.getPath(transform.position, destination);
            waypointIndex = 0;
        }
    }

    private void FixedUpdate()
    {
        if (path != null)
        {
            if (followPath()) rigidbody.velocity = Vector3.zero;
        }
    }
    
    private bool followPath()
    {
        movementDirection = (path[waypointIndex] - transform.position).normalized;

        rigidbody.velocity = movementDirection * speed * Time.deltaTime;
        transform.forward = new Vector3(movementDirection.x, 0, movementDirection.z);

        if (Vector3.Distance(transform.position, path[waypointIndex]) <= directionChangeThreshold)
        {
            if (waypointIndex < path.Count - 1)
                waypointIndex++;

            else return true;
        }
        
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 0f, 1f, 0.2f);
        Gizmos.DrawSphere(transform.position, directionChangeThreshold);

        if (previewPath && path != null)
        {
            Gizmos.color = pathColor;
            for (int i = 0; i < path.Count - 1; i++)
            {
                Gizmos.DrawLine(path[i], path[i + 1]);
            }
        }
    }
}