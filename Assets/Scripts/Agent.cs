using UnityEngine;

public class Agent : MonoBehaviour
{
    Vector3 Direction;

    private void Awake()
    {
        Direction = Random.insideUnitSphere;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Direction;
    }
}
