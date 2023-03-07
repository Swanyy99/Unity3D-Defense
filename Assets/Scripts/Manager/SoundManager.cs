using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : SingleTon<SoundManager>
{
    public AudioMixer Audio;

    public void MASTERSliderValueSet(float val)
    {
        Audio.SetFloat("MASTER", val);
    }

    public void BGMSliderValueSet(float val)
    {
        Audio.SetFloat("BGM", val);
    }

    public void SFXSliderValueSet(float val)
    {
        Audio.SetFloat("SFX", val);
    }
}
