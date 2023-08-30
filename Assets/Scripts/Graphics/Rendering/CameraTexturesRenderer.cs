using System;
using System.Collections.Generic;
using Extensions;
using Providers;
using UnityEngine;
using Zenject;

namespace Graphics.Rendering
{
    public class CameraTexturesRenderer : MonoBehaviour
    {
        [Header("Preferences")]
        [SerializeField] private SingleLayer _renderMask;
        [SerializeField] private SingleLayer _noRenderMask;

        private HashSet<RenderTarget> _targets = new HashSet<RenderTarget>();

        private Camera _textureCamera;

        [Inject]
        private void Constructor(TextureCameraProvider textureCameraProvider)
        {
            _textureCamera = textureCameraProvider.Value;
        }

        #region MonoBehaviour

        private void Awake()
        {
            gameObject.SetActive(_targets.Count != 0);
        }

        private void Update()
        {
            foreach (var target in _targets)
            {
                _textureCamera.targetTexture = target.Texture;

                foreach (var possibleTarget in _targets)
                {
                    possibleTarget.TargetObject.SetLayerRecursive(
                        possibleTarget == target
                            ? _renderMask.LayerIndex
                            : _noRenderMask.LayerIndex);
                }

                _textureCamera.Render();
            }
        }

        #endregion

        public void Add(RenderTarget target)
        {
            _targets.Add(target);

            gameObject.SetActive(_targets.Count != 0);
        }

        public void Remove(RenderTarget target)
        {
            _targets.Remove(target);

            gameObject.SetActive(_targets.Count != 0);

            target.TargetObject.SetLayerRecursive(_noRenderMask.LayerIndex);
        }

        [Serializable]
        public class RenderTarget
        {
            public readonly RenderTexture Texture;
            public readonly GameObject TargetObject;

            public RenderTarget(RenderTexture texture, GameObject targetObject)
            {
                Texture = texture;
                TargetObject = targetObject;
            }
        }
    }
}
