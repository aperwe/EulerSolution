using QBits.Intuition.AbstractLanguageIntelligence.Model.ProbabilityClouds.Meshes;
using QBits.Intuition.Logger;
using System;
using System.Threading;

namespace QBits.Intuition.AbstractLanguageIntelligence.Model
{
    /// <remarks>
    /// Context keeps track of current context, for example:
    /// - what is 'it' we are talking about.
    /// - who 'me' that is speaking
    /// - who is 'he' we may be talking to
    /// - etc.
    /// </remarks>
    internal class Context
    {
        internal Context()
        {
            //Create the context thread that will run until this class is not destroyed.
            LoggerSAP.Log("Creating ALIContext.");
            contextThread = new Thread(new ParameterizedThreadStart(ContextThreadEntryPoint));
            contextThread.IsBackground = true; //Set this thread as background
            contextThread.Name = "ALIContext"; //Name the thread so that we can identify it later.
            contextThread.Priority = ThreadPriority.BelowNormal; //Lower the priority of the thread.
            contextThread.Start(this);
        }
        /// <summary>
        /// The entry point for the context thread.
        /// </summary>
        /// <param name="threadParamObj"></param>
        private void ContextThreadEntryPoint(object threadParamObj)
        {
            LoggerSAP.Log(string.Format("ALIContext thread has started. Thread name: {0}", Thread.CurrentThread.Name));
            Context threadParam = (Context)threadParamObj;
            while (threadParam.isActive) //Message loop
            {
                //Do some useful work here
                LoggerSAP.Log("Context working...");
                Thread.Sleep(5000);
                if (threadParam.meshTest == null)
                {
                    threadParam.meshTest = new Test();
                }
            }
            LoggerSAP.Log(string.Format("ALIContext thread has finished. Thread name: {0}", Thread.CurrentThread.Name));
        }
        Thread contextThread = null;
        Test meshTest = null;
        bool isActive = true; //Mark the thread as active (it will continue running until this flag is set to true).
    }
}
