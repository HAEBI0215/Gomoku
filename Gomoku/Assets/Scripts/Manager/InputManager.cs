using UnityEngine;

public class InputManager : MonoBehaviour
{
    [Header("카메라 관련")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask panLayer;
    [SerializeField] private GameObject ChackSuR;
    [SerializeField] private PanManager panManager;

    public int CurrentX { get; private set; }
    public int CurrentY { get; private set; }

    private void Update()
    {
        UpdateChackSuR();
    }

    private void UpdateChackSuR()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, panLayer))
        {
            Vector3 nearPoint =
                panManager.GetNearPoint(hit.point, out int x, out int y);

            CurrentX = x;
            CurrentY = y;

            ChackSuR.transform.position = nearPoint;
        }
    }

    public bool Click()
    {
        return Input.GetMouseButtonDown(0);
    }
}