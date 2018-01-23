namespace Frederick.ProjectAircraft
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    /// <summary>
    /// 声音管理器。
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        /// <summary>
        /// 获取声音管理器的唯一实例。
        /// </summary>
        public static AudioManager Instance
        {
            get
            {
                if (mInstance == null)
                {
                    var root = new GameObject("_AudioManager");
                    mInstance = root.AddComponent<AudioManager>();
                    mInstance.mAudioListener = root.AddComponent<AudioListener>();
                    mInstance.mBGMSource = root.AddComponent<AudioSource>();
                }
                return mInstance;
            }
        }

        /// <summary>
        /// 播放背景音乐。
        /// </summary>
        /// <param name="clip">背景音乐</param>
        /// <param name="isLoop">是否循环播放</param>
        public void PlayBGM(AudioClip clip, bool isLoop)
        {
            mBGMSource.loop = isLoop;
            mBGMSource.clip = clip;
            mBGMSource.Play();
        }

        /// <summary>
        /// 播放声音音效。
        /// </summary>
        /// <param name="clip">声音剪辑</param>
        public void PlaySFX(AudioClip clip)
        {
            var freeSource = mSFXSources.FirstOrDefault(source => !source.isPlaying);
            if (freeSource == null)
            {
                freeSource = gameObject.AddComponent<AudioSource>();
                mSFXSources.Add(freeSource);
            }
            freeSource.clip = clip;
            freeSource.Play();
        }

        /// <summary>
        /// 停止背景音乐。
        /// </summary>
        public void StopBGM()
        {
            if (mBGMSource != null)
                mBGMSource.Stop();
        }

        private readonly List<AudioSource> mSFXSources = new List<AudioSource>();
        private static AudioManager mInstance;
        private AudioListener mAudioListener;
        private AudioSource mBGMSource;
    }
}