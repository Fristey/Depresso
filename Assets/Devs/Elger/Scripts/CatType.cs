using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cat Types", menuName = "Scriptable Objects/Cat Types")]
public class CatType : ScriptableObject
{
    public List<CatAction> catActions = new List<CatAction>();

    [System.Serializable]
    public class CatAction
    {
        public CalledFunction function;

        [SerializeField] private AnimationCurve minNumCurve;
        [SerializeField] private AnimationCurve maxNumCurve;

        public Vector2 GetWindow(float index)
        {
            return new Vector2(minNumCurve.Evaluate(index),maxNumCurve.Evaluate(index));
        }
    }
}
