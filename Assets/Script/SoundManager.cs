using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public AudioSource musicSource;
    public AudioSource sfxSource;

    public AudioClip[] bgmClips; // 0:메인, 1:게임, 2:게임오버
    public AudioClip[] sfxClips; //  1:성공, 2:실패

    private void Awake()
    {
        Instance = this;
    }


    public void PlayBGM(int index)
    {
        if (index < bgmClips.Length)
        {
            musicSource.clip = bgmClips[index];
            musicSource.Play();
        }
    }

    public void PlaySFX(int index)
    {
        if (index < sfxClips.Length)
        {
            sfxSource.PlayOneShot(sfxClips[index]);
        }
    }
}
