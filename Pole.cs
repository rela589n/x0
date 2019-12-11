using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace X0
{
    public delegate void FinishEventHandler(char status);
    public partial class Pole : UserControl
    {
        GameButton[,] P = new GameButton[3, 3];   //Ігрове поле з кнопок

        public event FinishEventHandler OnFinish; //Подія "Хтось виграв"
        public bool active = true; //Прапорець "Чи реагувати на кнопки?"
        int Moves = 0;  //Лічильник ходів на випадок "нічиєї"

        public Pole()
        {
            InitializeComponent();
            this.ClientSizeChanged += new EventHandler(PoleClientSizeChanged);

            //Створення кнопок
            for (int r = 0; r < 3; r++)
                for (int c = 0; c < 3; c++)
                {
                    P[r, c] = new GameButton();
                    P[r, c].Parent = this;
                    P[r, c].Click += new EventHandler(GameButtonClick);
                }
        }

        void Control() //перевірка, чи раптом хтось не виграв. Якщо виграв 
                       //- генерується подія OnFinish і передається символ-переможець
        {
            if (P[0, 0].IsCross && P[0, 1].IsCross && P[0, 2].IsCross || P[1, 0].IsCross && P[1, 1].IsCross && P[1, 2].IsCross || P[2, 0].IsCross && P[2, 1].IsCross && P[2, 2].IsCross || P[0, 0].IsCross && P[1, 0].IsCross && P[2, 0].IsCross || P[0, 1].IsCross && P[1, 1].IsCross && P[2, 1].IsCross || P[0, 2].IsCross && P[1, 2].IsCross && P[2, 2].IsCross || P[0, 0].IsCross && P[1, 1].IsCross && P[2, 2].IsCross || P[0, 2].IsCross && P[1, 1].IsCross && P[2, 0].IsCross)
            {
                active = false;
                OnFinish?.Invoke('X');
            }
            else if (P[0, 0].IsZero && P[0, 1].IsZero && P[0, 2].IsZero || P[1, 0].IsZero && P[1, 1].IsZero && P[1, 2].IsZero || P[2, 0].IsZero && P[2, 1].IsZero && P[2, 2].IsZero || P[0, 0].IsZero && P[1, 0].IsZero && P[2, 0].IsZero || P[0, 1].IsZero && P[1, 1].IsZero && P[2, 1].IsZero || P[0, 2].IsZero && P[1, 2].IsZero && P[2, 2].IsZero || P[0, 0].IsZero && P[1, 1].IsZero && P[2, 2].IsZero || P[0, 2].IsZero && P[1, 1].IsZero && P[2, 0].IsZero)
            {
                active = false;
                OnFinish?.Invoke('O');
            }
            else
            {
                for (int i = 0; i <= 2; ++i)
                {
                    for (int j = 0; j <= 2; ++j)
                    {
                        if (P[i, j].IsClear)
                        {
                            return;
                        }
                    }
                }

                active = false;
                OnFinish?.Invoke('N');
            }
        }

        void PoleClientSizeChanged(object sender, EventArgs e)
        {
            //Підгонка розмірів та розміщення кнопок на полі
            for (int r = 0; r < 3; r++)
                for (int c = 0; c < 3; c++)
                {
                    P[r, c].Width = ClientRectangle.Width / 3;
                    P[r, c].Height = ClientRectangle.Height / 3;
                    P[r, c].Left = c * P[r, c].Width;
                    P[r, c].Top = r * P[r, c].Height;
                }
        }

        public void Clear() //Підготовка до нової партії
        {
            for (int r = 0; r < 3; r++)
                for (int c = 0; c < 3; c++)
                    P[r, c].SetClear();
            active = true; Moves = 0;
        }

        void GameButtonClick(object sender, EventArgs e)
        {
            if (!active)
            {
                this.Clear();
                return;
            }

            if ((sender as GameButton).IsClear)
                if (active) //Якщо партія не закінчена
                {
                    (sender as GameButton).SetCross(); //ставимо на 
                                                       //кнопці хрестик
                    Control(); //Перевірити, чи не переміг 
                               //той, хто клацав
                    if (++Moves > 8) active = false;//Якщо останній 
                                                    //хід - блокуємо поле
                    if (active)     //Якщо потрібна відповідь 
                                    //комп'ютера
                    {
                        int kh = 0, nr = 0, ns = 0;
                        //Пошук виграшного ходу для нуликів
                        for (int r = 0; r < 3; r++)
                            for (int s = 0; s < 3; s++)
                                if (P[r, s].IsClear)
                                { //перевірка у стовпці
                                    if (kh != 2)
                                    {
                                        kh = 0; for (int rr = 0; rr < 3; rr++)
                                            if (P[rr, s].IsZero)
                                            {
                                                kh++;
                                                if (kh == 2) { ns = s; nr = r; }
                                            }
                                    }
                                    //перевірка у рядку
                                    if (kh != 2)
                                    {
                                        kh = 0;
                                        for (int ss = 0; ss < 3; ss++)
                                            if (P[r, ss].IsZero)
                                            {
                                                kh++;
                                                if (kh == 2) { ns = s; nr = r; }
                                            }
                                    }
                                    //перевірка у діагоналі 1
                                    if ((kh != 2) && (r == s))
                                    {
                                        kh = 0;
                                        for (int ss = 0; ss < 3; ss++)
                                            if (P[ss, ss].IsZero)
                                            {
                                                kh++;
                                                if (kh == 2) { ns = s; nr = r; }
                                            }
                                    }
                                    //перевірка у діагоналі 2
                                    if ((kh != 2) && (r == 2 - s))
                                    {
                                        kh = 0;
                                        for (int ss = 0; ss < 3; ss++)
                                            if (P[ss, 2 - ss].IsZero)
                                            {
                                                kh++;
                                                if (kh == 2) { ns = s; nr = r; }
                                            }
                                    }
                                }
                        if (kh != 2) //Якщо нулики не виграють цим ходом, то
                                     //пошук виграшного ходу хрестиків, щоб не програти
                            for (int r = 0; r < 3; r++)
                                for (int s = 0; s < 3; s++)
                                    if (P[r, s].IsClear)
                                    { //перевірка у стовпці
                                        if (kh != 2)
                                        {
                                            kh = 0;
                                            for (int rr = 0; rr < 3; rr++)
                                                if (P[rr, s].IsCross)
                                                {
                                                    kh++;
                                                    if (kh == 2) { ns = s; nr = r; }
                                                }
                                        }
                                        //перевірка у рядку
                                        if (kh != 2)
                                        {
                                            kh = 0;
                                            for (int ss = 0; ss < 3; ss++)
                                                if (P[r, ss].IsCross)
                                                {
                                                    kh++;
                                                    if (kh == 2) { ns = s; nr = r; }
                                                }
                                        }
                                        //перевірка у діагоналі 1
                                        if ((kh != 2) && (r == s))
                                        {
                                            kh = 0;
                                            for (int ss = 0; ss < 3; ss++)
                                                if (P[ss, ss].IsCross)
                                                {
                                                    kh++;
                                                    if (kh == 2) { ns = s; nr = r; }
                                                }
                                        }
                                        //перевірка у діагоналі 2
                                        if ((kh != 2) && (r == 2 - s))
                                        {
                                            kh = 0;
                                            for (int ss = 0; ss < 3; ss++)
                                                if (P[ss, 2 - ss].IsCross)
                                                {
                                                    kh++;
                                                    if (kh == 2) { ns = s; nr = r; }
                                                }
                                        }
                                    }
                        if (kh != 2)  //якщо немає виграшного ходу і
                                      //загрози програшу – випадковий хід
                        {
                            Random RN = new Random();
                            do { nr = RN.Next(3); ns = RN.Next(3); }
                            while (!P[nr, ns].IsClear);
                        }
                        P[nr, ns].SetNull(); Moves++;
                        Control(); return;
                    }
                }
        }//Кінець GameButtonClick
    }//Кінець Pole

}
