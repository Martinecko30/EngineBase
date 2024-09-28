using EngineBase.Shaders;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace EngineBase.Objects
{

    public class Skybox
    {
        float[] vertices =
        {
            // positions
            -1.0f, 1.0f, -1.0f,
            -1.0f, -1.0f, -1.0f,
            1.0f, -1.0f, -1.0f,
            1.0f, -1.0f, -1.0f,
            1.0f, 1.0f, -1.0f,
            -1.0f, 1.0f, -1.0f,

            -1.0f, -1.0f, 1.0f,
            -1.0f, -1.0f, -1.0f,
            -1.0f, 1.0f, -1.0f,
            -1.0f, 1.0f, -1.0f,
            -1.0f, 1.0f, 1.0f,
            -1.0f, -1.0f, 1.0f,

            1.0f, -1.0f, -1.0f,
            1.0f, -1.0f, 1.0f,
            1.0f, 1.0f, 1.0f,
            1.0f, 1.0f, 1.0f,
            1.0f, 1.0f, -1.0f,
            1.0f, -1.0f, -1.0f,

            -1.0f, -1.0f, 1.0f,
            -1.0f, 1.0f, 1.0f,
            1.0f, 1.0f, 1.0f,
            1.0f, 1.0f, 1.0f,
            1.0f, -1.0f, 1.0f,
            -1.0f, -1.0f, 1.0f,

            -1.0f, 1.0f, -1.0f,
            1.0f, 1.0f, -1.0f,
            1.0f, 1.0f, 1.0f,
            1.0f, 1.0f, 1.0f,
            -1.0f, 1.0f, 1.0f,
            -1.0f, 1.0f, -1.0f,

            -1.0f, -1.0f, -1.0f,
            -1.0f, -1.0f, 1.0f,
            1.0f, -1.0f, -1.0f,
            1.0f, -1.0f, -1.0f,
            -1.0f, -1.0f, 1.0f,
            1.0f, -1.0f, 1.0f
        };

        private int textureID = 0;

        private int VertexArrayObject, VertexBufferObject;

        private Shader shader;

        public Skybox(List<string> faces, Shader shader)
        {
            Initialize(faces, shader);
        }

        public Skybox(List<string> faces)
        {
            Initialize(faces, new Shader(
                "Resources\\Shaders\\DefaultSkyboxShader.vert",
                "Resources\\Shaders\\DefaultSkyboxShader.frag"
            ));
        }

        private void Initialize(List<string> faces, Shader shader)
        {
            textureID = Utils.BaseUtils.LoadCubemap(faces);

            this.shader = shader;

            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(
                BufferTarget.ArrayBuffer,
                vertices.Length * sizeof(float),
                vertices,
                BufferUsageHint.StaticDraw
            );
            VertexArrayObject = GL.GenVertexArray();

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
        }

        public void Render(Matrix4 view, Matrix4 projection)
        {
            GL.DepthMask(false);
            shader.Use();

            shader.SetMatrix4("view", view);
            shader.SetMatrix4("projection", projection);

            GL.BindVertexArray(VertexArrayObject);
            GL.BindTexture(TextureTarget.TextureCubeMap, textureID);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            GL.DepthMask(true);
        }
    }
}