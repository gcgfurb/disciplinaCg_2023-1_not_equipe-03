using System.Collections.Generic;
using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;

namespace gcgcg {
    internal class Spline: Objeto {
        public List<Ponto4D> pontosControle = new List<Ponto4D>();
        public int qtdPontos = 0;

        public Spline(Objeto paiRef, List<Ponto4D> pontosControle, int qtdPontos): base(paiRef) {
            this.PrimitivaTipo = PrimitiveType.LineStrip;
            this.pontosControle = pontosControle;
            this.qtdPontos = qtdPontos;
        }

        private Ponto4D splineInter(Ponto4D p1, Ponto4D p2, double t) {
            Ponto4D ponto = null;
            double x = 0;
            double y = 0;

            x = p1.X + (p2.X - p1.X) * t / qtdPontos;
            y = p1.Y + (p2.Y - p1.Y) * t / qtdPontos;

            ponto = new Ponto4D(x, y);

            return ponto;       
        }

        public void desenhaSpline() {
            this.pontosLista.Clear();

            Ponto4D p1p2 = null;
            Ponto4D p2p3 = null;
            Ponto4D p3p4 = null;

            Ponto4D p1p2p3 = null;
            Ponto4D p2p3p4 = null;

            Ponto4D p1p2p3p4 = null;
            for (int t = 0; t <= qtdPontos; t++) {
                p1p2 = splineInter(pontosControle[0], pontosControle[1], t);
                p2p3 = splineInter(pontosControle[1], pontosControle[2], t);
                p3p4 = splineInter(pontosControle[2], pontosControle[3], t);

                p1p2p3 = splineInter(p1p2, p2p3, t);
                p2p3p4 = splineInter(p2p3, p3p4, t);

                p1p2p3p4 = splineInter(p1p2p3, p2p3p4, t);

                base.PontosAdicionar(p1p2p3p4);
            }
        }
    }
}