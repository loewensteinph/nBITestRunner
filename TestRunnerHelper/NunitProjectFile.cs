using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;

namespace TestrunnerHelper
{
    /// <summary>
    /// Handels modifactions at runtime 
    /// modifies the nunit file
    /// </summary>
    public class NunitProjectFile
    {
        /// <summary>
        /// Constructor Method
        /// </summary>
        /// <param name="newAppBase"></param>
        public NunitProjectFile(string newAppBase)
        {
            NewAppBase = newAppBase;
        }

        public string NewAppBase { get; set; }
        public string FullNameNunitProjectFilename { get; set; }

        /// <summary>
        /// Sets appabse and config file name accordingly
        /// </summary>
        /// <param name="relativePathToNBiConfigFile"></param>
        public void PrepareNunitProject(string relativePathToNBiConfigFile)
        {
            try
            {
                var doc = new XmlDocument();

                var streamreader = new StreamReader(FullNameNunitProjectFilename);
                doc.Load(streamreader);

                var nodeList = doc.SelectNodes("//Settings");
                if (nodeList != null)
                    foreach (XmlNode node in nodeList)
                        if (node.Attributes != null) node.Attributes["appbase"].Value = NewAppBase;

                nodeList = doc.SelectNodes("//Config");
                if (nodeList != null)
                    foreach (XmlNode node in nodeList)
                    {
                        if (node.Attributes != null) node.Attributes["configfile"].Value = relativePathToNBiConfigFile;
                        if (node.Attributes != null) node.Attributes["appbase"].Value = NewAppBase;
                    }
                doc.PreserveWhitespace = true;

                streamreader.Close();

                var wrtr = new XmlTextWriter(FullNameNunitProjectFilename, Encoding.Unicode);

                doc.WriteTo(wrtr);
                wrtr.Close();
            }
            catch (Exception ex)
            {
                Debug.Write("There wa a problem modifying the NUnit Config File: " + ex.Message);
            }
        }
    }
}