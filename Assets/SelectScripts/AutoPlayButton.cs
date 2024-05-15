using MainGameScript;
using UnityEngine;
using UnityEngine.UI;

namespace SelectScripts
{
    public class AutoPlayButton : MonoBehaviour
    {
        public RawImage img;
        public Texture on, off;

        private void Start()
        {
            img.texture = NoteFalling.autoPlay ? on : off;
        }

        public void AutoPlayHold()
        {
            NoteFalling.autoPlay = !NoteFalling.autoPlay;
            img.texture = NoteFalling.autoPlay ? on : off;
        }
    }
}
