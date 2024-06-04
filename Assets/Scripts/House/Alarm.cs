using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Alarm : MonoBehaviour
{
    private const float MaxSoundValue = 1f;
    private const float MinSoundValue = 0f;

    [SerializeField] private float _volumeChangeRate = 0.1f;

    private AudioSource _audioSource;
    private bool _isWorking = false;  

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.loop = true;
    }

    private void Update()
    {
        if (_isWorking)
        {
            SmoothVolumeChange(MaxSoundValue);
        }
        else if (_audioSource.isPlaying)
        {
            SmoothVolumeChange(MinSoundValue);

            if(_audioSource.volume == 0)
            {
                _audioSource.Stop();
            }
        }
    }

    public void On()
    {
        if (_isWorking)
        {
            return;
        }

        if (_audioSource.isPlaying == false)
        {
            _audioSource.volume = MinSoundValue;
            _audioSource.Play();
        }        

        _isWorking = true;          
    }

    public void Off()
    {        
        _isWorking = false;
    }

    private void SmoothVolumeChange(float targetValue)
    {
        _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, targetValue, _volumeChangeRate * Time.deltaTime);        
    }
}