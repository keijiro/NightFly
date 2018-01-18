using UnityEngine;
using UnityEngine.Rendering;

public class BlitToBG : MonoBehaviour
{
    [SerializeField] Camera _targetCamera;

    CommandBuffer _cmd;
    RenderTexture _tempRT;

    void Start()
    {
        _tempRT = new RenderTexture(1920, 1080, 24);

        GetComponent<Camera>().targetTexture = _tempRT;

        _cmd = new CommandBuffer();
        _cmd.Blit(_tempRT, BuiltinRenderTextureType.CameraTarget);

        _targetCamera.AddCommandBuffer(CameraEvent.BeforeForwardOpaque, _cmd);
    }

    void OnDestroy()
    {
        Destroy(_tempRT);
    }
}
