using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Agenda
{
    public class Wekker : PictureBox
    {
        public bool IsGezet { get; private set; }
        public event EventHandler LooptAf;

        Form1 form1;
        FormWekker formWekker;
        DateTime alarmTijd;
        readonly PointF middelpunt = new PointF(29.5f, 29.5f);
        PointF eindpuntAlarmWijzer;
        Brush middenBrush;
        Pen wijzerPen;
        Pen alarmPen;
        ToolTip toolTip = new ToolTip();
        Timer timerIsGezet = new Timer();
        Timer timerLooptAf = new Timer();

        Func<PointF, double, int, PointF> eindpuntWijzer = (startpunt, fractie, straal) =>
            startpunt + new SizeF((float)Math.Sin(fractie * 6.283) * straal, -(float)Math.Cos(fractie * 6.283) * straal);

        public Wekker(Form1 form1)
        {
            this.form1 = form1;
            this.Cursor = Cursors.Hand;
            this.Size = new Size(60, 60);
            this.Visible = false;
            this.Paint += wekker_Paint;
            this.Click += wekker_Click;

            Bitmap bitmapWekker = Properties.Resources.Wekker;
            bitmapWekker.MakeTransparent(Color.White);
            this.Image = bitmapWekker;

            middenBrush = new SolidBrush(form1.LijnKleur);
            wijzerPen = new Pen(form1.LijnKleur, 2.0f);
            alarmPen = new Pen(Color.Red, 2.0f);

            timerIsGezet.Interval = 5000;
            timerIsGezet.Tick += new EventHandler(timerIsGezet_Tick);
            timerLooptAf.Interval = 800;
            timerLooptAf.Tick += new EventHandler(timerLooptAf_Tick);
        }

        public void ToonFormulier()
        {
            formWekker = new FormWekker(this, form1);
            formWekker.ShowDialog();
        }

        public void Zetten(DateTime tijd)
        {
            alarmTijd = tijd;
            toolTip.SetToolTip(this, "Alarmtijd: " + tijd.ToShortTimeString());
            eindpuntAlarmWijzer = eindpuntWijzer(middelpunt, tijd.TimeOfDay.TotalHours / 12.0, 17);
            Refresh();

            Visible = true;
            IsGezet = true;
            timerIsGezet.Start();
        }

        public void UitZetten()
        {
            Visible = false;
            IsGezet = false;
            timerIsGezet.Stop();
            timerLooptAf.Stop();
        }

        void wekker_Paint(object sender, PaintEventArgs e)
        {
            PointF eindpuntKleineWijzer =
                eindpuntWijzer(middelpunt, DateTime.Now.TimeOfDay.TotalHours / 12.0, 13);
            PointF eindpuntGroteWijzer =
                eindpuntWijzer(middelpunt, DateTime.Now.Minute / 60.0, 17);

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.FillEllipse(middenBrush, middelpunt.X - 3, middelpunt.Y - 3, 6f, 6f);
            e.Graphics.DrawLine(wijzerPen, middelpunt, eindpuntKleineWijzer);
            e.Graphics.DrawLine(wijzerPen, middelpunt, eindpuntGroteWijzer);
            e.Graphics.DrawLine(alarmPen, middelpunt, eindpuntAlarmWijzer);
        }

        void wekker_Click(object sender, EventArgs e)
        {
            if (IsGezet)
                ToonFormulier();
            else // wekker loopt af
                UitZetten();
        }

        void timerIsGezet_Tick(object sender, EventArgs e)
        {
            Refresh();

            if (DateTime.Now >= alarmTijd)
            {
                if (formWekker != null)
                    formWekker.Dispose();
                IsGezet = false;
                timerIsGezet.Stop();
                timerLooptAf.Start();
                if (LooptAf != null)
                    LooptAf(null, null);
            }
        }

        void timerLooptAf_Tick(object sender, EventArgs e)
        {
            System.Media.SystemSounds.Exclamation.Play();
            BackColor = Color.Red;
            Refresh();
            System.Threading.Thread.Sleep(200);
            BackColor = Color.Transparent;
            Refresh();
        }
    }
}
