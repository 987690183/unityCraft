namespace Frederick.ProjectAircraft
{
    using Multiplay;
    using UnityEngine;

    /// <summary>
    /// 连接面板。
    /// </summary>
    public class ConnectionPanel : MonoBehaviour
    {
        public UIInput AddressInput;

        /// <summary>
        /// 创建游戏。
        /// </summary>
        public void CreateGame()
        {
            Application.LoadLevel("CreateGame");
        }

        /// <summary>
        /// 加入游戏。
        /// </summary>
        public void JoinGame()
        {
            MultiplayManager.Instance.JoinGame(AddressInput.label.text);
        }

        /// <summary>
        /// 单人游戏。
        /// </summary>
        public void SingleGame()
        {
            Application.LoadLevel("GamePlay");
        }
    }
}