// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="AsyncHelpers.cs">
//
// </copyright>
// <summary>
//   The async helpers.
// </summary>
//
// --------------------------------------------------------------------------------------------------------------------
namespace AlexaSkillsKit
{
    /// <summary>
    ///     The async helpers.
    /// </summary>
    public static class AsyncHelpers
    {
        /// <summary>
        /// The run sync.
        /// </summary>
        /// <param name="task">
        /// The task.
        /// </param>
        public static void RunSync(System.Func<System.Threading.Tasks.Task> task)
        {
            var oldContext = System.Threading.SynchronizationContext.Current;
            var synch = new ExclusiveSynchronizationContext();
            System.Threading.SynchronizationContext.SetSynchronizationContext(synch);
            synch.Post(
                async _ =>
                    {
                        try
                        {
                            await task();
                        }
                        catch (System.Exception e)
                        {
                            synch.InnerException = e;
                            throw;
                        }
                        finally
                        {
                            synch.EndMessageLoop();
                        }
                    },
                null);
            synch.BeginMessageLoop();

            System.Threading.SynchronizationContext.SetSynchronizationContext(oldContext);
        }

        /// <summary>
        /// The run sync.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="task">
        /// The task.
        /// </param>
        /// <returns>
        /// The <see cref="T"/> .
        /// </returns>
        public static T RunSync<T>(System.Func<System.Threading.Tasks.Task<T>> task)
        {
            var oldContext = System.Threading.SynchronizationContext.Current;
            var synch = new ExclusiveSynchronizationContext();
            System.Threading.SynchronizationContext.SetSynchronizationContext(synch);
            T ret = default(T);
            synch.Post(
                async _ =>
                    {
                        try
                        {
                            ret = await task();
                        }
                        catch (System.Exception e)
                        {
                            synch.InnerException = e;
                            throw;
                        }
                        finally
                        {
                            synch.EndMessageLoop();
                        }
                    },
                null);
            synch.BeginMessageLoop();
            System.Threading.SynchronizationContext.SetSynchronizationContext(oldContext);
            return ret;
        }

        /// <summary>
        ///     The exclusive synchronization context.
        /// </summary>
        private class ExclusiveSynchronizationContext : System.Threading.SynchronizationContext
        {
            /// <summary>
            ///     The items.
            /// </summary>
            private readonly System.Collections.Generic.Queue<System.Tuple<System.Threading.SendOrPostCallback, object>> items =
                    new System.Collections.Generic.Queue
                        <System.Tuple<System.Threading.SendOrPostCallback, object>>();

            /// <summary>
            ///     The work <see cref="items" /> waiting.
            /// </summary>
            private readonly System.Threading.AutoResetEvent workItemsWaiting =
                new System.Threading.AutoResetEvent(false);

            /// <summary>
            ///     The done.
            /// </summary>
            private bool done;

            /// <summary>
            ///     Gets or sets the inner exception.
            /// </summary>
            public System.Exception InnerException { get; set; }

            /// <summary>
            ///     The begin message loop.
            /// </summary>
            /// <exception cref="AggregateException" />
            public void BeginMessageLoop()
            {
                while (!this.done)
                {
                    System.Tuple<System.Threading.SendOrPostCallback, object> task = null;
                    lock (this.items)
                    {
                        if (this.items.Count > 0)
                        {
                            task = this.items.Dequeue();
                        }
                    }

                    if (task != null)
                    {
                        task.Item1(task.Item2);
                        if (this.InnerException != null)
                        {
                            // the method threw an exeption
                            throw new System.AggregateException(
                                      "AsyncHelpers.Run method threw an exception.",
                                      this.InnerException);
                        }
                    }
                    else
                    {
                        this.workItemsWaiting.WaitOne();
                    }
                }
            }

            /// <summary>
            ///     The create copy.
            /// </summary>
            /// <returns>
            ///     The <see cref="SynchronizationContext" /> .
            /// </returns>
            public override System.Threading.SynchronizationContext CreateCopy()
            {
                return this;
            }

            /// <summary>
            ///     The end message loop.
            /// </summary>
            public void EndMessageLoop()
            {
                this.Post(_ => this.done = true, null);
            }

            /// <summary>
            /// The post.
            /// </summary>
            /// <param name="d">
            /// The d.
            /// </param>
            /// <param name="state">
            /// The state.
            /// </param>
            public override void Post(System.Threading.SendOrPostCallback d, object state)
            {
                lock (this.items)
                {
                    this.items.Enqueue(System.Tuple.Create(d, state));
                }

                this.workItemsWaiting.Set();
            }

            /// <summary>
            /// The send.
            /// </summary>
            /// <param name="d">
            /// The d.
            /// </param>
            /// <param name="state">
            /// The state.
            /// </param>
            /// <exception cref="NotSupportedException">
            /// </exception>
            public override void Send(System.Threading.SendOrPostCallback d, object state)
            {
                throw new System.NotSupportedException("We cannot send to our same thread");
            }
        }
    }
}