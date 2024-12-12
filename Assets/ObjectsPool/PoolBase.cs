using UnityEngine;
using UnityEngine.Pool;

namespace ObjectsPool
{
    public abstract class PoolBase<T> : MonoBehaviour where T : Component
    {
        public static PoolBase<T> Instance { get; private set; }

        /// <summary>
        /// Pool default object capacity.
        /// </summary>
        [field: SerializeField]
        public int DefaultCapacity { get; set; } = 10;

        /// <summary>
        /// Max pool object capacity.
        /// </summary>
        [field: SerializeField]
        public int MaxCapacity { get; set; } = 10000;

        /// <summary>
        /// Number of preloaded objects.
        /// </summary>
        [field: SerializeField]
        public int PreloadedAmount { get; set; } = 100;

        [SerializeField]
        protected T objectPrefab;

        protected ObjectPool<T> pool;

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }

            pool = new ObjectPool<T>(CreateObject, GetObject, ReleaseObject,
                defaultCapacity: DefaultCapacity,
                maxSize: MaxCapacity);
        }

        void Start()
        {
            PreloadObjects();
        }

        public T Get()
        {
            return pool.Get();
        }

        public void Release(T t)
        {
            pool.Release(t);
        }

        private void PreloadObjects()
        {
            for (int i = 0; i < PreloadedAmount; i++)
            {
                var obj = pool.Get();
                pool.Release(obj);
            }
        }

        protected abstract T CreateObject();

        protected abstract void GetObject(T t);

        protected abstract void ReleaseObject(T t);
    }
}
