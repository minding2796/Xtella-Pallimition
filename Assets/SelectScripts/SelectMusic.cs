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
        public TMP_InputField filter;
        public TextMeshProUGUI title, composer, level, speed;
        public RawImage levelImage;
        [FormerlySerializedAs("isPP")] public List<TextMeshProUGUI> isPp;
        private static List<double> _ranks = new();
        public RawImage icon;
        public GameObject contentList;
        public Button pauseButton;
        public Sprite pauseSprite;
        private int _musicIdx = 9;
        private static SelectMusic _instance;
        private SelectButtonInit[] _selectButtons;
        public static string lss;
        private static string _lqs;

        public static SelectMusic getInstance()
        {
            return _instance;
        }
        
        public void Awake()
        {
            _selectButtons = contentList.GetComponentsInChildren<SelectButtonInit>();
            lss ??= "Disorder|HyuN feat. Yuri|21|0|193|92050";
            SelectButton(lss);
            if (!Directory.Exists("Screenshots/")) Directory.CreateDirectory("Screenshots/");
            if (!Directory.Exists("Results/")) Directory.CreateDirectory("Results/");
            if (!File.Exists("Results/results.r")) File.Create("Results/results.r");
            if (!File.Exists("Results/offsets.r")) File.Create("Results/offsets.r");
            _ranks = NoteData.LoadRanks();
            var offsets = NoteData.LoadOffsets();
            NoteFalling.nfd = offsets.Item1;
            Lines.jd = offsets.Item2;
            while (_ranks.Count < isPp.Count()) _ranks.Add(0);
            _instance = this;
            speed.text = (!(Math.Abs(NoteFalling.speed - 1) <= 0) ? (Math.Abs(NoteFalling.speed - 2) <= 0 ? "2.0" : NoteFalling.speed) : "1.0") + "x";
            audioSource.pitch = NoteFalling.speed;
            StartCoroutine(FilterInit());
        }

        private IEnumerator FilterInit()
        {
            yield return null;
            filter.text = _lqs;
        }
        
        public void FilterMusic()
        {
            _lqs = filter.text;
            foreach (var button in _selectButtons) button.gameObject.SetActive(FilterButtonBy(button, _lqs));
        }

        private bool FilterButtonBy(SelectButtonInit button, string query)
        {
            query = query.ToLower();
            return query.Split("-").All(splitQuery=>
                splitQuery.Trim().Length <= 0 ||
                MaskSpecialTitle(button.data.Split("|")[0], splitQuery.Trim()) ||
                MaskSpecialComposer(button.data.Split("|")[1].ToLower(), splitQuery.Trim()) ||
                button.data.Split("|")[4].ToLower().Contains(splitQuery.Trim()) ||
                button.data.Split("|")[3].ToLower().Contains(splitQuery.Trim()) ||
                isPp[int.Parse(button.data.Split("|")[2])].text.ToLower().Contains(splitQuery.Trim()));
        }

        private static bool MaskSpecialTitle(string title, string query)
        {
            return title.Contains(query) || title
                .Replace("И", "n")
                .Replace("Ǝ", "e")
                .Replace("LΛ8YR", "labyr")
                .Replace("ö", "o")
                .ToLower()
                .Contains(query);
        }

        private static bool MaskSpecialComposer(string composer, string query)
        {
            return composer.Contains(query) || composer
                .Replace("かめりあ", "camellia")
                .Replace("モリモリあつし", "morimori atsushi")
                .Replace("awc sound team", "ardolf sdmne takehirotei xeno metahumanboi xh cinamoro billiummoto")
                .Contains(query);
        }

        public void SortMusic(int idx, bool inc)
        {
            Array.Sort(_selectButtons, (a, b) => compareButtonBy(a, b, idx, inc));
            foreach (var button in _selectButtons) button.transform.SetAsLastSibling();
        }

        private static int compareButtonBy(SelectButtonInit a, SelectButtonInit b, int index, bool inc)
        {
            var data1 = a.data.Split("|")[index];
            var data2 = b.data.Split("|")[index];
            int r;
            if (int.TryParse(data1.Split("~")[0], out var x) && int.TryParse(data2.Split("~")[0], out var y))
            {
                r = inc ? x.CompareTo(y) : -x.CompareTo(y);
                r = r == 0 ? string.Compare(a.data.Split("|")[0].ToLower(), b.data.Split("|")[0].ToLower(), StringComparison.Ordinal) : r;
                return r == 0 ? string.Compare(a.data.Split("|")[1].ToLower(), b.data.Split("|")[1].ToLower(), StringComparison.Ordinal) : r;
            }
            r = inc ? string.Compare(data1.ToLower(), data2.ToLower(), StringComparison.Ordinal) : -string.Compare(data1.ToLower(), data2.ToLower(), StringComparison.Ordinal);
            r = r == 0 ? string.Compare(a.data.Split("|")[0].ToLower(), b.data.Split("|")[0].ToLower(), StringComparison.Ordinal) : r;
            return r == 0 ? string.Compare(a.data.Split("|")[1].ToLower(), b.data.Split("|")[1].ToLower(), StringComparison.Ordinal) : r;
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
            lss = info;
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
            MusicController.pauseState = false;
            pauseButton.image.sprite = pauseSprite;
            _musicIdx = int.Parse(info.Split("|")[2]);
        }

        public void StartButton()
        {
            NoteFalling.title = title.text.Replace("\n", " ");
            NoteFalling.music = audioSource.clip;
            NoteFalling.startTime = Time.time * 1000;
            SceneManager.LoadScene("MainGame");
        }

        public void JudgeCaliButton()
        {
            SceneManager.LoadScene("OffsetSettingScene");
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
