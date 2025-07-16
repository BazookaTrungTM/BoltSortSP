using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Audio;



[Serializable]
public class AudioPlaying
{
    public AudioSource Source;
    public Coroutine Coroutine;
}

public class AudioManager : MonoBehaviour
{
    const string MIXER_BGM = "BGMVolume";
    const string MIXER_SFX = "SFXVolume";

    [SerializeField] AudioMixer audioMixer;
    public AudioSource bgmAudioSource;

    [SerializeField] AudioSource sfxAudioSourceShell;

  
    private List<AudioSource> sfxShellPool = new List<AudioSource>();
    //private BGMType currentBGM;

    private Dictionary<SoundType, bool> playingSfx = new Dictionary<SoundType, bool>();
    private Dictionary<SoundType, bool> cooldownSfx = new Dictionary<SoundType, bool>();

    //Serialized for debugging
    //[SerializeField] SerializedDictionary<AudioSource, Coroutine> waitingToClaimSFXSources;
    private List<AudioPlaying> waitingToClaimSFXSources = new List<AudioPlaying>();
    //public void PlayBGM(BGMType type)
    //{
    //    if (currentBGM == type)
    //        return;

    //    bgmAudioSource.clip = bgmClipMap[type];
    //    bgmAudioSource.Play();

    //    currentBGM = type;
    //}
    
    
    
    [SerializeField] AudioClip clickUI;
   // [SerializeField] AudioClip Coin_Collect;
    [SerializeField] AudioClip Nut_Go_To_Screw;
    [SerializeField] AudioClip Nut_Out_Of_Screw;
    [SerializeField] private AudioClip Bomb_Explore;
    [SerializeField] private AudioClip Bomb_Defuse;
  //  [SerializeField] private AudioClip Booster_Fill;
  //  [SerializeField] private AudioClip Booster_Add_Holder;
  //  [SerializeField] private AudioClip Update_Progress;
    [SerializeField] private AudioClip Show_Hidden;
   // [SerializeField] private AudioClip Done_Progress;
    [SerializeField] private AudioClip Firework;
    [SerializeField] private AudioClip Win;
    [SerializeField] private AudioClip Lose;
    [SerializeField] private AudioClip rope;

    [SerializeField] private AudioClip holdeClose;
   // [SerializeField] private AudioClip Feature_Anounce;
    //[SerializeField] private AudioClip BGM;
    [SerializeField] private AudioClip Complete_Holder_1;
    [SerializeField] private AudioClip Complete_Holder_2;
    [SerializeField] private AudioClip Complete_Holder_3;
    public static AudioManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void CheckBGM()
    {
        if (!bgmAudioSource.isPlaying)
            bgmAudioSource.Play();
        else
            bgmAudioSource.Stop();
    }

    public void StopBGM()
    {
        if (bgmAudioSource.isPlaying)
            bgmAudioSource.Stop();
    }

    public AudioSource PlaySFX(SoundType type, bool isLoop = false, Transform target = null, bool setParent = false, bool canPlayWhenThereIsDuplicatedSFX = true,
        float delayBeforeCanPlayAgain = 0, bool isBGM = false)
    {
        if (!canPlayWhenThereIsDuplicatedSFX)
        {
            if (!playingSfx.ContainsKey(type))
            {
                playingSfx.Add(type, true);
            }
            else
            {
                if (playingSfx[type])
                {
                    return null;
                }
                else
                {
                    playingSfx[type] = true;
                }
            }
        }

        if (delayBeforeCanPlayAgain > 0)
        {
            if (!cooldownSfx.ContainsKey(type))
            {
                cooldownSfx.Add(type, true);
                DOVirtual.DelayedCall(delayBeforeCanPlayAgain, () =>
                {
                    cooldownSfx[type] = false;
                });
            }
            else
            {
                if (cooldownSfx[type])
                {
                    return null;
                }
                else
                {
                    cooldownSfx[type] = true;
                    DOVirtual.DelayedCall(delayBeforeCanPlayAgain, () =>
                    {
                        cooldownSfx[type] = false;
                    });
                }
            }
        }
        AudioSource sfxAudioSource;
        if (sfxShellPool.Count > 0)
        {
            sfxAudioSource = sfxShellPool[0];

            sfxShellPool.Remove(sfxAudioSource);
        }
        else
        {
            sfxAudioSource = Instantiate(sfxAudioSourceShell, transform);
        }

        if (target != null)
        {
            sfxAudioSource.transform.position = target.position;
        }
        if (setParent)
        {
            sfxAudioSource.transform.parent = target;
        }

        sfxAudioSource.loop = isLoop;
        sfxAudioSource.clip = GetAudioClip(type);
        //sfxAudioSource.spatialBlend = (target == null) ? 0 : 1;
        sfxAudioSource.gameObject.SetActive(true);
        sfxAudioSource.Play();

        if (!isLoop)
        {
            Coroutine cor = StartCoroutine(IECollectSFXShell(sfxAudioSource, GetAudioClip(type).length, canPlayWhenThereIsDuplicatedSFX, type));
            var audioPlaying = new AudioPlaying();
            audioPlaying.Source = sfxAudioSource;
            audioPlaying.Coroutine = cor;
            waitingToClaimSFXSources.Add(audioPlaying);
        }

        if (isBGM)
        {
            bgmAudioSource.loop = true;
            if (bgmAudioSource != null)
                Destroy(bgmAudioSource.gameObject);
          //  bgmAudioSource = sfxAudioSource;
        }

        return sfxAudioSource;
    }

    public AudioClip GetAudioClip(SoundType type)
    {
        // foreach (var item in sfxClipMap)
        // {
        //     if (item.Type == type)
        //     {
        //         return item.Clip;
        //     }
        // }

        switch (type )
        {
            case SoundType.Click_UI:
                return clickUI;
            case SoundType.Coin_Collect:
                return null;
            case SoundType.Nut_Go_To_Screw:
                return Nut_Go_To_Screw;
            case SoundType.Nut_Out_Of_Screw:
                return Nut_Out_Of_Screw;
            case SoundType.Bomb_Explore:
                return Bomb_Explore;
            case SoundType.Bomb_Defuse:
                return Bomb_Defuse;
            case SoundType.Booster_Fill:
            //    return Booster_Fill;
            case SoundType.Booster_Add_Holder:
                return null;
            case SoundType.Show_Hidden:
                return Show_Hidden;
            case SoundType.Done_Progress:
             //   return Done_Progress;
            case SoundType.Firework:
                return Firework;
            case SoundType.Win:
                return Win;
            case SoundType.Lose:
                return Lose;
            case SoundType.Feature_Anounce:
                return null;
            case SoundType.BGM:
                return null;
            case SoundType.Update_Progress:
                return null;
            case SoundType.Rope_Reject:
                return rope;
            case SoundType.Holder_Close:
                return Complete_Holder_1;
            case SoundType.Complete_Holder_1:
                return Complete_Holder_1;
            case SoundType.Complete_Holder_2:
                return Complete_Holder_1;
            default: return null;
         
            
        }
        return null;
    }

    public bool ValidateAudioPlaying(AudioSource audioSource)
    {
        foreach (var item in waitingToClaimSFXSources)
        {
            if (item.Source == audioSource)
            {
                return true;
            }
        }
        return false;
    }

    public AudioPlaying GetAudioPlay(AudioSource source)
    {
        foreach (var item in waitingToClaimSFXSources)
        {
            if (item.Source == source)
            {
                return item;
            }
        }

        return null;
    }

    public void StopSFX(AudioSource audioSource)
    {
        if (audioSource == null)
            return;

        if (!ValidateAudioPlaying(audioSource))
        {
            return;
        }

        audioSource.clip = null;
        audioSource.gameObject.SetActive(false);
        audioSource.transform.parent = transform;
        sfxShellPool.Add(audioSource);

        waitingToClaimSFXSources.Remove(GetAudioPlay(audioSource));
    }

    private IEnumerator IECollectSFXShell(AudioSource audioSource, float delay, bool canDuplicatedPlaying = true, SoundType nonDuplicatedSFXType = 0)
    {
        yield return new WaitForSeconds(delay);

        StopSFX(audioSource);

        if (!canDuplicatedPlaying)
        {
            if (playingSfx.ContainsKey(nonDuplicatedSFXType))
            {
                playingSfx[nonDuplicatedSFXType] = false;
            }
        }
    }

    #region Better way to control volume
    private void SetMusicVolume(float value)
    {
        //value range from 0.0001 to 1
        audioMixer.SetFloat(MIXER_BGM, Mathf.Log10(value) * 20f);
    }

    private void SetSFXVolume(float value)
    {
        //value range from 0.0001 to 1
        audioMixer.SetFloat(MIXER_SFX, Mathf.Log10(value) * 20f);
    }

    public void MuteMusicVolume()
    {
        audioMixer.SetFloat(MIXER_BGM, -80f);
    }

    public void MuteSFXVolume()
    {
        audioMixer.SetFloat(MIXER_SFX, -80f);
    }

    public void UnmuteMusicVolume()
    {
        audioMixer.SetFloat(MIXER_BGM, 0f);
    }

    public void UnmuteSFXVolume()
    {
        audioMixer.SetFloat(MIXER_SFX, 0f);
    }
    #endregion
}