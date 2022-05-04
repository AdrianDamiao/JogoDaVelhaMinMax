using System;
using System.Linq;
using System.Media;
using System.Windows.Forms;
using JogoDaVelha.Engine;
using JogoDaVelha.Forms;

namespace JogoDaVelha
{
    public partial class MainForm : Form
    {
        public static int jogador = 0;
        public static int[,] tabuleiro = new int[3,3];
    

        public MainForm()
        {
            InitializeComponent();
        }

        private void JogoDaVelha()
        {
            No noRaiz = new No();
            noRaiz.Jogo = new int[3, 3];

            Console.WriteLine("Quem começará jogando? [1 - Máquina | -1 - Jogador]");
            var quemEstaJogando = 1;
            string promptValue = Prompt.ShowDialog("Test", "123");
            labelDisplay.Text = promptValue;

            PreencheArvore(noRaiz, quemEstaJogando);
            AvaliaMiniMax(noRaiz, quemEstaJogando);
            IniciaJogo(noRaiz, quemEstaJogando);

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

        public int AvaliaMiniMax(No no, int proximoJogador)
        {
            var ganhador = no.EncontraGanhador();
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

        public int IniciaJogo(No no, int proximoJogador)
        {
            SoundPlayer simpleSound2 = new SoundPlayer(@"c:\Windows\Media\chimes.wav");
            simpleSound2.Play();
            DialogResult dlgResult = MessageBox.Show("É a vez do " + SubstituiCaracter(proximoJogador) + " jogar", "Vez de quem?");
            if (dlgResult == DialogResult.OK)
            {
                MarcaTabuleiro(no.Jogo, proximoJogador);
                if (proximoJogador == 1)
                {
                    for(int i = 0; i < no.Filhos.Count; i++)
                    {
                        var jogadaMelhor = no.Filhos.Max(a => a.ValorMinMax); 
                        //Verificar o filho de maior pontuação para aquele tabuleiro jogar nela e chamar a recursão
                    }
                    proximoJogador = -1;
                    IniciaJogo(no, proximoJogador);
                }
                else
                {
                    proximoJogador = 1;
                    no.Jogo[0, 1] = -1;
                    for(int i = 0; i <= no.Filhos.Count; i++)
                    {
                        if(ComparaTabuleiro(no.Jogo))
                        {
                            IniciaJogo(no.Filhos[i], proximoJogador);
                        }
                       // Escolher onde quer jogar, se estiver disponivel jogar e chamar a recursao
                    }
                }

            }
            return 0;
        }

        public bool ComparaTabuleiro(int[,] jogoDoNo)
        {
            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    if(jogoDoNo[i, j] == tabuleiro[i, j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void MarcaTabuleiro(int[,] jogo, int jogadorSimbolo)
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

        public string SubstituiCaracter(int numero)
        {
            return numero switch
            {
                1 => "X",
                -1 => "O",
                _ => ""
            };
        }

        // Ações dos botões
        private void Button1_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            button.Text = button.Name;
            MessageBox.Show("Olá mundo");
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            JogoDaVelha();
        }
    }
}
