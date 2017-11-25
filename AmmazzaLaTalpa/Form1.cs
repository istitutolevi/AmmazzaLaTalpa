using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AmmazzaLaTalpa
{
    public partial class Form1 : Form
    {

        // Array contenente tutti i bottoni relativi ai buchi
        Button[] _bottoniBuchi;

        // Variabile che ci indica se stiamo giocando
        bool _stiamoGiocando;

        // Variabile contenente la posizione della talpa
        int _posizioneTalpa;

        // Variabile che ci indica quanto tempo abbiamo a disposizione per seccare
        // la talpa. E' in millisecondi
        int _tempoADisposizione;

        // Numero di talpe che abbiamo seccato
        int _talpeSeccate;

        // Orario in cui la talpa è uscita dal buco
        DateTime _orarioUscitaTalpa;

        // Generatore dei numeri casuali. Questo possiamo inizializzarlo anche qui...
        Random _random = new Random();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CreaArray();
            VuotaBuchi();

            timer1.Enabled = false;
            _stiamoGiocando = false;
        }


        // Creazione dell'array dei bottoni. Questo ci consente poi di lavorare esclusivamente
        // con il numero del buco.
        void CreaArray()
        {
            _bottoniBuchi = new Button[9];
            _bottoniBuchi[0] = button1;
            _bottoniBuchi[1] = button2;
            _bottoniBuchi[2] = button3;
            _bottoniBuchi[3] = button4;
            _bottoniBuchi[4] = button5;
            _bottoniBuchi[5] = button6;
            _bottoniBuchi[6] = button7;
            _bottoniBuchi[7] = button8;
            _bottoniBuchi[8] = button9;

        }

        // Funzione per vuotare tutti i buchi (colorarli di bianco).
        void VuotaBuchi()
        {
            for (int i = 0; i < 9; i++)
            {
                _bottoniBuchi[i].BackColor = Color.White;
            }
        }

        // Funzione per vuotare un singolo buco (colorarlo di bianco).
        void VuotaBuco(int numeroBuco)
        {
            _bottoniBuchi[numeroBuco].BackColor = Color.White;
        }

        // Funzione per vuotare un singolo buco (colorarlo di rosso).
        void RiempiBuco(int numeroBuco)
        {
            _bottoniBuchi[numeroBuco].BackColor = Color.Red;
        }


        private void btnInizio_Click(object sender, EventArgs e)
        {
            // Abilitiamo il timer (già impostato a 1ms). Nel Tick ci occuperemo
            // poi di tutti i controlli relativi al gioco
            timer1.Enabled = true;

            // Indichiamo che stiamo giocando
            _stiamoGiocando = true;

            // Inizialmente la talpa non è nascosta
            _posizioneTalpa = -1;

            // Quando uscirà la talpa l'utente dovrà colpirla in un secondo.
            _tempoADisposizione = 5000;

            // Le talpe inizialmente seccate sono 0 (il punteggio iniziale è 0)
            _talpeSeccate = 0;

            // Vuotiamo anche tutti i buchi
            VuotaBuchi();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Quando il timer viene richiamato abbiamo due diverse possibilità
            //  1.	La talpa è dentro. Dobbiamo farla uscire indicando l’orario di uscita
            //  2.	La talpa è fuori. Dobbiamo controllare che non sia passato troppo tempo da quando è uscita.
            //      Se è passato troppo tempo allora GameOver (scriviamo il metodo)

            // Controlliamo se la talpa è dentro
            if (_posizioneTalpa == -1)
            {
                _orarioUscitaTalpa = DateTime.Now; // Prendo il tempo in cui la talpa è uscita
                _posizioneTalpa = _random.Next(9); // Genero un numero casuale per il buco in cui si trova
                RiempiBuco(_posizioneTalpa);       // Mostro la talpa
            }
            else
            {
                // La talpa è fuori

                // Controllo che non sia passato troppo tempo
                if (DateTime.Now.AddMilliseconds(-_tempoADisposizione) > _orarioUscitaTalpa)
                {
                    // E' passato troppo tempo...
                    GameOver();
                }

            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Anche in questo caso abbiamo diverse possibilità
            //   1.	La talpa è dentro… Avevi fatto il furbetto :)… GameOver
            //   2.	La talpa è fuori. Recupero il numero del buco su cui è stato premuto il tasto
            //      (facciamo anche in questo caso un metodo BucoBastonato che deve prendere in 
            //      input un bottone e darci in output il numero del buco bastonato)
            //      e lo confronto con il buco in cui si trova la talpa. Se sono diversi GameOver
            //      altrimenti:
            //      a.  indico che la talpa non c’è più (ossia chiamo VuotaBuco per farlo diventare bianco e indico che la sua posizione _posizioneTalpa è -1). 
            //      b.	Per aumentare la difficoltà diminuiamo anche il tempo a disposizione 
            //          per la prossima talpa… Ad esempio del 20% (ossia per ottenere il nuovo 
            //          tempo a disposizione moltiplichiamo per 80 e dividiamo per 100 il vecchio tempo a disposizione).
            //      c.	Aumentare il numero di talpe seccate

            // Controlliamo se la talpa è dentro
            if (_posizioneTalpa == -1)
            {
                GameOver();
            }
            else
            {
                // Recuperiamo il numero del buco relativo al bottone
                // NB: il sender è l'oggetto su cui è avvenuto l'evento
                int buco = BucoBastonato(sender);

                // Confronto il buco su cui ho premuto il pulsante con la posizione
                // della talpa
                if (buco != _posizioneTalpa)
                {
                    // Sono diversi
                    GameOver();
                }
                else
                {
                    VuotaBuco(_posizioneTalpa);

                    // Indico che la talpa non c'é più
                    _posizioneTalpa = -1;

                    // Moltiplico il tempo a disposizione per .9
                    _tempoADisposizione = _tempoADisposizione * 90 / 100;

                    _talpeSeccate++;
                }
            }

        }


        void GameOver()
        {
            // ...cosa significa GameOver? Cosa dobbiamo fare quando c’è un GameOver? 
            //    Fermare il timer, 
            //    indicare che non stiamo giocando e 
            //    stampare il numero di talpe seccate…

            // Fermare il timer
            timer1.Enabled = false;

            // Indicare che non stiamo giocando
            _stiamoGiocando = false;

            // Stampare il numero delle talpe seccate
            MessageBox.Show(string.Format("Numero talpe seccate {0}", _talpeSeccate));

        }


        // Questa funzione viene richiamata con il pulsante premuto e ci 
        // restituisce il buco bastonato (ossia il numero del pulsante premuto
        // cosi' come ordinato nell'array.
        private int BucoBastonato(object bottone)
        {
            for (int i = 0; i < 9; i++)
            {
                if (_bottoniBuchi[i] == bottone)
                {
                    return i;
                }
            }
            return -1;
        }

    }
}
