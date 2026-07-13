using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [Header("턴 표시")]
    [SerializeField] private GameObject quad_B;
    [SerializeField] private GameObject quad_W;

    private Renderer quad_B_Ren;
    private Renderer quad_W_Ren;

    public bool IsBlackTurn { get; private set; } = true;

    private void Awake()
    {
        quad_B_Ren = quad_B.GetComponent<Renderer>();
        quad_W_Ren = quad_W.GetComponent<Renderer>();

        UpdateTurnEffect();
    }

    public void NextTurn()
    {
        IsBlackTurn = !IsBlackTurn;
        UpdateTurnEffect();
    }

    public void ResetTurn()
    {
        IsBlackTurn = true;
        UpdateTurnEffect();
    }

    private void UpdateTurnEffect()
    {
        if (IsBlackTurn)
        {
            quad_B_Ren.material.SetColor("_EmissionColor", Color.white);
            quad_W_Ren.material.SetColor("_EmissionColor", Color.gray);
        }
        else
        {
            quad_B_Ren.material.SetColor("_EmissionColor", Color.gray);
            quad_W_Ren.material.SetColor("_EmissionColor", Color.white);
        }
    }
}