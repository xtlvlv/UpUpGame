using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    
/// <summary>
/// 用栈实现的对象池
/// 
/// 用法
/// SimplePool.Spawn(obj, pos, rot);
/// SimplePool.Despawn(obj)
/// </summary>
public static class SimplePool
{
    private static Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();

    // 默认大小
    const int DEFAULT_POOL_SIZE = 3;

    /// <summary>
    /// The Pool class represents the pool for a particular prefab.
    /// Pool类表示特定预制件的池。
    /// </summary>
    class Pool
    {
        /// <summary>
        /// 给对象名加个id，区别池中对象
        /// </summary>
        int nextId = 1;

        /// <summary>
        /// 不活跃的对象，即在池子中待使用的对象
        /// </summary>
        Stack<GameObject> inactiveStack;
        
        /// <summary>
        /// 不活跃的对象，即在池子中待使用的对象
        /// </summary>
        Queue<GameObject> inactiveQueue;

        // The prefab that we are pooling
        //我们正在使用的预制件
        GameObject prefab;

        /// <summary>
        ///  构造函数
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="quantity">栈初始化数量</param>
        public Pool(GameObject prefab, int quantity)
        {
            this.prefab = prefab;
            inactiveStack = new Stack<GameObject>(quantity);
            inactiveQueue = new Queue<GameObject>(quantity);
        }

        // Spawn an object from our pool
        //从我们的池中生成一个对象
        public GameObject Spawn(Vector3 pos, Quaternion rot)
        {
            GameObject obj;
            if (inactiveQueue.Count == 0)
            {
                //没有对象的时候实例化一个全新的对象。
                obj = (GameObject)GameObject.Instantiate(prefab, pos, rot);
                obj.name = prefab.name + " (" + (nextId++) + ")";

                // 添加一个PoolMember组件，以便我们知道我们属于。哪个池
                obj.AddComponent<PoolMember>().myPool = this;
            }
            else
            {
                obj = inactiveQueue.Dequeue();
                if (obj == null)
                {
                    //我们期望找到的非活动对象不再存在。
                    //最可能的原因是：
                    //  - 有人在我们的对象上调用Destroy（）
                    //  - 场景变化（会破坏我们所有的物体）。
                    //注意：使用DontDestroyOnLoad可以防止这种情况
                    //如果你真的不想要这个
                    //不用担心 - 我们只是按顺序尝试下一个。
                    return Spawn(pos, rot);
                }
            }

            obj.transform.position = pos;
            obj.transform.rotation = rot;
            obj.SetActive(true);
            return obj;
        }
        
        public GameObject Spawn(Transform transform)
        {
            GameObject obj;
            if (inactiveQueue.Count == 0)
            {
                //没有对象的时候实例化一个全新的对象。
                obj = (GameObject)GameObject.Instantiate(prefab, transform);
                obj.name = prefab.name + " (" + (nextId++) + ")";

                // 添加一个PoolMember组件，以便我们知道我们属于。哪个池
                obj.AddComponent<PoolMember>().myPool = this;
            }
            else
            {
                obj = inactiveQueue.Dequeue();
                if (obj == null)
                {
                    return Spawn(transform);
                }
            }

            obj.SetActive(true);
            return obj;
        }

        public GameObject Spawn()
        {
            GameObject obj;

            if (inactiveQueue.Count == 0)
            {
                obj = UnityEngine.Object.Instantiate(prefab);
                obj.name = prefab.name + "(" + (nextId++) + ")";

                obj.AddComponent<PoolMember>().myPool = this;
            }
            else
            {
                obj = inactiveQueue.Dequeue();
                if (obj == null)
                {
                    return Spawn();
                }
            }
            obj.SetActive(true);
            return obj;
        }
        
        public GameObject SpawnOnly()
        {
            GameObject obj;
            if (nextId == 1) // 生成第一个
            {
                obj = (GameObject)GameObject.Instantiate(prefab);
                obj.name = prefab.name + " (" + (nextId++) + ")";

                obj.AddComponent<PoolMember>().myPool = this;
            }
            else if (inactiveQueue.Count == 0)
            {
                Log.Warning("只能生成一个物体");
                return null;
            }
            else
            {
                obj = inactiveQueue.Dequeue();
                if (obj == null)
                {
                    return Spawn();
                }
            }
            obj.SetActive(true);
            return obj;
        }

        /// <summary>
        /// Return an object to the inactive pool.
        /// 将对象返回到非活动池。
        /// </summary>
        /// <param name="obj"></param>
        public void Despawn(GameObject obj)
        {
            obj.SetActive(false);
            inactiveQueue.Enqueue(obj);
        }
    }

    /// <summary>
    /// Added to freshly instantiated objects, so we can link back
    /// to the correct pool on despawn.
    /// </summary>
    class PoolMember : MonoBehaviour
    {
        public Pool myPool;
    }

    // All of our pools
    // 我们所有的对象池
    static Dictionary<GameObject, Pool> pools;

    /// <summary>
    /// Init our dictionary.
    /// </summary>
    static void Init(GameObject prefab = null, int qty = DEFAULT_POOL_SIZE)
    {
        if (pools == null)
        {
            pools = new Dictionary<GameObject, Pool>();
        }
        if (prefab != null && pools.ContainsKey(prefab) == false)
        {
            pools[prefab] = new Pool(prefab, qty);
        }
    }

    /// <summary>
    /// 如果要在开始时预加载一些对象的副本场景，你可以使用它。 
    /// 真的不需要，除非你是很快就会从零实例变为10+。
    /// 技术上可以更多地优化，但在实践中
    /// Spawn / Despawn序列非常快速
    /// 这可以避免代码重复。
    /// </summary>
    public static void Preload(GameObject prefab, int qty = 1)
    {
        Init(prefab, qty);

        // Make an array to grab the objects we're about to pre-spawn.
        //创建一个数组来存储我们即将产生的对象。
        GameObject[] obs = new GameObject[qty];
        for (int i = 0; i < qty; i++)
        {
            obs[i] = Spawn(prefab, Vector3.zero, Quaternion.identity);
        }

        // Now despawn them all.
        for (int i = 0; i < qty; i++)
        {
            Despawn(obs[i]);
        }
    }

    /// <summary>
    /// Spawns a copy of the specified prefab (instantiating one if required).
    /// NOTE: Remember that Awake() or Start() will only run on the very first
    /// spawn and that member variables won't get reset.  OnEnable will run
    /// after spawning -- but remember that toggling IsActive will also
    /// call that function.
    /// 注意：请记住，Awake（）或Start（）只会在第一个spawn上运行并且成员变量不会被重置。
    /// OnEnable将在生成后运行
    /// 但请记住切换IsActive也会调用该函数（OnEnable）。
    /// </summary>
    private static GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        Init(prefab);

        return pools[prefab].Spawn(pos, rot);
    }
    
    /// <summary>
    /// 生成物体挂载到transform上，注意，如果不是第一次生成，挂载不生效
    /// </summary>
    /// <param name="prefab">预制体</param>
    /// <param name="transform">父物体</param>
    /// <returns></returns>
    public static GameObject Spawn(GameObject prefab, Transform transform)
    {
        Init(prefab);

        return pools[prefab].Spawn(transform);
    }

    public static GameObject Spawn(GameObject prefab)
    {
        Init(prefab);

        return pools[prefab].Spawn();
    }

    public static GameObject Spawn(string a_prefabPath)
    {
        if (_prefabCache.TryGetValue(a_prefabPath, out var l_prefab))
        {
            return SimplePool.Spawn(l_prefab);
        }
        else
        {
            l_prefab = Resources.Load<GameObject>(a_prefabPath);
            _prefabCache.Add(a_prefabPath, l_prefab);
            return SimplePool.Spawn(l_prefab);
        }
    }
    
    public static GameObject SpawnByObj(GameObject a_prefabPath)
    {
        if (_prefabCache.TryGetValue(a_prefabPath.name, out var l_prefab))
        {
            return SimplePool.Spawn(l_prefab);
        }
        else
        {
            _prefabCache.Add(a_prefabPath.name, a_prefabPath);
            return SimplePool.Spawn(a_prefabPath);
        }
    }

    
    /// <summary>
    /// 只生成一个，如果已存在就不再生成
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    public static GameObject SpawnOnly(GameObject prefab)
    {
        Init(prefab);

        return pools[prefab].SpawnOnly();
    }

    /// <summary>
    /// Despawn the specified gameobject back into its pool.
    /// 将指定的游戏对象回收到池中。
    /// </summary>
    public static void Despawn(GameObject obj)
    {
        PoolMember pm = obj.GetComponent<PoolMember>();
        if (pm == null)
        {
            Debug.Log("Object '" + obj.name + "' wasn't spawned from a pool. Destroying it instead.");
            GameObject.Destroy(obj);
        }
        else
        {
            pm.myPool.Despawn(obj);
        }
    }
}

}
