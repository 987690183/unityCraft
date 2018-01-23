namespace Frederick.ProjectAircraft
{
    using UnityEngine;

    /// <summary>
    /// 导弹发射器。
    /// </summary>
    public class MissileLauncher : MonoBehaviour
    {
        public AudioClip LaunchSFX;
        public int MissileIndex;
        public GameObject[] MissilePrefabs;

        /// <summary>
        /// 发射导弹。
        /// </summary>
        public void Launch()
        {
            if (MissileIndex < 0 || MissileIndex > MissilePrefabs.Length)
                return;
            var prefab = MissilePrefabs[MissileIndex];
            var missileRoot = (GameObject) Instantiate(prefab, mTransform.position, Quaternion.identity);
            missileRoot.transform.DetachChildren();
            Destroy(missileRoot);
            AudioManager.Instance.PlaySFX(LaunchSFX);
        }

        protected void Awake()
        {
            mTransform = transform;
        }

        private Transform mTransform;
    }
}