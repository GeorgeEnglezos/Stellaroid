using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship_Collisions : MonoBehaviour
{
    [SerializeField] private bool canTakeDamage;
    [SerializeField] private float damageCooldown;
    [SerializeField] private Material originalMaterial;
    [SerializeField] private GameObject skin;

    [SerializeField] private Spaceship_Weapons _weapons;
    private void Start()
    {
        originalMaterial = skin.GetComponent<SpriteRenderer>().material;
        canTakeDamage = true;
        _weapons = GetComponent<Spaceship_Weapons>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (canTakeDamage)
        {
            if (other.transform.CompareTag("Asteroid"))
            {
                SoundManager.Inst.Play("SpaceshipCrush",false,0f);
                Camera_Shake.Instance.ShakeCamera(other.gameObject.GetComponent<Asteroid_info>().size*1.5f,1f);
                canTakeDamage = false;
                List<string> data = new List<string>();
                data.Add(other.gameObject.GetComponent<Asteroid_info>().size.ToString());
                EventBroadcaster.call_Event_Crushed_On_Asteroid(data);
                StartCoroutine(takeDamage());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("bullets"))
        {
            SoundManager.Inst.Play("PickUp",false,0f);
            Drop bulletDrop = other.GetComponent<Drop>();
            int newBullets = bulletDrop.value;
//            Debug.Log("Got bullets: "+newBullets);
            _weapons.getNewBullets(bulletDrop.dropID,newBullets);
            ObjectPooler.Instance.enqueueGameObject(other.gameObject,other.GetComponent<Drop>().tag,true);
        }else if (other.CompareTag("Health Drop"))
        {
            SoundManager.Inst.Play("PickUp",false,0f);
            GetComponent<Spaceship_Info>().increaseHealth(other.GetComponent<Drop>().value);
            ObjectPooler.Instance.enqueueGameObject(other.gameObject,"HealthDrop",true);
        }
    }

    private IEnumerator takeDamage()
    {
        //skin.GetComponent<SpriteRenderer>().material = damageMaterial;
        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }
}
