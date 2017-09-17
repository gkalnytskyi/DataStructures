using System;
using System.Collections.Generic;

namespace DataStructures
{
    internal class UnrolledLinkedListBuilder<T>
    {
        private int _NodeCapacity = -1;
        private const int NUMBER_OF_NODES = 8;
        private List<T[]> _NodesContent = new List<T[]>(NUMBER_OF_NODES);

        public void SetNodeCapacity(int capacity)
        {
            if (capacity < 1)
                throw new ArgumentException("Capacity cannot not be less than 1");

            _NodeCapacity = capacity;
        }

        public void AddNode(T[] node)
        {
            _NodesContent.Add(node);
        }

        public void AddNodes(params T[][] nodes)
        {
            _NodesContent.AddRange(nodes);
        }

        public void ResetNodes()
        {
            _NodesContent.Clear();
            _NodesContent.Capacity = NUMBER_OF_NODES;
        }

        public UnrolledLinkedList<T> Build()
        {
            if (_NodeCapacity < 1)
            {
                throw new ArgumentException("Node capacity should be set");
            }

            if (_NodesContent.Count == 0)
            {
                return new UnrolledLinkedList<T>(_NodeCapacity);
            }

            int halfCapasity = (_NodeCapacity + 1) / 2;

            UnrolledLinkedListNode<T> firstNode = null;
            UnrolledLinkedListNode<T> currentNode = null;

            if (_NodesContent[0].Length < halfCapasity && _NodesContent.Count > 1)
                throw new Exception("Only last node can be less than half full");
            
            currentNode = new UnrolledLinkedListNode<T>(_NodeCapacity, _NodesContent[0]);
            firstNode = currentNode;
                

            int nodeCount = _NodesContent.Count;
            for (int i = 1; i < nodeCount; i++)
            {
                T[] nodeContent = _NodesContent[i];

                if (nodeContent.Length > _NodeCapacity)
                    throw new Exception("Node content is longer than node capacity");

                if (nodeContent.Length < halfCapasity && (i+1) < nodeCount)
                    throw new Exception("Only last node can be less than half full");

                var newNode = new UnrolledLinkedListNode<T>(_NodeCapacity, nodeContent);
                currentNode.AppendNode(newNode);
                currentNode = newNode;
            }

            var newList = new UnrolledLinkedList<T>(_NodeCapacity)
            {
                _FirstNode = firstNode,
                _LastNode = currentNode
            };

            return newList;
        }
    }
}
