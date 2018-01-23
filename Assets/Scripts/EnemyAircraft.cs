namespace Frederick.ProjectAircraft
{
    using UnityEngine;

    /// <summary>
    /// 敌方的飞机。
    /// </summary>
    public class EnemyAircraft : Aircraft
    {
        public int Score;

        /// <summary>
        /// 当飞机爆炸时调用此方法。
        /// </summary>
        protected override void OnExploded()
        {
            GameState.Instance.Score += Score;
        }

        /// <summary>
        /// 当飞机的一部分触发碰撞时调用此方法。
        /// </summary>
        /// <param name="part">碰撞的部分</param>
        /// <param name="other">碰撞的碰撞器</param>
        protected override void OnPartTriggerEnter(GameObject part, Collider other)
        {
            if (other.tag != "Player")
                return;
            var aircraftPart = other.GetComponent<AircraftPart>();
            aircraftPart.Aircraft.Explode();
        }
    }
}