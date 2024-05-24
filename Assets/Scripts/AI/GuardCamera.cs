using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GuardCamera : MonoBehaviour
{
    [SerializeField] Transform pivot;
    Transform Pivot
    {
        get
        {
            if (pivot == null)
                return transform;

            return pivot;
        }
    }

    [SerializeField][Range(1, 360)] float rotationSpeed = 1;
    [SerializeField][Range(0, 10)] float idleTime = 1;
    [SerializeField] SightSensor sightSensor;

    public UnityEvent OnDetectTarget;

    [SerializeField] Transform[] lookAtPositions;

    [SerializeField] LookAtType lookAtType;

    int currentLookAtIndex;
    bool isRotating;
    bool isBackward;

    enum LookAtType
    {
        Loop,
        PingPong
    }

    private void Awake()
    {

        foreach (var lookAtPosition in lookAtPositions)
        {
            lookAtPosition.parent = null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        sightSensor.OnSense += (target) =>
        {
            OnDetectTarget?.Invoke();
        };
    }

    void Update()
    {
        if (lookAtPositions.Length > 0)
        {
            switch (lookAtType)
            {
                case LookAtType.Loop:
                    LoopLookAt();
                    break;
                case LookAtType.PingPong:
                    PingPongLookAt();
                    break;
            }
        }
    }

    IEnumerator RotateToTarget(Transform target)
    {
        var originalRotation = Pivot.transform.rotation;
        var targetRotation = Quaternion.LookRotation(target.position - Pivot.transform.position);

        isRotating = true;

        float t = 0;
        float duration = Quaternion.Angle(originalRotation, targetRotation) / rotationSpeed;
        if (duration < 0.01)
        {
            isRotating = false;
            yield break;
        }

        while (t < duration)
        {
            t += Time.deltaTime;
            Pivot.transform.rotation = Quaternion.Slerp(originalRotation, targetRotation, t / duration);
            yield return null;
        }

        yield return new WaitForSeconds(idleTime);

        isRotating = false;
    }

    void LoopLookAt()
    {
        if (isRotating == false)
        {
            currentLookAtIndex++;
            if (currentLookAtIndex >= lookAtPositions.Length)
            {
                currentLookAtIndex = 0;
            }

            StopAllCoroutines();
            StartCoroutine(RotateToTarget(lookAtPositions[currentLookAtIndex]));
        }
    }

    void PingPongLookAt()
    {
        if (isRotating == false)
        {
            if (currentLookAtIndex == 0)
            {
                isBackward = false;
            }
            else if (currentLookAtIndex == lookAtPositions.Length - 1)
            {
                isBackward = true;
            }

            if (isBackward)
            {
                currentLookAtIndex--;
            }
            else
            {
                currentLookAtIndex++;
            }

            StopAllCoroutines();
            StartCoroutine(RotateToTarget(lookAtPositions[currentLookAtIndex]));
        }
    }

    [ContextMenu("Add Look At Position")]
    void AddLookAtPosition()
    {
        if (lookAtPositions == null)
        {
            lookAtPositions = new Transform[0];
        }

        Transform[] newLookAtPositions = new Transform[lookAtPositions.Length + 1];
        for (int i = 0; i < lookAtPositions.Length; i++)
        {
            newLookAtPositions[i] = lookAtPositions[i];
        }
        lookAtPositions = newLookAtPositions;

        lookAtPositions[lookAtPositions.Length - 1] = new GameObject().transform;
        lookAtPositions[lookAtPositions.Length - 1].position = transform.position;


        for (int i = 0; i < lookAtPositions.Length; i++)
        {
            lookAtPositions[i].transform.parent = transform;
            lookAtPositions[i].name = "LookAtPosition " + i;
        }
    }


    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < lookAtPositions.Length; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(lookAtPositions[i].position, 0.1f);

            if (i < lookAtPositions.Length - 1)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(lookAtPositions[i].position, lookAtPositions[i + 1].position);
            }
        }
    }


}
