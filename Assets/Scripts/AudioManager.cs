using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound {

    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 0.7f;
    [Range(0f, 1f)]
    public float pitch = 1f;
    [Range(0f, 0.5f)]
    public float randomVolume = 0.1f;
    [Range(0f, 0.5f)]
    public float randomPitch = 0.1f;

    public bool loop;

    private AudioSource source;

    public void SetSource(AudioSource _source) {
        source = _source;
        source.clip = clip;
    }

    public void Play() {
        source.volume = volume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2));
        source.pitch = pitch * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2));
        source.loop = loop;
        source.Play();
    }
}

public class AudioManager : MonoBehaviour {

    public static AudioManager instance;

    [SerializeField]
    private Sound[] sounds;
    public List<AudioSource> audioSources;

    private void Awake() {
        if (instance != null) {
            Debug.LogError("More than one AudioManager in the scene.");
        } else {
            instance = this;
        }
        foreach (var s in sounds) {
            GameObject _go = new GameObject("Sound_" + s.name);
            _go.transform.SetParent(this.transform);
            s.SetSource(_go.AddComponent<AudioSource>());
            audioSources.Add(_go.GetComponent<AudioSource>());
        }
    }

    public void PlaySound(string _name) {

        foreach (var s in sounds) {
            if (s.name == _name) {
                s.Play();
                return;
            }
        }

        //No sound with _name
        Debug.LogWarning("AudioManager: Sound not found in array, " + _name);
    }

    public void PauseAllSounds()
    {
        foreach (var source in audioSources)
        {
            source.Stop();
        }
    }

}
