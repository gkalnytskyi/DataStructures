using System;

namespace DataStructures
{
    class UnrolledLinkedListNode<T>
    {
        public UnrolledLinkedListNode<T> Previous;
        public UnrolledLinkedListNode<T> Next;
        public int Count;
        public T[] Data;

        public UnrolledLinkedListNode(int capacity)
        {
            Data = new T[capacity];
        }

        public UnrolledLinkedListNode(
            int capacity,
            UnrolledLinkedListNode<T> previous) : this(capacity)
        {
            Previous = previous;
        }

        public UnrolledLinkedListNode(
            int capacity,
            UnrolledLinkedListNode<T> previous,
            UnrolledLinkedListNode<T> next): this(capacity, previous)
        {
            Next = next;
        }

        public UnrolledLinkedListNode(
            int capacity,
            UnrolledLinkedListNode<T> previous,
            T firstElement) : this(capacity, previous)
        {
            Data[0] = firstElement;
            Count += 1;
        }
    }
}
