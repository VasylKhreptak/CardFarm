using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Graphics.Rendering
{
    public class TextureRenderer : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RenderTexture _texture;
        
        [Header("Preferences")]
        [SerializeField] private LayerMask _layer;

        private CameraTexturesRenderer.RenderTarget _target;

        private CameraTexturesRenderer _cameraTexturesRenderer;

        [Inject]
        private void Constructor(CameraTexturesRenderer cameraTexturesRenderer)
        {
            _cameraTexturesRenderer = cameraTexturesRenderer;
        }

        #region MonoBehavour

        private void Awake()
        {
            _target = new CameraTexturesRenderer.RenderTarget(_texture, _layer);
        }
        private void OnEnable()
        {
            _cameraTexturesRenderer.Add(_target);
        }

        private void OnDisable()
        {
            _cameraTexturesRenderer.Remove(_target);
        }

        #endregion
    }
}
