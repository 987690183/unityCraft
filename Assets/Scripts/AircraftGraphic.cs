namespace Frederick.ProjectAircraft
{
    using System;
    using UnityEngine;

    /// <summary>
    /// 用于控制飞机效果的显示。
    /// </summary>
    public class AircraftGraphic : MonoBehaviour
    {
        /// <summary>
        /// 获取飞机图像的边界。
        /// </summary>
        public Bounds Bounds
        {
            get { return SpriteAnimator.Sprite.GetBounds(); }
        }

        public string ExplodeClipName;
        public AudioClip ExplodeSFX;
        public string IdleClipName;
        public Color RenderColor = Color.white;
        public tk2dSpriteAnimator SpriteAnimator;
        public string UnderAttackClipName;

        /// <summary>
        /// 爆炸。
        /// </summary>
        /// <param name="callback">爆炸效果结束后的回调</param>
        public void Explode(Action callback)
        {
            if (callback != null)
            {
                SpriteAnimator.AnimationCompleted = (animator, clip) =>
                {
                    callback();
                    SpriteAnimator.AnimationCompleted = null;
                };
            }
            SpriteAnimator.Play(mExplodeClip);
            AudioManager.Instance.PlaySFX(ExplodeSFX);
        }

        /// <summary>
        /// 爆炸。
        /// </summary>
        public void Explode()
        {
            Explode(null);
        }

        /// <summary>
        /// 显示受到攻击效果。
        /// </summary>
        public void UnderAttack()
        {
            if (mUnderAttackClip == null)
                return;
            SpriteAnimator.AnimationCompleted = (animator, clip) =>
            {
                SpriteAnimator.AnimationCompleted = null;
                SpriteAnimator.Play(mIdleClip);
            };
            SpriteAnimator.Play(mUnderAttackClip);
        }

        protected virtual void Awake()
        {
            mIdleClip = SpriteAnimator.GetClipByName(IdleClipName);
            mUnderAttackClip = SpriteAnimator.GetClipByName(UnderAttackClipName);
            mExplodeClip = SpriteAnimator.GetClipByName(ExplodeClipName);
            if (mIdleClip == null)
                Debug.LogWarning(string.Format("Idle clip '{0}' not exist.", IdleClipName));
            if (mExplodeClip == null)
                Debug.LogWarning(string.Format("Explode clip '{0}' not exist.", ExplodeClipName));
        }

        protected virtual void Update()
        {
            SpriteAnimator.Paused = GameState.Instance.IsPaused;
            SpriteAnimator.Sprite.color = RenderColor;
        }

        private tk2dSpriteAnimationClip mExplodeClip;
        private tk2dSpriteAnimationClip mIdleClip;
        private tk2dSpriteAnimationClip mUnderAttackClip;
    }
}