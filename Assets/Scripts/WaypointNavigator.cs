using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointNavigator : MonoBehaviour
{
    PedestrianCharacterController controller;
    public Waypoint curentWaypoint;
    int direction;
    private void Awake()
    {
        controller = GetComponent<PedestrianCharacterController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        direction = Mathf.RoundToInt(Random.Range(0f, 1f));
        controller.SetDestination(curentWaypoint.GetPosition());
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.reachedDestination)
        {
            bool shouldBranch = false;

            if (curentWaypoint.branches != null && curentWaypoint.branches.Count > 0)
                shouldBranch = Random.Range(0f, 1f) <= curentWaypoint.branvhRatio ? true : false;

            if (shouldBranch)
            {
                curentWaypoint = curentWaypoint.branches[Random.Range(0, curentWaypoint.branches.Count - 1)];
            }
            else
            {
                if (direction == 0)
                {
                    if (curentWaypoint.nextWaypoint != null)
                    {
                        curentWaypoint = curentWaypoint.nextWaypoint;
                    }
                    else
                    {
                        curentWaypoint = curentWaypoint.previousWaypoint;
                        direction = 1;
                    }
                }
                else if (direction == 1)
                {
                    if (curentWaypoint.previousWaypoint != null)
                    {
                        curentWaypoint = curentWaypoint.previousWaypoint;
                    }
                    else
                    {
                        curentWaypoint = curentWaypoint.nextWaypoint;
                        direction = 0;
                    }
                }
            }
            controller.SetDestination(curentWaypoint.GetPosition());
        }
    }
}
