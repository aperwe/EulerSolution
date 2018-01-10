using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBits.Intuition.AbstractLanguageIntelligence.Model.ProbabilityClouds.Meshes
{
    class MeshNode
    {
        internal MeshNode(MeshNodeTemplate template, SimpleMesh mother)
        {
            _name = template._name;
            _mother = mother;
            _x = _mother.CreateNewCoordinate();
        }
        /// <summary>
        /// Mesh this node belongs to.
        /// </summary>
        SimpleMesh _mother;
        public Coordinate _x;
        public string _name;
        public class MeshNodeTemplate
        {
            public MeshNodeTemplate(string nodeName)
            {
                _name = nodeName;
            }
            internal string _name;
        }
    }
}
