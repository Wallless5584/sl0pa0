using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TrapEditorController))]
public class TrapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TrapEditorController targetTrap = (TrapEditorController)target;

        if (targetTrap.transform.parent != null)
        {
            // 1st block
            GUILayout.Label(text: "Rotation:");
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("-90°"))
            {
                targetTrap.Rotate(-90);
            }

            GUI.enabled = targetTrap.CheckIsRotated();
            if (GUILayout.Button("Reset Rotation"))
            {
                targetTrap.ResetRotation();
            }
            GUI.enabled = true;

            if (GUILayout.Button("+90°"))
            {
                targetTrap.Rotate(90);
            }

            GUILayout.EndHorizontal();



            // 2nd block
            GUILayout.Label(text: "Position:");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(Symbols.ArrowUpLeft))
            {
                targetTrap.MoveByHalfGrid(Vector2.up + Vector2.left);
            }
            if (GUILayout.Button(Symbols.ArrowUp))
            {
                targetTrap.MoveByHalfGrid(Vector2.up);
            }
            if (GUILayout.Button(Symbols.ArrowUpRight))
            {
                targetTrap.MoveByHalfGrid(Vector2.up + Vector2.right);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button(Symbols.ArrowLeft))
            {
                targetTrap.MoveByHalfGrid(Vector2.left);
            }
            GUI.enabled = !targetTrap.CheckIsAllignedToGrid();
            if (GUILayout.Button("Allign to Grid"))
            {
                targetTrap.AllignToGrid();
            }
            GUI.enabled = true;
            if (GUILayout.Button(Symbols.ArrowRight))
            {
                targetTrap.MoveByHalfGrid(Vector2.right);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button(Symbols.ArrowDownLeft))
            {
                targetTrap.MoveByHalfGrid(Vector2.down + Vector2.left);
            }
            if (GUILayout.Button(Symbols.ArrowDown))
            {
                targetTrap.MoveByHalfGrid(Vector2.down);
            }
            if (GUILayout.Button(Symbols.ArrowDownRight))
            {
                targetTrap.MoveByHalfGrid(Vector2.down + Vector2.right);
            }
            GUILayout.EndHorizontal();




            // 3nd row
            if (targetTrap.CheckForPair())
            {
                GUILayout.Label(text: "Pair controls:");
                GUILayout.BeginHorizontal();

                GUI.enabled = !targetTrap.CheckIfInDefaultPosition();
                if (GUILayout.Button("Reset Pair Position"))
                {
                    targetTrap.SetPairToDefaultPosition();
                }
                GUI.enabled = true;

                if (targetTrap.CheckIfInDefaultPosition())
                {
                    if (GUILayout.Button("Position Pair"))
                    {
                        targetTrap.SetPairBasePosition();
                    }
                }
                else
                {
                    if (GUILayout.Button("Reposition Pair"))
                    {
                        targetTrap.SetPairBasePosition();
                    }
                }

                GUILayout.EndHorizontal();
            }
        }
        else
        {
            GUI.enabled = false;
            if (GUILayout.Button("\nControls hidden in Prefab mode\n"))
            {
                
            }
            GUI.enabled = true;
        }

    }
}
