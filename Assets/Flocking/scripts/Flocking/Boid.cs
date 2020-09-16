using System.Collections;
using System.Linq;
using UnityEngine;

public class Boid : MonoBehaviour 
{
    public Vector3 position { get; set; }
    public Vector3 direction { set; get; }
    public Vector3 lookat   { get; set; }
    public Waypoint waypoint { get; set; }

    public Transform enemyPosition { get; set; }
    public Transform meatPosition { get; set; }

    public Vector3 boxPosition { get; set; }
    public Vector3 boxSize { get; set; }

    public FlowerManager flower { get; set; }

    public float speed;

    private void Awake()
    {
        StartCoroutine(UpdateBoidReferences(0.1f));
    }

    private IEnumerator UpdateBoidReferences(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);

            var flowers = FindObjectsOfType<FlowerManager>().Where(x => x.CanEat);
            var closestFlower = flowers.FirstOrDefault();
            foreach (var flower in flowers)
            {
                var newDistance = Vector3.Distance(gameObject.transform.position, flower.transform.position);
                var oldDistance = Vector3.Distance(gameObject.transform.position, closestFlower.transform.position);
                if (newDistance < oldDistance)
                    closestFlower = flower;
            }

            flower = closestFlower;

            var enemies = FindObjectsOfType<EnemyManager>();
            var closestEnemy = enemies.FirstOrDefault();
            foreach (var enemy in enemies)
            {
                var newDistance = Vector3.Distance(gameObject.transform.position, enemy.transform.position);
                var oldDistance = Vector3.Distance(gameObject.transform.position, closestEnemy.transform.position);
                if (newDistance < oldDistance)
                    closestEnemy = enemy;
            }
            
            if (closestEnemy != null)
                enemyPosition = closestEnemy.transform;

            if (flower == null)
            {
                meatPosition = null;
            }
            else {
                meatPosition = flower.transform;
            }
        }
    }

    void Update()
    {
        position = transform.position;
        transform.LookAt(position + direction);
        transform.position += direction * Time.deltaTime * speed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(position, direction.normalized);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(position, lookat.normalized);
    }
}
