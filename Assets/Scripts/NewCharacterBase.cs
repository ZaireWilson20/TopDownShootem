
using HeroEditor.Common;
using HeroEditor.Common.Data;
using HeroEditor.Common.Enums;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace HeroEditor.Common
{
    public abstract class NewCharacterBase : NetworkBehaviour
    {
        public SpriteCollection SpriteCollection;
        public SpriteRenderer EyesRenderer;
        public SpriteRenderer MouthRenderer;
        public SpriteRenderer BeardRenderer;
        public List<SpriteRenderer> BodyRenderers;
        public SpriteRenderer HelmetRenderer;
        public SpriteRenderer GlassesRenderer;
        public SpriteRenderer MaskRenderer;
        public SpriteRenderer EarringsRenderer;
        public SpriteRenderer PrimaryMeleeWeaponRenderer;
        public SpriteRenderer HairRenderer;
        public SpriteRenderer PrimaryMeleeWeaponTrailRenderer;
        public SpriteRenderer SecondaryMeleeWeaponTrailRenderer;
        public List<SpriteRenderer> ArmorRenderers;
        public SpriteRenderer CapeRenderer;
        public SpriteRenderer BackRenderer;
        public SpriteRenderer ShieldRenderer;
        public List<SpriteRenderer> BowRenderers;
        public List<SpriteRenderer> FirearmsRenderers;
        [Header("Animation")]
        public Animator Animator;
        public WeaponType WeaponType;
        public SpriteRenderer SecondaryMeleeWeaponRenderer;
        public SpriteRenderer EarsRenderer;
        public SpriteRenderer EyebrowsRenderer;
        public List<Sprite> Firearms;
        [Header("Body")]
        public Sprite Head;
        public Sprite HeadMask;
        public Sprite Ears;
        public Sprite Hair;
        public SpriteMask HairMask;
        public Sprite Beard;
        public List<Sprite> Body;
        [Header("Expressions")]
        public string Expression;
        [Header("Renderers")]
        public SpriteRenderer HeadRenderer;
        [Header("Equipment")]
        public Sprite Helmet;
        public List<Expression> Expressions;
        public Sprite Glasses;
        public Sprite Mask;
        public Sprite PrimaryMeleeWeapon;
        public Sprite SecondaryMeleeWeapon;
        public List<Sprite> Armor;
        public Sprite Cape;
        public Sprite Back;
        public Sprite Shield;
        public List<Sprite> Bow;
        public Sprite Earrings;

        protected NewCharacterBase() { }

        public void CopyFrom(NewCharacterBase character) { }
        public abstract void Initialize();
        public abstract void LoadFromJson(string serialized);
        public abstract string ToJson();
        public abstract void UpdateAnimation();
    }
}