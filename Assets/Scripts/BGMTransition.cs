namespace Frederick.ProjectAircraft
{
    using UnityEngine;

    /// <summary>
    /// 背景音乐切换过渡效果。
    /// </summary>
    public class BGMTransition : MonoBehaviour
    {
        public AudioClip AudioClip;
        public bool IsLoop;

        protected void Start()
        {
            if (AudioClip == null)
                AudioManager.Instance.StopBGM();
            else
                AudioManager.Instance.PlayBGM(AudioClip, IsLoop);
        }
    }
}