using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private float speed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = pointA.position; // Bắt đầu tại điểm A
        targetPosition = pointB.position; // Mục tiêu là điểm B
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            // Đổi hướng di chuyển khi đến điểm B
            if (targetPosition == pointB.position)
            {
                targetPosition = pointA.position; // Chuyển sang điểm A
            }
            else
            {
                targetPosition = pointB.position; // Chuyển sang điểm B
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Gắn Player vào Moving Platform
            collision.transform.SetParent(transform);
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Bỏ gắn Player khỏi Moving Platform
            collision.transform.SetParent(null);
        }
    }
}
