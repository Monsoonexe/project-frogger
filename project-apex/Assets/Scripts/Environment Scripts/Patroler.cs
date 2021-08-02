using UnityEngine;

public class Patroler : ApexMonoBehaviour
{
    [Header("---Settings---")]
    [SerializeField]
    private float moveSpeed = 10.0f;

    [Header("---Prefab Refs---")]
    [SerializeField]
    [Tooltip("Handle on the thing that is to be moved.")]
    private Transform mobileHandle;

    [SerializeField]
    private Transform[] points;

    //runtime
    private int waypointIndex;

    private void Reset()
    {
        mobileHandle = GetComponent<Transform>();
    }

    private void Update()
    {
        var currentPosition = mobileHandle.position;
        var targetPosition = points[waypointIndex].position;
        var step = moveSpeed * ApexGameController.DeltaTime;

        //combine all the above
        var moveVector = Vector3.MoveTowards(
            currentPosition, targetPosition, step);

        mobileHandle.position = moveVector;

        //determine if waypoint is complete
        if(moveVector == currentPosition)
        {   //loop waypoint index
            waypointIndex = (++waypointIndex >= points.Length) 
                ? 0 : waypointIndex;
        }
    }
}
