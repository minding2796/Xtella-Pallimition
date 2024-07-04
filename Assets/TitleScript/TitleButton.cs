using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace TitleScript
{
    public class TitleButton : MonoBehaviour
    {
        public AudioSource player;
        [FormerlySerializedAs("Musics")] public List<AudioClip> musics;

        private int _cur;
        // Start is called before the first frame update
        private void Start()
        {
            player.clip = musics[_cur = Random.Range(0, musics.Count)];
            player.Play();
        }
    
        public void Update()
        {
            if (!player.isPlaying)
            {
                player.clip = null;
                player.clip = musics[++_cur];
                player.Play();
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        public void StartButton()
        {
            SceneManager.LoadScene("SelectScene");
        }
    }
}
