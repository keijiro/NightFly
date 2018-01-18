using UnityEngine;
using Klak.Chromatics;

namespace Seido
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class PostFx : MonoBehaviour
    {
        #region Exposed attributes and public methods

        [SerializeField] CosineGradient[] _gradients;
        [SerializeField] float _gradientSelect = 0;
        [SerializeField] float _gradientFrequency = 1;
        [SerializeField] float _gradientSpeed = 1;
        [SerializeField, Range(0, 1)] float _opacity = 1;

        #endregion

        #region Private variables

        [SerializeField, HideInInspector] Shader _shader;
        Material _material;

        #endregion

        #region MonoBehaviour methods

        void OnDestroy()
        {
            if (Application.isPlaying)
                Destroy(_material);
            else
                DestroyImmediate(_material);
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (_material == null)
            {
                _material = new Material(_shader);
                _material.hideFlags = HideFlags.DontSave;
            }

            var grad = _gradients[Mathf.RoundToInt(_gradientSelect)];
            _material.SetVector("_GradientA", grad.coeffsA);
            _material.SetVector("_GradientB", grad.coeffsB);
            _material.SetVector("_GradientC", grad.coeffsC2);
            _material.SetVector("_GradientD", grad.coeffsD2);

            _material.SetFloat("_Frequency", _gradientFrequency);
            _material.SetFloat("_Opacity", _opacity);

            if (Application.isPlaying)
                _material.SetFloat("_LocalTime", _gradientSpeed * Time.time);
            else
                _material.SetFloat("_LocalTime", 0);

            Graphics.Blit(source, destination, _material, 0);
        }

        #endregion
    }
}
