namespace ImplementDataStructures
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class HashTable<TKey,TValue>
    {
        //If the array exceeds this fill percentage it will grow.
        const double _fillFactor = 0.75;

        //The maximum number of items to store before growing.
        //This is just a cached value of the fill factor calculation.
        int _maxItemsAtCurrentSize;

        //The number of items in the hash table.
        int _count;
        //The array where the items are stored.

        HashTableArray<TKey, TValue> _array;

        ///<summary>
        ///Constructs a hash table with the default capacity.
        ///</summary>
        public HashTable()
            : this(1000)
        { 
        }

        ///<summary>
        ///Constructs a hash table with the specified capacity.
        ///</summary>
        public HashTable(int initialCapacity)
        {
            if (initialCapacity < 1)
            {
                throw new ArgumentOutOfRangeException("initialCapacity");
            }

            _array = new HashTableArray<TKey, TValue>(initialCapacity);

            //When the count exceeds this value, the next Add will cause the
            //array to grow.
            _maxItemsAtCurrentSize = (int)(initialCapacity * _fillFactor) + 1;
        }

        /// <summary>
        /// O(1)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(TKey key, TValue value) 
        {
            if (_count >= _maxItemsAtCurrentSize)
            {
                //Allocate a larger array
                HashTableArray<TKey, TValue> largerArray = new HashTableArray<TKey, 
                    TValue>(_array.Capacity * 2);

                //and re-add each item to the new array.
                foreach (HashTableNodePair<TKey, TValue> node in _array.Items)
                {
                    largerArray.Add(node.Key, node.Value);
                }

                _array = largerArray;

                //Update the new max items cached value.
                _maxItemsAtCurrentSize = (int)(_array.Capacity * _fillFactor) + 1;
            }
            _array.Add(key, value);
            _count++;
        }

        public bool Remove(TKey key) 
        {
            bool removed = _array.Remove(key);
            if (removed)
            {
                _count--;
            }

            return removed;
        }

        public TValue this[TKey key] 
        {
            get 
            {
                TValue value;
                if (!_array.TryGetValue(key, out value))
                {
                    throw new ArgumentException("key");
                }
                return value;
            }
            set 
            {
                _array.Update(key, value);
            } 
        }

        public bool TryGetValue(TKey key, out TValue value) 
        {
            return _array.TryGetValue(key, out value);
        }

        public bool ContainsKey(TKey key) 
        {
            TValue value;
            return _array.TryGetValue(key, out value);
        }

        public bool ContainsValue(TValue value)
        {
            foreach (TValue foundValue in _array.Values)
            {
                if (value.Equals(foundValue))
                {
                    return true;
                }
            }

            return false;
        }

        public IEnumerable<TKey> Keys 
        {
            get 
            {
                foreach (TKey key in _array.Keys)
                {
                    yield return key;
                }
            }
        }

        public IEnumerable<TValue> Values 
        {
            get 
            {
                foreach (TValue value in _array.Values)
                {
                    yield return value;
                }
            }
        }

        public void Clear() 
        {
            _array.Clear();
            _count = 0;
        }

        public int Count 
        {
            get 
            {
                return _count;
            }
        }
    }

    ///<summary>
    ///The HashTableArray class is the backing array of the HashTable class. 
    ///It handles the jobs of finding the appropriate backing array index and 
    ///deferring to the HashTableArrayNode class.
    ///</summary>
    class HashTableArray<TKey, TValue>
    {
        HashTableArrayNode<TKey, TValue>[] _array;

        ///<summary>
        /// Constructs a new hash table array with the specified capacity. 
        ///</summary>
        public HashTableArray(int capacity)
        {
            _array = new HashTableArrayNode<TKey, TValue>[capacity];
        }

        /// <summary>
        /// O(1)
        /// Adds the key/value pair to the node. If the key already exists in the 
        /// node array, an ArgumentException will be thrown. 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(TKey key, TValue value)
        {
            int index = GetIndex(key);
            HashTableArrayNode<TKey, TValue> nodes = _array[index];
            if (nodes == null)
            {
                nodes = new HashTableArrayNode<TKey, TValue>();
                _array[index] = nodes;
            }

            nodes.Add(key, value);
        }
        /// <summary>
        /// O(n)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Update(TKey key, TValue value) 
        {
            HashTableArrayNode<TKey, TValue> nodes = _array[GetIndex(key)];
            if (nodes == null)
            {
                throw new ArgumentException("The key does not exist in the hash table", "key");
            }

            nodes.Update(key, value);
        }
        /// <summary>
        /// O(n)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(TKey key) 
        {
            HashTableArrayNode<TKey, TValue> nodes = _array[GetIndex(key)];
            if (nodes != null)
            {
                return nodes.Remove(key);
            }

            return false;
        }
        /// <summary>
        /// O(n)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            HashTableArrayNode<TKey, TValue> nodes = _array[GetIndex(key)];
            if (nodes != null)
            {
                return nodes.TryGetValue(key, out value);
            }

            value = default(TValue);
            return false;
        }
        /// <summary>
        /// O(1)
        /// </summary>
        public int Capacity 
        {
            get
            {
                return _array.Length;
            }
        }
        /// <summary>
        /// O(n)
        /// </summary>
        public void Clear() 
        {
            foreach (HashTableArrayNode<TKey, TValue> node in _array.Where(node => node != null))
            {
                node.Clear();
            }
        }
        /// <summary>
        /// O(n)
        /// </summary>
        public IEnumerable<TValue> Values 
        {
            get 
            {
                foreach (HashTableArrayNode<TKey, TValue> node in 
                    _array.Where(node => node != null))
                {
                    foreach (TValue value in node.Values)
                    {
                        yield return value;
                    }
                }
            }
        }
        /// <summary>
        /// O(n)
        /// </summary>
        public IEnumerable<TKey> Keys
        {
            get
            {
                foreach (HashTableArrayNode<TKey, TValue> node in
                    _array.Where(node => node != null))
                {
                    foreach (TKey key in node.Keys)
                    {
                        yield return key;
                    }
                }
            }
        }
        /// <summary>
        /// O(n)
        /// </summary>
        public IEnumerable<HashTableNodePair<TKey, TValue>> Items 
        {
            get 
            {
                foreach (HashTableArrayNode<TKey, TValue> node in
                    _array.Where(node => node != null))
                {
                    foreach (HashTableNodePair<TKey, TValue> pair in node.Items)
                    {
                        yield return pair;
                    }
                }
            }
        }
        /// <summary>
        /// O(1)
        /// Maps a key to the array index based on the hash code.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>

        private int GetIndex(TKey key) 
        {
            return Math.Abs(key.GetHashCode() % Capacity);
        }
    }

   

    internal class HashTableArrayNode<TKey, TValue>
    {
        //This list contains the actuak data in the hash table. It chains together
        //data collisions.
        System.Collections.Generic.LinkedList<HashTableNodePair<TKey, TValue>> _items;
        
        /// <summary>
        /// O(1)
        /// Adds the key/value pair to the node. If the key already exists in the 
        /// list, an ArgumentException will be thrown.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(TKey key, TValue value)
        {
            //Lazy init the linked list.
            if (_items == null)
            {
                _items = new System.Collections.Generic.LinkedList<HashTableNodePair<TKey, TValue>>();
            }
            else
            {
                //Multiple items might collide and exist in this list, but each
                //key should only be in the list once.
                foreach (HashTableNodePair<TKey, TValue> pair in _items)
                {
                    if (pair.Key.Equals(key))
                    {
                        throw new ArgumentException("The collection already contains the key");
                    }

                    //If we made it this farm add the item.
                    _items.AddFirst(new HashTableNodePair<TKey,TValue>(key,value));
                }
            }
        }
        /// <summary>
        /// O(n)
        /// Updates the value of the existing key/value pair in the list.
        /// If the key does not exist in the list, an ArgumentException
        /// will be thrown.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Update(TKey key, TValue value) 
        {
            bool updated = false;

            if (_items != null)
            {
                //Check each item in the list for the specified key.
                foreach (HashTableNodePair<TKey, TValue> pair in _items)
                {
                    if (pair.Key.Equals(key))
                    {
                        //Update the value.
                        pair.Value = value;
                        updated = true;
                        break;
                    }
                }
            }

            if (!updated)
            {
                throw new ArgumentException("The collection already contains the key");
            }
        }
        
        /// <summary>
        ///  Finds and returns the value for the specified key. 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            value = default(TValue);

            bool found = false;

            if (_items != null)
            {
                foreach (HashTableNodePair<TKey, TValue> pair in _items)
                {
                    if (pair.Key.Equals(key))
                    {
                        value = pair.Value;
                        found = true;
                        break;
                    }
                }
            }

            return found;
        }

        /// <summary>
        /// Removes the item from the list whose key matches 
        /// the specified key. 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(TKey key)
        {
            bool removed = false;
            if (_items != null)
            {
                System.Collections.Generic.
                    LinkedListNode<HashTableNodePair<TKey, TValue>> current = _items.First;
                while (current != null)
                {
                    if (current.Value.Key.Equals(key))
                    {
                        _items.Remove(current);
                        removed = true;
                        break;
                    }

                    current = current.Next;
                }
            }

            return removed;
        }

        /// <summary>
        /// O(1)
        /// </summary>
        public void Clear() 
        {
            if (_items != null)
            {
                _items.Clear();
            }
        }
        /// <summary>
        /// O(1)
        /// </summary>
        public IEnumerable<TValue> Values 
        {
            get
            {
                if (_items != null)
                {
                    foreach (HashTableNodePair<TKey, TValue> node in _items)
                    {
                        yield return node.Value;
                    }
                }
            } 
        }

        /// <summary>
        /// O(1)
        /// </summary>
        public IEnumerable<TKey> Keys 
        {
            get 
            {
                if (_items != null)
                {
                    foreach (HashTableNodePair<TKey, TValue> node in _items)
                    {
                        yield return node.Key;
                    }
                }
            }
        }
        /// <summary>
        /// O(1)
        /// </summary>
        public IEnumerable<HashTableNodePair<TKey, TValue>> Items 
        {
            get
            {
                if (_items != null)
                {
                    foreach (HashTableNodePair<TKey, TValue> node in _items)
                    {
                        yield return node;
                    }
                }
            } 
        }
    }

    ///<summary>
    ///A node in the hash table array.
    ///</summary>
    ///The type of the key of the key/value pair.
    ///The type of the vlaue of the key/value pair.
    public class HashTableNodePair<TKey, TValue>
    {
        ///<summary>
        ///Constructs a key/value pair for storage in hash table.
        ///</summary>
        ///The key of the key/value pair.
        ///The value of the key/value pair.
        public HashTableNodePair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        ///<summary>
        /// The key cannot be changed because it would affect the indexing 
        /// in the hash table.
        ///</summary>
        public TKey Key { get; private set; }

        ///<summary>
        /// The value. 
        ///</summary>
        public TValue Value { get; set; }
    }   
}
