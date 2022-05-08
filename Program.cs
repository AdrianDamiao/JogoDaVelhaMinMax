using System;
using System.Collections.Generic;
using System.Threading;
using JogoDaVelhaConsole.Extensions;

namespace JogoDaVelhaConsole
{
    public class Program
    {
        public static int[,] tabuleiro = new int[3,3];

        public static void Main(string[] args)
        {  
            JogoDaVelha();
        }

        public static void JogoDaVelha()
        {
            ConsoleExtension.ExibeMensagemDeBemVindo();
            ConsoleExtension.ExibeEscolhaDeJogador();

            No noRaiz = new No();
            noRaiz.Jogo = new int[3, 3];
            var quemEstaJogando = int.Parse(Console.ReadLine());
            quemEstaJogando = quemEstaJogando == 1 ? 1 : -1; 

            PreencheArvore(noRaiz, quemEstaJogando);
            AvaliaMiniMax(noRaiz, quemEstaJogando);
            IniciaJogo(noRaiz, quemEstaJogando);
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
                if (ganhador == 1)
                {
                    return 1;
                }
                else if (ganhador == -1)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                if (proximoJogador == -1)
                {
                    for (int i = 0; i < no.Filhos.Count; i++)
                    {
                        var resultado = AvaliaMiniMax(no.Filhos[i], proximoJogador * (-1));
                        if (resultado < no.ValorMinMax)
                            no.ValorMinMax = resultado;
                    }
                    return no.ValorMinMax;
                }
                else if (proximoJogador == 1)
                {
                    for (int i = 0; i < no.Filhos.Count; i++)
                    {
                        var resultado = AvaliaMiniMax(no.Filhos[i], proximoJogador * (-1));
                        if (resultado > no.ValorMinMax)
                            no.ValorMinMax = resultado;
                    }
                    return no.ValorMinMax;
                }
            }
            return 0;
        }

        public static int IniciaJogo(No no, int proximoJogador)
        {
            AtualizaTabuleiro(no.Jogo);
            ConsoleExtension.ImprimeTabuleiro(no.Jogo);

            var ganhador = no.EncontraGanhador();
            if (ganhador != 2 || TabuleiroEstaCompleto(tabuleiro))
            {
                ConsoleExtension.AnunciaJogador(ganhador);                
            } else {
                if (proximoJogador == 1)
                {    
                    if(TabuleiroEstaVazio())
                    {
                        Random random = new Random();
                        int indiceAleatorio = random.Next(no.Filhos.Count);
                        Console.WriteLine(indiceAleatorio);
                        AtualizaTabuleiro(no.Filhos[indiceAleatorio].Jogo);

                        ConsoleExtension.ImprimeTabuleiro(no.Jogo);
                        ConsoleExtension.ExibeJogadaDaMaquina(indiceAleatorio+1);
                        Thread.Sleep(2000);

                        IniciaJogo(no.Filhos[indiceAleatorio], proximoJogador*(-1));
                    } else {
                        int melhorPontuacao = Int32.MinValue;
                        int melhorFilho = 0;
                        for (int i = 0; i < no.Filhos.Count; i++)
                        {
                            if (no.Filhos[i].ValorMinMax > melhorPontuacao)
                            {
                                melhorPontuacao = no.Filhos[i].ValorMinMax;
                                melhorFilho = i;
                            }
                        }

                        var posicaoJogada= CoordenadaParaPosicao(no.Filhos[melhorFilho].Jogo);
                        AtualizaTabuleiro(no.Filhos[melhorFilho].Jogo);

                        ConsoleExtension.ImprimeTabuleiro(no.Jogo);
                        ConsoleExtension.ExibeJogadaDaMaquina(posicaoJogada);
                        Thread.Sleep(2000);

                        IniciaJogo(no.Filhos[melhorFilho], proximoJogador*(-1));
                    }
                }
                else
                {               
                    int filhoCorreto = 0;
                    (int i, int j) coordenada = (0, 0);
                    do {
                        ConsoleExtension.ExibeJogadaHumano();
                        var jogada = int.Parse(Console.ReadLine());
                        coordenada = TraduzPosicao(jogada, no.Jogo);
                        if(no.Jogo[coordenada.i, coordenada.j] != 0)
                        {
                            ConsoleExtension.JogadaInvalida();
                        }
                    }while(no.Jogo[coordenada.i, coordenada.j] != 0);

                    if(no.Jogo[coordenada.i, coordenada.j] == 0)
                    {
                        no.Jogo[coordenada.i, coordenada.j] = proximoJogador;
                        AtualizaTabuleiro(no.Jogo);

                        foreach (var (filho, index) in no.Filhos.LoopIndex())
                        {
                            if (ComparaTabuleiro(filho.Jogo))
                            {
                                filhoCorreto = index;
                                Console.WriteLine(filhoCorreto);
                            }
                        }
                        IniciaJogo(no.Filhos[filhoCorreto], proximoJogador * (-1));
                    } else {
                        ConsoleExtension.ErroInesperado();
                    }
                } 
            }
            return 0;
        }

        public static bool TabuleiroEstaVazio()
        {
            List<int> posicoesVazias = new List<int>();
            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    if(tabuleiro[i, j] == 0)
                        posicoesVazias.Add(CoordenadaParaPosicao(tabuleiro));
                }
            }
            return posicoesVazias.Count == 9 ? true : false;
        }

        public static bool TabuleiroEstaCompleto(int[,] tabuleiro)
        {
            List<int> posicoesVazias = new List<int>();
            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    if(tabuleiro[i, j] == 0)
                        posicoesVazias.Add(CoordenadaParaPosicao(tabuleiro));
                }
            }
            return posicoesVazias.Count == 0 ? true : false;
        }

        public static (int i, int j) TraduzPosicao(int jogada, int[,] jogo)
        {
            return jogada switch 
            {
                1 => (0, 0),
                2 => (0, 1),
                3 => (0, 2),
                4 => (1, 0),
                5 => (1, 1),
                6 => (1, 2),
                7 => (2, 0),
                8 => (2, 1),
                9 => (2, 2),
                _ => (0, 0),
            };
        }

        public static bool ComparaTabuleiro(int[,] jogoDoNo)
        {
            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    if(jogoDoNo[i, j] != tabuleiro[i, j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static void AtualizaTabuleiro(int[,] jogo)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    tabuleiro[i, j] = jogo[i,j];
                }
            }
        }

        public static int CoordenadaParaPosicao(int[,] jogo)
        {
            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    if(jogo[i, j] != tabuleiro[i, j])
                    {
                        return (i, j) switch {
                            (0, 0) => 1,
                            (0, 1) => 2,
                            (0, 2) => 3,
                            (1, 0) => 4,
                            (1, 1) => 5,
                            (1, 2) => 6,
                            (2, 0) => 7,
                            (2, 1) => 8,
                            (2, 2) => 9,
                            _ => 0,
                        };
                    }
                }
            }
            return 0;
        }        
    }
}