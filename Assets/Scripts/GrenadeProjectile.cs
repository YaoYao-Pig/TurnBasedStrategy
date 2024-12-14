using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    public Vector3 targetWorldPosition;
    [SerializeField] private int maxGrenadeDamage=30;
    private Action OnGrenadeBehaviourComplete;
    [SerializeField] private Transform grenadeVfxPrefab;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private AnimationCurve animationCurve;
    public static EventHandler OnAnyGrenadeExploded;
    private float totalDistance;
    private Vector3 positionXZ;
    private void Update()
    {
        Vector3 moveDir = (targetWorldPosition - positionXZ).normalized;
        float moveSpeed = 15f;
        positionXZ += moveDir * moveSpeed * Time.deltaTime;
        
        //acurve²ÉÑù
        float distance = Vector3.Distance(positionXZ, targetWorldPosition);
        float distanceNormalized = 1-distance / totalDistance;
        float maxHeight = totalDistance/3f;
        float positionY=animationCurve.Evaluate(distanceNormalized)* maxHeight;
        transform.position = new Vector3(positionXZ.x, positionY, positionXZ.z);

        float reachTargetDistance = .2f;
        if (Vector3.Distance(positionXZ, targetWorldPosition) < reachTargetDistance)
        {
            float damageRadius = 4f;
            Collider[] colliderArray=Physics.OverlapSphere(targetWorldPosition,damageRadius);
            foreach(Collider collider in colliderArray)
            {
                if(collider.TryGetComponent<Unit>(out Unit targetUnit))
                {
                    targetUnit.Damage(maxGrenadeDamage);
                }
                if (collider.TryGetComponent<DestructibleCrate>(out DestructibleCrate destructibleCrate))
                {
                    destructibleCrate.Damage();
                }
            }
            OnAnyGrenadeExploded?.Invoke(this, EventArgs.Empty);
            trailRenderer.transform.parent = null;
            Instantiate(grenadeVfxPrefab, targetWorldPosition+Vector3.up*1f, Quaternion.identity);
            Destroy(gameObject);
            OnGrenadeBehaviourComplete?.Invoke();
        }
    }
    public void SetUp(GridPosition targetGridPosition,Action OnGrenadeBehaviourComplete)
    {
        this.OnGrenadeBehaviourComplete = OnGrenadeBehaviourComplete;
        targetWorldPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
        positionXZ = transform.position;
        positionXZ.y = 0;
        totalDistance = Vector3.Distance(targetWorldPosition, positionXZ);
    }
}
