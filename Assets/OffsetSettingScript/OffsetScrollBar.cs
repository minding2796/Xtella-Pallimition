using System;
using MainGameScript;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace OffsetSettingScript
{
    public class OffsetScrollBar : MonoBehaviour
    {
        public TMP_InputField nfd, jd;

        private void Start()
        {
            nfd.text = NoteFalling.nfd + "";
            jd.text = Lines.jd + "";
            OnTextChanged(nfd);
            OnTextChanged(jd);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) Submit();
        }

        public void OnValueChanged(Scrollbar sb)
        {
            sb.GetComponentInParent<RawImage>().GetComponentInChildren<TMP_InputField>().text = (int) (Math.Round(sb.value * 2000) - 1000) + "";
            NoteFalling.nfd = float.Parse(nfd.text);
            Lines.jd = float.Parse(jd.text);
        }
        public void OnTextChanged(TMP_InputField tif)
        {
            tif.GetComponentInParent<RawImage>().GetComponentInChildren<Scrollbar>().value = Math.Max(Math.Min((int.Parse(tif.text) + 1000), 2000), 0) / 2000f;
            NoteFalling.nfd = float.Parse(nfd.text);
            Lines.jd = float.Parse(jd.text);
        }

        public void Submit()
        {
            NoteData.SaveOffsets(NoteFalling.nfd, Lines.jd);
            SceneManager.LoadScene("SelectScene");
        }
    }
}
