using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Following : MonoBehaviour
{
    [SerializeField] private Transform Camera_Transform;

    void LateUpdate()
    {
        Camera_Transform.position = new Vector3(transform.position.x, transform.position.y, Camera_Transform.position.z);
    }
}
