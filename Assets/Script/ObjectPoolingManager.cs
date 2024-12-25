using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    public static ObjectPoolingManager Inst;
    
    public Dictionary<Projectile, Queue<Projectile>> pools = new Dictionary<Projectile, Queue<Projectile>>();
    
    [SerializeField] private List<Projectile> prefabs;
    [SerializeField] private int basicPoolCount = 10;

    private Transform _parent;

    private void Awake()
    {
        if (Inst != this && Inst != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Inst = this;
        }
      
    }
    // Start is called before the first frame update
    void Start()
    {
        GameStart();
    }

    private void GameStart()
    {
        _parent = new GameObject("PoolingParent").transform;     //새로운 빈 오브젝트를 하나 만들어줌
        _parent.position = Vector3.zero;                 //새로운 빈 오브젝트의 위치 설정
        _parent.SetParent(transform, true);
        
        for (int i = 0; i < prefabs.Count; i++)
        {
            Queue<Projectile> _queue = new Queue<Projectile>();
            for (int j = 0; j < basicPoolCount; j++)
            {
                SpawnObj(prefabs[i],_queue);
            }
            pools.Add(prefabs[i],_queue);
        }
        
    }

    private void SpawnObj(Projectile prefab,Queue<Projectile> queue)
    {
        var _prefab = Instantiate(prefab,_parent);
        _prefab.gameObject.SetActive(false);
        queue.Enqueue(_prefab);
    }
    
    public Projectile GetObjectFromPool(string prefabName)
    {
        var dict =pools.FirstOrDefault(p => p.Key.name == prefabName);
        var _prefab = dict.Key;
        var _queue = dict.Value;
        Debug.Log(_queue.Count);
        if (_queue.Count == 0)
        {
            // 풀에 오브젝트가 없으면 새로 생성
            SpawnObj(_prefab,_queue);
        }

        var obj = _queue.Dequeue();
        obj.gameObject.SetActive(true); // 활성화
        return obj;
    }
    
    public void ReturnObjectToPool(Projectile obj)
    {
        var dict =pools.FirstOrDefault(p => p.Key.name == obj.GetType().Name);
        var _queue = dict.Value;
        obj.gameObject.SetActive(false); // 비활성화
        _queue.Enqueue(obj); // 풀에 다시 추가
    }

   

    
}
