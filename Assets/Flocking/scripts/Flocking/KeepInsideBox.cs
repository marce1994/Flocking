using System.Collections.Generic;
using UnityEngine;

public class KeepInsideBok : iRule
{
    public int minDist { get; set; }
    public int maxDist { get; set; }
    public int scalar { get; set; }

    public Vector3 getResult(List<Boid> boids, int current)
    {
        Boid currentBoid = boids[current];

        float minx = currentBoid.boxPosition.x - currentBoid.boxSize.x / 2;
        float miny = currentBoid.boxPosition.y - currentBoid.boxSize.y / 2;
        float minz = currentBoid.boxPosition.z - currentBoid.boxSize.z / 2;

        float maxx = currentBoid.boxPosition.x + currentBoid.boxSize.x / 2;
        float maxy = currentBoid.boxPosition.y + currentBoid.boxSize.y / 2;
        float maxz = currentBoid.boxPosition.z + currentBoid.boxSize.z / 2;

        Vector3 pos = currentBoid.transform.position;

        bool inside = (pos.x > minx && pos.x < maxx && pos.y > miny && pos.y < maxy && pos.z > minz && pos.z < maxz);

        return inside ? Vector3.zero : -pos.normalized * scalar;
    }
}
