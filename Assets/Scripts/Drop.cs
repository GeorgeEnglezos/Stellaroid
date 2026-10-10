using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Drop : MonoBehaviour
{
    public Transform Player;
    public int dropID;
    [Header("Stats")]
    [SerializeField] private float maxthrust;
    [SerializeField] private float maxtorque;
    [SerializeField] public int value;    
    [Header("Pool")]
    public string tag;
    [SerializeField] private float distanceToEnqueue;
    [SerializeField] private float distance;
    [SerializeField] private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector2 thrust = new Vector2(Random.Range(-maxthrust, maxthrust), UnityEngine.Random.Range(-maxthrust, maxthrust));
        float torque = Random.Range(-maxtorque, maxtorque);
        rb.AddForce(thrust);
        rb.AddTorque(torque);
        if (tag=="bullets0")
        {
            value = (int) Math.Floor((double)Random.Range(10,60));
        }else if (tag=="bullets1")
        {
            value = (int) Math.Floor((double)Random.Range(5,16));
        }
    }

    private void Update()
    {
        distance = Vector3.Distance(Player.position, transform.position);
        if (distance>distanceToEnqueue)
        {
            ObjectPooler.Instance.enqueueGameObject(gameObject,tag,true);
            
        }
    }
}
