namespace Frederick.ProjectAircraft.Menu
{
    using UnityEngine;

    /// <summary>
    /// 关于菜单。
    /// </summary>
    public class AboutMenu : MonoBehaviour
    {
        protected void Awake()
        {
        }

        /// <summary>
        /// 隐藏关于菜单。
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 显示关于菜单。
        /// </summary>
        public void Show()
        {
            gameObject.SetActive(true);
        }

        protected void OnEnable() {}
    }
}