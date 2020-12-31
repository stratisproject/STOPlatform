using Newtonsoft.Json;

namespace Stratis.STOPlatform.Data.Docs
{
    public class Document
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Json { get; private set; } = "{}";
        private object doc;

        public Document()
        {

        }

        public Document(object doc)
        {
            this.doc = doc;
            Key = doc.GetType().Name;
            Update();
        }

        public T As<T>() => (T)(doc ??= JsonConvert.DeserializeObject<T>(Json));

        internal void Update()
        {
            if (doc != null)
                Json = JsonConvert.SerializeObject(doc, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }
    }
}
