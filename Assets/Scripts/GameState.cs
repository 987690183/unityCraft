namespace Frederick.ProjectAircraft
{
    using System;
    using Config;
    using UnityEngine;

    /// <summary>
    /// 游戏状态。
    /// </summary>
    public class GameState : MonoBehaviour
    {
        /// <summary>
        /// 当关卡变更时调用此方法。
        /// </summary>
        public event Action LevelChanged;

        /// <summary>
        /// 获取游戏状态的唯一实例。
        /// </summary>
        public static GameState Instance
        {
            get
            {
                if (mInstance == null)
                {
                    var root = new GameObject {name = "_GameState"};
                    DontDestroyOnLoad(root);
                    mInstance = root.AddComponent<GameState>();
                }
                return mInstance;
            }
        }

        /// <summary>
        /// 获取或设置当前关卡配置。
        /// </summary>
        public LevelConfig Level
        {
            get { return mLevel; }
            set
            {
                mLevel = value;
                mNextLevel = ConfigManager.Instance.GameConfig.GetLevelConfig(mLevel.ID + 1);
                if (LevelChanged != null)
                    LevelChanged();
                Debug.Log("Level changed to " + mLevel.ID);
            }
        }

        /// <summary>
        /// 获取或设置游戏的当前分数。
        /// </summary>
        public int Score
        {
            get { return mScore; }
            set
            {
                mScore = value;
                if (mNextLevel != null && mNextLevel.Score <= value)
                    Level = mNextLevel;
            }
        }

        public bool IsGameOver;
        public bool IsMultiplaying;
        public bool IsPaused;

        protected void Awake()
        {
            Level = ConfigManager.Instance.GameConfig.GetLevelConfig(1);
        }

        private static GameState mInstance;
        private LevelConfig mLevel;
        private LevelConfig mNextLevel;
        private int mScore;
    }
}