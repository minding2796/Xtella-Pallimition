using UnityEngine;

namespace LoadingScript
{
    public class Loader : MonoBehaviour
    {
        public AudioClip[] clips;
        private static AudioSource _source;
        private int _index = 0;
        
        private void Start()
        {
            _source = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (_index >= clips.Length)
            {
                Throbber.loaded = true;
                return;
            }
            if (!_source.isPlaying) return;
            _source.clip = null;
            _source.clip = clips[_index++];
            _source.Play();
        }
    }
}
