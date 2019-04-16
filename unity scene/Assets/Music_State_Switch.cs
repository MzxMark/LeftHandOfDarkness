using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music_State_Switch : MonoBehaviour {

    public bool DebugInfo;

    public string EnterState = "default";
    public string ExitState = "default";
    public GameObject mPlayer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerEnter(Collider other)
    {        
        AkSoundEngine.SetSwitch("Exploration_State", EnterState, mPlayer);
        if (DebugInfo == true) { print("Music_Switch"); }
    }

    public void OnTriggerExit(Collider other)
    {
        AkSoundEngine.SetSwitch("Exploration_State", ExitState, mPlayer);
        if (DebugInfo == true) { print("Music_Switch"); }
    }
}
