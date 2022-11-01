using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyShip : MonoBehaviour
{
    public Shatter fragmentPool;
    public GameObject[] gameObjectsToDisable;

    private bool yeeted = false;
    public void Detonate(float time)
    {
        Invoke("Yeet", time);

    }

    private void Yeet()
    {
        if (!yeeted)
        {
            foreach (GameObject item in gameObjectsToDisable)
            {
                item.SetActive(false);
            }

            fragmentPool.gameObject.SetActive(true);
            fragmentPool.ExplodeFrags(fragmentPool.explosionForce, fragmentPool.upwardsMultiplier);
        }

        yeeted = true;
    }
}
