using System;
using System.Collections.Generic;
using DataStructures;
using NUnit.Framework;

namespace DataStructuresTest
{
    [TestFixture]
    public class UnrolledLinkedListBuilderTests
    {
        UnrolledLinkedListBuilder<int> builder;

        [SetUp]
        public void Init()
        {
            builder = new UnrolledLinkedListBuilder<int>();
        }

        [Test]
        public void SetCapacity_only_accepts_values_greater_than_zero([Values(-1, 0)]int val)
        {
            Assert.That(() => builder.SetNodeCapacity(val), Throws.ArgumentException);
        }

        [Test]
        public void AddNode_throws_when_null()
        {
            Assert.That(() => builder.AddNode(null), Throws.ArgumentNullException);
        }

        [Test]
        public void AddNodes_throws_when_one_of_the_arguments_null()
        {
            Assert.That(() => builder.AddNodes(new int[] { 1, 2, 3 }, null, new int[] { 8, 9 }),
                Throws.ArgumentNullException);
        }

        [Test]
        public void Build_throws_when_capacity_is_not_set()
        {
            Assert.That(() => builder.Build(), Throws.Exception);
        }

        [Test]
        public void Build_throws_when_node_size_exceeds_node_capacity()
        {
            builder.SetNodeCapacity(2);
            builder.AddNode(new int[] { 5, 6, 9 });

            Assert.That(() => builder.Build(), Throws.Exception);
        }

        [Test]
        public void Build_throws_when_non_last_node_is_less_than_half_capacity()
        {
            builder.SetNodeCapacity(4);
            builder.AddNodes(new int[] { 4, 6 },
                             new int[] { 5 },
                             new int[] { 7, 9, 2, 4 });

            Assert.That(() => builder.Build(), Throws.Exception);
        }

        [Test]
        public void Build_creates_empty_list()
        {
            builder.SetNodeCapacity(4);

            var list = builder.Build();

            Assert.That(list, Is.Not.Null);
            Assert.That(list.NodeCapacity, Is.EqualTo(4));
            Assert.That(list.Count, Is.EqualTo(0));
        }

        [Test]
        public void Build_creates_a_list()
        {
            builder.SetNodeCapacity(4);
            builder.AddNode(new int[] { 1, 2, 3 });
            builder.AddNode(new int[] { 5, 6 });

            var list = builder.Build();

            Assert.That(list.Count, Is.EqualTo(5));
            Assert.That(list._FirstNode.Count, Is.EqualTo(3));
            Assert.That(list._FirstNode.data, Is.EquivalentTo(new int[] { 1, 2, 3, 0 }));
            Assert.That(list._LastNode.Count, Is.EqualTo(2));
            Assert.That(list._LastNode.data, Is.EquivalentTo(new int[] { 5, 6, 0, 0 }));
        }
    }
}
