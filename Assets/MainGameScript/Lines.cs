using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.Mathematics;

public class Lines : MonoBehaviour
{
    public readonly Queue<Tuple<Tuple<float, float>, GameObject>> Line1 = new();
    public readonly Queue<Tuple<Tuple<float, float>, GameObject>> Line2 = new();
    public readonly Queue<Tuple<Tuple<float, float>, GameObject>> Line3 = new();
    public readonly Queue<Tuple<Tuple<float, float>, GameObject>> Line4 = new();
    public bool line1Active, line2Active, line3Active, line4Active;
    public static float jd = 0;
    public Camera mainCam, resultCam;
    private static Lines _instance;
    public TextMeshProUGUI judgement, rate, combo;
    public GameObject comboTexts;

    // Start is called before the first frame update
    void Start()
    {
        _instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        double rt = ((double) (100 * pfc + 80 * gtc + 50 * gdc + 20 * bdc)) / (pfc + gtc + gdc + bdc + msc);
        rate.text = (rt.ToString().Equals("NaN") ? "0" : rt.ToString().Substring(0, math.min(5, rt.ToString().Length))) + "%";
        combo.text = currentCombo.ToString();
        comboTexts.SetActive(currentCombo != 0);
        if (maxCombo < currentCombo) maxCombo = currentCombo;
        if (Line1.Count <= 0 && Line2.Count <= 0 && Line3.Count <= 0 && Line4.Count <= 0 &&
            NoteFalling.getInstance().noteData.Count <= 0)
        {
            mainCam.enabled = false;
            resultCam.enabled = true;
            Result.getInstance().updateResult();
        }

        if (NoteFalling.AutoPlay) return;
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (Line1.Count != 0)
            {
                string jg = CalcJudgement(Line1.Peek().Item1.Item1);
                if (!jg.Equals("FB"))
                {
                    judgement.text = jg;
                    if (Line1.Peek().Item1.Item1 - Line1.Peek().Item1.Item2 == 0) Destroy(Line1.Dequeue().Item2);
                    else line1Active = true;
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            if (line1Active)
            {
                line1Active = false;
                if (Line1.Count != 0)
                {
                    string jg = CalcLongJudgement(Line1.Peek().Item1.Item2);
                    judgement.text = jg;
                    Destroy(Line1.Dequeue().Item2);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (Line2.Count != 0)
            {
                string jg = CalcJudgement(Line2.Peek().Item1.Item1);
                if (!jg.Equals("FB"))
                {
                    judgement.text = jg;
                    if (Line2.Peek().Item1.Item1 - Line2.Peek().Item1.Item2 == 0)
                    {
                        Destroy(Line2.Dequeue().Item2);
                    }
                    else
                    {
                        line2Active = true;
                    }
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            if (line2Active)
            {
                line2Active = false;
                if (Line2.Count != 0)
                {
                    string jg = CalcLongJudgement(Line2.Peek().Item1.Item2);
                    judgement.text = jg;
                    Destroy(Line2.Dequeue().Item2);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Semicolon))
        {
            if (Line3.Count != 0)
            {
                string jg = CalcJudgement(Line3.Peek().Item1.Item1);
                if (!jg.Equals("FB"))
                {
                    judgement.text = jg;
                    if (Line3.Peek().Item1.Item1 - Line3.Peek().Item1.Item2 == 0) Destroy(Line3.Dequeue().Item2);
                    else line3Active = true;
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.Semicolon))
        {
            if (line3Active)
            {
                line3Active = false;
                if (Line3.Count != 0)
                {
                    string jg = CalcLongJudgement(Line3.Peek().Item1.Item2);
                    judgement.text = jg;
                    Destroy(Line3.Dequeue().Item2);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Quote))
        {
            if (Line4.Count != 0)
            {
                string jg = CalcJudgement(Line4.Peek().Item1.Item1);
                if (!jg.Equals("FB"))
                {
                    judgement.text = jg;
                    if (Line4.Peek().Item1.Item1 - Line4.Peek().Item1.Item2 == 0)
                    {
                        Destroy(Line4.Dequeue().Item2);
                    }
                    else
                    {
                        line4Active = true;
                    }
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.Quote))
        {
            if (line4Active)
            {
                line4Active = false;
                if (Line4.Count != 0)
                {
                    string jg = CalcLongJudgement(Line4.Peek().Item1.Item2);
                    judgement.text = jg;
                    Destroy(Line4.Dequeue().Item2);
                }
            }
        }
    }

    public void addNote(int line, GameObject note, Tuple<float, float> time)
    {
        switch (line)
        {
            case 1:
                Line1.Enqueue(new Tuple<Tuple<float, float>, GameObject>(time, note));
                break;
            case 2:
                Line2.Enqueue(new Tuple<Tuple<float, float>, GameObject>(time, note));
                break;
            case 3:
                Line3.Enqueue(new Tuple<Tuple<float, float>, GameObject>(time, note));
                break;
            case 4:
                Line4.Enqueue(new Tuple<Tuple<float, float>, GameObject>(time, note));
                break;
        }
    }

    public void removeNote(Vector3 line)
    {
        removeNote(GetLineByVector(line));
    }

    private void removeNote(int line)
    {
        if (NoteFalling.AutoPlay)
        {
            pfc++;
            currentCombo++;
            judgement.text = "Perfect!";
        }
        else
        {
            msc++;
            currentCombo = 0;
            judgement.text = "Miss..";
        }
        switch (line)
        {
            case 1:
                Destroy(Line1.Dequeue().Item2);
                break;
            case 2:
                Destroy(Line2.Dequeue().Item2);
                break;
            case 3:
                Destroy(Line3.Dequeue().Item2);
                break;
            case 4:
                Destroy(Line4.Dequeue().Item2);
                break;
        }
    }

    public static Lines getInstance()
    {
        return _instance;
    }

    public static int pfc, gtc, gdc, bdc, msc, currentCombo, maxCombo;

    private string CalcJudgement(float noteTime)
    {
        // return noteTime + "/" + (Time.time * 1000 - NoteFalling.startTime);
        float error = noteTime - (Time.time * 1000 - NoteFalling.startTime - jd) * NoteFalling.speed;
        error /= 1000;
        if (error > 0.3f) return "FB";
        error = Math.Abs(error);
        if (error <= 0.05f)
        {
            pfc++;
            currentCombo++;
            return "Perfect!";
        }

        if (error <= 0.08f)
        {
            gtc++;
            currentCombo++;
            return "Great!";
        }

        if (error <= 0.11f)
        {
            gdc++;
            currentCombo++;
            return "Good";
        }

        if (error <= 0.15f)
        {
            bdc++;
            currentCombo++;
            return "Bad..";
        }

        msc++;
        currentCombo = 0;
        return "Miss..";
    }
    private string CalcLongJudgement(float noteTime)
    {
        // return noteTime + "/" + (Time.time * 1000 - NoteFalling.startTime);
        float error = noteTime - (Time.time * 1000 - NoteFalling.startTime - jd) * NoteFalling.speed;
        error /= 1000;
        error = Math.Abs(error);
        if (error <= 0.15f)
        {
            currentCombo++;
            pfc++;
            return "Perfect!";
        }
        currentCombo = 0;
        msc++;
        return "Miss..";
    }

    private static int GetLineByVector(Vector3 v)
    {
        switch (v[0])
        {
            case -3:
                return 1;
            case -1:
                return 2;
            case 1:
                return 3;
            case 3:
                return 4;
        }

        return 0;
    }
}
