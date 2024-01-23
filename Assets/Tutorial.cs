using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
             gameObject.GetComponent<Room>().enemyList.ForEach(e => e.SetActive(true));
        }
    }
}
