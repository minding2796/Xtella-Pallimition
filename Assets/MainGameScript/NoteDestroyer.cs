using UnityEngine;

public class NoteDestroyer : MonoBehaviour
{
    private void Update()
    {
        float err = -150f;
        if (NoteFalling.AutoPlay) err = 0;
        if (Lines.getInstance().Line1.Count != 0)
        {
            if (!NoteFalling.AutoPlay)
            {
                while (Lines.getInstance().Line1.Peek().Item1.Item1 - (Time.time * 1000 - NoteFalling.startTime - Lines.jd) * NoteFalling.speed < err)
                {
                    if (Lines.getInstance().line1Active) break;
                    if (Lines.getInstance().Line1.Peek().Item1.Item2 - Lines.getInstance().Line1.Peek().Item1.Item1 != 0)
                    {
                        Lines.currentCombo = 0;
                        Lines.msc++;
                    }
                    Lines.getInstance().removeNote(Lines.getInstance().Line1.Peek().Item2.transform.position);
                    if (Lines.getInstance().Line1.Count == 0) break;
                }
            }
        }
        if (Lines.getInstance().Line1.Count != 0)
        {
            while (Lines.getInstance().Line1.Peek().Item1.Item2 - (Time.time * 1000 - NoteFalling.startTime - Lines.jd) * NoteFalling.speed < err)
            {
                if (NoteFalling.AutoPlay && Lines.getInstance().Line1.Peek().Item1.Item2 - Lines.getInstance().Line1.Peek().Item1.Item1 != 0)
                {
                    Lines.currentCombo++;
                    Lines.pfc++;
                }
                Lines.getInstance().line1Active = false;
                Lines.getInstance().removeNote(Lines.getInstance().Line1.Peek().Item2.transform.position);
                if (Lines.getInstance().Line1.Count == 0) break;
            }
        }
        if (Lines.getInstance().Line2.Count != 0)
        {
            if (!NoteFalling.AutoPlay)
            {
                while (Lines.getInstance().Line2.Peek().Item1.Item1 - (Time.time * 1000 - NoteFalling.startTime - Lines.jd) * NoteFalling.speed < err)
                {
                    if (Lines.getInstance().line2Active) break;
                    if (Lines.getInstance().Line2.Peek().Item1.Item2 - Lines.getInstance().Line2.Peek().Item1.Item1 != 0)
                    {
                        Lines.currentCombo = 0;
                        Lines.msc++;
                    }
                    Lines.getInstance().removeNote(Lines.getInstance().Line2.Peek().Item2.transform.position);
                    if (Lines.getInstance().Line2.Count == 0) break;
                }
            }
        }
        if (Lines.getInstance().Line2.Count != 0)
        {
            while (Lines.getInstance().Line2.Peek().Item1.Item2 - (Time.time * 1000 - NoteFalling.startTime - Lines.jd) * NoteFalling.speed < err)
            {
                if (NoteFalling.AutoPlay && Lines.getInstance().Line2.Peek().Item1.Item2 - Lines.getInstance().Line2.Peek().Item1.Item1 != 0)
                {
                    Lines.currentCombo++;
                    Lines.pfc++;
                }
                Lines.getInstance().line2Active = false;
                Lines.getInstance().removeNote(Lines.getInstance().Line2.Peek().Item2.transform.position);
                if (Lines.getInstance().Line2.Count == 0) break;
            }
        }
        if (Lines.getInstance().Line3.Count != 0)
        {
            if (!NoteFalling.AutoPlay)
            {
                while (Lines.getInstance().Line3.Peek().Item1.Item1 - (Time.time * 1000 - NoteFalling.startTime - Lines.jd) * NoteFalling.speed < err)
                {
                    if (Lines.getInstance().line3Active) break;
                    if (Lines.getInstance().Line3.Peek().Item1.Item2 - Lines.getInstance().Line3.Peek().Item1.Item1 != 0)
                    {
                        Lines.currentCombo = 0;
                        Lines.msc++;
                    }
                    Lines.getInstance().removeNote(Lines.getInstance().Line3.Peek().Item2.transform.position);
                    if (Lines.getInstance().Line3.Count == 0) break;
                }
            }
        }
        if (Lines.getInstance().Line3.Count != 0)
        {
            while (Lines.getInstance().Line3.Peek().Item1.Item2 - (Time.time * 1000 - NoteFalling.startTime - Lines.jd) * NoteFalling.speed < err)
            {
                if (NoteFalling.AutoPlay && Lines.getInstance().Line3.Peek().Item1.Item2 - Lines.getInstance().Line3.Peek().Item1.Item1 != 0)
                {
                    Lines.currentCombo++;
                    Lines.pfc++;
                }
                Lines.getInstance().line3Active = false;
                Lines.getInstance().removeNote(Lines.getInstance().Line3.Peek().Item2.transform.position);
                if (Lines.getInstance().Line3.Count == 0) break;
            }
        }
        if (Lines.getInstance().Line4.Count != 0)
        {
            if (!NoteFalling.AutoPlay)
            {
                while (Lines.getInstance().Line4.Peek().Item1.Item1 - (Time.time * 1000 - NoteFalling.startTime - Lines.jd) * NoteFalling.speed < err)
                {
                    if (Lines.getInstance().line4Active) break;
                    if (Lines.getInstance().Line4.Peek().Item1.Item2 - Lines.getInstance().Line4.Peek().Item1.Item1 != 0)
                    {
                        Lines.currentCombo = 0;
                        Lines.msc++;
                    }
                    Lines.getInstance().removeNote(Lines.getInstance().Line4.Peek().Item2.transform.position);
                    if (Lines.getInstance().Line4.Count == 0) break;
                }
            }
        }
        if (Lines.getInstance().Line4.Count != 0)
        {
            while (Lines.getInstance().Line4.Peek().Item1.Item2 - (Time.time * 1000 - NoteFalling.startTime - Lines.jd) * NoteFalling.speed < err)
            {
                if (NoteFalling.AutoPlay && Lines.getInstance().Line4.Peek().Item1.Item2 - Lines.getInstance().Line4.Peek().Item1.Item1 != 0)
                {
                    Lines.currentCombo++;
                    Lines.pfc++;
                }
                Lines.getInstance().line4Active = false;
                Lines.getInstance().removeNote(Lines.getInstance().Line4.Peek().Item2.transform.position);
                if (Lines.getInstance().Line4.Count == 0) break;
            }
        }
    }
}
