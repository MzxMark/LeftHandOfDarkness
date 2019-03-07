using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    #region Variables
    public static AudioManager instance;

    [SerializeField]
    AudioSource source;
    [SerializeField]
    AudioClip[] squeaks;
    #endregion

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void Squeak()
    {
        source.PlayOneShot(squeaks[Random.Range(0, 2)]);
    }
}
