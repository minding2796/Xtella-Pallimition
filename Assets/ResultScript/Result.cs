using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class Result : MonoBehaviour
{
    private static Result instance;
    public TextMeshProUGUI rank, pfc, gtc, gdc, bdc, msc, rate, title, combo;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public static Result getInstance()
    {
        return instance;
    }

    public void updateResult()
    {
        if (!NoteFalling.AutoPlay && NoteFalling.speed >= 1.0) SelectMusic.updateRank(getRate());
        pfc.text = " " + Lines.pfc;
        gtc.text = " " + Lines.gtc;
        gdc.text = " " + Lines.gdc;
        bdc.text = " " + Lines.bdc;
        msc.text = " " + Lines.msc;
        double rt = getRate();
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
        return rate.ToString().Substring(0, math.min(5, rate.ToString().Length)) + "%";
    }

    
    public static string getRank(double rate, bool checkUnrate)
    {
        if (((NoteFalling.speed < 1.0) || NoteFalling.AutoPlay) && checkUnrate) return "X";
        if (rate >= 100)
        {
            return "P";
        }
        if (rate >= 97)
        {
            return "S";
        }
        if (rate >= 90)
        {
            return "A";
        }
        if (rate >= 80)
        {
            return "B";
        }
        if (rate >= 70)
        {
            return "C";
        }
        if (rate >= 60)
        {
            return "D";
        }
        return "F";
    }

    public static Color getRankColor(double rate)
    {
        if (rate >= 100)
        {
            return Color.magenta;
        }
        if (rate >= 97)
        {
            return Color.yellow;
        }
        if (rate >= 90)
        {
            return Color.red;
        }
        if (rate >= 80)
        {
            return Color.blue;
        }
        if (rate >= 70)
        {
            return Color.cyan;
        }
        if (rate >= 60)
        {
            return Color.gray;
        }
        return Color.white;
    }
}
