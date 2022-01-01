using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; //for CustomEditor

[CustomEditor(typeof(CompositeBehaviour))]
public class CompositeBehaviourEditor : Editor //inherit from Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        //initial inspector setup

        CompositeBehaviour cb = (CompositeBehaviour)target;

        GUILayout.Space(10f);
        // Buttons to add and remove behaviours in our containers
        EditorGUILayout.BeginVertical();
        if (GUILayout.Button("Add Behaviour"))
        {
            AddBehaviour(cb);
            EditorUtility.SetDirty(cb);
        }
        if (GUILayout.Button("Remove Behaviour"))
        {
            RemoveBehaviour(cb);
            EditorUtility.SetDirty(cb);
        }
        EditorGUILayout.EndVertical();

        // Check for behaviours
        if (cb.behaviours == null || cb.behaviours.Length == 0)
        {
            EditorGUILayout.HelpBox("No behaviours in array.", MessageType.Warning);
        }
        else
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20f);
            //EditorGUILayout.LabelField("Number", GUILayout.MinWidth(60f), GUILayout.MaxWidth(60f));
            EditorGUILayout.LabelField("Behaviors", GUILayout.MinWidth(60f));
            EditorGUILayout.LabelField("Weights", GUILayout.MinWidth(60f), GUILayout.MaxWidth(60f));
            EditorGUILayout.EndHorizontal();

            EditorGUI.BeginChangeCheck();
            for (int i = 0; i < cb.behaviours.Length; ++i)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(i.ToString(), GUILayout.MaxWidth(15f));
                cb.behaviours[i] = (FlockBehaviour)EditorGUILayout.ObjectField(cb.behaviours[i], typeof(FlockBehaviour), false, GUILayout.MinWidth(60f));
                cb.weights[i] = EditorGUILayout.FloatField(cb.weights[i], GUILayout.MinWidth(60f), GUILayout.MaxWidth(60f));
                EditorGUILayout.EndHorizontal();
            }
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(cb);
            }
        }

        /*CompositeBehaviour cb = (CompositeBehaviour)target;

        Rect r = EditorGUILayout.BeginHorizontal();
        r.height = EditorGUIUtility.singleLineHeight;

        //check for behaviours whether they don't exist or if they do exist but it's an empty array
        if(cb.behaviours == null || cb.behaviours.Length == 0)
        {
            EditorGUILayout.HelpBox("No behaviours in array.", MessageType.Warning);
            EditorGUILayout.EndHorizontal();
            r = EditorGUILayout.BeginHorizontal();
            r.height = EditorGUIUtility.singleLineHeight;
        }
        else
        {
            r.x = 30f; //just some padding on the left
            r.width = EditorGUIUtility.currentViewWidth - 95f; //width minus 95 pixels
            EditorGUI.LabelField(r, "Behaviours");
            r.x = EditorGUIUtility.currentViewWidth - 65f;

            r.width = 60f; //about 5 pixels of padding/margin on right side
            EditorGUI.LabelField(r, "Weights");
            r.y += EditorGUIUtility.singleLineHeight * 1.2f;

            EditorGUI.BeginChangeCheck();
            for(int i = 0; i < cb.behaviours.Length; i++)
            {
                r.x = 5f;
                r.width = 20f;
                EditorGUI.LabelField(r, i.ToString());
                r.x = 30f;
                r.width = EditorGUIUtility.currentViewWidth - 95f;
                cb.behaviours[i] = (FlockBehaviour)EditorGUI.ObjectField(r, cb.behaviours[i], typeof(FlockBehaviour), false);

                r.x = EditorGUIUtility.currentViewWidth - 65f;
                r.width = 60f;
                cb.weights[i] = EditorGUI.FloatField(r, cb.weights[i]);

                r.y += EditorGUIUtility.singleLineHeight * 1.1f;
            }
            if(EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(cb);
            }

            
        }
        EditorGUILayout.EndHorizontal();
        r.x = 5f;
        r.width = EditorGUIUtility.currentViewWidth - 10f;
        r.y += EditorGUIUtility.singleLineHeight * 0.5f;
        if (GUI.Button(r, "Add Behaviour"))
        {
            //add behaviour
            AddBehaviour(cb);
            EditorUtility.SetDirty(cb);
        }

        r.y += EditorGUIUtility.singleLineHeight * 1.5f;
        if (cb.behaviours != null && cb.behaviours.Length > 0)
        {
            if (GUI.Button(r, "Remove Behaviour"))
            {
                //remove behaviour
                RemoveBehaviour(cb);
                EditorUtility.SetDirty(cb);
            }
        }*/
    }

    void AddBehaviour(CompositeBehaviour cb)
    {
        int oldCount = (cb.behaviours != null) ? cb.behaviours.Length : 0;
        FlockBehaviour[] newBehaviours = new FlockBehaviour[oldCount + 1];
        float[] newWeights = new float[oldCount + 1];

        for(int i = 0; i < oldCount; i++)
        {
            newBehaviours[i] = cb.behaviours[i];
            newWeights[i] = cb.weights[i];
        }
        newWeights[oldCount] = 1f;
        cb.behaviours = newBehaviours;
        cb.weights = newWeights;
    }

    void RemoveBehaviour(CompositeBehaviour cb)
    {
        int oldCount = cb.behaviours.Length;
        if(oldCount == 1)
        {
            cb.behaviours = null;
            cb.weights = null;
            return;
        }
        FlockBehaviour[] newBehaviours = new FlockBehaviour[oldCount - 1];
        float[] newWeights = new float[oldCount - 1];

        for (int i = 0; i < oldCount - 1; i++)
        {
            newBehaviours[i] = cb.behaviours[i];
            newWeights[i] = cb.weights[i];
        }
        cb.behaviours = newBehaviours;
        cb.weights = newWeights;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
