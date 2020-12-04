using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace Assets.HeroEditor.Common.CharacterScripts
{
    /// <summary>
    /// General behaviour for projectiles: bullets, rockets and other.
    /// </summary>
    public class Projectile : NetworkBehaviour
    {
        public List<Renderer> Renderers;
        public GameObject Trail;
        public GameObject Impact;
	    public Rigidbody2D Rigidbody;
        public Character playerShooting;
        private float _catchUpDistance;
        public FirearmFire weapon;
        private float speedy = 0f;
        private int damage_amount = 30;


        public void Start()
        {
            Destroy(gameObject, 5);
            Rigidbody = GetComponent<Rigidbody2D>();
            Debug.Log(netId);
        }

	    public void Update()
	    {
            /*
		    if (Rigidbody != null && Rigidbody.useGravity)
		    {
			    transform.right = Rigidbody.velocity.normalized;
		    }
            */
            float catchUpValue = 0f;

            Vector3 moveValue = (weapon.Character.Firearm.Params.MuzzleVelocity * (weapon.Character.Firearm.FireTransform.right + weapon._curSpread)
    * Mathf.Sign(weapon.Character.transform.lossyScale.x) * weapon._randomSpeed) * Time.deltaTime;
            
            if (_catchUpDistance > 0f)
            {
                float step = (_catchUpDistance * Time.deltaTime);
                _catchUpDistance -= step;
                catchUpValue = step;

                if (_catchUpDistance < (moveValue.magnitude * .1f))
                {
                    catchUpValue += _catchUpDistance;
                    _catchUpDistance = 0f; 
                    
                }
            }

            speedy = moveValue.magnitude;
	    }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Object")
            {

                Character c = collision.gameObject.GetComponent<Character>();

                if (c != null && playerShooting.netId != c.netId)
                {
                    Bang(collision.gameObject);
                    c.TakeDamage(Mathf.Sign(Rigidbody.velocity.x), damage_amount);
                    if(c.GetHealth() <= 0 && !c.respawning)
                    {
                        playerShooting.CMDUpdateKillCount(); 
                    }
                }
                else
                {
                    //Debug.Log("come on");
                }
            }
            //Debug.Log("bullet hit");
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Object")
            {
                Bang(collision.gameObject);
            }
            Debug.Log("bullet hit");
        }
        public void OnTriggerEnter(Collider other)
        {
            Bang(other.gameObject);
            Debug.Log("bullet hit");
        }

        public void OnCollisionEnter(Collision other)
        {
            Bang(other.gameObject);
            Debug.Log("bullet hit");

        }

        private void Bang(GameObject other)
        {
            ReplaceImpactSound(other);
            Impact.SetActive(true);
            Destroy(GetComponent<SpriteRenderer>());
            Destroy(GetComponent<Rigidbody>());
            Destroy(GetComponent<Collider>());
            Destroy(gameObject, 1);

            foreach (var ps in Trail.GetComponentsInChildren<ParticleSystem>())
            {
                ps.Stop();
            }

	        foreach (var tr in Trail.GetComponentsInChildren<TrailRenderer>())
	        {
		        tr.enabled = false;
			}
		}

        private void ReplaceImpactSound(GameObject other)
        {
            var sound = other.GetComponent<AudioSource>();

            if (sound != null && sound.clip != null)
            {
                Impact.GetComponent<AudioSource>().clip = sound.clip;
            }
        }

        public void Initialize(float duration)
        {
            _catchUpDistance = (duration * speedy);

        }
    }
}