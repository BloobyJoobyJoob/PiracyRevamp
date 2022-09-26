using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootCannons : MonoBehaviour
{
    public GameObject[] cannons;

    bool shooting;

    public bool FireCannons()
    {
        if (shooting)
        {
            return false;
        }
        else
        {
            shooting = true;
            return true;
        }
    }
}
