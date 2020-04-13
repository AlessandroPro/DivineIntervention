using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioAssistant : MonoBehaviour
{
	public enum AudioType
	{
		SoundFx,
		Background
	};
	public AudioType audioType = AudioType.SoundFx;

	private AudioSource audioSource;

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		switch(audioType)
		{
			case AudioType.Background:
				AudioManager.Instance.onBackgroundVolumeChanged.AddListener(onVolumeChanged);
				audioSource.volume = AudioManager.Instance.backgroundVolume;
				break;

			case AudioType.SoundFx:
				AudioManager.Instance.onSoundFxVolumeChanged.AddListener(onVolumeChanged);
				audioSource.volume = AudioManager.Instance.soundFxVolume;
				break;
		}
	}

	private void OnDestroy()
	{
		switch (audioType)
		{
			case AudioType.Background:
				AudioManager.Instance.onBackgroundVolumeChanged.RemoveListener(onVolumeChanged);
				break;

			case AudioType.SoundFx:
				AudioManager.Instance.onSoundFxVolumeChanged.RemoveListener(onVolumeChanged);
				break;
		}
	}

	void onVolumeChanged(float volume)
	{
		audioSource.volume = volume;
	}
}
