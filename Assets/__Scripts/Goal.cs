using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    static public bool goalMet = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            Goal.goalMet = true;
            Color color = GetComponent<Renderer>().material.color;
            color.a = 0.9f;
            GetComponent<Renderer>().material.color = color;
        }
    }
}
