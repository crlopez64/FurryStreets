using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for Item overworld placement and collection.
/// </summary>
public class ItemOverworld : Interactable
{
    private Rigidbody2D rb2D;
    private Item item;
    private Vector2 personalFloor;
    private Vector2 velocity;
    private float timer;
    private float tickTimer;

    //I can make a comment here
    //It be nice
    public bool timerOn;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        timer = 30f;
        tickTimer = 0.5f;
    }
    private void Start()
    {
        priority = 1;
        mustActionButton = true;
        SetItem(2, true, transform.position);
    }
    private void Update()
    {
        if (timerOn)
        {
            if (timer > 0f)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                Destroy(this.gameObject);
            }
            if (timer < 7f)
            {
                if (tickTimer > 0f)
                {
                    tickTimer -= Time.deltaTime;
                }
                else
                {
                    GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
                }
            }
        }
    }
    private void FixedUpdate()
    {
        if (rb2D != null)
        {
            //physics
            if (rb2D.velocity.y > 0)
            {
                velocity = new Vector2(rb2D.velocity.x, rb2D.velocity.y);
                rb2D.velocity = velocity;
            }
            else
            {
                if (transform.position.y <= personalFloor.y)
                {
                    rb2D.velocity = Vector2.zero;
                    rb2D.gravityScale = 0;
                }
            }
            
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.gameObject.layer == 6) || (collision.gameObject.layer == 8))
        {
            if (Grounded())
            {
                if (collision.gameObject.GetComponentInParent<PlayerAction>().NewInteractableHasPriority(this))
                {
                    collision.gameObject.GetComponentInParent<PlayerAction>().PrepareInteractable(this);
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.gameObject.layer == 6) || (collision.gameObject.layer == 8))
        {
            collision.gameObject.GetComponentInParent<PlayerAction>().PrepareInteractable(null);
        }
    }

    /// <summary>
    /// Set the item for the Overworld collectable, and set the original Enemy position to have its reference to its ground position. Can only set once.
    /// </summary>
    /// <param name="item"></param>
    public void SetItem(int itemID, bool makeJump, Vector2 enemyPosition)
    {
        if (itemID > 0 && (item == null))
        {
            item = GameManager.Instance.GetComponent<ItemDatabase>().GetItem(itemID);
            if (item == null)
            {
                Debug.LogError("ERROR: Did not get specified item");
                return;
            }
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(item.GetSpritePath());
            if (rb2D != null)
            {
                if (makeJump)
                {
                    //Make it jump once
                    rb2D.gravityScale = 6;
                    float horizontalSpread = Random.Range(-10f, 10f);
                    float verticalSpread = Random.Range(-2f, 2f);
                    personalFloor = enemyPosition + new Vector2(0, verticalSpread);
                    velocity = new Vector2(horizontalSpread, 30);
                    rb2D.velocity = velocity;
                }
                else
                {
                    rb2D.gravityScale = 0;
                }
            }
        }
    }
    public override void Interact()
    {
        base.Interact();
        if (GameManager.Instance.GetComponent<Inventory>().AddItemToInventory(item))
        {
            Debug.Log("Added item to inventory.");
            Destroy(this.gameObject);
        }
        else
        {
            Debug.LogWarning("Inventory full");
        }
    }


    private bool Grounded()
    {
        return rb2D.gravityScale <= 0;
    }
}
