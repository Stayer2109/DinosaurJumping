using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class PooledObject : MonoBehaviour
{
    public ObstacleType type;

    public ObjectPool<GameObject> originalPool;
}
