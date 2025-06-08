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

        StartCoroutine(MovePointToPoint());
    }

    // Update is called once per frame
    void Update()
    {
        Move();
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

    private Vector3 DirectionVector(Vector3 pointA,  Vector3 pointB)
    {
        return (pointB - pointA).normalized;
    }

    private void Move()
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

    IEnumerator MovePointToPoint()
    {
        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
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
                yield return new WaitForSeconds(1);
            }
            yield return null;
        }

    }    
}
