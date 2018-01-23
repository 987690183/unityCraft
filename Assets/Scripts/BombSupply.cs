namespace Frederick.ProjectAircraft
{
    using UnityEngine;

    /// <summary>
    /// 炸弹道具。
    /// </summary>
    public class BombSupply : Supply
    {
        protected override void OnSupply(Collider other)
        {
            var part = other.GetComponent<AircraftPart>();
            var controller = part.Aircraft.GetComponent<AircraftController>();
            controller.Bomb++;
        }
    }
}