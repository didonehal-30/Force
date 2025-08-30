using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(CircleCollider2D))]
public class GravityWell : MonoBehaviour
{
    [Header("Gravity Well Settings")]
    public string affectedTag = "Blocks";
    public float orbitRadius = 5f;
    public float angularSpeed = 1f;

    [Header("Ejection Settings")]
    public float maxChargeTime = 2f;         // Max time holding F to charge
    public float maxEjectionForce = 50f;     // Max speed when fully charged
    public float ejectionAngleOffset = 30f;  // Angle above tangent to launch

    private class OrbitState
    {
        public float angle;
        public Transform transform;
        public Rigidbody2D rb;
    }

    private List<OrbitState> orbiting = new();
    private float holdTime = 0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(affectedTag)) return;

        var rb = other.attachedRigidbody;
        if (rb == null) return;

        Vector2 diff = other.transform.position - transform.position;
        float initialAngle = Mathf.Atan2(diff.y, diff.x);

        rb.linearVelocity = Vector2.zero;
        rb.isKinematic = true;

        orbiting.Add(new OrbitState
        {
            angle = initialAngle,
            transform = other.transform,
            rb = rb
        });

        Debug.Log("Entered orbit: " + other.name);
    }

    private void Update()
    {
        // Orbit logic
        foreach (var obj in orbiting)
        {
            if (obj.transform == null) continue;

            obj.angle += angularSpeed * Time.deltaTime;
            Vector2 offset = new Vector2(Mathf.Cos(obj.angle), Mathf.Sin(obj.angle)) * orbitRadius;
            obj.transform.position = (Vector2)transform.position + offset;
        }

        // Charging E
        if (Input.GetKey(KeyCode.F))
        {
            holdTime += Time.deltaTime;
            holdTime = Mathf.Clamp(holdTime, 0f, maxChargeTime);
        }

        // Release to eject
        if (Input.GetKeyUp(KeyCode.F))
        {
            if (orbiting.Count > 0)
            {
                EjectObject(orbiting[0]);
                orbiting.RemoveAt(0);
            }

            holdTime = 0f;
        }
    }

    private void EjectObject(OrbitState obj)
    {
        if (obj == null || obj.rb == null) return;

        obj.rb.isKinematic = false;
        obj.rb.gravityScale = 1f;

        float chargePercent = Mathf.Clamp01(holdTime / maxChargeTime);
        float force = chargePercent * maxEjectionForce;

        Vector2 center = transform.position;
        Vector2 currentPos = obj.transform.position;

        // Radial vector from center to object
        Vector2 radial = (currentPos - center).normalized;

        // Tangent = perpendicular to radius (rotate 90� CCW)
        Vector2 tangent = new Vector2(-radial.y, radial.x);

        // Rotate tangent upward by ejectionAngleOffset degrees
        Quaternion rotation = Quaternion.Euler(0f, 0f, ejectionAngleOffset);
        Vector2 launchDir = (rotation * tangent).normalized;

        // Apply launch velocity
        obj.rb.linearVelocity = launchDir * force;

        Debug.Log($"[EJECT] {obj.transform.name} launched at angle {ejectionAngleOffset}� with force {force}, dir = {launchDir}");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        orbiting.RemoveAll(o => o.transform == other.transform);

        var rb = other.attachedRigidbody;
        if (rb != null)
        {
            rb.isKinematic = false;
        }
    }
}
