using System;

namespace JogoDaVelha
{
    class Program
    {
        static void Main(string[] args)
        {
            No noRaiz = new No();
            noRaiz.Jogo = new int[3,3]{
                {1, -1, 1},
                {-1, 1, -1},
                {-1, 0, 0}
            };

            Console.WriteLine("Quem começará jogando? [1 - Máquina | -1 - Jogador]");
            var quemEstaJogando = int.Parse(Console.ReadLine());

            PreencheArvore(noRaiz, quemEstaJogando);
            AvaliaMiniMax(noRaiz, quemEstaJogando);

            // Resultado
            switch(noRaiz.ValorMinMax)
            {
                case 0: Console.WriteLine("Empate");
                        break;
                case 1: Console.WriteLine("Maquina");
                        break;
                case -1: Console.WriteLine("humano");
                        break;
                default: Console.WriteLine("Bugou");
                        break;
            }
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

                        //ListaMatriz(filho.Jogo);
                        if (quemEstaJogando == 1)
                        {
                            filho.ValorMinMax = int.MaxValue;
                            PreencheArvore(filho, -1);
                        }
                        else
                        {
                            filho.ValorMinMax = int.MinValue;
                            PreencheArvore(filho, 1);
                        }
                    }
                }
            }
        }
        
        public static int AvaliaMiniMax(No no, int proximoJogador)
        {
            var ganhador = no.EncontraGanhador();
            if (ganhador != 2)
            {
                // Verificar o minimax
                if(ganhador == 1) {
                    return 1;
                } else if(ganhador == -1) {
                    return -1;
                } else {
                    return 0;
                }
            }
            else
            {
                if (proximoJogador == -1)
                {
                    // Se é minimização
                    for (int i = 0; i < no.Filhos.Count; i++)
                    {
                        var resultado = AvaliaMiniMax(no.Filhos[i], proximoJogador*(-1));
                        if(resultado < no.ValorMinMax)
                            no.ValorMinMax = resultado;
                    }
                    return no.ValorMinMax;
                }
                else if (proximoJogador == 1)
                {
                    // Se é maximização
                    for (int i = 0; i < no.Filhos.Count; i++)
                    {
                        var resultado = AvaliaMiniMax(no.Filhos[i], proximoJogador*(-1));
                        if(resultado > no.ValorMinMax)
                            no.ValorMinMax = resultado;
                    }
                    return no.ValorMinMax;
                }

            }
            return 0;
        }

        public static void ListaMatriz(int [,] jogo)
        {
            System.Console.WriteLine("Jogo no momento\n");
            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    System.Console.Write("| "+ jogo[i, j] + " |");
                }
                System.Console.WriteLine("");
            }
        }        
    }
}
