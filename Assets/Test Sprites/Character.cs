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

    //private Bullet bulletRef;

    private float currentZRot = 0;



    private bool hasBeenHit = false; 

    // Start is called before the first frame update
    void Start()
    {
        char_rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        TakeDamage(); 
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Damage")
        {
            hasBeenHit = true;
        }
    }

    private void TakeDamage()
    {
        if (hasBeenHit)
        {
            Debug.Log("took damage");
            char_rb.AddForce(transform.right * -1 * knockBackAmount, ForceMode2D.Impulse);
            hasBeenHit = false; 
        }
    }
}
