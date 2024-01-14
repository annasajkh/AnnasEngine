using AnnasEngine.Scripts.DataStructures.Vertex;
using AnnasEngine.Scripts.DataStructures.Vertex.Components;
using AnnasEngine.Scripts.Rendering.OpenGL.VertexArrayObjects;
using AnnasEngine.Scripts.Rendering.OpenGL.VertexArrayObjects.Components;
using Assimp;
using Assimp.Configs;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Mesh = AnnasEngine.Scripts.Rendering.Mesh;

namespace AnnasEngine.Scripts.Utils;

public static class Helpers
{
    private static AssimpContext importer = new AssimpContext();

    private static float SnapToGrid(float value, float gridSize)
    {
        return (float)(MathHelper.Floor(value / gridSize) * gridSize);
    }

    public static Vector3 SnapToGrid(Vector3 value, int gridSize)
    {
        return new Vector3(SnapToGrid(value.X, gridSize), SnapToGrid(value.Y, gridSize), SnapToGrid(value.Z, gridSize));
    }

    public static Color4 LerpColor(Color4 color1, Color4 color2, float t)
    {
        return new Color4(color1.R + (color2.R - color1.R) * t,
                          color1.G + (color2.G - color1.G) * t,
                          color1.B + (color2.B - color1.B) * t,
                          color1.A + (color2.A - color1.A) * t);
    }

    // t is between 0 - 1
    public static Color4 Lerp3Color(Color4 color1, Color4 color2, Color4 color3, float t)
    {
        if (t < 0.5f)
        {
            return LerpColor(color1, color2, t * 2);
        }
        else
        {
            return LerpColor(color2, color3, (t - 0.5f) * 2);
        }
    }

    public static List<Mesh> LoadModelFromFile(string path, BufferUsageHint bufferUsageHint, VertexArrayObject vertexArrayObject)
    {
        List<Mesh> meshes = new List<Mesh>();

        importer.SetConfig(new NormalSmoothingAngleConfig(66.0f));
        importer.SetConfig(new GlobalScaleConfig(1));

        var model = importer.ImportFile(Path.GetFullPath(path), PostProcessPreset.TargetRealTimeMaximumQuality);

        foreach (var mesh in model.Meshes)
        {
            List<float> vertices = new List<float>();

            uint[] indices = Array.ConvertAll(mesh.GetIndices(), value => checked((uint)value));

            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                Vertex vertex = new Vertex();

                foreach (var vertexArrayComponent in vertexArrayObject.Container.GetAllComponents())
                {
                    switch (vertexArrayComponent)
                    {
                        case VertexArrayPositionComponent:

                            if (!mesh.HasVertices)
                            {
                                throw new Exception("One of the mesh in the loaded obj doesn't contain position attribute");
                            }

                            vertex.AddComponent(new VertexPositionComponent(new Vector3()
                            {
                                X = mesh.Vertices[i].X,
                                Y = mesh.Vertices[i].Y,
                                Z = mesh.Vertices[i].Z,
                            }));
                            break;

                        case VertexArrayColorComponent:
                            if (!mesh.HasVertexColors(0))
                            {
                                throw new Exception("One of the mesh in the loaded obj doesn't contain color attribute");
                            }

                            vertex.AddComponent(new VertexNormalComponent(new Vector3()
                            {
                                X = mesh.Normals[i].X,
                                Y = mesh.Normals[i].Y,
                                Z = mesh.Normals[i].Z,
                            }));
                            break;

                        case VertexArrayNormalComponent:
                            if (!mesh.HasNormals)
                            {
                                throw new Exception("One of the mesh in the loaded obj doesn't contain normal attribute");
                            }

                            vertex.AddComponent(new VertexColorComponent(new Color4()
                            {
                                R = mesh.VertexColorChannels[0][i].R,
                                G = mesh.VertexColorChannels[0][i].G,
                                B = mesh.VertexColorChannels[0][i].B,
                                A = mesh.VertexColorChannels[0][i].A,
                            }));
                            break;

                        case VertexArrayTextureCoordinateComponent:
                            if (!mesh.HasTextureCoords(0))
                            {
                                throw new Exception("One of the mesh in the loaded obj doesn't contain textures coordinate attribute");
                            }

                            vertex.AddComponent(new VertexTextureCoordinateComponent(new Vector2()
                            {
                                X = mesh.TextureCoordinateChannels[0][i].X,
                                Y = mesh.TextureCoordinateChannels[0][i].Y,
                            }));
                            break;
                    }
                }

                vertices.AddRange(vertex.Build());
            }

            meshes.Add(new Mesh(bufferUsageHint, vertices.ToArray(), indices));
        }


        return meshes;
    }

}
