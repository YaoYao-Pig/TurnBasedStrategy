using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private Vector3 targetPosition;
    [SerializeField]private float moveSpeed = 4.0f;
    [SerializeField] private Animator unitAnimator;

    private float stoppingDistance = .1f;
    private float unitRotateSpeed=6.0f;
    private void Awake()
    {
        targetPosition = transform.position;
    }
    private void Update()
    {

        MoveTowardsTarget();
    }


    private void MoveTowardsTarget()
    {
        if (Vector3.Distance(targetPosition, transform.position) >= stoppingDistance)
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            transform.position += moveSpeed * moveDirection * Time.deltaTime;

            transform.forward = Vector3.Lerp(transform.forward, moveDirection, unitRotateSpeed * Time.deltaTime);
            unitAnimator.SetBool("IsWalking", true);

        }
        else
        {
            unitAnimator.SetBool("IsWalking", false);
        }

    }
    public void Move(Vector3 _targetPosition)
    {
        this.targetPosition = _targetPosition;

    }
}
