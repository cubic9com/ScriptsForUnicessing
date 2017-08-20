using UnityEngine;
using Unicessing;

/// <summary>
/// A script for drawing color chart script for Unicessing.
/// </summary>
/// <remarks>
/// HOW TO USE
/// <para>1. Launch Unity3D <https://unity3d.com>.</para>
/// <para>2. Import Unicessing <http://u3d.as/Dr2> to Your project.</para>
/// <para>3. Import assets for VR developers. For example, "Oculus Utilities for Unity", etc.</para>
/// <para>4. Attach this script to any object.</para>
/// <para>5. Set hand objects to hand fields on the inspector of this script.</para>
/// <para>6. Play the scene.</para>
/// <para>7. Move your both hand with the rhythm of music.</para>
/// </remarks>
public class ColorChartForUnicessing : UGraphics
{
    [SerializeField]
    private GameObject handLeft;
    [SerializeField]
    private GameObject handRight;

    private const int ChartWidth = 8;
    private const int ChartOffset = -128;
    private const float ChartScale = 0.001f;
    private const float ColorDelta = 0.125f;
    private const int PosDelta = 32;

    private Vector3 handLeftPos, handRightPos;
    private Quaternion handLeftRot, handRightRot;

    protected override void Setup()
    {
        rotateDegrees();
        stroke(100, 200, 255);
    }

    protected override void Draw()
    {
        UpdateHands();
        DrawHands();
        DrawColorChart();
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

    void DrawColorChart()
    {
        pushMatrix();
        for (float x = 0; x <= ChartWidth; x++)
        {
            for (float y = 0; y <= ChartWidth; y++)
            {
                for (float z = 0; z <= ChartWidth; z++)
                {
                    rotate(handLeftPos.z * 1f, handLeftPos.x * 1f, handLeftPos.y * 1f);
                    pushMatrix();
                    Color col = new Color(x * ColorDelta, y * ColorDelta, z * ColorDelta);
                    stroke(col);
                    fill(col);
                    float posX = (x * PosDelta + ChartOffset) * ChartScale;
                    float posY = (y * PosDelta + ChartOffset) * ChartScale + handRightPos.y / 3;
                    float posZ = (z * PosDelta + ChartOffset) * ChartScale;
                    translate(posX, posY, posZ);
                    translate(0f, 0f, 0.45f);
                    rotate(handRightRot.eulerAngles);
                    box(0.02f, 0.02f, 0.02f);
                    popMatrix();
                }
            }
        }
        popMatrix();
    }
}
