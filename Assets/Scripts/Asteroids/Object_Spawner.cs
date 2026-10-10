using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Object_Spawner : MonoBehaviour
{
    private GameManager _gameManager;
    [SerializeField] private float distance;
    [SerializeField] private Transform Parent;

    [Header("Spawning_Timers")]
    [SerializeField] private float enemyTimer;
    [SerializeField] private float enemyCooldown;
    [SerializeField]private bool haveToSpawnEnemy;
    
    [SerializeField] private float ammoTimer;
    [SerializeField] private float ammoCooldown;
    [SerializeField]private bool haveToSpawnAmmo;
    private void Awake()
    {
        _gameManager=GameManager.Instance;
        Parent = transform.parent;

        haveToSpawnAmmo = false;
        haveToSpawnEnemy = false;
    }

    private void Update()
    {
        ammoTimer += Time.deltaTime;
        enemyTimer += Time.deltaTime;
        if (ammoTimer>ammoCooldown)
        {
            haveToSpawnAmmo = true;
        }else if (enemyTimer>enemyCooldown)
        {
            haveToSpawnEnemy = true;
        }
    }

    public void SpawnNewAsteroids(int max)
    {
        for (int i = 0; i < max; i++)
        {

            //Object to Spawn
            int asterId = Random.Range(1, 6);
            String asteroidTag = "asteroid" + asterId;
            if (asterId==2 || asterId==4)
            {
                asteroidTag +="_"+Random.Range(1,3);
            }

            float yCord = Random.Range(distance, distance + 10);
            GameObject tmp=ObjectPooler.Instance.spawnFromPool(asteroidTag, new Vector3(transform.position.x, transform.position.y+yCord, 0), transform.rotation);
            
            if (tmp) //if we didn't get NULL
            {
                /*
                 1)Make this object (transform) parent of the newly SpawnedObject
                 2)Rotate this object (transform)
                 3)Make spawned object's parent= null;
                 */
                tmp.transform.parent = transform;
                transform.rotation = rotateObject();
                tmp.transform.parent = null;
                tmp.GetComponent<Asteroid_info>().spawner = gameObject;
                tmp.GetComponent<Asteroid_info>().Player=Parent;
                tmp.GetComponent<Asteroid_info>().resetAsteroid();

            }

            if (haveToSpawnAmmo)
            {
                haveToSpawnAmmo = false;
                ammoTimer = 0;
                if (Random.Range(1,20)<_gameManager.difficultiesList[_gameManager.difficultyLevel].ammoDropChance)
                {
                    Debug.Log("Spawning Drops");
                    GameObject dropSpawned;
                    if (Random.Range(1,5)==2)
                    {
                        dropSpawned=ObjectPooler.Instance.spawnFromPool("bullets1", new Vector3(transform.position.x+Random.Range(-5, 5), transform.position.y+yCord, 0), transform.rotation);
                    }
                    else
                    {                    
                        dropSpawned=ObjectPooler.Instance.spawnFromPool("bullets0", new Vector3(transform.position.x+Random.Range(-5, 5), transform.position.y+yCord, 0), transform.rotation);
                    }
                    if (dropSpawned)
                    {
                        dropSpawned.transform.parent = transform;
                        transform.rotation = rotateObject();
                        dropSpawned.transform.parent = null;
                        dropSpawned.GetComponent<Drop>().Player = Parent;
                    }
                }
            }

            if (haveToSpawnEnemy)
            {
                if (_gameManager.difficultyLevel>3)
                {
                    haveToSpawnEnemy = false;
                    enemyTimer = 0;
                    if (Random.Range(0,20)<5)
                    {
                        GameObject enemy=ObjectPooler.Instance.spawnFromPool("Enemy", new Vector3(transform.position.x+Random.Range(-5, 5), transform.position.y+yCord, 0), transform.rotation);
                        if (enemy)
                        {
                            enemy.transform.parent = transform;
                            transform.rotation = rotateObject();
                            enemy.transform.parent = null;
                            enemy.GetComponent<Enemy_Info>().resetSpaceship();
                        }
                    }
                }
            }

        }
    }

    private Quaternion rotateObject()
    {
        Vector3 Angle =new Vector3(0, 0,Random.Range(0, 360));
        Quaternion currentRot=new Quaternion();
        currentRot.eulerAngles= Angle;
        return currentRot;
    }

    public void DestroyAsteroid(GameObject objectToDestroy, int oldObject) //5= huge, 4= broken 5, 3 middle size, 2 small, 1 smallest
    {
        List<GameObject> lastObjects = new List<GameObject>();
        SpawnDrops(oldObject,objectToDestroy.transform.position,objectToDestroy.transform.rotation);
        ObjectPooler.Instance.enqueueGameObject(objectToDestroy,objectToDestroy.GetComponent<Asteroid_info>().tag,true);
        if (oldObject==5)
        {
            lastObjects.Add(ObjectPooler.Instance.spawnFromPool("asteroid4_1",objectToDestroy.transform.GetChild(0).position, transform.rotation));
            lastObjects.Add(ObjectPooler.Instance.spawnFromPool("asteroid4_2",objectToDestroy.transform.GetChild(1).position, transform.rotation));
        }
        if (oldObject == 4)
        {
            lastObjects.Add(ObjectPooler.Instance.spawnFromPool("asteroid1", objectToDestroy.transform.GetChild(0).position, transform.rotation));
            lastObjects.Add(ObjectPooler.Instance.spawnFromPool("asteroid1", objectToDestroy.transform.GetChild(1).position, transform.rotation));
        }
        if (oldObject==3)
        {
            
            lastObjects.Add(ObjectPooler.Instance.spawnFromPool("asteroid1", objectToDestroy.transform.GetChild(1).position, transform.rotation));
            lastObjects.Add(ObjectPooler.Instance.spawnFromPool("asteroid1", objectToDestroy.transform.GetChild(2).position, transform.rotation));
            lastObjects.Add(ObjectPooler.Instance.spawnFromPool("asteroid1", objectToDestroy.transform.GetChild(0).position, transform.rotation));            
        }
        if (oldObject==2)
        {
            lastObjects.Add(ObjectPooler.Instance.spawnFromPool("asteroid1", objectToDestroy.transform.GetChild(1).position, transform.rotation));
            lastObjects.Add(ObjectPooler.Instance.spawnFromPool("asteroid1", objectToDestroy.transform.GetChild(0).position, transform.rotation));
        }

        for (int i = 0; i < lastObjects.Count; i++)
        {
            if (lastObjects[i])
            {            
                Asteroid_info tmpScript=lastObjects[i].GetComponent<Asteroid_info>();
                tmpScript.spawner = gameObject;
                tmpScript.Player=Parent;
                tmpScript.resetAsteroid();
            }
        }
    }

    private void SpawnDrops(int Ast, Vector3 pos, Quaternion angle)
    {
        int rand = Random.Range(0, 10);
        if (rand<Ast && rand>1)
        {
            GameObject tmp=ObjectPooler.Instance.spawnFromPool("HealthDrop", pos, angle);
            if (tmp)
            {
                tmp.GetComponent<Drop>().Player = Parent;
            }
        }

        rand = Random.Range(0, 100);
        if (rand<65 && rand>55)
        {
            if (Random.Range(0,3)==1)
            {
                GameObject tmp=ObjectPooler.Instance.spawnFromPool("bullets1", pos, angle);
                if (tmp)
                {
                    tmp.GetComponent<Drop>().Player = Parent;
                }
            }
            else
            {
                GameObject tmp=ObjectPooler.Instance.spawnFromPool("bullets0", pos, angle);
                if (tmp)
                {
                    tmp.GetComponent<Drop>().Player = Parent;
                }
            }
        }
    }
        
}
