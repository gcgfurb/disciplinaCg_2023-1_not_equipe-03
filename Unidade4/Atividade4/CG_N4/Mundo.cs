﻿#define CG_Gizmo  // debugar gráfico.
#define CG_OpenGL // render OpenGL.
// #define CG_DirectX // render DirectX.
// #define CG_Privado // código do professor.

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
    private char rotuloNovo = '?';
    private Objeto objetoSelecionado = null;

    // private readonly float[] _sruEixos =
    // {
      // -0.5f,  0.0f,  0.0f, /* X- */      0.5f,  0.0f,  0.0f, /* X+ */
      //  0.0f, -0.5f,  0.0f, /* Y- */      0.0f,  0.5f,  0.0f, /* Y+ */
      //  0.0f,  0.0f, -0.5f, /* Z- */      0.0f,  0.0f,  0.5f  /* Z+ */
    // };

    // private int _vertexBufferObject_sruEixos;
    // private int _vertexArrayObject_sruEixos;

    private readonly float[] verticesText = {
      //Pontos                 //Normals            //Cood textura
      -1.05f, -1.05f, -1.05f,/*0.0f,  0.0f, -1.0f,*/0.0f,  0.0f,
       1.05f, -1.05f, -1.05f,/*0.0f,  0.0f, -1.0f,*/1.0f,  0.0f,
       1.05f,  1.05f, -1.05f,/*0.0f,  0.0f, -1.0f,*/1.0f,  1.0f,
       1.05f,  1.05f, -1.05f,/*0.0f,  0.0f, -1.0f,*/1.0f,  1.0f,
      -1.05f,  1.05f, -1.05f,/*0.0f,  0.0f, -1.0f,*/0.0f,  1.0f,
      -1.05f, -1.05f, -1.05f,/*0.0f,  0.0f, -1.0f,*/0.0f,  0.0f,

      -1.05f, -1.05f,  1.05f,/*0.0f,  0.0f,  1.0f,*/0.0f,  0.0f,
       1.05f, -1.05f,  1.05f,/*0.0f,  0.0f,  1.0f,*/1.0f,  0.0f,
       1.05f,  1.05f,  1.05f,/*0.0f,  0.0f,  1.0f,*/1.0f,  1.0f,
       1.05f,  1.05f,  1.05f,/*0.0f,  0.0f,  1.0f,*/1.0f,  1.0f,
      -1.05f,  1.05f,  1.05f,/*0.0f,  0.0f,  1.0f,*/0.0f,  1.0f,
      -1.05f, -1.05f,  1.05f,/*0.0f,  0.0f,  1.0f,*/0.0f,  0.0f,

      -1.05f,  1.05f,  1.05f,/*-1.0f,  0.0f,  0.0f,*/1.0f,  0.0f,
      -1.05f,  1.05f, -1.05f,/*-1.0f,  0.0f,  0.0f,*/1.0f,  1.0f,
      -1.05f, -1.05f, -1.05f,/*-1.0f,  0.0f,  0.0f,*/0.0f,  1.0f,
      -1.05f, -1.05f, -1.05f,/*-1.0f,  0.0f,  0.0f,*/0.0f,  1.0f,
      -1.05f, -1.05f,  1.05f,/*-1.0f,  0.0f,  0.0f,*/0.0f,  0.0f,
      -1.05f,  1.05f,  1.05f,/*-1.0f,  0.0f,  0.0f,*/1.0f,  0.0f,

       1.05f,  1.05f,  1.05f,/*1.0f,  0.0f,  0.0f,*/1.0f,  0.0f,
       1.05f,  1.05f, -1.05f,/*1.0f,  0.0f,  0.0f,*/1.0f,  1.0f,
       1.05f, -1.05f, -1.05f,/*1.0f,  0.0f,  0.0f,*/0.0f,  1.0f,
       1.05f, -1.05f, -1.05f,/*1.0f,  0.0f,  0.0f,*/0.0f,  1.0f,
       1.05f, -1.05f,  1.05f,/*1.0f,  0.0f,  0.0f,*/0.0f,  0.0f,
       1.05f,  1.05f,  1.05f,/*1.0f,  0.0f,  0.0f,*/1.0f,  0.0f,

      -1.05f, -1.05f, -1.05f,/*0.0f, -1.0f,  0.0f,*/0.0f,  1.0f,
       1.05f, -1.05f, -1.05f,/*0.0f, -1.0f,  0.0f,*/1.0f,  1.0f,
       1.05f, -1.05f,  1.05f,/*0.0f, -1.0f,  0.0f,*/1.0f,  0.0f,
       1.05f, -1.05f,  1.05f,/*0.0f, -1.0f,  0.0f,*/1.0f,  0.0f,
      -1.05f, -1.05f,  1.05f,/*0.0f, -1.0f,  0.0f,*/0.0f,  0.0f,
      -1.05f, -1.05f, -1.05f,/*0.0f, -1.0f,  0.0f,*/0.0f,  1.0f,

      -1.05f,  1.05f, -1.05f,/*0.0f,  1.0f,  0.0f,*/0.0f,  1.0f,
       1.05f,  1.05f, -1.05f,/*0.0f,  1.0f,  0.0f,*/1.0f,  1.0f,
       1.05f,  1.05f,  1.05f,/*0.0f,  1.0f,  0.0f,*/1.0f,  0.0f,
       1.05f,  1.05f,  1.05f,/*0.0f,  1.0f,  0.0f,*/1.0f,  0.0f,
      -1.05f,  1.05f,  1.05f,/*0.0f,  1.0f,  0.0f,*/0.0f,  0.0f,
      -1.05f,  1.05f, -1.05f,/*0.0f,  1.0f,  0.0f,*/0.0f,  1.0f
    };

    private readonly uint[] indicesText =
    {
        0, 1, 2, 2, 3, 0, // front face
        3, 2, 6, 6, 7, 3, // top face
        7, 6, 5, 5, 4, 7, // back face
        4, 0, 3, 3, 7, 4, // left face
        0, 1, 5, 5, 4, 0, // bottom face  
        1, 5, 6, 6, 2, 1,
    };

    // private int _vertexBufferObject_sruEixos;
    // private int _vertexArrayObject_sruEixos;

    private int _vertexBufferObject;
    private int _vertexArrayObject;

    private Shader _shader;
    private Texture _texture;

    private int _elementBufferObject;

    private Shader _shaderBranca;
    private Shader _shaderVermelha;
    private Shader _shaderVerde;
    private Shader _shaderAzul;
    private Shader _shaderCiano;
    private Shader _shaderMagenta;
    private Shader _shaderAmarela;

    private readonly Vector3 _lightPos = new Vector3(1.2f, 1.0f, 2.0f);
    private Shader _lampShader;
    private Shader _lightingShader;
    private int _vaoLamp;

    private Camera _camera;
    private float deltaX;
    private float deltaY;

    public Mundo(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
           : base(gameWindowSettings, nativeWindowSettings)
    {
      mundo = new Objeto(null, ref rotuloNovo);
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

      GL.Enable(EnableCap.DepthTest);       // Ativar teste de profundidade
      //GL.Enable(EnableCap.CullFace);     // Desenha os dois lados da face
      // GL.FrontFace(FrontFaceDirection.Cw);
      // GL.CullFace(CullFaceMode.FrontAndBack);

      #region Cores
      _shaderBranca = new Shader("Shaders/shader.vert", "Shaders/shaderBranca.frag");
      _shaderVermelha = new Shader("Shaders/shader.vert", "Shaders/shaderVermelha.frag");
      _shaderVerde = new Shader("Shaders/shader.vert", "Shaders/shaderVerde.frag");
      _shaderAzul = new Shader("Shaders/shader.vert", "Shaders/shaderAzul.frag");
      _shaderCiano = new Shader("Shaders/shader.vert", "Shaders/shaderCiano.frag");
      _shaderMagenta = new Shader("Shaders/shader.vert", "Shaders/shaderMagenta.frag");
      _shaderAmarela = new Shader("Shaders/shader.vert", "Shaders/shaderAmarela.frag");
      #endregion

      #region Eixos: SRU  
      // _vertexBufferObject_sruEixos = GL.GenBuffer();
      // GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject_sruEixos);
      // GL.BufferData(BufferTarget.ArrayBuffer, _sruEixos.Length * sizeof(float), _sruEixos, BufferUsageHint.StaticDraw);
      // _vertexArrayObject_sruEixos = GL.GenVertexArray();
      // GL.BindVertexArray(_vertexArrayObject_sruEixos);
      // GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
      // GL.EnableVertexAttribArray(0);
      #endregion

      #region Textura
      GL.Enable(EnableCap.Texture2D);

      _vertexArrayObject = GL.GenVertexArray();
      GL.BindVertexArray(_vertexArrayObject);

      _vertexBufferObject = GL.GenBuffer();
      GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
      GL.BufferData(BufferTarget.ArrayBuffer, verticesText.Length * sizeof(float), verticesText, BufferUsageHint.StaticDraw);

      _elementBufferObject = GL.GenBuffer();
      GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
      GL.BufferData(BufferTarget.ElementArrayBuffer, indicesText.Length * sizeof(uint), indicesText, BufferUsageHint.StaticDraw);

      _shader = new Shader("Shaders/shaderText.vert", "Shaders/shaderText.frag");
      _shader.Use();

      int vertexLocation = _shader.GetAttribLocation("aPosition");
      GL.EnableVertexAttribArray(vertexLocation);
      GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

      int texCoordLocation = _shader.GetAttribLocation("aTexCoord");
      GL.EnableVertexAttribArray(texCoordLocation);
      GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

      _texture = Texture.LoadFromFile("Texture/grupo.png");
      _texture.Use(TextureUnit.Texture0);
      #endregion

      #region Teste Iluminação Basic Lightning
      _lightingShader = new Shader("Shaders/shader.vert", "Shaders/lighting.frag");
      _lampShader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
      #endregion

      #region Objeto: Cubo
      objetoSelecionado = new Cubo(mundo, ref rotuloNovo);
      objetoSelecionado.shaderCor = _shader;
      #endregion

      CursorState = CursorState.Grabbed;

      deltaX = 0;
      deltaY = 0;

      _camera = new Camera(new Vector3(0.0f, 0.0f, 5.0f), Size.X / (float)Size.Y);
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
      base.OnRenderFrame(e);

      GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

      mundo.Desenhar(new Transformacao4D(), _camera);

      GL.BindVertexArray(_vertexArrayObject);

      _lightingShader.Use();

      _lightingShader.SetMatrix4("model", Matrix4.Identity);
      _lightingShader.SetMatrix4("view", _camera.GetViewMatrix());
      _lightingShader.SetMatrix4("projection", _camera.GetProjectionMatrix());

      _lightingShader.SetVector3("objectColor", new Vector3(1.0f, 0.5f, 0.31f));
      _lightingShader.SetVector3("lightColor", new Vector3(1.0f, 1.0f, 1.0f));
      _lightingShader.SetVector3("lightPos", _lightPos);
      _lightingShader.SetVector3("viewPos", _camera.Position);

      _texture.Use(TextureUnit.Texture0);
      _shader.Use();

      GL.DrawElements(PrimitiveType.Triangles, indicesText.Length, DrawElementsType.UnsignedInt, 0);

      GL.BindVertexArray(_vaoLamp);

      _lampShader.Use();

      Matrix4 lampMatrix = Matrix4.CreateScale(0.2f);
      lampMatrix = lampMatrix * Matrix4.CreateTranslation(_lightPos);

      _lampShader.SetMatrix4("model", lampMatrix);
      _lampShader.SetMatrix4("view", _camera.GetViewMatrix());
      _lampShader.SetMatrix4("projection", _camera.GetProjectionMatrix());

      GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

#if CG_Gizmo      
      Gizmo_Sru3D();
#endif
      SwapBuffers();
    }

    protected override void OnMouseWheel(MouseWheelEventArgs e) {
      base.OnMouseWheel(e);
      
      // Zoom da câmera
      _camera.Fov -= e.OffsetY;
    }

    protected override void OnMouseMove(MouseMoveEventArgs e) {
      base.OnMouseMove(e);

      deltaX += e.DeltaX;
      deltaY += e.DeltaY;

      if  (deltaY > 100)
          deltaY = 100;
      else if (deltaY < -100)
          deltaY = -100;

      Ponto4D posXZ = Matematica.GerarPtosCirculo(deltaX, 5.0f);
      Ponto4D posY = Matematica.GerarPtosCirculo(deltaY, 5.0f);
      
      _camera.Position = new Vector3((float) posXZ.X, (float) posY.Y, (float) posXZ.Y);
    }

    protected void basicLightning() {
      //
    }

    protected void lightingMaps() {
      //
    }

    protected void lightCasters_DirectionalLights() {
      //
    }

    protected void lightCasters_PointLights() {
      //
    }

    protected void lightCasters_Spotlight() {
      //
    }

    protected void multipleLights() {
      //
    }

    protected void sem_iluminacao() {
      //
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
      base.OnUpdateFrame(e);

      // ☞ 396c2670-8ce0-4aff-86da-0f58cd8dcfdc   TODO: forma otimizada para teclado.
      #region Teclado
      var input = KeyboardState;
      if  (input.IsKeyDown(Keys.Escape))
          Close();

      // Informações
      if  (input.IsKeyPressed(Keys.G))
          mundo.GrafocenaImprimir("");

      if  (input.IsKeyPressed(Keys.P) && objetoSelecionado != null)
          System.Console.WriteLine(objetoSelecionado.ToString());

      if  (input.IsKeyPressed(Keys.M) && objetoSelecionado != null)
          objetoSelecionado.MatrizImprimir();

      // Movimentação da Câmera
      /*
      const float cameraSpeed = 1.5f;
      if  (input.IsKeyDown(Keys.W))
          _camera.Position += _camera.Front * cameraSpeed * (float)e.Time;

      if  (input.IsKeyDown(Keys.S))
          _camera.Position -= _camera.Front * cameraSpeed * (float)e.Time;
          
      if  (input.IsKeyDown(Keys.A))
          _camera.Position -= _camera.Right * cameraSpeed * (float)e.Time;

      if  (input.IsKeyDown(Keys.D))
          _camera.Position += _camera.Right * cameraSpeed * (float)e.Time;
      */

      if  (input.IsKeyDown(Keys.KeyPad1))
          basicLightning();

      if  (input.IsKeyDown(Keys.KeyPad2))
          lightingMaps();

      if  (input.IsKeyDown(Keys.KeyPad3))
          lightCasters_DirectionalLights();

      if  (input.IsKeyDown(Keys.KeyPad4))
          lightCasters_PointLights();

      if  (input.IsKeyDown(Keys.KeyPad5))
          lightCasters_Spotlight();

      if  (input.IsKeyDown(Keys.KeyPad6))
          multipleLights();

      if  (input.IsKeyDown(Keys.KeyPad0))
          sem_iluminacao();
      #endregion
      
      /*
      #region  Mouse
      float deltaX = MousePosition.X - _lastPos.X;
      float deltaY = MousePosition.Y - _lastPos.Y;
      _lastPos = new Vector2(MousePosition.X, MousePosition.Y);

      _camera.Yaw += deltaX;
      _camera.Pitch -= deltaY;

      if  (_camera.Pitch > 89.0f) {
          _camera.Pitch = 89.0f;
      }
      else if (_camera.Pitch < -89.0f) {
          _camera.Pitch = -89.0f;
      }
      else {
          _camera.Pitch -= deltaX * 0.25f;
      }
      #endregion
      */
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

      // GL.DeleteBuffer(_vertexBufferObject_sruEixos);
      // GL.DeleteVertexArray(_vertexArrayObject_sruEixos);

      GL.DeleteProgram(_shaderBranca.Handle);
      GL.DeleteProgram(_shaderVermelha.Handle);
      GL.DeleteProgram(_shaderVerde.Handle);
      GL.DeleteProgram(_shaderAzul.Handle);
      GL.DeleteProgram(_shaderCiano.Handle);
      GL.DeleteProgram(_shaderMagenta.Handle);
      GL.DeleteProgram(_shaderAmarela.Handle);

      base.OnUnload();
    }

#if CG_Gizmo
    private void Gizmo_Sru3D()
    {
#if CG_OpenGL && !CG_DirectX
      var model = Matrix4.Identity;
      // GL.BindVertexArray(_vertexArrayObject_sruEixos);
      // EixoX
      // _shaderVermelha.SetMatrix4("model", model);
      // _shaderVermelha.SetMatrix4("view", _camera.GetViewMatrix());
      // _shaderVermelha.SetMatrix4("projection", _camera.GetProjectionMatrix());
      // _shaderVermelha.Use();
      // GL.DrawArrays(PrimitiveType.Lines, 0, 2);
      // EixoY
      // _shaderVerde.SetMatrix4("model", model);
      // _shaderVerde.SetMatrix4("view", _camera.GetViewMatrix());
      // _shaderVerde.SetMatrix4("projection", _camera.GetProjectionMatrix());
      // _shaderVerde.Use();
      // GL.DrawArrays(PrimitiveType.Lines, 2, 2);
      // EixoZ
      // _shaderAzul.SetMatrix4("model", model);
      // _shaderAzul.SetMatrix4("view", _camera.GetViewMatrix());
      // _shaderAzul.SetMatrix4("projection", _camera.GetProjectionMatrix());
      // _shaderAzul.Use();
      // GL.DrawArrays(PrimitiveType.Lines, 4, 2);
#elif CG_DirectX && !CG_OpenGL
      Console.WriteLine(" .. Coloque aqui o seu código em DirectX");
#elif (CG_DirectX && CG_OpenGL) || (!CG_DirectX && !CG_OpenGL)
      Console.WriteLine(" .. ERRO de Render - escolha OpenGL ou DirectX !!");
#endif
    }
#endif    

  }
}
