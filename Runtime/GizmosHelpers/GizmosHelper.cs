using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// ReSharper disable CheckNamespace
public class GizmosHelper
{
    private static Queue<GizmoData> GizmoQueue = new();
    
    public static void DrawWireAndSolidSphere(Vector3 center, float radius, Color color)
    {
        GizmoQueue.Enqueue(new GizmoData(){type = GizmoData.GizmoType.Sphere, center = center, radius = radius, color = color});
        GizmoQueue.Enqueue(new GizmoData(){type = GizmoData.GizmoType.WireSphere, center = center, radius = radius, color = color});
    }
    
    public static void DrawWireAndSolidCube(Vector3 center, Vector3 size, Color color)
    {
        GizmoQueue.Enqueue(new GizmoData(){type = GizmoData.GizmoType.Cube, center = center, size = size, color = color});
        GizmoQueue.Enqueue(new GizmoData(){type = GizmoData.GizmoType.WireCube, center = center, size = size, color = color});
    }
    public static void DrawWireSphere(Vector3 center, float radius, Color color)
    {
        GizmoQueue.Enqueue(new GizmoData(){type = GizmoData.GizmoType.WireSphere, center = center, radius = radius, color = color});
    }
    
    public static void DrawWireCube(Vector3 center, Vector3 size, Color color)
    {
        GizmoQueue.Enqueue(new GizmoData(){type = GizmoData.GizmoType.WireCube, center = center, size = size, color = color});
    }
    
    public static void DrawSphere(Vector3 center, float radius, Color color)
    {
        GizmoQueue.Enqueue(new GizmoData(){type = GizmoData.GizmoType.Sphere, center = center, radius = radius, color = color});
    }
    
    public static void DrawCube(Vector3 center, Vector3 size, Color color)
    {
        GizmoQueue.Enqueue(new GizmoData(){type = GizmoData.GizmoType.Cube, center = center, size = size, color = color});
    }
    
    public static void DrawLine(Vector3 from, Vector3 to, Color color)
    {
        GizmoQueue.Enqueue(new GizmoData(){type = GizmoData.GizmoType.Line, from = from, to = to, color = color});
    }
    
    public static void DrawWireDisc(Vector3 center, Vector3 normal, float radius, Color color, bool drawCross = false, int segments = 32)
    {
        normal = normal.normalized;

        Vector3 crossX = Vector3.right;
        
        if(normal != Vector3.up)
            crossX = Vector3.Cross(Vector3.up, normal);

        Vector3 crossY = Vector3.Cross(crossX, normal);
        
        // We'll just do debug draws for now. Enqueuing / Dequeuing so much seems wasteful.
        
        // Draw circle around center, up direction is normal
        //Debug.DrawRay(center, normal, Color.green);
        //Debug.DrawRay(center, crossX*0.2f, Color.magenta);
        //Debug.DrawRay(center, crossY*0.2f, Color.red);

        Vector3 lastDrawPos = center + (crossX* radius);
        //Debug.DrawRay(lastDrawPos, normal*0.2f, Color.magenta);
        
        for (int i = 0; i <= segments; ++i)
        {
            float angle = (360f / (segments)) * i;
            float xVal = Mathf.Cos(angle * Mathf.Deg2Rad);
            float yVal = Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector3 drawPos = center + (crossX * (xVal * radius)) + (crossY * (yVal * radius));
            Debug.DrawLine(lastDrawPos, drawPos, color);
            lastDrawPos = drawPos;
        }

        if (drawCross)
        {
            Debug.DrawLine(center + (crossX * radius), center - (crossX * radius), color);
            Debug.DrawLine(center + (crossY * radius), center - (crossY * radius), color);
        }
        
        //GizmoQueue.Enqueue(new GizmoData(){type = GizmoData.GizmoType.Line, from = from, to = to, color = color});
    }
    
    public static void DrawWireArrow(Vector3 from, Vector3 to, float arrowSize, Color color, bool drawCross = false, int segments = 32)
    {
        //arrowSize is percentage of length?

        float length = Vector3.Distance(from, to);
        float radius = length * arrowSize * 0.175f;
        Vector3 normal = (to-from).normalized;

        Vector3 center = Vector3.Lerp(from, to, 0.85f); // Arrow head start pos
        Vector3 crossX = Vector3.right;
        
        if(normal != Vector3.up)
            crossX = Vector3.Cross(Vector3.up, normal);

        Vector3 crossY = Vector3.Cross(crossX, normal);

        Vector3 lastDrawPos = center + (crossX* radius);
        //Debug.DrawRay(lastDrawPos, normal*0.2f, Color.magenta);
        
        Debug.DrawLine(from, to, color);
        
        for (int i = 0; i <= segments; ++i)
        {
            float angle = (360f / (segments)) * i;
            float xVal = Mathf.Cos(angle * Mathf.Deg2Rad);
            float yVal = Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector3 drawPos = center + (crossX * (xVal * radius)) + (crossY * (yVal * radius));
            Debug.DrawLine(lastDrawPos, drawPos, color);
            Debug.DrawLine(lastDrawPos, to, color);
            lastDrawPos = drawPos;
        }

        if (drawCross)
        {
            Debug.DrawLine(center + (crossX * radius), center - (crossX * radius), color);
            Debug.DrawLine(center + (crossY * radius), center - (crossY * radius), color);
        }
    }

    public static bool IsQueueEmpty()
    {
        return (GizmoQueue.Count <= 0);
    }
    
    public static GizmoData DrawNextGizmo()
    {
        GizmoData gizmoData = GizmoQueue.Dequeue();
        Gizmos.color = gizmoData.color;
        switch (gizmoData.type)
        {
            case(GizmoData.GizmoType.WireSphere):
                Gizmos.DrawWireSphere(gizmoData.center, gizmoData.radius);
                break;
            case(GizmoData.GizmoType.WireCube):
                Gizmos.DrawWireCube(gizmoData.center, gizmoData.size);
                break;
            case(GizmoData.GizmoType.Sphere):
                Gizmos.DrawSphere(gizmoData.center, gizmoData.radius);
                break;
            case(GizmoData.GizmoType.Cube):
                Gizmos.DrawCube(gizmoData.center, gizmoData.size);
                break;
            case(GizmoData.GizmoType.Line):
                Gizmos.DrawLine(gizmoData.from, gizmoData.to);
                break;
        }
        return (gizmoData);
    }
}

// ReSharper disable InconsistentNaming
public class GizmoData
{
    public enum GizmoType
    {
        WireSphere,
        WireCube,
        Sphere,
        Cube,
        Line,
    }
    
    public float radius;
    public Vector3 center;
    public Vector3 from;
    public Vector3 normal;
    public Vector3 size;
    public Vector3 to;
    public Color color;
    public GizmoType type;
}
