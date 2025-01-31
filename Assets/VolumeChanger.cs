using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeChanger : MonoBehaviour
{
	[SerializeField] AudioSource _audioSource;

	//Controls for audio fading in;
	[SerializeField] float _audioFadeInTime;
	private float _audioFadeInCurrentTime = 0;

    // Start is called before the first frame update
    void Start()
    {
		_audioSource.volume = 0; 
    }

    // Update is called once per frame
    void Update()
    {
		if (_audioFadeInCurrentTime < _audioFadeInTime)
		{
			_audioFadeInCurrentTime += Mathf.Clamp(
				_audioFadeInCurrentTime + Time.deltaTime,
				0,
				_audioFadeInTime);
			_audioSource.volume = _audioFadeInCurrentTime / _audioFadeInTime;	
		}
    }
}
