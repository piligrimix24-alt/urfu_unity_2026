using UnityEngine;

public class CubesSpinning : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;
    [Header("Cubes Settings")]
    [Range(0f, 7f)]
    [SerializeField] private float spinningRadius = 2f;
    [Range(40f, 160f)]
    [SerializeField] private float spinningSpeed = 1f;
    [SerializeField] private int cubeCount = 4;
    [SerializeField] private bool rotateClockWise = true;
    [SerializeField] private bool arrangeEvenly = true;
    [Range(20f, 50f)]
    [SerializeField] private float chainStep = 30f;

    private GameObject[] cubes;
    private float[] startAngles;
    //==================mew========================================
    private void Start()
    {
        if (cubePrefab != null)
        {
            cubes = new GameObject[cubeCount];
            startAngles = new float[cubeCount];
            var angleStep = 360f / cubeCount;

            for (int i = 0; i < cubeCount; i++)
            {
                GameObject newCube = Instantiate(cubePrefab, transform.position, Quaternion.identity);
                newCube.transform.SetParent(transform);
                cubes[i] = newCube;
                startAngles[i] = i * angleStep;

                startAngles[i] = arrangeEvenly ? i * angleStep : i * chainStep;
                SetCubePosition(newCube, startAngles[i]);
            }
        }
        else
        {
            Debug.LogError("Не назначен префаб");
        }
    }
    private void Update()
    {
        var dir = rotateClockWise ? -1f : 1f;
        transform.Rotate(0f, spinningSpeed * dir * Time.deltaTime, 0f);
        for (int i = 0; i < cubeCount; i++)
        {
            SetCubePosition(cubes[i], startAngles[i]);
        }
    }
    //==================mew========================================
    private void SetCubePosition(GameObject cube, float angleDeg)
    {
        var rad = angleDeg * Mathf.Deg2Rad;
        var x = Mathf.Cos(rad) * spinningRadius;
        var z = Mathf.Sin(rad) * spinningRadius;
        cube.transform.localPosition = new Vector3(x, 0f, z);
    }
}
