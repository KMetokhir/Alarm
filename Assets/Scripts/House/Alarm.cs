using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Alarm : MonoBehaviour
{
    private const float MaxSoundValue = 1f;
    private const float MinSoundValue = 0f;

    [SerializeField] private float _volumeChangeRate = 0.1f;
    [SerializeField] private float _waitingInterval = 0.1f;

    private Coroutine _volumeChangeCoroutine;
    private bool _isCoroutineRuning = false;

    private AudioSource _audioSource;
    private bool _isWorking = false;
    private float _targetSoundValue;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = MinSoundValue;
        _audioSource.loop = true;
    }

    public void Activate()
    {
        if (_isWorking)
        {
            return;
        }

        _isWorking = true;
        _targetSoundValue = MaxSoundValue;

        if (TryStartNewCoroutine())
        {
            _audioSource.Play();
        }
    }

    public void Deactivate()
    {
        if (_isWorking == false)
        {
            return;
        }

        _isWorking = false;
        _targetSoundValue = MinSoundValue;

        TryStartNewCoroutine();
    }

    private bool TryStartNewCoroutine()
    {
        bool isSuccess = false;

        if (_volumeChangeCoroutine == null)
        {
            _volumeChangeCoroutine = StartCoroutine(VolumeChange());
            isSuccess = true;
        }
        else
        {
            if (_isCoroutineRuning == false)
            {
                _volumeChangeCoroutine = StartCoroutine(VolumeChange());
                isSuccess = true;
            }
        }

        return isSuccess;
    }

    private void AudioSourceStop()
    {
        if (_audioSource.volume == MinSoundValue)
        {
            _audioSource.Stop();
        }
    }

    private IEnumerator VolumeChange()
    {
        WaitForSeconds waitingTime = new WaitForSeconds(_waitingInterval);
        _isCoroutineRuning = true;

        while (_audioSource.volume != _targetSoundValue)
        {
            _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, _targetSoundValue, _volumeChangeRate);

            yield return waitingTime;
        }

        AudioSourceStop();
        _isCoroutineRuning = false;
    }
}