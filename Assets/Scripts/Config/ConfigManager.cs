namespace Frederick.ProjectAircraft.Config
{
    using System.Linq;
    using UnityEngine;

    /// <summary>
    /// 游戏配置管理器。
    /// </summary>
    public class ConfigManager : MonoBehaviour
    {
        /// <summary>
        /// 获取当前游戏配置。
        /// </summary>
        public GameConfig GameConfig { get; private set; }

        /// <summary>
        /// 获取配置管理器的唯一实例。
        /// </summary>
        public static ConfigManager Instance
        {
            get
            {
                if (mInstance == null)
                {
                    var root = new GameObject("_ConfigManager");
                    DontDestroyOnLoad(root);
                    mInstance = root.AddComponent<ConfigManager>();
                    mInstance.Load();
                }
                return mInstance;
            }
        }

        /// <summary>
        /// 获取指定分数所对应的关卡配置。
        /// </summary>
        /// <param name="score">分数</param>
        /// <returns>关卡配置</returns>
        public LevelConfig GetLevelByScore(int score)
        {
            var query = from level in GameConfig.Levels
                        where level.Score <= score
                        orderby level.Score ascending
                        select level;
            var ret = query.Last();
            return ret;
        }

        /// <summary>
        /// 加载本地游戏配置。
        /// </summary>
        public void Load()
        {
            var asset = Resources.Load("Config/" + mConfigFileName) as TextAsset;
            if (asset == null)
            {
                Debug.LogWarning("gameconfig not exist.");
                return;
            }
			// TextAsset 如果是二进制数据，就是asset.bytes
            GameConfig = XmlConvert.DeserializeObject<GameConfig>(asset.text);
        }

        private const string mConfigFileName = "gameconfig";
        private static ConfigManager mInstance;
    }
}