namespace Frederick.ProjectAircraft
{
    using Config;
    using Randoms;
    using UnityEngine;

    /// <summary>
    /// 敌机生成器。
    /// </summary>
    public class EnemyGenerator : MonoBehaviour
    {
        public GameObject[] EnemyPrefabs;
        public float GenerationCooldown = 1.0f;

        protected void Awake()
        {
            mTransform = transform;
            GameState.Instance.LevelChanged += updateRandomEnemies;
            updateRandomEnemies();
        }

        protected void Update()
        {
            if (GameState.Instance.IsPaused || GameState.Instance.IsGameOver)
                return;
            if (!GameState.Instance.IsMultiplaying || Network.isServer)
            {
                mElaspedTime += Time.deltaTime;
                if (!(mElaspedTime > GenerationCooldown))
                    return;
                mElaspedTime = 0;
                generate();
            }
        }

        private void generate()
        {
            GenerationCooldown = GameState.Instance.Level.EnemySpawnTime;
            var config = mEnemyRandom.Next();
            var prefab = EnemyPrefabs[config.Index];
            prefab.GetComponent<EnemyAircraftController>().SpeedRate = Random.Range(config.MinSpeed, config.MaxSpeed);
            var graphic = prefab.GetComponent<AircraftGraphic>();
            var pos = getRandomPosition(graphic.Bounds);
            if (!GameState.Instance.IsMultiplaying)
            {
                Instantiate(prefab, pos, Quaternion.identity);
                return;
            }
            if (Network.isServer) {}
            Network.Instantiate(prefab, pos, Quaternion.identity, 0);
        }

        private Vector3 getRandomPosition(Bounds bounds)
        {
            var x = Random.Range(bounds.extents.x, 640 - bounds.extents.x);
            var y = mTransform.position.y + bounds.extents.y;
            return new Vector3(x, y, mTransform.position.z);
        }

        private void updateRandomEnemies()
        {
            mEnemyRandom.Clear();
            foreach (var enemy in GameState.Instance.Level.Enemies)
                mEnemyRandom.Add(enemy, enemy.Weight);
        }

        private readonly WeightedRandom<EnemyConfig> mEnemyRandom = new WeightedRandom<EnemyConfig>();

        private float mElaspedTime;
        private Random mRandom = new Random();
        private Transform mTransform;
    }
}