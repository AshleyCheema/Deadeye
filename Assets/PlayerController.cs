using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using DG.Tweening;
using System;

public class PlayerController : MonoBehaviour
{
    private Animator anim;

    private bool isAiming;

    [Header("Cinemachine")]
    [SerializeField]
    private CinemachineFreeLook thirdPersonCam;
    [SerializeField]
    private Camera cam;

    [Space]

    [Header("Settings")]
    private float originalZoom;
    public float originalOffsetAmount;
    public float zoomOffsetAmount;
    public float aimTime;

    [Space]

    [Header("LookAtSettings")]
    private float mouseX, MouseY;

    [SerializeField]
    private float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        originalZoom = thirdPersonCam.m_Orbits[1].m_Radius;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(1))
        {
            Aim(true);
            PlayerLookAround();
        }
        else
        {
            Aim(false);
        }
    }

    public void Aim(bool state)
    {
        isAiming = state;

        float xOrigOffset = state ? originalOffsetAmount : zoomOffsetAmount;
        float xCurrentOffset = state ? zoomOffsetAmount : originalOffsetAmount;
        float yOrigOffset = state ? 1.5f : 1.5f - .1f;
        float yCurrentOffset = state ? 1.5f - .1f : 1.5f;
        float zoom = state ? 20 : 40;

        DOVirtual.Float(xOrigOffset, xCurrentOffset, aimTime, HorizontalOffset);
        DOVirtual.Float(thirdPersonCam.m_Lens.FieldOfView, zoom, aimTime, CameraZoom);

        anim.SetBool("IsAiming", state);
    }

    //Zooms the camera in by adjusting the FOV value
    void CameraZoom(float cameraZoomDistance)
    {
        thirdPersonCam.m_Lens.FieldOfView = cameraZoomDistance;
    }

    //Changes the 3 rig's tracked offset on the X, on the FreeLook Cinemachine camera
    void HorizontalOffset(float x)
    {
        for (int i = 0; i < 3; i++)
        {
            CinemachineComposer c = thirdPersonCam.GetRig(i).GetCinemachineComponent<CinemachineComposer>();
            c.m_TrackedObjectOffset.x = x;
        }
    }

    void PlayerLookAround()
    {
        //mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
        //MouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;

        transform.LookAt(cam.transform.forward + (cam.transform.right * .1f));

        //transform.rotation = Quaternion.Euler(0, mouseX, 0);
    }
}
