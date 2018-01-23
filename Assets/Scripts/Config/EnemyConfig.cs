namespace Frederick.ProjectAircraft.Config
{
    using System.Xml.Serialization;

    /// <summary>
    /// 敌机生成设置。
    /// </summary>
    public class EnemyConfig
    {
        /// <summary>
        /// 获取或设置敌机预设的索引值。
        /// </summary>
        [XmlAttribute("Index")]
        public int Index { get; set; }

        /// <summary>
        /// 获取或设置敌机最高速度。
        /// </summary>
        [XmlAttribute("MaxSpeed")]
        public float MaxSpeed { get; set; }

        /// <summary>
        /// 获取或设置敌机最低速度。
        /// </summary>
        [XmlAttribute("MinSpeed")]
        public float MinSpeed { get; set; }

        /// <summary>
        /// 获取或设置随机到该配置的概率权重。
        /// </summary>
        [XmlAttribute("Weight")]
        public int Weight { get; set; }
    }
}