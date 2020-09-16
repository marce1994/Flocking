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
        StartCoroutine(WaitAndFindToEat(1));

        var enemy = FindObjectOfType<EnemyManager>();
        if (enemy == null) return;
            enemyPosition = enemy.gameObject.transform;
    }

    private IEnumerator WaitAndFindToEat(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            flower = FindObjectsOfType<FlowerManager>().FirstOrDefault(x => x.CanEat);
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
        transform.LookAt( lookat );
        if (flower == null) meatPosition = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(position, direction.normalized);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(position, lookat.normalized);
    }
}
