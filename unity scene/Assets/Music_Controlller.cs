
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music_Controlller : MonoBehaviour {

    

	// Use this for initialization
	void Start () {

        AkSoundEngine.RegisterGameObj(gameObject);
        AkSoundEngine.SetSwitch("Exploration_State", "On_Foot", gameObject);
        StartMusic();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void StartMusic()
    {
        AkSoundEngine.PostEvent("Play_Music", gameObject);
    }
    
    void StopMusic()
    {
        AkSoundEngine.PostEvent("Stop_Music", gameObject);
    }
}
