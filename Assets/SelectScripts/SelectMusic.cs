using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using MainGameScript;
using ResultScript;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace SelectScripts
{
    public class SelectMusic : MonoBehaviour
    {
        [FormerlySerializedAs("AudioSource")] public AudioSource audioSource;
        [FormerlySerializedAs("Musics")] public List<AudioClip> musics;
        public List<Texture> icons;
        public TMP_InputField nfd, jd;
        public TextMeshProUGUI title, composer, level, speed;
        [FormerlySerializedAs("isPP")] public List<TextMeshProUGUI> isPp;
        private static List<double> _ranks = new();
        public RawImage icon, checkBox;
        private int _musicIdx = 9;
        private static SelectMusic _instance;
        private static string _lss;

        public void Awake()
        {
            _lss ??= "Exit This Earth's Atomosphere|かめりあ|18|4|237781";
            SelectButton(_lss);
            if (!Directory.Exists("Screenshots/")) Directory.CreateDirectory("Screenshots/");
            if (!Directory.Exists("Results/")) Directory.CreateDirectory("Results/");
            if (!File.Exists("Results/results.r")) File.Create("Results/results.r");
            _ranks = NoteData.LoadRanks();
            while (_ranks.Count < isPp.Count()) _ranks.Add(0);
            _instance = this;
            checkBox.color = NoteFalling.autoPlay ? new Color(255, 255, 255, 255) : new Color(255, 255, 255, 0);
            speed.text = (!(Math.Abs(NoteFalling.speed - 1) <= 0) ? (Math.Abs(NoteFalling.speed - 2) <= 0 ? "2.0" : NoteFalling.speed) : "1.0") + "x";
            audioSource.pitch = NoteFalling.speed;
        }

        public void Update()
        {
            for (int i = 0; i < isPp.Count; i++)
            {
                isPp[i].text = Result.SplitRate(_ranks[i]) + " " + Result.getRank(_ranks[i], false);
                isPp[i].color =  Result.getRankColor(_ranks[i]);
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        public static void updateRank(double rate)
        {
            if (_ranks[_instance._musicIdx] < rate) _ranks[_instance._musicIdx] = rate;
            NoteData.SaveRanks(_ranks);
        }

        public void SelectButton(string info)
        {
            _lss = info;
            title.text = info.Split("|")[0].Replace("\\n", "\n");
            composer.text = info.Split("|")[1].Replace("\\n", "\n");
            level.text = info.Split("|")[3];
            if (info.Split("|")[3].Equals("ERROR"))
            {
                level.color = Color.red;
                title.color = Color.red;
            }
            else
            {
                level.color = Color.magenta;
                title.color = Color.white;
            }
            icon.texture = icons[int.Parse(info.Split("|")[2])];
            audioSource.clip = null;
            audioSource.clip = musics[int.Parse(info.Split("|")[2])];
            audioSource.time = float.Parse(info.Split("|")[4]) / 1000;
            audioSource.Play();
            _musicIdx = int.Parse(info.Split("|")[2]);
        }

        public void StartButton()
        {
            NoteFalling.title = title.text.Replace("\n", " ");
            NoteFalling.music = audioSource.clip;
            NoteFalling.nfd = float.Parse(nfd.text);
            Lines.jd = float.Parse(jd.text);
            NoteFalling.startTime = Time.time * 1000;
            SceneManager.LoadScene("MainGame");
        }

        public void AutoPlayButton()
        {
            NoteFalling.autoPlay = !NoteFalling.autoPlay;
            checkBox.color = NoteFalling.autoPlay ? new Color(255, 255, 255, 255) : new Color(255, 255, 255, 0);
        }

        public void SpeedDecreaseButton()
        {
            speed.text = Math.Max(float.Parse(speed.text[..^1]) - 0.09, 0.5).ToString(CultureInfo.InvariantCulture)[..3] + "x";
            NoteFalling.speed = float.Parse(speed.text[..^1]);
            audioSource.pitch = NoteFalling.speed;
        }

        public void SpeedIncreaseButton()
        {
            speed.text = Math.Min(float.Parse(speed.text[..^1]) + 0.11, _ranks.TrueForAll(s => Math.Abs(s - 100) <= 0) ? 2.55 : 2.05).ToString(CultureInfo.InvariantCulture)[..3] + "x";
            NoteFalling.speed = float.Parse(speed.text[..^1]);
            audioSource.pitch = NoteFalling.speed;
        }

        public void SpeedResetButton()
        {
            speed.text = "1.0x";
            NoteFalling.speed = float.Parse(speed.text[..^1]);
            audioSource.pitch = NoteFalling.speed;
        }
    }
}
