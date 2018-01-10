using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBits.Intuition.AbstractLanguageIntelligence.Model.ProbabilityClouds.Meshes
{
    class Test
    {
        public Test()
        {
            Test1();
            Test2();
        }


        private void Test1()
        {
            Logger.LoggerSAP.Log("Performing {0}().", "Test1");
            mesh1 = new SimpleMesh();
            //Enqueue a couple of nodes to be added to the mesh.
            mesh1.AddNode(new MeshNode.MeshNodeTemplate("Node 1"));
            mesh1.AddNode(new MeshNode.MeshNodeTemplate("Node 2"));
            mesh1.AddNode(new MeshNode.MeshNodeTemplate("Node 3"));
            mesh1.AddNode(new MeshNode.MeshNodeTemplate("Node 4"));
            mesh1.AddNode(new MeshNode.MeshNodeTemplate("Node 5"));
            mesh1.AddNode(new MeshNode.MeshNodeTemplate("Node 6"));
            mesh1.AddNode(new MeshNode.MeshNodeTemplate("Node 7"));
            mesh1.AddNode(new MeshNode.MeshNodeTemplate("Node 8"));
            mesh1.AddNode(new MeshNode.MeshNodeTemplate("Node 9"));
            mesh1.AddNode(new MeshNode.MeshNodeTemplate("Node 10"));
            mesh1.AddNode(new MeshNode.MeshNodeTemplate("Node 11"));
            mesh1.AddNode(new MeshNode.MeshNodeTemplate("Node 12"));
            mesh1.AddNode(new MeshNode.MeshNodeTemplate("Node 13"));
            mesh1.AddNode(new MeshNode.MeshNodeTemplate("Node 14"));
            mesh1.AddNode(new MeshNode.MeshNodeTemplate("Node 15"));
        }
        private void Test2()
        {
            string fileName = "MS-DOS.csv";
            Logger.LoggerSAP.Log("Performing {0}({1}).", "Test2", fileName);
            mesh2 = new EdbCsvMeshBuilder(fileName);
        }
        SimpleMesh mesh1;
        EdbCsvMeshBuilder mesh2;
    }
}
