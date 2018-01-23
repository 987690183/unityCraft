namespace Frederick.ProjectAircraft
{
    using UnityEngine;

    /// <summary>
    /// 补给道具。
    /// </summary>
    public class Supply : MonoBehaviour
    {
        public AudioClip SupplySFX;

        protected void Awake()
        {
            mAnimation = GetComponent<Animation>();
        }

        /// <summary>
        /// 当道具碰到玩家飞机时调用此方法。
        /// </summary>
        ///<param name="other">飞机的碰撞器</param>
        protected virtual void OnSupply(Collider other) {}

        protected void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Player" || mIsDestoryed)
                return;
            AudioManager.Instance.PlaySFX(SupplySFX);
            OnSupply(other);
            destroy();
            mIsDestoryed = true;
        }

        protected void Update()
        {
            if (GameState.Instance.IsPaused)
            {
                mAnimation[mAnimation.clip.name].speed = 0;
                return;
            }
            mAnimation[mAnimation.clip.name].speed = 1.0f;
            if (!mAnimation.isPlaying)
                destroy();
        }

        private void destroy()
        {
            Destroy(transform.parent.gameObject);
        }

        private Animation mAnimation;
        private bool mIsDestoryed;
    }
}