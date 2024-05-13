using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SelectScripts
{
    public class SortButton : MonoBehaviour
    {
        public TextMeshProUGUI cycleText;
        public Image indec;
        public Sprite inc, dec;
        private static int _cycle = 3;
        private static bool _inc = true;

        private void Start()
        {
            SelectMusic.getInstance().SortMusic(_cycle, _inc);
            indec.sprite = _inc ? inc : dec;
            cycleText.text = getIdx(_cycle);
        }

        public void cycle()
        {
            SelectMusic.getInstance().SortMusic(_cycle = ++_cycle % 5, _inc);
            cycleText.text = getIdx(_cycle);
        }

        public void idc()
        {
            SelectMusic.getInstance().SortMusic(_cycle, _inc = !_inc);
            indec.sprite = _inc ? inc : dec;
        }

        private static string getIdx(int i)
        {
            return i switch
            {
                0 => "TITLE",
                1 => "COMPOSER",
                2 => "ADDED DATE",
                4 => "BPM",
                _ => "LEVEL"
            };
        }
    }
}
