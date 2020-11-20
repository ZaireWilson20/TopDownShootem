#region Assembly HeroEditor.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Users\Zaire\Documents\GDev\TopDownShootem\Assets\HeroEditor\Common\HeroEditor.Common.dll
#endregion
using HeroEditor.Common;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HeroEditor.Common
{
    public abstract class NewCharacterEditorBase : MonoBehaviour
    {
        public SpriteCollection SpriteCollection;
        public Dropdown AngryMouthDropdown;
        public Dropdown DeadEyebrowsDropdown;
        public Dropdown DeadEyesDropdown;
        public Dropdown DeadMouthDropdown;
        [Header("Armor Parts")]
        public Dropdown ArmorArmLDropdown;
        public Dropdown ArmorArmRDropdown;
        public Dropdown ArmorForearmRDropdown;
        public Dropdown ArmorHandLDropdown;
        public Dropdown ArmorHandRDropdown;
        public Dropdown ArmorFingerDropdown;
        public Dropdown ArmorSleeveRDropdown;
        public Dropdown ArmorLegDropdown;
        public Dropdown ArmorPelvisDropdown;
        public Dropdown ArmorShinDropdown;
        public Dropdown ArmorTorsoDropdown;
        public Dropdown UpperArmorDropdown;
        public Dropdown LowerArmorDropdown;
        public Dropdown GlovesDropdown;
        public Dropdown BootsDropdown;
        [Header("Editors")]
        public GameObject MainEditor;
        public GameObject ExpressionEditor;
        public GameObject ArmorPartsEditor;
        [Header("Links")]
        public string LinkToBasicVersion;
        public string LinkToProVersion;
        [Header("Debug")]
        public Material DefaultMaterial;
        public bool ForceDefaultMaterial;
        public bool ForcePaint;
        public Dropdown AngryEyesDropdown;
        [Header("Expressions")]
        public Dropdown AngryEyebrowsDropdown;
        public Dropdown ArmorForearmLDropdown;
        public Dropdown SuppliesDropdown;
        public AnimationManager AnimationManager;
        public NewCharacterBase Character;
        [Header("UI")]
        public GameObject Editor;
        public GameObject CommonPalette;
        public GameObject SkinPalette;
        public List<Button> EditorOnlyButtons;
        public Dropdown EarsDropdown;
        public Dropdown HairDropdown;
        public Dropdown EyebrowsDropdown;
        public Dropdown EyesDropdown;
        public Dropdown MouthDropdown;
        public Dropdown BeardDropdown;
        public Dropdown BodyDropdown;
        public Dropdown HelmetDropdown;
        public Dropdown HeadDropdown;
        public Dropdown GlassesDropdown;
        public Dropdown MaskDropdown;
        public Dropdown ArmorDropdown;
        public Dropdown CapeDropdown;
        public Dropdown BackDropdown;
        public Dropdown MeleeWeapon1HDropdown;
        public Dropdown EarringsDropdown;
        public Dropdown MeleeWeaponPairedDropdown;
        public Dropdown MeleeWeapon2HDropdown;
        public Dropdown BowDropdown;
        public Dropdown Firearms1HDropdown;
        public Dropdown Firearms2HDropdown;
        public Dropdown ShieldDropdown;

        protected NewCharacterEditorBase() { }

        public void ClosePalette() { }
        public void Flip() { }
        public abstract void Load(string path);
        public virtual void Load(NewCharacterBase character) { }
        public void Navigate(string url) { }
        public void OpenPalette(PaletteButton paletteButton) { }
        public void PickColor(Color color) { }
        public void Randomize(bool armorParts) { }
        public abstract void Save(string path);
        public void Start() { }
        public void SwitchToArmorParts() { }
        public void SwitchToExpressions() { }
        protected abstract void FeedbackTip();
        protected void InitializeDropdowns() { }
        protected abstract void OpenPalette(GameObject palette, Color selected);
        protected void RequestFeedbackResult(bool success, bool basic) { }
        protected abstract void SetFirearmParams(SpriteGroupEntry entry);
    }
}