using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [Header("--------- Audio Sources ---------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [Header("--------- Audio Clip ---------")]
    public AudioClip background;
    public AudioClip EarnCoin;
    public AudioClip jumpSound;
    public AudioClip attackSound;




    private void Start()
    {
       // musicSource.clip = background;
      //  musicSource.Play();
    }

    //Enable access from other scripts
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}
