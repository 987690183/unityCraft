namespace Frederick.ProjectAircraft.Config
{
    using System.Linq;
    using System.Xml.Serialization;

    /// <summary>
    /// 游戏配置。
    /// </summary>
    public class GameConfig
    {
        /// <summary>
        /// 获取或设置关卡设置。
        /// </summary>
        [XmlArray("Levels")]
        [XmlArrayItem("Level")]
        public LevelConfig[] Levels { get; set; }

        /// <summary>
        /// 获取指定编号的关卡配置。
        /// </summary>
        /// <param name="levelID">关卡编号</param>
        /// <returns>关卡配置</returns>
        public LevelConfig GetLevelConfig(int levelID)
        {
            var query = from level in Levels
                        where level.ID == levelID
                        select level;
            var ret = query.SingleOrDefault();
            return ret;
        }
    }
}