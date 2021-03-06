using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace StarSystemSimulator.Graphics
{
	/// <summary>
	/// Class that stores the variable locations of a shader.
	/// These are needed to load in the parameters at the right spots into GPU memory.
	/// </summary>
	public class UniformManager
	{
		public const int PositionID = 0;
		public const int NormalID = 1;
		public const int ColorID = 2;
		public const int TexCoordID = 3;

		const int uniformCount = 5;

		readonly int[] ids;

		/// <summary>
		/// Gets/Sets the variable locations of the specific shader.
		/// </summary>
		public UniformManager(string name, int programID)
		{
			ids = new int[uniformCount];

			// Get the uniform locations. The call returns -1 if the uniform variable is not used in the shader.
			ids[0] = GL.GetUniformLocation(programID, "location");
			ids[1] = GL.GetUniformLocation(programID, "projection");
			ids[2] = GL.GetUniformLocation(programID, "modelView");
			ids[3] = GL.GetUniformLocation(programID, "modelColor");
			ids[4] = GL.GetUniformLocation(programID, "modelEmissive");

			Utils.CheckError("UniformManager1 " + name);

			// Set attribute locations to the given ones.
			GL.BindAttribLocation(programID, PositionID, "position");
			GL.BindAttribLocation(programID, NormalID, "normal");
			GL.BindAttribLocation(programID, ColorID, "color");
			GL.BindAttribLocation(programID, TexCoordID, "texCoord");

			Utils.CheckError("UniformManager2 " + name);
		}

		/// <summary>
		/// Push the uniform variables into GPU memory to their location.
		/// </summary>
		public void Uniform()
		{
			if (ids[0] >= 0)
				GL.Uniform2(ids[0], Camera.Location.Xy);

			UniformProjection(Camera.CameraMatrix);
			UniformModelView(Camera.IdentityMatrix);
			UniformColor(Color4.White);
		}

		/// <summary>
		/// Push the projection matrix into GPU memory to their location.
		/// 
		/// This matrix determines the camera position in 3D space.
		/// </summary>
		public void UniformProjection(Matrix4 matrix)
		{
			if (ids[1] >= 0)
				GL.UniformMatrix4(ids[1], false, ref matrix);
		}

		/// <summary>
		/// Push the modelView matrix into GPU memory to their location.
		/// 
		/// This matrix translates, rotates and scales an object in 3D space.
		/// </summary>
		public void UniformModelView(Matrix4 objectMatrix)
		{
			if (ids[2] >= 0)
				GL.UniformMatrix4(ids[2], false, ref objectMatrix);
		}

		/// <summary>
		/// Push the uniform color into GPU memory to their location.
		/// 
		/// The color will be multiplied with the color given in the Vector structure.
		/// </summary>
		public void UniformColor(Color4 color)
		{
			if (ids[3] >= 0)
				GL.Uniform4(ids[3], color);
		}

		/// <summary>
		/// Push the uniform emission strength into GPU memory to their location.
		/// 
		/// The strength will be multiplied with the color given in the Vector structure.
		/// </summary>
		public void UniformEmissionStrength(float strength)
		{
			if (ids[4] >= 0)
				GL.Uniform1(ids[4], strength);
		}
	}
}
