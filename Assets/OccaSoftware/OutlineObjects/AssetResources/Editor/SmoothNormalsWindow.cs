using System.Collections.Generic;
using System.Linq;

using UnityEditor;

using UnityEngine;

namespace OccaSoftware.OutlineObjects.Editor
{
	public class SmoothNormalsWindow : EditorWindow
	{
		private Mesh mesh = null;
		private bool isInputValid = false;

		[MenuItem("Tools/Generate Smooth Normals")]
		static void OpenWindow()
		{
			//Create window
			SmoothNormalsWindow window = GetWindow<SmoothNormalsWindow>("Generate Smooth Normals");
			window.Show();
			window.ValidateInputs();

		}


		private void OnGUI()
		{
			DrawUI();
			DrawActionButton();
		}

		private void DrawUI()
		{
			EditorGUI.BeginChangeCheck();

			EditorGUILayout.HelpBox(
				"This tool will bake smooth normals to the Mesh's UV3 channel.\n\n" +
				"Set the mesh you want to bake, then press the \"Bake\" button.\n\n" +
				"Drag and drop the newly created mesh to the mesh input for the object (automatically created in the same directory as the original mesh)\n\n" +
				"Then, enable usage of the smoothed normals from the material inspector (enable Use Smoothed Normals).",
				MessageType.Info
				);

			mesh = (Mesh)EditorGUILayout.ObjectField("Mesh", mesh, typeof(Mesh), false);

			if (EditorGUI.EndChangeCheck())
			{
				ValidateInputs();
			}
		}

		private void DrawActionButton()
		{
			GUI.enabled = isInputValid;
			if (GUILayout.Button("Bake"))
			{
				GenerateSmoothNormals();
			}
			GUI.enabled = true;
		}

		private void ValidateInputs()
		{
			isInputValid = false;
			if (mesh != null)
			{
				isInputValid = true;
			}
		}

		List<Vector3> vertices = new List<Vector3>();
		List<Vector3> normals = new List<Vector3>();
		List<int> vertexIndices = new List<int>();
		Dictionary<UniqueVertex, List<VertexData>> meshData = new Dictionary<UniqueVertex, List<VertexData>>();

		private void GenerateSmoothNormals()
		{
			mesh.GetVertices(vertices);
			vertexIndices = mesh.GetTriangles(0).ToList();

			meshData.Clear();
			normals.Clear();

			const int VERTEX_COUNT = 3;
			int[] localIds = new int[VERTEX_COUNT];
			Vector3[] localVertices = new Vector3[VERTEX_COUNT];

			for (int i = 0; i < vertexIndices.Count; i += VERTEX_COUNT)
			{
				#region Set up Per-Vertex Normal Data
				System.Array.Clear(localIds, 0, VERTEX_COUNT);
				System.Array.Clear(localVertices, 0, VERTEX_COUNT);

				for (int a = 0; a < VERTEX_COUNT; a++)
				{
					localIds[a] = vertexIndices[i + a];
					localVertices[a] = vertices[localIds[a]];
				}

				Vector3 AB = localVertices[1] - localVertices[0];
				Vector3 AC = localVertices[2] - localVertices[0];
				Vector3 normal = Vector3.Cross(AB, AC);
				if (normal.magnitude > 0)
				{
					normal /= normal.magnitude;
				}
				#endregion

				#region Assign Vertex Data to a Unique Vertex
				List<VertexData> vertexData;
				UniqueVertex uniqueVertex;

				for (int b = 0; b < VERTEX_COUNT; b++)
				{
					uniqueVertex = new UniqueVertex(localVertices[b]);

					if (!meshData.TryGetValue(uniqueVertex, out vertexData))
					{
						vertexData = new List<VertexData>();
						meshData.Add(uniqueVertex, vertexData);
					}

					vertexData.Add(new VertexData(localIds[b], normal));
				}
				#endregion
			}
			#region Setup List of Normal by Vertex
			for (int i = 0; i < mesh.vertexCount; i++)
			{
				Vector3 vertexNormal = Vector3.zero;

				List<VertexData> vertexData = meshData[new UniqueVertex(vertices[i])];
				vertexData = vertexData.Distinct(new UniqueVertexDataComparer()).ToList();

				foreach (VertexData v in vertexData)
				{
					vertexNormal += v.Normal.normalized;
				}

				vertexNormal.Normalize();

				normals.Add(vertexNormal);
			}
			#endregion

			Mesh meshToSave = Instantiate(mesh);

			meshToSave.SetUVs(3, normals);

			string path = AssetDatabase.GetAssetPath(mesh);
			path = System.IO.Path.GetDirectoryName(path) + System.IO.Path.DirectorySeparatorChar;
			AssetDatabase.CreateAsset(meshToSave, path + mesh.name + "_Smoothed" + ".asset");
			AssetDatabase.SaveAssets();
		}

		class UniqueVertexDataComparer : IEqualityComparer<VertexData>
		{
			public bool Equals(VertexData x, VertexData y)
			{
				return x.Index == y.Index;
			}

			public int GetHashCode(VertexData obj)
			{
				return obj.Index.GetHashCode();
			}
		}

		class UniqueVertex
		{
			private const float Precision = 0.00001f;
			private const int Tolerance = (int)(1f / Precision);

			private long x;
			private long y;
			private long z;

			public UniqueVertex(Vector3 position)
			{
				x = (long)Mathf.Round(position.x * Tolerance);
				y = (long)Mathf.Round(position.y * Tolerance);
				z = (long)Mathf.Round(position.z * Tolerance);
			}


			public override bool Equals(object obj)
			{
				UniqueVertex key = (UniqueVertex)obj;
				return x == key.x && y == key.y && z == key.z;
			}

			public override int GetHashCode()
			{
				return System.Tuple.Create(x, y, z).GetHashCode();
			}
		}

		class VertexData
		{
			public int Index;
			public Vector3 Normal;

			public VertexData(int vertexIndex, Vector3 normal)
			{
				Index = vertexIndex;
				Normal = normal;
			}
		}

	}
}
