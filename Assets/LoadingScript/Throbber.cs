using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Quaternion;

namespace LoadingScript
{
    public class Throbber : MonoBehaviour
    {
        private RawImage _img;
        public TextMeshProUGUI loadingText;
        public Image processBar;
        public Image subProcessBar;
        private Transform _transform;
        private float _z;
        private int dotState = 1;
        private float dotStateTimer;
        public static bool loaded;

        private void Start()
        {
            _transform = GetComponent<Transform>();
            _img = GetComponent<RawImage>();
        }

        private void Update()
        {
            processBar.color = _img.color = loadingText.color = Color.Lerp(loadingText.color, loaded ? new Color(1, 1, 1, 0) : Color.white, Time.deltaTime * 2);
            subProcessBar.color = new Color(1, 1, 1, processBar.color.a / 4);
            _z += Time.deltaTime * -720;
            _transform.rotation = Euler(0, 0, _z);
            dotStateTimer += Time.deltaTime;
            if (dotStateTimer > 1)
            {
                dotState = dotState % 3 + 1;
                dotStateTimer = 0;
            }
            loadingText.text = "Now Loading" + new string('.', dotState);
            if (_img.color.a < 0.01f) SceneManager.LoadScene("SelectScene");
        }
    }
}
