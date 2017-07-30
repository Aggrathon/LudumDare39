using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour {

	public static SoundManager instance { get; protected set; }

	public AudioClip[] powerSwitchSounds;

#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
	AudioSource audio;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword


	private void Awake()
	{
		instance = this;
		audio = GetComponent<AudioSource>();
	}

	public static void PowerSwitch()
	{
		instance.audio.pitch = Random.Range(0.9f, 1.1f);
		instance.audio.panStereo = Random.Range(-0.2f, 0.2f);
		instance.audio.PlayOneShot(instance.powerSwitchSounds[Random.Range(0, instance.powerSwitchSounds.Length)]);
	}
}
