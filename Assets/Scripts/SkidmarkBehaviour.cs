using UnityEngine;
using System.Collections;

[RequireComponent (typeof (MeshFilter))]
[RequireComponent (typeof (MeshRenderer))]

// This SkidmarkBehaviour script controlles the skidmarks for the car. 
// It registers the position, normal etc. of all the small sections of
// the skidmarks that combined makes up the entire skidmark mesh.
// A new mesh is auto generated whenever the skidmarks change.
// The script was translated and slightly simplyfied from the skimark.js
// scipt of the Unity Car Tutorial.

public class SkidmarkBehaviour : MonoBehaviour
{
	public 	int 	maxMarks = 1024;		// Maximum number of marks
	public  float   markWidth = 0.275f;		// The width of the skidmarks
	public  float 	groundOffset = 0.02f;	// The distance above the surface
	
	
	// Variables for each mark created. Needed to generate the correct mesh.
	private class markSegment
	{
		public Vector3 	pos 	= Vector3.zero;	// center position
		public Vector3 	posl 	= Vector3.zero;	// vertex 0 of the mark quat
		public Vector3 	posr 	= Vector3.zero; // vertex 1 of the mark quat
		public Vector3 	normal 	= Vector3.zero;	// normal of the mark quat
		public Vector4 	tangent = Vector4.zero;	// posl - posr
		public float 	intensity = 0.0f;		// alpha intensity
		public int 		lastIndex = 0;			// index of the last mark
	};
	
	private markSegment[] skidmarks;			// The skidmark array
	private int 		  numMarks = 0;			// No. of marks used in
	private bool 		  updateMesh = false;	// flag if mesh mus
	
	// Initiallizes the array holding the skidmark sections.
	void Awake()
	{
		skidmarks = new markSegment[maxMarks];
		for(var i = 0; i < maxMarks; i++)
			skidmarks[i]=new markSegment();
		
		if(GetComponent<MeshFilter>().mesh == null)
			GetComponent<MeshFilter>().mesh = new Mesh();
	}
	
	// Function Add is called by the wheels that is skidding. 
	// Gathers all the information needed to create the mesh later in LateUpdate. 
	// It sets the intensity of the skidmark segment by setting the alpha
	// of the vertex color. 
	public int Add(Vector3 pos, Vector3 normal, float intensity, int lastIndex, string name)
	{

		// change skidmarks possition to the wheels
		if (name == "WheelColliderFR" || name == "WheelColliderRR") {
			pos.x -= 0.14f;
		} else {
			pos.x += 0.14f;
		}

		// reduce intensity for min. alpha blending (= opaque)
		if(intensity > 1) intensity = 1.0f;
		
		// finsih skidmark with an intensity lower than zero
		if(intensity < 0) return -1;
		
		// init new skidmark at index numMarks%maxMark (overwritting old ones)
		markSegment curr = skidmarks[numMarks % maxMarks];
		curr.pos = pos + normal * groundOffset;
		curr.normal = normal;
		curr.intensity = intensity;
		curr.lastIndex = lastIndex;
		
		// if it is not the first skidmark position
		if(lastIndex != -1)
		{
			// get the last mark to attach the new mark
			markSegment last = skidmarks[lastIndex % maxMarks];
			
			// compute the direction & perpendicular vector of the new mark
			Vector3 dir = curr.pos - last.pos;
			Vector3 perpDir = Vector3.Cross(dir,normal).normalized;
			Vector3 perpHalfWidth = perpDir * markWidth * 0.5f;
			
			curr.posl = curr.pos + perpHalfWidth;
			//curr.posl = curr.pos + perpDir;
			Debug.Log ("curr.posl: " + curr.posl);
			curr.posr = curr.pos - perpHalfWidth;
			//curr.posr = curr.pos - perpDir;
			Debug.Log ("curr.posr: " + curr.posr);
			curr.tangent = new Vector4(perpDir.x, perpDir.y, perpDir.z, 1);
			
			// if it is the second skidmark position finish the data of the 1st pos.
			if(last.lastIndex == -1)
			{	last.tangent = curr.tangent;
				last.posl = curr.pos + perpHalfWidth;
				last.posr = curr.pos - perpHalfWidth;
			}
		}
		
		numMarks++;
		updateMesh = true;
		
		// return index of the new mark
		return numMarks - 1;
	}
	
	// Update the mesh if new skidmark segments where defined
	void LateUpdate()
	{
		if(!updateMesh) return;
		
		// get the mesh & clear all data
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		mesh.Clear();
		
		// count all segments in the array skidmark that are used
		int segmentCount = 0;
		for(int j = 0; j < numMarks && j < maxMarks; j++)
			if(skidmarks[j].lastIndex != -1 && skidmarks[j].lastIndex > numMarks-maxMarks)
				segmentCount++;
		
		// create temp. arrays for mesh attributes 
		int       numV      = segmentCount*4;		// No. of vertices
		int       numT      = segmentCount*6;		// No. of triangle vertex indexes
		Vector3[] vertices 	= new Vector3[numV];	// vertex positions
		Vector3[] normals 	= new Vector3[numV];	// normal vectors
		Vector4[] tangents 	= new Vector4[numV];	// tangent vectors
		Color[]	  colors 	= new Color	 [numV];	// RGBA colors
		Vector2[] uvs 		= new Vector2[numV];	// texture coordinates
		int[] 	  triangles = new int	 [numT];	// triangle vertex indexes
		
		// set mesh data for all segments by looping over the skidmark array
		segmentCount = 0;
		for(int i = 0; i < numMarks && i < maxMarks; i++)
		{
			if(skidmarks[i].lastIndex != -1 && skidmarks[i].lastIndex > numMarks-maxMarks)
			{
				markSegment curr = skidmarks[i];
				markSegment last = skidmarks[curr.lastIndex % maxMarks];
				
				// Set vertices, normals, tangents & uv-coordinates
				numV = segmentCount*4;
				vertices[numV    ] = last.posl;
				vertices[numV + 1] = last.posr;
				vertices[numV + 2] = curr.posl;
				vertices[numV + 3] = curr.posr;
				
				normals[numV    ] = last.normal;
				normals[numV + 1] = last.normal;
				normals[numV + 2] = curr.normal;
				normals[numV + 3] = curr.normal;
				
				tangents[numV    ] = last.tangent;
				tangents[numV + 1] = last.tangent;
				tangents[numV + 2] = curr.tangent;
				tangents[numV + 3] = curr.tangent;
				
				colors[numV    ] = new Color(0, 0, 0, last.intensity);
				colors[numV + 1] = new Color(0, 0, 0, last.intensity);
				colors[numV + 2] = new Color(0, 0, 0, curr.intensity);
				colors[numV + 3] = new Color(0, 0, 0, curr.intensity);
				
				uvs[numV    ] = new Vector2(0, 0);
				uvs[numV + 1] = new Vector2(1, 0);
				uvs[numV + 2] = new Vector2(0, 1);
				uvs[numV + 3] = new Vector2(1, 1);
				
				// Set 6 triangle indexes of 2 triangles
				numT = segmentCount*6;
				triangles[numT    ] = numV;
				triangles[numT + 2] = numV + 1;
				triangles[numT + 1] = numV + 2;
				triangles[numT + 3] = numV + 2;
				triangles[numT + 5] = numV + 1;
				triangles[numT + 4] = numV + 3;
				
				segmentCount++;
			}
		}
		
		// finally set new attributs to
		mesh.vertices  = vertices;
		mesh.normals   = normals;
		mesh.tangents  = tangents;
		mesh.triangles = triangles;
		mesh.colors	   = colors;
		mesh.uv        = uvs;
		
		updateMesh = false;
	}
}
