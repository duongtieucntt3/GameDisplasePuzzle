using UnityEngine;

namespace VisualFlow
{
    public abstract class BaseHeaderGraphic : MonoBehaviour
    {
        [SerializeField] private Transform customPivot;

        public abstract Bounds GraphicBounds { get; }
        public Vector3 Center => this.trans.position;

        private Transform trans;

        protected virtual void Awake()
        {
            this.trans = this.customPivot == null ? transform : this.customPivot;
            SetActive(false);
        }

        public abstract void SetActive(bool active);
    }
}