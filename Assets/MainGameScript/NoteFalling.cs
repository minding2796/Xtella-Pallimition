using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace MainGameScript
{
    public class NoteFalling : MonoBehaviour
    {
        public GameObject note;

        [FormerlySerializedAs("AudioSource")] public AudioSource audioSource;

        public static float nfd = 0;
        public static string title = "Test";
        public static AudioClip music;
        public static bool autoPlay = false;
        public static float speed = 1.0f;

        public Queue<Tuple<Tuple<float, float>, int>> NoteData;

        private static NoteFalling _instance;

        public static float startTime;
        // Start is called before the first frame update
        public static NoteFalling getInstance()
        {
            return _instance;
        }

        private void Start()
        {
            audioSource.pitch = speed;
            _instance = this;
            audioSource.clip = music;
            audioSource.Play();
            NoteData = MainGameScript.NoteData.LoadData(title);
        }

        // Update is called once per frame
        private void Update()
        {
            if (NoteData.Count <= 0) return;
            while (NoteData.Peek().Item1.Item1 <= (Time.time * 1000 + nfd - startTime) * speed)
            {
                if (NoteData.Peek().Item1.Item1 - NoteData.Peek().Item1.Item2 == 0)
                {
                    var noteInstance = Instantiate(note, getPosByLine(NoteData.Peek().Item2, Math.Min(NoteData.Peek().Item1.Item1 - (Time.time * 1000 + nfd - startTime) * speed, 0)), note.transform.rotation);
                    noteInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -20);
                    Lines.getInstance().addNote(NoteData.Peek().Item2, noteInstance, NoteData.Peek().Item1);
                    NoteData.Dequeue();
                    if (NoteData.Count <= 0) return;
                }
                else
                {
                    var noteInstance = Instantiate(note, getPosByLine(NoteData.Peek().Item2, Math.Min(NoteData.Peek().Item1.Item1 - (Time.time * 1000 + nfd - startTime) * speed, 0), NoteData.Peek().Item1.Item2 - NoteData.Peek().Item1.Item1), note.transform.rotation);
                    noteInstance.transform.localScale = getNoteSize(NoteData.Peek().Item1.Item2 - NoteData.Peek().Item1.Item1);
                    noteInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -20);
                    Lines.getInstance().addNote(NoteData.Peek().Item2, noteInstance, NoteData.Peek().Item1);
                    NoteData.Dequeue();
                    if (NoteData.Count <= 0) return;
                }
            }
        }

        private static Vector3 getPosByLine(int line, float overtime)
        {
            var loc = overtime / 50 / speed;
            return line switch
            {
                1 => new Vector3(-3, 6 + loc),
                2 => new Vector3(-1, 6 + loc),
                3 => new Vector3(1, 6 + loc),
                4 => new Vector3(3, 6 + loc),
                _ => new Vector3(0, 0)
            };
        }

        public static Vector3 getPosByLine(int line, float overtime, float size)
        {
            var cal = size / 50 / speed;
            var loc = overtime / 50 / speed;
            return line switch
            {
                1 => new Vector3(-3, 6 + cal / 2 + loc),
                2 => new Vector3(-1, 6 + cal / 2 + loc),
                3 => new Vector3(1, 6 + cal / 2 + loc),
                4 => new Vector3(3, 6 + cal / 2 + loc),
                _ => new Vector3(0, 0)
            };
        }

        public static Vector3 getNoteSize(float size)
        {
            return new Vector3(1.3985f, 0.2734f + size/50/speed);
        }
    }
}
