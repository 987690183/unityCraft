namespace Frederick.ProjectAircraft
{
    using UnityEngine;

    /// <summary>
    /// 飞机的一部分。
    /// </summary>
    [RequireComponent(typeof (CollisionEventDispatcher))]
    [RequireComponent(typeof (Rigidbody))]
    public class AircraftPart : MonoBehaviour
    {
        /// <summary>
        /// 获取或设置所属的飞机对象。
        /// </summary>
        public Aircraft Aircraft { get; set; }
    }
}