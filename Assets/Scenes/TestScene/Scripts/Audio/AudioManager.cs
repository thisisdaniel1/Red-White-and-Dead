using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;

    public static AudioManager Instance;

    void Awake(){
        Instance = this;
    }

    public void Play(AudioClip audioClip){
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
