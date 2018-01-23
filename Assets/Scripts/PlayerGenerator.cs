namespace Frederick.ProjectAircraft
{
    using UnityEngine;

    /// <summary>
    /// 玩家飞机生成器。
    /// </summary>
    [RequireComponent(typeof (NetworkView))]
    public class PlayerGenerator : MonoBehaviour
    {
        public GameObject PlayerPrefab;
        public Transform SpawnPoint;

        protected void Start()
        {
            if (GameState.Instance.IsMultiplaying)
                Network.Instantiate(PlayerPrefab, SpawnPoint.position, Quaternion.identity, 0);
            else
                Instantiate(PlayerPrefab, SpawnPoint.position, Quaternion.identity);
        }
    }
}