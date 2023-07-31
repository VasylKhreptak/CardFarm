using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using UnityEngine;

namespace Graphics.Animations
{
    public class GearsAnimation : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private List<Gear> _gears = new List<Gear>();

        [Header("Preferences")]
        [SerializeField] private float _generalSpeed;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        private void Awake()
        {
            foreach (var gear in _gears)
            {
                gear.InitialLocalRotation = gear.Transform.localRotation;
            }
        }

        private void Update()
        {
            RotateStep();
        }

        private void OnDisable()
        {
            ResetLocalRotation();
        }

        #endregion

        private void RotateStep()
        {
            foreach (var gear in _gears)
            {
                gear.Transform.Rotate(gear.RotationAxis, _generalSpeed * gear.SpeedAmplifier * Time.deltaTime, Space.Self);
            }
        }

        private void ResetLocalRotation()
        {
            foreach (var gear in _gears)
            {
                gear.Transform.localRotation = gear.InitialLocalRotation;
            }
        }

        private void Validate()
        {
            if (_gears != null && _gears.Count != 0) return;

            List<Transform> transforms = transform.gameObject.GetChildren().Select(x => x.transform).ToList();

            List<Gear> gears = new List<Gear>(transforms.Count);

            for (int i = 0; i < transforms.Count; i++)
            {
                gears.Add(new Gear
                {
                    Transform = transforms[i],
                    SpeedAmplifier = 1,
                    RotationAxis = Vector3.forward
                });
            }

            _gears = gears;
        }

        [Serializable]
        public class Gear
        {
            public Transform Transform;
            public float SpeedAmplifier;
            public Vector3 RotationAxis;
            [HideInInspector] public Quaternion InitialLocalRotation;
        }
    }
}
