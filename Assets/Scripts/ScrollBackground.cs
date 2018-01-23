namespace Frederick.ProjectAircraft
{
    using UnityEngine;

    /// <summary>
    /// 滚动背景。
    /// </summary>
    public class ScrollBackground : MonoBehaviour
    {
        public GameObject BackgroundPrefab;
        public float Speed;

        protected void Awake()
        {
            mTransform = transform;
            var bg1 = (GameObject) Instantiate(BackgroundPrefab);
            bg1.transform.parent = mTransform;
            var bg2 = (GameObject) Instantiate(BackgroundPrefab);
            bg2.transform.parent = mTransform;
            bg2.transform.localPosition = new Vector3(0, 960);
        }

        protected void Update()
        {
            if (GameState.Instance.IsPaused)
                return;
            mOffset -= Time.deltaTime * Speed;
            if (mOffset <= -960)
                mOffset = 0;
            mTransform.localPosition = new Vector3(0, mOffset);
        }

        private float mOffset;
        private Transform mTransform;
    }
}