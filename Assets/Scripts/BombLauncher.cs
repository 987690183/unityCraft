namespace Frederick.ProjectAircraft
{
    using System.Linq;
    using Multiplay;
    using UnityEngine;

    /// <summary>
    /// 炸弹发射器。
    /// </summary>
    public class BombLauncher : MonoBehaviour
    {
        public AudioClip LaunchSFX;

        /// <summary>
        /// 发射全屏炸弹。
        /// </summary>
        public void Launch()
        {
            GetComponent<NetworkView>().CallAtAll(launch);
        }

        [RPC]
        private void launch()
        {
            AudioManager.Instance.PlaySFX(LaunchSFX);
            var query = from enemy in GameObject.FindGameObjectsWithTag("Enemy")
                        let part = enemy.GetComponent<AircraftPart>()
                        where part.Aircraft.IsAlive
                        select part.Aircraft;
            foreach (var aircraft in query)
                aircraft.Explode();
        }
    }
}