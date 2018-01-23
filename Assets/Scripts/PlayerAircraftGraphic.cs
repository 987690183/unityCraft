namespace Frederick.ProjectAircraft
{
    using UnityEngine;

    /// <summary>
    /// 玩家飞机图形显示。
    /// </summary>
    public class PlayerAircraftGraphic : AircraftGraphic
    {
        public Color[] RenderColors;

        protected override void Awake()
        {
            base.Awake();
            mController = GetComponent<AircraftController>();
        }

        protected override void Update()
        {
            base.Update();
            if (GameState.Instance.IsMultiplaying)
                RenderColor = RenderColors[(int) mController.ID];
        }

        private AircraftController mController;
    }
}