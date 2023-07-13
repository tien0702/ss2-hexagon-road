using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeEnd : MonoBehaviour
{
    public void OnDestroy()
    {
        Destroy(gameObject);
    }
}
