using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Core.Helper;
using UnityEngine;

namespace _Project.Scripts.Core.Managers
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private List<SoundData> soundList;

        private Dictionary<SoundType, AudioClip> _soundDict;

        protected override void Awake()
        {
            Configure(config =>
            {
                config.Lazy = false;
                config.Persist = false;
                config.DestroyOthers = false;
            });
            base.Awake();
            InitializeSoundDict();
        }

        private void InitializeSoundDict()
        {
            _soundDict = new Dictionary<SoundType, AudioClip>();
            foreach (var sound in soundList)
            {
                if (!_soundDict.ContainsKey(sound.soundType))
                {
                    _soundDict.Add(sound.soundType, sound.clip);
                }
            }
        }

        public void PlaySound(SoundType soundType, float volume = 1f)
        {
            if (_soundDict.TryGetValue(soundType, out var clip))
            {
                sfxSource.volume = volume;
                sfxSource.PlayOneShot(clip, volume);
            }
            else
            {
                Debug.LogWarning($"Sound not found: {soundType}");
            }
        }


        public void PlaySoundWithFade(SoundType soundType, float duration)
        {
            StartCoroutine(FadeOutSound(soundType, duration));
        }


        private IEnumerator FadeOutSound(SoundType soundType, float duration)
        {
            if (!_soundDict.TryGetValue(soundType, out var spinClip)) yield break;

            sfxSource.clip = spinClip;
            sfxSource.volume = 1f;
            sfxSource.loop = true;
            sfxSource.Play();

            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                sfxSource.volume = Mathf.Lerp(1f, 0f, elapsed / duration);
                yield return null;
            }

            sfxSource.Stop();
            sfxSource.loop = false;
        }
    }

    public enum SoundType
    {
        Spin,
        BallDrop,
    }

    [System.Serializable]
    public class SoundData
    {
        public SoundType soundType;
        public AudioClip clip;
    }
}