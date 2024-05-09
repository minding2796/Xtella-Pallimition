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
        // Start is called before the first frame update
        private void Start()
        {
            player.clip = musics[Random.Range(0, musics.Count)];
            player.Play();
        }
    
        public void Update()
        {
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
