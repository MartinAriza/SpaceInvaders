using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    
    static ArrayList waypoints = new ArrayList();   //Referencia a todos los waypoints de la escena
    Queue<Waypoint> queue = new Queue<Waypoint>();  //Cola de procesamiento de waypoints en orden de profundidad
    bool foundEndWaypoint = false;

    #region splines
    [SerializeField]int splineGrade = 3;
    [SerializeField] bool splineAproximation = false;
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

    public List<Vector3> getPath(Vector3 start, Vector3 end)
    {
        //Waypoints inicial y final aproximados según las coordenadas dadas en el mundo
        Waypoint startWaypoint = getCloserWaypointToCoordinates(start);
        Waypoint endWaypoint = getCloserWaypointToCoordinates(end);

        BreadthFirstSearch(startWaypoint, endWaypoint);
        List<Vector3> path = createPath(startWaypoint, endWaypoint);
        return path;
    }

    private void BreadthFirstSearch(Waypoint startWaypoint, Waypoint endWaypoint)
    {
        ArrayList visitedWaypoints = new ArrayList(); 
        Waypoint searchCenter;  //Waypoint alrededor del cual estamos buscando "vecinos"
        foundEndWaypoint = false;

        queue.Enqueue(startWaypoint);   //Metemos en la cola de procesamiento el Waypoint incial

        while (!foundEndWaypoint && queue.Count > 0)
        {
            searchCenter = queue.Dequeue();
            visitedWaypoints.Add(searchCenter);

            if (searchCenter == endWaypoint) foundEndWaypoint = true;
            ExploreNeighbours(searchCenter, visitedWaypoints);  //Exploramos los waypoints vecinos del waypoint searchCenter
        }
    }

    private void ExploreNeighbours(Waypoint searchCenter, ArrayList visitedWaypoints)
    {
        if (foundEndWaypoint) return;

        foreach (Waypoint waypoint in searchCenter.explorableWaypoints)
        {
            if ( visitedWaypoints.Contains(waypoint) || queue.Contains(waypoint) ) { }
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

        while (previousWaypoint != startWaypoint)
        {
            path.Add(previousWaypoint.transform.position);
            previousWaypoint = previousWaypoint.exploredFrom;
        }
        path.Add(startWaypoint.transform.position);
        path.Reverse(); //le damos la vuelta al camino para que coincida con el que ha de recorrer el agente
        Debug.Log("First point is: " + path[0] + " last point is: " + path[path.Count - 1]);
        if(splineAproximation)
            path = createSplinePath(path);  //Tenemos un camino lineal y queremos conseguir un camino suave usando splines

        return path;
    }

    private List<Vector3> createSplinePath(List<Vector3> rawPath)
    {
        List<Vector3> path = new List<Vector3>();

        Vector3 bSpline = new Vector3();
        int curveOrder = splineGrade + 1;

        float splineParameter = 0;
        float splineParameterIncrement = 0.01f;

        float[] nodes = new float[rawPath.Count + curveOrder + 1];

        for(int i = 0; i < nodes.Length; i++) nodes[i] = (float)i / (nodes.Length - 1);

        while(splineParameter <= 1)
        {
            bSpline = new Vector3();
            for (int i = 0; i < rawPath.Count; i++)
            {
                bSpline += calculateBaseFunction(i, nodes, curveOrder, splineParameter) * rawPath[i];
            }

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
    }

    private Waypoint getCloserWaypointToCoordinates(Vector3 v) //Se compara la distancia entre cada waypoint con las coordenadas dadas y nos quedamos con el más cercano
    {
        //Presuponemos que el primer waypoint es el más cercano
        Waypoint closestWaypoint = (Waypoint)waypoints[0];
        float minDistance = (closestWaypoint.transform.position - v).magnitude;

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
 * Hacer que el movimiento de los agentes sea suave con splines (BUG: Último y primer punto del path conectados, probablemente fallo de índices)
 * Añadir sistema de patrulla
 * Detección de "ruido"
 * Añadir cono de visión a los agentes
 * Hacer que los agentes puedan disparar a su objetivo
 */