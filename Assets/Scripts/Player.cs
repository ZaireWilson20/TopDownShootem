using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D player_rb;
    public float speed_scale;

    [SerializeField]
    private Bullet bulletRef;

    private float currentZRot = 0;

    private float knockBackAmount = 2;
    public int ammo;
    public int magAmmo = 200;
    // Start is called before the first frame update
    void Start()
    {
        ammo = 50;
        player_rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(ammo > 0)
            { 
                ShootBullet();
                ammo -= 1;
            }
            else
            {
                if (magAmmo > 0)
                {
                    ammo += 50;
                    magAmmo -= 50;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0) * speed_scale;
        player_rb.AddForce(movement,ForceMode2D.Impulse);
        RotateCharacter(Input.mousePosition);


    }


    private void RotateCharacter(Vector3 mousePos)
    {
        Vector3 diff = Camera.main.ScreenToWorldPoint(mousePos) - transform.position;
        float rotationZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotationZ);
        currentZRot = rotationZ; 
    }

    private void ShootBullet()
    {
        Bullet bulletCopy = Instantiate(bulletRef);
        bulletCopy.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        bulletCopy.gameObject.SetActive(true);
        bulletCopy.transform.rotation = Quaternion.Euler(0, 0, currentZRot);
        bulletCopy.FireBullet();
    }
}
