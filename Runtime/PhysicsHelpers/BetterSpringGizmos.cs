using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpringJoint))]
public class BetterSpringGizmos : MonoBehaviour
{
    [Range(0.01f,0.1f)]
    public float gizmoSize = 0.1f;
    
    private SpringJoint spring;

    private void Start()
    {
        GetSpringComponent();
    }

    private void GetSpringComponent()
    {
        if(spring == null)
            spring = this.GetComponent<SpringJoint>();
    }

    private void OnDrawGizmos()
    {
        GetSpringComponent();

        Vector3 selfAnchorPoint = this.transform.TransformPoint(spring.anchor);
        Vector3 connectedAnchorPoint = spring.connectedBody.transform.TransformPoint(spring.connectedAnchor);
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(selfAnchorPoint, gizmoSize);
        Gizmos.DrawSphere(connectedAnchorPoint, gizmoSize);
        
        Gizmos.color = new Color(1f, 0.75f, 0f);
        Gizmos.DrawWireSphere(selfAnchorPoint, gizmoSize);
        Gizmos.DrawWireSphere(connectedAnchorPoint, gizmoSize);

        Gizmos.color = new Color(1f, 0.5f, 0f);
        int segments = 10;
        for (int i = 0; i < segments; ++i)
        {
            float segmentPercent = (float)i / (float)(segments-1);
            Vector3 drawPos = Vector3.Lerp(selfAnchorPoint, connectedAnchorPoint, segmentPercent);
            Gizmos.DrawWireSphere(drawPos, gizmoSize * 0.5f);
        }
    }
}
