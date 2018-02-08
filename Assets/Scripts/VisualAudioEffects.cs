using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class VisualAudioEffects : MonoBehaviour {

    public AudioSource music;
    public float[] spectrum;
    public Transform musicalParticlesEmissionSource;
    private ParticleSystem musicalParticlesSystem;
    public int spectrumFocus = 178;
    public float timeAmplification = 1000;
    public float lastZValue;

    void Start()
    {
        spectrum = new float[256];
        music = GetComponent<AudioSource>();
        
    }

    void Update()
    {
        music.GetSpectrumData(spectrum, 1, FFTWindow.Blackman);

        var emis = musicalParticlesEmissionSource.GetComponent<ParticleSystem>().emission;
        emis.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0, (short)(spectrum[0] * 50)) });

        lastZValue = spectrum[spectrumFocus] * timeAmplification;
        musicalParticlesEmissionSource.transform.localPosition = new Vector3(lastZValue, Mathf.Clamp(lastZValue, 3f, 60f), 0);

    }

    void LateUpdate()
    {

    }
}
