using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingTest : MonoBehaviour
{
    [SerializeField] Vector3 start;
    [SerializeField] Vector3 end;
    Pathfinder pathfinder;
    void Start()
    {
        pathfinder = FindObjectOfType<Pathfinder>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(FollowPath(pathfinder.getPath(start, end)));
        }
    }

    IEnumerator FollowPath(List<Waypoint> path)
    {
        foreach (Waypoint waypoint in path)
        {
            transform.position = waypoint.transform.position;
            yield return new WaitForSeconds(1f);
        }
    }
}
