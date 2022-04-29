using System;

namespace JogoDaVelha
{
    class Program
    {
        static void Main(string[] args)
        {
            No noRaiz = new No();

            Console.WriteLine("Quem começará jogando? [1 - Jogador | -1 - Máquina]");
            var quemEstaJogando = int.Parse(Console.ReadLine());

            PreencheArvore(noRaiz, quemEstaJogando);
            AvaliaMiniMax(noRaiz, quemEstaJogando);
        }

        public static void PreencheArvore(No noPai, int quemEstaJogando)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (noPai.Jogo[i, j] == 0)
                    {
                        No filho = new No();
                        filho.CopiaMatriz(noPai);
                        filho.Jogo[i, j] = quemEstaJogando;
                        noPai.Filhos.Add(filho);

                        if (quemEstaJogando == 1)
                        {
                            PreencheArvore(filho, -1);
                        }
                        else
                        {
                            PreencheArvore(filho, 1);
                        }
                    }
                }
            }
        }

        public static void AvaliaMiniMax(No no, int jogador)
        {
            var ganhador = no.EncontraGanhador();
            if (ganhador != 0)
            {
                // Verifica minimax
            }
            else
            {
                if (jogador == -1)
                {
                    // minimizacao
                    for (int i = 0; i < no.Filhos.Count; i++)
                    {
                    }
                }
                else if (jogador == 1)
                {
                    // maximizacao
                    for (int i = 0; i < no.Filhos.Count; i++)
                    {
                    }
                }

            }
        }
    }
}
