﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GhostCore
{
    public sealed class TaskCompletionNotifier<TResult> : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the task being watched. This property never changes and is never <c>null</c>.
        /// </summary>
        public Task<TResult> Task { get; private set; }

        /// <summary>
        /// Gets the result of the task. Returns the default value of TResult if the task has not completed successfully.
        /// </summary>
        public TResult Result { get { return (Task.Status == TaskStatus.RanToCompletion) ? Task.Result : default(TResult); } }

        /// <summary>
        /// Gets whether the task has completed.
        /// </summary>
        public bool IsCompleted { get { return Task.IsCompleted; } }

        /// <summary>
        /// Gets whether the task has completed successfully.
        /// </summary>
        public bool IsSuccessfullyCompleted { get { return Task.Status == TaskStatus.RanToCompletion; } }

        /// <summary>
        /// Gets whether the task has been canceled.
        /// </summary>
        public bool IsCanceled { get { return Task.IsCanceled; } }

        /// <summary>
        /// Gets whether the task has faulted.
        /// </summary>
        public bool IsFaulted { get { return Task.IsFaulted; } }

        public TaskCompletionNotifier(Task<TResult> task)
        {
            Task = task;
            if (!task.IsCompleted)
            {
                var scheduler = (SynchronizationContext.Current == null) ? TaskScheduler.Current : TaskScheduler.FromCurrentSynchronizationContext();
                task.ContinueWith(t =>
                {
                    var propertyChanged = PropertyChanged;
                    if (propertyChanged != null)
                    {
                        propertyChanged(this, new PropertyChangedEventArgs("IsCompleted"));
                        if (t.IsCanceled)
                        {
                            propertyChanged(this, new PropertyChangedEventArgs("IsCanceled"));
                        }
                        else if (t.IsFaulted)
                        {
                            propertyChanged(this, new PropertyChangedEventArgs("IsFaulted"));
                            propertyChanged(this, new PropertyChangedEventArgs("ErrorMessage"));
                        }
                        else
                        {
                            propertyChanged(this, new PropertyChangedEventArgs("IsSuccessfullyCompleted"));
                            propertyChanged(this, new PropertyChangedEventArgs("Result"));
                        }
                    }
                },
                CancellationToken.None,
                TaskContinuationOptions.ExecuteSynchronously,
                scheduler);
            }
        }
    }
}
