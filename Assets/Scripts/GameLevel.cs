using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Splines;

public class GameLevel : MonoBehaviour
{
    [SerializeField] private int gameLevel;
    [SerializeField] private Transform landerStartPositionTransform;
    [SerializeField] private Transform cameraZoomOutStartPositionTransform;
    [SerializeField] private CinemachineSplineCart splineCart;

    public event EventHandler onCameraReachDestination;

    private bool isCheckCameraEvent = true;

    private void Awake()
    {
        splineCart.PositionUnits = PathIndexUnit.Normalized;
    }

    private void Update()
    {
        if (isCheckCameraEvent && splineCart.SplinePosition >= 1)
        {
            isCheckCameraEvent = false;
            onCameraReachDestination?.Invoke(this, EventArgs.Empty);
        }
    }

    public int GetGameLevel()
    {
        return gameLevel;
    }

    public Vector3 GetLanderStartPosition()
    {
        return landerStartPositionTransform.position;
    }

    public Transform GetCinemachineSplineCartTransform()
    {
        return splineCart.transform;
    }

    public Transform GetCameraZoomOutStartTransform()
    {
        return cameraZoomOutStartPositionTransform.transform;
    }
}
