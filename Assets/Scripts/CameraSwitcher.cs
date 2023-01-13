using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public static class CameraSwitcher 
{
    static List<CinemachineVirtualCamera> cameras = new List<CinemachineVirtualCamera>();

    public static CinemachineVirtualCamera activeCamera = null;

    public static bool isActiveCamera(CinemachineVirtualCamera camera)
    {
        return camera == activeCamera;
    }
    public static void SwitchCamera(CinemachineVirtualCamera camera)
    {
        camera.Priority = 10;
        activeCamera = camera;

        foreach (CinemachineVirtualCamera c in cameras)
        {
            if (c != camera && c.Priority != 0)
            {
                c.Priority = 0;
            }
        }
    }

    public static void Register(CinemachineVirtualCamera camera)
    {
        cameras.Add(camera);
        Debug.Log("Camera registered: " + camera);
    }

    public static void Unregister(CinemachineVirtualCamera camera)
    {
        cameras.Remove(camera);
        Debug.Log("Camera unregistered: " + camera);

    }
}
