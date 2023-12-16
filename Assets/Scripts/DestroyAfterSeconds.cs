using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    [SerializeField] float secondsToWait;
    // Update is called once per frame
    void Update()
    {
        StartCoroutine(DestroyAfterSecs());
    }
    IEnumerator DestroyAfterSecs()
    {
        yield return new WaitForSeconds(secondsToWait);
        Destroy(gameObject);
    }
}
