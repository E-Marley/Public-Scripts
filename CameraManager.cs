using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] CinemachineSmoothPath dollyPath;
    [SerializeField] CinemachineDollyCart dollyCart;
    private CinemachineVirtualCamera dollyCam;
    private float pathEndPos;
    private float pos;
    // Start is called before the first frame update

    private void Awake()
    {
        dollyCam = GetComponent<CinemachineVirtualCamera>();
    }
    void Start()
    {
        pathEndPos = dollyPath.PathLength;
    }

    // Update is called once per frame
    void Update()
    {
        pos = dollyCart.m_Position;
        if(pos == pathEndPos ||Input.GetButtonDown("Cancel"))
        {
            dollyCam.Priority = 9; //Switch to main 3rd person camera (priority 10)
        }
    }
}
