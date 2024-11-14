using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider HealthSlider;
    public float maxHealth = 100f;
    public Slider ManaSlider;
    public float maxMana = 50f;
    public float Health;
    public float Mana;
    public Text healthText;
    public Text manaText;


    // Start is called before the first frame update
    void Start()
    {
        Health = maxHealth;
        Mana = maxMana;
    }

    // Update is called once per frame
    void Update()
    {
        if(HealthSlider.value != Health)
        {
            HealthSlider.value = Health;
            healthText.text = Health + "/" + maxHealth;
        }
        if(ManaSlider.value != Mana)
        {
            ManaSlider.value = Mana;
            manaText.text = Mana + "/" + maxMana;
        }
    }
}
