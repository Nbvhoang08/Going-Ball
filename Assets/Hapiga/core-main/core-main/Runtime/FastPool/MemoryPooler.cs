using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hapiga.Core.Runtime.Pool
{
    public class MemoryPool<T> where T : IPoolable, new()
    {
        private Stack<T> _items;
        private object _sync;

        public MemoryPool(int maxCount)
        {
            _items = new Stack<T>(maxCount);
            for (int i = 0; i < maxCount; i++)
            {
                _items.Push(new T());
            }

            _sync = new object();
        }

        public T Get()
        {
            lock (_sync)
            {
                //if(typeof(T) == typeof(Hint))
                //{ Debug.Log("Pool count Get: " + _items.Count);}
                if (_items.Count == 0)
                {
                    return new T();
                }

                return _items.Pop();
            }
        }


        public void Free(T item)
        {
            lock (_sync)
            {
                item.Reset();
                _items.Push(item);
                //if (typeof(T) == typeof(Hint))
                //{ Debug.Log("Pool count Free: " + _items.Count);}
            }
        }
    }

    public interface IPoolable
    {
        void Reset();
    }
}