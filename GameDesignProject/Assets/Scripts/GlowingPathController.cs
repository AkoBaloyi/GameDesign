using UnityEngine;

public class GlowingPathController : MonoBehaviour
{
	public GameObject[] pathSegments; // enable to guide player
	public AudioSource audioSource;
	public AudioClip pathOnSfx;

	public void EnablePath()
	{
		foreach (var seg in pathSegments)
		{
			if (seg != null) seg.SetActive(true);
		}
		if (audioSource != null && pathOnSfx != null)
		{
			audioSource.PlayOneShot(pathOnSfx);
		}
	}
}


