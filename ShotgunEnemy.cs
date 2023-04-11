using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunEnemy : MonoBehaviour
{
public GameObject player; 
private float speed;
[SerializeField]private float initSpeed;
public float effectiveRange;
private float distance; 
    // Start is called before the first frame update
    void Start()
    {
        speed = initSpeed;
    }

    // Update is called once per frame
    void Update()
    {
       distance = Vector2.Distance(transform.position, player.transform.position); 
       Vector2 direction = player.transform.position - transform.position;
       float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

       transform.position = Vector2.MoveTowards (this.transform.position, player.transform.position, speed * Time.deltaTime);
       transform.rotation = Quaternion.Euler(Vector3.forward * angle);

       if (distance <= effectiveRange)
       {
       speed = 0;
       }
       else 
       {
       speed = initSpeed;
       }
    }
}
