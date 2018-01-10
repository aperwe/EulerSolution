using QBits.Intuition.Logger;
using System;
using System.Collections.Generic;
using System.Threading;

namespace QBits.Intuition.AbstractLanguageIntelligence.Model.ProbabilityClouds.Meshes
{
    class SimpleMesh : AbstractThreadedMesh
    {
        protected override void MeshThreadEntryPoint(AbstractThreadedMesh threadParamObj)
        {
            _rank = 3; //Default dimensions (3)
            //Message loop.
            while (isActive) //Message loop
            {
                Thread.Sleep(5000);
                ProcessNodeQueue(newNodeQueue);
            }
        }
        /// <summary>
        /// Processes one entry from the node queue (if the queue is non-empty).
        /// </summary>
        /// <param name="newNodeQueue">Queue that is to be processed.</param>
        /// <returns>True if the queue needs more processing.</returns>
        private bool ProcessNodeQueue(Queue<MeshNode.MeshNodeTemplate> newNodeQueue)
        {
            if (newNodeQueue.Count == 0) return false; //Nothing to process
            semaphore.WaitOne();
            MeshNode newNode = new MeshNode(newNodeQueue.Dequeue(), this);
            nodeList.Add(newNode);
            LoggerSAP.Log("Created mesh node named {0} (with {1} dimensions) from it's template.", newNode._name, newNode._x.rank);
            semaphore.Release();
            return true;
        }
        /// <summary>
        /// This mesh rank (number of dimensions).
        /// </summary>
        protected int _rank;
        internal Coordinate CreateNewCoordinate()
        {
            Coordinate retVal = new Coordinate(_rank);
            return retVal;
        }
        protected Queue<MeshNode.MeshNodeTemplate> newNodeQueue = new Queue<MeshNode.MeshNodeTemplate>();
        protected List<MeshNode> nodeList = new List<MeshNode>();
        internal void AddNode(MeshNode.MeshNodeTemplate node)
        {
            semaphore.WaitOne();
            newNodeQueue.Enqueue(node);
            semaphore.Release();
        }
    }
}
