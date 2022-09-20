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
    private CinemachinePOV pov;

    private float scroll;

    private float speed;

    private void Start()
    {
        transposer = cam.GetCinemachineComponent<CinemachineFramingTransposer>();
        pov = cam.GetCinemachineComponent<CinemachinePOV>();
        speed = pov.m_VerticalAxis.m_MaxSpeed;
    }
    private void Update()
    {
        // Scroll in and out

        scroll = Mouse.current.scroll.ReadValue().y;
        if (scroll != 0) transposer.m_CameraDistance = Mathf.Clamp(transposer.m_CameraDistance + (-scroll * scrollAmount * Time.deltaTime), minScroll, maxScroll);

        // Lock camera aim when esc is pressed

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.None)
            {
                pov.m_VerticalAxis.m_MaxSpeed = speed;
                pov.m_HorizontalAxis.m_MaxSpeed = speed;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                pov.m_VerticalAxis.m_MaxSpeed = 0;
                pov.m_HorizontalAxis.m_MaxSpeed = 0;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}
