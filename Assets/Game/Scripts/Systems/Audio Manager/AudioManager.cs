using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioManager : Singleton<AudioManager>
{
    public bool mute = false;

	public float backgroundVolume = 1.0f;
	public float soundFxVolume = 1.0f;

    private bool savedSound;
    private float savedBackgroundVolume;
    private float savedSoundFxVolume;

	[System.Serializable]
	public class VolumeChangeEvent : UnityEvent<float> { }

	[HideInInspector] public VolumeChangeEvent onBackgroundVolumeChanged = new VolumeChangeEvent();
	[HideInInspector] public VolumeChangeEvent onSoundFxVolumeChanged = new VolumeChangeEvent();

	private void Awake()
	{
		// Handle Loading here
	}
    private void Update()
    {
        if(mute == true)
        {
            if(savedSound == false)
            {
                savedSound = true;

                savedBackgroundVolume = backgroundVolume;
                savedSoundFxVolume = soundFxVolume;

                setSoundFxVolume(0.0f);
                setBackgroundVolume(0.0f);

            }
        }
        else
        {
            if(savedSound == true)
            {
                savedSound = false;

                setSoundFxVolume(savedSoundFxVolume);
                setBackgroundVolume(savedBackgroundVolume);
            }
        }
    }

    public void setBackgroundVolume(float volume)
	{
		if (volume > 1.0f)
		{
			volume = 1.0f;
		}
		else if (volume < 0.0f)
		{
			volume = 0.0f;
		}
		backgroundVolume = volume;
		onBackgroundVolumeChanged.Invoke(backgroundVolume);
	}

	public void setSoundFxVolume(float volume)
	{
		if (volume > 1.0f)
		{
			volume = 1.0f;
		}
		else if (volume < 0.0f)
		{
			volume = 0.0f;
		}
		soundFxVolume = volume;
		onSoundFxVolumeChanged.Invoke(soundFxVolume);
	}
}
