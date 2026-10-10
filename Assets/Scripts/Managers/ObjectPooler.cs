using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{

    #region Singleton_region

    public static ObjectPooler Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion
    
    
    
    [System.Serializable]
    public class pool
    {
        public string tag;
        public GameObject prefab;
        public int spawnAmount;
    }

    [SerializeField] private GameObject poolCollection;
    public List<pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (var p in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < p.spawnAmount; i++)
            {
                GameObject tmp = Instantiate(p.prefab,poolCollection.transform);
                tmp.SetActive(false);
                objectPool.Enqueue(tmp);
            }
            poolDictionary.Add(p.tag,objectPool);
        }
    }

    public GameObject spawnFromPool(string tag, Vector3 pos, Quaternion rot)
    {
        if (poolDictionary.ContainsKey(tag))
        {
            if (poolDictionary[tag].Count>0)
            {
//                Debug.Log("We have "+tag);
                GameObject tmp= poolDictionary[tag].Dequeue();
                tmp.SetActive(true);
                tmp.transform.position = pos;
                tmp.transform.rotation = rot;
                if (tag=="bulletCol")
                {
                    poolDictionary[tag].Enqueue(tmp);
                }
                return tmp;
            }
            else
            {
                Debug.Log("Ran out of: "+tag);
                return null;
            }
        }
        else
        {
            Debug.Log("Wrong Tag "+ tag);
            return null;
        }
    }

    public void enqueueGameObject(GameObject tmp, string tag,bool hide)
    {
        poolDictionary[tag].Enqueue(tmp);
        if (hide)
        {
            tmp.SetActive(false);
        }
    }
}
