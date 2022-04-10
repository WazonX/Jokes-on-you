using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudienceController : MonoBehaviour
{
    [SerializeField] AudioClip _wowSFX;
    [SerializeField] AudioClip _booSFX;
    [SerializeField] AudioClip _indiffrentSFX;


    public enum AudienceReaction
    {
        Indiffrent,
        Wow,
        Boo
    }

    AudienceReaction EntertainmentValueToReaction(float value)
    {
        if (value > 0)
        {
            return AudienceReaction.Wow;
        }
        else if (value < 0)
        {
            return AudienceReaction.Boo;
        }
        else
        {
            return AudienceReaction.Indiffrent;
        }
    }

    public void PlaySound(float entertainmentValue)
    {
        PlaySound(EntertainmentValueToReaction(entertainmentValue));
    }

    public void PlaySound(AudienceReaction reaction)
    {
        if (GameMusic.instance != null)
        {
            AudioClip clip = null;
            switch(reaction)
            {
                case AudienceReaction.Indiffrent:
                default:
                    clip = _indiffrentSFX;
                    break;
                case AudienceReaction.Wow:
                    clip = _wowSFX;
                    break;
                case AudienceReaction.Boo:
                    clip = _booSFX;
                    break;
            }
            GameMusic.instance.PlaySFX(clip);
        }
    }
}
