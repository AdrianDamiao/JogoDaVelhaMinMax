using System;

namespace JogoDaVelhaConsole.Extensions
{
    public static class ConsoleExtension
    {
        public static void ExibeJogadaHumano()
        {
            Console.Write("Onde ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("você");
            Console.ResetColor();
            Console.WriteLine(" quer jogar? [1-9]");
        }

        public static void JogadaInvalida()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Posição Inválida");
            Console.ResetColor();
        }

        public static void ErroInesperado()
        {
            Console.WriteLine("Erro Inesperado");
        }

        public static void ExibeMensagemDeBemVindo()
        {
            Console.Clear();
            Console.Beep();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("==================================");
            Console.WriteLine("Bem vindo ao meu Jogo da Velha C#");

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nAs posições do tabuleiro podem ser \ninterpretadas da seguinte forma:\n");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("=============================");
            Console.WriteLine("|                           |");
            Console.Write("|");
            Console.ResetColor();
            Console.Write("         1 | 2 | 3         ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("|");
            Console.Write("|");
            Console.ResetColor();
            Console.Write("         4 | 5 | 6         ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("|");
            Console.Write("|");
            Console.ResetColor();
            Console.Write("         7 | 8 | 9         ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("|");
            Console.WriteLine("|                           |");
            Console.WriteLine("=============================");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\nFeito por Adrian Damião");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Pressione enter para iniciar");
            Console.ReadKey();
            Console.Clear();
        }

        public static void ExibeJogadaDaMaquina(int posicao)
        {
            Console.Write("A");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(" Máquina");
            Console.ResetColor();
            Console.Write(" jogou na posição " + posicao);
        }

        public static void ImprimeTabuleiro(int[,] jogo)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("============================");
            Console.WriteLine("|                          |");
            Console.ResetColor();

            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    if(j == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("|");
                        Console.ResetColor();
                        Console.Write("        " + SubstituiCaracter(jogo[i, j]) + " |");
                    }
                    else if(j == 2)
                    {
                        Console.Write(" " + SubstituiCaracter(jogo[i, j]));
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write(" " + SubstituiCaracter(jogo[i, j]) + " |");
                    }
                }
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("         |");
                Console.ResetColor();
                Console.WriteLine();
                if(i != 2){
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("|");
                    Console.ResetColor();
                    Console.Write("       -----------        ");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("|");
                    Console.ResetColor();
                    Console.WriteLine();
                }
            }
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("|                          |");
            Console.WriteLine("============================");
            Console.ResetColor();
        }

        public static void ExibeEscolhaDeJogador()
        {
            Console.Write("Quem começará jogando? [1 - ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Maquina");
            Console.ResetColor();
            Console.Write(" | 2 - ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Humano");
            Console.ResetColor();
            Console.WriteLine("]");
        }

        public static void AnunciaJogador(int ganhador)
        {
            if(ganhador == 1)
            {
                Console.Write("A ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Máquina");
                Console.ResetColor();
                Console.Write(" ganhou !");
            }
            else if(ganhador == -1)
            {
                Console.Write("O ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Humano");
                Console.ResetColor();
                Console.Write(" ganhou");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Houve um empate");
                Console.ResetColor();
            }
            Console.WriteLine();
        }

        public static void ExibeEscolhaDeDificuldade()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("=================================================");
            Console.ResetColor();
            Console.Write("Qual será a dificuldade? [1 - ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Mais Fácil");
            Console.ResetColor();
            Console.Write(" | 9 - ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Mais Difícil");
            Console.ResetColor();
            Console.WriteLine("]");

        }

        public static void ExibeOpcaoDeHeuristica()
        {
            Console.Write("Deseja utilizar a heuristica adicional? [ 1 - ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Sim");
            Console.ResetColor();
            Console.Write(" | 2 - ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Não");
            Console.ResetColor();
            Console.WriteLine("]");
        }

        public static void OpcaoInvalida()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Opção Inválida");
            Console.ResetColor();
        }

        public static string SubstituiCaracter(int jogador)
        {
            return jogador switch
            {
                1 => "X",
                -1 => "O",
                _ => " "
            };
        }
    }
}