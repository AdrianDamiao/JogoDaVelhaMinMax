using System.Collections.Generic;

namespace JogoDaVelhaConsole
{
    public class No
    {
        public int[,] Jogo { get; set; }
        public int ValorMinMax { get; set; }
        public List<No> Filhos { get; set; }

        public No()
        {
            Jogo = new int[3, 3];
            Filhos = new List<No>();
        }

        public void CopiaMatriz(No noPai)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Jogo[i, j] = noPai.Jogo[i, j];
                }
            }
        }

        public int EncontraGanhador()
        {
            for (int i = 0; i < 3; i++)
            {
                if (Jogo[i, 0] == Jogo[i, 1] && Jogo[i, 1] == Jogo[i, 2] && Jogo[i, 0] != 0)
                    return Jogo[i, 0];
            }

            for (int j = 0; j < 3; j++)
            {
                if (Jogo[0, j] == Jogo[1, j] && Jogo[1, j] == Jogo[2, j] && Jogo[0, j] != 0)
                    return Jogo[0, j];
            }

            if (Jogo[0, 0] == Jogo[1, 1] && Jogo[1, 1] == Jogo[2, 2] && Jogo[0, 0] != 0)
            {
                return Jogo[0, 0];
            }

            if (Jogo[2, 0] == Jogo[1, 1] && Jogo[1, 1] == Jogo[0, 2] && Jogo[2, 0] != 0)
            {
                return Jogo[2, 0];
            }

            if(Jogo[0, 0] == Jogo[0, 2] && Jogo[0, 2] == Jogo[2, 0] && Jogo[2, 0] == Jogo[2, 2] && Jogo[0, 0] != 0)
            {
                return Jogo[0, 0];
            }

            return 2; // ninguem ganhou
        }
    }
}