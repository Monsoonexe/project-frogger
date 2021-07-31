﻿using UnityEngine;

[SelectionBase]
public class PlayerInputController : ApexMobile
{
    [Header("---Settings---")]
    public Vector3 gravityVector = new Vector3(0, -9.81f, 0);

    public float groundCheckRadius = 1.0f;
    public LayerMask groundLayerMask;
    public float groundCheckRayLength = 0.15f;

    [Header("---Prefab Refs---")]
    [SerializeField]
    private FroggerMobile characterController;

    [SerializeField]
    private Transform groundCheckPoint;

    [Header("---Inputs---")]
    [SerializeField]
    private StringVariable horizontalAxisName;

    [SerializeField]
    private StringVariable vertivalAxisName;

    /// <summary>
    /// Is the mobile contacting the ground within a threshold?
    /// </summary>
    public bool IsGrounded { get; private set; }

    private void Update()
    {
        //snap inputs instead of damping
        var hor = Input.GetAxis(horizontalAxisName);
        if (hor > 0) hor = 1;
        else if (hor < 0) hor = -1;

        //snap inputs instead of damping
        var vert = Input.GetAxis(vertivalAxisName);
        if (vert > 0) vert = 1;
        else if (vert < 0) vert = -1;

        var playerInput = new Vector2(
            hor, vert);
        //Debug.Log(playerInput);

        characterController.Move(playerInput);
    }

    //private Vector2 GetInputs()
    //{
    //    var playerInput = new Vector2
    //    (
    //        Input.GetAxis(horizontalAxisName),
    //        Input.GetAxis(vertivalAxisName)
    //    );

    //    return playerInput;
    //}

    private bool CheckIfGrounded()
    {
        var rayOrigin = myTransform.TransformPoint(
            groundCheckPoint.position);

        IsGrounded = Physics.SphereCast(rayOrigin,
            groundCheckRadius, gravityVector,
            out RaycastHit hitInfo, groundCheckRayLength,
            groundLayerMask);

        return IsGrounded;
    }
}
