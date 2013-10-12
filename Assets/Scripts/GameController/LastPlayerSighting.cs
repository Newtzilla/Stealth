using UnityEngine;
using System.Collections;

public class LastPlayerSighting : MonoBehaviour {

	public Vector3 position = new Vector3(1000f, 1000f, 1000f);         // The last global sighting of the player.
    public Vector3 resetPosition = new Vector3(1000f, 1000f, 1000f);    // The default position if the player is not in sight.
    public float lightHighIntensity = 0.25f;                            // The directional light's intensity when the alarms are off.
    public float lightLowIntensity = 0f;                                // The directional light's intensity when the alarms are on.
    public float fadeSpeed = 7f;                                        // How fast the light fades between low and high intensity.
    public float musicFadeSpeed = 1f;                                   
    
    
    private AlarmLight alarm;                                           // Reference to the AlarmLight script.
    private Light mainLight;                                            // Reference to the main light.
    private AudioSource panicAudio;                                     // Reference to the AudioSource of the panic msuic.
    private AudioSource[] sirens;                                       // Reference to the AudioSources of the megaphones.
    
	void Awake()
	{
		alarm = GameObject.FindGameObjectWithTag(Tags.alarm).GetComponent<AlarmLight>();
		mainLight = GameObject.FindGameObjectWithTag(Tags.mainLight).light;
		panicAudio = transform.Find("secondaryMusic").audio;
		GameObject[] sirenGameObjects = GameObject.FindGameObjectsWithTag(Tags.siren);
		sirens = new AudioSource[sirenGameObjects.Length];
		
		for(int i = 0; i < sirens.Length; i++)
        {
            sirens[i] = sirenGameObjects[i].audio;
        }
	}
	
	void Update()
	{
		SwitchAlarms();
		MusicFading();
	}
	
	void SwitchAlarms()
	{
		alarm.alarmOn = (position != resetPosition);
		
		float newIntensity;
		if(position != resetPosition)
		{
			newIntensity = lightLowIntensity;
		}
		else
		{
			newIntensity = lightHighIntensity;
		}
		
		mainLight.intensity = Mathf.Lerp(mainLight.intensity, newIntensity, fadeSpeed * Time.deltaTime);
		
		// For all of the sirens...
        for(int i = 0; i < sirens.Length; i++)
        {
            // ... if alarm is triggered and the audio isn't playing, then play the audio.
            if(position != resetPosition && !sirens[i].isPlaying)
                sirens[i].Play();
            // Otherwise if the alarm isn't triggered, stop the audio.
            else if(position == resetPosition)
                sirens[i].Stop();
        }
	}
	
	void MusicFading()
	{
		if(position != resetPosition)
		{
			audio.volume = Mathf.Lerp(audio.volume, 0f, musicFadeSpeed * Time.deltaTime);
			panicAudio.volume = Mathf.Lerp(panicAudio.volume, 0.8f, musicFadeSpeed * Time.deltaTime);
		}
		else
		{
			panicAudio.volume = Mathf.Lerp(panicAudio.volume, 0f, musicFadeSpeed * Time.deltaTime);
			audio.volume = Mathf.Lerp(audio.volume, 0.8f, musicFadeSpeed * Time.deltaTime);
		}
	}
	
	
}
