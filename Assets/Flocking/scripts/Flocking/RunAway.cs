using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAway : iRule
{
    public int minDist { get; set; }
    public int maxDist { get; set; }
    public int scalar { get; set; }

    public Vector3 getResult(List<Boid> boids, int current)
    {
        Boid currentBoid = boids[current];

        if (currentBoid.enemyPosition == null)
            return Vector3.zero;

        if (Vector3.Distance(currentBoid.transform.position, currentBoid.enemyPosition.position) < maxDist)
            return (currentBoid.transform.position - currentBoid.enemyPosition.position).normalized * scalar;

        return Vector3.zero;
    }
}