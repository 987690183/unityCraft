namespace Frederick.ProjectAircraft
{
    using System.Collections.Generic;
    using Multiplay;
    using UnityEngine;

    /// <summary>
    /// 飞机。
    /// </summary>
    public class Aircraft : MonoBehaviour
    {
        /// <summary>
        /// 获取飞机的图像显示。
        /// </summary>
        public AircraftGraphic Graphic { get; private set; }

        /// <summary>
        /// 获取一个值，表示飞机是否存活。
        /// </summary>
        public bool IsAlive
        {
            get { return Health > 0; }
        }

        /// <summary>
        /// 获取飞机所有的部分。
        /// </summary>
        public IEnumerable<AircraftPart> Parts
        {
            get { return mParts.AsReadOnly(); }
        }

        public Transform Transform { get; private set; }

        public int Health;

        /// <summary>
        /// 令飞机直接爆炸。
        /// </summary>
        public void Explode()
        {
            GetComponent<NetworkView>().CallAtMasterServer(explode);
        }

        /// <summary>
        /// 将飞机移动到指定位置。
        /// </summary>
        /// <param name="pos">目标位置</param>
        public virtual void MoveTo(Vector3 pos)
        {
            Transform.position = pos;
        }

        /// <summary>
        /// 飞机受到指定伤害。
        /// </summary>
        /// <param name="damage">伤害值</param>
        public void ReceiveDamage(int damage)
        {
            Health -= damage;
            if (Health <= 0)
                Explode();
            else
                Graphic.UnderAttack();
        }

        protected virtual void Awake()
        {
            Transform = transform;
            Graphic = GetComponent<AircraftGraphic>();

            var self = GetComponent<AircraftPart>();
            var children = GetComponentsInChildren<AircraftPart>();
            if (self != null)
                mParts.Add(self);
            mParts.AddRange(children);
            foreach (var part in mParts)
            {
                part.Aircraft = this;
                var dispatcher = part.GetComponent<CollisionEventDispatcher>();
                dispatcher.TriggerEntered += OnPartTriggerEnter;
            }
        }

        protected virtual void OnDestroy()
        {
            foreach (var part in mParts)
            {
                part.Aircraft = null;
                var dispatcher = part.GetComponent<CollisionEventDispatcher>();
                dispatcher.TriggerEntered -= OnPartTriggerEnter;
            }
        }

        /// <summary>
        /// 当飞机爆炸时调用此方法。
        /// </summary>
        protected virtual void OnExploded() {}

        /// <summary>
        /// 当飞机的一部分触发碰撞时调用此方法。
        /// </summary>
        /// <param name="part">碰撞的部分</param>
        /// <param name="other">碰撞的碰撞器</param>
        protected virtual void OnPartTriggerEnter(GameObject part, Collider other) {}

        [RPC]
        private void explode()
        {
            Health = 0;
            Graphic.Explode(() =>
            {
                if (GameState.Instance.IsMultiplaying)
                {
                    if (GetComponent<NetworkView>().isMine)
                        Network.Destroy(gameObject);
                }
                else
                {
                    Destroy(gameObject);
                }
            });
            OnExploded();
        }

        private readonly List<AircraftPart> mParts = new List<AircraftPart>();
    }
}