namespace Frederick.NGUIExtension
{
    using UnityEngine;

    /// <summary>
    /// <see cref="UIRoot"/>扩展，用于适配屏幕。
    /// </summary>
    [RequireComponent(typeof (UIRoot))]
    [ExecuteInEditMode]
    public class UIRootExtension : MonoBehaviour
    {
        public int GameHeight = 960;
        public int GameWidth = 640;

        protected void Awake()
        {
            mUIRoot = GetComponent<UIRoot>();
        }

        protected void Update()
        {
            if (GameHeight < GameWidth || mUIRoot.scalingStyle == UIRoot.Scaling.PixelPerfect)
                return;
            if (Screen.height < Screen.width)
            {
                mUIRoot.manualHeight = GameHeight;
                return;
            }
            var desireWidth = Screen.height * GameWidth / (float) GameHeight;
            var desireScale = desireWidth / Screen.width;
            mUIRoot.manualHeight = (int) (GameHeight * desireScale);
        }

        private UIRoot mUIRoot;
    }
}