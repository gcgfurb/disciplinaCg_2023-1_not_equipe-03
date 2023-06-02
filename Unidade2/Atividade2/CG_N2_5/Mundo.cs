#define CG_Gizmo  // debugar gráfico.
#define CG_OpenGL // render OpenGL.
//#define CG_DirectX // render DirectX.
// #define CG_Privado // código do professor.

using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using OpenTK.Mathematics;

//FIXME: padrão Singleton

namespace gcgcg
{
  public class Mundo : GameWindow
  {
    private List<Objeto> objetosLista = new List<Objeto>();
    private Objeto objetoSelecionado = null;
    private char rotulo = '@';

    private readonly float[] _sruEixos =
    {
      -0.5f,  0.0f,  0.0f, /* X- */      0.5f,  0.0f,  0.0f, /* X+ */
       0.0f, -0.5f,  0.0f, /* Y- */      0.0f,  0.5f,  0.0f, /* Y+ */
       0.0f,  0.0f, -0.5f, /* Z- */      0.0f,  0.0f,  0.5f, /* Z+ */
    };

    private int _vertexBufferObject_sruEixos;
    private int _vertexArrayObject_sruEixos;

    private Shader _shaderVermelha;
    private Shader _shaderVerde;
    private Shader _shaderAzul;

    private bool _firstMove = true;
    private Vector2 _lastPos;
    private List<Objeto> CircMenor = new List<Objeto>();
    private List<Objeto> CircMaior = new List<Objeto>();
    private List<Objeto> BBox = new List<Objeto>();
    private List<Objeto> retasBBox = new List<Objeto>();

    public Mundo(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
           : base(gameWindowSettings, nativeWindowSettings)
    {
    }

    private void ObjetoNovo(Objeto objeto, Objeto objetoFilho = null)
    {
      if (objetoFilho == null)
      {
        objetosLista.Add(objeto);
        objeto.Rotulo = rotulo = Utilitario.CharProximo(rotulo);
        objeto.ObjetoAtualizar();
        objetoSelecionado = objeto;
      }
      else
      {
        objeto.FilhoAdicionar(objetoFilho);
        objetoFilho.Rotulo = rotulo = Utilitario.CharProximo(rotulo);
        objetoFilho.ObjetoAtualizar();
        objetoSelecionado = objetoFilho;
      }
    }

    private void criaBBox() {
      // Criando a BBox.
      Ponto4D pontoRetangulo1 = Matematica.GerarPtosCirculo(225, 0.3);
      pontoRetangulo1.X += 0.3;
      pontoRetangulo1.Y += 0.3;
      Ponto4D pontoRetangulo2 = Matematica.GerarPtosCirculo(45, 0.3);
      pontoRetangulo2.X += 0.3;
      pontoRetangulo2.Y += 0.3;
      Retangulo r = new Retangulo(null, pontoRetangulo1, pontoRetangulo2);
      BBox.Add(r);
      ObjetoNovo(r);

      SegReta ret = null;
      for (int i = 0; i < 3; i++) {
          ret = new SegReta(null, r.PontosId(i), r.PontosId(i+1));
          retasBBox.Add(ret);
          ObjetoNovo(ret);
      }

      ret = new SegReta(null, r.PontosId(0), r.PontosId(3));
      retasBBox.Add(ret);
      ObjetoNovo(ret);
    }

    protected override void OnLoad()
    {
      base.OnLoad();

      GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

      // Eixos
      _vertexBufferObject_sruEixos = GL.GenBuffer();
      GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject_sruEixos);
      GL.BufferData(BufferTarget.ArrayBuffer, _sruEixos.Length * sizeof(float), _sruEixos, BufferUsageHint.StaticDraw);
      _vertexArrayObject_sruEixos = GL.GenVertexArray();
      GL.BindVertexArray(_vertexArrayObject_sruEixos);
      GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
      GL.EnableVertexAttribArray(0);
      _shaderVermelha = new Shader("Shaders/shader.vert", "Shaders/shaderVermelha.frag");
      _shaderVerde = new Shader("Shaders/shader.vert", "Shaders/shaderVerde.frag");
      _shaderAzul = new Shader("Shaders/shader.vert", "Shaders/shaderAzul.frag");

      // Objeto objetoNovo = null;

      #region BBox
      // Criando o círculo menor.
      Circulo c_menor = new Circulo(null, 0.1, 0.3, 0.3);
      ObjetoNovo(c_menor);

      Ponto p_c_menor = new Ponto(null, new Ponto4D(0.3, 0.3));
      p_c_menor.PrimitivaTamanho = 10;
      ObjetoNovo(p_c_menor);

      // Guardando objetos para depois.
      CircMenor.Add(c_menor);
      CircMenor.Add(p_c_menor);

      // Criando o círculo maior.
      Circulo c_maior = new Circulo(null, 0.3, 0.3, 0.3);
      ObjetoNovo(c_maior);
      CircMaior.Add(c_maior);

      criaBBox();
      #endregion

#if CG_Privado
      #region Objeto: circulo  
      objetoNovo = new Circulo(null, 0.2, new Ponto4D());
      objetoNovo.shaderCor = new Shader("Shaders/shader.vert", "Shaders/shaderAmarela.frag");
      ObjetoNovo(objetoNovo); objetoNovo = null;
      #endregion

      #region Objeto: SrPalito  
      objetoNovo = new SrPalito(null);
      ObjetoNovo(objetoNovo); objetoNovo = null;
      SrPalito objSrPalito = objetoSelecionado as SrPalito;
      #endregion

      #region Objeto: Spline
      objetoNovo = new Spline(null);
      ObjetoNovo(objetoNovo); objetoNovo = null;
      Spline objSpline = objetoSelecionado as Spline;
      #endregion
#endif

    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
      base.OnRenderFrame(e);

      GL.Clear(ClearBufferMask.ColorBufferBit);

#if CG_Gizmo      
      Sru3D();
#endif
      for (var i = 0; i < objetosLista.Count; i++)
        objetosLista[i].Desenhar();

      SwapBuffers();
    }

    private bool passouCircMaior() {
        Circulo c = (Circulo) CircMaior[0];
        Ponto p = (Ponto) CircMenor[1];

        double dist = 0;
        for (int i = 0; i < 72; i++) {
            dist = Math.Pow(p.PontosId(0).X - c.PontosId(i).X, 2);
            dist += Math.Pow(p.PontosId(0).Y - c.PontosId(i).Y, 2);
            if (dist >= 0.35)
                return true;
        }

        return false;
    }

    private void testeBBox() {
        if (BBox.Count == 0)
            // Se BBox foi destruída, remonta para fazer o teste.
            criaBBox();

        Ponto p = (Ponto) CircMenor[1];
        Retangulo r = (Retangulo) BBox[0];

        if ((p.PontosId(0).X < r.PontosId(0).X) ||
            (p.PontosId(0).Y < r.PontosId(0).Y) ||
            (p.PontosId(0).X > r.PontosId(2).X) ||
            (p.PontosId(0).Y > r.PontosId(2).Y)) {
            // Extrapolou limites da BBox
            foreach (Objeto b in retasBBox) {
                b.PontosAlterar(new Ponto4D(0.3, 0.3), 0);
                b.PontosAlterar(new Ponto4D(0.3, 0.3), 1);
                b.ObjetoAtualizar();
            }

            BBox.Clear();
        }
    }

    private void atualizaCircMenor(char direcao) {
        Circulo c = (Circulo) CircMenor[0];
        Ponto p = (Ponto) CircMenor[1];

        switch(direcao) {
            case 'C':
                c.centroY += 0.01;
                p.PontosId(0).Y += 0.01;
                if (passouCircMaior()) {
                    c.centroY -= 0.01;
                    p.PontosId(0).Y -= 0.01;
                }
                break;
            case 'B':
                c.centroY -= 0.01;
                p.PontosId(0).Y -= 0.01;
                if (passouCircMaior()) {
                    c.centroY += 0.01;
                    p.PontosId(0).Y += 0.01;
                }
                break;
            case 'E':
                c.centroX -= 0.01;
                p.PontosId(0).X -= 0.01;
                if (passouCircMaior()) {
                    c.centroX += 0.01;
                    p.PontosId(0).X += 0.01;
                }
                break;
            case 'D':
                c.centroX += 0.01;
                p.PontosId(0).X += 0.01;
                if (passouCircMaior()) {
                    c.centroX -= 0.01;
                    p.PontosId(0).X -= 0.01;
                }
                break;
        }

        c.desenhaCirc();
        c.ObjetoAtualizar();
        p.ObjetoAtualizar();

        testeBBox();
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
      base.OnUpdateFrame(e);

      #region Teclado
      var input = KeyboardState;
      if (input.IsKeyPressed(Keys.C)) {
          atualizaCircMenor('C');
      } else if (input.IsKeyPressed(Keys.B)) {
          atualizaCircMenor('B');
      } else if (input.IsKeyPressed(Keys.E)) {
          atualizaCircMenor('E');
      } else if (input.IsKeyPressed(Keys.D)) {
          atualizaCircMenor('D');
      }      
      #endregion
    }

    protected override void OnResize(ResizeEventArgs e)
    {
      base.OnResize(e);

      GL.Viewport(0, 0, Size.X, Size.Y);
    }

    protected override void OnUnload()
    {
      foreach (var objeto in objetosLista)
      {
        objeto.OnUnload();
      }

      GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
      GL.BindVertexArray(0);
      GL.UseProgram(0);

      GL.DeleteBuffer(_vertexBufferObject_sruEixos);
      GL.DeleteVertexArray(_vertexArrayObject_sruEixos);

      GL.DeleteProgram(_shaderVermelha.Handle);
      GL.DeleteProgram(_shaderVerde.Handle);
      GL.DeleteProgram(_shaderAzul.Handle);

      base.OnUnload();
    }

#if CG_Gizmo
    private void Sru3D()
    {
#if CG_OpenGL && !CG_DirectX
      GL.BindVertexArray(_vertexArrayObject_sruEixos);
      // EixoX
      _shaderVermelha.Use();
      GL.DrawArrays(PrimitiveType.Lines, 0, 2);
      // EixoY
      _shaderVerde.Use();
      GL.DrawArrays(PrimitiveType.Lines, 2, 2);
      // EixoZ
      _shaderAzul.Use();
      GL.DrawArrays(PrimitiveType.Lines, 4, 2);
#elif CG_DirectX && !CG_OpenGL
      Console.WriteLine(" .. Coloque aqui o seu código em DirectX");
#elif (CG_DirectX && CG_OpenGL) || (!CG_DirectX && !CG_OpenGL)
      Console.WriteLine(" .. ERRO de Render - escolha OpenGL ou DirectX !!");
#endif
    }
#endif    

  }
}
