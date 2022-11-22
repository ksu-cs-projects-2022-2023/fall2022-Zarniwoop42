using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class DeathScreen : MonoBehaviour
{

    public TextMeshProUGUI pointsText;
    public void Setup(int score){
        gameObject.SetActive(true);
        pointsText.text = score.ToString() + " points";
    }

    public void PlayGame(){
        Debug.Log("Game");
        SceneManager.LoadScene("Game");
    }

    public void Menu(){
        Debug.Log("Menu");
        SceneManager.LoadScene("Menu");
    }
}
