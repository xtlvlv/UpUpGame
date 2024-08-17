using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class SkillEditorWindow : EditorWindow
{
    private List<Skill> skills = new List<Skill>();
    private Vector2 scrollPos;
    private Skill selectedSkill;

    [MenuItem("Tools/Skill Editor")]
    public static void ShowWindow()
    {
        GetWindow<SkillEditorWindow>("Skill Editor");
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();

        // 左侧的技能列表
        DrawSidebar();

        // 右侧的技能编辑区域
        DrawSkillEditor();

        EditorGUILayout.EndHorizontal();
    }

    private void DrawSidebar()
    {
        EditorGUILayout.BeginVertical(GUILayout.Width(200));

        EditorGUILayout.LabelField("Skills", EditorStyles.boldLabel);

        if (GUILayout.Button("Add New"))
        {
            Skill newSkill = new Skill { Name = "DefaultNew" };
            skills.Add(newSkill);
            selectedSkill = newSkill;
        }

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.ExpandHeight(true));

        foreach (var skill in skills)
        {
            if (GUILayout.Button(skill.Name))
            {
                SaveSkills();
                selectedSkill = skill;
            }
        }

        EditorGUILayout.EndScrollView();

        EditorGUILayout.EndVertical();
    }

    private void DrawSkillEditor()
    {
        if (selectedSkill == null)
        {
            EditorGUILayout.LabelField("Select or create a skill to edit.");
            return;
        }

        EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(true));

        EditorGUILayout.LabelField("Skill Editor", EditorStyles.boldLabel);

        selectedSkill.Name = EditorGUILayout.TextField("Name", selectedSkill.Name);
        selectedSkill.Cooldown = EditorGUILayout.FloatField("Cooldown", selectedSkill.Cooldown);
        selectedSkill.Damage = EditorGUILayout.IntField("Damage", selectedSkill.Damage);
        selectedSkill.Description = EditorGUILayout.TextField("Description", selectedSkill.Description);
        selectedSkill.Animation = (AnimationClip)EditorGUILayout.ObjectField("Animation", selectedSkill.Animation, typeof(AnimationClip), false);
        selectedSkill.Icon = (Sprite)EditorGUILayout.ObjectField("Icon", selectedSkill.Icon, typeof(Sprite), false);

        if (GUILayout.Button("Save Skill"))
        {
            SaveSkills();
        }

        if (GUILayout.Button("Delete Skill"))
        {
            skills.Remove(selectedSkill);
            selectedSkill = null;
        }

        EditorGUILayout.EndVertical();
    }

    private void SaveSkills()
    {
        GUI.FocusControl(null);

        // 可以序列化到JSON文件或SO
        Debug.Log("Skills saved.");
    }

    private void LoadSkills()
    {
        Debug.Log("Skills loaded.");
    }
}


[System.Serializable]
public class Skill
{
    public string        Name;
    public float         Cooldown;
    public int           Damage;
    public string        Description;
    public AnimationClip Animation;
    public Sprite        Icon;
}
