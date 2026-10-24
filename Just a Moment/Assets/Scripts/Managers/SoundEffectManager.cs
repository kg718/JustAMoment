using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager Instance;
    [SerializeField] private GameObject soundPrefab;

    void Start()
    {
        Instance = this;
    }

    void Update()
    {
        
    }

    public void PlaySoundEffect(AudioClip _clip)
    {
        GameObject _newSoundObject = Instantiate(soundPrefab);
        AudioSource _source = _newSoundObject.GetComponent<AudioSource>();
        _source.clip = _clip;
        _source.Play();
        _newSoundObject.GetComponent<DestroyOnTimer>().destroyTime = _clip.length;
    }

    public void PlaySoundEffect(AudioClip _clip, float _minPitch, float _maxPitch)
    {
        GameObject _newSoundObject = Instantiate(soundPrefab);
        AudioSource _source = _newSoundObject.GetComponent<AudioSource>();
        _source.clip = _clip;
        _source.pitch = Random.Range(_minPitch, _maxPitch);
        _source.Play();
        _newSoundObject.GetComponent<DestroyOnTimer>().destroyTime = _clip.length + 1f;
    }
}
