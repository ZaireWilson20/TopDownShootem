using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.HeroEditor.Common.CommonScripts.Springs;
using Assets.HeroEditor.Common.Data;
using Assets.HeroEditor.Common.EditorScripts;
using Assets.HeroEditor.Common.Enums;
using HeroEditor.Common;
using HeroEditor.Common.Enums;
using Random = UnityEngine.Random;
using UnityEngine;
using Mirror;
using TMPro; 


namespace Assets.HeroEditor.Common.CharacterScripts
{
    /// <summary>
    /// Character presentation in editor. Contains sprites, renderers, animation and so on.
    /// </summary>
    public partial class Character : NewCharacterBase
    {
        [Header("Weapons")]
        public MeleeWeapon MeleeWeapon;
        public Firearm Firearm;
        public BowShooting BowShooting;
        public Ammo ammo;


		[SerializeField]
		public int health = 100;
        public int maxAmmo = 200;

		[SerializeField]
		private float respawn_max_time = 3f;
		private float current_respawn_time = 3f;
		public bool respawning = false;
		[SyncVar(hook = nameof(HandleKillChange))] private int kills = 0;
		private int playerNum = 0; 

		[Header("Service")]
		public LayerManager LayerManager;

		public GameObject viewCamera; 
		[SerializeField] private string displayName;

		private GamePlayer[] allPlayers; 
        public Vector2 BodyScale
	    {
		    get { return BodyRenderers.Single(i => i.name == "Torso").transform.localScale; }
		    set { FindObjectOfType<CharacterBodySculptor>().OnCharacterLoaded(value); }
	    }
     

        [SerializeField]
		private float speed_scale = 3f; 

		[SerializeField]
		Rigidbody2D player_rb;
		private Vector2 movementVector;

		[SerializeField] private ShootNetworkManager netMan = null;
		[SerializeField] private TMP_Text[] name_text = new TMP_Text[4];

		
		[SerializeField] private TMP_Text[] kills_text = new TMP_Text[4];
		//NetworkIdentity netId; 




		private ShootNetworkManager room;
		private ShootNetworkManager Room
		{
			get
			{
				if (room != null) { return room; }
				return room = NetworkManager.singleton as ShootNetworkManager;
			}
		}

		void Start()
		{
			Debug.Log(netId);
			player_rb = GetComponent<Rigidbody2D>();
			viewCamera = GameObject.FindGameObjectWithTag("MainCamera");
			current_respawn_time = respawn_max_time;
			SetDisplayName(); 

			
		}

		
		private void SetDisplayName()
        {
			allPlayers = FindObjectsOfType<GamePlayer>();
			foreach(GamePlayer p in allPlayers)
            {
                if (p.hasAuthority && hasAuthority)
                {
					displayName = p.GetDisplayName();
					playerNum = p.playerNum;
					 
					
                }
				p.kills = kills;
				name_text[playerNum].text = p.GetDisplayName();

			}
		}

		private void HandleKillChange(int oldVal, int newVal)
        {

			if (hasAuthority)
			{
				for (int i = 0; i < Room.gamePlayers.Count; i++)
				{

					kills_text[i].text = room.gamePlayers[i].kills.ToString();

				}
			}
		}
		private void FixedUpdate()
		{
			if (hasAuthority)
			{
				movementVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
				Vector3 movement = new Vector3(movementVector.x, movementVector.y, 0) * speed_scale;
				player_rb.AddForce(movement, ForceMode2D.Impulse);
			}
			//RotateCharacter(Input.mousePosition);
		}

        private void Update()
        {
			if (hasAuthority)
			{
				if (movementVector.x == 0 && movementVector.y == 0)
				{
					Animator.SetBool("Walk", false);
				}
				else
				{
					Animator.SetBool("Walk", true);


				}
				camFollow();
			}

			if (health <= 0)
			{
				Die();
			}

			if (respawning)
			{
				Respawn();
			}

		}

		private void Die()
		{
			Debug.Log("Ded");

			SpriteRenderer[] children = GetComponentsInChildren<SpriteRenderer>();
			Color newColor;
			foreach (SpriteRenderer child in children)
			{
				newColor = child.color;
				newColor.a = 0;
				child.color = newColor;
			}
			/*
			Transform allChildren = GetComponentInChildren<Transform>();
			foreach (Transform child in allChildren)
			{
				child.gameObject.SetActive(false);
			}*/
			respawning = true;
		}

		private void Respawn()
		{
			//gameObject.SetActive(true);
			Debug.Log("Respwan has authority? : " + hasAuthority);
			// Set transform position to spawn point
			if (current_respawn_time <= 0)
			{

				SpriteRenderer[] children = GetComponentsInChildren<SpriteRenderer>();
				Color newColor;
				foreach (SpriteRenderer child in children)
				{
					newColor = child.color;
					newColor.a = 1;
					child.color = newColor;
				}
				/*
				Transform allChildren = GetComponentInChildren<Transform>();
				foreach (Transform child in allChildren)
				{
					child.gameObject.SetActive(true);
				}*/
				respawning = false;
				current_respawn_time = respawn_max_time;
				health = 100;
				transform.position = PlayerSpawn.spawnPoints[Random.Range(0, PlayerSpawn.spawnPoints.Count)].gameObject.transform.position;
				
			}
			else
			{
				Debug.Log(current_respawn_time);
				current_respawn_time -= Time.deltaTime;
			}

			
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.gameObject.tag == "Damage")
			{
				Bullet object_hit_by = collision.gameObject.GetComponent<Bullet>();
				//this.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.right * -1 * 1.5f, ForceMode2D.Impulse);

				//current_dmg_amount = object_hit_by.GetDamageAmount();
				//hasBeenHit = true;
			}
		}

		private void camFollow()
		{
			float charPosX = transform.position.x;
			float charPosY = transform.position.y;
			float cameraOffset = 18.0f;

			viewCamera.transform.position = new Vector3(charPosX, charPosY, viewCamera.transform.position.z);
		}

		public void TakeDamage(float direction, int dmgAmount)
        {
			Debug.Log(direction + " " + gameObject.tag);
			gameObject.GetComponent<Rigidbody2D>().AddForce(transform.right * direction * 1.5f, ForceMode2D.Impulse);
			health -= dmgAmount;
            Debug.Log("Player's Health: " + health);
        }

		public int GetHealth()
        {
			return health; 
        }

		[Command]
		public void CMDUpdateKillCount()
        {

			kills++;
			Room.gamePlayers[playerNum].kills = kills; 

        }

		/// <summary>
		/// Called automatically when something was changed.
		/// </summary>
		public void OnValidate()
        {
            if (Head == null) return;

            Initialize();
        }
        
        /// <summary>
        /// Called automatically when object was enabled.
        /// </summary>
        public void OnEnable()
        {
            HairMask.isCustomRangeActive = true;
            HairMask.frontSortingOrder = HelmetRenderer.sortingOrder;
            HairMask.backSortingOrder = HairRenderer.sortingOrder;
			UpdateAnimation();
        }

	    public void OnDisable()
	    {
		    _animationState = -1;
	    }

	    private int _animationState = -1;

		/// <summary>
		/// Refer to Animator window to learn animation params, states and transitions!
		/// </summary>
		public override void UpdateAnimation()
        {
	        if (!Animator.isInitialized) return;

			var state = 100 * (int) WeaponType;

			Animator.SetInteger("WeaponType", (int) WeaponType);

	        if (WeaponType == WeaponType.Firearms1H || WeaponType == WeaponType.Firearms2H || WeaponType == WeaponType.FirearmsPaired)
	        {
		        Animator.SetInteger("MagazineType", (int)Firearm.Params.MagazineType);
		        Animator.SetInteger("HoldType", (int)Firearm.Params.HoldType);
		        state += (int) Firearm.Params.HoldType;
	        }

	        if (state == _animationState) return; // No need to change animation.

	        _animationState = state;
			Animator.SetBool("Ready", true);
            Animator.SetBool("Stand", true);

	        if (WeaponType == WeaponType.Firearms1H || WeaponType == WeaponType.Firearms2H)
	        {
		        Animator.Play("IdleFirearm", 0); // Upper body
			}
	        else
	        {
				Animator.Play("IdleMelee", 0); // Upper body
			}

            Animator.Play("Stand", 1); // Lower body
        }

        /// <summary>
        /// Initializes character renderers with selected sprites.
        /// </summary>
        public override void Initialize()
        {
			try // Disable try/catch for debugging.
            {
                TryInitialize();
            }
            catch (Exception e)
            {
                Debug.LogWarningFormat("Unable to initialize character {0}: {1}", name, e.Message);
            }
        }

		/// <summary>
		/// Set character's expression.
		/// </summary>
	    public void SetExpression(string expression)
		{
			if (Expressions.Count < 3) throw new Exception("Character must have at least 3 basic expressions: Default, Angry and Dead.");
			
			var e = Expressions.Single(i => i.Name == expression);

			Expression = expression;
			EyebrowsRenderer.sprite = e.Eyebrows;
			EyesRenderer.sprite = e.Eyes;
			MouthRenderer.sprite = e.Mouth;

			if (EyebrowsRenderer.sprite == null) EyebrowsRenderer.sprite = Expressions[0].Eyebrows;
			if (EyesRenderer.sprite == null) EyesRenderer.sprite = Expressions[0].Eyes;
			if (MouthRenderer.sprite == null) MouthRenderer.sprite = Expressions[0].Mouth;
		}

	    /// <summary>
	    /// Set character's body.
	    /// </summary>
	    public void SetBody(Sprite head, List<Sprite> body)
	    {
		    Head = head;
		    Body = body;
			Initialize();
		}

		#region Equipment

		/// <summary>
		/// Remove all equipment.
		/// </summary>
		public void ResetEquipment()
	    {
		    for (var i = 0; i < Armor.Count; i++)
		    {
			    Armor[i] = null;
		    }

		    for (var i = 0; i < Bow.Count; i++)
		    {
			    Bow[i] = null;
		    }

		    Helmet = Cape = Back = PrimaryMeleeWeapon = SecondaryMeleeWeapon = Shield = null;
			Firearms = new List<Sprite>();
		    WeaponType = WeaponType.Melee1H;
		    Initialize();
			UpdateAnimation();
	    }

		/// <summary>
		/// Equip melee weapon.
		/// </summary>
		/// <param name="sprite">Weapon sprite. It can be obtained from SpriteCollection.Instance.MeleeWeapon1H/2H[].Sprites.</param>
		/// <param name="trail">Melee weapon trail. It is LinkedSprite of SpriteGroupEntry.</param>
		/// <param name="twoHanded">If two-handed melee weapon.</param>
		public void EquipMeleeWeapon(Sprite sprite, Sprite trail, bool twoHanded = false)
	    {
		    PrimaryMeleeWeapon = sprite;
		    PrimaryMeleeWeaponTrailRenderer.sprite = trail;
			WeaponType = twoHanded ? WeaponType.Melee2H : WeaponType.Melee1H;
		    Initialize();
	    }
	   
	    /// <summary>
	    /// Equip paired melee weapons.
	    /// </summary>
	    public void EquipMeleeWeaponPaired(Sprite spritePrimary, Sprite trailPrimary, Sprite spriteSecondary, Sprite trailSecondary)
	    {
		    PrimaryMeleeWeapon = spritePrimary;
		    PrimaryMeleeWeaponTrailRenderer.sprite = trailPrimary;
			SecondaryMeleeWeapon = spriteSecondary;
		    SecondaryMeleeWeaponTrailRenderer.sprite = trailSecondary;
			WeaponType = WeaponType.MeleePaired;
		    Initialize();
	    }

		/// <summary>
		/// Equip bow.
		/// </summary>
		/// <param name="sprites">A list of sprites from bow atlas (multiple sprite). It can be obtained from SpriteCollection.Instance.Bow[].Sprites.</param>
		public void EquipBow(List<Sprite> sprites)
	    {
		    Bow = sprites;
		    WeaponType = WeaponType.Bow;
			Initialize();
	    }

		/// <summary>
		/// Equip firearm.
		/// </summary>
		/// <param name="sprites">A list of sprites from armor atlas (multiple sprite). It can be obtained from SpriteCollection.Instance.Firearms1H/2H[].Sprites.</param>
		/// <param name="firearmParams">Firearm params. Can be obtained from FirearmeCollection.Instance.Firearms[].</param>
		/// <param name="twoHanded">If two-handed firearm.</param>
		public void EquipFirearm(List<Sprite> sprites, FirearmParams firearmParams, bool twoHanded = false)
	    {
			Firearms = sprites;
		    Firearm.Params = firearmParams;
		    WeaponType = twoHanded ? WeaponType.Firearms2H : WeaponType.Firearms1H;
			Initialize();
		}

		/// <summary>
		/// Equip shield.
		/// </summary>
		/// <param name="sprite">Shield sprite. It can be obtained from SpriteCollection.Instance.Shield[].Sprite.</param>
		public void EquipShield(Sprite sprite)
	    {
		    Shield = sprite;
		    WeaponType = WeaponType.Melee1H;
		    Initialize();
	    }

		/// <summary>
		/// Equip helmet.
		/// </summary>
		/// <param name="sprite">Helmet sprite. It can be obtained from SpriteCollection.Instance.Helmet[].Sprite.</param>
		public void EquipHelmet(Sprite sprite)
	    {
		    Helmet = sprite;
		    Initialize();
	    }

		/// <summary>
		/// Equip armor.
		/// </summary>
		/// <param name="sprites">A list of sprites from armor atlas (multiple sprite). It can be obtained from SpriteCollection.Instance.Armor[].Sprites.</param>
		public void EquipArmor(List<Sprite> sprites)
		{
			Armor = sprites;
			Initialize();
		}

		/// <summary>
		/// Equip armor.
		/// </summary>
		/// <param name="sprites">A list of sprites from armor atlas (multiple sprite). It can be obtained from SpriteCollection.Instance.Armor[].Sprites.</param>
		public void EquipUpperArmor(List<Sprite> sprites)
		{
			foreach (var part in new[] { "ArmL", "ArmR", "Finger", "ForearmL", "ForearmR", "HandL", "HandR", "SleeveR", "Torso" })
			{
				SetArmorPart(part, sprites);
			}

			Initialize();
		}

		/// <summary>
		/// Equip lower armor.
		/// </summary>
		/// <param name="sprites">A list of sprites from armor atlas (multiple sprite). It can be obtained from SpriteCollection.Instance.Armor[].Sprites.</param>
		public void EquipLowerArmor(List<Sprite> sprites)
		{
			foreach (var part in new[] { "Leg", "Pelvis", "Shin" })
			{
				SetArmorPart(part, sprites);
			}

			Initialize();
		}

		/// <summary>
		/// Equip body armor.
		/// </summary>
		/// <param name="sprites">A list of sprites from armor atlas (multiple sprite). It can be obtained from SpriteCollection.Instance.Armor[].Sprites.</param>
		public void EquipBodyArmor(List<Sprite> sprites)
		{
			foreach (var part in new[] { "ArmL", "ArmR", "Torso", "Pelvis" })
			{
				SetArmorPart(part, sprites);
			}

			Initialize();
		}

		/// <summary>
		/// Equip gloves.
		/// </summary>
		/// <param name="sprites">A list of sprites from armor atlas (multiple sprite). It can be obtained from SpriteCollection.Instance.Armor[].Sprites.</param>
		public void EquipGloves(List<Sprite> sprites)
		{
			foreach (var part in new[] { "ForearmL", "ForearmR", "HandL", "HandR", "SleeveR", "Finger" })
			{
				SetArmorPart(part, sprites);
			}

			Initialize();
		}

		/// <summary>
		/// Equip boots.
		/// </summary>
		/// <param name="sprites">A list of sprites from armor atlas (multiple sprite). It can be obtained from SpriteCollection.Instance.Armor[].Sprites.</param>
		public void EquipBoots(List<Sprite> sprites)
		{
			foreach (var part in new[] { "Shin" })
			{
				SetArmorPart(part, sprites);
			}

			Initialize();
		}

        /// <summary>
        /// Alternative way to Hit character (with a script).
        /// </summary>
        public void Spring()
        {
            ScaleSpring.Begin(this, 1f, 1.1f, 40, 2);
        }

        private void SetArmorPart(string part, List<Sprite> armor)
	    {
		    var sprite = armor.Single(j => j.name == part);

		    Armor.RemoveAll(i => i == null);

		    for (var i = 0; i < Armor.Count; i++)
		    {
			    if (Armor[i] != null && Armor[i].name == part)
			    {
				    Armor[i] = sprite;
				    return;
			    }
		    }

		    Armor.Add(sprite);
	    }

		#endregion

	    /// <summary>
		/// Initializes character renderers with selected sprites.
		/// </summary>
		private void TryInitialize()
        {
			if (Expressions.All(i => i.Name != "Default") || Expressions.All(i => i.Name != "Angry") || Expressions.All(i => i.Name != "Dead"))
				throw new Exception("Character must have at least 3 basic expressions: Default, Angry and Dead.");

			HeadRenderer.sprite = Head;
            HairRenderer.sprite = Hair;
            HairRenderer.maskInteraction = Helmet == null || Helmet.name.Contains("[FullHair]") ? SpriteMaskInteraction.None : SpriteMaskInteraction.VisibleInsideMask;
            EarsRenderer.sprite = Ears;
			SetExpression(Expression);
			BeardRenderer.sprite = Beard;
            MapSprites(BodyRenderers, Body);
            HelmetRenderer.sprite = Helmet;
            GlassesRenderer.sprite = Glasses;
            MaskRenderer.sprite = Mask;
	        EarringsRenderer.sprite = Earrings;
			MapSprites(ArmorRenderers, Armor);
            CapeRenderer.sprite = Cape;
            BackRenderer.sprite = Back;
            PrimaryMeleeWeaponRenderer.sprite = PrimaryMeleeWeapon;
            SecondaryMeleeWeaponRenderer.sprite = SecondaryMeleeWeapon;
            BowRenderers.ForEach(i => i.sprite = Bow.SingleOrDefault(j => j != null && i.name.Contains(j.name)));
            FirearmsRenderers.ForEach(i => i.sprite = Firearms.SingleOrDefault(j => j != null && i.name.Contains(j.name)));
            ShieldRenderer.sprite = Shield;

            PrimaryMeleeWeaponRenderer.enabled = WeaponType != WeaponType.Bow;
            SecondaryMeleeWeaponRenderer.enabled = WeaponType == WeaponType.MeleePaired;
            BowRenderers.ForEach(i => i.enabled = WeaponType == WeaponType.Bow);
            
            if (Hair != null && Hair.name.Contains("[HideEars]") && HairRenderer.maskInteraction == SpriteMaskInteraction.None)
            {
                EarsRenderer.sprite = null;
            }

            switch (WeaponType)
            {
                case WeaponType.Firearms1H:
                case WeaponType.Firearms2H:
                    Firearm.AmmoShooted = 0;
	                BuildFirearms(Firearm.Params);
					break;
            }
		}

	    private void BuildFirearms(FirearmParams firearmParams)
	    {
		    Firearm.Params = firearmParams; // TODO:
		    Firearm.SlideTransform.localPosition = firearmParams.SlidePosition;
		    Firearm.MagazineTransform.localPosition = firearmParams.MagazinePosition;
		    Firearm.FireTransform.localPosition = firearmParams.FireMuzzlePosition;
		    Firearm.AmmoShooted = 0;

		    if (Firearm.Params.LoadType == FirearmLoadType.Lamp)
		    {
			    Firearm.Fire.SetLamp(firearmParams.GetColorFromMeta("LampReady"));
		    }
	    }

		private void MapSprites(List<SpriteRenderer> spriteRenderers, List<Sprite> sprites)
        {
            foreach (var part in spriteRenderers)
            {
                part.sprite = sprites.SingleOrDefault(i => i != null && i.name == part.name.Split('[')[0]);
            }
        }
    }
}