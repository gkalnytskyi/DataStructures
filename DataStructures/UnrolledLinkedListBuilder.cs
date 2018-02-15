using System;
using System.Collections.Generic;

namespace DataStructures
{
    internal class UnrolledLinkedListBuilder<T>
    {
        private int _NodeCapacity = -1;
        private const int NUMBER_OF_NODES = 8;
        private List<T[]> _NodesContent = new List<T[]>(NUMBER_OF_NODES);

        public UnrolledLinkedListBuilder<T> SetNodeCapacity(int capacity)
        {
            if (capacity < 1)
                throw new ArgumentException("Capacity cannot not be less than 1");

            _NodeCapacity = capacity;
            ResetNodes();
            return this;
        }

        public UnrolledLinkedListBuilder<T> AddNode(T[] node)
        {
            if (node == null)
                throw new ArgumentNullException("node");
            _NodesContent.Add(node);
            return this;
        }

        public UnrolledLinkedListBuilder<T> AddNodes(params T[][] nodes)
        {
            if (nodes == null)
            {
                throw new ArgumentNullException("nodes");
            }

            if (Array.FindIndex(nodes, x => x == null) > -1)
            {
                throw new ArgumentException("nodes", "Contains at least one 'null' element");
            }
            
            return AddNodes((IEnumerable<T[]>)nodes);
        }

        public UnrolledLinkedListBuilder<T> AddNodesFromCollection(IEnumerable<T> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            return AddNodes(Batch(items));
        }

        private UnrolledLinkedListBuilder<T> AddNodes(IEnumerable<T[]> nodes)
        {
            _NodesContent.AddRange(nodes);
            return this;
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
                throw new Exception("Node capacity should be set");
            }

            if (_NodesContent.Count == 0)
            {
                return new UnrolledLinkedList<T>(_NodeCapacity);
            }

            int halfCapasity = (_NodeCapacity + 1) / 2;
            int count = 0;

            UnrolledLinkedListNode<T> firstNode = null;
            UnrolledLinkedListNode<T> currentNode = null;

            if (_NodesContent[0].Length > _NodeCapacity)
                throw new Exception("Node content is longer than node capacity");

            if (_NodesContent[0].Length < halfCapasity && _NodesContent.Count > 1)
                throw new Exception("Only last node can be less than half full");

            currentNode = new UnrolledLinkedListNode<T>(_NodeCapacity, _NodesContent[0]);
            firstNode = currentNode;
            count += currentNode.Count;


            int nodeCount = _NodesContent.Count;
            for (int i = 1; i < nodeCount; i++)
            {
                T[] nodeContent = _NodesContent[i];

                if (nodeContent.Length > _NodeCapacity)
                    throw new Exception("Node content is longer than node capacity");

                if (nodeContent.Length < halfCapasity && (i + 1) < nodeCount)
                    throw new Exception("Only last node can be less than half full");

                var newNode = new UnrolledLinkedListNode<T>(_NodeCapacity, nodeContent);
                currentNode.AppendNode(newNode);
                currentNode = newNode;
                count += currentNode.Count;
            }

            var newList = new UnrolledLinkedList<T>(_NodeCapacity)
            {
                _FirstNode = firstNode,
                _LastNode = currentNode,
                _Count = count
            };

            return newList;
        }

        private IEnumerable<T[]> Batch(IEnumerable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            T[] bucket = null;
            int count = 0;

            foreach (T item in source)
            {
                if (bucket == null)
                {
                    bucket = new T[_NodeCapacity];
                    count = 0;
                }

                bucket[count++] = item;

                if (count == _NodeCapacity)
                {
                    yield return bucket;
                    bucket = null;
                }
            }

            if (bucket != null && count > 0)
            {
                T[] partial = new T[count];
                Array.Copy(bucket, partial, count);
                yield return partial;
            }
        }
    }
}
