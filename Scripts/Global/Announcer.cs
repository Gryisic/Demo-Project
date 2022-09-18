using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Announcer : MonoBehaviour
{
    public enum AnnouncementType 
    {
        Battle,
        KO
    }

    [SerializeField] private List<AudioClip> _clips;
    [SerializeField] private AudioClip _battle;
    [SerializeField] private AudioClip _ko;

    private AudioSource _audio;

    private void Awake() => _audio = GetComponent<AudioSource>();

    public void Announce(AnnouncementType type) 
    {
        switch (type)
        {
            case AnnouncementType.Battle:
                _audio.clip = _battle;
                break;

            case AnnouncementType.KO:
                _audio.clip = _ko;
                break;
        }

        _audio.Play();
    }

    public void AnnounceRound(int index) 
    {
        _audio.clip = _clips[index - 1];
        _audio.Play();
    }
}
