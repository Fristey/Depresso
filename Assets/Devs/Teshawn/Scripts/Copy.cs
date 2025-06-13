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
                Object.Destroy(to.transform.GetChild(i).gameObject);
            }

            CopyComponentsRecursive(from.transform, to.transform, true);

            Vector3 oldPos = to.transform.position;
            Quaternion oldRot = to.transform.rotation;
            Vector3 oldScale = to.transform.lossyScale;

            to.transform.SetParent(from.transform.parent);
            to.transform.position = oldPos;
            to.transform.rotation = oldRot;

            Vector3 parentScale = to.transform.parent ? to.transform.parent.lossyScale : Vector3.one;
            to.transform.localScale = new Vector3(oldScale.x / parentScale.x, oldScale.y / parentScale.y, oldScale.z / parentScale.z);

            to.transform.SetSiblingIndex(from.transform.GetSiblingIndex());
            to.name = from.name;

            Collider colFrom = from.GetComponent<Collider>();
            Collider colTo = to.GetComponent<Collider>();

            if (colFrom != null)
            {
                if (colTo == null)
                {
                    colTo = to.AddComponent(colFrom.GetType()) as Collider;
                }
                else if (colTo.GetType() != colFrom.GetType())
                {
                    Object.Destroy(colTo);
                    colTo = to.AddComponent(colFrom.GetType()) as Collider;
                }

                if (colTo != null)
                {
                    ComponentUtility.CopyComponent(colFrom);
                    ComponentUtility.PasteComponentValues(colTo);
                }
            }

            if (!AssetDatabase.Contains(from))
            {
                Object.DestroyImmediate(from, true);
            }
        }

        /// <summary>
        /// handels the object if it had chid objects and copies those too
        /// </summary
        /// <param name="from">the prefab</param>
        /// <param name="to">the scene object</param>
        /// <param name="isRoot">scene object</param>
        private static void CopyComponentsRecursive(Transform from, Transform to, bool isRoot = false)
        {
            CopySingleObjectComponents(from.gameObject, to.gameObject, isRoot);

            for (int i = to.childCount - 1; i >= 0; i--)
            {
                Object.Destroy(to.GetChild(i).gameObject);
            }

            for (int i = 0; i < from.childCount; i++)
            {
                Transform fromChild = from.GetChild(i);

                GameObject newChild = new GameObject(fromChild.name);
                Transform toChild = newChild.transform;
                toChild.SetParent(to);

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
            Component[] sourceComponents = from.GetComponents<Component>();

            foreach (var sourceComp in sourceComponents)
            {
                if (sourceComp == null || sourceComp is Transform) continue;

                var type = sourceComp.GetType();
                Component destComp = to.GetComponent(type);
                if (!destComp)
                    destComp = to.AddComponent(type);

                if (destComp)
                {
                    ComponentUtility.CopyComponent(sourceComp);
                    ComponentUtility.PasteComponentValues(destComp);

                    if (sourceComp is MeshFilter srcMF && destComp is MeshFilter dstMF)
                        dstMF.sharedMesh = srcMF.sharedMesh;
                }
            }

            Component[] destComponents = to.GetComponents<Component>();
            foreach (var destComp in destComponents)
            {
                if (destComp == null || destComp is Transform) continue;

                var destType = destComp.GetType();

                bool typeFoundInSource = false;
                foreach (var sourceComp in sourceComponents)
                {
                    if (sourceComp != null && sourceComp.GetType() == destType)
                    {
                        typeFoundInSource = true;
                        break;
                    }
                }

                if (!typeFoundInSource && !(destComp is Rigidbody) && !(destComp is Collider) && !(destComp is MonoBehaviour))
                    Object.Destroy(destComp);
            }
        }
    }
}
