using System.Collections;
using System.Collections.Generic;
using UnityEditor;

#if UNITY_EDITOR
using System;
using UnityEngine;
#endif

public class ScalableObject : MonoBehaviour
{

    [SerializeField]
    private bool isScalable = true;
    
    [HideInInspector] public bool overrideLimits = false;
    [HideInInspector] public bool overrideScaleAxis = false;
    
        
    private float minimumScale = 0.25f;
    private float maximumScale = 2.0f;

    private Vector3 scaleFactor = new Vector3(1, 1, 1);

    public bool canScale()
    {
        return isScalable;
    }

    public Vector3 getScaledAxis()
    {
        return scaleFactor;
    }

    public float getMinimumScale()
    {
        return minimumScale;
    }
    
    public float getMaximumScale()
    {
        return maximumScale;
    }


    #region Editor
#if UNITY_EDITOR
    [CustomEditor(typeof(ScalableObject))]
    public class ScalableObjectEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ScalableObject scalableObject = (ScalableObject) target;
            
            EditorGUILayout.Separator();
            scalableObject.overrideScaleAxis = EditorGUILayout.Toggle("Override Axis Scaling", scalableObject.overrideScaleAxis);
            if (scalableObject.overrideScaleAxis)
            {
                GUILayout.ExpandWidth(false);
                scalableObject.scaleFactor.x = Convert.ToSingle(EditorGUILayout.Toggle("Scale X?", Convert.ToBoolean(scalableObject.scaleFactor.x)));
                scalableObject.scaleFactor.y = Convert.ToSingle(EditorGUILayout.Toggle("Scale Y?", Convert.ToBoolean(scalableObject.scaleFactor.y)));
                scalableObject.scaleFactor.z = Convert.ToSingle(EditorGUILayout.Toggle("Scale Z?", Convert.ToBoolean(scalableObject.scaleFactor.z)));
                EditorGUILayout.Separator();
            }

            scalableObject.overrideLimits = EditorGUILayout.Toggle("Override Limits", scalableObject.overrideLimits);
            if (scalableObject.overrideLimits)
            {
                scalableObject.minimumScale = EditorGUILayout.FloatField("Minimum Scale", scalableObject.minimumScale);
                scalableObject.maximumScale = EditorGUILayout.FloatField("Maximum Scale", scalableObject.maximumScale);
            }
        }
    }
#endif
    #endregion
}