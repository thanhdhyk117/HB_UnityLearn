using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.SetSavePoint();// Lưu vị trí của SavePoint
                Debug.Log("Save point set at: " + transform.position);
            }
        }
    }
}
