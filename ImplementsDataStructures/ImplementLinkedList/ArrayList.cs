namespace ImplementDataStructures
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ArrayList<T> : System.Collections.Generic.IList<T>
    {
        T[] _items;

        public ArrayList()
            : this(0)
        {
        }

        public ArrayList(int length)
        {
            if (length < 0)
            {
                throw new ArgumentException("length");
            }

            _items = new T[length];
        }

        /// <summary>
        /// 1. Allocate a lager array.
        /// 2. Copy elements from the smaller to the larger array.
        /// 3. Update the internal array to be the larger array.
        /// O(n)
        /// allocate memory algoritm 
        /// c#: size=size==0 ?1:size*2;
        /// java:size=(size*3)/2+1;
        /// </summary>
        /// <param name="item"></param>
        private void GrowArray()
        {
            int newLength = _items.Length == 0 ? 16 : _items.Length << 1;
            T[] newArray = new T[newLength];
            _items.CopyTo(newArray, 0);
            _items = newArray;
        }

        /// <summary>
        /// O(1)
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            if (_items.Length == Count)
            {
                GrowArray();
            }
            _items[Count++] = item;
        }
        
        /// <summary>
        /// O(n)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            return IndexOf(item) != -1;
        }
        
        /// <summary>
        /// O(n)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(T item)
        {
            for (int i = 0; i < Count; i++)
            {
                if (_items[i].Equals(item))
                {
                    RemoveAt(i);
                    return true;
                }
            }
            return false;
        }        
       
        /// <summary>
        /// O(1)
        /// </summary>
        public void Clear()
        {
            _items = new T[0];
        }

        /// <summary>
        /// O(n)
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(_items, 0, array, arrayIndex, Count);
        }

        /// <summary>
        /// O(1)
        /// </summary>
        public int Count
        {
            get;
            private set;
        }

        /// <summary>
        /// O(1)
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        #region IList<T> Methods

        /// <summary>
        /// O(n)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(T item)
        {
            for (int i = 0; i < Count; i++)
            {
                if (_items[i].Equals(item))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        ///O(n)
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, T item)
        {
            if (index >= Count)
            {
                throw new IndexOutOfRangeException();
            }
            if (_items.Length == this.Count)
            {
                this.GrowArray();
            }

            //Shift all teh items following index one slot to the right.
            Array.Copy(_items, index, _items, index + 1, Count - index);

            _items[index] = item;
            Count++;
        }

        /// <summary>
        /// O(n)
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            if (index >= Count)
            {
                throw new IndexOutOfRangeException();
            }

            int shiftStart = index + 1;
            if (shiftStart < Count)
            {
                //Shift all the items following index one slot to the left.
                Array.Copy(_items, shiftStart, _items, index, Count - shiftStart);
            }

            Count--;
        }

        /// <summary>
        /// O(1)
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index]
        {
            get
            {
                if (index < Count)
                {
                    return _items[index];
                }
                throw new IndexOutOfRangeException();
            }
            set
            {
                if (index < Count)
                {
                    _items[index] = value;
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        #endregion

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return _items[i];
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
