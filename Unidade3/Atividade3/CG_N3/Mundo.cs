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
    
    // Manipulação dos objetos na tela
    private Objeto objetoSelecionado = null;
    private Objeto Bbox = null;
    private List<Objeto> listaPoligonos = new List<Objeto>();
    
    // Propriedades Acessórias.
    private Shader corPadrao;
    private bool criandoObjeto = false;

    // Propriedades p/ manipular grafo de cena.
    private Objeto objPai = null;
    private Objeto objFilho = null;

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
    private Shader _shaderAmarela;
    private Shader _shaderBranca;

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
      _shaderAmarela = new Shader("Shaders/shader.vert", "Shaders/shaderAmarela.frag");
      _shaderBranca = new Shader("Shaders/shader.vert", "Shaders/shaderBranca.frag");
      #endregion

      corPadrao = _shaderVermelha;
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

    #region codigoAtividades
    protected Ponto4D pontoClick() {
        int janelaLargura = Size.X;
        int janelaAltura = Size.Y;

        Ponto4D ponto = new Ponto4D(MousePosition.X, MousePosition.Y);
        ponto = Utilitario.NDC_TelaSRU(janelaLargura, janelaAltura, ponto);

        return ponto;
    }

    // Questão 2
    protected void criaPoligono(Ponto4D novoPonto, bool finaliza = false) {
        if (finaliza) {
            if (objetoSelecionado == null || listaPoligonos.Contains(objetoSelecionado))
                return;

            listaPoligonos.Add(objetoSelecionado);
        } else {
            if (novoPonto == null)
                return;

            if (objetoSelecionado == null)
                objetoSelecionado = new Poligono(mundo, ref rotuloAtual, new List<Ponto4D>() {novoPonto});
            else {
                if (listaPoligonos.Contains(objetoSelecionado)) {
                    // objetoSelecionado.shaderCor = _shaderBranca;
                    objetoSelecionado.ObjetoAtualizar();

                    objetoSelecionado = new Poligono(mundo, ref rotuloAtual, new List<Ponto4D>() {novoPonto});
                } else {
                    objetoSelecionado.PontosAdicionar(novoPonto);
                }
            }

            objetoSelecionado.shaderCor = corPadrao;
            objetoSelecionado.ObjetoAtualizar();
        }
    }

    protected void proximoPoligono() {
        if (listaPoligonos.Count == 0) {
            objetoSelecionado = null;
            return;
        }
        
        // Objeto atual volta com a cor branca.    
        //objetoSelecionado.shaderCor = _shaderBranca;
        //objetoSelecionado.ObjetoAtualizar();

        int nextPos = 0;
        if (objetoSelecionado != null)
            nextPos = (listaPoligonos.IndexOf(objetoSelecionado) + 1) % listaPoligonos.Count;

        objetoSelecionado = listaPoligonos[nextPos];

        objetoSelecionado.shaderCor = corPadrao;
        objetoSelecionado.ObjetoAtualizar();
    }

    // Questão 3
    protected void deletaPoligono() {
        Objeto objAtual = objetoSelecionado;
        // Remove objeto da lista de polígonos criados.
        proximoPoligono();
        listaPoligonos.Remove(objAtual);
        // Some com o polígono da tela.
        objAtual.deletaObjeto();
    }

    // Questão 4
    protected int selecionaVerticeProximo(Ponto4D posMouse) {
        if  (objetoSelecionado == null)
            return -1;

        Poligono poligono = (Poligono) objetoSelecionado;

        int qtdPtosPol = poligono.qtdPontos(), ptoSelecionado = -1;
        double distPto = Int32.MaxValue;

        for (int i = 0; i < qtdPtosPol; i++) {
            if (Matematica.distancia(posMouse, poligono.PontosId(i)) < distPto) {
                ptoSelecionado = i;
                distPto = Matematica.distancia(posMouse, poligono.PontosId(i));
            }
        }

        return ptoSelecionado;
    }

    // Questão 5
    protected void deletaVerticePoligono(Ponto4D posMouse) {
        int id = selecionaVerticeProximo(posMouse);

        if  (id > -1) { 
            Poligono p = (Poligono) objetoSelecionado;
            if (p.qtdPontos() > 1)
                objetoSelecionado.deletaVertice(id);
            else
                deletaPoligono();
        }
    }

    // Questão 9
    protected double xInt(Ponto4D pt1, Ponto4D pt2, Ponto4D ptoSelecao) {
        double t = (ptoSelecao.Y - pt1.Y) / (pt2.Y - pt1.Y);
        double x = pt1.X + (pt2.X - pt1.X) * t;

        return x;
    }

    // Questão 9
    protected bool scanLine(Ponto4D pontoMouse) {
        Poligono P = (Poligono) objetoSelecionado;
        double ptoxInt = 0;

        int paridade = 0;
        for (int i = 0; i < P.qtdPontos() - 1; i++) {
            if (P.PontosId(i).Y != P.PontosId(i + 1).Y) {
                ptoxInt = xInt(P.PontosId(i), P.PontosId(i+1), pontoMouse);
                if  (ptoxInt == pontoMouse.X)
                    return true;
                else {
                    if  ((ptoxInt > pontoMouse.X) && (pontoMouse.Y > Math.Min(P.PontosId(i).Y, P.PontosId(i+1).Y)) && (pontoMouse.Y <= Math.Max(P.PontosId(i).Y, P.PontosId(i+1).Y)))
                        paridade += 1;
                }
            } else {
                if  ((pontoMouse.Y == P.PontosId(i).Y) && (pontoMouse.X >= Math.Min(P.PontosId(i).X, P.PontosId(i+1).X)) && (pontoMouse.X <= Math.Max(P.PontosId(i).X, P.PontosId(i+1).X)))
                    return true;
            }
        }

        if  (paridade % 2 == 0)
            return false;
        else
            return true;
    }

    // Questão 9
    protected void selecionaPoligonoBbox(Ponto4D posMouse) {
        if  (Bbox != null)
            Bbox.deletaObjeto();

        foreach (Objeto poligono in listaPoligonos) {
            objetoSelecionado = poligono;
            if (Matematica.Dentro(poligono.Bbox(), posMouse)) {
                if (scanLine(posMouse)) {
                    // Desenha BBox
                    Ponto4D ptoInfEsq = new Ponto4D(poligono.Bbox().obterMenorX, poligono.Bbox().obterMenorY);
                    Ponto4D ptoSupDir = new Ponto4D(poligono.Bbox().obterMaiorX, poligono.Bbox().obterMaiorY);

                    Bbox = new Retangulo(mundo, ref rotuloAtual, ptoInfEsq, ptoSupDir);
                    Bbox.PrimitivaTipo = PrimitiveType.LineLoop;
                    Bbox.shaderCor = _shaderAmarela;
                    Bbox.ObjetoAtualizar();
                    break;
                }
            }
        }

        if  (Bbox == null)
            objetoSelecionado = null;
    }
    #endregion

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
      base.OnUpdateFrame(e);
      // Pega localização do click na tela.
      Ponto4D pontoMouse = null;

      // TODO: Questão 14.
      // TODO: Ajuste movimentação vértice quando tem transformação.
      // TODO: Criar método p/ saber qual polígono está selecionado.
      // TODO: Polir código se possível.

      #region Mouse
      var mouse = MouseState;
      // Questão 2
      if (mouse.IsButtonPressed(MouseButton.Left)) {
          pontoMouse = pontoClick();      
          // Cria novo ponto para o polígono.
          criaPoligono(pontoMouse);
          criandoObjeto = true;
      }
      
      // Questão 4
      if (mouse.IsButtonDown(MouseButton.Right) && (objetoSelecionado != null)) {
          pontoMouse = pontoClick();

          // Seleciona qual o vértice mais próximo no polígono atual.
          int id = selecionaVerticeProximo(pontoMouse);
          if  (id > -1) {
              objetoSelecionado.PontosAlterar(pontoMouse, id);
              objetoSelecionado.ObjetoAtualizar();
          }
      }
      #endregion

      #region Teclado
      var teclado = KeyboardState;
      // Questão 2
      if (teclado.IsKeyPressed(Keys.Enter)) {
          // Finaliza criação do polígono.
          criaPoligono(null, true);
          criandoObjeto = false;
      }
      
      // Questão 3
      if (teclado.IsKeyPressed(Keys.D))
          deletaPoligono();

      if (teclado.IsKeyPressed(Keys.Space) && !criandoObjeto)
          proximoPoligono();

      // Questão 5
      if (teclado.IsKeyPressed(Keys.E)) {
          pontoMouse = pontoClick();
          deletaVerticePoligono(pontoMouse);
      }

      // Questão 7
      if (teclado.IsKeyPressed(Keys.P) && objetoSelecionado != null) {
          if (objetoSelecionado.PrimitivaTipo == PrimitiveType.LineLoop)
              objetoSelecionado.PrimitivaTipo = PrimitiveType.LineStrip;
          else
              objetoSelecionado.PrimitivaTipo = PrimitiveType.LineLoop;

          objetoSelecionado.ObjetoAtualizar();
      }
      
      // Questão 8
      if (teclado.IsKeyPressed(Keys.R)) {
          corPadrao = _shaderVermelha;
          if  (objetoSelecionado != null) {
              objetoSelecionado.shaderCor = corPadrao;
              objetoSelecionado.ObjetoAtualizar();
          }
      }

      // Questão 8
      if (teclado.IsKeyPressed(Keys.G)) {
          corPadrao = _shaderVerde;
          if  (objetoSelecionado != null) {
              objetoSelecionado.shaderCor = corPadrao;
              objetoSelecionado.ObjetoAtualizar();
          }
      }

      // Questão 8
      if (teclado.IsKeyPressed(Keys.B)) {
          corPadrao = _shaderAzul;
          if  (objetoSelecionado != null) {
              objetoSelecionado.shaderCor = corPadrao;
              objetoSelecionado.ObjetoAtualizar();
          }
      }

      // Questão 9
      if (teclado.IsKeyPressed(Keys.S)) {
          pontoMouse = pontoClick();
          selecionaPoligonoBbox(pontoMouse);
      }

      if (teclado.IsKeyPressed(Keys.I) && objetoSelecionado != null)
          objetoSelecionado.MatrizImprimir();

      // Questão 10 - TRANSLAÇÃO
      
      if (teclado.IsKeyPressed(Keys.Up))
          objetoSelecionado.MatrizTranslacaoXYZ(0.0, 0.1, 0.1);

      if (teclado.IsKeyPressed(Keys.Left))
          objetoSelecionado.MatrizTranslacaoXYZ(-0.1, 0, -0.1);

      if (teclado.IsKeyPressed(Keys.Right))
          objetoSelecionado.MatrizTranslacaoXYZ(0.1, 0, 0.1);
    
      if (teclado.IsKeyPressed(Keys.Down))
          objetoSelecionado.MatrizTranslacaoXYZ(0, -0.1, -0.1);

      // Questão 11 - ESCALA
      
      if (teclado.IsKeyPressed(Keys.Home))
          objetoSelecionado.MatrizEscalaXYZBBox(0.5, 0.5, 0.5);

      if (teclado.IsKeyPressed(Keys.End))
          objetoSelecionado.MatrizEscalaXYZBBox(2, 2, 2);

      // Questão 12 - ROTAÇÃO

      if (teclado.IsKeyPressed(Keys.KeyPad3))
          objetoSelecionado.MatrizRotacaoZBBox(5);

      if (teclado.IsKeyPressed(Keys.KeyPad4))
          objetoSelecionado.MatrizRotacaoZBBox(-5);

      // Questão 13
      if  (teclado.IsKeyPressed(Keys.F)) {
           // Primeira vez: Seleciona objeto pai.
           if  (objPai == null)
               objPai = objetoSelecionado;
           else {
               if  (objetoSelecionado == objPai)
                   // Objeto selecionado é o mesmo de antes, desfaz tudo.
                   objPai = null;
               else {
                   objPai.FilhoAdicionar(objetoSelecionado);
               }
           }
      }

      if  (teclado.IsKeyPressed(Keys.C))
          objetoSelecionado.GrafocenaImprimir("Poligono");
  
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
