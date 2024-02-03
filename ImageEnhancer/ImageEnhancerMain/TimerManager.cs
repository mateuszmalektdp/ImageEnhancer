using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageEnhancerMain
{
    internal class TimerManager
    {
        public TimerManager() {
            StartTimer();
            StopTimer("0");
        }
        Stopwatch stopwatch;
        public void StartTimer()
        {
            stopwatch = Stopwatch.StartNew();
        }

        public String StopTimer(String ExecutionTime)
        {
            stopwatch.Stop();
            long ticks = stopwatch.ElapsedTicks;
            long microSeconds = (ticks * 1000000) / Stopwatch.Frequency; // Converting ticks to microseconds
            ExecutionTime = microSeconds.ToString() + " μs";
            return ExecutionTime;
        }

    }
}
