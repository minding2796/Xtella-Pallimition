using UnityEngine;
using UnityEngine.UI;

namespace AudioVisualizer
{
    public class AudioVisualizer : MonoBehaviour
    {
        public int size;
        public bool reverse;
        public GameObject bar;
        public Color color;
        
        private void Start()
        {
            for (var i = 0; i < size / 4; i++)
            {
                var inst = Instantiate(bar, transform);
                inst.GetComponent<RawImage>().color = color;
            }
        }

        private void Update()
        {
            var images = GetComponentsInChildren<RawImage>();
            var data = new float[size];
            AudioListener.GetSpectrumData(data, 0, FFTWindow.Rectangular);
            if (reverse)
            {
                var idx = 0;
                for (var i = images.Length - 1; i >= 0; i--) images[i].transform.localScale = new Vector3(1920f/images.Length, Mathf.Lerp(images[i].transform.localScale.y, data[idx++]*size*4, 0.1f));
            }
            else for (var i = 0; i < images.Length; i++) images[i].transform.localScale = new Vector3(1920f/images.Length, Mathf.Lerp(images[i].transform.localScale.y, data[i]*size*4, 0.1f));
        }
    }
}
