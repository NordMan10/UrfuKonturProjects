using System;
using System.Collections;
using System.Collections.Generic;

namespace Generics.BinaryTrees
{
    public class Node<TNodeValue>
    {
        public Node(TNodeValue value)
        {
            Value = value;
        }

        public TNodeValue Value { get; private set; }
        public Node<TNodeValue> Left { get; set; }
        public Node<TNodeValue> Right { get; set; }
    }

    public class BinaryTree<TValue> : IEnumerable
        where TValue : IComparable
    {
        public TValue Value => root.Value;

        private Node<TValue> root;

        public Node<TValue> Left => root.Left;

        public Node<TValue> Right => root.Right;

        public void Add(TValue value)
        {
            if (IsEmpty)
                root = new Node<TValue>(value);
            else
                Add(root, value);
        }

        private void Add(Node<TValue> node, TValue value)
        {
            if (value.CompareTo(node.Value) <= 0)
                if (node.Left == null) node.Left = new Node<TValue>(value);
                else Add(node.Left, value);
            else
                if (node.Right == null) node.Right = new Node<TValue>(value);
                else Add(node.Right, value);
        }

        private bool IsEmpty => root == null;

        public TValue First()
        {
            return root.Value;
        }

        public IEnumerator GetEnumerator()
        {
            if (root == null) yield break;

            var top = root;

            var stack = new Stack<Node<TValue>>();
            while (top != null || stack.Count != 0)
            {
                if (stack.Count != 0)
                {
                    top = stack.Pop();
                    yield return top.Value;

                    if (top.Right != null) top = top.Right;
                    else top = null;
                }
                while (top != null)
                {
                    stack.Push(top);
                    top = top.Left;
                }
            }
        }
    }

    public class BinaryTree
    {
        public static BinaryTree<TValue> Create<TValue>(params TValue[] values)
            where TValue : IComparable
        {
            var tree = new BinaryTree<TValue>();
            foreach (var e in values)
            {
                tree.Add(e);
            }

            return tree;
        }
    }
}
