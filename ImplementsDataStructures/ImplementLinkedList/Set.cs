namespace ImplementDataStructures
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Set<T>:IEnumerable<T>
        where T:IComparable<T>
    {
        private readonly List<T> _items = new List<T>();

        public Set()
        { 
        }

        public Set(IEnumerable<T> items)
        {
            AddRange(items);
        }

        /// <summary>
        /// O(n)
        /// If the item already exists in set InvalidOperationException is thrown.
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            if (Contains(item))
            {
                throw new InvalidOperationException("Item already exists in Set");
            }
            _items.Add(item);
        }
        /// <summary>
        /// O(mn)
        /// duplicates items InvalidOperationException is thrown.
        /// </summary>
        /// <param name="items"></param>
        public void AddRange(IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                Add(item);
            }
        }

        /// <summary>
        /// O(n)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(T item) 
        {
            return _items.Remove(item);
        }

        /// <summary>
        /// O(n)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            return _items.Contains(item);
        }

        /// <summary>
        /// O(1)
        /// </summary>
        public int Count
        {
            get 
            {
                return _items.Count;
            }
        }

        /// <summary>
        /// O(mn), where m and n are the number of items in the provided and current sets, respectively
        /// [1, 2, 3, 4] union [3, 4, 5, 6] = [1, 2, 3, 4, 5, 6] 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Set<T> Union(Set<T> other)
        {
            Set<T> result = new Set<T>(_items);

            foreach (T item in other._items)
            {
                if (!Contains(item))
                {
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// O(mn), where m and n are the number of items in the provided and current sets, respectively. 
        /// [1, 2, 3, 4] intersect [3, 4, 5, 6] = [3, 4] 
        /// </summary>
        /// <param name="other"></param>        
        /// <returns></returns>
        public Set<T> Intersection(Set<T> other)
        {
            Set<T> result = new Set<T>();

            foreach (T item in _items)
            {
                if (other._items.Contains(item))
                {
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// O(mn), where m and n are the number of items in the provided and current sets, respectively. 
        /// [1, 2, 3, 4] difference [3, 4, 5, 6] = [1, 2] 
        /// </summary>        
        /// <param name="other"></param>
        /// <returns></returns>
        public Set<T> Difference(Set<T> other)
        {
            Set<T> result = new Set<T>(_items);

            foreach (T item in other._items)
            {
                result.Remove(item);
            }

            return result;
        }

        /// <summary>
        /// O(mn), where m and n are the number of items in the provided and current sets, respectively. 
        /// [1, 2, 3, 4] symmetric difference [3, 4, 5, 6] = [1, 2, 5, 6] 
        /// Step by step, it looks like this: 
        ///[1, 2, 3, 4] union [3, 4, 5, 6] = [1, 2, 3, 4, 5, 6] 
        ///[1, 2, 3, 4] intersection [3, 4, 5, 6] = [3, 4] 
        ///[1, 2, 3, 4, 5, 6] set difference [3, 4] = [1, 2, 5, 6] 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Set<T> SymmetricDifference(Set<T> other)
        {
            Set<T> union = Union(other);
            Set<T> intersection = Intersection(other);

            return union.Difference(intersection);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }
}
