using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Target : iRule
{
    public int minDist { get; set; }
    public int maxDist { get; set; }
    public int scalar { get; set; }

    public Target()
    {
    }

    //create an empty vector to store the result of the flocking rule.
    public Vector3 getResult(List<Boid> boids, int current)
    {
        Boid currentBoid = boids[current];

        Debug.DrawLine(boids[current].transform.position, currentBoid.waypoint.transform.position);

        if (Vector3.Distance(currentBoid.waypoint.transform.position, currentBoid.transform.position) < maxDist/2)
            currentBoid.waypoint = currentBoid.waypoint.getNextWaypoint();
        return (currentBoid.waypoint.transform.position - boids[current].transform.position).normalized * scalar;
    }
}