using System.Reflection;
using Cysharp.Threading.Tasks;
using Lean.Common;
using Lean.Touch;
using UnityEngine;
using NaughtyAttributes;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VisualFlow
{
    public class DragLockAxis : BaseDrag, IPathHint
    {
        [SerializeField, Required] private BaseArea targetArea;
        [SerializeField] private Orientation orientation;
        [SerializeField] private bool checkTargetOnFingerUp;

        private bool inside;

        public Vector3[] Path { private set; get; }

        protected override async UniTask OnInitializing()
        {
            await base.OnInitializing();
            Path = new[] {TargetTransform.position, this.targetArea.Center};
            switch (this.orientation)
            {
                case Orientation.Horizontal:
                    Draggable.LockY = true;
                    break;
                case Orientation.Vertical:
                    Draggable.LockX = true;
                    break;
            }
        }

        protected override void OnDragStart()
        {
        }

        protected override void OnDragUpdate(LeanFinger finger)
        {
            if (!this.targetArea.Active) return;
            Vector3 checkPos = TargetTransform.position;
            checkPos.z = 0f;
            this.inside = this.targetArea.ContainsWorldSpace(checkPos);

            if (!this.checkTargetOnFingerUp && this.inside)
            {
                InvokeDragComplete();
            }
        }

        protected override void OnDragEnd(LeanFinger finger)
        {
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/Visual Flow/Mechanics/Drag Lock Axis", false, -10000)]
        private static void CreateDragIn(MenuCommand menuCommand)
        {
            var go = new GameObject(nameof(DragLockAxis));
            go.transform.position = Vector3.zero;
            var dragObject = go.AddComponent<DragLockAxis>();
            var graphicGo = new GameObject("DragTarget");
            graphicGo.transform.SetParent(go.transform);
            graphicGo.transform.localPosition = Vector3.zero;
            var spriteRenderer = graphicGo.AddComponent<SpriteRenderer>();
            var selectable = graphicGo.AddComponent<LeanSelectable>();
            var leanDragTranslate = graphicGo.AddComponent<Draggable>();
            leanDragTranslate.Use.RequiredSelectable = selectable;
            dragObject.GetType().BaseType.GetField("targetTransform", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.SetValue(dragObject, spriteRenderer.transform);
            dragObject.GetType().BaseType.GetField("selectable", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.SetValue(dragObject, selectable);
            dragObject.GetType().BaseType.GetField("leanDragTranslate", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.SetValue(dragObject, leanDragTranslate);
            var boxCol = graphicGo.AddComponent<BoxCollider>();
            boxCol.isTrigger = true;
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(go, go.GetInstanceID().ToString());
            Selection.activeObject = go;
            var area = BoxArea.Create(new MenuCommand(go.transform.parent.gameObject));
            area.transform.SetParent(go.transform);
            dragObject.GetType().GetField("targetArea", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.SetValue(dragObject, area);
        }
#endif
    }
}