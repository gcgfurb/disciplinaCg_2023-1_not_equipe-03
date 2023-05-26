#define CG_Gizmo  // debugar gráfico.
#define CG_OpenGL // render OpenGL.
// #define CG_DirectX // render DirectX.
#define CG_Privado // código do professor.

using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using System;
using OpenTK.Mathematics;
using System.Collections.Generic;

//FIXME: padrão Singleton

namespace gcgcg
{
  public class Mundo : GameWindow
  {
    Objeto mundo;
    private char rotuloAtual = '?';
    private Objeto objetoSelecionado = null;
    private List<Objeto> listaPoligonos = new List<Objeto>();

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

    public Mundo(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
           : base(gameWindowSettings, nativeWindowSettings)
    {
      mundo = new Objeto(null, ref rotuloAtual);
    }

    private void Diretivas()
    {
#if DEBUG
      Console.WriteLine("Debug version");
#endif      
#if RELEASE
    Console.WriteLine("Release version");
#endif      
#if CG_Gizmo      
      Console.WriteLine("#define CG_Gizmo  // debugar gráfico.");
#endif
#if CG_OpenGL      
      Console.WriteLine("#define CG_OpenGL // render OpenGL.");
#endif
#if CG_DirectX      
      Console.WriteLine("#define CG_DirectX // render DirectX.");
#endif
#if CG_Privado      
      Console.WriteLine("#define CG_Privado // código do professor.");
#endif
      Console.WriteLine("__________________________________ \n");
    }

    protected override void OnLoad()
    {
      base.OnLoad();

      Diretivas();

      GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

      #region Eixos: SRU  
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
      #endregion

      // #region Objeto: polígono qualquer  
      // List<Ponto4D> pontosPoligono = new List<Ponto4D>();
      // pontosPoligono.Add(new Ponto4D(0.25, 0.25));
      // pontosPoligono.Add(new Ponto4D(0.75, 0.25));
      // pontosPoligono.Add(new Ponto4D(0.75, 0.75));
      // pontosPoligono.Add(new Ponto4D(0.50, 0.50));
      // pontosPoligono.Add(new Ponto4D(0.25, 0.75));
      // objetoSelecionado = new Poligono(mundo, ref rotuloAtual, pontosPoligono);
      // #endregion
      // #region NÃO USAR: declara um objeto filho ao polígono
      // objetoSelecionado = new Ponto(objetoSelecionado, ref rotuloAtual, new Ponto4D(0.50, 0.75));
      // objetoSelecionado.ToString();
      // #endregion

      // #region Objeto: retângulo  
      // objetoSelecionado = new Retangulo(mundo, ref rotuloAtual, new Ponto4D(-0.25, 0.25), new Ponto4D(-0.75, 0.75));
      // objetoSelecionado.PrimitivaTipo = PrimitiveType.LineLoop;
      // #endregion

      #region Objeto: segmento de reta  
      // objetoSelecionado = new SegReta(mundo, ref rotuloAtual, new Ponto4D(-0.5, -0.5), new Ponto4D());
      #endregion

      // #region Objeto: ponto  
      // objetoSelecionado = new Ponto(mundo, ref rotuloAtual, new Ponto4D(-0.25, -0.25));
      // objetoSelecionado.PrimitivaTipo = PrimitiveType.Points;
      // objetoSelecionado.PrimitivaTamanho = 5; // FIXME: não está mudando o tamanho
      // #endregion

#if CG_Privado
      // #region Objeto: circulo  
      // objetoSelecionado = new Circulo(mundo, ref rotuloAtual, 0.2, new Ponto4D());
      // objetoSelecionado.shaderCor = new Shader("Shaders/shader.vert", "Shaders/shaderAmarela.frag");
      // #endregion

      // #region Objeto: SrPalito  
      // objetoSelecionado = new SrPalito(mundo, ref rotuloAtual);
      // #endregion

      // #region Objeto: Spline
      // objetoSelecionado = new Spline(mundo, ref rotuloAtual);
      // #endregion
#endif

    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
      base.OnRenderFrame(e);

      GL.Clear(ClearBufferMask.ColorBufferBit);

#if CG_Gizmo      
      Sru3D();
#endif
      mundo.Desenhar();
      SwapBuffers();
    }

    protected Ponto4D pontoFormatado(Vector2 posMouse, Vector2i janela) {
        float x, y;

        // Solução p/ as posições do mouse encontrada em:
        //     https://community.khronos.org/t/opentk-drawing-points-by-mouse-click/75583/4

        // Retorna um valor entre -1 e 1.
        // Parece que o cálculo tenta ajustar as coordenadas de MouseState
        // para essa escala.

        // TODO: Pedir explicação
        x = (posMouse.X - janela.X / 2f) / (janela.X / 2f);
        y = -(posMouse.Y - janela.Y / 2f) / (janela.Y / 2f);

        return new Ponto4D(x, y);
    }

    protected void manipulaPoligono(Ponto4D novoPonto) {
        if (objetoSelecionado == null) {
            List<Ponto4D> primeiroPto = new List<Ponto4D>();
            primeiroPto.Add(novoPonto);
            objetoSelecionado = new Poligono(mundo, ref rotuloAtual, primeiroPto);
        }

        if (novoPonto == null) {
            // Pressionou ENTER, finaliza a criação do polígono.
            listaPoligonos.Add(objetoSelecionado);
            objetoSelecionado = null;
        } else {      
            objetoSelecionado.PontosAdicionar(novoPonto);
            objetoSelecionado.ObjetoAtualizar();
        }
            
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
      base.OnUpdateFrame(e);

      // ☞ 396c2670-8ce0-4aff-86da-0f58cd8dcfdc
      #region Teclado
      var teclado = KeyboardState;

      if (teclado.IsKeyPressed(Keys.Enter))
          manipulaPoligono(null);
      #endregion

      #region  Mouse
      var mouse = MouseState;

      Ponto4D novoPonto = null;
      if (mouse.IsButtonPressed(MouseButton.Left)) {          
          novoPonto = pontoFormatado(mouse.Position, this.ClientRectangle.Size);
          manipulaPoligono(novoPonto);
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
      mundo.OnUnload();

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
      var transform = Matrix4.Identity;
      GL.BindVertexArray(_vertexArrayObject_sruEixos);
      // EixoX
      _shaderVermelha.SetMatrix4("transform", transform);
      _shaderVermelha.Use();
      GL.DrawArrays(PrimitiveType.Lines, 0, 2);
      // EixoY
      _shaderVerde.SetMatrix4("transform", transform);
      _shaderVerde.Use();
      GL.DrawArrays(PrimitiveType.Lines, 2, 2);
      // EixoZ
      _shaderAzul.SetMatrix4("transform", transform);
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
