using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    private Rigidbody2D char_rb;

    [SerializeField]
    private float speed_scale;
    [SerializeField]
    private float knockBackAmount = 2;
    [SerializeField]
    private int health = 100;

    [SerializeField]
    private float respawn_max_time = 3f;
    private float current_respawn_time = 3f;
    private bool respawning = false; 

    private int current_dmg_amount = 0; 

    //private Bullet bulletRef;

    private float currentZRot = 0;



    private bool hasBeenHit = false; 

    // Start is called before the first frame update
    void Start()
    {
        current_respawn_time = respawn_max_time;
        char_rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        TakeDamage(); 

        if(health <= 0)
        {
            Die(); 
        }

        if (respawning)
        {
            Respawn(); 
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Damage")
        {
            Bullet object_hit_by = collision.gameObject.GetComponent<Bullet>();
            current_dmg_amount = object_hit_by.GetDamageAmount(); 
            hasBeenHit = true;
        }
    }

    private void TakeDamage()
    {
        if (hasBeenHit)
        {
            Debug.Log("took damage");
            char_rb.AddForce(transform.right * -1 * knockBackAmount, ForceMode2D.Impulse);
            health -= current_dmg_amount; 
            hasBeenHit = false; 
        }
    }

    private void Die()
    {
        Debug.Log("Ded");
        Transform allChildren = GetComponentInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            child.gameObject.SetActive(false);
        }
        respawning = true; 
    }

    private void Respawn()
    {
        // Set transform position to spawn point
        if(current_respawn_time <= 0)
        {
            Transform allChildren = GetComponentInChildren<Transform>();
            foreach (Transform child in allChildren)
            {
                child.gameObject.SetActive(true);
            }
            respawning = false;
            current_respawn_time = respawn_max_time;
            health = 100; 
        }
        else
        {
            Debug.Log(current_respawn_time);
            current_respawn_time -= Time.deltaTime; 
        }

        gameObject.SetActive(true);
    }
}
