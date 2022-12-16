using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;



public class DeathScreen : MonoBehaviour
{

    public TextMeshProUGUI pointsText;
    public void Setup(int score, int floor, float gameTime){
        if(GameObject.FindGameObjectsWithTag("ES").Length == 0){
            var eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            eventSystem.tag = "ES";
        }
        gameObject.SetActive(true);
        pointsText.text = score.ToString() + " points\nfloor " + floor.ToString() + "\n" + gameTime.ToString() + " seconds";
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
