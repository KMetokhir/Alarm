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

    private AudioSource _audioSource;
    private float _targetSoundValue;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = MinSoundValue;
        _audioSource.loop = true;
    }

    public void Activate()
    {
        _targetSoundValue = MaxSoundValue;

        if (TryStartNewCoroutine())
        {
            _audioSource.Play();
        }
    }

    public void Deactivate()
    {
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

        return isSuccess;
    }

    private void StopAudio()
    {
        if (_audioSource.volume == MinSoundValue)
        {
            _audioSource.Stop();
        }
    }

    private IEnumerator VolumeChange()
    {
        WaitForSeconds waitingTime = new WaitForSeconds(_waitingInterval);

        while (_audioSource.volume != _targetSoundValue)
        {
            _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, _targetSoundValue, _volumeChangeRate);

            yield return waitingTime;
        }

        StopAudio();
        _volumeChangeCoroutine = null;
    }
}