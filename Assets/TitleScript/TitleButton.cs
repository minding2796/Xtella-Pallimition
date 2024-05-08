using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButton : MonoBehaviour
{
    public AudioSource player;
    public List<AudioClip> Musics;
    // Start is called before the first frame update
    void Start()
    {
        player.clip = Musics[Random.Range(0, Musics.Count)];
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
