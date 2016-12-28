using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SpriteJoint))] 
public class SpriteJointEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SpriteJoint joint = target as SpriteJoint;
        EditorGUILayout.LabelField("--------- 精灵挂接点：----------");
        //joint.jointArray = new Transform[EditorGUILayout.IntField("精灵挂接点：", (int)SpriteJoint.JointType.Length)];
        for (int i = 0; i < joint.jointArray.Length; ++i)
        {
            string desc;
            switch ((SpriteJoint.JointType)i)
            {
                case SpriteJoint.JointType.Default: desc = "原点"; break;
                case SpriteJoint.JointType.Head: desc = "头部"; break;
                case SpriteJoint.JointType.Chest: desc = "胸部"; break;
                case SpriteJoint.JointType.Side: desc = "腰部"; break;
                case SpriteJoint.JointType.LeftHand: desc = "左手心"; break;
                case SpriteJoint.JointType.RightHand: desc = "右手心"; break;
                case SpriteJoint.JointType.LeftWeapon: desc = "左武器攻击点"; break;
                case SpriteJoint.JointType.RightWeapon: desc = "右武器攻击点"; break;
                default: desc = ((SpriteJoint.JointType)i).ToString(); break;

            }
            joint.jointArray[i] = EditorGUILayout.ObjectField(desc, joint.jointArray[i], typeof(Transform),true) as Transform;
        }
    }
}
