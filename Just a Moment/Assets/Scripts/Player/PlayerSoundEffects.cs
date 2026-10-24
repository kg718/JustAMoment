using UnityEngine;

public class PlayerSoundEffects : MonoBehaviour
{
    [SerializeField] private AudioClip stepSound;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void PlayStepSound()
    {
        SoundEffectManager.Instance.PlaySoundEffect(stepSound, 0.8f, 1.2f);
    }
}
