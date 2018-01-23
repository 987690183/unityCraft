namespace Frederick
{
    using System;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;

    /// <summary>
    /// Xml转换器。
    /// </summary>
    public static class XmlConvert
    {
        #region Public methods

        /// <summary>
        /// 将Xml反序列化为指定类型的对象。
        /// </summary>
        /// <typeparam name="T">需要反序列化的类型</typeparam>
        /// <param name="xml">Xml文本</param>
        /// <returns>反序列化后的对象</returns>
        public static T DeserializeObject<T>(string xml)
        {
            if (xml == null)
                throw new ArgumentNullException("xml");
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            {
                var serializer = new XmlSerializer(typeof (T));
                var obj = (T) serializer.Deserialize(stream);
                return obj;
            }
        }

        /// <summary>
        /// 将指定对象序列化为Xml。
        /// </summary>
        /// <typeparam name="T">序列化对象类型</typeparam>
        /// <param name="obj">序列化对象</param>
        /// <returns>序列化后的Xml结果</returns>
        public static string SerializeObject<T>(T obj)
        {
            if (Equals(obj, default(T)))
                return string.Empty;
            using (var stream = new MemoryStream())
            {
                var serializer = new XmlSerializer(typeof (T));
                serializer.Serialize(stream, obj);
                stream.Seek(0, SeekOrigin.Begin);
                var reader = new StreamReader(stream);
                var xml = reader.ReadToEnd();
                return xml;
            }
        }

        #endregion
    }
}