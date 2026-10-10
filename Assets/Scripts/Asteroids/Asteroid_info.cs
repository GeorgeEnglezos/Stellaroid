using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Asteroid_info : MonoBehaviour
{
    
    [Header("Stats")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    
    [Header("Pool")]
    public string tag;
    public int size;
    [SerializeField] private float distanceToEnqueue;
    [SerializeField] private float distance;

    [Header("OTHER")]
    [SerializeField] public Transform Player;
    [SerializeField] public GameObject spawner;
    [SerializeField] private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void resetAsteroid()
    {
        currentHealth = maxHealth;
        float maxThrust = GameManager.Instance.difficultiesList[GameManager.Instance.difficultyLevel].maxThrust;
        float maxTorque = GameManager.Instance.difficultiesList[GameManager.Instance.difficultyLevel].maxTorque;
//        Debug.Log("Thrust: "+maxThrust);
        Vector2 thrust = new Vector2(Random.Range(-maxThrust, maxThrust), UnityEngine.Random.Range(-maxThrust, maxThrust));
        float torque = Random.Range(-maxTorque, maxTorque);
        rb.AddForce(thrust);
        rb.AddTorque(torque);
    }

    private void Update()
    {
        distance = Vector3.Distance(Player.position, transform.position);
        if (distance>distanceToEnqueue)
        {
            ObjectPooler.Instance.enqueueGameObject(gameObject,tag,true);
        }
    }

    public void takeDamage(int damage)
    {
        EventBroadcaster.ListEventArgs data=new EventBroadcaster.ListEventArgs(new List<string>());
        data.Data.Add(size.ToString());
        EventBroadcaster.call_Event_Hit_asteroid(data.Data);
        
        currentHealth -= damage;
        if (currentHealth<=0)
        {
            Debug.Log("destroyed asteroid");
            EventBroadcaster.call_Event_Destroy_asteroid(data.Data);
            spawner.GetComponent<Object_Spawner>().DestroyAsteroid(gameObject,size);
        }
    }


}
