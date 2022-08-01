using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] AudioClip coinPickUp;
    [SerializeField] int point = 100; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            FindObjectOfType<GameSession>().AddInScore(point);
            AudioSource.PlayClipAtPoint(coinPickUp, Camera.main.transform.position);
            Destroy(gameObject);
        }
    }
}
