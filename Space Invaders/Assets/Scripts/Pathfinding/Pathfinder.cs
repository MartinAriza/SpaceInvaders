using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    static ArrayList waypoints = new ArrayList();
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
        ArrayList visitedWaypoints = new ArrayList(); 
        Waypoint searchCenter;
        bool foundEndWaypoint = false;

        queue.Enqueue(startWaypoint);

        while (!foundEndWaypoint && queue.Count > 0)
        {
            searchCenter = queue.Dequeue();
            visitedWaypoints.Add(searchCenter);

            if (searchCenter == endWaypoint) foundEndWaypoint = true;
            else ExploreNeighbours(searchCenter, visitedWaypoints);
        }
    }

    private void ExploreNeighbours(Waypoint searchCenter, ArrayList visitedWaypoints)
    {
        foreach (Waypoint waypoint in searchCenter.explorableWaypoints)
        {
            if ( visitedWaypoints.Contains(waypoint) || queue.Contains(waypoint) ) { }
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

        Waypoint previousWaypoint = endWaypoint.exploredFrom;

        while (previousWaypoint != startWaypoint)
        {
            path.Add(previousWaypoint);
            previousWaypoint = previousWaypoint.exploredFrom;
        }
        path.Add(startWaypoint);
        path.Reverse();

        return path;
    }

    private Waypoint getCloserWaypointToCoordinates(Vector3 v)
    {
        float minDistance = 99999f;
        Waypoint closestWaypoint = null;

        foreach (Waypoint waypoint in waypoints)
        {
            if ( (waypoint.transform.position - v).magnitude < minDistance)
            {
                closestWaypoint = waypoint;
                minDistance = (waypoint.transform.position - v).magnitude;
            }
        }
        return closestWaypoint;
    }
}

/* TO DO:
 * Hacer que el movimiento de los agentes sea suave
 * Añadir sistema de patrulla
 * Detección de "ruido"
 * Añadir cono de visión a los agentes
 * Hacer que los agentes puedan disparar a su objetivo
 */