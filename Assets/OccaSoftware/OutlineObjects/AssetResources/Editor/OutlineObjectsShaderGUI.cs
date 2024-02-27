using UnityEditor;

using UnityEngine;



namespace OccaSoftware.OutlineObjects.Editor
{
	public class OutlineObjectsShaderGUI : ShaderGUI
	{
		public override void OnGUI(MaterialEditor e, MaterialProperty[] properties)
		{

			MaterialProperty _OutlineColor = FindProperty("_OutlineColor", properties);
			MaterialProperty _OutlineThickness = FindProperty("_OutlineThickness", properties);
			MaterialProperty _CompleteFalloffDistance = FindProperty("_CompleteFalloffDistance", properties);
			MaterialProperty _NoiseTexture = FindProperty("_NoiseTexture", properties);
			MaterialProperty _NoiseFrequency = FindProperty("_NoiseFrequency", properties);
			MaterialProperty _NoiseFramerate = FindProperty("_NoiseFramerate", properties);

			MaterialProperty _USE_VERTEX_COLOR_ENABLED = FindProperty("_USE_VERTEX_COLOR_ENABLED", properties);
			MaterialProperty _ATTENUATE_BY_DISTANCE_ENABLED = FindProperty("_ATTENUATE_BY_DISTANCE_ENABLED", properties);
			MaterialProperty _RANDOM_OFFSETS_ENABLED = FindProperty("_RANDOM_OFFSETS_ENABLED", properties);
			MaterialProperty _USE_SMOOTHED_NORMALS_ENABLED = FindProperty("_USE_SMOOTHED_NORMALS_ENABLED", properties);


			Material mat = e.target as Material;
			EditorGUI.BeginChangeCheck();

			EditorGUILayout.LabelField("Basic Configuration", EditorStyles.boldLabel);
			EditorGUI.indentLevel++;
			e.ShaderProperty(_OutlineColor, new GUIContent("Outline Color"));
			DrawFloatWithMinValue(new GUIContent("Outline Thickness"), _OutlineThickness, 0);
			EditorGUI.indentLevel--;

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Vertex Color Configuration", EditorStyles.boldLabel);
			EditorGUI.indentLevel++;
			e.ShaderProperty(_USE_VERTEX_COLOR_ENABLED, new GUIContent("Use Vertex Color", "Uses the Vertex Color (R Channel) to reduce the size of the outline. If the Vertex Color (R) is 0, the outline will have a width of 0."));
			EditorGUI.indentLevel--;


			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Distance-based Attenuation", EditorStyles.boldLabel);
			EditorGUI.indentLevel++;
			e.ShaderProperty(_ATTENUATE_BY_DISTANCE_ENABLED, new GUIContent("Attenuate by Distance"));
			if (_ATTENUATE_BY_DISTANCE_ENABLED.intValue == 1)
			{
				e.ShaderProperty(_CompleteFalloffDistance, new GUIContent("Complete Falloff Distance"));
			}
			EditorGUI.indentLevel--;

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Noise", EditorStyles.boldLabel);
			EditorGUI.indentLevel++;
			DrawTextureProperty(_NoiseTexture, new GUIContent("Noise Texture", "Distance-based noise field"));


			if (_NoiseTexture.textureValue != null)
			{
				EditorGUI.indentLevel++;
				e.ShaderProperty(_NoiseFrequency, new GUIContent("Noise Frequency"));
				e.ShaderProperty(_NoiseFramerate, new GUIContent("Noise Framerate"));
				e.ShaderProperty(_RANDOM_OFFSETS_ENABLED, new GUIContent("Jitter"));
				EditorGUI.indentLevel--;
			}
			EditorGUI.indentLevel--;

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Normals", EditorStyles.boldLabel);

			EditorGUI.indentLevel++;
			e.ShaderProperty(_USE_SMOOTHED_NORMALS_ENABLED, new GUIContent("Use Smoothed Normals", "Use the Generate Smooth Normals tool to bake smoothed normals to the UV3 channel of your mesh."));
			EditorGUI.indentLevel--;


			EditorGUILayout.Space();
			if (EditorGUILayout.LinkButton("Register Asset"))
			{
				Application.OpenURL("https://www.occasoftware.com/register");
			}

			if (EditorGUILayout.LinkButton("Documentation"))
			{
				Application.OpenURL("https://www.occasoftware.com/assets/outline-objects");
			}

			void DrawTextureProperty(MaterialProperty p, GUIContent c)
			{
				EditorGUI.BeginChangeCheck();
				EditorGUI.showMixedValue = p.hasMixedValue;
				Texture2D t = (Texture2D)EditorGUILayout.ObjectField(c, p.textureValue, typeof(Texture2D), false, GUILayout.Height(EditorGUIUtility.singleLineHeight));
				if (EditorGUI.EndChangeCheck())
				{
					p.textureValue = t;
				}
				EditorGUI.showMixedValue = false;
			}


			void DrawFloatWithMinValue(GUIContent content, MaterialProperty a, float min)
			{
				EditorGUI.BeginChangeCheck();
				EditorGUI.showMixedValue = a.hasMixedValue;
				e.ShaderProperty(a, content);

				if (EditorGUI.EndChangeCheck())
					a.floatValue = Mathf.Max(min, a.floatValue);

				EditorGUI.showMixedValue = false;
			}
		}

		private static class ShaderParams
		{
			public static int outlineColor = Shader.PropertyToID("_OutlineColor");
			public static int outlineThickness = Shader.PropertyToID("_OutlineThickness");
			public static int completeFalloffDistance = Shader.PropertyToID("_CompleteFalloffDistance");
			public static int noiseTexture = Shader.PropertyToID("_NoiseTexture");
			public static int noiseFrequency = Shader.PropertyToID("_NoiseFrequency");
			public static int noiseFramerate = Shader.PropertyToID("_NoiseFramerate");

			public static string useVertexColors = "USE_VERTEX_COLOR_ENABLED";
			public static string attenuateByDistance = "ATTENUATE_BY_DISTANCE_ENABLED";
			public static string randomOffset = "RANDOM_OFFSETS_ENABLED";
		}
	}
}