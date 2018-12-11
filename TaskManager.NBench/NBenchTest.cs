using NBench;
using ProjectManager.Controllers;
using ProjectManager.Data.Models.Custom;
using System;
using System.Web.Script.Serialization;

namespace ProjectManager.NBench
{
    public class NBenchTest
    {
        private Counter _objCounter;

        [PerfSetup]
        public void SetUp(BenchmarkContext context)
        {
            _objCounter = context.GetCounter("ProjectCounter");
        }

        #region TASK
        TaskController taskController;

        [PerfBenchmark(Description = "Counter iteration performance test GETPARENTTASK()", NumberOfIterations = 10, RunMode = RunMode.Throughput, TestMode = TestMode.Measurement, RunTimeMilliseconds = 1000)]
        [CounterMeasurement("ProjectCounter")]
        [MemoryMeasurement(MemoryMetric.TotalBytesAllocated)]
        public void NBench_GetParentTask()
        {
            var bytes = new byte[1024];
            taskController = new TaskController();
            var result = taskController.GetParentTask();
            _objCounter.Increment();
        }

        [PerfBenchmark(Description = "Counter iteration performance test for GETALLTASK()", NumberOfIterations = 10, RunMode = RunMode.Throughput, TestMode = TestMode.Measurement, RunTimeMilliseconds = 1000)]
        [CounterMeasurement("ProjectCounter")]
        [MemoryMeasurement(MemoryMetric.TotalBytesAllocated)]
        public void NBench_GetAllTask()
        {
            var bytes = new byte[1024];
            taskController = new TaskController();
            var result = taskController.GetAllTask();
            _objCounter.Increment();
        }
        #endregion

        #region PROJECT
        ProjectController projectController;

        [PerfBenchmark(Description = "Counter iteration performance test GETALLPROJECT()", NumberOfIterations = 10, RunMode = RunMode.Throughput, TestMode = TestMode.Measurement, RunTimeMilliseconds = 1000)]
        [CounterMeasurement("ProjectCounter")]
        [MemoryMeasurement(MemoryMetric.TotalBytesAllocated)]
        public void NBench_GetAllProject()
        {
            var bytes = new byte[1024];
            projectController = new ProjectController();
            var result = projectController.GetAllProject();
            _objCounter.Increment();
        }
        #endregion

        #region USER
        UserController userController;

        [PerfBenchmark(Description = "Counter iteration performance test GETACTIVEUSERLIST()", NumberOfIterations = 10, RunMode = RunMode.Throughput, TestMode = TestMode.Measurement, RunTimeMilliseconds = 1000)]
        [CounterMeasurement("ProjectCounter")]
        [MemoryMeasurement(MemoryMetric.TotalBytesAllocated)]
        public void NBench_GetActiveUserList()
        {
            var bytes = new byte[1024];
            userController = new UserController();
            var result = userController.GetActiveUserList();
            _objCounter.Increment();
        }
        #endregion

        [PerfCleanup]
        public void Clean()
        {
            //does nothing
        }
    }
}
