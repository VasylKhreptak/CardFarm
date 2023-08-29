using System;
using System.Collections.Generic;
using Providers;
using UnityEngine;
using Zenject;

namespace Graphics.Rendering
{
    public class CameraTexturesRenderer : MonoBehaviour
    {
        private HashSet<RenderTarget> _targets = new HashSet<RenderTarget>();

        private Camera _textureCamera;

        [Inject]
        private void Constructor(TextureCameraProvider textureCameraProvider)
        {
            _textureCamera = textureCameraProvider.Value;
        }

        #region MonoBehaviour

        private void Update()
        {
            foreach (var target in _targets)
            {
                _textureCamera.targetTexture = target.Texture;
                _textureCamera.cullingMask = target.Layer.value;
                _textureCamera.Render();
            }
        }

        #endregion

        public void Add(RenderTarget target)
        {
            _targets.Add(target);
        }

        public void Remove(RenderTarget target)
        {
            _targets.Remove(target);
        }

        [Serializable]
        public class RenderTarget
        {
            public readonly RenderTexture Texture;
            public readonly LayerMask Layer;

            public RenderTarget()
            {

            }

            public RenderTarget(RenderTexture texture, LayerMask layer)
            {
                Texture = texture;
                Layer = layer;
            }

            public override int GetHashCode()
            {
                return Layer.value;
            }
        }
    }
}
