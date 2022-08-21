using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class CameraScroll : MonoBehaviour
{
    public CinemachineVirtualCamera cam;
    public float minScroll;
    public float maxScroll;
    public float scrollAmount;

    private CinemachineFramingTransposer transposer;

    private float scroll;
    private void Start()
    {
        transposer = cam.GetCinemachineComponent<CinemachineFramingTransposer>();
    }
    private void Update()
    {
        scroll = Mouse.current.scroll.ReadValue().y;
        if (scroll != 0) transposer.m_CameraDistance = Mathf.Clamp(transposer.m_CameraDistance + (-scroll * scrollAmount * Time.deltaTime), minScroll, maxScroll);
    }
}
