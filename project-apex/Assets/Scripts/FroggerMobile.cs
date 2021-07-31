using UnityEngine;

public class FroggerMobile : ApexMobile
{
    [Header("---Settings---")]
    [SerializeField]
    private Vector2 stepAmount = new Vector2(1, 1);

    [Header("---Prefab Refs---")]
    [SerializeField]
    private Transform playerModelHandle;

    public void Move(Vector2 moveVector)
    {

    }
}
