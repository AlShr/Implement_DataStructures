namespace ImplementDataStructures
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class SkipListNode<T>
    {
        ///<summary>
        ///Creates a new node with the specified value
        ///</summary>
        public SkipListNode(T value, int height)
        {
            Value = value;
            Next = new SkipListNode<T>[height];
        }

        ///<summary>
        ///The array of links. The number of items
        ///is the height of the links.
        ///</summary>
        public SkipListNode<T>[] Next
        {
            get;
            private set;
        }
        
        ///<summary>
        ///The contained value.
        ///</summary>
        public T Value
        {
            get;
            private set;
        }
    }

    public class SkipList<T> : ICollection<T>
        where T : IComparable<T>
    {
        //Used to determine the random height of the node links.
        private readonly Random _rand = new Random();

        //The non-data node which starts the list.
        private SkipListNode<T> _head;

        //There is always one level of depth(the base list).
        private int _levels = 1;

        //The number of items currently in the list.
        private int _count = 0;

        public SkipList() { }
        /// <summary>
        /// O(log n)
        /// </summary>
        /// <param name="value"></param>
        public void Add(T item)
        {
            //1. Pick random height for the node.
            //2. Allocate a node with the random height and a specific value.
            //3. Find the appropriate place to insert the node into the sorted list.
            //4. Insert the node
            int level = PickRandomLevel();
            SkipListNode<T> newNode = new SkipListNode<T>(item, level + 1);
            SkipListNode<T> current = _head;

            for (int i = _levels - 1; i > 0; i--)
            {
                while (current.Next[i] != null)
                {
                    if (current.Next[i].Value.CompareTo(item) > 0)
                    {
                        break;
                    }

                    current = current.Next[i];
                }

                if (i <= level)
                {
                    //Adding "c" to the list: a->b->d->e.
                    //Current is node b and current.Next[i] is d.

                    //1.Link the new node (c) to the existing node (d):
                    //c.Next = d
                    newNode.Next[i] = current.Next[i];

                    //Insert c into the list after b:
                    //b.Next = c
                    current.Next[i] = newNode;
                }
            }

            _count++;
        }

        private int PickRandomLevel()
        {
            int rand = _rand.Next();
            int level = 0;
            // We're using the bit mask of a random integer to determine if the max      
            // level should increase by one or not.      
            // Say the 8 LSBs of the int are 00101100. In that case, when the      
            // LSB is compared against 1, it tests to 0 and the while loop is never     
            // entered so the level stays the same. That should happen 1/2 of the time.     
            // Later, if the _levels field is set to 3 and the rand value is 01101111,     
            // the while loop will run 4 times and on the last iteration will     
            // run another 4 times, creating a node with a skip list height of 4. This should      
            // only happen 1/16 of the time.
            while ((rand & 1) == 1)
            {
                if (level == _levels)
                {
                    _levels++;
                    break;
                }
                rand >>= 1;
                level++;
            }
            return level;
        }

        /// <summary>
        /// O(log n)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            SkipListNode<T> cur = _head;
            for (int i = _levels - 1; i >= 0; i--)
            {
                while (cur.Next[i] != null)
                {
                    int cmp = cur.Next[i].Value.CompareTo(item);

                    if (cmp > 0)
                    {
                        //The value is too large, so go down one level
                        //and take smaller steps.
                        break;
                    }

                    if (cmp == 0)
                    {
                        //Found it !
                        return true;
                    }

                    cur = cur.Next[i];
                }
            }

            return false;
        }

        /// <summary>
        /// O(log n)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Remove(T item) 
        {
            SkipListNode<T> cur = _head;

            bool removed = false;

            //Walk down each level in the list (make big jumps)
            for (int level = _levels - 1; level >= 0; level--)
            {
                while (cur.Next[level] != null)
                {
                    //If we found our node
                    if (cur.Next[level].Value.CompareTo(item) == 0)
                    {
                        //remove the node
                        cur.Next[level] = cur.Next[level].Next[level];
                        removed = true;

                        //and go down to the next level (where
                        //we will find our node again if were
                        //not at the bottom level.
                        break;
                    }

                    //If we went too farm go down level.
                    if (cur.Next[level].Value.CompareTo(item) > 0)
                    {
                        break;
                    }

                    cur = cur.Next[level];
                }
                if (removed)
                {
                    _count--;
                }
            }

            return removed;
        }

        /// <summary>
        /// O(1)
        /// </summary>
        public void Clear() 
        {
            _head = new SkipListNode<T>(default(T), 32 + 1);
            _count = 0;
        }

        /// <summary>
        /// O(n)
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(T[] array, int arrayIndex) 
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            int offset = 0;
            foreach (T item in this)
            {
                array[arrayIndex + offset++] = item;
            }
        }
        /// <summary>
        /// O(1)
        /// </summary>
        public int Count 
        {
            get { return _count; }
        }

        /// <summary>
        /// O(1)
        /// </summary>
        public bool IsReadOnly 
        {
            get { return false; } 
        }

        public IEnumerator<T> GetEnumerator() 
        {
            SkipListNode<T> cur = _head.Next[0];
            while (cur != null)
            {
                yield return cur.Value;
                cur = cur.Next[0];
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() 
        {
            return GetEnumerator();
        }
    }
}
