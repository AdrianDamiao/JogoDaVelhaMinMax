using System;
using System.IO;
using System.Linq;
using System.Media;
using System.Threading;
using System.Windows.Forms;
using JogoDaVelha.Engine;
using JogoDaVelha.Extensions;
using JogoDaVelha.Forms;

namespace JogoDaVelha
{
    public partial class MainForm : Form
    {
        public static int jogador = 0;
        public static int[,] tabuleiro = new int[3,3];
        public static bool continua = false;
    

        public MainForm()
        {
            InitializeComponent();
        }

        private void JogoDaVelha()
        {
            No noRaiz = new No();
            noRaiz.Jogo = new int[3, 3];


            string promptValue = ChoosePlayer.ShowDialog("Quem começará jogando?", "Escolher o jogador");
            labelDisplay.Text = promptValue;
            var quemEstaJogando = promptValue.Equals("Maquina") ? 1 : 0;

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
                        if (resultado < no.ValorMinMax)
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
                        if (resultado > no.ValorMinMax)
                            no.ValorMinMax = resultado;
                    }
                    return no.ValorMinMax;
                }

            }
            return 0;
        }

        public int IniciaJogo(No no, int proximoJogador)
        {
            labelDisplay.Text = SubstituiCaracter(proximoJogador);
            SoundPlayer simpleSound2 = new SoundPlayer(@"c:\Windows\Media\chimes.wav");
            simpleSound2.Play();
            DialogResult dlgResult = MessageBox.Show("É a vez do " + SubstituiCaracter(proximoJogador) + " jogar", "Vez de quem?");
            if (dlgResult == DialogResult.OK)
            {
                AtualizaTabuleiro(no.Jogo);
                MarcaTabuleiro(tabuleiro);
                if (proximoJogador == 1)
                {
                    int melhorPontuacao = Int32.MinValue;
                    int melhorFilho = 0;
                    for (int i = 0; i < no.Filhos.Count; i++)
                    {
                        if (no.Filhos[i].ValorMinMax > melhorPontuacao)
                        {
                            melhorPontuacao = no.Filhos[i].ValorMinMax;
                            melhorFilho = i;
                        }
                        //Verificar o filho de maior pontuação para aquele tabuleiro jogar nela e chamar a recursão
                    }

                    jogador = proximoJogador;
                    IniciaJogo(no.Filhos[melhorFilho], proximoJogador*(-1));
                }
                else
                {
                    jogador = proximoJogador;
                    button1.UseWaitCursor = true;
                    button1.Click += (sender, e) =>
                    {

                        if (button1.DialogResult == DialogResult.OK)
                        {
                            int filhoCorreto = 0;
                            continua = false;
                            foreach (var (filho, index) in no.Filhos.LoopIndex())
                            {
                                if (ComparaTabuleiro(filho.Jogo))
                                {
                                    filhoCorreto = index;
                                }
                            }

                            IniciaJogo(no.Filhos[filhoCorreto], proximoJogador * (-1));
                        }
                    };

                    // Escolher onde quer jogar, se estiver disponivel jogar e chamar a recursao
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
                    if(jogoDoNo[i, j] != tabuleiro[i, j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void AtualizaTabuleiro(int[,] jogo)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    tabuleiro[i, j] = jogo[i,j];
                }
            }
        }

        public void MarcaTabuleiro(int[,] jogo)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    //tabuleiro[i, j] = jogo[i, j];
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

        private void button1_Click(object sender, EventArgs e, No no)
        {
            Button button = (Button)sender;
            if (!string.IsNullOrEmpty(button.Text))
            {
                DialogResult dialogResult = MessageBox.Show("Você não pode jogar aqui", "Jogada inválida", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                button.Text = SubstituiCaracter(jogador);
                AtualizaPorTabIndex(button.TabIndex);

                button.DialogResult = DialogResult.OK;
            }
        }

        private int AtualizaPorTabIndex(int tabIndex)
        {
            return tabIndex switch
            {
                1 => tabuleiro[0, 0] = jogador,
                2 => tabuleiro[0, 1] = jogador,
                3 => tabuleiro[0, 2] = jogador,
                4 => tabuleiro[1, 0] = jogador,
                5 => tabuleiro[1, 1] = jogador,
                6 => tabuleiro[1, 2] = jogador,
                7 => tabuleiro[2, 0] = jogador,
                8 => tabuleiro[2, 1] = jogador,
                9 => tabuleiro[2, 2] = jogador,
                _ => 0,
            };
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            JogoDaVelha();
        }
    }
}
