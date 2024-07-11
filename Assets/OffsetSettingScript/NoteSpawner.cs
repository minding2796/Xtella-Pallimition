using System;
using System.Collections.Generic;
using MainGameScript;
using TMPro;
using UnityEngine;

namespace OffsetSettingScript
{
    public class NoteSpawner : MonoBehaviour
    {
        public GameObject note;
        public AudioSource audioSource;
        public TextMeshProUGUI error;
        private readonly Queue<Tuple<Tuple<float, float>, int>> _noteData = new();
        private readonly Queue<Tuple<GameObject, float>> notes = new();

        private static float _startTime;
        private void Update()
        {
            if (!audioSource.isPlaying)
            {
                _noteData.Enqueue(new Tuple<Tuple<float, float>, int>(new Tuple<float, float>(0, 0), 4));
                _noteData.Enqueue(new Tuple<Tuple<float, float>, int>(new Tuple<float, float>(400, 400), 1));
                _noteData.Enqueue(new Tuple<Tuple<float, float>, int>(new Tuple<float, float>(800, 800), 4));
                _startTime = Time.time * 1000;
                while (notes.Count > 0) Destroy(notes.Dequeue().Item1);
                audioSource.Play();
            }
            while (_noteData.Count > 0 && _noteData.Peek().Item1.Item1 <= Time.time * 1000 + NoteFalling.nfd - _startTime)
            {
                var noteInstance = Instantiate(note,
                    getPosByLine(_noteData.Peek().Item2,
                        Math.Min(_noteData.Peek().Item1.Item1 - (Time.time * 1000 + NoteFalling.nfd - _startTime), 0)),
                    note.transform.rotation);
                noteInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -20);
                notes.Enqueue(new Tuple<GameObject, float>(noteInstance, _noteData.Peek().Item1.Item1));
                _noteData.Dequeue();
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (notes.Count != 0)
                {
                    error.text = "Judgement Offset : " + (-notes.Peek().Item2 + (Time.time * 1000 - _startTime - NoteFalling.nfd));
                    Destroy(notes.Dequeue().Item1);
                }
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (notes.Count != 0)
                {
                    error.text = "Judgement Offset : " + (-notes.Peek().Item2 + (Time.time * 1000 - _startTime - NoteFalling.nfd));
                    Destroy(notes.Dequeue().Item1);
                }
            }
            if (Input.GetKeyDown(KeyCode.Semicolon))
            {
                if (notes.Count != 0)
                {
                    error.text = "Judgement Offset : " + (-notes.Peek().Item2 + (Time.time * 1000 - _startTime - NoteFalling.nfd));
                    Destroy(notes.Dequeue().Item1);
                }
            }
            if (Input.GetKeyDown(KeyCode.Quote))
            {
                if (notes.Count != 0)
                {
                    error.text = "Judgement Offset : " + (-notes.Peek().Item2 + (Time.time * 1000 - _startTime - NoteFalling.nfd));
                    Destroy(notes.Dequeue().Item1);
                }
            }
        }
        private static Vector3 getPosByLine(int line, float overtime)
        {
            var loc = overtime / 50;
            return line switch
            {
                1 => new Vector3(-7, 6 + loc),
                2 => new Vector3(-5, 6 + loc),
                3 => new Vector3(-3, 6 + loc),
                4 => new Vector3(-1, 6 + loc),
                _ => new Vector3(0, 0)
            };
        }
    }
}
