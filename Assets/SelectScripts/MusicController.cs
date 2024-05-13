using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SelectScripts
{
    public class MusicController : MonoBehaviour
    {
        public Scrollbar scrollbar;
        public TextMeshProUGUI currentTime, maxTime;
        public Button pauseButton;
        public Sprite pause, play;
        private bool _pauseState;
        
        private void Update()
        {
            
            scrollbar.value = SelectMusic.getInstance().audioSource.time / SelectMusic.getInstance().audioSource.clip.length;
            currentTime.text = getTimeFromFloat(SelectMusic.getInstance().audioSource.time);
            maxTime.text = getTimeFromFloat(SelectMusic.getInstance().audioSource.clip.length);
        }

        private static string getTimeFromFloat(float time)
        {
            var t = (int) time;
            return t / 60 + ":" + (t % 60 < 10 ? "0" : "") + t % 60;
        }

        public void TimeChange()
        {
            SelectMusic.getInstance().audioSource.time = Math.Max(Math.Min(scrollbar.value * SelectMusic.getInstance().audioSource.clip.length, SelectMusic.getInstance().audioSource.clip.length-0.001f), 0);
        }

        public void Pause()
        {
            _pauseState = !_pauseState;
            pauseButton.image.sprite = _pauseState ? play : pause;
            if (_pauseState) SelectMusic.getInstance().audioSource.Pause();
            else SelectMusic.getInstance().audioSource.Play();
        }

        public void Skip()
        {
            SelectMusic.getInstance().audioSource.time = Math.Min(SelectMusic.getInstance().audioSource.clip.length-0.001f, SelectMusic.getInstance().audioSource.time + 10);
        }

        public void Reverse()
        {
            SelectMusic.getInstance().audioSource.time = Math.Max(0, SelectMusic.getInstance().audioSource.time - 10);
        }
    }
}
