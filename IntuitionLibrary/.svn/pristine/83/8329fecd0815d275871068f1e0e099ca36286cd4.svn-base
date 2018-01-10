using QBits.Intuition.Logger;
using QBits.Intuition.Xml;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;

namespace QBits.Intuition.AbstractLanguageIntelligence.Model.ProbabilityClouds.Meshes
{
    class EdbCsvMeshBuilder : SimpleMesh
    {
        public EdbCsvMeshBuilder(string fileName)
        {
            _csvEdbFileName = fileName;
        }
        protected override void MeshThreadEntryPoint(AbstractThreadedMesh threadParamObj)
        {
            _rank = 1; //Default dimensions (currently 1, since we don't have metric defined yet for edb terms)
            parser = new CSVAnalyser(this);
            ReadInput();
            while (isActive)
            {
                Thread.Sleep(10);
            }
        }
        class MyStreamReader : TextReader
        {
            public MyStreamReader(string path)
            {
                _path = path;
                _file = File.OpenRead(_path);
            }
            string _path;
            FileStream _file;
            public bool EndOfStream
            {
                get
                {
                    return _file.Position >= _file.Length;
                }
            }
            public override int Read()
            {
                return ConvertWindowsANSICharToUnicode((byte)_file.ReadByte());
            }

            private static char ConvertWindowsANSICharToUnicode(byte inByte)
            {
                byte[] inByteArray = new byte[1];
                inByteArray[0] = inByte;
                Encoding fileEncoding = Encoding.Default;
                Encoding unicode = Encoding.Unicode;
                byte[] outByteArray = Encoding.Convert(fileEncoding, unicode, inByteArray);
                //char[] unicodeChars = new char[unicode.GetCharCount(outByteArray)];
                char[] unicodeChars = unicode.GetChars(outByteArray);
                return unicodeChars[0];
            }
            System.Text.Decoder _d = Encoding.UTF8.GetDecoder();

            public override void Close()
            {
                _file.Close();
                base.Close();
            }
            public override string ReadLine()
            {
                StringBuilder retVal = new StringBuilder();
                bool fEndOfLineDetected = false;
                while ((!EndOfStream)
                    && (!fEndOfLineDetected))
                {
                    char nextChar = (char)Read();
                    switch (nextChar)
                    {
                        case '\r':
                            if (this.Peek().Equals('\n'))
                            {
                                //This is a DOS-like end of line.
                                Read(); //Consume the EOF character.
                                fEndOfLineDetected = true;
                            }
                            break;
                        case '\n': fEndOfLineDetected = true; break;
                        default:
                            retVal.Append(nextChar); break;
                    }
                }
                return retVal.ToString();
            }
        }
        private void ReadInput()
        {
            //Start reading our file.
            LoggerSAP.Log("Reading in: {0}...", _csvEdbFileName);
            MyStreamReader sr = new MyStreamReader(_csvEdbFileName);
            string csvLine;
            int lineCount = 0;
            while (!sr.EndOfStream)
            {
                csvLine = sr.ReadLine(); lineCount++;
                AddNode(new CSVMeshNode.CSVMeshNodeTemplate(csvLine)); //No need to explicitly wait for semaphore, because AddNode() already does this.
                Thread.SpinWait(1); //Give away a little of processor time.
            }
            sr.Close();
            LoggerSAP.Log("Finished reading {0} ({1} entries).", _csvEdbFileName, lineCount);
        }
        string _csvEdbFileName;
        CSVAnalyser parser;
        class CSVAnalyser : AbstractThreadedMesh
        {
            internal CSVAnalyser(EdbCsvMeshBuilder container)
            {
                _container = container;
            }
            protected override void MeshThreadEntryPoint(AbstractThreadedMesh threadParamObj)
            {
                //Message loop
                while (isActive)
                {
                    CheckInputQueue();
                }
            }

            private void CheckInputQueue()
            {
                _container.semaphore.WaitOne();
                int queueEntries = _container.newNodeQueue.Count;
                _container.semaphore.Release();
                if (queueEntries == 0)
                {
                    LoggerSAP.Log("Nothing to process. Going to sleep.");
                    sleepTime = 5000;
                }
                else
                {
                    LoggerSAP.Log("Wow, {0} csv entries to process! Getting down to it.", queueEntries);
                    _container.semaphore.WaitOne();
                    CSVMeshNode.CSVMeshNodeTemplate csvLine = (CSVMeshNode.CSVMeshNodeTemplate)_container.newNodeQueue.Dequeue();
                    _container.semaphore.Release();
                    CreateMeshNode(csvLine);
                }
            }

            private void CreateMeshNode(CSVMeshNode.CSVMeshNodeTemplate csvLine)
            {
                CSVMeshNode newNode = new CSVMeshNode(csvLine, _container);
                _container.semaphore.WaitOne();
                _container.nodeList.Add(newNode);
                _container.semaphore.Release();
                LoggerSAP.Log("Created mesh node named {0} (with {1} dimensions) from it's template.", newNode._name, newNode._x.rank);
            }
            EdbCsvMeshBuilder _container;
            new bool isActive
            {
                get
                {
                    Thread.Sleep(_sleepTime);
                    _sleepTime = 1;
                    return base.isActive;
                }
            }
            /// <summary>
            /// Adds more time (miliseconds) to processor sleeping, as requested in message loop,
            /// so that the thread sleeps more in the next iteration.
            /// </summary>
            int sleepTime
            {
                set
                {
                    _sleepTime += value;
                }
            }
            int _sleepTime = 0;
        }
        class CSVMeshNode : MeshNode
        {
            internal CSVMeshNode(CSVMeshNodeTemplate template, SimpleMesh mother)
                : base(template, mother)
            {
                _definition = _nodeCreator.CreateNode("node");
                XmlNode rawcontents = _nodeCreator.CreateChildNode(_definition, "raw");
                _nodeCreator.AddAttribute("type", "EdbCsvMeshBuilder", rawcontents);
                _nodeCreator.AddAttribute("source", template._name, rawcontents);
                XmlNode parsedContents = _nodeCreator.CreateChildNode(_definition, "parsed");
                string[] parsedContent = template._name.Split(',');
                _nodeCreator.AddAttribute("splitCount", parsedContent.Length.ToString(), parsedContents);
                foreach (string s in parsedContent)
                {
                    _nodeCreator.CreateTextNode(parsedContents, "parsed", s);
                }

                DebugDump();
            }
            /// <summary>
            /// Dumps the contents of the XML to file.
            /// </summary>
            private void DebugDump()
            {
                string nodeFname = "some.xml";
                string fullFname = "some1.xml";
                XmlTextWriter xtw = new XmlTextWriter(nodeFname, Encoding.Unicode);
                _definition.WriteTo(xtw);
                xtw.Flush();
                xtw.Close();
                //Now append the newly created xml dump to the pre-existing dump.
                StreamReader sr = new StreamReader(nodeFname);
                StreamWriter sw = new StreamWriter(fullFname, true);
                sw.Write(sr.ReadToEnd());
                sr.Close();
                sw.Flush();
                sw.Close();
            }
            /// <summary>
            /// All information related to this node.
            /// Includes raw data from the input file, parsed data, as well
            /// as all predicted traits on the node (term).
            /// </summary>
            internal XmlNode _definition;
            NonPersistentXMLDocument _nodeCreator = new NonPersistentXMLDocument("root");

            internal class CSVMeshNodeTemplate : MeshNodeTemplate
            {
                /// <summary>
                /// The nodeName of this template is the whole CSV string.
                /// Clumsy and probably will need remaking into something more readable.
                /// </summary>
                /// <param name="nodeName"></param>
                public CSVMeshNodeTemplate(string nodeName)
                    : base(nodeName)
                {
                }
            }
        }
    }
}
