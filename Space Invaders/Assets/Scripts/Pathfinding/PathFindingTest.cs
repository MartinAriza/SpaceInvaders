using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingTest : MonoBehaviour
{
    //[SerializeField] Vector3 start;
    //[SerializeField] Vector3 end;
    Pathfinder pathfinder;
    void Start()
    {
        pathfinder = GetComponent<Pathfinder>();
    }

    private void Update()
    {
        Random.InitState((int)transform.position.magnitude);

        Vector3 randomVector = new Vector3(
            Random.Range(-20, 20),
            Random.Range(-10, 10),
            Random.Range(-20, 20));

        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(FollowPath(pathfinder.getPath(transform.position, randomVector)));
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
