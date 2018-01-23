namespace Frederick.ProjectAircraft.Menu
{
    using Multiplay;
    using UnityEngine;

    /// <summary>
    /// 创建游戏菜单。
    /// </summary>
    public class CreateGameMenu : MonoBehaviour
    {
        public UILabel AddressLabel;
        public UILabel ClientCountLabel;
        public UISprite ReadySprite;
        public UIButton StartGameButton;
        public UISprite WaitSprite;

        /// <summary>
        /// 创建游戏。
        /// </summary>
        public void CreateGame()
        {
            MultiplayManager.Instance.CreateNewGame();
        }

        /// <summary>
        /// 开始游戏。
        /// </summary>
        public void StartGame()
        {
            MultiplayManager.Instance.StartGame();
        }

        protected void Start()
        {
            AddressLabel.text = Network.player.ipAddress;
        }

        protected void Update()
        {
            if (Network.connections.Length == 0)
            {
                WaitSprite.enabled = true;
                ReadySprite.enabled = false;
                ClientCountLabel.enabled = false;
                StartGameButton.isEnabled = false;
            }
            else
            {
                WaitSprite.enabled = false;
                ReadySprite.enabled = true;
                ClientCountLabel.enabled = true;
                ClientCountLabel.text = Network.connections.Length.ToString();
                StartGameButton.isEnabled = true;
            }
        }
    }
}