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
        public static bool pauseState;

        public static MusicController Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (!SelectMusic.getInstance().audioSource.isPlaying && SelectMusic.getInstance().audioSource.time >= SelectMusic.getInstance().audioSource.clip.length)
            {
                SelectMusic.getInstance().audioSource.clip = null;
                SelectMusic.getInstance().audioSource.clip = SelectMusic.getInstance().musics[int.Parse(SelectMusic.lss.Split("|")[2])];
                SelectMusic.getInstance().audioSource.time = float.Parse(SelectMusic.lss.Split("|")[5]) / 1000;
                SelectMusic.getInstance().audioSource.Play();
            }
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
            if (!SelectMusic.getInstance().audioSource.isPlaying && !pauseState) SelectMusic.getInstance().audioSource.Play();
        }

        public void Pause()
        {
            pauseState = !pauseState;
            pauseButton.image.sprite = pauseState ? play : pause;
            if (pauseState) SelectMusic.getInstance().audioSource.Pause();
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
