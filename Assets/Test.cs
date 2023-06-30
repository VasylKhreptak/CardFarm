using UnityEngine;

[ExecuteAlways]
public class Test : MonoBehaviour
{
    private void Update()
    {
        Bounds bounds = GetComponent<Collider>().bounds;

        Debug.Log(bounds.ToString());
    }
}
