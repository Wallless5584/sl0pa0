using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    [SerializeField] int damage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        other.GetComponent<PlayerController>().RecieveDmg(damage);
        Debug.Log($"{other.name} hit {gameObject.name}!");
    }
}
