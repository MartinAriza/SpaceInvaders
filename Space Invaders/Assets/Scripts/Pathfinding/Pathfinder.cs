using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    ArrayList waypoints = new ArrayList();
    Queue<Waypoint> queue = new Queue<Waypoint>();

    void Awake()
    {
        loadWaypoints();
    }
    
    void loadWaypoints()
    {
        Waypoint[] aux = FindObjectsOfType<Waypoint>();

        foreach (Waypoint waypoint in aux)
        {
            waypoints.Add(waypoint);
        }
    }

    public List<Waypoint> getPath(Vector3 start, Vector3 end)
    {
        Waypoint startWaypoint = getCloserWaypointToCoordinates(start);
        Waypoint endWaypoint = getCloserWaypointToCoordinates(end);

        BreadthFirstSearch(startWaypoint, endWaypoint);
        List<Waypoint> path = createPath(startWaypoint, endWaypoint);

        return path;
    }

    private void BreadthFirstSearch(Waypoint startWaypoint, Waypoint endWaypoint)
    {
        Waypoint searchCenter;
        bool isRunning = true;

        queue.Enqueue(startWaypoint);

        while (isRunning && queue.Count > 0)
        {
            searchCenter = queue.Dequeue();
            searchCenter.isExplored = true;

            if (searchCenter == endWaypoint) isRunning = false;
            else ExploreNeighbours(searchCenter);
        }
    }

    private void ExploreNeighbours(Waypoint searchCenter)
    {
        foreach (Waypoint waypoint in searchCenter.explorableWaypoints)
        {
            if (waypoint.isExplored || queue.Contains(waypoint)) { }
            else
            {
                queue.Enqueue(waypoint);
                waypoint.exploredFrom = searchCenter;
            }
        }
    }

    private List<Waypoint> createPath(Waypoint startWaypoint, Waypoint endWaypoint)
    {
        List<Waypoint> path = new List<Waypoint>();
        path.Add(endWaypoint);

        Waypoint previous = endWaypoint.exploredFrom;

        while (previous != startWaypoint)
        {
            path.Add(previous);
            previous = previous.exploredFrom;
        }

        path.Add(startWaypoint);
        path.Reverse();

        return path;
    }

    private Waypoint getCloserWaypointToCoordinates(Vector3 v)
    {
        float minDistance = 9999999999999999999999999999999999999f;
        Waypoint closestWaypoint = null;

        foreach (Waypoint waypoint in waypoints)
        {
            if ( (waypoint.transform.position - v).magnitude < minDistance)
            {
                closestWaypoint = waypoint;
                minDistance = Mathf.Abs(waypoint.transform.position.magnitude - v.magnitude);
            }
        }
        return closestWaypoint;
    }
}
