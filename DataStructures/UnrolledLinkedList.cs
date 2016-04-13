using System;
using System.Collections;
using System.Collections.Generic;

namespace DataStructures
{
    public partial class UnrolledLinkedList<T> : ICollection<T>, IEnumerable<T>
    {
        private readonly int _NodeCapasity;
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
            Count = 0;
        }

        public int Count { get; private set; }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

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

        private void AddLast(T item)
        {
            if (!_LastNode.TryAddItem(item))
            {
                var newNode = new UnrolledLinkedListNode<T>(_NodeCapasity, _LastNode, item);
                _LastNode.Next = newNode;
                _LastNode = newNode;
            }
            Count += 1;
        }


        public bool Remove(T item)
        {
            return RemoveFirst(item);
        }

        private bool RemoveFirst(T item)
        {
            var found = false;
            var currentNode = _FirstNode;

            do
            {
                var index_found = Array.IndexOf(currentNode.Data, item, 0, currentNode.Count);
                if (index_found > -1)
                {
                    found = true;
                    currentNode.RemoveItem(index_found);
                    Count -= 1;
                    if (currentNode.IsEmpty())
                    {
                        var prevNode = currentNode.Previous;
                        var nextNode = currentNode.Next;
                        if (nextNode != null)
                        {
                            nextNode.Previous = prevNode;
                        }
                        if (prevNode != null)
                        {
                            prevNode.Next = nextNode;
                        }
                    }
                    break;
                }
                currentNode = currentNode.Next;
            }
            while ((currentNode != null));

            return found;
        }

        public void Clear()
        {
            _LastNode = _FirstNode = new UnrolledLinkedListNode<T>(_NodeCapasity);
            Count = 0;
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }
    }
}
