using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Oscillator : MonoBehaviour
{
    public double frequency = 440.0;
    private double increment;
    private double phase;
    private double samplingFrequency = 48000.0;

    public float gain;
    public float volume = .1f;
    public float[] frequencies;
    public int thisFreq;

    private void Start()
    {
        frequencies = new float[12];
        frequencies[0] = 261.63f;
        frequencies[1] = 277.18f;
        frequencies[2] = 293.66f;
        frequencies[3] = 311.13f;
        frequencies[4] = 329.63f;
        frequencies[5] = 349.23f;
        frequencies[6] = 369.99f;
        frequencies[7] = 392.00f;
        frequencies[8] = 415.3f;
        frequencies[9] = 440f;
        frequencies[10] = 466.16f;
        frequencies[11] = 493.88f;
        gain = 0;
    }

    public IEnumerator playNote(float freq, float time)
    {
        frequency = freq;
        gain = volume;
        yield return new WaitForSeconds(time);
        gain = 0;
    }
    public float frequencyChecker(float oldfreq, float freq)
    {
        if (freq < oldfreq)
        {
            freq *= 2;
            if (freq < oldfreq)
            {
                freq *= 2;
            }
        }
        return freq;
    }

    public IEnumerator dropNote(float freq)
    {
        for (int x = 0; x < 300; x++)
        {
            frequency = freq;
            freq -= x;
            yield return new WaitForSeconds(0.02f);
        }
        gain = 0;
    }
    public float[] OrderFrequencies(float firstFreq, float secondFreq, float thirdFreq)
    {
        
        List<float> tempList = new List<float>();
        tempList.Add(firstFreq);
        tempList.Add(secondFreq);
        tempList.Add(thirdFreq);
        tempList.Sort();
        float[] orderedFreq = tempList.ToArray();
        return orderedFreq;
    }

    public float transformCharacterToPitch(string noteName)
    {
        int pitch = 0;
        if (noteName == "C" || noteName == "B#" || noteName == "Dbb")
        {
            pitch = 0;
        }
        else if (noteName == "C#" || noteName == "Db")
        {
            pitch = 1;
        }
        else if (noteName == "D" || noteName == "Cx" || noteName == "Ebb")
        {
            pitch = 2;
        }
        else if (noteName == "D#" || noteName == "Eb")
        {
            pitch = 3;
        }
        else if (noteName == "E" || noteName == "Fb" || noteName == "Dx")
        {
            pitch = 4;
        }
        else if (noteName == "F" || noteName == "E#" || noteName == "Gbb")
        {
            pitch = 5;
        }
        else if (noteName == "F#" || noteName == "Gb")
        {
            pitch = 6;
        }
        else if (noteName == "G" || noteName == "Fx" || noteName == "Abb")
        {
            pitch = 7;
        }
        else if (noteName == "G#" || noteName == "Ab")
        {
            pitch = 8;
        }
        else if (noteName == "A" || noteName == "Gx" || noteName == "Bbb")
        {
            pitch = 9;
        }
        else if (noteName == "A#" || noteName == "Bb")
        {
            pitch = 10;
        }
        else if (noteName == "Cb" || noteName == "B" || noteName == "Ax")
        {
            pitch = 11;
        }
        else
        {
            print("something went wrong osc");
        }
        return frequencies[pitch];
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        increment = frequency * 2.0 * Mathf.PI / samplingFrequency;

        for(int i = 0; i<data.Length; i += channels)
        {
            phase += increment;
            data[i] = (float) (gain * Mathf.Sin((float)phase));

            if(channels == 2)
            {
                data[i + 1] = data[i];
            }
            if(phase >( Mathf.PI * 2))
            {
                phase = 0.0;
            }
        }
    }
}
