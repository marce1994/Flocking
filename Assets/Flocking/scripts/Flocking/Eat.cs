﻿using System.Collections.Generic;
using UnityEngine;

public class Eat : iRule
{
    public int minDist { get; set; }
    public int maxDist { get; set; }
    public int scalar { get; set; }

    public Vector3 getResult(List<Boid> boids, int current)
    {
        Vector3 result = Vector3.zero;
        Boid currentBoid = boids[current];

        if (currentBoid.meatPosition == null)
            return Vector3.zero;

        if (Vector3.Distance(currentBoid.transform.position, currentBoid.meatPosition.position) < maxDist)
            result = (currentBoid.meatPosition.position - currentBoid.transform.position).normalized * scalar;

        if (Vector3.Distance(currentBoid.transform.position, currentBoid.meatPosition.position) < minDist)
        {
            bool eat = currentBoid.flower.Eat();

            if (eat) {
                var instance = GameManager.GetInstance();
                instance.InstantiateEnemy(currentBoid.gameObject.transform.position);
                instance.DeleteBoid(currentBoid);
            }
        }

        return result;
    }

}
