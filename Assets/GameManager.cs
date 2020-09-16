using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Slider;

/*
aca va de nuevo y poco mas claro
Crear un sistema con 3 tipos de entidades (boids, amenazas y comestibles) y con las siguientes características:

Un BoidManager se encargará de instanciar inicialmente una cantidad X de boids
Un FoodSpawner creará instancias de objetos comestibles con cierta frecuencia, que caerán como copos de nieve desde arriba de donde se hallen los boids.
Los comestibles no tienen más comportamiento que caer.
Las amenazas no tienen más comportamiento que desplazarse dentro de la escena (y eventualmente aparecer del otro lado si se alejan mucho)
Desde el inspector del BoidManager podrán modificarse los pesos de los comportamientos de los boids.
En la escena habrá un recorrido dado por una serie de Vector3 en la escena (Waypoints).
Cada boid se comportará según sus steering behaviours:
Evadir amenazas (peso 10-20)
Perseguir comestibles (peso 5-10)
Separación
Alineación
Cohesión
Seguimiento de waypoints.
Cuando un boid se halle lo suficientemente cerca de un comestible, lo comerá, eliminándolo de la escena e incrementando un

 */


public class GameManager : MonoBehaviour
{
    private Transform cameraTransform;
    public GameObject butterflyPrefab;

    public List<Boid> boids { get; private set; }

    public Vector3 boxPosition;
    public Vector3 boxSize;

    List<iRule> behaviours;

    //Alignment _alignment;
    //Cohesion _cohesion;
    //Separation _separation;
    //Target _target;
    //RunAway _runAway;
    //Meat _meat;
    //KeepInsideBok _keepInsideBok;

    private List<Waypoint> waypoints;
    private bool isEnemy = false;

    public Slider alignment;
    public Slider cohesion;
    public Slider separation;
    public Slider target;

    // Refactor -> no quiero abstraer y reescribir cosas :)
    private void AddBehaviour<T>(int viewRange, int weight) where T : iRule
    {
        if (behaviours == null)
            behaviours = new List<iRule>();

        T behaviour = (T)Activator.CreateInstance(typeof(T));
        
        behaviour.minDist = 1;
        behaviour.maxDist = viewRange;
        behaviour.scalar = weight;
        
        behaviours.Add(behaviour);
    }

    private void Awake()
    {
        waypoints = FindObjectsOfType<Waypoint>().ToList();
        boids = new List<Boid>();
        cameraTransform = Camera.main.transform;

        int viewRange = 20;

        AddBehaviour<Alignment>(viewRange, 1);
        AddBehaviour<Cohesion>(viewRange, 1);
        AddBehaviour<Separation>(viewRange, 1);
        AddBehaviour<Target>(viewRange, 1);
        AddBehaviour<Scape>(viewRange, 8);
        AddBehaviour<Eat>(viewRange, 8);
        AddBehaviour<KeepInsideBok>(viewRange, 100);

        alignment.onValueChanged.AddListener((x)=> {
            var alignments = behaviours.Where(y => y is Alignment);
            foreach (var alignment in alignments)
            {
                alignment.scalar = (int)x;
            }
        });

        cohesion.onValueChanged.AddListener((x) => {
            var alignments = behaviours.Where(y => y is Cohesion);
            foreach (var alignment in alignments)
            {
                alignment.scalar = (int)x;
            }
        });

        separation.onValueChanged.AddListener((x) => {
            var alignments = behaviours.Where(y => y is Separation);
            foreach (var alignment in alignments)
            {
                alignment.scalar = (int)x;
            }
        });

        target.onValueChanged.AddListener((x) => {
            var alignments = behaviours.Where(y => y is Target);
            foreach (var alignment in alignments)
            {
                alignment.scalar = (int)x;
            }
        });
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(boxPosition, boxSize);
    }

    // Start is called before the first frame update
    public void SpawnBoid()
    {
        var gameObject = new GameObject();

        gameObject.transform.position = UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(1, 100);
        
        GameObject buterfly = Instantiate(butterflyPrefab);
        buterfly.transform.position = gameObject.transform.position;
        buterfly.transform.localScale = Vector3.one / 2;
        buterfly.transform.parent = gameObject.transform;

        Boid boid = gameObject.AddComponent<Boid>();
        boid.direction = UnityEngine.Random.insideUnitSphere.normalized;
        boid.waypoint = waypoints.First(x => x.name == "Point");
        boid.speed = UnityEngine.Random.Range(10,25);
        boid.boxPosition = boxPosition;
        boid.boxSize = boxSize;

        boids.Add(boid);
    }

    // Update is called once per frame
    void Update()
    {
        IterateBoids();
        if (Input.GetKey(KeyCode.Space))
            SpawnBoid();
    }

    void IterateBoids() {
        Vector3 middleOfFlock = Vector3.zero;
        int flockCount = 0;

        for (int i = boids.Count - 1; i >= 0; --i)
        {
            Boid boid = boids[i];
            flockCount++;
            middleOfFlock += boid.position;

            Vector3 rendToDirection = boid.direction;

            foreach (var behaviour in behaviours)
            {
                rendToDirection += behaviour.getResult(boids, i);
            }

            rendToDirection.Normalize();
            boid.direction = rendToDirection;
            boid.lookat = boid.position + rendToDirection;
        }

        if(boids.Count > 0)
            cameraTransform.LookAt(middleOfFlock / flockCount);
        
        for (int i = boids.Count - 1; i >= 0; --i)
            boids[i].transform.position += boids[i].direction * Time.deltaTime * boids[i].speed;
    }
}