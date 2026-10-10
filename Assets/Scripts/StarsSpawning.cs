using UnityEngine;

public class StarsSpawning : MonoBehaviour
{
    [SerializeField] private float spawnTimer;
    [SerializeField] private float spawnCooldown;
    [SerializeField] private ParticleSystem[] ps;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (spawnTimer>=spawnCooldown)
        {
            spawnTimer = 0;
            for (int i = 0; i < ps.Length; i++)
            {
                ps[i].Play();
            }
        }
    }
}
