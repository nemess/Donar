using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Donar.Interfaces;

namespace DonarTest
{
    [TestClass]
    public class DatabaseImpTest
    {
        string testFolder;
        IDatabase iDatabase;

        string projectName = "project name";
        string defexport = "Defualt Export name";
        string defimport = "Defualt Import name";

        [TestInitialize]
        public void TestInitialize()
        {
            testFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(testFolder);
            iDatabase = new JsonDatabase.DatabaseImp();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            iDatabase.Close();
            Directory.Delete(testFolder, true);
        }


        [TestMethod]
        public void TestDefaultProperties()
        {
            // Than
            Assert.IsFalse(iDatabase.IsOpen);
            Assert.IsFalse(iDatabase.IsModified);
            Assert.IsNull(iDatabase.Name);
            Assert.IsNull(iDatabase.Folder);
            Assert.IsNull(iDatabase.DefaultExport);
            Assert.IsNull(iDatabase.DefaultImport);
        }

        [TestMethod]
        public void TestProperties()
        {
            // When
            iDatabase.Create(testFolder, projectName);
            // Than
            Assert.IsTrue(iDatabase.IsOpen);
            Assert.IsFalse(iDatabase.IsModified);
            Assert.AreEqual(iDatabase.Name, projectName);
            Assert.AreEqual(iDatabase.Folder, testFolder);
            Assert.AreEqual(iDatabase.DefaultExport, "");
            Assert.AreEqual(iDatabase.DefaultImport, "");
        }

        [TestMethod]
        public void SaveProperties()
        {
            // Given
            iDatabase.Create(testFolder, projectName);
            iDatabase.DefaultExport = defexport;
            iDatabase.DefaultImport = defimport;
            Assert.AreEqual(iDatabase.DefaultExport, defexport);
            Assert.AreEqual(iDatabase.DefaultImport, defimport);
            Assert.IsTrue(iDatabase.IsModified);
            iDatabase.SaveAll();
            iDatabase.Close();
            // When 
            iDatabase.Open(testFolder);
            // Than
            Assert.AreEqual(iDatabase.Name, projectName);
            Assert.AreEqual(iDatabase.Folder, testFolder);
            Assert.AreEqual(iDatabase.DefaultExport, defexport);
            Assert.AreEqual(iDatabase.DefaultImport, defimport);
        }

        string unitID = "testunitID";
        [TestMethod]
        public void AddTextUnit()
        {
            // Given
            iDatabase.Create(testFolder, projectName);
            //When
            iDatabase.Translate.Add(unitID);
            // Than
            Assert.IsTrue(iDatabase.IsModified);
            Assert.IsTrue(iDatabase.Translate.IsModified);
            IUnit unit = iDatabase.Translate[unitID];
            Assert.IsFalse(unit.IsModified);
            Assert.IsNotNull(unit);
            Assert.IsNotNull(unit[TextType.Source]);
            Assert.AreEqual(unit[TextType.Source].Count, 0);
            Assert.IsFalse(unit[TextType.Source].IsModified);
            Assert.IsNotNull(unit[TextType.Target]);
            Assert.AreEqual(unit[TextType.Target].Count, 0);
            Assert.IsFalse(unit[TextType.Target].IsModified);
        }

        [TestMethod]
        public void AddUnitAndText()
        {
            // Given
            iDatabase.Create(testFolder, projectName);
            iDatabase.Translate.Add(unitID);
            //When
            ITextEntry src = iDatabase.Translate[unitID][TextType.Source];
            src.FromString("Multi\nLine\nExample");
            // Than
            Assert.IsTrue(iDatabase.Translate[unitID].IsModified);
            Assert.IsTrue(src.IsModified);
            Assert.AreEqual(src.Count, 3);
            Assert.IsTrue(src[0].IsModified);
            Assert.AreEqual(src[0].Text, "Multi");
            Assert.IsTrue(src[1].IsModified);
            Assert.AreEqual(src[1].Text, "Line");
            Assert.IsTrue(src[2].IsModified);
            Assert.AreEqual(src[2].Text, "Example");
        }
        [TestMethod]
        public void SaveDataAndLoad()
        {
            // Given
            iDatabase.Create(testFolder, projectName);
            iDatabase.Translate.Add(unitID);
            iDatabase.Translate[unitID][TextType.Source].FromString("Multi\nLine\nExample");
            iDatabase.Translate[unitID][TextType.Target].FromString("Több\nSoros\nPélda");
            iDatabase.SaveAll();
            iDatabase.Close();
            //When
            iDatabase.Open(testFolder);
            //Than
            ITextEntry src = iDatabase.Translate[unitID][TextType.Source];
            Assert.IsFalse(iDatabase.Translate[unitID].IsModified);
            Assert.IsFalse(src.IsModified);
            Assert.AreEqual(src.Count, 3);
            Assert.IsFalse(src[0].IsModified);
            Assert.AreEqual(src[0].Text, "Multi");
            Assert.IsFalse(src[1].IsModified);
            Assert.AreEqual(src[1].Text, "Line");
            Assert.IsFalse(src[2].IsModified);
            Assert.AreEqual(src[2].Text, "Example");

            ITextEntry trg = iDatabase.Translate[unitID][TextType.Target];
            Assert.IsFalse(trg.IsModified);
            Assert.AreEqual(trg.Count, 3);
            Assert.IsFalse(trg[0].IsModified);
            Assert.AreEqual(trg[0].Text, "Több");
            Assert.IsFalse(trg[1].IsModified);
            Assert.AreEqual(trg[1].Text, "Soros");
            Assert.IsFalse(trg[2].IsModified);
            Assert.AreEqual(trg[2].Text, "Példa");
        }
    }
}
