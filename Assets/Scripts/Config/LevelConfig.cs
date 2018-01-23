namespace Frederick.ProjectAircraft.Config
{
    using System.Xml.Serialization;

    /// <summary>
    /// 关卡配置。
    /// </summary>
    public class LevelConfig
    {
        /// <summary>
        /// 获取或设置该关卡所有敌机生成概率权重。
        /// </summary>
        [XmlArray("Enemies")]
        [XmlArrayItem("Enemy")]
        public EnemyConfig[] Enemies { get; set; }

        /// <summary>
        /// 获取或设置敌机生成的时间间隔。
        /// </summary>
        [XmlAttribute("EnemySpawnTime")]
        public float EnemySpawnTime { get; set; }

        /// <summary>
        /// 获取或设置关卡编号。
        /// </summary>
        [XmlAttribute("ID")]
        public int ID { get; set; }

        /// <summary>
        /// 获取或设置进入关卡所达到的最低分数。
        /// </summary>
        [XmlAttribute("Score")]
        public int Score { get; set; }
    }
}