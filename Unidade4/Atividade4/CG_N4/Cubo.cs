//https://github.com/mono/opentk/blob/main/Source/Examples/Shapes/Old/Cube.cs

#define CG_Debug
using CG_Biblioteca;
using OpenTK.Mathematics;
using System.Drawing;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;

namespace gcgcg
{
  internal class Cubo : Objeto
  {

    public Cubo(Objeto paiRef, ref char _rotulo, Shader cor) : base(paiRef, ref _rotulo)
    {
      List<Ponto4D> ptosFrente_0 = new List<Ponto4D>() {
        new Ponto4D(-0.5, -0.5, 0.5),
        new Ponto4D(0.5, -0.5, 0.5),
        new Ponto4D(0.5, 0.5, 0.5),
        new Ponto4D(-0.5, 0.5, 0.5)
      };

      Objeto faceFrente_0 = new Poligono(paiRef, ref _rotulo, ptosFrente_0);
      //faceFrente.ObjetoCor = cor;
      faceFrente_0.ObjetoAtualizar();
      faceFrente_0.PrimitivaTipo = PrimitiveType.TriangleFan;

      List<Ponto4D> ptosFrente_1 = new List<Ponto4D>() {
        new Ponto4D(-0.5, 0.5, 0.5),
        new Ponto4D(0.5, 0.5, 0.5),
        new Ponto4D(0.5, -0.5, 0.5),
        new Ponto4D(-0.5, -0.5, 0.5)
      };

      Objeto faceFrente_1 = new Poligono(paiRef, ref _rotulo, ptosFrente_1);
      //faceFrente_1.ObjetoCor = cor;
      faceFrente_1.ObjetoAtualizar();
      faceFrente_1.PrimitivaTipo = PrimitiveType.TriangleFan;

      // ---

      List<Ponto4D> ptosAbaixo_0 = new List<Ponto4D>() {
        new Ponto4D(-0.5, -0.5, -0.5),
        new Ponto4D(0.5, -0.5, -0.5),
        new Ponto4D(-0.5, -0.5, 0.5),
        new Ponto4D(0.5, -0.5, 0.5)
      };

      Objeto faceAbaixo_0 = new Poligono(paiRef, ref _rotulo, ptosAbaixo_0);
      //faceAbaixo_0.ObjetoCor = cor;
      faceAbaixo_0.ObjetoAtualizar();
      faceAbaixo_0.PrimitivaTipo = PrimitiveType.TriangleFan;

      List<Ponto4D> ptosAbaixo_1 = new List<Ponto4D>() {
        new Ponto4D(0.5, -0.5, 0.5),
        new Ponto4D(-0.5, -0.5, 0.5),
        new Ponto4D(0.5, -0.5, -0.5),
        new Ponto4D(-0.5, -0.5, -0.5)
      };

      Objeto faceAbaixo_1 = new Poligono(paiRef, ref _rotulo, ptosAbaixo_1);
      //faceAbaixo_0.ObjetoCor = cor;
      faceAbaixo_1.ObjetoAtualizar();
      faceAbaixo_1.PrimitivaTipo = PrimitiveType.TriangleFan;

      // ---

      List<Ponto4D> ptosAtras_0 = new List<Ponto4D>() {
        new Ponto4D(-0.5, -0.5, -0.5),
        new Ponto4D(0.5, -0.5, -0.5),
        new Ponto4D(-0.5, 0.5, -0.5),
        new Ponto4D(0.5, 0.5, -0.5)
      };

      Objeto faceAtras_0 = new Poligono(paiRef, ref _rotulo, ptosAtras_0);
      //faceAtras.ObjetoCor = cor;
      faceAtras_0.ObjetoAtualizar();
      faceAtras_0.PrimitivaTipo = PrimitiveType.TriangleFan;

      List<Ponto4D> ptosAtras_1 = new List<Ponto4D>() {
        new Ponto4D(0.5, -0.5, -0.5),
        new Ponto4D(0.5, 0.5, -0.5),
        new Ponto4D(-0.5, 0.5, -0.5),
        new Ponto4D(-0.5, -0.5, -0.5)
      };

      Objeto faceAtras_1 = new Poligono(paiRef, ref _rotulo, ptosAtras_1);
      //faceAtras.ObjetoCor = cor;
      faceAtras_1.ObjetoAtualizar();
      faceAtras_1.PrimitivaTipo = PrimitiveType.TriangleFan;

      // ---

      List<Ponto4D> ptosEsquerda_0 = new List<Ponto4D>() {
        new Ponto4D(-0.5, -0.5, -0.5),
        new Ponto4D(-0.5, 0.5, -0.5),
        new Ponto4D(-0.5, -0.5, 0.5),
        new Ponto4D(-0.5, 0.5, 0.5)
      };

      Objeto faceEsquerda_0 = new Poligono(paiRef, ref _rotulo, ptosEsquerda_0);
      //faceEsquerda.ObjetoCor = cor;
      faceEsquerda_0.ObjetoAtualizar();
      faceEsquerda_0.PrimitivaTipo = PrimitiveType.TriangleFan;

      List<Ponto4D> ptosEsquerda_1 = new List<Ponto4D>() {
        new Ponto4D(-0.5, 0.5, 0.5),
        new Ponto4D(-0.5, -0.5, 0.5),
        new Ponto4D(-0.5, 0.5, -0.5),
        new Ponto4D(-0.5, -0.5, -0.5)
      };

      Objeto faceEsquerda_1 = new Poligono(paiRef, ref _rotulo, ptosEsquerda_1);
      //faceEsquerda_1.ObjetoCor = cor;
      faceEsquerda_1.ObjetoAtualizar();
      faceEsquerda_1.PrimitivaTipo = PrimitiveType.TriangleFan;

      // ---

      List<Ponto4D> ptosDireita_0 = new List<Ponto4D>() {
        new Ponto4D(0.5, -0.5, -0.5),
        new Ponto4D(0.5, 0.5, -0.5),
        new Ponto4D(0.5, -0.5, 0.5),
        new Ponto4D(0.5, 0.5, 0.5)
      };

      Objeto faceDireita_0 = new Poligono(paiRef, ref _rotulo, ptosDireita_0);
      //faceDireita_0.ObjetoCor = cor;
      faceDireita_0.ObjetoAtualizar();
      faceDireita_0.PrimitivaTipo = PrimitiveType.TriangleFan;

      List<Ponto4D> ptosDireita_1 = new List<Ponto4D>() {
        new Ponto4D(0.5, 0.5, 0.5),
        new Ponto4D(0.5, -0.5, 0.5),
        new Ponto4D(0.5, 0.5, -0.5),
        new Ponto4D(0.5, -0.5, -0.5)   
      };

      Objeto faceDireita_1 = new Poligono(paiRef, ref _rotulo, ptosDireita_1);
      //faceDireita_1.ObjetoCor = cor;
      faceDireita_1.ObjetoAtualizar();
      faceDireita_1.PrimitivaTipo = PrimitiveType.TriangleFan;

      // ---

      List<Ponto4D> ptosAcima_0 = new List<Ponto4D>() {
        new Ponto4D(-0.5, 0.5, -0.5),
        new Ponto4D(0.5, 0.5, -0.5),
        new Ponto4D(-0.5, 0.5, 0.5),
        new Ponto4D(0.5, 0.5, 0.5)
      };

      Objeto faceAcima_0 = new Poligono(paiRef, ref _rotulo, ptosAcima_0);
      //faceAcima_0.ObjetoCor = new Cor(255, 145, 255, 255);
      faceAcima_0.ObjetoAtualizar();
      faceAcima_0.PrimitivaTipo = PrimitiveType.TriangleFan;

      List<Ponto4D> ptosAcima_1 = new List<Ponto4D>() {
        new Ponto4D(0.5, 0.5, 0.5),
        new Ponto4D(-0.5, 0.5, 0.5),
        new Ponto4D(0.5, 0.5, -0.5),
        new Ponto4D(-0.5, 0.5, -0.5),       
      };

      Objeto faceAcima_1 = new Poligono(paiRef, ref _rotulo, ptosAcima_1);
      //faceAcima_1.ObjetoCor = new Cor(255, 145, 255, 255);
      faceAcima_1.ObjetoAtualizar();
      faceAcima_1.PrimitivaTipo = PrimitiveType.TriangleFan;

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