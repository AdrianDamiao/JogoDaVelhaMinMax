using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JogoDaVelha.Forms
{
    public static class ChoosePlayer
    {
        public static string ShowDialog(string text, string caption)
        {
            Form choosePlayer = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };

            Control jogador = new Control();
           
            Button confirmacaoMaquina = new Button() { Text = "Maquina", Left = 150, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmacaoMaquina.Click += (sender, e) => {
                jogador.Text = "Maquina";
                choosePlayer.Close();
            };

            Button confirmacaoHumano = new Button() { Text = "Humano", Left = 250, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmacaoHumano.Click += (sender, e) => {
                jogador.Text = "Humano";
                choosePlayer.Close(); 
            };

            choosePlayer.Controls.Add(confirmacaoMaquina);
            choosePlayer.Controls.Add(confirmacaoHumano);
            choosePlayer.Controls.Add(jogador);
            choosePlayer.AcceptButton = confirmacaoMaquina;
            choosePlayer.CancelButton = confirmacaoHumano;

            return choosePlayer.ShowDialog() == DialogResult.OK ? jogador.Text : "";
        }
    }
}
