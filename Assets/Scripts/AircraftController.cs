namespace Frederick.ProjectAircraft
{
    using Multiplay;
    using UnityEngine;

    /// <summary>
    /// 玩家飞机控制器。
    /// </summary>
    [RequireComponent(typeof (BoxCollider))]
    public class AircraftController : MonoBehaviour
    {
        public int Bomb;
        public float DoubleMissileTime;
        public PlayerIndex ID;

        /// <summary>
        /// 增加双倍弹药的时间。
        /// <param name="duration">增加的秒数</param>
        /// </summary>
        public void AddDoubleMissileTime(float duration)
        {
            mNetworkView.CallAtMasterServer<float>(setupDoubleMissileTime, duration);
        }

        /// <summary>
        /// 释放炸弹。
        /// </summary>
        public void LaunchBomb()
        {
            if (Bomb <= 0 || GameState.Instance.IsGameOver)
                return;
            Bomb--;
            mBombLauncher.Launch();
        }

        protected void Awake()
        {
            Input.multiTouchEnabled = false;
            mMainCamera = Camera.main;
            mTransform = transform;
            mNetworkView = GetComponent<NetworkView>();
            mAircraft = GetComponent<Aircraft>();
            mMissileLauncher = GetComponentInChildren<MissileLauncher>();
            mBombLauncher = GetComponentInChildren<BombLauncher>();
        }

        protected void OnMouseDown()
        {
            mIsDragging = true;
            var mouseWorldPos = mMainCamera.ScreenToWorldPoint(Input.mousePosition);
            mOffset = mouseWorldPos - mTransform.position;
        }

        protected void OnMouseUp()
        {
            mIsDragging = false;
        }

        protected void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo msg)
        {
            var time = 0;
            //mID = (int) ID;
            if (stream.isWriting)
            {
                stream.Serialize(ref DoubleMissileTime);
                //stream.Serialize(ref mID);
            }
            else
            {
                stream.Serialize(ref time);
                DoubleMissileTime = time;
                //stream.Serialize(ref mID);
                //ID = (PlayerIndex) mID;
            }
        }

        protected void Start()
        {
            if (GameState.Instance.IsMultiplaying)
            {
                GetComponent<Collider>().enabled = mNetworkView.isMine;
                if (mNetworkView.isMine)
                    mNetworkView.CallAtAll(setupPlayerIndex, int.Parse(Network.player.ToString()));
            }
        }

        protected void Update()
        {
            if (GameState.Instance.IsPaused)
                return;
            if (GameState.Instance.IsMultiplaying)
                updateMultiGame();
            else
                updateSingleGame();
        }

        private void performBomb()
        {
            if (Input.GetMouseButtonUp(1))
                LaunchBomb();
        }

        private void performFire()
        {
            if (mLastLaunchElapsedTime > mLaunchCooldown)
            {
                mLastLaunchElapsedTime = 0;
                mMissileLauncher.Launch();
            }
            else
            {
                mLastLaunchElapsedTime += Time.deltaTime;
            }
        }

        private void performMissile()
        {
            DoubleMissileTime -= Time.deltaTime;
            if (DoubleMissileTime <= 0)
            {
                DoubleMissileTime = 0;
                mMissileLauncher.MissileIndex = 0;
            }
            else
            {
                mMissileLauncher.MissileIndex = 1;
            }
        }

        private void performMovement()
        {
            if (!mIsDragging || !Input.GetMouseButton(0))
                return;
            var mouseWorldPos = mMainCamera.ScreenToWorldPoint(Input.mousePosition);
            var pos = mouseWorldPos - mOffset;
            mAircraft.MoveTo(pos);
        }

        [RPC]
        private void setupDoubleMissileTime(float duration)
        {
            DoubleMissileTime = duration;
        }

        [RPC]
        private void setupPlayerIndex(int index)
        {
            ID = (PlayerIndex) index;
        }

        private void updateMultiGame()
        {
            performMissile();
            if (mNetworkView.isMine)
                performBomb();
            if (mAircraft.IsAlive)
            {
                if (mNetworkView.isMine)
                    performMovement();
                performFire();
            }
        }

        private void updateSingleGame()
        {
            performMissile();
            performBomb();
            if (mAircraft.IsAlive)
            {
                performMovement();
                performFire();
            }
        }

        private const float mLaunchCooldown = 0.18f;

        private Aircraft mAircraft;
        private BombLauncher mBombLauncher;
        private int mID;
        private bool mIsDragging;
        private float mLastLaunchElapsedTime = float.MaxValue;
        private Camera mMainCamera;
        private MissileLauncher mMissileLauncher;
        private NetworkView mNetworkView;
        private Vector3 mOffset;
        private Transform mTransform;
    }
}