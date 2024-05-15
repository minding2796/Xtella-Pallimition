using System;
using ResultScript;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MainGameScript
{
    public class ActiveLines : MonoBehaviour
    {
        public GameObject l1, l2, l3, l4, d1, d2, d3, d4;
        [FormerlySerializedAs("PE")] public RawImage pe;

        private void Start()
        {
            d1.SetActive(true);
            d2.SetActive(true);
            d3.SetActive(true);
            d4.SetActive(true);
        }

        // Update is called once per frame
        private void Update()
        {
            l1.SetActive(Input.GetKey(KeyCode.A));
            l2.SetActive(Input.GetKey(KeyCode.S));
            l3.SetActive(Input.GetKey(KeyCode.Semicolon));
            l4.SetActive(Input.GetKey(KeyCode.Quote));
            if (Input.GetKeyDown(KeyCode.A)) d1.SetActive(false);
            if (Input.GetKeyDown(KeyCode.S)) d2.SetActive(false);
            if (Input.GetKeyDown(KeyCode.Semicolon)) d3.SetActive(false);
            if (Input.GetKeyDown(KeyCode.Quote)) d4.SetActive(false);
            pe.color = new Color(255, 255, 255, Result.getRate() >= 100 ? Math.Min(pe.color.a+0.01f, 1) : Math.Max(pe.color.a-0.01f, 0));
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Lines.currentCombo = 0;
                Lines.maxCombo = 0;
                Lines.pfc = 0;
                Lines.gtc = 0;
                Lines.gdc = 0;
                Lines.bdc = 0;
                Lines.msc = 0;
                SceneManager.LoadScene("SelectScene");
            }
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                Lines.currentCombo = 0;
                Lines.maxCombo = 0;
                Lines.pfc = 0;
                Lines.gtc = 0;
                Lines.gdc = 0;
                Lines.bdc = 0;
                Lines.msc = 0;
                NoteFalling.startTime = Time.time * 1000;
                SceneManager.LoadScene("MainGame");
            }
            if (Input.GetKeyDown(KeyCode.F12))
            {
                ScreenCapture.CaptureScreenshot("../Screenshots/Screenshot-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".png");
            }
        }
    }
}
