using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Dice : MonoBehaviour
{
    [Header("Сила броска")]
    [SerializeField] private float minForce = 20f;
    [SerializeField] private float maxForce = 40f;
    [SerializeField] private float minTorque = 3f;
    [SerializeField] private float maxTorque = 6f;

    private Rigidbody _rb;
    private Vector3[] _faceNormals = {
        Vector3.up, Vector3.down, Vector3.right, Vector3.left, Vector3.forward, Vector3.back
    };
    private int[] _faceValues = { 1, 6, 4, 3, 2, 5 };

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Roll()
    {
        var randomDir = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(0.5f, 1f),
            Random.Range(-1f, 1f)
        ).normalized;

        var forceMagnitude = Random.Range(minForce, maxForce);
        var force = randomDir * forceMagnitude;
        var torque = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ).normalized * Random.Range(minTorque, maxTorque);

        _rb.AddForce(force, ForceMode.Impulse);
        _rb.AddTorque(torque, ForceMode.Impulse);
    }

    public bool IsStopped()
    {
        return _rb.linearVelocity.sqrMagnitude < 0.1 && _rb.angularVelocity.sqrMagnitude < 0.1;
    }

    public int GetTopFaceValue()
    {
        var maxDot = -Mathf.Infinity;
        var topValue = 1;
        for (int i = 0; i < _faceNormals.Length; i++)
        {
            var worldNormal = transform.TransformDirection(_faceNormals[i]);
            var dot = Vector3.Dot(worldNormal, Vector3.up);
            if (dot > maxDot)
            {
                maxDot = dot;
                topValue = _faceValues[i];
            }
        }

        return topValue;
    }
}