using UnityEngine;
using System.Collections;

public class SoundDestroyOnEnd : MonoBehaviour
{

    public AudioSource audio;

	// Use this for initialization
	void Start ()
	{
	    audio.pitch += Random.Range(-1, 1) * 0.05f;
        AudioSource.PlayClipAtPoint(audio.clip, transform.position);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
