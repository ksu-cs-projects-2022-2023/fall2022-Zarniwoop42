using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class AudioVolume : MonoBehaviour
{
  public AudioMixer m;
    //with help from: https://youtu.be/xNHSGMKtlv4
  public void AudioLevel(float v){
    m.SetFloat("AudioLevel", Mathf.Log10(v)*20);
  }
}
