using UnityEngine;
using Unicessing;

/// <summary>
/// A script for drawing rotating-cubes for Unicessing.
/// </summary>
/// <remarks>
/// HOW TO USE
/// <para>1. Launch Unity3D <https://unity3d.com>.</para>
/// <para>2. Import Unicessing <http://u3d.as/Dr2> to Your project.</para>
/// <para>3. Import assets for VR developers. For example, "Oculus Utilities for Unity", etc.</para>
/// <para>4. Attach this script to any object.</para>
/// <para>5. Set hand objects to hand fields on the inspector of this script.</para>
/// <para>6. Play the scene.</para>
/// <para>7. Push A-Button with the rhythm of music.</para>
/// </remarks>
public class RotatingCubesForUnicessing : UGraphics
{
    [SerializeField]
    private GameObject handLeft;
    [SerializeField]
    private GameObject handRight;

    private Vector3 handLeftPos, handRightPos;
    private Quaternion handLeftRot, handRightRot;
    private float angle;

    protected override void Setup()
    {
        rotateDegrees();
        stroke(100, 200, 255);
        noLights();
        angle = 0f;
    }

    protected override void Draw()
    {
        UpdateHands();
        DrawHands();
        DrawCubes();
    }

    void UpdateHands()
    {
        if ((handLeft == null) || (handRight == null))
        {
            Debug.LogError("Please set hand objects to hand fields on the inspector.");
        }
        else
        {
            handLeftPos = handLeft.transform.position;
            handRightPos = handRight.transform.position;
            handLeftRot = handLeft.transform.rotation;
            handRightRot = handRight.transform.rotation;
        }
    }

    void DrawHands()
    {
        DrawHandBox(handLeftPos, handLeftRot);
        DrawHandBox(handRightPos, handRightRot);
    }

    void DrawHandBox(Vector3 pos, Quaternion quat)
    {
        pushMatrix();
        translate(pos);
        rotate(quat.eulerAngles);
        noFill();
        box(0.15f);
        popMatrix();
    }

    void DrawCubes()
    {
        pushMatrix();

        randomSeed(0);

        for (int i = 3; i < 60; i++)
        {
            pushMatrix();

            translate(handRightPos + handRight.transform.up * 0f + handRight.transform.right * 0.04f + handRight.transform.forward * 0.04f);

            Quaternion cubeRot = handRightRot;
            rotate(cubeRot.eulerAngles);

            Color cubeColor = new Color(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f));
            stroke(cubeColor);
            fill(cubeColor);

            float x = (i + 1) * (i + 1) * (i + 1) * 0.00005f * Mathf.Cos(angle + i);
            float y = (i + 1) * (i + 1) * (i + 1) * 0.00005f * Mathf.Sin(angle + i);
            float z = i * i * 0.03f * handRightPos.z;
            Vector3 cubePos = new Vector3(x, y, z);

            Vector3 boxScale = (i + 1) * (i + 1) * (i + 1) * 0.000008f * Vector3.one;

            box(cubePos, boxScale);

            popMatrix();
        }

        if (OVRInput.Get(OVRInput.RawButton.A))
        {
            angle -= 0.04f;
        }
        else {
            angle -= 0.01f;
        }

        popMatrix();
    }
}
