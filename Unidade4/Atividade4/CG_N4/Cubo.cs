//https://github.com/mono/opentk/blob/main/Source/Examples/Shapes/Old/Cube.cs

#define CG_Debug
using CG_Biblioteca;
using OpenTK.Mathematics;
using System.Drawing;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;

namespace gcgcg
{
  internal class Cubo : Objeto
  {
    Vector3[] vertices;
    int[] indices;
    // Vector3[] normals;

    List<Ponto4D> listaPtos = new List<Ponto4D>();

    public Cubo(Objeto paiRef, ref char _rotulo) : base(paiRef, ref _rotulo)
    {
      vertices = new Vector3[]
      {
        new Vector3(-1.0f, -1.0f,  1.0f),
        new Vector3( 1.0f, -1.0f,  1.0f),
        new Vector3( 1.0f,  1.0f,  1.0f),
        new Vector3(-1.0f,  1.0f,  1.0f),
        new Vector3(-1.0f, -1.0f, -1.0f),
        new Vector3( 1.0f, -1.0f, -1.0f),
        new Vector3( 1.0f,  1.0f, -1.0f),
        new Vector3(-1.0f,  1.0f, -1.0f)
      };

      indices = new int[]
      {
        0, 1, 2, 2, 3, 0, // front face
        3, 2, 6, 6, 7, 3, // top face
        7, 6, 5, 5, 4, 7, // back face
        4, 0, 3, 3, 7, 4, // left face
        0, 1, 5, 5, 4, 0, // bottom face  
        1, 5, 6, 6, 2, 1, // right face
      };

      // normals = new Vector3[]
      // {
      //   new Vector3(-1.0f, -1.0f,  1.0f),
      //   new Vector3( 1.0f, -1.0f,  1.0f),
      //   new Vector3( 1.0f,  1.0f,  1.0f),
      //   new Vector3(-1.0f,  1.0f,  1.0f),
      //   new Vector3(-1.0f, -1.0f, -1.0f),
      //   new Vector3( 1.0f, -1.0f, -1.0f),
      //   new Vector3( 1.0f,  1.0f, -1.0f),
      //   new Vector3(-1.0f,  1.0f, -1.0f),
      // };

      foreach (Vector3 v in vertices)
        listaPtos.Add(new Ponto4D(v.X, v.Y, v.Z));

      Objeto cubo = new Objeto(paiRef, ref _rotulo);
      cubo.PrimitivaTipo = PrimitiveType.Triangles;

      for (int i = 0; i < 36; i++) {
        cubo.PontosAdicionar(listaPtos[indices[i]]);
      }

      Atualizar();
    }

    public static int ColorToRgba32(Color c)
    {
      return (int)((c.A << 24) | (c.B << 16) | (c.G << 8) | c.R);
    }

    private void Atualizar()
    {

      base.ObjetoAtualizar();
    }

#if CG_Debug
    public override string ToString()
    {
      string retorno;
      retorno = "__ Objeto Cubo _ Tipo: " + PrimitivaTipo + " _ Tamanho: " + PrimitivaTamanho + "\n";
      retorno += base.ImprimeToString();
      return (retorno);
    }
#endif

  }
}