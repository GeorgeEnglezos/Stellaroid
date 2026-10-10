using UnityEngine;

public class Enemy_Weapon : MonoBehaviour
{
    [Header("Weapon")] 
    [SerializeField] private GameObject Player;
    [SerializeField] private Transform shootingPos;
    [SerializeField] private bool CanShoot;
    [SerializeField] private bool shootingRange;
    [SerializeField] private bool isShooting;

    [Header("Max_Bullets")] 
    [SerializeField] private int bulletsShot;
    [SerializeField] private int maxBullets;
    
    [Header("Weapon_Cooling")]
    [SerializeField] private bool isResting;


    [Header("Standard_Shooting_Cooldown")]
    [SerializeField] private float shootTimer;
    [SerializeField] private float shootCooldown;
    
    private Enemy_Info _enemyInfo;

    private void Awake()
    {        
        _enemyInfo=GetComponent<Enemy_Info>();

    }

    public void resetWeapons()
    {
        CanShoot = true;
        bulletsShot = 0;
        Player=GameObject.Find("Spaceship");
    }

    void Update()
    {
        //Shooting Range
        if (Vector3.Distance(transform.position,Player.transform.position)<10)
        {
            shootingRange = true;
        }
        else
        {
            shootingRange = false;
        }
        

        //Timers for shooting interval
        if (shootTimer<=shootCooldown)
        {
            shootTimer += Time.deltaTime;
        }
        else
        {
            CanShoot = true;
        }
    }

 
    private void FixedUpdate()
    {
        
        if (shootingRange && !isResting && CanShoot && _enemyInfo.canMove)
        {
            shootTimer = 0;
            CanShoot = false;
            
            isShooting = true;
            bulletsShot++;
            if (bulletsShot>=maxBullets)
            {
                isResting = true;
                CanShoot = false;
                isShooting = false;
                Invoke("resetBullets",5f);
            }
            shoot();
        }
    }

    private void resetBullets()
    {
        bulletsShot = 0;
        isResting = false;
    }

    private void shoot()
    {
        GameObject tmp=ObjectPooler.Instance.spawnFromPool("weapon0", shootingPos.position, shootingPos.rotation);
        if (tmp)
        {
            tmp.GetComponent<Bullet>().enemyTag = "Player";
        }
    }
}
