using System;
using Script.Utils;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource soundEffect;
    public AudioSource soundMusic;
   
    
    public bool isMute;
    
    [Range(0f, 1f)]
    public float volume = 1f;
    public Sounds[] sound;
    
    private void Start() 
    {
        if (soundMusic!=null)
        {
            PlayMusic(global::SoundTypes.Music);    
            
        }
    }

    public void Mute(bool status)
    {
        isMute = status;
    }
    public void SetVolume(float volume)
    {
        this.volume = volume;
        soundMusic.volume = this.volume;
        soundEffect.volume = this.volume;
    }

    private void PlayMusic(SoundTypes soundType)
    {   
        if(isMute) 
            return;
        AudioClip clip = GetSoundClip(soundType);
        if(clip != null) {
            soundMusic.clip = clip;
            soundMusic.Play();
        }   else {
                Debug.LogError("Clip not found for sound type: " + soundType );
        }
    }

    public void Play(SoundTypes soundType)
    {  
         if(isMute)
            return;
         
         AudioClip clip = GetSoundClip(soundType);
         if(clip != null) {
             soundEffect.PlayOneShot(clip);
         }   else {
             Debug.LogError("Clip not found for sound type: " + soundType );
         }
    }

    private AudioClip GetSoundClip(SoundTypes soundType)
    {
        Sounds item = Array.Find(sound, item => item.soundType == soundType);
        if(item != null) 
           return item.soundClip;
        return null;
        
    }
}
[Serializable]
public class Sounds
{
    public SoundTypes soundType;
    public AudioClip soundClip;
}
public enum SoundTypes
{   
   
    Music,
    CellClear,
    ButtonClick,
    CellRestore,
    CellRefill,
    CellPlace,
    GameOver,
    
}