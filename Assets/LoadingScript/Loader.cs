using UnityEngine;
using UnityEngine.UI;

namespace LoadingScript
{
    public class Loader : MonoBehaviour
    {
        public Slider slider;
        public Slider subSlider;
        public AudioClip[] clips;
        private int _index;

        private void Update()
        {
            subSlider.value = Mathf.InverseLerp(0, clips.Length, _index - 1);
            slider.value = Mathf.Lerp(slider.value, subSlider.value, 0.01f);
            if (_index != 0 && _index <= clips.Length && clips[_index - 1].loadState != AudioDataLoadState.Loaded) return;
            if (_index >= clips.Length)
            {
                _index++;
                Throbber.loaded = true;
                return;
            }
            clips[_index++].LoadAudioData();
        }
    }
}
