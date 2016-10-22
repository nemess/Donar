using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Donar.Interfaces;

namespace JsonDatabase
{
    [DataContract]
    class BaseJson<T>
    {
        static DataContractJsonSerializer JsonSerializer = new DataContractJsonSerializer(typeof(T));

        static public T ReadObject(Stream stream)
        {
            return (T)JsonSerializer.ReadObject(stream);
        }
        static public void WriteObject(Stream stream, T obj)
        {
            JsonSerializer.WriteObject(stream, obj);
        }
    }

    [DataContract]
    class MainRecordJson : BaseJson<MainRecordJson>
    {
        [DataMember]
        public string ProjectName;
        [DataMember]
        public string DefaultImport;
        [DataMember]
        public string DefaultExport;
    }

    [DataContract]
    class TextEntryJson : BaseJson<TextEntryJson>
    {
        [DataMember]
        public string ID;
        [DataMember]
        public TextType type;
        [DataMember]
        public string[] lines;
    }
}