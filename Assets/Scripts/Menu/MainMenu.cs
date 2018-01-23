namespace Frederick.ProjectAircraft.Menu
{
    using System.Collections;
    using System.Collections.Generic;
    using Multiplay;
    using UnityEngine;

    /// <summary>
    /// 主菜单。
    /// </summary>
    public class MainMenu : MonoBehaviour
    {
        /// <summary>
        /// 获取当前菜单。
        /// </summary>
        protected GameObject CurrentMenu
        {
            get { return mMenuStack.Count > 0 ? mMenuStack.Peek() : null; }
        }

        public GameObject AboutButton;
        public GameObject AboutMenu;
        public GameObject CreateGameMenu;
        public GameObject JoinGameMenu;
        public GameObject LogoSprite;
        public GameObject ModeMenu;
        public GameObject MultiplayMenu;
        public GameObject ReturnButton;

        /// <summary>
        /// 进入关于菜单。
        /// </summary>
        public void EnterAboutMenu()
        {
            enterMenu(AboutMenu);
        }

        /// <summary>
        /// 进入创建游戏菜单。
        /// </summary>
        public void EnterCreateGameMenu()
        {
            MultiplayManager.Instance.CreateNewGame();
            enterMenu(CreateGameMenu);
        }

        /// <summary>
        /// 进入加入游戏菜单。
        /// </summary>
        public void EnterJoinGameMenu()
        {
            enterMenu(JoinGameMenu);
        }

        /// <summary>
        /// 进入选择多人游戏模式。
        /// </summary>
        public void EnterMultiplayMenu()
        {
            enterMenu(MultiplayMenu);
        }

        /// <summary>
        /// 返回上层菜单。
        /// </summary>
        public void Return()
        {
            if (mMenuStack.Count <= 1)
                return;
            var current = mMenuStack.Pop();
            fadeOut(current, -1);
            var old = mMenuStack.Peek();
            fadeBack(old);
            if (Network.peerType == NetworkPeerType.Connecting
                || Network.peerType == NetworkPeerType.Client
                || Network.peerType == NetworkPeerType.Server)
                Network.Disconnect();
        }

        /// <summary>
        /// 进入单人游戏模式。
        /// </summary>
        public void Singleplay()
        {
            Application.LoadLevel("GamePlay");
        }

        protected void Awake()
        {
            mMenuStack.Push(ModeMenu);
        }

        protected void Update()
        {
            LogoSprite.SetActive(CurrentMenu != AboutMenu);
            AboutButton.SetActive(CurrentMenu == ModeMenu);
            ReturnButton.SetActive(mMenuStack.Count > 1);
        }

        private void enterMenu(GameObject item)
        {
            item.SetActive(true);
            var old = mMenuStack.Peek();
            fadeOut(old, 1);
            fadeIn(item);
            mMenuStack.Push(item);
        }

        private void fadeBack(GameObject item)
        {
            item.SetActive(true);
            item.transform.localPosition = Vector3.zero;
            iTween.MoveFrom(item, new Hashtable
            {
                {"x", -320},
                {"time", 1f},
                {"islocal", true},
            });

            item.GetComponent<TweenAlpha>().Play(true);
        }

        private void fadeIn(GameObject item)
        {
            item.SetActive(true);
            item.transform.localPosition = Vector3.zero;
            iTween.MoveFrom(item, new Hashtable
            {
                {"x", 320},
                {"time", 0.5f},
                {"islocal", true},
            });

            item.GetComponent<TweenAlpha>().Play(true);
        }

        private void fadeOut(GameObject item, int direction)
        {
            item.transform.localPosition = Vector3.zero;
            iTween.MoveTo(item, new Hashtable
            {
                {"x", -320 * direction},
                {"time", 1f},
                {"islocal", true},
            });

            item.GetComponent<TweenAlpha>().Play(false);
        }

        private readonly Stack<GameObject> mMenuStack = new Stack<GameObject>();
    }
}