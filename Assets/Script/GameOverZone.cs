using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverZone : MonoBehaviour
{
    [SerializeField]
    private AudioManager audioManager;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerController>().GameOver();

            Debug.Log("Game Over");

            StartCoroutine(audioManager.PlayBGM(3));
        }
    }
}
