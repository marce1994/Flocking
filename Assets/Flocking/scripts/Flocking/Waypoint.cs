using System.Linq;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public Waypoint _nextWaypoint;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, _nextWaypoint.transform.position);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.3f);
    }

    public Waypoint getNextWaypoint()
    {
        if (_nextWaypoint == null) {
            _nextWaypoint = FindObjectsOfType<Waypoint>().First(x => x != this);
        }

        return _nextWaypoint;
    }
}
