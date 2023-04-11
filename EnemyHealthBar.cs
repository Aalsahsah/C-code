using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    private GameObject enemyHealthBar;
    private EnemyController enemyController;

    private float healthPercentage;

    //newScale is what the new scale of the health bar object should be
    private Vector3 maxBarScale, newScale;

    //Initialize Image attached to game object & Initialize enemy data
    private void Awake()
    {
        enemyHealthBar = this.gameObject;
        enemyController = GetComponentInParent<EnemyController>();
    }

    void Start()
    {
        //subscribe to health change event
        enemyController.OnEnemyHealthChange += UpdateHealthBarFill;

        //set max and newScale vectors for reference to what the scale started as for updating damage
        maxBarScale = transform.localScale;
        newScale = transform.localScale;

        healthPercentage = 1f;
    }
    private void OnDestroy()
    {
        enemyController.OnEnemyHealthChange -= UpdateHealthBarFill;
    }

    private void UpdateHealthBarFill(int enemyCurrentHealth, int enemyMaxHealth)
    {
        //Must type case current health to a float so it can convert
        healthPercentage = (float)enemyCurrentHealth / enemyMaxHealth;
        newScale.x = maxBarScale.x * healthPercentage;
        enemyHealthBar.transform.localScale = newScale;
    }
}
