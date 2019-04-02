using System;
using System.Collections.Generic;
using System.Threading;
using Discord_Bot.LoggerNS;

namespace Discord_Bot.SchedulerNS
{
    public class TaskScheduler
    {
        private int CheckDelayms;
        private Thread schedulerThread;

        public bool Running;
        
        protected internal List<Task> tasks = new List<Task>();
        protected internal List<Task> executedTasks = new List<Task>();
        
        public TaskScheduler(int checkDelayms = 50)
        {
            CheckDelayms = checkDelayms;
            
            schedulerThread = new Thread(Start);
            Running = true;
            schedulerThread.Start();
        }

        public void AddTask(Task task)
        {
            DiscordBot.Logger.Log(Level.Verbose, $"Added task to be executed in {task.TimeSpan.TotalMilliseconds} milliseconds.");
            tasks.Add(task);
        }
        
        public void Stop(bool force=true)
        {
            int timems =1;

            if (!force)
            {
                foreach (Task task in tasks)
                {
                    if (task.TimeSpan.TotalMilliseconds < timems)
                    {
                        timems = (int) Math.Round(task.TimeSpan.TotalMilliseconds);
                    }
                }
            }
            
            tasks.Add(new Task(() =>
            {
                DiscordBot.Logger.Log(Level.Verbose, "Task scheduler has stopped.");
                Running = false;
                schedulerThread.Join();
            }, new TimeSpan(0, 0, 0, 0, timems + CheckDelayms)));
        }        
        
        public void Start()
        {
            while (Running)
            {
                Thread.Sleep(CheckDelayms);
                
                for (int i = 0; i < tasks.Count; i++)
                {
                    Task task = tasks[i];
                    int newtime = (int) task._timeSpan.TotalMilliseconds - CheckDelayms;

                    if (newtime < 0)
                        newtime = 0;
                    
                    task._timeSpan = new TimeSpan(0, 0, 0, 0, newtime);

                    if (task.TimeSpan.TotalMilliseconds <= 0)
                    {
                        ExecuteTask(task, i);
                    }
                }
            }
        }

        private void ExecuteTask(Task task, int index)
        {
            executedTasks.Add(task);
            task.Execute();
            DiscordBot.Logger.Log(Level.Verbose, $"Task at index {index} has been executed.");
            tasks.Remove(task);
        }
        
    }
}