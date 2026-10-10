using System;
using UnityEngine;

public class Spaceship_Radar : MonoBehaviour
{
    private Collider2D[] asteroids;
    [Header("Agro")]
    [SerializeField] private float radius;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Object_Spawner _asteroidSpawner;
    [Header("Spawning")] 
    public int asteroidsToSpawn;
    [SerializeField] private int AsteroidsInRange;
    [SerializeField] private float spawnCooldown;
    [SerializeField] private float spawnTimer;

    private void Awake()
    {
        EventBroadcaster.OnGameStart += Call_Event_Game_Start;
    }

    private void Call_Event_Game_Start(object sender, EventArgs e)
    {
        asteroids = Physics2D.OverlapCircleAll(transform.position, radius, _layerMask);
        AsteroidsInRange = asteroids.Length;
        foreach (var aster in asteroids)
        {
            ObjectPooler.Instance.enqueueGameObject(aster.gameObject,aster.GetComponent<Asteroid_info>().tag,true);
        }
        spawnTimer = 4;
    }

    private void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer>spawnCooldown)
        {
            asteroids = Physics2D.OverlapCircleAll(transform.position, radius, _layerMask);
            AsteroidsInRange = asteroids.Length;
            if (AsteroidsInRange<7)
            {
                
                Debug.Log("Spawning new Asteroids");
                _asteroidSpawner.SpawnNewAsteroids(asteroidsToSpawn+AsteroidsInRange);
            }
            spawnTimer = 0;
        }

    }

    private void OnDrawGizmos()
    {
        Color color=Color.magenta;
        Gizmos.DrawWireSphere (transform.position, radius);
    }
}
