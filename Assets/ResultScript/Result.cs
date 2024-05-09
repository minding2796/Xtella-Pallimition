using System.Globalization;
using MainGameScript;
using SelectScripts;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

namespace ResultScript
{
    public class Result : MonoBehaviour
    {
        private static Result _instance;
        public TextMeshProUGUI rank, pfc, gtc, gdc, bdc, msc, rate, title, combo;
        // Start is called before the first frame update
        private void Start()
        {
            _instance = this;
        }

        public static Result getInstance()
        {
            return _instance;
        }

        public void updateResult()
        {
            if (!NoteFalling.autoPlay && NoteFalling.speed >= 1.0) SelectMusic.updateRank(getRate());
            pfc.text = " " + Lines.pfc;
            gtc.text = " " + Lines.gtc;
            gdc.text = " " + Lines.gdc;
            bdc.text = " " + Lines.bdc;
            msc.text = " " + Lines.msc;
            var rt = getRate();
            rate.text = " " + SplitRate(rt);
            rank.text = getRank(rt, true);
            rank.color = getRankColor(rt);
            title.text = NoteFalling.title;
            combo.text = " " + Lines.maxCombo;
        }

        public static double getRate()
        {
            return ((double) (100 * Lines.pfc + 80 * Lines.gtc + 50 * Lines.gdc + 20 * Lines.bdc)) / (Lines.pfc + Lines.gtc + Lines.gdc + Lines.bdc + Lines.msc);
        }

        public static string SplitRate(double rate)
        {
            return rate.ToString(CultureInfo.InvariantCulture)[..math.min(5, rate.ToString(CultureInfo.InvariantCulture).Length)] + "%";
        }

    
        public static string getRank(double rate, bool checkUnrate)
        {
            if (((NoteFalling.speed < 1.0) || NoteFalling.autoPlay) && checkUnrate) return "X";
            return rate switch
            {
                >= 100 => "P",
                >= 97 => "S",
                >= 90 => "A",
                >= 80 => "B",
                >= 70 => "C",
                >= 60 => "D",
                _ => "F"
            };
        }

        public static Color getRankColor(double rate)
        {
            return rate switch
            {
                >= 100 => Color.magenta,
                >= 97 => Color.yellow,
                >= 90 => Color.red,
                >= 80 => Color.blue,
                >= 70 => Color.cyan,
                >= 60 => Color.gray,
                _ => Color.white
            };
        }
    }
}
