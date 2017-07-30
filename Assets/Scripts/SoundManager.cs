using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour {

	public static SoundManager instance { get; protected set; }

	public AudioClip[] powerSwitchSounds;
	public AudioClip tutorialTipSound;
	public AudioClip[] buildSounds;

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
		instance.audio.pitch = Random.Range(0.8f, 1.2f);
		instance.audio.panStereo = Random.Range(-0.2f, 0.2f);
		instance.audio.PlayOneShot(instance.powerSwitchSounds[Random.Range(0, instance.powerSwitchSounds.Length)]);
	}

	public static void TutorialTip()
	{
		instance.audio.pitch = 1;
		instance.audio.panStereo = 0;
		instance.audio.PlayOneShot(instance.tutorialTipSound);
	}

	public static void Build()
	{
		instance.audio.pitch = Random.Range(0.8f, 1.2f);
		instance.audio.panStereo = Random.Range(-0.2f, 0.2f);
		instance.audio.PlayOneShot(instance.buildSounds[Random.Range(0, instance.buildSounds.Length)]);
	}
}
