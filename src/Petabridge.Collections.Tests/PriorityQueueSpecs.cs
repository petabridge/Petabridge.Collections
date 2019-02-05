// -----------------------------------------------------------------------
// <copyright file="PriorityQueueSpecs.cs" company="Petabridge, LLC">
//      Copyright (C) 2015 - 2019 Petabridge, LLC <https://petabridge.com>
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using FsCheck;
using FsCheck.Xunit;

namespace Petabridge.Collections.Tests
{
    public class PriorityQueueSpecs
    {
        [Property]
        public Property Default_PriorityQueue_count_will_correctly_reflect_all_added_items(long[] items)
        {
            var priorityQueue = new PriorityQueue<long>();
            foreach (var item in items)
                priorityQueue.Enqueue(item);

            return (priorityQueue.Count == items.Length).Label(
                $"Expected priority queue to have size of {items.Length} " +
                $"when fully populated, but was {priorityQueue.Count}");
        }

        [Property]
        public Property Default_PriorityQueue_will_contain_all_added_items(long[] items)
        {
            var priorityQueue = new PriorityQueue<long>();
            foreach (var item in items)
                priorityQueue.Enqueue(item);

            return items.All(x => priorityQueue.Contains(x)).Label(
                $"Expected priority queue to contain [{string.Join(";", items)}] " +
                $"but instead was [{string.Join(";", priorityQueue)}] (unsorted)");
        }

        [Property]
        public Property Default_PriorityQueue_will_sort_items_into_ascending_order_on_dequeue(long[] items)
        {
            var priorityQueue = new PriorityQueue<long>();
            var expectedOrder = items.OrderBy(x => x).ToArray();
            var actualOrder = new List<long>(items.Length);

            foreach (var item in items)
                priorityQueue.Enqueue(item);


            var count = priorityQueue.Count;
            for (var i = 0; i < count; i++)
                actualOrder.Add(priorityQueue.Dequeue());

            return expectedOrder.SequenceEqual(actualOrder).Label($"Expected input [{string.Join(";", items)}] to " +
                                                                  $"be sorted into [{string.Join(";", expectedOrder)}]" +
                                                                  $" but was [{string.Join(";", actualOrder)}]");
        }
    }
}