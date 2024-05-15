using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SelectScripts
{
    public class SelectButtonInit : MonoBehaviour
    {
        public string data;
        public MusicInfo info;

        private void Start()
        {
            info.icon.texture = SelectMusic.getInstance().icons[int.Parse(data.Split("|")[2])];
            info.title.text = data.Split("|")[0];
            info.composer.text = data.Split("|")[1];
            info.bpm.text = "BPM : " + data.Split("|")[4];
            info.button.onClick.AddListener(() => SelectMusic.getInstance().SelectButton(data));
            var block = info.button.GetComponent<Button>().colors;
            Color.RGBToHSV(SelectMusic.getColorByLevel(data.Split("|")[3]), out var h, out var s, out var v);
            block.highlightedColor = Color.HSVToRGB(h, s - 0.1f, v);
            block.pressedColor = Color.HSVToRGB(h, s, v - 0.3f);
            block.selectedColor = SelectMusic.getColorByLevel(data.Split("|")[3]);
            info.button.GetComponent<Button>().colors = block;
        }
    }

    [Serializable]
    public class MusicInfo
    {
        public Button button;
        public RawImage icon;
        public TextMeshProUGUI title, composer, bpm;
    }
}
