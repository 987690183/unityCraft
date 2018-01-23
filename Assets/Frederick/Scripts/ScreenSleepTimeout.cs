namespace Frederick
{
    using UnityEngine;

    /// <summary>
    /// 屏幕休眠控制。
    /// </summary>
    public class ScreenSleepTimeout : MonoBehaviour
    {
        public int Seconds;
        public Timeout Style;

        protected void Awake()
        {
            if (Style == Timeout.ManualSetting)
                Screen.sleepTimeout = Seconds;
            else
                Screen.sleepTimeout = (int) Style;
        }
    }

    /// <summary>
    /// 休眠方式。
    /// </summary>
    public enum Timeout
    {
        /// <summary>
        /// 禁止休眠。
        /// </summary>
        NeverSleep = SleepTimeout.NeverSleep,

        /// <summary>
        /// 系统设置。
        /// </summary>
        SystemSetting = SleepTimeout.SystemSetting,

        /// <summary>
        /// 手动设置。
        /// </summary>
        ManualSetting = 0,
    }
}