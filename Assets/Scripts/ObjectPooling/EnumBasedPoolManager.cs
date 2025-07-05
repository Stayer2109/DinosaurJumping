using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnumBasedPoolManager : MonoBehaviour
{
    public static EnumBasedPoolManager Instance;

    [System.Serializable]
    public class ObstacleEntry
    {
        public ObstacleType type;
        public List<GameObject> prefabs;
    }

    [SerializeField]
    private List<ObstacleEntry> obstacleEntries;

    private Dictionary<ObstacleType, List<ObjectPool<GameObject>>> poolGroups;
    private Dictionary<ObstacleType, List<GameObject>> prefabGroups;
    private List<ObstacleType> validTypes; // ✅ only types with a prefab

    void Awake()
    {
        Instance = this;
        prefabGroups = new Dictionary<ObstacleType, List<GameObject>>();
        poolGroups = new Dictionary<ObstacleType, List<ObjectPool<GameObject>>>();
        validTypes = new List<ObstacleType>();

        // Cache all enum values (AOT-safe)
        // allTypes = (ObstacleType[])System.Enum.GetValues(typeof(ObstacleType));
        validTypes = new List<ObstacleType>();

        foreach (var entry in obstacleEntries)
        {
            if (entry.prefabs == null || entry.prefabs.Count == 0) continue;

            validTypes.Add(entry.type);
            prefabGroups[entry.type] = new List<GameObject>();
            poolGroups[entry.type] = new List<ObjectPool<GameObject>>();

            foreach (var prefab in entry.prefabs)
            {
                if (prefab == null) continue;

                prefabGroups[entry.type].Add(prefab);

                // 👇 This part right here!
                ObjectPool<GameObject> pool = null; // Temporary so we can reference it in lambda

                pool = new ObjectPool<GameObject>(
                    () =>
                    {
                        GameObject obj = Instantiate(prefab);
                        var pooled = obj.GetComponent<PooledObject>();
                        if (pooled == null) pooled = obj.AddComponent<PooledObject>();
                        pooled.type = entry.type;
                        pooled.originalPool = pool; // ✅ Save the pool reference here!
                        return obj;
                    },
                    obj => obj.SetActive(true),
                    obj => obj.SetActive(false),
                    obj => Destroy(obj),
                    true, 5, 20
                );

                poolGroups[entry.type].Add(pool);
            }
        }
    }

    public GameObject Spawn(ObstacleType type, Vector3 position)
    {
        if (!poolGroups.ContainsKey(type) || poolGroups[type].Count == 0)
        {
            Debug.LogWarning($"❌ No prefab pools available for type {type}");
            return null;
        }

        int randIndex = Random.Range(0, poolGroups[type].Count);
        var obj = poolGroups[type][randIndex].Get();
        obj.transform.position = position;
        return obj;
    }

    public GameObject SpawnRandom(Vector3 position)
    {
        if (validTypes.Count == 0)
        {
            Debug.LogWarning("❌ No valid obstacle types with prefabs.");
            return null;
        }

        ObstacleType randomType = validTypes[Random.Range(0, validTypes.Count)];
        return Spawn(randomType, position);
    }

    public void Despawn(GameObject obj)
    {
        var pooled = obj.GetComponent<PooledObject>();
        if (pooled != null && pooled.originalPool != null)
        {
            pooled.originalPool.Release(obj);
        }
        else
        {
            Debug.LogWarning($"⚠️ Attempted to despawn object without a valid origin pool: {obj.name}");
            Destroy(obj); // fallback
        }
    }

    public void AddPrefabToPool(ObstacleType type, GameObject newPrefab)
    {
        if (newPrefab == null) return;

        // If the group doesn’t exist yet
        if (!poolGroups.ContainsKey(type))
        {
            poolGroups[type] = new List<ObjectPool<GameObject>>();
            prefabGroups[type] = new List<GameObject>();
            validTypes.Add(type);
        }

        prefabGroups[type].Add(newPrefab);

        ObjectPool<GameObject> newPool = null;

        newPool = new ObjectPool<GameObject>(
            () =>
            {
                GameObject obj = Instantiate(newPrefab);
                var pooled = obj.GetComponent<PooledObject>();
                if (pooled == null) pooled = obj.AddComponent<PooledObject>();
                pooled.type = type;
                pooled.originalPool = newPool;
                return obj;
            },
            obj => obj.SetActive(true),
            obj => obj.SetActive(false),
            obj => Destroy(obj),
            true, 5, 20
        );

        poolGroups[type].Add(newPool);
    }
}
