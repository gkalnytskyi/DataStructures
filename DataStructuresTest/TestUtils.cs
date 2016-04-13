using System;
using DataStructures;

namespace DataStructuresTest
{
    static class TestUtils
    {
        public static void GenerateSequence(UnrolledLinkedList<int> list, int size)
        {
            int ceil_size = size + 1;
            for (int i = 1; i < ceil_size; ++i)
            {
                list.Add(i);
            }
        }

        public static UnrolledLinkedList<int> GetUnrolledLinkedListWithItems(
            int nodeCapacity,
            int sequenceLength)
        {
            var list = new UnrolledLinkedList<int>(nodeCapacity);
            GenerateSequence(list, sequenceLength);
            return list;
        }
    }
}
