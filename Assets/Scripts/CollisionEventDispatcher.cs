namespace Frederick.ProjectAircraft
{
    using UnityEngine;

    /// <summary>
    /// 碰撞事件分发器。
    /// </summary>
    public class CollisionEventDispatcher : MonoBehaviour
    {
        /// <summary>
        /// 碰撞事件处理器。
        /// </summary>
        /// <param name="collision">碰撞信息</param>
        public delegate void CollisionEventHandler(Collision collision);

        /// <summary>
        /// 触发器碰撞事件处理器。
        /// </summary>
        /// <param name="sender">发送事件的游戏对象</param>
        /// <param name="other">触发事件的另外一个碰撞器</param>
        public delegate void TriggerEventHandler(GameObject sender, Collider other);

        /// <summary>
        /// 当有触发器进入时候触发此事件。
        /// </summary>
        public event TriggerEventHandler TriggerEntered;

        protected void OnTriggerEnter(Collider other)
        {
            if (TriggerEntered != null)
                TriggerEntered(gameObject, other);
        }
    }
}