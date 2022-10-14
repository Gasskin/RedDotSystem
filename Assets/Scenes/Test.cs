using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        for (int i = 0; i < 100; i++)
        {
            Debug.Log(i);
            yield return new WaitForSeconds(1f);
        }
    }

}
