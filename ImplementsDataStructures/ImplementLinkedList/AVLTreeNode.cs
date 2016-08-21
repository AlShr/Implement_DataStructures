namespace ImplementDataStructures
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Each node in the tree will have at most two child nodes(the "left" and "right" children).
    /// Values smaller than the current node will go to the left.
    /// Values larger than or equal to the current node will go on the right.
    /// </summary>
    /// The height of the left and right nodes will never differ by more than 1.
    /// <typeparam name="TNode"></typeparam>
    public class AVLTreeNode<TNode> :IComparable<TNode>
        where TNode: IComparable
    {
        AVLTree<TNode> _tree;
        AVLTreeNode<TNode> _left;
        AVLTreeNode<TNode> _right;

        public AVLTreeNode(TNode value, AVLTreeNode<TNode> parent, AVLTree<TNode> tree)
        {
            Value = value;
            Parent = parent;
            _tree = tree;
        }

        public AVLTreeNode<TNode> Left
        {
            get 
            {
                return _left;
            }
            internal set
            {
                _left = value;
                if (_left != null)
                {
                    _left.Parent = this;
                }
            }
        }

        public AVLTreeNode<TNode> Right
        {
            get 
            {
                return _right;
            }
            internal set 
            {
                _right = value;
                if (_right != null)
                {
                    _right.Parent = this;
                }
            }
        }

        public AVLTreeNode<TNode> Parent { get; internal set; }
        public TNode Value { get; private set; }

        ///<summary>
        ///Compares the current node to the provided value.
        ///</summary>
        ///<param name = "other"> The node value to compare to. </param>
        ///<returns> 1 if the instance value is greater than the provided values, -1 if less,
        ///or 0 if equal.</returns>
        public int CompareTo(TNode other)
        {
            return Value.CompareTo(other);
        }
                
        internal void Balance() 
        {
            if (State == TreeState.RightHeavy)
            {
                if (Right != null && Right.BalanceFactor < 0)
                {
                    LeftRightRotation();
                }
                else
                {
                    LeftRotation();
                }
            }
            else if (State == TreeState.LeftHeavy)
            {
                if (Left != null && Left.BalanceFactor > 0)
                {
                    RightLeftRotation();
                }
                else 
                {
                    RightRotation();
                }
            }
        }

        private int LeftHeight
        {
            get
            {
                return MaxChildHeight(Left);
            }
        }

        private int RightHeight
        {
            get 
            {
                return MaxChildHeight(Right);
            }
        }

        
        private int MaxChildHeight(AVLTreeNode<TNode> node)
        {
            if (node != null)
            {
                return 1 + Math.Max(MaxChildHeight(node.Left), MaxChildHeight(node.Right));
            }
            return 0;
        }

        private TreeState State
        {
            get 
            {
                if (LeftHeight - RightHeight > 1)
                {
                    return TreeState.LeftHeavy;
                }

                if (RightHeight - LeftHeight > 1)
                {
                    return TreeState.RightHeavy;
                }

                return TreeState.Balanced;
            }
        }
        private int BalanceFactor
        {
            get
            {
                return RightHeight - LeftHeight;
            }
        }

        enum TreeState
        {
            Balanced,
            LeftHeavy,
            RightHeavy
        }

        /// <summary>
        /// Rotation Methods
        /// </summary>
        private void LeftRotation()
        {
            //  a
            //   \
            //    b
            //     \
            //      c
            // becomes
            //       b
            //      / \
            //     a   c

            AVLTreeNode<TNode> newRoot = Right;

            //Replace the current root with the new root.
            ReplaceRoot(newRoot);

            //Take ownership of right`s left child as right (now parent).
            Right = newRoot.Left;

            //The new root takes this as its left.
            newRoot.Left = this;
        }

        private void RightRotation()
        {
            //      c(this)
            //     / 
            //    b  
            //   /   
            //  a
            // becomes
            //      b
            //     / \    
            //    a   c

            AVLTreeNode<TNode> newRoot = Left;

            //Replace the current root with the new root.
            ReplaceRoot(newRoot);

            //Take ownership of left`s right child as left (now parent).
            Left = newRoot.Right;

            //The now root takes this as its right.
            newRoot.Right = this;
        }

        private void LeftRightRotation()
        {
            Right.RightLeftRotation();
            LeftRotation();
        }

        private void RightLeftRotation()
        {
            Left.LeftRotation();
            RightRotation();
        }

        private void ReplaceRoot(AVLTreeNode<TNode> newRoot)
        {
            if (this.Parent != null)
            {
                if (this.Parent.Left == this)
                {
                    this.Parent.Left = newRoot;
                }
                else if (this.Parent.Right == this)
                {
                    this.Parent.Right = newRoot;
                }
            }
            else
            {
                _tree.Head = newRoot;
            }

            newRoot.Parent = this.Parent;
            this.Parent = newRoot;
        }

        
    }

    public class AVLTree<T> : IEnumerable<T>
        where T:IComparable
    {
        public AVLTreeNode<T> Head { get; internal set; }
        /// <summary>
        /// Adds the specific value to the tree, ensuring that the tree is in balanced state
        /// when the method completes.
        /// O(log n)
        /// </summary>
        /// <param name="value"></param>
        public void Add(T value) 
        {
            //Case 1: The tree is empty --allocate the head.
            if (Head == null)
            {
                Head = new AVLTreeNode<T>(value, null, this);
            }
            //Case 2: The tree is not empty -- find the right location to insert.
            else
            {
                AddTo(Head, value);
            }

            Count++;
        }

        //Recursive add algorithm.
        private void AddTo(AVLTreeNode<T> node, T value)
        {
            //Case 1 : Value is less than the current node value.
            if (value.CompareTo(node.Value) < 0)
            {
                //If the is no left child, make this the new left.
                if (node.Left == null)
                {
                    node.Left = new AVLTreeNode<T>(value, node, this);
                }
                else
                {
                    //Else, add it to the left node.
                    AddTo(node.Left, value);
                }
            }
            //Case 2 : Value is equal to or greater than the current value.
            else
            {
                //If there is no right, add it to the right.
                if (node.Right == null)
                {
                    node.Right = new AVLTreeNode<T>(value, node, this);
                }
                else
                {
                    //Else, add it to the right node.
                    AddTo(node.Right, value);
                }
            }

            node.Balance();
        }


        public bool Contains(T value)
        {
            return Find(value) != null;
        }


        ///<summary>
        /// Finds and returns the first node containing the specified value. If the value 
        /// is not found, it returns null. It also returns the parent of the found node (or null) 
        /// which is used in Remove. 
        /// </summary>
        private AVLTreeNode<T> Find(T value)
        {
            AVLTreeNode<T> current = Head;

            while (current != null)
            {
                int result = current.CompareTo(value);

                if (result > 0)
                {
                    // If the value is less than current, go left. 
                    current = current.Left;
                }
                else if (result < 0)
                {
                    // If the value is greater than current, go right
                    current = current.Right;
                }
                else 
                {
                    break;
                }
            }

            return current;
        }

        public bool Remove(T value)
        {
            AVLTreeNode<T> current;
            current = Find(value);
            if (current == null)
            {
                return false;
            }

            AVLTreeNode<T> treeBalance = current.Parent;
            Count--;

            // Case 1: If current has no right child, then current's left replaces current. 
            if (current.Right == null)
            {
                if (current.Parent == null)
                {
                    Head = current.Left;
                    if (Head != null)
                    {
                        Head.Parent = null;
                    }
                }
                else
                {
                    int result = current.Parent.CompareTo(current.Value);
                    if (result > 0)
                    {
                        // If the parent value is greater than the current value,                 
                        // make the current left child a left child of parent.   
                        current.Parent.Left = current.Left;
                    }
                    else if (result < 0)
                    {
                        // If the parent value is less than the current value,                 
                        // make the current left child a right child of parent. 
                        current.Parent.Right = current.Left;
                    }
                }
            }

            //Case 2 : If currents rigth child has no left child, 
            //then currents right child replace current

            else if (current.Right.Left == null)
            {
                current.Right.Left = current.Left;
                if (current.Parent == null)
                {
                    Head = current.Right;
                    if (Head != null)
                    {
                        Head.Parent = null;
                    }
                }
                else
                {
                    int result = current.Parent.CompareTo(current.Value);
                    if (result > 0)
                    {
                        //If parent value is greater than the current value,
                        //make the current right child a left child parent.
                        if (result > 0)
                        {
                            current.Parent.Left = current.Right;
                        }
                        else if (result < 0)
                        {
                            //If parent value is less than the current value,
                            //make the current right child a right child parent.
                            current.Parent.Right = current.Right;
                        }
                    }
                }
            }

            //Case 3 :If currents right child has a left child, replace current 
            //with currents right child leftmost child.
            else
            {
                //Find the rights leftmost child.
                AVLTreeNode<T> leftmost = current.Right.Left;

                while (leftmost.Left != null)
                {
                    leftmost = leftmost.Left;
                }

                //The purrent left subtree becomes the leftmost right subtree
                leftmost.Parent.Left = leftmost.Right;

                //Assign leftmost left and right to currents left and right children.
                leftmost.Left = current.Left;

                leftmost.Right = current.Right;

                if (current.Parent == null)
                {
                    Head = leftmost;
                    if (Head != null)
                    {
                        Head.Parent = null;
                    }
                }
                else
                {
                    int result = current.Parent.CompareTo(current.Value);
                    if (result > 0)
                    {
                        //If the parent value is greater than the current value,
                        //make leftmost the parents left child.
                        current.Parent.Left = leftmost;
                    }
                    else if (result < 0)
                    {
                        //If the parent value is less than the current value,
                        //make leftmost the parents right child.
                        current.Parent.Right = leftmost;
                    }
                }
            }

            if (treeBalance != null)
            {
                treeBalance.Balance();
            }
            else
            {
                if (Head != null)
                {
                    Head.Balance();
                }
            }
            return true;

        }

        /// <summary>
        /// Enumerates the values contained in the binary tree in inorder traversal order.
        /// </summary>
        /// <returns></returns>
        /// 
        public IEnumerator<T> InOrderTraversal()
        {
            // This is a non-recursive algorithm using a stack to demonstrate removing 
            // recursion to make using the yield syntax easier. 

            if (Head != null)
            {
                Stack<AVLTreeNode<T>> stack = new Stack<AVLTreeNode<T>>();
                AVLTreeNode<T> current = Head;

                // When removing recursion, we need to keep track of whether         
                // we should be going to the left nodes or the right nodes next. 
                bool goLeftNext = true;

                //Start by pushing Head into the stack
                stack.Push(current);

                while (stack.Count > 0)
                {
                    if(goLeftNext)
                    {
                        while (current.Left != null)
                        {
                            stack.Push(current);
                            current = current.Left;
                        }
                    }
                    yield return current.Value;
                }

                if (current.Right != null)
                {
                    current = current.Right;
                    // Once we've gone right once, we need to start                 
                    // going left again.   
                    goLeftNext = true;


                }
                else
                {
                    // If we can't go right we need to pop off the parent node                 
                    // so we can process it and then go to its right node.
                    current = stack.Pop();
                    goLeftNext = false;
                }
            }
        }


        /// <summary> 
        /// Returns an enumerator that performs an inorder traversal of the binary tree. 
        /// </summary> 
        /// <returns>The inorder enumerator.</returns> 
        public IEnumerator<T> GetEnumerator()
        {
            return GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        //public void Clear();
        public int Count { get; private set; }


    }
}
