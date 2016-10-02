using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonarDatabase.Translate
{
    class UnitImp : IUnit
    {
        public UnitImp(TranslateImp trn, string uID)
        {
            translateImp = trn;
            unitID = uID;
            sourceText = new TextImp(this, TextType.Source);
            targetText = new TextImp(this, TextType.Target);
        }

        public IText this[TextType type]
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

        public bool IsModified
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ITranslate Translate
        {
            get
            {
                return translateImp;
            }
        }

        public string UnitID
        {
            get
            {
                return unitID;
            }
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        TranslateImp translateImp;
        string unitID;
        TextImp sourceText;
        TextImp targetText;
    }
}
