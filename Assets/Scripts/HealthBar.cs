using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

<<<<<<< HEAD
namespace Assets.HeroEditor.Common.CharacterScripts
{
    public class HealthBar : MonoBehaviour
    {
        public Character dummy;
        public Slider slider;
        public Gradient gradient;
        public Image fill;

        void Start()
        {
            SetMaxHealth(dummy.health);
            SetHealth(dummy.health);
        }

        private void Update()
        {
            Debug.Log("Health " + dummy.health);
            SetHealth(dummy.health);
        }

        public void SetMaxHealth(int health)
        {
            slider.maxValue = health;
            slider.value = health;
            fill.color = gradient.Evaluate(1f);
        }

        public void SetHealth(int health)
        {
            slider.value = health;

            fill.color = gradient.Evaluate(slider.normalizedValue);
        }
=======
public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        slider.value = health;

        fill.color = gradient.Evaluate(slider.normalizedValue);
>>>>>>> 8f02b57a4eb49cd56e4dba60790378d77358a534
    }
}
