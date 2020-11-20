using HeroEditor.Common;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HeroEditor.Common
{
    public class NewAnimationManager : MonoBehaviour
    {
        public NewCharacterBase Character;
        public Text UpperClipName;
        public Text LowerClipName;
        public List<string> UpperAnimationClips;
        public List<string> LowerAnimationClips;

        public NewAnimationManager() { }

        public void PlayLowerBodyAnimation(int direction) { }
        public void PlayUpperBodyAnimation(int direction) { }
        public void Start() { }
    }
}