using System;
using System.Threading;

namespace Pathfinder
{
    /// <summary>
    /// Pool of finite resources, ensuring only one resource instance per thread.
    /// This class is thread safe.
    /// </summary>
    /// <typeparam name="T">Type of resource.</typeparam>
    public class ResourcePool<T>
    {
        /// <summary>
        /// Resource pool.
        /// </summary>
        private volatile T[] _resources;

        /// <summary>
        /// Initializes the resource pool.
        /// </summary>
        /// <param name="size">Max number of resources to allow.</param>
        public ResourcePool(int size)
        {
            // Allocate pool.
            _resources = new T[size];
        }

        /// <summary>
        /// Initializes the resource pool.
        /// </summary>
        /// <param name="resources">Resource instances to use.</param>
        public ResourcePool(T[] resources)
        {
            // Allocate pool.
            _resources = resources;
        }

        public T Acquire()
        {
            return Acquire(true);
        }

        /// <summary>
        /// Acquires the next available resource from the pool.
        /// </summary>
        /// <remarks>
        /// Must release this resource back to the pool when finished, using <cref>Release</cref>.
        /// </remarks>
        /// <param name="blockUntilAcquired">Blocks call until next resource is available.</param>
        /// <returns>Resource or default/null if pool is full and blockUntilAcquired is false.</returns>
        public T Acquire(bool blockUntilAcquired)
        {
            lock (this)
            {
                // Loop indefinitely if blocking was requested.
                while (blockUntilAcquired)
                {
                    // Iterate all resources.
                    foreach (T resource in _resources)
                    {
                        // Try to acquire lock on resource.
                        if (Monitor.TryEnter(resource))
                        {
                            // Lock succeded.
                            return resource;
                        }
                    }

                    // Sleep to prevent CPU spikes.
                    if (blockUntilAcquired)
                        Thread.Sleep(1);
                }

                return default(T);
            }
        }

        /// <summary>
        /// Releases acquired resource back to the pool.
        /// </summary>
        /// <param name="resource">Resource to release.</param>
        public void Release(T resource)
        {
            lock (this)
            {
                // Release lock on resource.
                Monitor.Exit(resource);
            }
        }
    }
}
