namespace Frederick.ProjectAircraft
{
    using System.Collections;
    using Multiplay;
    using UnityEngine;

    /// <summary>
    /// 游戏界面。
    /// </summary>
    public class GameUI : MonoBehaviour
    {
        public UILabel BombLabel;
        public UISprite BombSprite;
        public GameResultPanel GameResultPanel;
        public UIPanel MenuPanel;
        public UILabel ScoreLabel;

        /// <summary>
        /// 暂停游戏。
        /// </summary>
        public void Pause()
        {
            MultiplayManager.Instance.PauseGame();
        }

        /// <summary>
        /// 重新开始游戏。
        /// </summary>
        public void Restart()
        {
            MultiplayManager.Instance.RestartGame();
        }

        /// <summary>
        /// 继续游戏。
        /// </summary>
        public void Resume()
        {
            MultiplayManager.Instance.ResumeGame();
        }

        /// <summary>
        /// 返回主菜单。
        /// </summary>
        public void ReturnToMainMenu()
        {
            MultiplayManager.Instance.ReturnToMainMenu();
        }

        /// <summary>
        /// 使用炸弹。
        /// </summary>
        public void UseBomb()
        {
            mAircraftController.LaunchBomb();
        }

        protected void Start()
        {
            var body = GameObject.FindGameObjectWithTag("Player");
            var part = body.GetComponent<AircraftPart>();
            mAircraftController = part.Aircraft.GetComponent<AircraftController>();
        }

        protected void Update()
        {
            ScoreLabel.text = GameState.Instance.Score.ToString();
            MenuPanel.gameObject.SetActive(GameState.Instance.IsPaused);
            if (mAircraftController.Bomb > 0)
            {
                BombSprite.enabled = true;
                BombLabel.enabled = true;
                BombLabel.text = string.Format("x{0}", mAircraftController.Bomb);
            }
            else
            {
                BombSprite.enabled = false;
                BombLabel.enabled = false;
            }

            if (GameState.Instance.IsGameOver && !mIsPendingGameResult)
                StartCoroutine(showGameResult());
        }

        private IEnumerator showGameResult()
        {
            mIsPendingGameResult = true;
            yield return new WaitForSeconds(1.0f);
            GameResultPanel.Show(GameState.Instance.Score);
        }

        private AircraftController mAircraftController;
        private bool mIsPendingGameResult;
    }
}