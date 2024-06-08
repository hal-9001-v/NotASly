using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Splines;
using UnityEngine.UIElements;

public class WallGenerator : MonoBehaviour
{
    [SerializeField][Range(0.1f, 30)] float height = 3;
    [SerializeField] SplineContainer spline;

    Matrix4x4[] wallMatrices;

    MeshRenderer MeshRenderer => GetComponent<MeshRenderer>();
    MeshFilter MeshFilter => GetComponent<MeshFilter>();
    MeshCollider MeshCollider => GetComponent<MeshCollider>();

    [SerializeField] WeightedSet walls;

    List<GameObject> assets;

    Transform colliderHolder;


    [ContextMenu("Generate Walls")]
    public void Walls()
    {
        assets = new List<GameObject>();
        var length = spline.CalculateLength();

        if (colliderHolder == null)
        {
            colliderHolder = new GameObject("Collider Holder").transform;
            colliderHolder.SetParent(transform);
        }

        for (int i = 0; i < colliderHolder.childCount; i++)
        {
            DestroyImmediate(colliderHolder.GetChild(i).gameObject);
        }

        float initialLength = 0;
        int maxIterations = 1000;
        int iterations = 0;
        while (initialLength < length)
        {
            if (iterations++ > maxIterations)
            {
                Debug.LogError("Max iterations reached");
                return;
            }

            var asset = walls.GetRandom();
            var size = asset.GetComponent<MeshFilter>().sharedMesh.bounds.size;

            if (initialLength + size.x > length)
                break;

            assets.Add(asset);
            initialLength += size.x * asset.transform.localScale.x;
        }
        
        wallMatrices = new Matrix4x4[assets.Count];
        List<CombineInstance> combines = new List<CombineInstance>();
        List<Material> materials = new();

        Vector3 scale = new Vector3();
        scale.z = 1;
        scale.x = length / initialLength;
        float t = 0;

        for (int i = 0; i < assets.Count; i++)
        {
            var assetMeshFilter = assets[i].GetComponent<MeshFilter>();
            var subMeshes = new Mesh[assetMeshFilter.sharedMesh.subMeshCount];

            var mesh = assets[i].GetComponent<MeshFilter>().sharedMesh;

            var size = mesh.bounds.size;
            size.x *= assets[i].transform.localScale.x;
            size.y *= assets[i].transform.localScale.y;
            size.z *= assets[i].transform.localScale.z;

            float step = size.x * scale.x / length;
            scale.y = height / size.y;

            var translation = spline.EvaluatePosition(t + step * 0.5f);
            var rotation = Quaternion.LookRotation(spline.EvaluateTangent(t + step * 0.5f), Vector3.up) * assets[i].transform.localRotation;

            var meshScale = assets[i].transform.localScale;
            meshScale.Scale(scale);
            wallMatrices[i] = Matrix4x4.TRS(translation, rotation, meshScale);


            var vertices = new List<Vector3>();
            assetMeshFilter.sharedMesh.GetVertices(vertices);
            var normals = new List<Vector3>();
            assetMeshFilter.sharedMesh.GetNormals(normals);
            var tangents = new List<Vector4>();
            assetMeshFilter.sharedMesh.GetTangents(tangents);

            //Combine meshes https://gist.github.com/simonwittber/0297a2a8127efddd818d624ce14bd6a8
            for (int j = 0; j < subMeshes.Length; j++)
            {
                var subMesh = Extract(assetMeshFilter.sharedMesh, j);

                combines.Add(new CombineInstance
                {
                    mesh = subMesh,
                    transform = wallMatrices[i]
                });
            }

            materials.AddRange(assets[i].GetComponent<MeshRenderer>().sharedMaterials);
            //AddCollision(assets[i], wallMatrices[i]); -->Right now it doesnt work

            t += step;
        }


        MeshFilter.sharedMesh = new Mesh();
        MeshFilter.sharedMesh.CombineMeshes(combines.ToArray(), false, true);
        MeshFilter.sharedMesh.Optimize();

        MeshRenderer.sharedMaterials = materials.ToArray();

        MeshCollider.sharedMesh = MeshFilter.sharedMesh;

        //Debug
        //MeshFilter.sharedMesh = MeshCollider.sharedMesh;
    }

    public static Mesh Extract(Mesh m, int meshIndex)
    {
        var vertices = m.vertices;
        var normals = m.normals;
        var uvs = m.uv;

        var newVerts = new List<Vector3>();
        var newNorms = new List<Vector3>();
        var newUVs = new List<Vector2>();
        var newTris = new List<int>();
        var triangles = m.GetTriangles(meshIndex);
        for (var i = 0; i < triangles.Length; i += 3)
        {
            var A = triangles[i + 0];
            var B = triangles[i + 1];
            var C = triangles[i + 2];
            newVerts.Add(vertices[A]);
            newVerts.Add(vertices[B]);
            newVerts.Add(vertices[C]);
            newNorms.Add(normals[A]);
            newNorms.Add(normals[B]);
            newNorms.Add(normals[C]);
            newUVs.Add(uvs[A]);
            newUVs.Add(uvs[B]);
            newUVs.Add(uvs[C]);
            newTris.Add(newTris.Count);
            newTris.Add(newTris.Count);
            newTris.Add(newTris.Count);
        }
        var mesh = new Mesh();
        mesh.indexFormat = newVerts.Count > 65536 ? IndexFormat.UInt32 : IndexFormat.UInt16;
        mesh.SetVertices(newVerts);
        mesh.SetNormals(newNorms);
        mesh.SetUVs(0, newUVs);
        mesh.SetTriangles(newTris, 0, true);

        return mesh;
    }

    //Todo: make it work!
    void AddCollision(GameObject asset, Matrix4x4 matrix)
    {
        var assetCollider = asset.GetComponent<Collider>();

        if (assetCollider)
        {

            switch (assetCollider)
            {
                case BoxCollider box:


                    var child = new GameObject("Collider");
                    var collider = child.AddComponent<BoxCollider>();
                    var meshFilter = asset.GetComponent<MeshFilter>();

                    var scale = new Vector3();
                    scale.z = 1;
                    //scale.x = mean / meshFilter.sharedMesh.bounds.size.x;
                    scale.y = height / meshFilter.sharedMesh.bounds.size.y;

                    child.transform.SetParent(colliderHolder);
                    collider.size = new Vector3(box.size.x * scale.x, box.size.y * scale.y, box.size.z);
                    collider.center = box.center;

                    collider.transform.localPosition = matrix.GetColumn(3);
                    collider.transform.localRotation = matrix.rotation;
                    break;

                case MeshCollider meshCollider:

                    if (MeshCollider.sharedMesh != null)
                    {
                        CombineInstance[] combine = new CombineInstance[2];
                        combine[0].mesh = meshCollider.sharedMesh;
                        combine[0].transform = matrix;
                        combine[1].mesh = MeshCollider.sharedMesh;
                        combine[1].transform = Matrix4x4.identity;

                        MeshCollider.sharedMesh = new Mesh();
                        MeshCollider.sharedMesh.CombineMeshes(combine, true, true);
                    }
                    else
                    {
                        MeshCollider.sharedMesh = meshCollider.sharedMesh;
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public void AddCorner(Vector3 position)
    {
        var bezier = new BezierKnot(position);

        spline.Spline.Add(bezier);
    }

    float previousLength;

    private void Update()
    {
        if (previousLength != spline.CalculateLength())
        {
            Walls();
            previousLength = spline.CalculateLength();
        }
    }
    class ProceduralAsset
    {
        public Mesh Mesh;
        public Collider Collider;
        public Transform Transform;
    }

}
