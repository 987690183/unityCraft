namespace Frederick.ProjectAircraft.Menu
{
    using System.Collections;
    using Multiplay;
    using UnityEngine;

    /// <summary>
    /// 加入游戏菜单。
    /// </summary>
    public class JoinGameMenu : MonoBehaviour
    {
        public UIInput AddressInput;
        public UISprite AddressSprite;
        public UIButton CancelButton;
        public UISprite ConnectedSprite;
        public UISprite ConnectingSprite;
        public UISprite ConnectionFailedSprite;
        public UIButton JoinGameButton;

        /// <summary>
        /// 取消加入游戏。
        /// </summary>
        public void Cancel()
        {
            mIsConnecting = false;
            Network.Disconnect();
        }

        /// <summary>
        /// 加入游戏。
        /// </summary>
        public void JoinGame()
        {
            var address = AddressInput.label.text;
            var error = MultiplayManager.Instance.JoinGame(address);
            if (error == NetworkConnectionError.NoError)
                mIsConnecting = true;
        }

        protected void OnFailedToConnect(NetworkConnectionError error)
        {
            mIsConnecting = false;
            mIsConnectionFailed = true;
            StartCoroutine(resetConnectionFailedFlag());
        }

        protected void Update()
        {
            switch (Network.peerType)
            {
                case NetworkPeerType.Disconnected:
                    if (mIsConnecting)
                        onConnecting();
                    else
                        onDisconnected();
                    break;
                case NetworkPeerType.Client:
                    onClient();
                    break;
                case NetworkPeerType.Connecting:
                    onConnecting();
                    break;
            }
        }

        private void onClient()
        {
            mIsConnecting = false;

            ConnectionFailedSprite.enabled = false;
            AddressSprite.enabled = false;
            ConnectedSprite.enabled = true;
            ConnectingSprite.enabled = false;

            JoinGameButton.gameObject.SetActive(false);
            JoinGameButton.isEnabled = false;

            CancelButton.gameObject.SetActive(true);
            CancelButton.isEnabled = true;
        }

        private void onConnecting()
        {
            ConnectionFailedSprite.enabled = false;
            AddressSprite.enabled = false;
            ConnectedSprite.enabled = false;
            ConnectingSprite.enabled = true;

            JoinGameButton.gameObject.SetActive(false);
            JoinGameButton.isEnabled = false;

            CancelButton.gameObject.SetActive(true);
            CancelButton.isEnabled = true;
        }

        private void onDisconnected()
        {
            if (mIsConnectionFailed)
            {
                ConnectionFailedSprite.enabled = true;
                AddressSprite.enabled = false;
                ConnectedSprite.enabled = false;
                ConnectingSprite.enabled = false;
            }
            else
            {
                ConnectionFailedSprite.enabled = false;
                AddressSprite.enabled = true;
                ConnectedSprite.enabled = false;
                ConnectingSprite.enabled = false;
            }
            JoinGameButton.gameObject.SetActive(true);
            JoinGameButton.isEnabled = true;

            CancelButton.gameObject.SetActive(false);
            CancelButton.isEnabled = false;
        }

        private IEnumerator resetConnectionFailedFlag()
        {
            yield return new WaitForSeconds(2);
            mIsConnectionFailed = false;
        }

        private void showErrorTip(string msg)
        {
            Debug.Log(msg);
        }

        private bool mIsConnecting;
        private bool mIsConnectionFailed;
    }
}