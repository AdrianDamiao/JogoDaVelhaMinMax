using System;
using System.Collections.Generic;
using System.Threading;
using JogoDaVelhaConsole.Extensions;

namespace JogoDaVelhaConsole
{
    public class Program
    {
        public static int[,] tabuleiro = new int[3,3];
        public static bool heuristicaOpcional = true;

        public static void Main(string[] args)
        {  
            JogoDaVelha();
        }

        private static void JogoDaVelha()
        {
            ConsoleExtension.ExibeMensagemDeBemVindo();
            
            var configuracao = ConfiguraJogo();
            PreencheArvore(configuracao.noRaiz, configuracao.quemComecaJogando);
            AvaliaMiniMax(configuracao.noRaiz, configuracao.quemComecaJogando, configuracao.dificuldade);
            IniciaJogo(configuracao.noRaiz, configuracao.quemComecaJogando);
        }

        private static (int dificuldade, int quemComecaJogando, No noRaiz) ConfiguraJogo()
        {
            int dificuldade, 
                quemComecaJogando = 1;

            do{
                ConsoleExtension.ExibeEscolhaDeDificuldade();
                dificuldade = int.Parse(Console.ReadLine());
                if(dificuldade < 1 || dificuldade > 9)
                    ConsoleExtension.OpcaoInvalida();
            }while(dificuldade < 1 || dificuldade > 9);
    
            do{
                ConsoleExtension.ExibeEscolhaDeJogador();
                quemComecaJogando = int.Parse(Console.ReadLine());
                if(quemComecaJogando != 1 && quemComecaJogando != 2)
                    ConsoleExtension.OpcaoInvalida();
            }while(quemComecaJogando != 1 && quemComecaJogando != 2);


            ConsoleExtension.ExibeOpcaoDeHeuristica();
            heuristicaOpcional = int.Parse(Console.ReadLine()) == 1 ? true : false;

            No noRaiz = new No();
            noRaiz.Jogo = new int[3, 3];
            quemComecaJogando = quemComecaJogando == 1 ? 1 : -1; 

            return (dificuldade, quemComecaJogando, noRaiz);
        }

        private static void PreencheArvore(No noPai, int quemEstaJogando)
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

        private static int AvaliaMiniMax(No no, int proximoJogador, int dificuldade)
        {
            var ganhador = no.EncontraGanhador();
            if (ganhador != 2 || dificuldade == 0)
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
                        var resultado = AvaliaMiniMax(no.Filhos[i], proximoJogador * (-1), (dificuldade-1));
                        if (resultado < no.ValorMinMax)
                            no.ValorMinMax = resultado;
                    }
                    return no.ValorMinMax;
                }
                else if (proximoJogador == 1)
                {
                    for (int i = 0; i < no.Filhos.Count; i++)
                    {
                        var resultado = AvaliaMiniMax(no.Filhos[i], proximoJogador * (-1), (dificuldade-1));
                        if (resultado > no.ValorMinMax)
                            no.ValorMinMax = resultado;
                    }
                    return no.ValorMinMax;
                }
            }
            return 0;
        }

        private static int IniciaJogo(No no, int proximoJogador)
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
                    if(TabuleiroEstaVazio()) //IA faz jogada aleatória se o tabuleiro estiver vazio
                    {
                        Random random = new Random();
                        int indiceAleatorio = random.Next(no.Filhos.Count);
                        AtualizaTabuleiro(no.Filhos[indiceAleatorio].Jogo);

                        ConsoleExtension.ImprimeTabuleiro(no.Jogo);
                        ConsoleExtension.ExibeJogadaDaMaquina(indiceAleatorio+1);
                        Thread.Sleep(2000);

                        IniciaJogo(no.Filhos[indiceAleatorio], proximoJogador*(-1));
                    } else {
                        int melhorPontuacao = Int32.MinValue;
                        int indiceMelhorFilho = 0;
                        for (int i = 0; i < no.Filhos.Count; i++)
                        {
                            if (no.Filhos[i].ValorMinMax > melhorPontuacao)
                            {
                                melhorPontuacao = no.Filhos[i].ValorMinMax;
                                indiceMelhorFilho = i;
                            }
                        }

                        if(heuristicaOpcional) //Heuristica opcional
                        {
                            int indiceRandomico = RandomizaMelhorFilho(no.Filhos, indiceMelhorFilho);
                            int posicaoRandomicaJogada = LugarJogado(no.Filhos[indiceRandomico].Jogo);
                            AtualizaTabuleiro(no.Filhos[indiceRandomico].Jogo);
                            ConsoleExtension.ExibeJogadaDaMaquina(posicaoRandomicaJogada);
                            ConsoleExtension.ImprimeTabuleiro(no.Jogo);
                            Thread.Sleep(2000);

                            IniciaJogo(no.Filhos[indiceRandomico], proximoJogador*(-1));
                        } else {
                            var posicaoJogada = LugarJogado(no.Filhos[indiceMelhorFilho].Jogo);
                            AtualizaTabuleiro(no.Filhos[indiceMelhorFilho].Jogo);

                            ConsoleExtension.ImprimeTabuleiro(no.Jogo);
                            ConsoleExtension.ExibeJogadaDaMaquina(posicaoJogada);
                            Thread.Sleep(2000);

                            IniciaJogo(no.Filhos[indiceMelhorFilho], proximoJogador*(-1));
                        }
                    }
                }
                else
                {               
                    int filhoCorreto = 0;
                    (int i, int j) coordenada = (0, 0);
                    do {
                        ConsoleExtension.ExibeJogadaHumano();
                        var jogada = int.Parse(Console.ReadLine());
                        coordenada = PosicaoParaCoordenada(jogada);
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

        private static int RandomizaMelhorFilho(List<No> filhos, int indiceMelhorFilho)
        {
            List<int> filhosComMesmaPontuacao = new List<int>();
            for(int i = 0; i < filhos.Count; i++)
            {
                if(filhos[i].ValorMinMax == filhos[indiceMelhorFilho].ValorMinMax)
                {
                    filhosComMesmaPontuacao.Add(i);
                }
            }

            int indiceRandomico = new Random().Next(filhosComMesmaPontuacao.Count);

            return filhosComMesmaPontuacao[indiceRandomico];
        }

        public static bool TabuleiroEstaVazio()
        {
            List<int> posicoesVazias = new List<int>();
            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    if(tabuleiro[i, j] == 0)
                        posicoesVazias.Add(CoordenadaParaPosicao(i, j));
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
                        posicoesVazias.Add(CoordenadaParaPosicao(i, j));
                }
            }
            return posicoesVazias.Count == 0 ? true : false;
        }

        public static (int i, int j) PosicaoParaCoordenada(int jogada)
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

        private static void AtualizaTabuleiro(int[,] jogo)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    tabuleiro[i, j] = jogo[i,j];
                }
            }
        }

        public static int LugarJogado(int[,] jogo)
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

        public static int CoordenadaParaPosicao(int i, int j)
        {
            return (i, j) switch 
            {
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