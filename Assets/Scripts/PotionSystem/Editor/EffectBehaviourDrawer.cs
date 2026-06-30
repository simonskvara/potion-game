using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

// IMGUI property drawer that gives each [SerializeReference] EffectBehaviour element a type-picker
// dropdown. Drawn per-element, so it works regardless of which inspector renders the parent object
// (NaughtyAttributes' global IMGUI inspector has no built-in managed-reference picker; this adds one).
[CustomPropertyDrawer(typeof(EffectBehaviour), true)]
public class EffectBehaviourDrawer : PropertyDrawer
{
    private static Type[] _concreteTypes;

    private static Type[] ConcreteTypes =>
        _concreteTypes ??= TypeCache.GetTypesDerivedFrom<EffectBehaviour>()
            .Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition)
            .ToArray();

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        Rect line = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        Rect dropdownRect = EditorGUI.PrefixLabel(line, label);

        if (EditorGUI.DropdownButton(dropdownRect, new GUIContent(FriendlyName(property.managedReferenceFullTypename)), FocusType.Keyboard))
        {
            ShowTypeMenu(property, dropdownRect);
        }

        if (!string.IsNullOrEmpty(property.managedReferenceFullTypename))
        {
            EditorGUI.indentLevel++;
            float y = line.yMax + EditorGUIUtility.standardVerticalSpacing;

            SerializedProperty end = property.GetEndProperty();
            SerializedProperty child = property.Copy();
            bool enterChildren = true;
            while (child.NextVisible(enterChildren) && !SerializedProperty.EqualContents(child, end))
            {
                enterChildren = false;
                float h = EditorGUI.GetPropertyHeight(child, true);
                EditorGUI.PropertyField(new Rect(position.x, y, position.width, h), child, true);
                y += h + EditorGUIUtility.standardVerticalSpacing;
            }
            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = EditorGUIUtility.singleLineHeight;
        if (!string.IsNullOrEmpty(property.managedReferenceFullTypename))
        {
            SerializedProperty end = property.GetEndProperty();
            SerializedProperty child = property.Copy();
            bool enterChildren = true;
            while (child.NextVisible(enterChildren) && !SerializedProperty.EqualContents(child, end))
            {
                enterChildren = false;
                height += EditorGUI.GetPropertyHeight(child, true) + EditorGUIUtility.standardVerticalSpacing;
            }
        }
        return height;
    }

    private void ShowTypeMenu(SerializedProperty property, Rect rect)
    {
        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent("(None)"), string.IsNullOrEmpty(property.managedReferenceFullTypename),
            () => AssignType(property, null));

        foreach (Type type in ConcreteTypes)
        {
            menu.AddItem(new GUIContent(ObjectNames.NicifyVariableName(type.Name)), false,
                () => AssignType(property, type));
        }
        menu.DropDown(rect);
    }

    private static void AssignType(SerializedProperty property, Type type)
    {
        property.serializedObject.Update();
        property.managedReferenceValue = type == null ? null : Activator.CreateInstance(type);
        property.serializedObject.ApplyModifiedProperties();
    }

    private static string FriendlyName(string managedReferenceFullTypename)
    {
        if (string.IsNullOrEmpty(managedReferenceFullTypename)) return "(Select type)";

        string[] split = managedReferenceFullTypename.Split(' ');
        string full = split.Length > 1 ? split[1] : split[0];
        int dot = full.LastIndexOf('.');
        return ObjectNames.NicifyVariableName(dot >= 0 ? full.Substring(dot + 1) : full);
    }
}
