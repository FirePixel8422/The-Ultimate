using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SkillBaseSO))]
public sealed class BaseSkillSOEditor : Editor
{
    // --- Property path constants ---
    private static readonly string SkillPropName = nameof(SkillBaseSO.Skill);
    private static readonly string SkillEffectsPropName = nameof(SkillBase.effects);

    private Type[] cachedEffectTypes;
    private Type[] cachedSkillTypes;

    private void OnEnable()
    {
        // Cache all concrete SkillEffectBase types
        cachedEffectTypes = TypeCache.GetTypesDerivedFrom<SkillBaseEffect>()
            .Where(t => !t.IsAbstract && t.IsClass && !t.IsGenericType)
            .ToArray();

        // Cache all concrete SkillBase types
        cachedSkillTypes = TypeCache.GetTypesDerivedFrom<SkillBase>()
            .Where(t => !t.IsAbstract && t.IsClass && !t.IsGenericType)
            .ToArray();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SkillBaseSO so = (SkillBaseSO)target;
        SerializedProperty skillProp = serializedObject.FindProperty(SkillPropName);

        // Draw SkillBase fields inline
        if (so.Skill != null)
        {
            if (skillProp != null)
            {
                EditorGUILayout.PropertyField(skillProp, true); // draw all subclass fields
            }

            GUILayout.Space(8);

            // Add SkillEffect button
            if (GUILayout.Button("Add Skill Effect"))
                ShowAddEffectMenu();
        }
        else
        {
            EditorGUILayout.HelpBox("No Skill assigned. Click 'Set SkillType' to create one.", MessageType.Info);
        }

        GUILayout.Space(8);

        // Set SkillBase type button
        if (GUILayout.Button("Set SkillType"))
            ShowSetSkillBaseMenu();

        serializedObject.ApplyModifiedProperties();
    }

    // --- Skill Effect menu ---
    private void ShowAddEffectMenu()
    {
        GenericMenu menu = new GenericMenu();

        foreach (Type type in cachedEffectTypes)
        {
            Type capturedType = type;
            menu.AddItem(new GUIContent(capturedType.Name), false, () => AddEffect(capturedType));
        }

        menu.ShowAsContext();
    }

    private void AddEffect(Type type)
    {
        SkillBaseSO so = (SkillBaseSO)target;
        if (so.Skill == null)
        {
            Debug.LogWarning("Cannot add SkillEffect: SkillBase is null.");
            return;
        }

        Undo.RecordObject(so, "Add SkillEffect");
        serializedObject.Update();

        SerializedProperty skillProp = serializedObject.FindProperty(SkillPropName);
        SerializedProperty effectsProp = skillProp.FindPropertyRelative(SkillEffectsPropName);

        if (effectsProp == null)
        {
            Debug.LogError("SkillEffects property is null. Make sure SkillBase has [SerializeReference] SkillEffects array or list.");
            return;
        }

        int index = effectsProp.arraySize;
        effectsProp.InsertArrayElementAtIndex(index);

        SerializedProperty element = effectsProp.GetArrayElementAtIndex(index);
        element.managedReferenceValue = Activator.CreateInstance(type);

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(so);
        Repaint();
    }

    // --- SkillBase menu ---
    private void ShowSetSkillBaseMenu()
    {
        GenericMenu menu = new GenericMenu();

        foreach (Type type in cachedSkillTypes)
        {
            Type capturedType = type;
            menu.AddItem(new GUIContent(capturedType.Name), false, () => SetSkillBase(capturedType));
        }

        menu.ShowAsContext();
    }

    private void SetSkillBase(Type type)
    {
        SkillBaseSO so = (SkillBaseSO)target;

        Undo.RecordObject(so, "Set SkillType");

        // Direct assignment works safely for [SerializeReference]
        so.Skill = CreateSkill(type);

        EditorUtility.SetDirty(so);

        serializedObject.Update();
        Repaint();
    }

    private SkillBase CreateSkill(Type type)
    {
        SkillBase skill = (SkillBase)Activator.CreateInstance(type);

        if (skill.effects == null)
            skill.effects = new SkillBaseEffect[0];

        return skill;
    }
}
