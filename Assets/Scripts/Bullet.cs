using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int tag;
    [SerializeField] private bool canMove;
    public string enemyTag;
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private int damage;
    private Vector3 originalScale;

    [SerializeField] private float enqueueTimer;
    [SerializeField] private float enqueueCooldown;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
    }

    private void Start()
    {
        canMove = true;
        enqueueTimer = 0;
    }

    private void OnEnable()
    {
        canMove = true;
        enqueueTimer = 0;
    }

    private void Update()
    {
        enqueueTimer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            rb.linearVelocity = transform.up * speed;
        }

        if (enqueueTimer>enqueueCooldown)
        {
            if (tag==0)
            {
                ObjectPooler.Instance.enqueueGameObject(gameObject,"weapon0",true);
            }else if (tag==1)
            {
                ObjectPooler.Instance.enqueueGameObject(gameObject,"weapon1",true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Asteroid"))
        {
            other.GetComponent<Asteroid_info>().takeDamage(damage);
            if (tag==0)
            {
                GameObject tmp=ObjectPooler.Instance.spawnFromPool("bulletCol",transform.position,transform.rotation);
                if (tmp)
                {
                    tmp.GetComponent<ParticleSystem>().Play();
                }
                ObjectPooler.Instance.enqueueGameObject(gameObject,"weapon"+tag,true);
            }else if (tag==1)
            {
                canMove = false;
                StartCoroutine(bombEffect(other));
            }
        }

        if (other.CompareTag(enemyTag))
        {
            if (enemyTag=="Player")
            {
                other.GetComponent<Spaceship_Info>().takeDamage(4);
            }else if (enemyTag=="Enemy")
            {
                if (tag == 1)
                {                
                    other.GetComponent<Enemy_Info>().takeDamage(8);
                }
                else
                {other.GetComponent<Enemy_Info>().takeDamage(4);
                }                


            }
            if (tag==0)
            {
                GameObject tmp=ObjectPooler.Instance.spawnFromPool("bulletCol",transform.position,transform.rotation);
                if (tmp)
                {
                    tmp.GetComponent<ParticleSystem>().Play();
                }
                ObjectPooler.Instance.enqueueGameObject(gameObject,"weapon"+tag,true);
            }else if (tag==1)
            {
                canMove = false;
                StartCoroutine(bombEffect(other));
            }
        }
    }

    private IEnumerator bombEffect(Collider2D other)
    {

        rb.linearVelocity=Vector2.zero;
        gameObject.transform.localScale = new Vector3(1, 1, 0);
        yield return new WaitForSeconds(.1f);
        gameObject.transform.localScale = new Vector3(2, 2, 0);
        yield return new WaitForSeconds(.1f);
        gameObject.transform.localScale = new Vector3(3, 3, 0);
        yield return new WaitForSeconds(.1f);
        gameObject.transform.localScale = new Vector3(4, 4, 0);
        ResetTransform();
        SoundManager.Inst.Play("CanonExplosion",false,0f);
        ObjectPooler.Instance.enqueueGameObject(gameObject,"weapon"+tag,true);
    }

    private void ResetTransform()
    {
        transform.localScale = originalScale;
    }
}
