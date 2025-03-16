using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource buttonSource;
    public AudioSource jingleSource;
    public AudioSource singingSource;

    public AudioClip goodJingle;
    public AudioClip badJingle;
    public AudioClip menuSinging;
    public AudioClip stageSinging;
    public AudioClip celebration;
    public AudioClip cursing;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void PlayButtonSound()
    {
        buttonSource.Play();
    }

    public void PlayJingle(bool win)
    {
        if (jingleSource.clip != null) { jingleSource.clip = null; }

        if (win)
        {
            jingleSource.clip = goodJingle;
        }
        else
        {
            jingleSource.clip = badJingle;
        }

        jingleSource.volume = 0.7f;
        jingleSource.Play();
    }

    public void PlaySinging(bool isMenu)
    {
        if (singingSource.clip != null) { singingSource.clip = null; }

        if (isMenu)
        {
            singingSource.clip = menuSinging;
        }
        else
        {
            singingSource.clip = stageSinging;
        }

        singingSource.volume = 0.4f;
        singingSource.loop = true;
        singingSource.Play();
    }

    public void StopSinging()
    {
        singingSource.Stop();
    }

    public void PlayReaction(bool win)
    {
        if (singingSource.clip != null) { singingSource.clip = null; }

        if (win)
        {
            singingSource.clip = cursing;
        }
        else
        {
            singingSource.clip = celebration;
        }

        singingSource.volume = 1f;
        singingSource.loop = false;
        singingSource.Play();
    }
}
