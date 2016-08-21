namespace ImplementDataStructures
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class LinkedListNode<T>
    {
        ///<summary>
        ///Contructs a new node with the specified value.
        ///</summary>
        public LinkedListNode(T value)
        {
            Value = value;
        }

        ///<summary>
        ///The node value.
        ///</summary>
        public T Value { get; internal set; }

        ///<summary>
        ///The next node in linked list(null if last node).
        ///</summary>
        public LinkedListNode<T> Next { get; internal set; }

        ///<summary>
        ///The previous node in the linked list (null if first node)
        ///</summary>
        public LinkedListNode<T> Previous { get; internal set; }
    }

    public class LinkedList<T> : System.Collections.Generic.ICollection<T>
    {
        private LinkedListNode<T> _head;
        private LinkedListNode<T> _tail;


        public LinkedListNode<T> Head
        {
            get { return _head; }
        }

        public LinkedListNode<T> Tail
        {
            get { return _tail; }
        }


        public void AddFirst(T value)
        {
            LinkedListNode<T> node = new LinkedListNode<T>(value);

            //Save off the head node so we dont lose it
            LinkedListNode<T> temp = _head;

            //Point head to the new node
            _head = node;

            //Insert the rest of the list behind head
            _head.Next = temp;

            if (Count == 0)
            {
                //if the list was empty then head and tail should
                //both point to the new node.
                _tail = _head;
            }

            else 
            {
                // Before: head -------> 5 <-> 7 -> null
                // After:  head  -> 3 <-> 5 <-> 7 -> null
                temp.Previous = _head;
            }
            Count++;
        }


        public void AddLast(T value)
        {
            LinkedListNode<T> node = new LinkedListNode<T>(value);

            if (Count == 0)
            {
                _head = node;
            }
            else
            {
                _tail.Next = node;
                // Before: Head -> 3 <-> 5 -> null
                // After:  Head -> 3 <-> 5 <-> 7 -> null
                // 7.Previous = 5
                node.Previous = _tail;

            }
            _tail = node;
            Count++;

        }

        /// <summary>
        /// O(1)
        /// 1.Allocate new LinkedListNode instance.
        /// 2.Find the last node ot the existing list.
        /// 3.Point the Next property of the last node to the new node.
        /// </summary>
        /// <param name="item"></param>       

        public void Add(T value)
        {
            LinkedListNode<T> node = new LinkedListNode<T>(value);
            if (_head == null)
            {
                _head = node;
                _tail = node;
            }
            else
            {
                _tail.Next = node;
                _tail = node;
            }
            Count++;
        }

        /// <summary>
        /// O(1)
        /// </summary>
        public void Clear()
        {
            _head = null;
            _tail = null;
            Count = 0;
        }

        /// <summary>
        /// O(n)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            LinkedListNode<T> current = _head;
            while (current != null)
            {
                if (current.Value.Equals(item))
                {
                    return true;
                }

                current = current.Next;
            }
            return false;
        }

        /// <summary>
        /// O(n)
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            LinkedListNode<T> current = _head;
            while (current != null)
            {
                array[arrayIndex++] = current.Value;
                current = current.Next;
            }
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

        //O(n)
        //1.Find the node to remove.
        //2.Update the Next property of te node that precedes the node being removed
        // to point to the node that follows the node being removed.
        public bool Remove(T item)
        {
            LinkedListNode<T> previous = null;
            LinkedListNode<T> current = _head;
            //1:Empty list: Do nothing.
            //2:Single node:Previous is null.
            //3:Many nodes:
            //  a: Node to remove is the first node.
            //  b: Node to remove is the middle or last.
            while (current != null)
            {

                if (current.Value.Equals(item))
                {
                    //It`s a node in the middle or end.
                    if (previous != null)
                    {
                        //Case 3b.
                        //Before: Head ->3->5->null
                        //After:Head ->3------>null
                        previous.Next = current.Next;

                        //It was the end, so update _tail.
                        if (current.Next == null)
                        {
                            _tail = previous;
                        }
                    }
                    else
                    {
                        //Case 2 or 3a.
                        //Before: Head ->3->5
                        //After:Head-->5

                        //Head->3->null
                        //Head---->null

                        _head = _head.Next;

                        //Is the list now empty;
                        if (_head == null)
                        {
                            _tail = null;
                        }
                    }

                    Count--;

                    return true;
                }

                previous = current;
                current = current.Next;
            }

            return false;
        }

        public void RemoveFirst()
        {
            if (Count != 0)
            {
                // Before: Head -> 3 <-> 5
                // After:  Head -------> 5

                // Head -> 3 -> null
                // Head ------> null

                _head = _head.Next;
                Count--;
                if (Count == 0)
                {
                    _tail = null;
                }
                else
                {
                    /// 5.Previous was 3, now null
                    _head.Previous = null;
                }
            }
        }

        public void RemoveLast()
        {
            if (Count != 0)
            {
                if (Count == 1)
                {
                    _head = null;
                    _tail = null;
                }
                else
                {
                    // Before: Head --> 3 --> 5 --> 7
                    //         Tail = 7
                    // After:  Head --> 3 --> 5 --> null
                    //         Tail = 5
                    // Null out 5's Next pointerproperty
                    _tail.Previous.Next = null;
                    _tail = _tail.Previous;
                }
            }
            Count--;
        }

        /// <summary>
        /// O(n)
        /// </summary>
        /// <returns></returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            LinkedListNode<T> current = _head;
            while (current != null)
            {
                yield return current.Value;
                current = current.Next;
            }
        }

        /// <summary>
        /// O(1)
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)this).GetEnumerator();
        }
    }
}
