using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SoundManager : MonoBehaviour
{
    public bool soundOn = true;

    public AudioSource bgSource;
    public AudioSource carEngine;
    public AudioSource sfx;

    public List<AudioClip> bgMusicList = new List<AudioClip>();
    public AudioClip jump;
    public AudioClip revving;
    public AudioClip engine;
    public AudioClip screech;


    private bool screechOn = false;
    private int currentBg = 0;

    void Start()
    {
        soundOn = PlayerInfo.soundOn;
        if (soundOn)
        {
            currentBg = Random.Range(0, bgMusicList.Count);
            playBgMusic();
        }
    }

    public void toggleAllSound(bool state)
    {
        PlayerInfo.soundOn = state;

        if (PlayerInfo.soundOn)
        {
            if (bgSource.clip == null)
            {
                currentBg = Random.Range(0, bgMusicList.Count);
                playBgMusic();
            }
                        
            bgSource.UnPause();
            carEngine.UnPause();
            
        }
        else
        {
            bgSource.Pause();
            carEngine.Pause();
        }

    }


    public void revEngine()
    {
        if(!soundOn){return;}
        sfx.clip = revving;
        sfx.Play();
    }
    public void startEngine()
    {
        if(!soundOn){return;}

        carEngine.clip = engine;
        carEngine.Play();
        
    }

    public void stopEngine()
    {
        if(!soundOn){return;}

        carEngine.Stop();
        revEngine();
    }

    public void playjump()
    {
        if(!soundOn){return;}

        sfx.Stop();
        sfx.clip = jump;
        sfx.Play();

        screechOn = true;
        var length = jump.length;
        Invoke("turnOffScreech", length);

    }

    public void playScreech()
    {
        if(!soundOn){return;}

        if (screechOn)
        {
            return;
        }

        sfx.Stop();
        sfx.clip = screech;
        sfx.Play();

        float length = screech.length;
        screechOn = true;
        Invoke("turnOffScreech", length *2);

    }

    public void stopScreech()
    {
        if(!soundOn){return;}

        if (sfx.clip.Equals(screech))
        {
            sfx.Stop();
            screechOn = false;

        }
    }

    private void turnOffScreech()
    {
        if(!soundOn){return;}

        screechOn = false;
    }


    public void playBgMusic()
    {
        if(!soundOn){return;}
        
        currentBg = (currentBg + 1) % bgMusicList.Count;
        float clipLength = bgMusicList[currentBg].length;
        bgSource.clip = bgMusicList[currentBg];
        bgSource.Play();
        Invoke("playBgMusic", clipLength);
    }
}
