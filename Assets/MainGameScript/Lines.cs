using System;
using System.Collections.Generic;
using System.Globalization;
using ResultScript;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

namespace MainGameScript
{
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
        public GameObject noteParticle;

        // Start is called before the first frame update
        private void Start()
        {
            _instance = this;
        }

        // Update is called once per frame
        private void Update()
        {
            var rt = ((double) (100 * pfc + 80 * gtc + 50 * gdc + 20 * bdc)) / (pfc + gtc + gdc + bdc + msc);
            rate.text = (rt.ToString(CultureInfo.InvariantCulture).Equals("NaN") ? "0" : rt.ToString(CultureInfo.InvariantCulture)[..math.min(5, rt.ToString(CultureInfo.InvariantCulture).Length)]) + "%";
            combo.text = currentCombo.ToString();
            comboTexts.SetActive(currentCombo != 0);
            if (maxCombo < currentCombo) maxCombo = currentCombo;
            if (Line1.Count <= 0 && Line2.Count <= 0 && Line3.Count <= 0 && Line4.Count <= 0 &&
                NoteFalling.getInstance().NoteData.Count <= 0)
            {
                mainCam.enabled = false;
                mainCam.GetComponent<AudioListener>().enabled = false;
                resultCam.enabled = true;
                resultCam.GetComponent<AudioListener>().enabled = true;
                Result.getInstance().updateResult();
            }

            if ((Input.GetKey(KeyCode.A) || NoteFalling.autoPlay) && Line1.Count > 0 && (Line1.Peek().Item1.Item1 - (Time.time * 1000 - NoteFalling.startTime - NoteFalling.nfd - jd) * NoteFalling.speed) < 0)
            {
                var time = (Time.time * 1000 - NoteFalling.startTime - NoteFalling.nfd - jd) * NoteFalling.speed;
                Line1.Peek().Item2.transform.localScale = NoteFalling.getNoteSize(Line1.Peek().Item1.Item2 - time);
                Line1.Peek().Item2.transform.position = NoteFalling.getPosByLine(1, time - (Time.time * 1000 + NoteFalling.nfd - NoteFalling.startTime) * NoteFalling.speed, Line1.Peek().Item1.Item2 - time);
            }
            if ((Input.GetKey(KeyCode.S) || NoteFalling.autoPlay) && Line2.Count > 0 && (Line2.Peek().Item1.Item1 - (Time.time * 1000 - NoteFalling.startTime - NoteFalling.nfd - jd) * NoteFalling.speed) < 0)
            {
                var time = (Time.time * 1000 - NoteFalling.startTime - NoteFalling.nfd - jd) * NoteFalling.speed;
                Line2.Peek().Item2.transform.localScale = NoteFalling.getNoteSize(Line2.Peek().Item1.Item2 - time);
                Line2.Peek().Item2.transform.position = NoteFalling.getPosByLine(2, time - (Time.time * 1000 + NoteFalling.nfd - NoteFalling.startTime) * NoteFalling.speed, Line2.Peek().Item1.Item2 - time);
            }

            if ((Input.GetKey(KeyCode.Semicolon) || NoteFalling.autoPlay) && Line3.Count > 0 && (Line3.Peek().Item1.Item1 - (Time.time * 1000 - NoteFalling.startTime - NoteFalling.nfd - jd) * NoteFalling.speed) < 0)
            {
                var time = (Time.time * 1000 - NoteFalling.startTime - NoteFalling.nfd - jd) * NoteFalling.speed;
                Line3.Peek().Item2.transform.localScale = NoteFalling.getNoteSize(Line3.Peek().Item1.Item2 - time);
                Line3.Peek().Item2.transform.position = NoteFalling.getPosByLine(3, time - (Time.time * 1000 + NoteFalling.nfd - NoteFalling.startTime) * NoteFalling.speed, Line3.Peek().Item1.Item2 - time);
            }

            if ((Input.GetKey(KeyCode.Quote) || NoteFalling.autoPlay) && Line4.Count > 0 && (Line4.Peek().Item1.Item1 - (Time.time * 1000 - NoteFalling.startTime - NoteFalling.nfd - jd) * NoteFalling.speed) < 0)
            {
                var time = (Time.time * 1000 - NoteFalling.startTime - NoteFalling.nfd - jd) * NoteFalling.speed;
                Line4.Peek().Item2.transform.localScale = NoteFalling.getNoteSize(Line4.Peek().Item1.Item2 - time);
                Line4.Peek().Item2.transform.position = NoteFalling.getPosByLine(4, time - (Time.time * 1000 + NoteFalling.nfd - NoteFalling.startTime) * NoteFalling.speed, Line4.Peek().Item1.Item2 - time);
            }
            
            if (NoteFalling.autoPlay) return;
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (Line1.Count != 0)
                {
                    var jg = CalcJudgement(Line1.Peek().Item1.Item1);
                    if (!jg.Equals("FB"))
                    {
                        judgement.text = jg;
                        if (Line1.Peek().Item1.Item1 - Line1.Peek().Item1.Item2 == 0)
                        {
                            Destroy(Instantiate(noteParticle, new Vector3(Line1.Peek().Item2.transform.position.x, -3.5f), Quaternion.Euler(0, 0, 0)),2f);
                            NoteFalling.getInstance().PoolingObjects.Enqueue(Line1.Peek().Item2);
                            Line1.Dequeue().Item2.SetActive(false);
                        }
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
                        var jg = CalcLongJudgement(Line1.Peek().Item1.Item2);
                        judgement.text = jg;
                        Destroy(Instantiate(noteParticle, new Vector3(Line1.Peek().Item2.transform.position.x, -3.5f), Quaternion.Euler(0, 0, 0)),2f);
                        NoteFalling.getInstance().PoolingObjects.Enqueue(Line1.Peek().Item2);
                        Line1.Dequeue().Item2.SetActive(false);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                if (Line2.Count != 0)
                {
                    var jg = CalcJudgement(Line2.Peek().Item1.Item1);
                    if (!jg.Equals("FB"))
                    {
                        judgement.text = jg;
                        if (Line2.Peek().Item1.Item1 - Line2.Peek().Item1.Item2 == 0)
                        {
                            Destroy(Instantiate(noteParticle, new Vector3(Line2.Peek().Item2.transform.position.x, -3.5f), Quaternion.Euler(0, 0, 0)),2f);
                            NoteFalling.getInstance().PoolingObjects.Enqueue(Line2.Peek().Item2);
                            Line2.Dequeue().Item2.SetActive(false);
                        }
                        else line2Active = true;
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
                        var jg = CalcLongJudgement(Line2.Peek().Item1.Item2);
                        judgement.text = jg;
                        Destroy(Instantiate(noteParticle, new Vector3(Line2.Peek().Item2.transform.position.x, -3.5f), Quaternion.Euler(0, 0, 0)),2f);
                        NoteFalling.getInstance().PoolingObjects.Enqueue(Line2.Peek().Item2);
                        Line2.Dequeue().Item2.SetActive(false);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Semicolon))
            {
                if (Line3.Count != 0)
                {
                    var jg = CalcJudgement(Line3.Peek().Item1.Item1);
                    if (!jg.Equals("FB"))
                    {
                        judgement.text = jg;
                        if (Line3.Peek().Item1.Item1 - Line3.Peek().Item1.Item2 == 0)
                        {
                            Destroy(Instantiate(noteParticle, new Vector3(Line3.Peek().Item2.transform.position.x, -3.5f), Quaternion.Euler(0, 0, 0)),2f);
                            NoteFalling.getInstance().PoolingObjects.Enqueue(Line3.Peek().Item2);
                            Line3.Dequeue().Item2.SetActive(false);
                        }
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
                        var jg = CalcLongJudgement(Line3.Peek().Item1.Item2);
                        judgement.text = jg;
                        Destroy(Instantiate(noteParticle, new Vector3(Line3.Peek().Item2.transform.position.x, -3.5f), Quaternion.Euler(0, 0, 0)),2f);
                        NoteFalling.getInstance().PoolingObjects.Enqueue(Line3.Peek().Item2);
                        Line3.Dequeue().Item2.SetActive(false);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Quote))
            {
                if (Line4.Count != 0)
                {
                    var jg = CalcJudgement(Line4.Peek().Item1.Item1);
                    if (!jg.Equals("FB"))
                    {
                        judgement.text = jg;
                        if (Line4.Peek().Item1.Item1 - Line4.Peek().Item1.Item2 == 0)
                        {
                            Destroy(Instantiate(noteParticle, new Vector3(Line4.Peek().Item2.transform.position.x, -3.5f), Quaternion.Euler(0, 0, 0)),2f);
                            NoteFalling.getInstance().PoolingObjects.Enqueue(Line4.Peek().Item2);
                            Line4.Dequeue().Item2.SetActive(false);
                        }
                        else line4Active = true;
                    }
                }
            }

            if (!Input.GetKeyUp(KeyCode.Quote)) return;
            {
                if (!line4Active) return;
                line4Active = false;
                if (Line4.Count == 0) return;
                var jg = CalcLongJudgement(Line4.Peek().Item1.Item2);
                judgement.text = jg;
                Destroy(Instantiate(noteParticle, new Vector3(Line4.Peek().Item2.transform.position.x, -3.5f), Quaternion.Euler(0, 0, 0)),2f);
                NoteFalling.getInstance().PoolingObjects.Enqueue(Line4.Peek().Item2);
                Line4.Dequeue().Item2.SetActive(false);
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
            if (NoteFalling.autoPlay)
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
                    Destroy(Instantiate(noteParticle, new Vector3(Line1.Peek().Item2.transform.position.x, -3.5f), Quaternion.Euler(0, 0, 0)),2f);
                    NoteFalling.getInstance().PoolingObjects.Enqueue(Line1.Peek().Item2);
                    Line1.Dequeue().Item2.SetActive(false);
                    break;
                case 2:
                    Destroy(Instantiate(noteParticle, new Vector3(Line2.Peek().Item2.transform.position.x, -3.5f), Quaternion.Euler(0, 0, 0)),2f);
                    NoteFalling.getInstance().PoolingObjects.Enqueue(Line2.Peek().Item2);
                    Line2.Dequeue().Item2.SetActive(false);
                    break;
                case 3:
                    Destroy(Instantiate(noteParticle, new Vector3(Line3.Peek().Item2.transform.position.x, -3.5f), Quaternion.Euler(0, 0, 0)),2f);
                    NoteFalling.getInstance().PoolingObjects.Enqueue(Line3.Peek().Item2);
                    Line3.Dequeue().Item2.SetActive(false);
                    break;
                case 4:
                    Destroy(Instantiate(noteParticle, new Vector3(Line4.Peek().Item2.transform.position.x, -3.5f), Quaternion.Euler(0, 0, 0)),2f);
                    NoteFalling.getInstance().PoolingObjects.Enqueue(Line4.Peek().Item2);
                    Line4.Dequeue().Item2.SetActive(false);
                    break;
            }
        }

        public static Lines getInstance()
        {
            return _instance;
        }

        public static int pfc, gtc, gdc, bdc, msc, currentCombo, maxCombo;

        private static string CalcJudgement(float noteTime)
        {
            // return noteTime + "/" + (Time.time * 1000 - NoteFalling.startTime);
            var error = noteTime - (Time.time * 1000 - NoteFalling.startTime - NoteFalling.nfd - jd) * NoteFalling.speed;
            error /= 1000;
            if (error > 0.3f) return "FB";
            error = Math.Abs(error);
            switch (error)
            {
                case <= 0.05f:
                    pfc++;
                    currentCombo++;
                    return "Perfect!";
                case <= 0.08f:
                    gtc++;
                    currentCombo++;
                    return "Great!";
                case <= 0.11f:
                    gdc++;
                    currentCombo++;
                    return "Good";
                case <= 0.15f:
                    bdc++;
                    currentCombo++;
                    return "Bad..";
            }
            msc++;
            currentCombo = 0;
            return "Miss..";
        }
        private static string CalcLongJudgement(float noteTime)
        {
            // return noteTime + "/" + (Time.time * 1000 - NoteFalling.startTime);
            var error = noteTime - (Time.time * 1000 - NoteFalling.startTime - NoteFalling.nfd - jd) * NoteFalling.speed;
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
            return v[0] switch
            {
                -3 => 1,
                -1 => 2,
                1 => 3,
                3 => 4,
                _ => 0
            };
        }
    }
}
