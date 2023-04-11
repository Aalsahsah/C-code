using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectZoom : MonoBehaviour
{
    private Transform target;
    [SerializeField] private float zoomSpeed;

    [SerializeField] private bool targetIsPlayer;

    private void OnEnable()
    {
        if (targetIsPlayer)
        {
            target = FindObjectOfType<PlayerData>().gameObject.transform;
        }
    }

    private void FixedUpdate()
    {
        Zoom();
        zoomSpeed *= 1.02f;
    }

    private void Zoom()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, zoomSpeed);
    }
}
