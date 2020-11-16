using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public bool hasBeenFired = false;
    [SerializeField]
    private int damage_amount = 30; 
    private Rigidbody2D rb; 
    public float fireForce = 3; 
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (hasBeenFired)
        {
            Vector2 bullVector = transform.right * fireForce;
            rb.AddForce(bullVector, ForceMode2D.Impulse);
        }
    }

    public void FireBullet()
    {
        hasBeenFired = true; 
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collided");
        Destroy(this.gameObject);
    }

    public int GetDamageAmount()
    {
        return damage_amount; 
    }
}
