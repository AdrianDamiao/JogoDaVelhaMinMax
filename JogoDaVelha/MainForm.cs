using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JogoDaVelha.Engine;

namespace JogoDaVelha
{
    public partial class MainForm : Form
    {
        public static int jogador = 0;
        public MainForm()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            button.Text = button.Name;
            MessageBox.Show("Olá mundo");
        }
        private void buttonStart_Click(object sender, EventArgs e)
        {
            IniciaJogo();
        }

        private void IniciaJogo()
        {
            No noRaiz = new No();
            noRaiz.Jogo = new int[3, 3];
            MarcaTabuleiro(noRaiz.Jogo, 1);

            Console.WriteLine("Quem começará jogando? [1 - Máquina | -1 - Jogador]");
            var quemEstaJogando = 1;

            PreencheArvore(noRaiz, quemEstaJogando);
            AvaliaMiniMax(noRaiz, quemEstaJogando);

            // Resultado
            switch (noRaiz.ValorMinMax)
            {
                case 0:
                    Console.WriteLine("Empate");
                    break;
                case 1:
                    Console.WriteLine("Maquina");
                    break;
                case -1:
                    Console.WriteLine("humano");
                    break;
                default:
                    Console.WriteLine("Bugou");
                    break;
            }
        }

        public static int contador = 0;

        public void MarcaTabuleiro(int[,] jogo, int jogadorSimbolo)
        {
            contador++;
            buttonRestart.Text = contador.ToString();
            SoundPlayer simpleSound2 = new SoundPlayer(@"c:\Windows\Media\chimes.wav");
            simpleSound2.Play();
            DialogResult dlgResult = MessageBox.Show("É a vez do " + SubstituiCaracter(jogadorSimbolo) + " jogar", "Vez de quem?");
            if (dlgResult == DialogResult.OK)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (i == 0 && j == 0)
                            this.button1.Text = SubstituiCaracter(jogo[i, j]);
                        else if (i == 0 && j == 1)
                            this.button2.Text = SubstituiCaracter(jogo[i, j]);
                        else if (i == 0 && j == 2)
                            this.button3.Text = SubstituiCaracter(jogo[i, j]);
                        else if (i == 1 && j == 0)
                            this.button4.Text = SubstituiCaracter(jogo[i, j]);
                        else if (i == 1 && j == 1)
                            this.button5.Text = SubstituiCaracter(jogo[i, j]);
                        else if (i == 1 && j == 2)
                            this.button6.Text = SubstituiCaracter(jogo[i, j]);
                        else if (i == 2 && j == 0)
                            this.button7.Text = SubstituiCaracter(jogo[i, j]);
                        else if (i == 2 && j == 1)
                            this.button8.Text = SubstituiCaracter(jogo[i, j]);
                        else if (i == 2 && j == 2)
                            this.button9.Text = SubstituiCaracter(jogo[i, j]);
                    }
                }
            }
        }

        public void PreencheArvore(No noPai, int quemEstaJogando)
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

        public string SubstituiCaracter(int numero)
        {
            return numero switch
            {
                1 => "X",
                -1 => "O",
                _ => ""
            };
        }

        public int AvaliaMiniMax(No no, int proximoJogador)
        {
            MarcaTabuleiro(no.Jogo, proximoJogador);
            var ganhador = no.EncontraGanhador();
            ExibeGanhador(ganhador);
            if (ganhador != 2)
            {
                // Verificar o minimax
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
                    // Se é minimização
                    for (int i = 0; i < no.Filhos.Count; i++)
                    {
                        var resultado = AvaliaMiniMax(no.Filhos[i], proximoJogador * (-1));
                        if (resultado > no.ValorMinMax)
                            no.ValorMinMax = resultado;
                    }
                    return no.ValorMinMax;
                }
                else if (proximoJogador == 1)
                {
                    // Se é maximização
                    for (int i = 0; i < no.Filhos.Count; i++)
                    {
                        var resultado = AvaliaMiniMax(no.Filhos[i], proximoJogador * (-1));
                        if (resultado < no.ValorMinMax)
                            no.ValorMinMax = resultado;
                    }
                    return no.ValorMinMax;
                }

            }
            return 0;
        }

        public void ExibeGanhador(int ganhador)
        {
            string rootLocation = typeof(Program).Assembly.Location;
            // appending sound location
            SoundPlayer simpleSound2 = new SoundPlayer(@"c:\Users\adria\source\repos\JogoDaVelha\JogoDaVelha\Sounds\win.wav");
            simpleSound2.Play();
            DialogResult dlgResult = MessageBox.Show("O " + SubstituiCaracter(ganhador) + "ganhou! Parabens", "Fim de Jogo");
        }
    }
}
