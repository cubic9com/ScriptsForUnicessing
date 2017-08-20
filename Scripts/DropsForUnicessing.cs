using UnityEngine;
using Unicessing;

/// <summary>
/// A script for drawing drops for Unicessing.
/// </summary>
/// <remarks>
/// HOW TO USE
/// <para>1. Launch Unity3D <https://unity3d.com>.</para>
/// <para>2. Import Unicessing <http://u3d.as/Dr2> to Your project.</para>
/// <para>3. Import assets for VR developers. For example, "Oculus Utilities for Unity", etc.</para>
/// <para>4. Attach this script to any object.</para>
/// <para>5. Set hand objects to hand fields on the inspector of this script.</para>
/// <para>6. Play the scene.</para>
/// <para>7. Raise and drop your right hand with the rhythm of music.</para>
/// </remarks>
public class DropsForUnicessing : UGraphics
{
    [SerializeField]
    private GameObject handLeft;
    [SerializeField]
    private GameObject handRight;

    private const int MatrixWidth = 8;
    private const int NumOfSpheres = 16;

    private Vector3 handLeftPos, handRightPos;
    private Quaternion handLeftRot, handRightRot;
    private bool nextTurnReady = false;
    private int chosenPosX = 0;
    private int chosenPosZ = 0;
    private float[] PosYs = new float[NumOfSpheres];

    protected override void Setup()
    {
        rotateDegrees();
        stroke(100, 200, 255);
        noLights();
    }

    protected override void Draw()
    {
        UpdateHands();
        DrawHands();
        DrawSpheres();
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

    void DrawSpheres()
    {

        pushMatrix();

        for (int x = 0; x < MatrixWidth; x++)
        {
            for (int z = 0; z < MatrixWidth; z++)
            {
                PosYs[0] = handRightPos.y;
                for (int i = 0; i < NumOfSpheres; i++)
                {
                    pushMatrix();
                    stroke(64f, x * 12f + 64f, z * 12f + 64f);
                    fill(64f, x * 12f + 64f, z * 12f + 64f);
                    if (x == chosenPosX && z == chosenPosZ)
                    {
                        translate((x * 32f - 128f) / 1000f, PosYs[i], (z * 32f - 128f) / 1000f);
                        translate(0f, -0.6f, 0.45f);
                        sphere(0.01f - 0.0005f * i);
                        popMatrix();
                    }
                    else
                    {
                        translate((x * 32f - 128f) / 1000f, 0, (z * 32f - 128f) / 1000f);
                        translate(0f, -0.6f, 0.45f);
                        sphere(0.01f);
                        popMatrix();
                        break;
                    }
                }
            }
        }

        if (handRightPos.y < 0f && nextTurnReady)
        {
            chosenPosX = Random.Range(0, MatrixWidth);
            chosenPosZ = Random.Range(0, MatrixWidth);
            nextTurnReady = false;
        }
        if (handRightPos.y > 0.1f)
        {
            nextTurnReady = true;
        }
        for (int i = NumOfSpheres - 2; i >= 0; i--)
        {
            PosYs[i + 1] = PosYs[i];
        }

        popMatrix();
    }
}
