using System;

namespace Discord_Bot.SchedulerNS
{
    public class Task
    {
        Action Action { get; }

        public bool Executed = false;
        
        protected internal TimeSpan _timeSpan;
        public TimeSpan TimeSpan => _timeSpan;
        
        public Task(Action action, TimeSpan timeSpan)
        {
            Action = action;
            _timeSpan = timeSpan;
        }

        public void Execute()
        {
            Executed = true;
            Action();
        }
    }
}