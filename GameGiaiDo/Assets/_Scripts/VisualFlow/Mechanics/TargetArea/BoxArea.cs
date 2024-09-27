using System.Reflection;
using NaughtyAttributes;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VisualFlow
{
    public class BoxArea : BaseArea
    {
        [SerializeField]
        private Vector2 size = Vector2.one;

        [SerializeField] private BoxCollider2D boxCollider;

        public override Vector3 Center => transform.position;
        public override Collider2D Collider => this.boxCollider;

        private Transform trans;

        private void Awake()
        {
            this.trans = transform;
            if (this.boxCollider == null)
            {
                this.boxCollider = gameObject.AddComponent<BoxCollider2D>();
                this.boxCollider.isTrigger = true;
                this.boxCollider.size = this.size;
            }
        }

        public override bool Intersect(BaseArea otherArea)
        {
            return this.boxCollider.IsTouching(otherArea.Collider);
        }

        public override bool ContainsWorldSpace(Vector2 position)
        {
            var bounds = new Bounds(this.trans.position, this.size);
            return bounds.Contains(position);
        }

        public override bool ContainsScreenPosition(Vector2 screenPos, Camera renderCamera)
        {
            var bounds = new Bounds(this.trans.position, this.size);
            return bounds.IntersectRay(renderCamera.ScreenPointToRay(screenPos));
        }

        public override void Occupy(Vector2 position)
        {
            SetActive(false);
        }

#if UNITY_EDITOR
        private Bounds debugBounds;

        private void OnSizeChanged()
        {
            this.boxCollider.size = this.size;
        }

        private void OnDrawGizmos()
        {
            this.debugBounds.center = transform.position;
            this.debugBounds.size = this.size;
            //DebugExtension.DrawBounds(this.debugBounds, Color.red);
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(debugBounds.center, debugBounds.size);
        }

        [MenuItem("GameObject/Visual Flow/Areas/Box", false, -10000)]
        public static void CreateBoxArea(MenuCommand menuCommand)
        {
            var area = CreateVisualActionHelper.CreateGameObject("BoxArea", menuCommand).AddComponent<BoxArea>();
            var collider = area.gameObject.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            collider.size = Vector2.one;
            area.GetType().GetField("boxCollider", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.SetValue(area, collider);
        }

        public static BoxArea Create(MenuCommand menuCommand)
        {
            var area = CreateVisualActionHelper.CreateGameObject("BoxArea", menuCommand).AddComponent<BoxArea>();
            var collider = area.gameObject.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            collider.size = Vector2.one;
            area.GetType().GetField("boxCollider", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.SetValue(area, collider);
            return area;
        }
#endif
    }
}