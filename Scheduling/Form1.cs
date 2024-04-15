using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Scheduling
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<job> processo = new List<job>();
        int i = 0;

        private void btnInserisci_Click(object sender, EventArgs e)
        {
            string nome = "";
            int tempo = 0;
            processo.Add(new job());
            try
            {
                tempo = Convert.ToInt32(txtTempo.Text);
                nome = Convert.ToString(txtNome.Text);
                processo[i].nome = nome;
                processo[i].tempo = tempo;
                listBox1.Items.Add("Nome:  " + processo[i].nome + "          Tempo:  " + processo[i].tempo + "ms");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore  inserimento job: "+ ex.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                txtNome.Clear();
                txtTempo.Clear();
                processo.RemoveAt(i);

            }


            
            txtNome.Clear();
            txtTempo.Clear();
            i++;
        }

        private async void btnEsegui_Click(object sender, EventArgs e)
        {
            switch (comboBoxScheduling.SelectedIndex)
            {
                case 0:
                    listBox1.Items.Clear();
                    int tempoEsecuzione = 0;
                    for (int j=0;j<processo.Count;j++)
                    {
                        tempoEsecuzione += processo[j].tempo;
                        Thread.Sleep(processo[j].tempo);
                        listBox1.Items.Add("Processo:  "+processo[j].nome+ "     tempo: "+ processo[j].tempo + "ms    eseguito con successo");
                        
                    }
                    
                    listBox1.Items.Add("Processi eseguiti in " + tempoEsecuzione + "ms");
                    i = 0;
                    processo.Clear();
                    break;
                case 1:
                    listBox1.Items.Clear();
                    int varAusilTempo = processo[0].tempo;
                    string varAusilNome = processo[0].nome;
                    tempoEsecuzione = 0;
                    for (int j = 0; j < processo.Count; j++)
                    {
                        tempoEsecuzione += processo[j].tempo;
                        Thread.Sleep(processo[j].tempo);
                    }
                    while (processo.Count > 0)
                    {
                        //INIZIALIZZA k A PROCESSIINSERITI.COUNT-1 PERCHE' GLI INDICI VANNO FINO A N-1
                        int k = processo.Count - 1;
                        for (int j = 0; j < processo.Count-1; j++)
                        {
                            if (processo[j + 1].tempo > processo[j].tempo)
                            {
                                //ORDINA LA LISTA DAL JOB CON TEMPO PIù GRANDE AL PIU' PICCOLO
                                varAusilNome = processo[j].nome;
                                varAusilTempo = processo[j].tempo;
                                processo[j].tempo = processo[j + 1].tempo;
                                processo[j].nome = processo[j + 1].nome;
                                processo[j + 1].nome = varAusilNome;
                                processo[j + 1].tempo = varAusilTempo;
                            }
                        }
                        //STAMPA L'ULTIMO ELEMENTO DELLA LISTA A CUI E' ASSEGNATO IL JOB CON TEMPO MINORE, RIMUOVE L'OGGETTO PERCHE' NON SERVE PIU' E DECREMENTA  LA i VISTO CHE SI HA UN JOB IN MENO
                        listBox1.Items.Add("Processo " + processo[k].nome + " eseguito con successo in " + processo[k].tempo + "ms");
                        i--;
                        processo.RemoveAt(k); // Rimuovi l'elemento dopo aver stampato il messaggio
                    }

                    listBox1.Items.Add("Processi eseguiti in " + tempoEsecuzione + "ms");
                    break;

                
                case 2:
                    listBox1.Items.Clear();
                    tempoEsecuzione = 0;
                    for (int j = 0; j < processo.Count; j++)
                    {
                        tempoEsecuzione += processo[j].tempo;
                        Thread.Sleep(processo[j].tempo);
                    }
                    int timeSlice = 20;
                    //Cicla finche ci sono elemnti nella lista
                    while (processo.Count>0)
                    {
                        for(int j=0;j<processo.Count;j++)
                        {

                            //Aggiorna tempo rimanente del processo
                            processo[j].tempo -= timeSlice;

                            //Rimuovi il procecsso se completato
                            if (processo[j].tempo<=0)
                            {
                                listBox1.Items.Add("Il processo " + processo[j].nome + " è stato completato");
                                processo.RemoveAt(j);
                                j--;
                            }
                            else
                            {
                                listBox1.Items.Add("Il processo " + processo[j].nome + " è stato sospeso");

                            }

                        }

                    }
                    listBox1.Items.Add("Processi eseguiti in " + tempoEsecuzione + "ms");
                    break;
                default:
                    MessageBox.Show("Inserire la politica da usare","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }
    }
}
