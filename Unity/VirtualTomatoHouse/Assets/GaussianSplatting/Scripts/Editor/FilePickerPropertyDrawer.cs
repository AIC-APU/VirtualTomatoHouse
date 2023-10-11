using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental;
using System.IO;
using System.Linq;

[CustomPropertyDrawer(typeof(FilePickerAttribute))]
public class FilePickerPropertyDrawer : PropertyDrawer
{
    const string kLastPathPref = "nesnausk.utils.FilePickerLastPath";
    static Texture2D s_Icon => EditorGUIUtility.FindTexture(EditorResources.folderIconName);
    static GUIStyle s_StyleTextFieldText;
    static GUIStyle s_StyleTextFieldDropdown;
    static readonly int kPathFieldControlID = "FilePickerPathField".GetHashCode();
    const int kIconSize = 15;
    const int kRecentPathsCount = 10;

    public static string PathToDisplayString(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return "<none>";
        path = path.Replace('\\', '/');
        string[] parts = path.Split('/');
        if (parts.Length >= 4)
            path = string.Join('/', parts.TakeLast(4));
        path = path.Replace('/', '-');
        return path;
    }

    class PreviousPaths
    {
        public PreviousPaths(List<string> paths)
        {
            this.paths = paths;
            UpdateContent();
        }
        public void UpdateContent()
        {
            this.content = paths.Select(p => new GUIContent(PathToDisplayString(p))).ToArray();
        }
        public List<string> paths;
        public GUIContent[] content;
    }
    Dictionary<string, PreviousPaths> m_PreviousPaths = new();

    void PopulatePreviousPaths(string nameKey)
    {
        if (m_PreviousPaths.ContainsKey(nameKey))
            return;

        List<string> prevPaths = new();
        for (int i = 0; i < kRecentPathsCount; ++i)
        {
            string path = EditorPrefs.GetString($"{kLastPathPref}-{nameKey}-{i}");
            if (!string.IsNullOrWhiteSpace(path))
                prevPaths.Add(path);
        }
        m_PreviousPaths.Add(nameKey, new PreviousPaths(prevPaths));
    }

    void UpdatePreviousPaths(string nameKey, string path)
    {
        if (!m_PreviousPaths.ContainsKey(nameKey))
        {
            m_PreviousPaths.Add(nameKey, new PreviousPaths(new List<string>()));
        }
        var prevPaths = m_PreviousPaths[nameKey];
        prevPaths.paths.Remove(path);
        prevPaths.paths.Insert(0, path);
        while (prevPaths.paths.Count > kRecentPathsCount)
            prevPaths.paths.RemoveAt(prevPaths.paths.Count - 1);
        prevPaths.UpdateContent();

        for (int i = 0; i < prevPaths.paths.Count; ++i)
        {
            EditorPrefs.SetString($"{kLastPathPref}-{nameKey}-{i}", prevPaths.paths[i]);
        }
    }

    static bool CheckPath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return false;
        if (!File.Exists(path))
            return false;
        return true;
    }

    static string PathAbsToStorage(string path)
    {
        path = path.Replace('\\', '/');
        var dataPath = Application.dataPath;
        if (path.StartsWith(dataPath, StringComparison.Ordinal))
        {
            path = Path.GetRelativePath($"{dataPath}/..", path);
            path = path.Replace('\\', '/');
        }
        return path;
    }

    bool CheckAndSetNewPath(ref string path, string nameKey)
    {
        path = PathAbsToStorage(path);
        if (CheckPath(path))
        {
            EditorPrefs.SetString($"{kLastPathPref}-{nameKey}", path);
            UpdatePreviousPaths(nameKey, path);
            GUI.changed = true;
            Event.current.Use();
            return true;
        }
        return false;
    }

    string PreviousPathsDropdown(Rect position, string value, string nameKey)
    {
        PopulatePreviousPaths(nameKey);
        
        if (string.IsNullOrWhiteSpace(value))
            value = EditorPrefs.GetString($"{kLastPathPref}-{nameKey}");

        m_PreviousPaths.TryGetValue(nameKey, out var prevPaths);

        EditorGUI.BeginDisabledGroup(prevPaths == null || prevPaths.paths.Count == 0);
        EditorGUI.BeginChangeCheck();
        int oldIndent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        int parameterIndex = EditorGUI.Popup(position, GUIContent.none, -1, prevPaths.content, s_StyleTextFieldDropdown);
        if (EditorGUI.EndChangeCheck() && parameterIndex < prevPaths.paths.Count)
        {
            string newValue = prevPaths.paths[parameterIndex];
            if (CheckAndSetNewPath(ref newValue, nameKey))
                value = newValue;
        }
        EditorGUI.indentLevel = oldIndent;
        EditorGUI.EndDisabledGroup();
        return value;
    }

    public string PathFieldGUI(Rect position, GUIContent label, string value, string extension, string nameKey)
    {
        if (s_StyleTextFieldText == null) s_StyleTextFieldText = new GUIStyle("TextFieldDropDownText");
        if (s_StyleTextFieldDropdown == null) s_StyleTextFieldDropdown = new GUIStyle("TextFieldDropdown");

        int controlId = GUIUtility.GetControlID(kPathFieldControlID, FocusType.Keyboard, position);
        Rect fullRect = EditorGUI.PrefixLabel(position, controlId, label);
        Rect textRect = new Rect(fullRect.x, fullRect.y, fullRect.width - s_StyleTextFieldDropdown.fixedWidth, fullRect.height);
        Rect dropdownRect = new Rect(textRect.xMax, fullRect.y, s_StyleTextFieldDropdown.fixedWidth, fullRect.height);
        Rect iconRect = new Rect(textRect.xMax - kIconSize, textRect.y, kIconSize, textRect.height);

        value = PreviousPathsDropdown(dropdownRect, value, nameKey);

        string displayText = PathToDisplayString(value);

        Event evt = Event.current;
        switch (evt.type)
        {
            case EventType.KeyDown:
                if (GUIUtility.keyboardControl == controlId)
                {
                    if (evt.keyCode is KeyCode.Backspace or KeyCode.Delete)
                    {
                        value = null;
                        EditorPrefs.SetString($"{kLastPathPref}-{nameKey}", "");
                        GUI.changed = true;
                        evt.Use();
                    }
                }
                break;
            case EventType.Repaint:
                s_StyleTextFieldText.Draw(textRect, new GUIContent(displayText), controlId, DragAndDrop.activeControlID == controlId);
                //s_StyleTextFieldDropdown.Draw(dropdownRect, GUIContent.none, controlId, DragAndDrop.activeControlID == controlId);
                GUI.DrawTexture(iconRect, s_Icon, ScaleMode.ScaleToFit);
                break;
            case EventType.MouseDown:
                if (evt.button != 0 || !GUI.enabled)
                    break;

                if (textRect.Contains(evt.mousePosition))
                {
                    if (iconRect.Contains(evt.mousePosition))
                    {
                        if (string.IsNullOrWhiteSpace(value))
                            value = EditorPrefs.GetString($"{kLastPathPref}-{nameKey}");
                        string openToPath = string.Empty;
                        if (File.Exists(value))
                            openToPath = Path.GetDirectoryName(value);
                        string newPath = EditorUtility.OpenFilePanel("Select path", openToPath, extension);
                        if (CheckAndSetNewPath(ref newPath, nameKey))
                        {
                            value = newPath;
                            GUI.changed = true;
                            evt.Use();
                        }
                    }
                    else if (File.Exists(value))
                    {
                        EditorUtility.RevealInFinder(value);
                    }
                    GUIUtility.keyboardControl = controlId;
                }
                break;
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (textRect.Contains(evt.mousePosition) && GUI.enabled)
                {
                    if (DragAndDrop.paths.Length > 0)
                    {
                        DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
                        string path = DragAndDrop.paths[0];
                        path = PathAbsToStorage(path);
                        if (CheckPath(path))
                        {
                            if (evt.type == EventType.DragPerform)
                            {
                                UpdatePreviousPaths(nameKey, path);
                                value = path;
                                GUI.changed = true;
                                DragAndDrop.AcceptDrag();
                                DragAndDrop.activeControlID = 0;
                            }
                            else
                                DragAndDrop.activeControlID = controlId;
                        }
                        else
                            DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
                        evt.Use();
                    }
                }
                break;
            case EventType.DragExited:
                if (GUI.enabled)
                {
                    HandleUtility.Repaint();
                }
                break;
        }
        return value;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var attr = (FilePickerAttribute) attribute;
        string newAsset = PathFieldGUI(position, label, property.stringValue, attr.extension, attr.nameKey);
        if (GUI.changed)
        {
            property.stringValue = newAsset;
        }
    }
}
