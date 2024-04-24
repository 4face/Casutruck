using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{

    [SerializeField] private AudioSource BG_Audio;
    [SerializeField] private float volume;

    private void Start()
    {
        BG_Audio.volume = PlayerPrefs.GetFloat("Volume"); 
    }
    private void Update()
    {
        if (UIController.played && !BG_Audio.isPlaying)
        {
            BG_Audio.Play();
        }
    }
    public void VolumeController(Slider slider)
    {
        volume = slider.value;
        if (slider.value != PlayerPrefs.GetFloat("Volume")) 
        {
            PlayerPrefs.SetFloat("Volume", slider.value);
        }
        BG_Audio.volume = volume;
    }

}
