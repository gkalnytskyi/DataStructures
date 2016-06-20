using System;
using System.Collections;
using System.Collections.Generic;

namespace DataStructures
{
    public sealed partial class UnrolledLinkedList<T> : IList<T>, ICollection<T>, IEnumerable<T>
    {
        public const int NODE_CAPACITY = 16;
        private readonly int _NodeCapacity;
        private int _Count = 0;
        internal UnrolledLinkedListNode<T> _FirstNode;
        internal UnrolledLinkedListNode<T> _LastNode;


        #region Constructors
        public UnrolledLinkedList() : this(NODE_CAPACITY) { }

        public UnrolledLinkedList(int nodeCapacity)
        {
            if (nodeCapacity < 1)
            {
                throw new ArgumentOutOfRangeException("NodeCapacity cannot be less than 1");
            }
            _NodeCapacity = nodeCapacity;
            _LastNode = _FirstNode = new UnrolledLinkedListNode<T>(_NodeCapacity);
            _Count = 0;
        }

        public UnrolledLinkedList(
            int nodeCapacity,
            IEnumerable<T> collection) : this(nodeCapacity)
        {
            AddRange(collection);
        }
        #endregion Constructors


        #region ICollection<T> Properties
        public int Count { get { return _Count; } }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }
        #endregion


        #region IList<T> Properties
        public T this[int index]
        {
            get
            {
                ValidateIndex(index, _Count - 1);
                var nodeandindex = FindNodeAndNodeIndex(index);
                return nodeandindex.Node.data[nodeandindex.Index];
            }

            set
            {
                ValidateIndex(index, _Count - 1);
                var nodeandindex = FindNodeAndNodeIndex(index);
                nodeandindex.Node.data[nodeandindex.Index] = value;
            }
        }
        #endregion


        #region IEnumerable<T> Methods
        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion IEnumerable<T> Methods

        #region ICollection<T> Methods
        /// <summary>
        /// Adds an item to the end of the collection
        /// <param name="item"></param>
        public void Add(T item)
        {
            Insert(_Count, item);
        }

        /// <summary>
        /// Removes the first occurrence of the item in the list
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(T item)
        {
            var found = false;
            int nodeIndex = -1;
            var node = _FirstNode;


            while (node != null)
            {
                nodeIndex = node.IndexOf(item);

                if (nodeIndex > -1)
                {
                    found = true;
                    break;
                }
                node = node.next;
            }

            if (found)
            {
                node.RemoveAt(nodeIndex);
                _Count--;
                UpdateLastNode(node);
            }

            return found;
        }

        /// <summary>
        /// Removes all items from the collection
        /// </summary>
        public void Clear()
        {
            _LastNode = _FirstNode = new UnrolledLinkedListNode<T>(_NodeCapacity);
            _Count = 0;
        }

        /// <summary>
        /// Determines whether the collection contains a specific value.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            bool found = false;
            var currentNode = _FirstNode;
            while (!found && currentNode != null)
            {
                found = currentNode.Contains(item);
                currentNode = currentNode.next;
            }
            return found;
        }

        /// <summary>
        /// Copies the elements of the collection to an Array, starting at a particular Array index.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("Parameter 'array' cannot be null");
            }

            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException("Parameter 'arrayIndex' cannot be negative");
            }

            if (array.Length - arrayIndex < Count)
            {
                throw new ArgumentException(
                    "Data will not fit into destination 'array' " +
                    "starting from specified index. " + 
                    "{\"array.Length\" : {{{0}}}, \"arrayIndex\" : {{{1}}}}");
            }

            var currentNode = _FirstNode;
            while (currentNode != null)
            {
                currentNode.CopyTo(array, arrayIndex);
                arrayIndex += currentNode.Count;
                currentNode = currentNode.next;
            }
        }
        #endregion ICollection<T> Methods


        #region IList<T> Methods
        public int IndexOf(T item)
        {
            bool found = false;
            int nodeIndex = -1;
            var node = _FirstNode;
            int itemsSearched = 0;

            while (node != null && !found)
            {
                nodeIndex = node.IndexOf(item);
                if (nodeIndex > -1)
                {
                    found = true;
                    break;
                }
                itemsSearched += node.Count;
                node = node.next;
            }
            return (found) ? (itemsSearched + nodeIndex) : (nodeIndex);
        }

        public void Insert(int index, T item)
        {
            ValidateIndex(index, _Count);
            if (index == _Count)
            {
                if (_LastNode.IsNotFull())
                {
                    _LastNode.Add(item);
                }
                else
                {
                    var newNode = new UnrolledLinkedListNode<T>(_NodeCapacity);
                    newNode.Add(item);
                    _LastNode.AppendNode(newNode);
                    _LastNode = newNode;
                }
                _Count++;
                return;
            }

            var nodeandindex = FindNodeAndNodeIndex(index);
            var node = nodeandindex.Node;
            var nodeIndex = nodeandindex.Index;
            if (node.IsNotFull())
            {
                node.Insert(nodeIndex, item);
            }
            else
            {
                var newNode = new UnrolledLinkedListNode<T>(_NodeCapacity);
                node.InsertNodeNext(newNode);
                var targetNode = node;
                int midIndex = _NodeCapacity / 2;

                if (nodeIndex > midIndex)
                {
                    midIndex += 1;
                    nodeIndex -= midIndex;
                    targetNode = newNode;
                }

                node.PushItemsToNextNode(midIndex);

                targetNode.Insert(nodeIndex, item);

                if (node == _LastNode)
                    _LastNode = newNode;
            }
            _Count++;
        }

        public void RemoveAt(int index)
        {
            ValidateIndex(index, _Count - 1);
            var nodeandindex = FindNodeAndNodeIndex(index);
            var node = nodeandindex.Node;
            var nodeIndex = nodeandindex.Index;
            node.RemoveAt(nodeIndex);
            _Count--;

            UpdateLastNode(node);
        }
        #endregion IList<T> Methods


        #region Public Non-Interface Methods
        #region List-like methods
        public void AddRange(IEnumerable<T> range)
        {
            InsertRange(_Count, range);
        }

        public void InsertRange(int index, IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("Collection cannot be null");
            }

            ValidateIndex(index, _Count);

            int collectionLength = 0;

            var nodeandindex = FindNodeAndNodeIndex(index);
            var node = nodeandindex.Node;
            var nodeIndex = nodeandindex.Index;

            var initialNextNode = node.next;
            int remainingNodeItemsCount = node.Count - nodeIndex;

            ICollection<T> c = collection as ICollection<T>;
            if (c != null)
            {
                collectionLength = c.Count;

                T[] newData = new T[collectionLength + remainingNodeItemsCount];
                c.CopyTo(newData, 0);

                node.CopyTo(nodeIndex, newData, collectionLength, remainingNodeItemsCount);
                node.RemoveRange(nodeIndex, remainingNodeItemsCount);

                int copyStartIndex = 0;
                int newDataLength = newData.Length;

                while (copyStartIndex < newDataLength)
                {
                    int itemsToCopyCount = Math.Min(
                        _NodeCapacity - node.Count,
                        newDataLength - copyStartIndex);
                    node.AddRange(newData, copyStartIndex, itemsToCopyCount);
                    copyStartIndex += itemsToCopyCount;
                    var newNode = new UnrolledLinkedListNode<T>(_NodeCapacity);
                    node.AppendNode(newNode);
                    node = newNode;
                }
            }
            else
            {
                T[] remainingNodeData = new T[remainingNodeItemsCount];
                node.CopyTo(nodeIndex, remainingNodeData, 0, remainingNodeItemsCount);
                node.RemoveRange(nodeIndex, remainingNodeItemsCount);

                // Don't want to import System.Linq namespace, so no Concat methods
                var enumerator = collection.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    node.Add(enumerator.Current);
                    collectionLength++;
                    if (node.Count == _NodeCapacity)
                    {
                        var newNode = new UnrolledLinkedListNode<T>(_NodeCapacity);
                        node.AppendNode(newNode);
                        node = newNode;
                    }
                }

                for (int i = 0; i < remainingNodeItemsCount; i++)
                {
                    node.Add(remainingNodeData[i]);
                    if (node.Count == _NodeCapacity)
                    {
                        var newNode = new UnrolledLinkedListNode<T>(_NodeCapacity);
                        node.AppendNode(newNode);
                        node = newNode;
                    }
                }
            }

            if (node.IsEmpty())
            {
                node = node.previous;
            }

            node.AppendNode(initialNextNode);
            node.PullItemsFromNextNode();
            UpdateLastNode(node);

            _Count += collectionLength;
        }

        private void RemoveRange(int index, int count)
        {
            throw new NotImplementedException();
        }
        #endregion List-like methods
        #endregion Public Non-Interface Methods


        #region Private Methods
        private void ValidateIndex(int index, int maxValue)
        {
            if (index < 0 || index > maxValue)
                throw new IndexOutOfRangeException(
                      string.Format("Index cannot be less than 0, " +
                                    "or greater than Count: {0}", maxValue));
        }

        private struct NodeAndNodeIndex
        {
            public UnrolledLinkedListNode<T> Node;
            public int Index;

            public NodeAndNodeIndex(UnrolledLinkedListNode<T> node, int index)
            {
                Node = node;
                Index = index;
            }
        }

        private NodeAndNodeIndex FindNodeAndNodeIndex(int index)
        {
            var currentNode = _FirstNode;
            while (currentNode != null)
            {
                if (index < currentNode.Count || currentNode.Count == 0)
                {
                    break;
                }
                index -= currentNode.Count;
                currentNode = currentNode.next;
            }

            if (currentNode == null)
            {
                currentNode = new UnrolledLinkedListNode<T>(_NodeCapacity);
                _LastNode.AppendNode(currentNode);
                _LastNode = currentNode;
            }
            return new NodeAndNodeIndex(currentNode, index);
        }

        private void UpdateLastNode(UnrolledLinkedListNode<T> node)
        {
            // prevNode, node, nextNode;
            var nextNode = node.next;
            var prevNode = node.previous;
            if (nextNode == _LastNode && nextNode.IsEmpty())
            {
                node.ByPassNextNode();
                _LastNode = node;
                return;
            }
            if (nextNode == null && !node.IsEmpty())
            {
                _LastNode = node;
                return;
            }
            if (node.IsEmpty() && prevNode != null)
            {
                prevNode.ByPassNextNode();
                _LastNode = prevNode;
                return;
            }
        }
        #endregion Private Methods
    }
}
