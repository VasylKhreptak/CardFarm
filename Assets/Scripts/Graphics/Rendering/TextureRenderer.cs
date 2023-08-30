using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Graphics.Rendering
{
    public class TextureRenderer : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _target;
        [SerializeField] private RawImage _rawImage;

        [Header("Preferences")]
        [SerializeField] private int _resolution = 512;

        private RenderTexture _renderTexture;
        private CameraTexturesRenderer.RenderTarget _renderTarget;

        private CameraTexturesRenderer _texturesRenderer;

        [Inject]
        private void Constructor(CameraTexturesRenderer cameraTexturesRenderer)
        {
            _texturesRenderer = cameraTexturesRenderer;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _rawImage ??= GetComponent<RawImage>();
        }

        private void Awake()
        {
            _renderTexture = new RenderTexture(_resolution, _resolution, 1);
            _rawImage.texture = _renderTexture;
            _renderTarget = new CameraTexturesRenderer.RenderTarget(_renderTexture, _target);
        }

        private void OnEnable()
        {
            _texturesRenderer.Add(_renderTarget);
        }

        private void OnDisable()
        {
            if (_texturesRenderer != null)
            {
                _texturesRenderer.Remove(_renderTarget);
            }
        }

        #endregion
    }
}
