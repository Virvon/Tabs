using System.Collections.Generic;
using UnityEngine;

namespace Sources.UI
{
    public class ListView<T> : MonoBehaviour where T : Component
    {
        [SerializeField] private T _prefab;
        [SerializeField] private Transform _container;

        private readonly List<T> _items = new();
        private readonly Queue<T> _freeList = new();

        public T SpawnItem()
        {
            if (_freeList.TryDequeue(out T item))
            {
                item.gameObject.SetActive(true);
            }
            else
            {
                item = Instantiate(_prefab, _container);
            }
            
            _items.Add(item);

            return item;
        }

        public void UnspawnItem(T item)
        {
            if (item != null && _items.Remove(item))
            {
                item.gameObject.SetActive((false));
                _freeList.Enqueue(item);
            }
        }

        public void Clear()
        {
            for (int i = 0, count = _items.Count; i < count; i++)
            {
                T item = _items[i];
                item.gameObject.SetActive(false);
                _freeList.Enqueue(item);
            }
            
            _items.Clear();
        }
    }
}