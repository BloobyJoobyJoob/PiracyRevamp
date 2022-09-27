using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class Cannon
{
    public Transform transform;
    public ParticleSystem ps;
    public float postDelay = 1;
}

public class Cannons : MonoBehaviour
{
    public Cannon[] cannons;

    public float recoilDistance = 2;
    public float recoilTime = 0.2f;
    public float cannonAlignTime = 1;

    public float fireDelay;

    bool shooting;

    public bool TryFireCannons()
    {
        if (shooting)
        {
            return false;
        }
        else
        {
            shooting = true;

            StartCoroutine(BeginShooting(Time.time));

            return true;
        }
    }

    IEnumerator BeginShooting(float timeStarted)
    {
        foreach (Cannon cannon in cannons)
        {
            cannon.ps.Play();

            Transform t = cannon.transform;

            t.DOLocalMoveX(t.localPosition.x + (t.localRotation.eulerAngles.y == 270 ? 1 : -1 * recoilDistance), recoilTime).OnComplete(() => 
            {
                t.DOLocalMoveX(t.localPosition.x - (t.localRotation.eulerAngles.y == 270 ? 1 : -1 * recoilDistance), cannonAlignTime).SetEase(Ease.InSine);
            });

            yield return new WaitForSeconds(cannon.postDelay);
        }

        yield return new WaitForSeconds(fireDelay - (Time.time - timeStarted));

        shooting = false;
    }
}
