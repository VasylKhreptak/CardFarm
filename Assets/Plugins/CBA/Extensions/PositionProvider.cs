using System.Collections.Generic;

namespace CBA.Extensions
{
    public static class PositionProvider
    {
        public static UnityEngine.Vector3[] ToVector3Array(List<Animations.Transform.Move.PositionProvider.Core.PositionProvider> positionProviders)
        {
            UnityEngine.Vector3[] positions = new UnityEngine.Vector3[positionProviders.Count];

            for (int i = 0; i < positionProviders.Count; i++)
            {
                positions[i] = positionProviders[i].position;
            }

            return positions;
        }
        
        public static UnityEngine.Vector2[] ToVector2Array(List<Animations.Transform.Move.PositionProvider.Core.PositionProvider> positionProviders)
        {
            UnityEngine.Vector2[] positions = new UnityEngine.Vector2[positionProviders.Count];

            for (int i = 0; i < positionProviders.Count; i++)
            {
                positions[i] = positionProviders[i].position;
            }

            return positions;
        }
    }
}
