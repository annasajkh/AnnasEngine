using OpenTK.Graphics.OpenGL4;

namespace AnnasEngine.Scripts.Rendering;

public static class MeshInstance
{
    private static Dictionary<string, Mesh> meshesCache = new Dictionary<string, Mesh>();

    // performace optimization will not allocated unless it is get called
    // also will get the cache instead of creating a new one each time
    public static Mesh Quad
    {
        get
        {
            if (meshesCache.ContainsKey("Quad"))
            {
                return meshesCache["Quad"];
            }
            else
            {
                meshesCache["Quad"] = new Mesh(

                    BufferUsageHint.StaticDraw,

                    new float[]
                    {
                        -0.5f, -0.5f, 0,                   0, 1,
                        0.5f, -0.5f, 0,                    1, 1,
                        0.5f, 0.5f, 0,                     1, 0,
                        -0.5f, 0.5f, 0,                    0, 0,
                    },

                    new uint[]
                    {
                        0, 1, 3,
                        1, 2, 3
                    }
                );

                return meshesCache["Quad"];
            }
        }
    }


    public static Mesh Plane
    {
        get
        {
            if (meshesCache.ContainsKey("Plane"))
            {
                return meshesCache["Plane"];
            }
            else
            {
                meshesCache["Plane"] = new Mesh(

                    BufferUsageHint.StaticDraw,

                    new float[]
                    {
                        -0.5f, 0, -0.5f,                    0, 1,
                        -0.5f, 0, 0.5f,                     1, 1,
                        0.5f, 0, 0.5f,                      1, 0,
                        0.5f, 0, -0.5f,                     0, 0,
                    },

                    new uint[]
                    {
                        0, 1, 3,
                        1, 2, 3
                    }
                );

                return meshesCache["Plane"];
            }
        }
    }


    public static Mesh Cube
    {
        get
        {
            if (meshesCache.ContainsKey("Cube"))
            {
                return meshesCache["Cube"];
            }
            else
            {
                meshesCache["Cube"] = new Mesh(

                    BufferUsageHint.StaticDraw,

                    new float[]
                    {
                        // front
                        -0.5f, -0.5f, 0.5f,                   0, 1,
                        0.5f, -0.5f, 0.5f,                    1, 1,
                        0.5f, 0.5f, 0.5f,                     1, 0,
                        -0.5f, 0.5f, 0.5f,                    0, 0,

                        // right
                        0.5f, -0.5f, -0.5f,                   1, 1,
                        0.5f, 0.5f, -0.5f,                    1, 0,
                        0.5f, 0.5f, 0.5f,                     0, 0,
                        0.5f, -0.5f, 0.5f,                    0, 1,

                        // back
                        0.5f, 0.5f, -0.5f,                    0, 0,
                        0.5f, -0.5f, -0.5f,                   0, 1,
                        -0.5f, -0.5f, -0.5f,                  1, 1,
                        -0.5f, 0.5f, -0.5f,                   1, 0,

                        // left
                        -0.5f, 0.5f, -0.5f,                   0, 0,
                        -0.5f, -0.5f, -0.5f,                  0, 1,
                        -0.5f, -0.5f,  0.5f,                  1, 1,
                        -0.5f, 0.5f,  0.5f,                   1, 0,

                        // top
                        0.5f, 0.5f, -0.5f,                    1, 0,
                        -0.5f, 0.5f, -0.5f,                   0, 0,
                        -0.5f, 0.5f, 0.5f,                    0, 1,
                        0.5f, 0.5f, 0.5f,                     1, 1,

                        // bottom
                        -0.5f, -0.5f, -0.5f,                  1, 0,
                        0.5f, -0.5f, -0.5f,                   0, 0,
                        0.5f, -0.5f, 0.5f,                    0, 1,
                        -0.5f, -0.5f, 0.5f,                   1, 1,
                    },

                    new uint[]
                    {
                        0, 1, 3,
                        1, 2, 3,

                        4, 5, 7,
                        5, 6, 7,

                        8, 9, 11,
                        9, 10, 11,

                        12, 13, 15,
                        13, 14, 15,

                        16, 17, 19,
                        17, 18, 19,

                        20, 21, 23,
                        21, 22, 23,
                    }
                );

                return meshesCache["Cube"];
            }
        }
    }
}
