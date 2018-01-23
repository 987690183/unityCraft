namespace Frederick.ProjectAircraft
{
    using UnityEngine;

    /// <summary>
    /// 道具生成器。
    /// </summary>
    public class SupplyGenerator : MonoBehaviour
    {
        public float GenerationCooldown = 20.0f;
        public GameObject[] SupplyPrefabs;

        protected void Awake()
        {
            mTransform = transform;
        }

        protected void Update()
        {
            if (GameState.Instance.IsPaused || GameState.Instance.IsGameOver)
                return;
            mElaspedTime += Time.deltaTime;
            if (!(mElaspedTime > GenerationCooldown))
                return;
            mElaspedTime = 0;
            generate();
        }

        private void generate()
        {
            var prefab = getRandomPrefab();
            var pos = getRandomPosition();
            if (!GameState.Instance.IsMultiplaying)
            {
                Instantiate(prefab, pos, Quaternion.identity);
                return;
            }
            if (Network.isServer)
                Network.Instantiate(prefab, pos, Quaternion.identity, 0);
        }

        private Vector3 getRandomPosition()
        {
            var x = Random.Range(64, 576);
            var y = mTransform.position.y + 64;
            return new Vector3(x, y, mTransform.position.z);
        }

        private GameObject getRandomPrefab()
        {
            var index = Random.Range(0, SupplyPrefabs.Length);
            var ret = SupplyPrefabs[index];
            return ret;
        }

        private float mElaspedTime;
        private Random mRandom = new Random();
        private Transform mTransform;
    }
}