using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(tk2dSpriteFromTexture))]
class tk2dSpriteFromTextureEditor : Editor {

	void SpriteCollectionSizeField(tk2dSpriteCollectionSize spriteCollectionSize) {
		GUIContent sc = new GUIContent("Sprite Collection Size", null, "The resolution this sprite will be pixel perfect at");
		spriteCollectionSize.type = (tk2dSpriteCollectionSize.Type)EditorGUILayout.EnumPopup(sc, spriteCollectionSize.type);
		if (spriteCollectionSize.type == tk2dSpriteCollectionSize.Type.Explicit) {
			EditorGUI.indentLevel++;
			EditorGUILayout.LabelField("Resolution", "");
			EditorGUI.indentLevel++;
			spriteCollectionSize.width = EditorGUILayout.IntField("Width", (int)spriteCollectionSize.width);
			spriteCollectionSize.height = EditorGUILayout.IntField("Height", (int)spriteCollectionSize.height);
			EditorGUI.indentLevel--;
			spriteCollectionSize.orthoSize = EditorGUILayout.FloatField("Ortho Size", spriteCollectionSize.orthoSize);
			EditorGUI.indentLevel--;
		}
	}

	public override void OnInspectorGUI() {
		tk2dSpriteFromTexture target = (tk2dSpriteFromTexture)this.target;
		EditorGUIUtility.LookLikeInspector();

		EditorGUI.BeginChangeCheck();

		Texture texture = EditorGUILayout.ObjectField("Texture", target.texture, typeof(Texture), false) as Texture;

		if (texture == null) {
			EditorGUIUtility.LookLikeControls();
			tk2dGuiUtility.InfoBox("Drag a texture into the texture slot above.", tk2dGuiUtility.WarningLevel.Error);
		}

		tk2dBaseSprite.Anchor anchor = target.anchor;
		tk2dSpriteCollectionSize spriteCollectionSize = new tk2dSpriteCollectionSize();
		spriteCollectionSize.CopyFrom( target.spriteCollectionSize );

		if (texture != null) {
			anchor = (tk2dBaseSprite.Anchor)EditorGUILayout.EnumPopup("Anchor", target.anchor);
			SpriteCollectionSizeField( spriteCollectionSize );
		}

		if (EditorGUI.EndChangeCheck()) {
			Undo.RegisterUndo( target, "Sprite From Texture" );
			target.Create( spriteCollectionSize, texture, anchor );
		}
	}

    [MenuItem("GameObject/Create Other/tk2d/Sprite From Selected Texture", true, 12952)]
    static bool ValidateCreateSpriteObjectFromTexture()
    {
    	return Selection.activeObject != null && Selection.activeObject is Texture;
    }

    [MenuItem("GameObject/Create Other/tk2d/Sprite From Texture", true, 12953)]
    static bool ValidateCreateSpriteObject()
    {
    	return Selection.activeObject == null || !(Selection.activeObject is Texture);
    }

    [MenuItem("GameObject/Create Other/tk2d/Sprite From Selected Texture", false, 12952)]
    [MenuItem("GameObject/Create Other/tk2d/Sprite From Texture", false, 12953)]
    static void DoCreateSpriteObjectFromTexture()
    {
    	Texture tex = Selection.activeObject as Texture;
 
 		GameObject go = tk2dEditorUtility.CreateGameObjectInScene("Sprite");
		go.AddComponent<tk2dSprite>();
		tk2dSpriteFromTexture sft = go.AddComponent<tk2dSpriteFromTexture>();
		if (tex != null) {
			sft.Create( new tk2dSpriteCollectionSize(), tex, tk2dBaseSprite.Anchor.MiddleCenter );
		}
		Selection.activeGameObject = go;
		Undo.RegisterCreatedObjectUndo(go, "Create Sprite From Texture");
    }
}