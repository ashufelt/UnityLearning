using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] Sprite closedChestSprite;
    [SerializeField] Sprite openChestSprite;
    [SerializeField] Color interactableColor;
    [SerializeField] Color notInteractableColor;

    SpriteRenderer mySpriteRenderer;
    CircleCollider2D myInteractCollider;
    bool isOpen;

    void Start()
    {
        myInteractCollider = GetComponent<CircleCollider2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        isOpen = false;
    }

    /*
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") 
        {
            mySpriteRenderer.color = Color.red;
            Debug.Log("Player near chest (chest log)");
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            mySpriteRenderer.color = Color.white;
            Debug.Log("Player left chest (chest log)");
        }
    }
    */

    // Called by player object when player interacts with chest. Player script will
    // identify chest by overlapping with collider and pressing the interact button
    public void ChestInteract()
    {
        if(isOpen)
        {
            mySpriteRenderer.sprite = closedChestSprite;
            isOpen = false;
        }
        else
        {
            mySpriteRenderer.sprite = openChestSprite;
            isOpen = true;
        }
    }
}
