using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectMusic : MonoBehaviour
{
    public AudioSource AudioSource;
    public List<AudioClip> Musics;
    public List<Texture> icons;
    public TMP_InputField nfd, jd;
    public TextMeshProUGUI title, composer, level, speed;
    public List<TextMeshProUGUI> isPP;
    private static List<double> ranks = new();
    public RawImage icon, checkBox;
    private int musicIdx = 9;
    private static SelectMusic instance;

    public void Awake()
    {
        if (!Directory.Exists("Screenshots/")) Directory.CreateDirectory("Screenshots/");
        if (!Directory.Exists("Results/")) Directory.CreateDirectory("Results/");
        if (!File.Exists("Results/results.r")) File.Create("Results/results.r");
        ranks = NoteData.LoadRanks();
        while (ranks.Count < isPP.Count()) ranks.Add(0);
        instance = this;
        AudioSource.time = 88.250f;
        checkBox.color = NoteFalling.AutoPlay ? new Color(255, 255, 255, 255) : new Color(255, 255, 255, 0);
        speed.text = (NoteFalling.speed == 1 ? "1.0" : (NoteFalling.speed == 2 ? "2.0" : NoteFalling.speed)) + "x";
        AudioSource.pitch = NoteFalling.speed;
    }

    public void Update()
    {
        for (int i = 0; i < isPP.Count; i++)
        {
            isPP[i].text = Result.SplitRate(ranks[i]) + " " + Result.getRank(ranks[i], false);
            isPP[i].color =  Result.getRankColor(ranks[i]);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public static void updateRank(double rate)
    {
        if (ranks[instance.musicIdx] < rate) ranks[instance.musicIdx] = rate;
        NoteData.SaveRanks(ranks);
    }

    public void SelectButton(string info)
    {
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
        AudioSource.clip = null;
        AudioSource.clip = Musics[int.Parse(info.Split("|")[2])];
        AudioSource.time = float.Parse(info.Split("|")[4]) / 1000;
        AudioSource.Play();
        musicIdx = int.Parse(info.Split("|")[2]);
    }

    public void StartButton()
    {
        NoteFalling.title = title.text.Replace("\n", " ");
        NoteFalling.music = AudioSource.clip;
        NoteFalling.nfd = float.Parse(nfd.text);
        Lines.jd = float.Parse(jd.text);
        NoteFalling.startTime = Time.time * 1000;
        SceneManager.LoadScene("MainGame");
    }

    public void AutoPlayButton()
    {
        NoteFalling.AutoPlay = !NoteFalling.AutoPlay;
        checkBox.color = NoteFalling.AutoPlay ? new Color(255, 255, 255, 255) : new Color(255, 255, 255, 0);
    }

    public void SpeedDecreaseButton()
    {
        speed.text = Math.Max(float.Parse(speed.text.Substring(0, speed.text.Length - 1)) - 0.09, 0.5).ToString().Substring(0, 3) + "x";
        NoteFalling.speed = float.Parse(speed.text.Substring(0, speed.text.Length - 1));
        AudioSource.pitch = NoteFalling.speed;
    }

    public void SpeedIncreaseButton()
    {
        speed.text = Math.Min(float.Parse(speed.text.Substring(0, speed.text.Length - 1)) + 0.11, ranks.TrueForAll(s => s == 100) ? 2.55 : 2.05).ToString().Substring(0, 3) + "x";
        NoteFalling.speed = float.Parse(speed.text.Substring(0, speed.text.Length - 1));
        AudioSource.pitch = NoteFalling.speed;
    }

    public void SpeedResetButton()
    {
        speed.text = "1.0x";
        NoteFalling.speed = float.Parse(speed.text.Substring(0, speed.text.Length - 1));
        AudioSource.pitch = NoteFalling.speed;
    }
}
