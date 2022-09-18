using System.Collections.Generic;
using UnityEngine;

public enum AudioType 
{
    AttackVoiceLine,
    TakeHitVoiceline,
    DeathVoiceLine,
    Effect
}

[System.Serializable]
public class UnitAudio 
{
    [SerializeField] private AudioSource _voiceEffects;
    [SerializeField] private AudioSource _audioEffects;
    [SerializeField] private List<AudioClip> _attackVoiceClips;
    [SerializeField] private List<AudioClip> _takeHitVoiceClips;
    [SerializeField] private AudioClip _deathVoiceClip;
    [SerializeField] private List<AudioClip> _effectsClips;

    public void PlayRandomAudioClip(AudioType type) 
    {
        AudioSource source;
        List<AudioClip> _clips;

        switch (type) 
        {
            case AudioType.AttackVoiceLine:
                source = _voiceEffects;
                _clips = _attackVoiceClips;
                break;

            case AudioType.TakeHitVoiceline:
                source = _voiceEffects;
                _clips = _takeHitVoiceClips;
                break;

            case AudioType.Effect:
                source = _audioEffects;
                _clips = _effectsClips;
                break;

            default:
                throw new System.ArgumentException("Invalid AudioClip type");
        }

        source.clip = GetRandomClip(_clips);
        source.Play();
    }

    public void PlayDeathAudioClip() 
    {
        _voiceEffects.Stop();
        _voiceEffects.clip = _deathVoiceClip;
        _voiceEffects.Play();
    }

    private AudioClip GetRandomClip(List<AudioClip> clips) => clips[Random.Range(0, clips.Count)];
}
