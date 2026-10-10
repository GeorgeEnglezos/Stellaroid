using TMPro;using UnityEditor;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class Spaceship_Weapons : MonoBehaviour
{
    
    [Header("UI")]
    [SerializeField] private GameObject bulletUI_Desktop;
    [SerializeField] private GameObject bulletUI_Android;
    [SerializeField] private Sprite[] bulletSprite;
    
    [Header("Weapon_Variables")]
    [SerializeField] private Transform shootingPos;
    [SerializeField] private int currentWeapon;
    [SerializeField] private int[] weaponBullets;
    [SerializeField] private bool shooting;
    
    [Header("Weapon_Cooldowns")]
    [SerializeField] private float basicWeaponTimer;
    [SerializeField] private float basicWeaponCooldown;
    [SerializeField] private float heavyWeaponTimer;
    [SerializeField] private float heavyWeaponCooldown;
    public static Spaceship_Weapons Instance;

    private void Awake()
    {
        Instance = this;
    }
    
    public void resetWeapons()
    {
        currentWeapon = 0;
        weaponBullets[0] = 100;
        weaponBullets[1] = 30;
        resetUI();
    }


    private void resetUI()
    {
        if (GameManager.Instance.DesktopBuild)
        {
            bulletUI_Desktop.GetComponent<Image>().sprite = bulletSprite[currentWeapon];
            bulletUI_Desktop.GetComponentInChildren<TextMeshProUGUI>().text = weaponBullets[currentWeapon] + "\n/\n100";
        }
        else if (GameManager.Instance.AndroidBuild)
        {
            bulletUI_Android.GetComponent<Image>().sprite = bulletSprite[currentWeapon];
            bulletUI_Android.GetComponentInChildren<TextMeshProUGUI>().text = weaponBullets[currentWeapon] + "\n/\n100";
        }

    }

    public void changeWeapon()
    {
        shooting = false;
        if (currentWeapon == 0)
        {
            currentWeapon = 1;
        }
        else currentWeapon = 0;

        resetUI();

    }

    private void Update()
    {
        basicWeaponTimer += Time.deltaTime;
        heavyWeaponTimer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.E))
        {
            changeWeapon();
        }

        if (GameManager.Instance.DesktopBuild)
        {
            if (Input.GetKey(KeyCode.Space)&& GameManager.Instance.gameStarted)
            {
                shooting = true;
            }
            else
            {
                shooting = false;
            }
        }

    }

    private void FixedUpdate()
    {
        if (shooting && weaponBullets[currentWeapon] > 0)
        {
            if (currentWeapon == 0 && basicWeaponTimer > basicWeaponCooldown)
            {
                basicWeaponTimer = 0;
                SoundManager.Inst.Play("Laser",false,0f);
                shoot(0);
            }
            else if (currentWeapon == 1 && heavyWeaponTimer > heavyWeaponCooldown)
            {
                SoundManager.Inst.Play("Canon",false,0f);
                heavyWeaponTimer = 0;
                shoot(1);
            }
        }
    }

    private void shoot(int current)
    {
        basicWeaponTimer = 0;
        weaponBullets[current]--;
        resetUI();
        GameObject tmp=ObjectPooler.Instance.spawnFromPool("weapon" + current, shootingPos.position, shootingPos.rotation);
        if (tmp)
        {
            tmp.GetComponent<Bullet>().enemyTag = "Enemy";
        }
        //ObjectPooler.Instance.enqueueGameObject(tmp,"weapon" + current,false);
    }

    public void getNewBullets(int id, int bullets)
    {
        weaponBullets[id] += bullets;
        if (id == 0 && weaponBullets[id] > 100)
        {
            weaponBullets[id] = 100;
        }
        else if (id == 1 && weaponBullets[id] > 30)
        {
            weaponBullets[id] = 30;
        }

        resetUI();
    }

    public void PressedShootButton()
    {
        shooting = true;
    }
    public void ReleasedShootButton()
    {
        shooting = false;
    }
}