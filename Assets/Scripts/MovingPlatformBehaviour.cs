using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class MovingPlatformBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject[] Waypoints = null;

    //waypoint to which the platform has to go
    private int CurrentWaypointIndex = 0;

    [Range(0, 10.0f)][SerializeField] private float PlatformSpeed = 2.0f;

    // Update is called once per frame
    void Update()
    {
        if (UnityEngine.Vector2.Distance(Waypoints[CurrentWaypointIndex].transform.position, transform.position) < .1f) { 
            CurrentWaypointIndex = CurrentWaypointIndex + 1;
            if(CurrentWaypointIndex >= Waypoints.Length)
            {
                CurrentWaypointIndex = 0;
            }
        }
        transform.position = UnityEngine.Vector2.MoveTowards(transform.position, Waypoints[CurrentWaypointIndex].transform.position, Time.deltaTime * PlatformSpeed);
    }
}
