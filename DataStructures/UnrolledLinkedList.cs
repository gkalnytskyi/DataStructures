using System;
using System.Collections;
using System.Collections.Generic;

namespace DataStructures
{
    public partial class UnrolledLinkedList<T> : IEnumerable<T>
    {
        private readonly int _NodeCapasity;
        private int _Count;
        private UnrolledLinkedListNode<T> _FirstNode;
        private UnrolledLinkedListNode<T> _LastNode;

        public UnrolledLinkedList() : this(16) { }

        public UnrolledLinkedList(int nodeCapasity)
        {
            if (nodeCapasity < 1)
            {
                throw new ArgumentOutOfRangeException("NodeCapasity cannot be less than 1");
            }
            _NodeCapasity = nodeCapasity;
            _LastNode = _FirstNode = new UnrolledLinkedListNode<T>(this._NodeCapasity);
            _Count = 0;
        }

        public int Count { get { return _Count; } }

        public IEnumerator<T> GetEnumerator()
        {
            return new UnroledLinkedListEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            AddLast(item);
        }

        public void AddLast(T item)
        {
            if (_LastNode.Count < _NodeCapasity)
            {
                _LastNode.Data[_LastNode.Count] = item;
                _LastNode.Count += 1;
            }
            else
            {
                var newNode = new UnrolledLinkedListNode<T>(_NodeCapasity, _LastNode, item);
                _LastNode.Next = newNode;
                _LastNode = newNode;
            }
            _Count += 1;
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }
    }
}
