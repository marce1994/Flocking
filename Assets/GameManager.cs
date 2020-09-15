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

    Alignment _alignment;
    Cohesion _cohesion;
    Separation _separation;
    Target _target;
    RunAway _runAway;
    Meat _meat;
    KeepInsideBok _keepInsideBok;

    private List<Waypoint> waypoints;
    private bool isEnemy = false;

    public Slider alignment;
    public Slider cohesion;
    public Slider separation;
    public Slider target;

    private void Awake()
    {
        waypoints = FindObjectsOfType<Waypoint>().ToList();
        cameraTransform = Camera.main.transform;

        boids = new List<Boid>();
        _alignment = new Alignment();
        _cohesion = new Cohesion();
        _separation = new Separation();
        _target = new Target();
        _runAway = new RunAway();
        _meat = new Meat();
        _keepInsideBok = new KeepInsideBok();

        _alignment.maxDist = _cohesion.maxDist = _separation.maxDist = _target.maxDist = _runAway.maxDist = 20;
        _alignment.minDist = _cohesion.minDist = _separation.minDist = _target.minDist = _meat.minDist = 1;
        _meat.maxDist = 50;
        _runAway.maxDist = 10;

        _alignment.scalar = 1;
        alignment.value = 1;
        _cohesion.scalar = 1;
        cohesion.value = 1;
        _separation.scalar = 1;
        separation.value = 1;
        _target.scalar = 2;
        target.value = 2;

        _runAway.scalar = 10;
        _meat.scalar = 5;
        _keepInsideBok.scalar = 100;

        alignment.onValueChanged.AddListener((x)=> {
            _alignment.scalar = (int)x;
        });

        cohesion.onValueChanged.AddListener((x) => {
            _cohesion.scalar = (int)x;
        });

        separation.onValueChanged.AddListener((x) => {
            _separation.scalar = (int)x;
        });

        target.onValueChanged.AddListener((x) => {
            _target.scalar = (int)x;
        });
    }

    private void AddBehaviour(iRule rule, int weight)
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(boxPosition, boxSize);
    }

    // Start is called before the first frame update
    public void SpawnBoid()
    {
        var gameObject = new GameObject();

        gameObject.transform.position = Random.insideUnitSphere * Random.Range(1, 100);
        
        GameObject buterfly = Instantiate(butterflyPrefab);
        buterfly.transform.position = gameObject.transform.position;
        buterfly.transform.localScale = Vector3.one / 2;
        buterfly.transform.parent = gameObject.transform;

        Boid boid = gameObject.AddComponent<Boid>();
        boid.velocity = Random.insideUnitSphere.normalized;
        boid.waypoint = waypoints.First(x => x.name == "Point");
        boid.speed = Random.Range(10,25);
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
        //run through all boids.
        for (int i = boids.Count - 1; i >= 0; --i)
        {
            Boid b = boids[i];
            flockCount++;
            middleOfFlock += b.position;
            //get the boids current velocity.
            Vector3 velocity = b.velocity;

            //add the influences of neighboring boids to the velocity.
            velocity += _alignment.getResult(boids, i);
            velocity += _cohesion.getResult(boids, i);
            velocity += _separation.getResult(boids, i);
            velocity += _target.getResult(boids, i);
            velocity += _runAway.getResult(boids, i);
            velocity += _meat.getResult(boids, i);
            velocity += _keepInsideBok.getResult(boids, i);

            //normalize the velocity and make sure that the boids new velocity is updated.
            velocity.Normalize();
            b.velocity = velocity;

            b.lookat = b.position + velocity;
        }

        if(boids.Count > 0)
        {
            //cameraTransform.position = middleOfFlock / flockCount;
            cameraTransform.LookAt(middleOfFlock / flockCount);
        }

        for (int i = boids.Count - 1; i >= 0; --i)
        {
            //update the boids position in the mainthread.
            boids[i].transform.position += boids[i].velocity * Time.deltaTime * boids[i].speed;
        }
    }
}