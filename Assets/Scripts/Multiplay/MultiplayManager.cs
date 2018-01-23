namespace Frederick.ProjectAircraft.Multiplay
{
    using System;
    using Config;
    using UnityEngine;

    /// <summary>
    /// 多人游戏管理器。
    /// </summary>
    public class MultiplayManager : MonoBehaviour
    {
        /// <summary>
        /// 获取管理器的唯一实例。
        /// </summary>
        public static MultiplayManager Instance
        {
            get
            {
                if (mInstance == null)
                {
                    var root = new GameObject("_MultiplayManager");
                    DontDestroyOnLoad(root);
                    mInstance = root.AddComponent<MultiplayManager>();
                    mInstance.mNetwork = root.AddComponent<NetworkView>();
                }
                return mInstance;
            }
        }

        /// <summary>
        /// 创建新游戏。
        /// </summary>
        public void CreateNewGame()
        {
            var error = Network.InitializeServer(mMaxPlayerCount - 1, mListenPort, false);
            Debug.Log("Create new game:" + error);
        }

        /// <summary>
        /// 游戏结束。
        /// </summary>
        public void GameOver()
        {
            GetComponent<NetworkView>().CallAtAll(gameOver);
        }

        /// <summary>
        /// 获取指定玩家的控制器编号。
        /// </summary>
        /// <param name="player">玩家</param>
        /// <returns>控制器编号</returns>
        public PlayerIndex GetPlayerIndex(NetworkPlayer player)
        {
            return (PlayerIndex) int.Parse(player.ToString());
        }

        /// <summary>
        /// 加入游戏。
        /// </summary>
        /// <param name="ip">主机地址</param>
        public NetworkConnectionError JoinGame(string ip)
        {
            var error = Network.Connect(ip, mListenPort);
            Debug.Log("join game:" + error);
            return error;
        }

        /// <summary>
        /// 暂停游戏。
        /// </summary>
        public void PauseGame()
        {
            if (GameState.Instance.IsGameOver)
                return;
            mNetwork.CallAtAll(pauseGame);
        }

        /// <summary>
        /// 重开游戏。
        /// </summary>
        public void RestartGame()
        {
            mNetwork.CallAtAll(restartGame);
        }

        /// <summary>
        /// 继续游戏。
        /// </summary>
        public void ResumeGame()
        {
            mNetwork.CallAtAll(resumeGame);
        }

        /// <summary>
        /// 返回到主菜单。
        /// </summary>
        public void ReturnToMainMenu()
        {
            switch (Network.peerType)
            {
                case NetworkPeerType.Client:
                case NetworkPeerType.Connecting:
                    Network.Disconnect();
                    exitToMainMenu();
                    break;
                case NetworkPeerType.Server:
                    mNetwork.CallAtAll(exitToMainMenu);
                    break;
                default:
                    exitToMainMenu();
                    break;
            }
        }

        /// <summary>
        /// 开始游戏。
        /// </summary>
        public void StartGame()
        {
            if (Network.peerType != NetworkPeerType.Server)
                throw new InvalidOperationException("Master server only.");
            mNetwork.RPC(startMultiplay, RPCMode.All);
        }

        /// <summary>
        /// 当有新玩家连接时调用此方法。
        /// </summary>
        /// <param name="player">玩家</param>
        protected void OnPlayerConnected(NetworkPlayer player)
        {
            Debug.Log(player + " connected.");
        }

        /// <summary>
        /// 当有玩家断开连接时调用此方法。
        /// </summary>
        /// <param name="player">玩家</param>
        protected void OnPlayerDisconnected(NetworkPlayer player)
        {
            Debug.Log(player + "disconnected.");
            Network.RemoveRPCs(player);
            Network.DestroyPlayerObjects(player);
        }

        [RPC]
        private void endGame()
        {
            GameState.Instance.IsGameOver = false;
            GameState.Instance.IsPaused = false;
            GameState.Instance.Score = 0;
        }

        [RPC]
        private void exitToMainMenu()
        {
            endGame();
            Application.LoadLevel("MainMenu");
        }

        [RPC]
        private void gameOver()
        {
            var bodies = GameObject.FindGameObjectsWithTag("Player");
            foreach (var body in bodies)
            {
                var aircraft = body.GetComponent<AircraftPart>();
                if (aircraft.Aircraft.IsAlive)
                    return;
            }
            GameState.Instance.IsGameOver = true;
        }

        [RPC]
        private void pauseGame()
        {
            GameState.Instance.IsPaused = true;
        }

        [RPC]
        private void restartGame()
        {
            GameState.Instance.IsGameOver = false;
            GameState.Instance.IsPaused = false;
            GameState.Instance.Score = 0;
            GameState.Instance.Level = ConfigManager.Instance.GameConfig.GetLevelConfig(1);
            Application.LoadLevel("GamePlay");
        }

        [RPC]
        private void resumeGame()
        {
            GameState.Instance.IsPaused = false;
        }

        [RPC]
        private void startMultiplay()
        {
            GameState.Instance.IsMultiplaying = true;
            Application.LoadLevel("GamePlay");
        }

        private const int mListenPort = 8921;
        private const int mMaxPlayerCount = 4;
        private static MultiplayManager mInstance;
        private NetworkView mNetwork;
    }
}