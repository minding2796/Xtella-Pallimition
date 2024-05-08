using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class NoteFalling : MonoBehaviour
{
    public GameObject note;

    public AudioSource AudioSource;

    public static float nfd = 0;
    public static string title = "Test";
    public static AudioClip music;
    public static bool AutoPlay = false;
    public static float speed = 1.0f;

    public Queue<Tuple<Tuple<float, float>, int>> noteData;

    private static NoteFalling instance;

    public static float startTime;
    // Start is called before the first frame update
    public static NoteFalling getInstance()
    {
        return instance;
    }
    void Start()
    {
        AudioSource.pitch = speed;
        instance = this;
        AudioSource.clip = music;
        AudioSource.Play();
        noteData = NoteData.LoadData(title);
    }

    // Update is called once per frame
    void Update()
    {
        if (noteData.Count <= 0) return;
        while (noteData.Peek().Item1.Item1 <= (Time.time * 1000 + nfd - startTime) * speed)
        {
            if (noteData.Peek().Item1.Item1 - noteData.Peek().Item1.Item2 == 0)
            {
                GameObject noteInstance = Instantiate(note, getPosByLine(noteData.Peek().Item2, Math.Min(noteData.Peek().Item1.Item1 - (Time.time * 1000 + nfd - startTime) * speed, 0)), note.transform.rotation);
                noteInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -20);
                Lines.getInstance().addNote(noteData.Peek().Item2, noteInstance, noteData.Peek().Item1);
                noteData.Dequeue();
                if (noteData.Count <= 0) return;
            }
            else
            {
                GameObject noteInstance = Instantiate(note, getPosByLine(noteData.Peek().Item2, Math.Min(noteData.Peek().Item1.Item1 - (Time.time * 1000 + nfd - startTime) * speed, 0), noteData.Peek().Item1.Item2 - noteData.Peek().Item1.Item1), note.transform.rotation);
                noteInstance.transform.localScale = getNoteSize(noteData.Peek().Item1.Item2 - noteData.Peek().Item1.Item1);
                noteInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -20);
                Lines.getInstance().addNote(noteData.Peek().Item2, noteInstance, noteData.Peek().Item1);
                noteData.Dequeue();
                if (noteData.Count <= 0) return;
            }
        }
    }

    Vector3 getPosByLine(int line, float overtime)
    {
        float loc = overtime / 50 / speed;
        switch (line)
        {
            case 1:
                return new Vector3(-3, 6+loc);
            case 2:
                return new Vector3(-1, 6+loc);
            case 3:
                return new Vector3(1, 6+loc);
            case 4:
                return new Vector3(3, 6+loc);
        }
        return new Vector3(0, 0);
    }
    Vector3 getPosByLine(int line, float overtime, float size)
    {
        float cal = size / 50 / speed;
        float loc = overtime / 50 / speed;
        switch (line)
        {
            case 1:
                return new Vector3(-3, 6 + cal/2+loc);
            case 2:
                return new Vector3(-1, 6 + cal/2+loc);
            case 3:
                return new Vector3(1, 6 + cal/2+loc);
            case 4:
                return new Vector3(3, 6 + cal/2+loc);
        }

        return new Vector3(0, 0);
    }
    Vector3 getNoteSize(float size)
    {
        return new Vector3(1.3985f, 0.2734f + size/50/speed);
    }
}
