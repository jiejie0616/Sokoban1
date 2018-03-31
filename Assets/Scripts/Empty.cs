using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Empty : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log(other.transform.gameObject.tag);
        other.transform.position = this.transform.position;         // 位置对齐
    }
}
