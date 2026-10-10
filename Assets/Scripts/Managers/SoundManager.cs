using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    #region Singleton_region

    public static SoundManager Inst;
    private bool muted;

    private void Awake()
    {
        if (Inst==null)
        {
            Inst = this;
            setSounds();
            muted=false;
        }
        else
        {
            Destroy(gameObject);
        }

    }
    #endregion
    [System.Serializable]
    public class Sound
    {
        public String tag;
        public AudioClip clip;
        public AudioSource source;
        public bool loop;
        [Range(0f,1f)]public float volume;
        [Range(.1f,3f)]public float pitch;
    }
    public Sound[] sounds;
    [SerializeField] private Image muteButtonImage;
    public Sprite[] muteSprites;

    public void MuteOrUnmuteSounds()
    {
        muted = !muted;
        if (muted)
        {
            muteButtonImage.sprite = muteSprites[1];
        }
        else
        { 
            muteButtonImage.sprite = muteSprites[0];
        }

        foreach (Sound s in sounds)
        {
            s.source.mute = muted; 
        }
    }

    private void setSounds()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play(String tag,bool fade,float fadeSpeed)
    {
        Sound s =Array.Find(sounds, sound => sound.tag == tag);
        if (s!=null && !fade)
        {
            s.source.Play();
        }
        else if (s!=null && fade)
        {
            StartCoroutine(PlayWithFade(s,.5f , .02f));
        }
        else
        {
            Debug.Log("No sound");
        }
    }

    private IEnumerator PlayWithFade(Sound s,float Speed, float volumeStep)
    {
        float currentVolume = 0;
        //s.volume = 0;
        float originalVolume = s.source.volume;
        s.source.volume = currentVolume;
        s.source.Play();
        while (currentVolume<originalVolume)
        {
            currentVolume += volumeStep;
            s.source.volume = currentVolume;
            yield return new WaitForSeconds(Speed);
            if (currentVolume>=originalVolume)
            {
                s.source.volume = originalVolume;
                yield break;
            }
        }

        yield return null;
    }

}
