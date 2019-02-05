using System.Collections.Concurrent;
using NBench;

namespace Petabridge.Collections.Tests.Performance
{
    /// <summary>
    ///     Performance specs for checking the underlying Collections' implementation's read / write performance
    /// </summary>
    public class AllCollectionsSpec
    {
        private const string InsertCounterName = "ItemInserts";
        private const int ItemCount = 1000;
        private const int ResizedItemCount = 10*ItemCount;
        private Counter _insertsCounter;

        private readonly CircularBuffer<int> _circularBuffer = new CircularBuffer<int>(ItemCount);

        private readonly ConcurrentCircularBuffer<int> _concurrentCircularBuffer =
            new ConcurrentCircularBuffer<int>(ItemCount);

        private readonly ConcurrentQueue<int> _concurrentQueue = new ConcurrentQueue<int>();

        [PerfSetup]
        public void SetUp(BenchmarkContext context)
        {
            _insertsCounter = context.GetCounter(InsertCounterName);
        }

        [PerfBenchmark(NumberOfIterations = 13, RunMode = RunMode.Iterations, TestMode = TestMode.Measurement)]
        [CounterMeasurement(InsertCounterName)]
        [MemoryMeasurement(MemoryMetric.TotalBytesAllocated)]
        [GcMeasurement(GcMetric.TotalCollections, GcGeneration.AllGc)]
        public void CircularBufferWithoutResizing(BenchmarkContext context)
        {
            for (var i = 0; i < ItemCount;)
            {
                _circularBuffer.Add(i);
                _insertsCounter.Increment();
                ++i;
            }
        }

        [PerfBenchmark(NumberOfIterations = 13, RunMode = RunMode.Iterations, TestMode = TestMode.Measurement)]
        [CounterMeasurement(InsertCounterName)]
        [MemoryMeasurement(MemoryMetric.TotalBytesAllocated)]
        [GcMeasurement(GcMetric.TotalCollections, GcGeneration.AllGc)]
        public void CircularBufferWithResizing(BenchmarkContext context)
        {
            for (var i = 0; i < ResizedItemCount;)
            {
                _circularBuffer.Add(i);
                _insertsCounter.Increment();
                ++i;
            }
        }

        [PerfBenchmark(NumberOfIterations = 13, RunMode = RunMode.Iterations, TestMode = TestMode.Measurement)]
        [CounterMeasurement(InsertCounterName)]
        [MemoryMeasurement(MemoryMetric.TotalBytesAllocated)]
        [GcMeasurement(GcMetric.TotalCollections, GcGeneration.AllGc)]
        public void ConcurrentCircularBufferWithoutResizing(BenchmarkContext context)
        {
            for (var i = 0; i < ItemCount;)
            {
                _concurrentCircularBuffer.Add(i);
                _insertsCounter.Increment();
                ++i;
            }
        }

        [PerfBenchmark(NumberOfIterations = 13, RunMode = RunMode.Iterations, TestMode = TestMode.Measurement)]
        [CounterMeasurement(InsertCounterName)]
        [MemoryMeasurement(MemoryMetric.TotalBytesAllocated)]
        [GcMeasurement(GcMetric.TotalCollections, GcGeneration.AllGc)]
        public void ConcurrentCircularBufferWithResizing(BenchmarkContext context)
        {
            for (var i = 0; i < ResizedItemCount;)
            {
                _concurrentCircularBuffer.Add(i);
                _insertsCounter.Increment();
                ++i;
            }
        }

        [PerfBenchmark(NumberOfIterations = 13, RunMode = RunMode.Iterations, TestMode = TestMode.Measurement)]
        [CounterMeasurement(InsertCounterName)]
        [MemoryMeasurement(MemoryMetric.TotalBytesAllocated)]
        [GcMeasurement(GcMetric.TotalCollections, GcGeneration.AllGc)]
        public void ConcurrentQueueWithoutResizing(BenchmarkContext context)
        {
            for (var i = 0; i < ItemCount;)
            {
                _concurrentQueue.Enqueue(i);
                _insertsCounter.Increment();
                ++i;
            }
        }

        [PerfBenchmark(NumberOfIterations = 13, RunMode = RunMode.Iterations, TestMode = TestMode.Measurement)]
        [CounterMeasurement(InsertCounterName)]
        [MemoryMeasurement(MemoryMetric.TotalBytesAllocated)]
        [GcMeasurement(GcMetric.TotalCollections, GcGeneration.AllGc)]
        public void ConcurrentQueueWithResizing(BenchmarkContext context)
        {
            for (var i = 0; i < ResizedItemCount;)
            {
                _concurrentQueue.Enqueue(i);
                _insertsCounter.Increment();
                ++i;
            }
        }
    }
}