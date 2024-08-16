using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class NotePadWindow : EditorWindow
{
    private Dictionary<string, string> notes = new Dictionary<string, string>();
    private string selectedNote = "";
    private string newNoteName = "New Note";
    private Vector2 scrollPos;
    private Vector2 sidebarScrollPos;
    private const string noteDirectory = "Editor/Note/Notes";

    [MenuItem("Tools/NotePad")]
    public static void ShowWindow()
    {
        GetWindow<NotePadWindow>("NotePad");
    }

    private void OnEnable()
    {
        LoadAllNotes();
    }

    private void OnDisable()
    {
        SaveNote();
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();

        // Sidebar for selecting notes
        DrawSidebar();

        // Main area for editing notes
        DrawNoteEditor();

        EditorGUILayout.EndHorizontal();
    }

    private void DrawSidebar()
    {
        EditorGUILayout.BeginVertical(GUILayout.Width(200));

        EditorGUILayout.LabelField("Notes", EditorStyles.boldLabel);
        sidebarScrollPos = EditorGUILayout.BeginScrollView(sidebarScrollPos, GUILayout.ExpandHeight(true));

        foreach (var note in notes)
        {
            if (GUILayout.Button(note.Key))
            {
                SaveNote();
                selectedNote = note.Key;
                Repaint(); // 强制刷新窗口
            }
        }

        EditorGUILayout.EndScrollView();

        newNoteName = EditorGUILayout.TextField("New Name", newNoteName);

        if (GUILayout.Button("Add New"))
        {
            if (!notes.ContainsKey(newNoteName))
            {
                notes.Add(newNoteName, "");
                selectedNote = newNoteName;
                SaveNote();
                Repaint(); // 强制刷新窗口
            }
            else
            {
                Debug.LogWarning("Note with this name already exists!");
            }
        }

        EditorGUILayout.EndVertical();
    }

    private void DrawNoteEditor()
    {
        EditorGUILayout.BeginVertical();

        if (!string.IsNullOrEmpty(selectedNote))
        {
            EditorGUILayout.LabelField(selectedNote, EditorStyles.boldLabel);

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            if (notes.ContainsKey(selectedNote))
            {
                notes[selectedNote] = EditorGUILayout.TextArea(notes[selectedNote], GUILayout.ExpandHeight(true));
            }
            EditorGUILayout.EndScrollView();

            if (GUILayout.Button("Save Note"))
            {
                SaveNote();
            }

            if (GUILayout.Button("Delete Note"))
            {
                DeleteNote();
            }
        }
        else
        {
            EditorGUILayout.LabelField("Select or create a note to edit.");
        }

        EditorGUILayout.EndVertical();
    }

    private void LoadAllNotes()
    {
        notes.Clear();

        if (!Directory.Exists(Path.Combine(Application.dataPath, noteDirectory)))
        {
            Directory.CreateDirectory(Path.Combine(Application.dataPath, noteDirectory));
        }

        string[] files = Directory.GetFiles(Path.Combine(Application.dataPath, noteDirectory), "*.txt");

        foreach (var file in files)
        {
            string noteName = Path.GetFileNameWithoutExtension(file);
            string noteContent = File.ReadAllText(file);
            notes.Add(noteName, noteContent);
        }

        if (notes.Count > 0)
        {
            selectedNote = notes.Keys.GetEnumerator().Current;
        }
    }

    private void SaveNote()
    {
        GUI.FocusControl(null);
        if (!string.IsNullOrEmpty(selectedNote) && notes.ContainsKey(selectedNote))
        {
            string filePath = Path.Combine(Application.dataPath, noteDirectory, selectedNote + ".txt");
            File.WriteAllText(filePath, notes[selectedNote]);
            Debug.Log("Note saved: " + selectedNote);
        }
    }

    private void DeleteNote()
    {
        if (!string.IsNullOrEmpty(selectedNote) && notes.ContainsKey(selectedNote))
        {
            string filePath = Path.Combine(Application.dataPath, noteDirectory, selectedNote + ".txt");

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Debug.Log("Note deleted: " + selectedNote);
            }

            notes.Remove(selectedNote);
            selectedNote = notes.Count > 0 ? notes.Keys.GetEnumerator().Current : "";
        }
    }
}