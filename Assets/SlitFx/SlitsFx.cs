using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[ExecuteInEditMode, RequireComponent(typeof(Camera))]
public class SlitsFx : MonoBehaviour, ITimeControl, IPropertyPreview
{
    #region Editable attributes

    [SerializeField] Color _color = Color.white;
    [SerializeField] float _frequency = 3;
    [SerializeField] float _speed = 3;
    [SerializeField] float _amplitude = 1;

    #endregion

    #region Internal resources

    [SerializeField, HideInInspector] Shader _shader;
    Material _material;

    #endregion

    #region Utility properties for internal use

    float LocalTime {
        get {
            if (_controlTime < 0)
                return Application.isPlaying ? Time.time : 0;
            else
                return _controlTime;
        }
    }

    #endregion

    #region ITimeControl implementation

    float _controlTime = -1;

    public void OnControlTimeStart()
    {
    }

    public void OnControlTimeStop()
    {
        _controlTime = -1;
    }

    public void SetTime(double time)
    {
        _controlTime = (float)time;
    }

    #endregion

    #region IPropertyPreview implementation

    public void GatherProperties(PlayableDirector director, IPropertyCollector driver)
    {
    }

    #endregion

    #region MonoBehaviour implementation

    void OnDestroy()
    {
        if (_material != null)
        {
            if (Application.isPlaying)
                Destroy(_material);
            else
                DestroyImmediate(_material);
        }
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (_material == null)
        {
            _material = new Material(_shader);
            _material.hideFlags = HideFlags.DontSave;
        }

        _material.SetColor("_Color", _color);
        _material.SetFloat("_Frequency", _frequency);
        _material.SetFloat("_Amplitude", _amplitude);
        _material.SetFloat("_LocalTime", LocalTime * _speed);

        Graphics.Blit(source, destination, _material, 0);
    }

    #endregion
}
