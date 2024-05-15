using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using MainGameScript;
using ResultScript;
using TMPro;
using Unity.VisualScripting;
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
        public RawImage levelImage;
        [FormerlySerializedAs("isPP")] public List<TextMeshProUGUI> isPp;
        private static List<double> _ranks = new();
        public RawImage icon;
        public GameObject contentList;
        private int _musicIdx = 9;
        private static SelectMusic _instance;
        private static string _lss;

        public static SelectMusic getInstance()
        {
            return _instance;
        }
        
        public void Awake()
        {
            _lss ??= "Disorder|HyuN feat. Yuri|21|0|193|92050";
            SelectButton(_lss);
            if (!Directory.Exists("Screenshots/")) Directory.CreateDirectory("Screenshots/");
            if (!Directory.Exists("Results/")) Directory.CreateDirectory("Results/");
            if (!File.Exists("Results/results.r")) File.Create("Results/results.r");
            _ranks = NoteData.LoadRanks();
            while (_ranks.Count < isPp.Count()) _ranks.Add(0);
            _instance = this;
            speed.text = (!(Math.Abs(NoteFalling.speed - 1) <= 0) ? (Math.Abs(NoteFalling.speed - 2) <= 0 ? "2.0" : NoteFalling.speed) : "1.0") + "x";
            audioSource.pitch = NoteFalling.speed;
        }

        public void SortMusic(int idx, bool inc)
        {
            var arr = contentList.GetComponentsInChildren<SelectButtonInit>();
            Array.Sort(arr, (a, b) => compareButtonBy(a, b, idx, inc));
            foreach (var button in arr) button.transform.SetAsLastSibling();
        }

        private static int compareButtonBy(SelectButtonInit a, SelectButtonInit b, int index, bool inc)
        {
            var data1 = a.data.Split("|")[index];
            var data2 = b.data.Split("|")[index];
            if (int.TryParse(data1.Split("~")[0], out var x) && int.TryParse(data2.Split("~")[0], out var y)) return inc ? x.CompareTo(y) : -x.CompareTo(y);
            return inc ? string.Compare(data1, data2, StringComparison.Ordinal) : -string.Compare(data1, data2, StringComparison.Ordinal);
        }

        public void Update()
        {
            for (var i = 0; i < isPp.Count; i++)
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
            level.text = "LEVEL : " + info.Split("|")[3];
            levelImage.color = getColorByLevel(info.Split("|")[3]);
            title.color = info.Split("|")[3].Equals("ERROR") ? Color.red : Color.white;
            icon.texture = icons[int.Parse(info.Split("|")[2])];
            audioSource.clip = null;
            audioSource.clip = musics[int.Parse(info.Split("|")[2])];
            audioSource.time = float.Parse(info.Split("|")[5]) / 1000;
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

        public static Color getColorByLevel(string level)
        {
            if (level.Equals("ERROR")) return Color.HSVToRGB(0.91f,0.85f,1f);
            return int.Parse(level) switch
            {
                < 2 => Color.HSVToRGB(0.48f, 0.18f, 0.97f),
                < 5 => Color.HSVToRGB(0.36f, 0.63f, 0.97f),
                < 10 => Color.HSVToRGB(0.56f,0.63f,0.97f),
                < 20 => Color.HSVToRGB(0.08f,0.76f,0.93f),
                < 30 => Color.HSVToRGB(0f, 1f, 1f),
                < 40 => Color.HSVToRGB(0.81f, 0.59f, 0.89f),
                < 50 => Color.HSVToRGB(0.7f, 0.59f, 0.89f),
                _ => Color.gray
            };
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
