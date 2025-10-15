using UnityEngine;

public class LightsController : MonoBehaviour
{
	[Header("Targets")]
	public Light[] lights;
	public ParticleSystem[] activationVfx;
	public AudioSource audioSource;
	public AudioClip powerOnSfx;

	public void ActivateLights()
	{
		foreach (var l in lights)
		{
			if (l != null) l.enabled = true;
		}
		foreach (var v in activationVfx)
		{
			if (v != null) v.Play();
		}
		if (audioSource != null && powerOnSfx != null)
		{
			audioSource.PlayOneShot(powerOnSfx);
		}
	}
}


