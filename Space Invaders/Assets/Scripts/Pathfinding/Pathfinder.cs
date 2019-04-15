using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    static List<Waypoint> waypoints = new List<Waypoint>();   //Referencia a todos los waypoints de la escena
    Queue<Waypoint> queue = new Queue<Waypoint>();  //Cola de procesamiento de waypoints en orden de profundidad

    bool foundEndWaypoint = false;
    List<Waypoint> exploredWaypoints = new List<Waypoint>();

    #region splines
    int splineGrade = 3;
    bool splineAproximation = false;
    #endregion

    void Awake()
    {
        loadWaypoints();
    }
    
    void loadWaypoints()    //Cargamos todos los waypoints en la ArrayList
    {
        Waypoint[] aux = FindObjectsOfType<Waypoint>();

        foreach (Waypoint waypoint in aux)
        {
            waypoints.Add(waypoint);
        }
    }

    public List<Vector3> getPath(List<Waypoint> patrolPath, Vector3 position, Waypoint closestPatrolWaypoint) //Camino de patrulla
    {
        List<Vector3> path = new List<Vector3>();

        foreach(Waypoint patrolW in patrolPath)
        {
            path.Add(patrolW.transform.position);
        }
        path.Add(patrolPath[0].transform.position);
        return path;
    }

    public List<Vector3> getPath(Vector3 start, Vector3 end)
    {
        //Waypoints inicial y final aproximados según las coordenadas dadas en el mundo
        Waypoint startWaypoint = getCloserWaypointToCoordinates(start, waypoints);
        Waypoint endWaypoint = getCloserWaypointToCoordinates(end, waypoints);

        BreadthFirstSearch(startWaypoint, endWaypoint);
        List<Vector3> path = createPath(startWaypoint, endWaypoint);
        return path;
    }

    private void BreadthFirstSearch(Waypoint startWaypoint, Waypoint endWaypoint)
    {
        exploredWaypoints = new List<Waypoint>();
        Waypoint searchCenter;  //Waypoint alrededor del cual estamos buscando "vecinos"
        foundEndWaypoint = false;

        queue.Enqueue(startWaypoint);   //Metemos en la cola de procesamiento el Waypoint incial

        while (!foundEndWaypoint && queue.Count > 0)
        {
            searchCenter = queue.Dequeue();
            exploredWaypoints.Add(searchCenter);

            if (searchCenter == endWaypoint) foundEndWaypoint = true;
            ExploreNeighbours(searchCenter);  //Exploramos los waypoints vecinos del waypoint searchCenter
        }
    }

    private void ExploreNeighbours(Waypoint searchCenter)
    {
        if (foundEndWaypoint) return;

        foreach (Waypoint waypoint in searchCenter.explorableWaypoints)
        {
            if ( exploredWaypoints.Contains(waypoint) || queue.Contains(waypoint)) { }
            else
            {
                queue.Enqueue(waypoint);    //Colocamos los waypoints a los que puedes ir desde el searchCenter en la cola de procesamiento
                waypoint.exploredFrom = searchCenter;   //Marcamos que ese nodo a sido explorado desde el searchCenter
            }
        }
    }

    //Para crear el camino comenzamos desde el último nodo, vemnos desde donde ha sido explorado 
    //y repetimos esta operación hasta llegar al nodo inicial (seguimos el camino inverso que seguirá el agente después)
    private List<Vector3> createPath(Waypoint startWaypoint, Waypoint endWaypoint) 
    {
        List<Vector3> path = new List<Vector3>();
        path.Add(endWaypoint.transform.position);

        Waypoint previousWaypoint = endWaypoint.exploredFrom;
        while (previousWaypoint != startWaypoint && previousWaypoint != null)
        {
            path.Add(previousWaypoint.transform.position);
            previousWaypoint = previousWaypoint.exploredFrom;
        }
        path.Add(startWaypoint.transform.position);
        path.Reverse(); //le damos la vuelta al camino para que coincida con el que ha de recorrer el agente

        //if(splineAproximation) path = createSplinePath(path);  //Tenemos un camino lineal y queremos conseguir un camino suave usando splines

        return path;
    }

    /*private List<Vector3> createSplinePath(List<Vector3> rawPath)
    {
        List<Vector3> path = new List<Vector3>();

        Vector3 bSpline = new Vector3();
        int curveOrder = splineGrade + 1;

        float splineParameter = 0;
        float splineParameterIncrement = 0.01f;
        float splineEndThreshold = 1.0f;

        bool splineFinished = false;

        float[] nodes = new float[rawPath.Count + curveOrder + 1];

        for(int i = 0; i < nodes.Length; i++) nodes[i] = (float)i / (nodes.Length - 1);

        while(splineParameter <= 1.0f && !splineFinished)
        {
            bSpline = new Vector3();
            for (int i = 0; i < rawPath.Count; i++)
            {
                bSpline += calculateBaseFunction(i, nodes, curveOrder, splineParameter) * rawPath[i];
            }

            if( (bSpline - rawPath[rawPath.Count - 1]).magnitude <= splineEndThreshold) splineFinished = true;

            path.Add(bSpline);
            splineParameter += splineParameterIncrement;
        }
        
        return path;
    }

    private float calculateBaseFunction(int nodeIndex, float[] nodes, int curveOrder, float splineParameter)
    {
        float N = 0.0f;

        if(curveOrder == 1) //NnodeIndex, 1(splineParameter)
        {
            if (splineParameter >= nodes[nodeIndex] && splineParameter < nodes[nodeIndex + 1]) N = 1.0f;
            else N = 0.0f;
        }

        else //NnodeIndex, curveOrder(splineParameter)
        {
            N = ( (splineParameter - nodes[nodeIndex]) / (nodes[nodeIndex + curveOrder - 1] - nodes[nodeIndex])) * calculateBaseFunction(nodeIndex, nodes, curveOrder - 1, splineParameter);
            N += ((nodes[nodeIndex + curveOrder] - splineParameter) / (nodes[nodeIndex + curveOrder] - nodes[nodeIndex + 1])) * calculateBaseFunction(nodeIndex + 1, nodes, curveOrder - 1, splineParameter);
        }

        return N;
    }*/

    public Waypoint getCloserWaypointToCoordinates(Vector3 v, List<Waypoint> waypoints) //Se compara la distancia entre cada waypoint con las coordenadas dadas y nos quedamos con el más cercano
    {
        //Presuponemos que el primer waypoint es el más cercano
        Waypoint closestWaypoint = (Waypoint)waypoints[0];
        float minDistance = (closestWaypoint.transform.position - v).magnitude;

        foreach (Waypoint waypoint in waypoints)
        {
            if ((waypoint.transform.position - v).magnitude < minDistance)
            {
                closestWaypoint = waypoint;
                minDistance = (waypoint.transform.position - v).magnitude;
            }
        }
        return closestWaypoint;
    }

    public void reset()
    {
        foreach (Waypoint waypoint in waypoints)
        {
            waypoint.exploredFrom = null;
        }
    }
}