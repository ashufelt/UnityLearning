using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] int coinScoreValue = 10;
    [SerializeField] AudioClip coinPickupSFX;

    bool wasCollected = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !wasCollected)
        {
            wasCollected = true;

            AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position);
            FindObjectOfType<GameSession>().AddScore(coinScoreValue);
            Destroy(gameObject);
        }
    }
}
