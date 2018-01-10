using QBits.Intuition.Logger;
using System;
using System.Threading;

namespace QBits.Intuition.AbstractLanguageIntelligence.Model.ProbabilityClouds.Meshes
{
    /// <summary>
    /// A base skeleton for a mesh class supporting multithreading.
    /// Derive a class from this class and override the MeshThreadEntryPoint() where your
    /// class will enter as a separate thread.
    /// </summary>
    abstract class AbstractThreadedMesh
    {
        /// <summary>
        /// The base constructor takes care of creating the threaded mesh for you.
        /// </summary>
        protected AbstractThreadedMesh()
        {
            //Create the context thread that will run until this class is not destroyed.
            LoggerSAP.Log("Creating abstract mesh.");
            semaphore = new Semaphore(1, 1, _nextSemaphoreId()); //Unique name for the semaphore.
            meshThread = new Thread(new ParameterizedThreadStart(DefaultSafeThreadStarter));
            meshThread.IsBackground = true; //Set this thread as background
            meshThread.Name = _nextSemaphoreId(); //Name the thread so that we can identify it later.
            meshThread.Priority = ThreadPriority.BelowNormal; //Lower the priority of the thread.
            meshThread.Start(this);
        }
        /// <summary>
        /// Provides catching top-level exceptions from the thread, so that the application doesn't crash
        /// when specific thread doesn't catch all it's exceptions.
        /// </summary>
        /// <param name="threadParamObj">Object from ParametrizedThreadStart. It is ourselves, actually.</param>
        private void DefaultSafeThreadStarter(object threadParamObj)
        {
            try
            {
                string className = GetType().Name;
                LoggerSAP.Log("{0} ({1}) thread was born.", Thread.CurrentThread.Name, className);
                Thread.Sleep(2); //Wait for the framework to initialize 'this' object. Without this wait, members assigned to in constructor may not be initialized.
                AbstractThreadedMesh param = (AbstractThreadedMesh)threadParamObj;
                MeshThreadEntryPoint(param);
                LoggerSAP.Log("{0} ({1}) thread has died.", Thread.CurrentThread.Name, className);
            }
            catch (Exception ex)
            {
                LoggerSAP.Log("The '{0} thread didn't catch its exception. The thread has died. Exception message: {1}.",
                    Thread.CurrentThread.Name,
                    ex.Message);
            }
        }
        abstract protected void MeshThreadEntryPoint(AbstractThreadedMesh threadParamObj);
        protected Semaphore semaphore = null;
        Thread meshThread = null;
        /// <summary>
        /// Indicates whether the thread of this mesh is running or if it should be stopped.
        /// Setting this flag to false will close the thread as soon as it passes around its message loop.
        /// </summary>
        protected bool isActive = true;
        static int _semaphoreID = 0;
        /// <summary>
        /// Gets unique name for each semaphore.
        /// </summary>
        /// <returns>Name for the next newly created semaphore.</returns>
        static string _nextSemaphoreId()
        {
            return string.Format("Mesh {0}", _semaphoreID++);
        }
    }
}
