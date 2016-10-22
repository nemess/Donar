using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Donar.Interfaces;
using System.Runtime.Serialization.Json;

namespace JsonDatabase
{
    class UnitImp : IUnit
    {
        public UnitImp(TranslateImp trn, string uID)
        {
            translateImp = trn;
            unitID = uID;
            sourceText = new TextEntryImp(this, TextType.Source);
            targetText = new TextEntryImp(this, TextType.Target);
        }

        #region IUnit implementation
        ITextEntry IUnit.this[TextType type]
        {
            get
            {
                switch (type)
                {
                    case TextType.Source:
                        return sourceText;
                    case TextType.Target:
                        return targetText;
                }
                throw new ArgumentOutOfRangeException("type", type, "Invalid text type!");
            }
        }

        bool IUnit.IsModified
        {
            get { return modified; }
        }

        ITranslate IUnit.Translate
        {
            get { return translateImp; }
        }

        string IUnit.UnitID
        {
            get { return unitID; }
        }

        //void IUnit.Save()
        //{
        //    Save();
        //}
        #endregion

        public bool IsModified
        {
            get { return modified; }
            set
            {
                modified = value;
                if (value) translateImp.IsModified = true;
            }
        }

        #region Public functions
        public void Save(FileStream sw)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(TextEntryJson));
            foreach (TextType tp in Enum.GetValues(typeof(TextType)))
            {
                TextEntryImp tei = (TextEntryImp)(this as IUnit)[tp];
                if (tei.IsModified)
                {
                    TextEntryJson tej = tei.ToJsonEntry();
                    TextEntryJson.WriteObject(sw, tej);
                    sw.WriteByte((byte)'\n');
                    tei.IsModified = false;
                }
            }
        }
        public void SetJsonRecord(TextEntryJson record)
        {
            ((TextEntryImp)(this as IUnit)[record.type]).SetAllParagraphes(record.lines);
        }
        #endregion

        #region Provate variables
        TranslateImp translateImp;
        string unitID;
        bool modified = false;
        TextEntryImp sourceText;
        TextEntryImp targetText;
        #endregion
    }
}
