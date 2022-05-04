using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JogoDaVelha.Forms
{
    public static class Prompt
    {
        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form()
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
                prompt.Close();
            };

            Button confirmacaoHumano = new Button() { Text = "Humano", Left = 250, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmacaoHumano.Click += (sender, e) => {
                jogador.Text = "Humano";
                prompt.Close(); 
            };

            prompt.Controls.Add(confirmacaoMaquina);
            prompt.Controls.Add(confirmacaoHumano);
            prompt.Controls.Add(jogador);
            prompt.AcceptButton = confirmacaoMaquina;
            prompt.CancelButton = confirmacaoHumano;

            return prompt.ShowDialog() == DialogResult.OK ? jogador.Text : "";
        }
    }
}
