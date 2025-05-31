using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Copying
{
    public static class Copy
    {
        /// <summary>
        /// replaces an object from the scene with a prefab taking some components
        /// </summary>
        /// <param name="from"> the prefab</param>
        /// <param name="to"> the scene object</param>
        public static void CopyingComponents(GameObject from, GameObject to)
        {
            if (from == null || to == null) return;

            for (int i = to.transform.childCount - 1; i >= 0; i--)
            {
                Object.DestroyImmediate(to.transform.GetChild(i).gameObject);
            }

            CopyComponentsRecursive(from.transform, to.transform, true);

            Vector3 oldPos = to.transform.position;
            Quaternion oldRot = to.transform.rotation;
            Vector3 oldScale = to.transform.lossyScale;

            to.transform.SetParent(from.transform.parent);

            to.transform.position = oldPos;
            to.transform.rotation = oldRot;

            Vector3 parentScale = to.transform.parent ? to.transform.parent.lossyScale : Vector3.one;
            to.transform.localScale = new Vector3(
                oldScale.x / parentScale.x,
                oldScale.y / parentScale.y,
                oldScale.z / parentScale.z
            );

            to.transform.SetSiblingIndex(from.transform.GetSiblingIndex());
            to.name = from.name;

            Collider colTo = to.GetComponent<Collider>();
            if (colTo != null)
                Object.DestroyImmediate(colTo);

            Collider colFrom = from.GetComponent<Collider>();
            if (colFrom != null)
            {
                var newCol = to.AddComponent(colFrom.GetType()) as Collider;
                if (newCol != null)
                {
                    ComponentUtility.CopyComponent(colFrom);
                    ComponentUtility.PasteComponentValues(newCol);
                }
            }

            if (!AssetDatabase.Contains(from))
                Object.DestroyImmediate(from, true);
        }

        /// <summary>
        /// handels the object if it had chid objects and copies those too
        /// </summary>
        /// <param name="from">the prefab</param>
        /// <param name="to">the scene object</param>
        /// <param name="isRoot">scene object</param>
        private static void CopyComponentsRecursive(Transform from, Transform to, bool isRoot = false)
        {
            CopySingleObjectComponents(from.gameObject, to.gameObject, isRoot);

            for (int i = 0; i < from.childCount; i++)
            {
                Transform fromChild = from.GetChild(i);

                Transform toChild;
                if (i < to.childCount)
                {
                    toChild = to.GetChild(i);
                }
                else
                {
                    GameObject newChild = new GameObject(fromChild.name);
                    toChild = newChild.transform;
                    toChild.SetParent(to);
                }

                toChild.name = fromChild.name;
                toChild.SetSiblingIndex(fromChild.GetSiblingIndex());
                toChild.localPosition = fromChild.localPosition;
                toChild.localRotation = fromChild.localRotation;
                toChild.localScale = fromChild.localScale;

                CopyComponentsRecursive(fromChild, toChild, false);
            }
        }

        /// <summary>
        /// handels the components from the objects "from" and "to"
        /// </summary>
        /// <param name="from">the prefab</param>
        /// <param name="to">the scene object</param>
        /// <param name="isRoot">checks if it is the scene object</param>
        private static void CopySingleObjectComponents(GameObject from, GameObject to, bool isRoot)
        {
            if (from.GetComponent<MeshFilter>() == null && to.GetComponent<MeshFilter>() != null)
                Object.DestroyImmediate(to.GetComponent<MeshFilter>());

            if (from.GetComponent<MeshRenderer>() == null && to.GetComponent<MeshRenderer>() != null)
                Object.DestroyImmediate(to.GetComponent<MeshRenderer>());

            Component[] sourceComponents = from.GetComponents<Component>();

            foreach (var sourceComp in sourceComponents)
            {
                if (sourceComp == null || sourceComp is Transform) continue;

                if (isRoot && (sourceComp is Collider))
                    continue;

                if (sourceComp is Rigidbody)
                    continue;

                if (sourceComp is MonoBehaviour script)
                {
                    if (PrefabUtility.IsPartOfPrefabAsset(script) || PrefabUtility.IsPartOfPrefabInstance(script))
                        continue;
                }

                var type = sourceComp.GetType();
                var destComp = to.GetComponent(type);
                if (!destComp)
                    destComp = to.AddComponent(type);

                if (destComp)
                {
                    ComponentUtility.CopyComponent(sourceComp);
                    ComponentUtility.PasteComponentValues(destComp);
                }
            }
        }
    }
}
