﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace DataStructures
{
    public partial class UnrolledLinkedList<T>
    {
        internal class UnroledLinkedListEnumerator<T> : IEnumerator<T>, IEnumerator
        {
            private UnrolledLinkedList<T> _List;
            private int _CurrentIndex;
            private UnrolledLinkedListNode<T> _CurrentNode;

            public UnroledLinkedListEnumerator(UnrolledLinkedList<T> list)
            {
                _List = list;
                _CurrentIndex = -1;
                _CurrentNode = _List._FirstNode;
            }

            public T Current
            {
                get
                {
                    if (_CurrentIndex == -1)
                    {
                        throw new InvalidOperationException();
                    }

                    return _CurrentNode.Data[_CurrentIndex];
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return (object)Current;
                }
            }

            public bool MoveNext()
            {
                if (_CurrentNode == null)
                {
                    return false;
                }

                _CurrentIndex += 1;
                if (_CurrentIndex < _CurrentNode.Count)
                {
                    return true;
                }

                _CurrentIndex = 0;
                _CurrentNode = _CurrentNode.Next;

                if (_CurrentNode == null)
                {
                    _CurrentIndex = -1;
                    return false;
                }

                return true;
            }

            public void Reset()
            {
                _CurrentIndex = -1;
                _CurrentNode = _List._FirstNode;
            }

            public void Dispose() { }
        }
    }
}
