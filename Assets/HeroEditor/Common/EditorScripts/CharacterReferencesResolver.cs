using Assets.HeroEditor.Common.CharacterScripts;
using HeroEditor.Common;
using UnityEngine;

namespace Assets.HeroEditor.Common.EditorScripts
{
    public class CharacterReferencesResolver : MonoBehaviour
    {
        public Character Character;

        /// <summary>
        /// When character prefab is replaced, all references need to be fixed.
        /// </summary>
        public void OnValidate()
        {
            if (Character != null) return;

            Character = FindObjectOfType<Character>();

            FindObjectOfType<NewCharacterEditorBase>().Character = Character;
            FindObjectOfType<NewAnimationManager>().Character = Character;
            FindObjectOfType<WeaponControls>().Character = Character;
        }
    }
}