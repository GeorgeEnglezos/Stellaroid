using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    [SerializeField] public GameObject Player;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float maxPlayerDistance;
    [SerializeField] private float speed;
    [SerializeField] private bool moveTowardsPlayer;

    private Enemy_Info _enemyInfo;

    private void Awake()
    {        
        _enemyInfo=GetComponent<Enemy_Info>();

    }    
    private void OnEnable()
    {
        Player=GameObject.Find("Spaceship");
    }
    void Update()
    {
//        Debug.Log(Vector3.Distance(transform.position,Player.transform.position));
        if (Vector3.Distance(transform.position,Player.transform.position)>maxPlayerDistance)
        {
            moveTowardsPlayer = true;
        }
        else
        {           
            moveTowardsPlayer = false;
        }

       

    }

    private void FixedUpdate()
    {
        Vector3 diff = Player.transform.position - transform.position;
        //diff.Normalize();
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        
        if (moveTowardsPlayer && _enemyInfo.canMove)
        {
            rb.AddForce(transform.up*speed);
        }

    }


}
