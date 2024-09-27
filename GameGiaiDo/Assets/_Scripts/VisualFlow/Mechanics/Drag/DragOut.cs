using System.Reflection;
using DG.Tweening;
using Lean.Common;
using Lean.Touch;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VisualFlow
{
    public class DragOut : BaseDrag
    {
        [SerializeField] private BaseArea escapeArea;
        [SerializeField] private bool checkTargetOnFingerUp;

        private bool outside;

        protected override void OnDragStart()
        {
            TargetTransform.transform.DOScale(1.1f, 0.1f);
        }

        protected override void OnDragUpdate(LeanFinger finger)
        {
            if (finger.IsOverGui) return;
            if (!this.escapeArea.Active) return;
            Vector3 checkPos = TargetTransform.position;
            checkPos.z = 0f;
            this.outside = !this.escapeArea.ContainsWorldSpace(checkPos);

            if (!this.checkTargetOnFingerUp && this.outside)
            {
                InvokeDragComplete();
            }
        }

        protected override void OnDragEnd(LeanFinger finger)
        {
            TargetTransform.transform.DOScale(1f, 0.1f);
            if (this.outside && this.checkTargetOnFingerUp)
            {
                InvokeDragComplete();
            }
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/Visual Flow/Mechanics/Drag Out", false, -10000)]
        private static void CreateDragIn(MenuCommand menuCommand)
        {
            var go = new GameObject(nameof(DragOut));
            go.transform.position = Vector3.zero;
            var dragObject = go.AddComponent<DragOut>();
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
            dragObject.GetType().GetField("escapeArea", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.SetValue(dragObject, area);
        }
#endif
    }
}