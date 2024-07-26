using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioFade : MonoBehaviour
{
    [SerializeField] float _fadeTime = 0.5f;
    [SerializeField] AnimationCurve _fadeCurve;

    float _currentTimeOnCurve;
    AudioSource _audioSource;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = 0f;
        _currentTimeOnCurve = 0f;

        #if UNITY_WEBGL
        enabled = false;
        #endif
    }

    void Update()
    {
        bool isDone = false;

        _currentTimeOnCurve += Time.deltaTime;
        if (_currentTimeOnCurve > _fadeTime)
        {
            _currentTimeOnCurve = _fadeTime;
            isDone = true;
        }
        _currentTimeOnCurve = Mathf.Clamp(_currentTimeOnCurve, 0f, _fadeTime);

        float fadeCurveValue = _fadeCurve.Evaluate(_currentTimeOnCurve / _fadeTime);
        _audioSource.volume = Mathf.Clamp01(fadeCurveValue);

        if (isDone)
        {
            enabled = false;
        }
    }
}
