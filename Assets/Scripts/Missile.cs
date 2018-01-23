namespace Frederick.ProjectAircraft
{
    using UnityEngine;

    /// <summary>
    /// 导弹。
    /// </summary>
    [RequireComponent(typeof (BoxCollider))]
    public class Missile : MonoBehaviour
    {
        public int Power = 1;
        public float StantardSpeed = 800;

        protected void Awake()
        {
            mTransform = transform;
        }

        protected void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Enemy")
                return;
            Destroy(gameObject);
            var part = other.GetComponent<AircraftPart>();
            part.Aircraft.ReceiveDamage(Power);
        }

        protected void Update()
        {
            if (GameState.Instance.IsPaused)
                return;
            var position = mTransform.localPosition + mDirection * StantardSpeed * Time.deltaTime;
            if (position.y > 960)
            {
                Destroy(gameObject);
                return;
            }
            mTransform.localPosition = position;
        }

        private readonly Vector3 mDirection = Vector3.up;
        private Transform mTransform;
    }
}