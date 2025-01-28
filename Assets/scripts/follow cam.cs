using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followcam : MonoBehaviour
{
    [SerializeField] GameObject thingtofollow;

    private void LateUpdate()
    {
        transform.position = thingtofollow.transform.position + new Vector3(0, 0, -10);
    }
}
