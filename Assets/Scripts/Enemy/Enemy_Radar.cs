using System.Collections;
using UnityEngine;

public class Enemy_Radar : MonoBehaviour
{
    private Collider2D[] asteroids;
    public GameObject Player;

    [SerializeField] private Animator anim;
    [SerializeField] private CircleCollider2D circleCol;

    [Header("Agro")]
    [SerializeField] private float radius;
    [SerializeField] private LayerMask _asteroidLayerMask;
    [Header("Timers")] 
    [SerializeField] private float teleportCooldown;
    [SerializeField] private float teleportTimer;

    private void OnEnable()
    {
        Player=GameObject.Find("Spaceship");
    }

    private void Update()
    {
        teleportTimer += Time.deltaTime;
        if (teleportTimer>teleportCooldown && (FoundAsteroids(transform.position) || Vector3.Distance(Player.transform.position,transform.position)<2))
        {
            teleportTimer = 0;
            StartCoroutine(Teleport());
        }
    }

    private IEnumerator Teleport()
    {
        circleCol.enabled = false;
        anim.Play("TeleportationFade");
        Vector3 newPos=FindTeleportPosition();
        yield return new WaitForSeconds(.2f);
        if (newPos!=Vector3.zero)
        {
            transform.position = newPos;
        }
        anim.Play("TeleportationAppear");
        circleCol.enabled = true;

    }
    private Vector3 FindTeleportPosition()
    {
        int teleportCounter=0;
        bool foundPos=false;
        float MaxPlayerDistance=100;
        Vector3 posToCheck=Vector3.zero;
        Vector3 goodPos=Vector3.zero;
        Vector3 bestPos=Vector3.zero;
        do
        {
            posToCheck = new Vector3(transform.position.x+UnityEngine.Random.Range(-10, 10),transform.position.y+ UnityEngine.Random.Range(-10, 10));
            if (!FoundAsteroids(posToCheck))
            {
                Debug.Log("Found teleportation location");
                goodPos = posToCheck;
                float playerDist = Vector3.Distance(posToCheck, Player.transform.position);
                if (playerDist<8 && playerDist>3)
                {
                    bestPos = posToCheck;
                    break;
                }
            }
            teleportCounter++;
        } while (teleportCounter <15);

        if (bestPos!=Vector3.zero)
        {
            return bestPos;
        }else if (goodPos!=Vector3.zero)
        {
            return goodPos;
        }
        else return Vector3.zero;
    }

    private bool FoundAsteroids(Vector3 posToCheck)
    {
        asteroids=Physics2D.OverlapCircleAll(posToCheck, radius, _asteroidLayerMask);
        if (asteroids.Length > 0)
        {
            return true;
        }
        else return false;
    }

    private void OnDrawGizmos()
    {
        Color color=Color.magenta;
        Gizmos.DrawWireSphere (transform.position, radius);
    }
}
