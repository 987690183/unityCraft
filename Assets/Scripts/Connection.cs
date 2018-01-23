namespace Frederick.ProjectAircraft
{
    using UnityEngine;

    /// <summary>
    /// 连接工具。
    /// </summary>
    public class Connection : MonoBehaviour
    {
        protected void OnConnectedToServer()
        {
            Debug.Log("Server connected.");
        }

        protected void OnDisconnectedFromServer(NetworkDisconnection disconnection)
        {
            Debug.Log("Disconnected from server.disconnection=" + disconnection);
        }

        protected void OnFailedToConnect(NetworkConnectionError error)
        {
            Debug.Log("failed to connect " + error);
        }

        protected void OnFailedToConnectToMaster(NetworkConnectionError error)
        {
            Debug.Log("failed to connect to master " + error);
        }

        protected void OnGUI()
        {

        }

        protected void OnMasterServerEvent(MasterServerEvent serverEvent)
        {
            Debug.Log("on master server event " + serverEvent);
        }

        protected void OnServerInitialized()
        {
            Debug.Log("server initialized.");
        }
    }
}