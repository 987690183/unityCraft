namespace Frederick.ProjectAircraft
{
    using UnityEngine;

    /// <summary>
    /// 敌方飞机控制器。
    /// </summary>
    public class EnemyAircraftController : MonoBehaviour
    {
        public float SpeedRate = 1.0f;
        public float StantardSpeed = 100;

        protected void Awake()
        {
            mTransform = transform;
            mAircraft = GetComponent<Aircraft>();
        }

        protected void Update()
        {
            if (GameState.Instance.IsPaused)
                return;
            if (!mAircraft.IsAlive)
                return;
            var position = mTransform.localPosition + mDirection * StantardSpeed * SpeedRate * Time.deltaTime;
            if (position.y < -100)
            {
                Destroy(gameObject);
                return;
            }
            mTransform.localPosition = position;
        }

        private readonly Vector3 mDirection = Vector3.down;
        private Aircraft mAircraft;
        private Transform mTransform;
    }
}