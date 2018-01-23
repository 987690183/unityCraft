namespace Frederick.ProjectAircraft
{
    using Multiplay;
    using UnityEngine;

    /// <summary>
    /// 玩家飞机。
    /// </summary>
    public class PlayerAircraft : Aircraft
    {
        /// <summary>
        /// 将飞机移动到指定位置。
        /// </summary>
        /// <param name="pos">目标位置</param>
        public override void MoveTo(Vector3 pos)
        {
            var x = pos.x;
            var y = pos.y;

            var left = pos.x - Graphic.Bounds.extents.x;
            var right = pos.x + Graphic.Bounds.extents.x;
            var top = pos.y + Graphic.Bounds.extents.y;
            var bottom = pos.y - Graphic.Bounds.extents.y;

            if (left < mGameBounds.min.x)
                x = mGameBounds.min.x + Graphic.Bounds.extents.x;
            else if (right > mGameBounds.max.x)
                x = mGameBounds.max.x - Graphic.Bounds.extents.x;
            if (top > mGameBounds.max.y)
                y = mGameBounds.max.y - Graphic.Bounds.extents.y;
            else if (bottom < mGameBounds.min.y)
                y = mGameBounds.min.y + Graphic.Bounds.extents.y;

            pos = new Vector3(x, y);
            base.MoveTo(pos);
        }

        /// <summary>
        /// 当飞机爆炸时调用此方法。
        /// </summary>
        protected override void OnExploded()
        {
            MultiplayManager.Instance.GameOver();
        }

        private Bounds mGameBounds = new Bounds(new Vector3(320, 480), new Vector3(640, 960));
    }
}