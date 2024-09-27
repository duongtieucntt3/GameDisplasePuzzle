#if UNITY_EDITOR
using Spine.Unity;
using UnityEditor;
using UnityEngine;

namespace VisualFlow
{
    public static class CreateVisualActionHelper
    {
        [MenuItem("GameObject/Visual Flow/Actors/Spine", false, -10000)]
        private static void SpineActor(MenuCommand menuCommand)
        {
            var actorGo = new GameObject("SpineActor");
            actorGo.transform.position = Vector3.zero;
            var graphicGo = new GameObject("Graphic");
            graphicGo.transform.SetParent(actorGo.transform);
            graphicGo.transform.localPosition = Vector3.zero;
            graphicGo.AddComponent<SkeletonAnimation>();
            GameObjectUtility.SetParentAndAlign(actorGo, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(actorGo, actorGo.GetInstanceID().ToString());
            Selection.activeObject = actorGo;
        }

        [MenuItem("GameObject/Visual Flow/Actors/Sprite", false, -10000)]
        private static void SpriteActor(MenuCommand menuCommand)
        {
            var actorGo = new GameObject("SpriteActor");
            actorGo.transform.position = Vector3.zero;
            var graphicGo = new GameObject("Graphic");
            graphicGo.transform.SetParent(actorGo.transform);
            graphicGo.transform.localPosition = Vector3.zero;
            graphicGo.AddComponent<SpriteRenderer>();
            GameObjectUtility.SetParentAndAlign(actorGo, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(actorGo, actorGo.GetInstanceID().ToString());
            Selection.activeObject = actorGo;
        }

        // [MenuItem("GameObject/Visual Flow/Actions/Drag/Draggable Object", false, -10000)]
        // private static void DraggableObject(MenuCommand menuCommand)
        // {
        //     var go = new GameObject("DragObject");
        //     go.transform.position = Vector3.zero;
        //     var dragObject = go.AddComponent<Drag>();
        //     var graphicGo = new GameObject("Graphic");
        //     graphicGo.transform.SetParent(go.transform);
        //     graphicGo.transform.localPosition = Vector3.zero;
        //     var spriteRenderer = graphicGo.AddComponent<SpriteRenderer>();
        //     var selectable = graphicGo.AddComponent<LeanSelectable>();
        //     var leanDragTranslate = go.GetComponent<LeanDragTranslate>();
        //     leanDragTranslate.Use.RequiredSelectable = selectable;
        //     dragObject.GetType().GetField("spriteRenderer", BindingFlags.Instance | BindingFlags.NonPublic)
        //         ?.SetValue(dragObject, spriteRenderer);
        //     dragObject.GetType().GetField("selectable", BindingFlags.Instance | BindingFlags.NonPublic)
        //         ?.SetValue(dragObject, selectable);
        //     var boxCol = graphicGo.AddComponent<BoxCollider>();
        //     boxCol.isTrigger = true;
        //     GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        //     Undo.RegisterCreatedObjectUndo(go, go.GetInstanceID().ToString());
        //     Selection.activeObject = go;
        //
        //     SpriteHeader(new MenuCommand(go));
        //     DragBoxDropZone(new MenuCommand(go.transform.parent.gameObject));
        // }
        //
        // [MenuItem("GameObject/Visual Flow/Actions/Drag/Header Sprite", false, -10000)]
        // private static void SpriteHeader(MenuCommand menuCommand)
        // {
        //     var go = new GameObject("SpriteHeader");
        //     go.transform.position = Vector3.zero;
        //     var spriteHeader = go.AddComponent<SpriteHeader>();
        //     var graphicGo = new GameObject("Graphic");
        //     graphicGo.transform.SetParent(go.transform);
        //     graphicGo.transform.localPosition = Vector3.zero;
        //     var renderer = graphicGo.AddComponent<SpriteRenderer>();
        //     spriteHeader.GetType().GetField("spriteRenderer", BindingFlags.Instance | BindingFlags.NonPublic)
        //         ?.SetValue(spriteHeader, renderer);
        //     GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        //     Undo.RegisterCreatedObjectUndo(go, go.GetInstanceID().ToString());
        //     Selection.activeObject = go;
        // }
        //
        // [MenuItem("GameObject/Visual Flow/Actions/Drag/Header Spine", false, -10000)]
        // private static void MeshHeader(MenuCommand menuCommand)
        // {
        //     var go = new GameObject("SpineHeader");
        //     go.transform.position = Vector3.zero;
        //     var header = go.AddComponent<MeshHeader>();
        //     var graphicGo = new GameObject("Graphic");
        //     graphicGo.transform.SetParent(go.transform);
        //     graphicGo.transform.localPosition = Vector3.zero;
        //     graphicGo.AddComponent<SkeletonAnimation>();
        //     var renderer = graphicGo.GetComponent<MeshRenderer>();
        //     header.GetType().GetField("meshRenderer", BindingFlags.Instance | BindingFlags.NonPublic)
        //         ?.SetValue(header, renderer);
        //     GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        //     Undo.RegisterCreatedObjectUndo(go, go.GetInstanceID().ToString());
        //     Selection.activeObject = go;
        // }
        //
        // [MenuItem("GameObject/Visual Flow/Actions/Show Hint", false, -10000)]
        // private static void ShowHint(MenuCommand menuCommand)
        // {
        //     // var go = new GameObject("ShowHint");
        //     // go.transform.position = Vector3.zero;
        //     // var showHint = go.AddComponent<ShowHint>();
        //     //
        //     // var startGo = new GameObject("Start");
        //     // startGo.transform.SetParent(go.transform);
        //     // startGo.transform.localPosition = Vector3.zero;
        //     //
        //     // var endGo = new GameObject("End");
        //     // endGo.transform.SetParent(go.transform);
        //     // endGo.transform.localPosition = Vector3.zero;
        //     //
        //     // showHint.GetType().GetField("start", BindingFlags.Instance | BindingFlags.NonPublic)
        //     //     ?.SetValue(showHint, startGo.transform);
        //     //
        //     // showHint.GetType().GetField("end", BindingFlags.Instance | BindingFlags.NonPublic)
        //     //     ?.SetValue(showHint, endGo.transform);
        //     //
        //     // GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        //     // Undo.RegisterCreatedObjectUndo(go, go.GetInstanceID().ToString());
        //     // Selection.activeObject = go;
        // }
        //
        // [MenuItem("GameObject/Visual Flow/Actions/Drag/Drop Zone Box", false, -10000)]
        // private static void DragBoxDropZone(MenuCommand menuCommand)
        // {
        //     CreateGameObject("BoxDropZone", menuCommand).AddComponent<BoxArea>();
        // }
        //
        // [MenuItem("GameObject/Visual Flow/Actions/Drag/Drop Zone Polygon", false, -10000)]
        // private static void DragPolygonDropZone(MenuCommand menuCommand)
        // {
        //     CreateGameObject("PolygonDropZone", menuCommand).AddComponent<PolygonArea>();
        // }
        //
        // [MenuItem("GameObject/Visual Flow/Actions/Sequence", false, -10000)]
        // private static void Sequence(MenuCommand menuCommand)
        // {
        //     CreateGameObject("Sequence", menuCommand).AddComponent<Sequence>();
        // }
        //
        // [MenuItem("GameObject/Visual Flow/Actions/Parallel", false, -10000)]
        // private static void Parallel(MenuCommand menuCommand)
        // {
        //     CreateGameObject("Parallel", menuCommand).AddComponent<Parallel>();
        // }
        //
        // [MenuItem("GameObject/Visual Flow/Actions/StopPlaying", false, -10000)]
        // private static void StopPlaying(MenuCommand menuCommand)
        // {
        //     // CreateGameObject("StopPlaying", menuCommand).AddComponent<StopPlaying>();
        // }
        //
        //
        // [MenuItem("GameObject/Visual Flow/Actions/Spine/Play Spine Animation", false, -10000)]
        // private static void PlaySpineAnim(MenuCommand menuCommand)
        // {
        //     CreateGameObject("PlaySpineAnim", menuCommand).AddComponent<PlaySpineAnim>();
        // }
        //
        // [MenuItem("GameObject/Visual Flow/Actions/Wait Milliseconds", false, -10000)]
        // private static void WaitMs(MenuCommand menuCommand)
        // {
        //     CreateGameObject("WaitMs", menuCommand).AddComponent<WaitMs>();
        // }
        //
        // [MenuItem("GameObject/Visual Flow/Actions/Mechanics/Set Active Drop Target", false, -10000)]
        // private static void SetActiveDropTarget(MenuCommand menuCommand)
        // {
        //     CreateGameObject("SetActiveDropTarget", menuCommand).AddComponent<SetActiveArea>();
        // }
        //
        // [MenuItem("GameObject/Visual Flow/Actions/Check Action Complete", false, -10000)]
        // private static void CheckActionComplete(MenuCommand menuCommand)
        // {
        //     CreateGameObject("CheckActionComplete", menuCommand).AddComponent<CheckActionComplete>();
        // }
        //
        // [MenuItem("GameObject/Visual Flow/Actions/Spine/Mix Skin Additive", false, -10000)]
        // private static void MixSkinAdditive(MenuCommand menuCommand)
        // {
        //     CreateGameObject("MixSpineSkin", menuCommand).AddComponent<MixSkinAdditive>();
        // }
        //
        // [MenuItem("GameObject/Visual Flow/Actions/Spine/Remove Skin", false, -10000)]
        // private static void RemoveSkin(MenuCommand menuCommand)
        // {
        //     CreateGameObject("RemoveSkin", menuCommand).AddComponent<RemoveSkin>();
        // }
        //
        // [MenuItem("GameObject/Visual Flow/Actions/Level Complete", false, -10000)]
        // private static void LevelComplete(MenuCommand menuCommand)
        // {
        //     CreateGameObject("LevelComplete", menuCommand).AddComponent<LevelComplete>();
        // }
        //
        // [MenuItem("GameObject/Visual Flow/Level Root", false, -10000)]
        // private static void LevelRoot(MenuCommand menuCommand)
        // {
        //     GameObject go = CreateGameObject("LevelRoot", menuCommand);
        //     var levelPlayer = go.AddComponent<LevelPlayer>();
        //     var timeline = go.GetComponent<Timeline>();
        //     levelPlayer.GetType().GetField("timeline", BindingFlags.Instance | BindingFlags.NonPublic)
        //         ?.SetValue(levelPlayer, timeline);
        // }
        //
        public static GameObject CreateGameObject(string name, MenuCommand menuCommand)
        {
            var go = new GameObject(name);
            go.transform.position = Vector3.zero;
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(go, go.GetInstanceID().ToString());
            Selection.activeObject = go;
            return go;
        }
        //
        // [MenuItem("CONTEXT/BoxCollider/Fit Collider")]
        // private static void FitBoxCollider(MenuCommand command)
        // {
        //     var boxCollider = (BoxCollider) command.context;
        //     var renderer = boxCollider.GetComponent<Renderer>();
        //     var bounds = renderer.bounds;
        //     boxCollider.size = bounds.size;
        // }
        //
        // [MenuItem("GameObject/Visual Flow/Utils/Fit All Colliders", false, -10000)]
        // private static void FitAllColliders(MenuCommand menuCommand)
        // {
        //     var colliders = GameObject.FindObjectsOfType<BoxCollider>();
        //
        //     foreach (var collider in colliders)
        //     {
        //         var renderer = collider.GetComponent<Renderer>();
        //         var bounds = renderer.bounds;
        //         collider.size = bounds.size;
        //     }
        // }
        //
        // [MenuItem("GameObject/Visual Flow/Actions/Play Audio", false, -10000)]
        // private static void PlayAudio(MenuCommand menuCommand)
        // {
        //     GameObject go = CreateGameObject("PlayAudio", menuCommand);
        //     var playAudio = go.AddComponent<PlayAudio>();
        //     var audioSourceGo = new GameObject("AudioSource");
        //     audioSourceGo.transform.SetParent(go.transform);
        //     audioSourceGo.transform.localPosition = Vector3.zero;
        //     var audioSource = audioSourceGo.AddComponent<AudioSource>();
        //     playAudio.GetType().GetField("audioSource", BindingFlags.Instance | BindingFlags.NonPublic)
        //         ?.SetValue(playAudio, audioSource);
        //     GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        //     Undo.RegisterCreatedObjectUndo(go, go.GetInstanceID().ToString());
        //     Selection.activeObject = go;
        // }
        //
        // [MenuItem("GameObject/Visual Flow/Actions/Spine/Drag Spine Anim", false, -10000)]
        // private static void DragSpineAnim(MenuCommand menuCommand)
        // {
        //     CreateGameObject("DragSpineAnim", menuCommand).AddComponent<DragSpineAnim>();
        //
        //     var go = new GameObject("DragSpineAnim");
        //     go.transform.position = Vector3.zero;
        //     var dragObject = go.AddComponent<DragSpineAnim>();
        //     var boxColliderGo = new GameObject("BoxCollider");
        //     boxColliderGo.transform.SetParent(go.transform);
        //     boxColliderGo.transform.localPosition = Vector3.zero;
        //     var boxCol = boxColliderGo.AddComponent<BoxCollider>();
        //     boxCol.isTrigger = true;
        //     var selectable = boxColliderGo.AddComponent<LeanSelectable>();
        //     GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        //     Undo.RegisterCreatedObjectUndo(go, go.GetInstanceID().ToString());
        //     Selection.activeObject = go;
        // }
        //
        // [MenuItem("GameObject/Visual Flow/Actions/Mechanics/Set Active Game Object", false, -10000)]
        // private static void SetActiveGameObject(MenuCommand menuCommand)
        // {
        //     CreateGameObject("SetActiveGameObject", menuCommand).AddComponent<SetActiveGameObject>();
        // }
        //
        // [MenuItem("GameObject/Visual Flow/Actions/Touch/Touch Object", false, -10000)]
        // private static void TouchObject(MenuCommand menuCommand)
        // {
        //     var go = new GameObject("TouchObject");
        //     go.transform.position = Vector3.zero;
        //     var touchObject = go.AddComponent<TouchArea>();
        //     var graphicGo = new GameObject("Graphic");
        //     graphicGo.transform.SetParent(go.transform);
        //     graphicGo.transform.localPosition = Vector3.zero;
        //     var spriteRenderer = graphicGo.AddComponent<SpriteRenderer>();
        //     touchObject.GetType().GetField("spriteRenderer", BindingFlags.Instance | BindingFlags.NonPublic)
        //         ?.SetValue(touchObject, spriteRenderer);
        //     var boxCol = graphicGo.AddComponent<BoxCollider>();
        //     boxCol.isTrigger = true;
        //     GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        //     Undo.RegisterCreatedObjectUndo(go, go.GetInstanceID().ToString());
        //     Selection.activeObject = go;
        // }
        //
        // [MenuItem("GameObject/Visual Flow/Actions/Check Correct Area", false, -10000)]
        // private static void CheckCorrectArea(MenuCommand menuCommand)
        // {
        //     CreateGameObject("CheckCorrectArea", menuCommand).AddComponent<CheckCorrectArea>();
        // }
        //
        // [MenuItem("GameObject/Visual Flow/Actions/Spine/Set Time Scale", false, -10000)]
        // private static void SetTimeScale(MenuCommand menuCommand)
        // {
        //     CreateGameObject("SetTimeScale", menuCommand).AddComponent<SetTimeScale>();
        // }
        //
        // [MenuItem("GameObject/Visual Flow/Actions/WaitAny", false, -10000)]
        // private static void WaitAny(MenuCommand menuCommand)
        // {
        //     CreateGameObject("WaitAny", menuCommand).AddComponent<WaitAny>();
        // }
        //
        // [MenuItem("GameObject/Visual Flow/Actions/Drag/CompositeArea", false, -10000)]
        // private static void CompositeArea(MenuCommand menuCommand)
        // {
        //     CreateGameObject("CompositeArea", menuCommand).AddComponent<CompositeArea>();
        // }
    }
}
#endif