using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropRNG : MonoBehaviour
{
    /// <summary>
    /// This script is used as the drop table for enemies.
    /// It does this by rolling each drop with a given fraction and if the number rolled is a 1,
    /// the item will be dropped.
    /// There is an option to roll every drop or instead to roll the drops in the order of rarest to most common.
    /// If any of the drops are rolled successfully, the more common drops will not get rolled.
    /// </summary>

    [SerializeField] bool canOnlyRollEachDropOnce;
    private bool obtainedDrop = false;

    //Make sure the drop rates are the same element in the array as the object to drop
    [SerializeField] private GameObject[] drops;
    [SerializeField] int[] dropChances;

    //This is where the script gets the enemy controller to know when to drop the items from the event
    [SerializeField] EnemyController enemyController;

    private void Start()
    {
        obtainedDrop = false;
        enemyController.OnEnemyDeath += RollDropTable;
    }
    private void OnDisable()
    {
        enemyController.OnEnemyDeath -= RollDropTable;
    }

    public void RollDropTable()
    {
        //This for loop iterates backwards through the drops from last element to first.
        //It is intended to be rarest to most common
        for(int numOfDropID = drops.Length - 1; numOfDropID > -1; numOfDropID--)
        {
            if(obtainedDrop == false)
            {
                RollDrop(numOfDropID);
            }
        }
    }

    
    void RollDrop(int dropNumber)
    {
        //The roll adds plus 1 to the drop chance because Random.Range is maxExclusive
        int roll = Random.Range(1, dropChances[dropNumber] + 1);

        if(roll == 1)
        {
            Vector3 dropOffset = new Vector3();

            //1 can be temporary, maybe make it dependent on enemy size???
            dropOffset.x = Random.Range(-1f, 1f);
            dropOffset.y = Random.Range(-1f, 1f);

            Instantiate(drops[dropNumber], transform.position + dropOffset, Quaternion.identity);

            if (canOnlyRollEachDropOnce)
            {
                obtainedDrop = true;
            }
        }
    }
    
}
