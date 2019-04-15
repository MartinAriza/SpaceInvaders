using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienAgent : MonoBehaviour
{
    static VinMov vin;
    [SerializeField]float timeToReturnToPatrol = 5.0f;
    [SerializeField]Vector3 destination;

    Pathfinder pathfinder;
    Rigidbody rigidbody;
    [SerializeField]ParticleSystem laser;

    List<Vector3> path = null;
    int waypointIndex = 0;
    bool changePath = true;
    bool seenVin = false;

    [SerializeField] List<Waypoint> patrolPath;
    [SerializeField] bool isPatrolling = false;

    [SerializeField] bool previewPath = true;
    [ColorUsageAttribute(true, true)]
    [SerializeField] Color pathColor = new Color(1f, 0f, 0f, 1f);

    [SerializeField] float speed = 100.0f;
    [SerializeField][Tooltip("Cuando está a esta distancia del waypoint pasa a dirigirse al siguiente del camino")] float directionChangeThreshold = 1f;
    Vector3 movementDirection;

    void Start()
    {
        vin = FindObjectOfType<VinMov>();
        pathfinder = GetComponent<Pathfinder>();
        rigidbody = GetComponent<Rigidbody>();
        laser.transform.forward = transform.forward;
    }

    void Update()
    {
        if(seenVin)
        {
            destination = vin.transform.position;
            findPath();
        }
    }

    public void findPath()
    {
        pathfinder.reset();
        waypointIndex = 0;

        if (isPatrolling)
        {
            Waypoint closestPatrolPathWaypoint = pathfinder.getCloserWaypointToCoordinates(transform.position, patrolPath);
            if (isOnPatrolPath())
            {
                path = pathfinder.getPath(patrolPath, transform.position, closestPatrolPathWaypoint);
            }

            else
            {
                path = pathfinder.getPath(transform.position, closestPatrolPathWaypoint.transform.position);
            }

        }
        else path = pathfinder.getPath(transform.position, destination);
    }

    private void FixedUpdate()
    {
        if (path != null)
        {
            if (followPath())
            {
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
            }
        }
    }
    
    private bool followPath()
    {
        movementDirection = (path[waypointIndex] - transform.position).normalized;

        rigidbody.velocity = movementDirection * speed * Time.deltaTime;

        if(isPatrolling) transform.forward = new Vector3(movementDirection.x, 0, movementDirection.z);
        else transform.forward = new Vector3(vin.transform.position.x - transform.position.x, 0, vin.transform.position.z - transform.position.z);

        if (Vector3.Distance(transform.position, path[waypointIndex]) <= directionChangeThreshold)
        {
            if (waypointIndex < path.Count - 1)
                waypointIndex++;

            else
            {
                if (isPatrolling) findPath();
                return true;
            } 
        }
        
        return false;
    }

    public void goTo(Vector3 position)
    {
        LayerMask staticSolidMask = LayerMask.GetMask("staticSolid");
        Vector3 rayCastDirection = (position - transform.position).normalized;
        float rayCastMaxDistance = (position - transform.position).magnitude;

        if (!Physics.Raycast(transform.position, rayCastDirection, rayCastMaxDistance, staticSolidMask))
        {
            destination = position;
            isPatrolling = false;
            findPath(); findPath();
        }

        else
        {
            isPatrolling = true;
            StartCoroutine(goBackToPatrol());
        }
    }

    public void goToNoRayCast(Vector3 position)
    {
        destination = position;
        isPatrolling = false;
        findPath(); findPath();
    }

    private bool isOnPatrolPath()
    {
        Waypoint closestPatrolPathWaypoint = pathfinder.getCloserWaypointToCoordinates(transform.position, patrolPath);

        if ((closestPatrolPathWaypoint.transform.position - transform.position).magnitude >= directionChangeThreshold) return false;

        return true;
    }

    public void shoot(Vector3 position)
    {
        laser.transform.forward = position - transform.position;
        if(!laser.isPlaying) laser.Play();
    }

    IEnumerator goBackToPatrol()
    {
        yield return new WaitForSecondsRealtime(timeToReturnToPatrol);
        findPath(); findPath();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Vin") shoot(other.transform.position);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Vin") goTo(other.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "NoiseSphere") goToNoRayCast(other.transform.position);
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