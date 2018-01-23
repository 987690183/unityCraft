namespace Frederick.ProjectAircraft
{
    using Multiplay;
    using UnityEngine;

    /// <summary>
    /// 游戏结果分数面板。
    /// </summary>
    public class GameResultPanel : MonoBehaviour
    {
        public UILabel ScoreLabel;

        /// <summary>
        /// 重新开始游戏。
        /// </summary>
        public void Restart()
        {
            MultiplayManager.Instance.RestartGame();
        }

        /// <summary>
        /// 显示游戏分数。
        /// </summary>
        /// <param name="score">分数</param>
        public void Show(int score)
        {
            ScoreLabel.text = score.ToString();
            gameObject.SetActive(true);
        }
    }
}