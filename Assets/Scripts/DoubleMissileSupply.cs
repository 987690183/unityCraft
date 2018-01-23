namespace Frederick.ProjectAircraft
{
    using UnityEngine;

    /// <summary>
    /// 双倍导弹道具。
    /// </summary>
    public class DoubleMissileSupply : Supply
    {
        public float Duration = 15;

        /// <summary>
        /// 当道具碰到玩家飞机时调用此方法。
        /// </summary>
        ///<param name="other">飞机的碰撞器</param>
        protected override void OnSupply(Collider other)
        {
            var part = other.GetComponent<AircraftPart>();
            var controller = part.Aircraft.GetComponent<AircraftController>();
            controller.AddDoubleMissileTime(Duration);
        }
    }
}