using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnDeath();
public class HeathComponent : MonoBehaviour
{
    public OnDeath onDeath;
    [SerializeField] float HP = 5;
    [SerializeField] float MaxHP = 5;
    [SerializeField] float regenRate = 0.5f;
    GameUIManager GameUIManager;

    private Coroutine healthRegen;

    private void Start()
    {
        GameUIManager = FindObjectOfType<GameUIManager>();
    }
    public void TakeDmg(float dmg)
    {
        HP = Mathf.Clamp(HP - dmg, 0, MaxHP);
        //print("Took dmg" + " Current health is at " + HP);
        UpdateHealthBar();
        if(HP <= 0)
        {
            if(onDeath != null)
            {
                onDeath.Invoke();
            }
        }
        StartRegenerateHealth();
    }

    void UpdateHealthBar()
    {
        GameUIManager.UpdatePlayerHealthBar(HP/MaxHP);
    }
    void StartRegenerateHealth()
    {
        if (healthRegen != null)
        {
            StopCoroutine(healthRegen);
        }
        healthRegen = StartCoroutine(RegenerateHealth());
    }

    IEnumerator RegenerateHealth()
    {
        yield return new WaitForSeconds(5);
        while(HP != MaxHP)
        {
            if(HP == MaxHP)
            {
                break;
            }
            HP = Mathf.Clamp(HP + regenRate, 0, MaxHP);
            UpdateHealthBar();
            yield return new WaitForEndOfFrame();
        }
    }
}
