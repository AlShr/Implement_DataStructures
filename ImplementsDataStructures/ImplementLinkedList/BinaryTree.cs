namespace ImplementDataStructures
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    #region Binary Tree
    /// <summary>
    /// 1.Each node can have 0,1 or 2 children.
    /// 2.Any values less than the nodes values goes to the left child(or child of the left child).
    /// 3.Any value greater than, or equal to , the nodes values goes to the right child(or a child thereof).
    /// </summary>
    public class BinaryTree<T> : IEnumerable<T> 
        where T : IComparable<T>
    {
        private BinaryTreeNode<T> _head;

        private int _count;
        /// <summary>
        /// O(log n) on average -----> O(n)
        /// </summary>
        /// <param name="value"></param>
        
        public void Add(T value)
        {
            //Case 1: The tree is empty. Allocate the head.
            if (_head == null)
            {
                _head = new BinaryTreeNode<T>(value);
            }
            //Case 2: The tree is not empty, so recursively
            //find the right location to insert the node.
            else
            {
                AddTo(_head, value);
            }

            _count++;
        }

        //Recursive add algorithm.
        private void AddTo(BinaryTreeNode<T> node, T value)
        {
            //Case 1: Value is less than the current node value
            if (value.CompareTo(node.Value) < 0)
            {
                //If there is no left child, make this the new left,
                if (node.Left == null)
                {
                    node.Left = new BinaryTreeNode<T>(value);
                }
                else
                {
                    //else add it to the left node.
                    AddTo(node.Left, value);
                }
            }
            //Case 2:Value is equal to or greater than the current value.
            else
            {
                if (node.Right == null)
                {
                    node.Right = new BinaryTreeNode<T>(value);
                }
                else
                {
                    //else add it to the right node.
                    AddTo(node.Right, value);
                }
            }
        }

        private BinaryTreeNode<T> FindWithParent(T value, out BinaryTreeNode<T> parent)
        {
            //Now, try to find data in the tree.
            BinaryTreeNode<T> current = _head;
            parent = null;
            
            //While we dont have a match...
            while (current != null)
            {
                int result = current.CompareTo(value);
                if (result > 0)
                {
                    //If the values is less than current, go left
                    parent = current;
                    current = current.Left;
                }
                else if (result < 0)
                {
                    //If the value is more than current, go right.
                    parent = current;
                    current = current.Right;
                }
                else
                {
                    //We have match!
                    break;
                }
            }
            return current;
        }

        public bool Remove(T value)
        {
            BinaryTreeNode<T> current, parent;
            //Find the node to remove.
            current = FindWithParent(value, out parent);
            if (current == null)
            {
                return false;
            }
            _count--;

            //Case 1:If current has no right child, currents left repleces current.
            if (current.Right == null)
            {
                if (parent == null)
                {
                    _head = current.Left;
                }
                else
                {
                    int result = parent.CompareTo(current.Value);
                    if (result > 0)
                    {
                        //If parent value is greater than current value,
                        //make the current left child a left child of parent.
                        parent.Left = current.Left;
                    }
                    else if (result < 0)
                    {
                        //If parent value is less than current value,
                        //make the current left child a right child of parent.
                        parent.Right = current.Left;
                    }
                }
            }
            //Case 2: If currents right child has no left child, currents right child
            //replaces current
            else if (current.Right.Left == null)
            {
                current.Right.Left = current.Left;
                if (parent == null)
                {
                    _head = current.Right;
                }
                else
                {
                    int result = parent.CompareTo(current.Value);
                    if (result > 0)
                    {
                        //If parent value is greater than current value,
                        //make the current right child a left child of parent.
                        parent.Left = current.Right;
                    }
                    else if (result < 0)
                    {
                        //If parent value is less than current value,
                        //make the current right child a right child of parent.
                        parent.Right = current.Right;
                    }
                }
            }
            //Case 3: If currents right child has child has a left childs replace current with
            //currents right childs left most child
            else
            {
                BinaryTreeNode<T> leftmost = current.Right.Left;
                BinaryTreeNode<T> leftmostParent = current.Right;
                while (leftmost.Left != null)
                {
                    leftmostParent = leftmost;
                    leftmost = leftmost.Left;
                }

                //The parent left subtree brvomes the leftmosts right subtree.
                leftmostParent.Left = leftmost.Right;

                //Assign leftmosts left and right to currents left and right children.
                leftmost.Left = current.Left;
                leftmost.Right = current.Right;

                if (parent == null)
                {
                    _head = leftmost;
                }
                else
                {
                    int result = parent.CompareTo(current.Value);
                    if (result > 0)
                    {
                        //If parent value is greater than current value,
                        //make leftmost the parents left child.
                        parent.Left = leftmost;
                    }
                    else if (result < 0)
                    {
                        //If parent value is less than current value,
                        //make leftmost the parents right child.
                        parent.Right = leftmost;
                    }
                }
            }

            return true;  
        }

        /// <summary>
        /// 1. If the current node is null, return null.
        /// 2. If current node value equals the sought value, return the current node.
        /// 3. If sought value is less than the current value, set the current node to left child and go
        /// to step #1.
        /// 4 Set current  node to right chils and go to step #1.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(T value)
        {
            //Defer to the node search helper function.
            BinaryTreeNode<T> parent;
            return FindWithParent(value, out parent) != null;
        }

        /// <summary>
        /// O(n)
        /// A common usage of the preorder traversal would be to create a copy 
        /// of the tree that contained not just the same node values, but also 
        /// the same hierarchy. 
        /// </summary>
        /// <param name="action"></param>
        public void PreOrderTraversal(Action<T> action)
        {
            PreOrderTraversal(action, _head);
        }

        private void PreOrderTraversal(Action<T> action, BinaryTreeNode<T> node)
        {
            if(node != null)
            {
                action(node.Value);
                PreOrderTraversal(action, node.Left);
                PreOrderTraversal(action, node.Right);
            }
            
        }
        /// <summary>
        /// O(n)
        /// Postorder traversals are often used to delete an entire tree, such as in
        /// programming languages where each node must be freed, or to delete subtrees.
        /// </summary>
        /// <param name="action"></param>
        public void PostOrderTraversal(Action<T> action)
        {
            PostOrderTraversal(action, _head);
        }

        private void PostOrderTraversal(Action<T> action, BinaryTreeNode<T> node)
        {
            if (node != null)
            {
                PostOrderTraversal(action, node.Left);
                PostOrderTraversal(action, node.Right);
                action(node.Value);
            }
        }

        /// <summary>
        /// O(n)
        /// </summary>
        /// <param name="action"></param>
        public void InOrderTraversal(Action<T> action)
        {
            InOrderTraversal(action, _head);
        }


        private void InOrderTraversal(Action<T> action, BinaryTreeNode<T> node)
        {
            if (node != null)
            {
                InOrderTraversal(action, node.Left);
                action(node.Value);
                InOrderTraversal(action, node.Right);
            }
        }

        public IEnumerator<T> InOrderTraversal()
        {
            //This is a non-recursive algoritm using a stack to demonstrate removing recursion.
            if (_head != null)
            {
                //Store the nodes we have skipped in this stack(avoids recursion).
                Stack<BinaryTreeNode<T>> stack = new Stack<BinaryTreeNode<T>>();
                BinaryTreeNode<T> current = _head;

                //When removing recursion we need to keep track of whether
                //we should be going to the left node or the right node next.
                bool goLeftNext = true;

                //Start by pushing Hesd onto the stack.
                stack.Push(current);

                while (stack.Count > 0)
                {
                    //If we are heading left...
                    if (goLeftNext)
                    {
                        //Push everything but the left-most node  to the stack.
                        //We yield the left-most after the block.
                        while (current.Left != null)
                        {
                            stack.Push(current);
                            current = current.Left;
                        }
                    }

                    //Inorder is left->yield->right.
                    yield return current.Value;

                    //If we can go right do so.
                    if (current.Right != null)
                    {
                        current = current.Right;

                        //Once weve gone right once we need to start
                        //going left again.
                        goLeftNext = true;
                    }
                    else
                    {
 
                        //If we cant go right then we need to pop off the parent node
                        //so we can process it and then go its right node.
                        current = stack.Pop();
                        goLeftNext = false;
                    }
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return InOrderTraversal();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// O(1)
        /// </summary>
        public void Clear()
        {
            _head = null;
            _count = 0;
        }

        /// <summary>
        /// O(1)
        /// </summary>
        public int Count
        {
            get 
            {
                return _count;
            }
        }
    }

    class BinaryTreeNode<TNode> : IComparable<TNode>
        where TNode : IComparable<TNode>
    {
        public BinaryTreeNode(TNode value)
        {
            Value = value;
        }
        public BinaryTreeNode<TNode> Left { get; set; }
        public BinaryTreeNode<TNode> Right { get; set; }
        public TNode Value { get; private set; }

        ///<summary>
        ///Compares the current node to the provided value.
        ///</summary>
        ///<param name="other"> The node value to compare to </param>
        ///<returns>1 if the instance value is greater than
        ///provided value, -1 if less, or 0 if equal.</returns>
        public int CompareTo(TNode other)
        {
            return Value.CompareTo(other);
        }
    }

   

    #endregion
}
