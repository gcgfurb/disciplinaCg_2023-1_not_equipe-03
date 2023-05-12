using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;

namespace gcgcg
{
  internal class Circulo : Objeto
  {
    public double raio = 0;
    public double centroX = 0;
    public double centroY = 0;

    public Circulo(Objeto paiRef, double raio, double centroX, double centroY) : base(paiRef)
    {
        PrimitivaTipo = PrimitiveType.LineLoop;
        PrimitivaTamanho = 1;
        this.raio = raio;
        this.centroX = centroX;
        this.centroY = centroY;

        desenhaCirc(raio, centroX, centroY);
    }

    public void desenhaCirc() {
        base.pontosLista.Clear();
        for (int i = 0; i < 360; i += 5) {
            Ponto4D ponto = new Ponto4D(Matematica.GerarPtosCirculo(i, raio));
            ponto.X += centroX;
            ponto.Y += centroY;

            PrimitivaTamanho = 5;
            base.PontosAdicionar(ponto);
      }
    }

    public void desenhaCirc(double raio, double centroX, double centroY) {
        base.pontosLista.Clear();
        for (int i = 0; i < 360; i += 5) {
            Ponto4D ponto = new Ponto4D(Matematica.GerarPtosCirculo(i, raio));
            ponto.X += centroX;
            ponto.Y += centroY;

            PrimitivaTamanho = 5;
            base.PontosAdicionar(ponto);
      }
    }
  }
}
