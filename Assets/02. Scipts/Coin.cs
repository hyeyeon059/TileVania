using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] AudioClip coinPickUp;
    [SerializeField] int point = 100;

    bool wasCollected = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !wasCollected)
        {
            wasCollected = true;

            FindObjectOfType<GameSession>().AddInScore(point);
            AudioSource.PlayClipAtPoint(coinPickUp, Camera.main.transform.position);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
