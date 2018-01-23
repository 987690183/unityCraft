namespace Frederick.ProjectAircraft.Multiplay
{
    using System;
    using UnityEngine;

    /// <summary>
    /// <see cref="NetworkView"/>扩展方法。
    /// </summary>
    public static class NetworkViewExtension
    {
        /// <summary>
        /// 在指定玩家客户端调用指定方法。
        /// </summary>
        /// <typeparam name="T">方法参数类型</typeparam>
        /// <param name="networkView">网络视图</param>
        /// <param name="player">玩家</param>
        /// <param name="function">方法</param>
        /// <param name="param">参数</param>
        public static void CallAt<T>(this NetworkView networkView, NetworkPlayer player, Action<T> function, T param)
        {
            networkView.RPC(function.Method.Name, player, param);
        }

        /// <summary>
        /// 在所有主机上调用指定方法。
        /// </summary>
        /// <param name="networkView">网络视图</param>
        /// <param name="function">方法</param>
        public static void CallAtAll(this NetworkView networkView, Action function)
        {
            if (GameState.Instance.IsMultiplaying)
                networkView.RPC(function.Method.Name, RPCMode.All);
            else
                function();
        }

        /// <summary>
        /// 在所有主机上调用指定方法。
        /// </summary>
        /// <param name="networkView">网络视图</param>
        /// <param name="function">方法</param>
        /// <param name="param">参数</param>
        public static void CallAtAll<T>(this NetworkView networkView, Action<T> function, T param)
        {
            if (GameState.Instance.IsMultiplaying)
                networkView.RPC(function.Method.Name, RPCMode.All, param);
            else
                function(param);
        }

        /// <summary>
        /// 当多人游戏时仅远程调用服务端方法，单人游戏时直接调用方法。
        /// </summary>
        /// <param name="networkView">网络视图</param>
        /// <param name="function">方法</param>
        public static void CallAtMasterServer(this NetworkView networkView, Action function)
        {
            if (GameState.Instance.IsMultiplaying)
            {
                if (Network.isServer)
                {
                    networkView.RPC(function.Method.Name, RPCMode.All);
                }
            }
            else
            {
                function();
            }
        }

        /// <summary>
        /// 当多人游戏时仅远程调用服务端方法，单人游戏时直接调用方法。
        /// </summary>
        /// <param name="networkView">网络视图</param>
        /// <param name="function">方法</param>
        /// <param name="param">参数</param>
        public static void CallAtMasterServer<T>(this NetworkView networkView, Action<T> function, T param)
        {
            if (GameState.Instance.IsMultiplaying)
            {
                if (Network.isServer)
                {
                    networkView.RPC(function.Method.Name, RPCMode.All, param);
                }
            }
            else
            {
                function(param);
            }
        }

        /// <summary>
        /// 强类型的远程过程调用。
        /// </summary>
        /// <param name="networkView">网络视图</param>
        /// <param name="function">方法</param>
        /// <param name="mode">调用方式</param>
        public static void RPC(this NetworkView networkView, Action function, RPCMode mode)
        {
            networkView.RPC(function.Method.Name, mode);
        }
    }
}