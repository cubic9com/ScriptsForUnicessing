using UnityEngine;
using Unicessing;
using UnityEngine.PostProcessing;

/// <summary>
/// A script for drawing blinking-lights for Unicessing.
/// </summary>
/// <remarks>
/// HOW TO USE
/// <para>1. Launch Unity3D <https://unity3d.com>.</para>
/// <para>2. Import Unicessing <http://u3d.as/Dr2> to Your project.</para>
/// <para>3. Import Post Processing Stack <http://u3d.as/KTp> to Your project.</para>
/// <para>4. Import assets for VR developers. I use Oculus Utilities for Unity, Oculus Avatar SDK and VRTK.</para>
/// <para>5. Attach this script to any object.</para>
/// <para>6. Set hand objects to hand fields on the inspector of this script.</para>
/// <para>7. Set CenterEyeAnchor objects to Center Eye Anchor field on the inspector of this script.</para>
/// <para>8. Play the scene.</para>
/// <para>9. Raise and drop your right hand and Push and pull your left hand with the rhythm of music.</para>
/// </remarks>
public class BlinkingLightsForUnicessing : UGraphics
{
    [SerializeField]
    private GameObject handLeft;
    [SerializeField]
    private GameObject handRight;
    [SerializeField]
    private GameObject centerEyeAnchor;

    private Vector3 handLeftPos, handRightPos;
    private Quaternion handLeftRot, handRightRot;
    private PostProcessingBehaviour ppb;

    protected override void Setup()
    {
        rotateDegrees();
        stroke(100, 200, 255);
    }

    protected override void Draw()
    {
        UpdateHands();
        DrawHands();
        DrawLights();
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

        if (centerEyeAnchor == null)
        {
            Debug.LogError("Please set CenterEyeAnchor object to Center Eye Anchor field on the inspector.");
        }
        else
        {
            ppb = centerEyeAnchor.GetComponent<PostProcessingBehaviour>();
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

    void DrawLights()
    {
        BloomModel.Settings settings = ppb.profile.bloom.settings;
        settings.bloom.intensity = handRightPos.z * 1000f;
        ppb.profile.bloom.settings = settings;

        pushMatrix();
        for (float angle = 0; angle < Mathf.PI * 2; angle += Mathf.PI / 12f)
        {
            pushMatrix();
            translate(Mathf.Cos(angle) * 100f * handLeftPos.z, 1f * handRightPos.y + 1f * Mathf.Cos(Time.time * 2f + angle * 5f), Mathf.Sin(angle) * 100f * handLeftPos.z);
            rotate(new Vector3(0f, - Time.time % 360 * 100f, -Time.time % 360 * 100f));  //  回転する
            box(0.5f, 0.5f, 0.5f);
            popMatrix();
        }
        popMatrix();
    }
}
