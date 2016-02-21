using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Shapes;

namespace OverParse
{
    public class Combatant
    {
        private const float maxBGopacity = 0.6f;
        public int Damage;
        public int Healing;
        public string ID;
        public string Name { get; set; }
        public int MaxHitNum;
        public string MaxHitID;
        public float DPS;
        public float PercentDPS;
        public List<Attack> Attacks;
        public Color Color { get; set; }
        public LinearGradientBrush Brush
        {
            get
            {
                Color c = new Color();
                c.A=(byte)(Color.A*maxBGopacity);
                c.R=Color.R;
                c.G=Color.G;
                c.B=Color.B;

                LinearGradientBrush lgb = new LinearGradientBrush();
                lgb.StartPoint=new System.Windows.Point(0, 0);
                lgb.EndPoint=new System.Windows.Point(1, 0);
                lgb.GradientStops.Add(new GradientStop(c, 0));
                lgb.GradientStops.Add(new GradientStop(c, PercentDPS/maxShare));
                lgb.GradientStops.Add(new GradientStop(new Color(), PercentDPS/maxShare));
                lgb.GradientStops.Add(new GradientStop(new Color(), 1));
                lgb.SpreadMethod=GradientSpreadMethod.Repeat;
                return lgb;
            }
        }
        public SolidColorBrush BarBrush { get { SolidColorBrush sbc = new SolidColorBrush(Color); sbc.Opacity=PercentDPS/maxShare; return sbc; } }
        public Rectangle bar { get; set; }
        public static float maxShare = 0;//oh boy

        private static Color[] colors = new Color[] {//The eight accent colors of Solarized, with averaged in-betweens
                (Color)ColorConverter.ConvertFromString("#FFdc322f"),//red
                (Color)ColorConverter.ConvertFromString("#FFd35722"),
                (Color)ColorConverter.ConvertFromString("#FFcb4b16"),//orange
                (Color)ColorConverter.ConvertFromString("#FFc06a08"),
                (Color)ColorConverter.ConvertFromString("#FFb58900"),//yellow
                (Color)ColorConverter.ConvertFromString("#FF9D9100"),
                (Color)ColorConverter.ConvertFromString("#FF859900"),//green
                (Color)ColorConverter.ConvertFromString("#FF579d4c"),
                (Color)ColorConverter.ConvertFromString("#FF2aa198"),//cyan
                (Color)ColorConverter.ConvertFromString("#FF2896b5"),
                (Color)ColorConverter.ConvertFromString("#FF268bd2"),//blue
                (Color)ColorConverter.ConvertFromString("#FF497ecb"),
                (Color)ColorConverter.ConvertFromString("#FF6c71c4"),//violet
                (Color)ColorConverter.ConvertFromString("#FF9f53a3"),
                (Color)ColorConverter.ConvertFromString("#FFd33682"),//magenta
                (Color)ColorConverter.ConvertFromString("#FFd73458")};

        public bool isAlly
        {
            get
            {
                string[] SuAttacks = {"487482498", "2785585589", "639929291"};
                if (int.Parse(ID) >= 10000000)
                {
                    return true;
                }

                bool allied = false;
                foreach (Attack a in Attacks)
                {
                    if (SuAttacks.Contains(a.ID))
                    {
                        allied = true;
                        return allied;
                    }
                }

                return allied;
            }
        }

        public string MaxHit
        {
            get
            {
                string attack = "Unknown";
                if (MainWindow.skillDict.ContainsKey(MaxHitID))
                {
                    attack = MainWindow.skillDict[MaxHitID];
                }

                return MaxHitNum.ToString("N0") + $" ({attack})";
            }
        }

        public string DPSReadout
        {
            get
            {
                if (RawDPSHack.ShowRawDPS)
                {
                    return FormatNumber(DPS);
                } else
                {
                    if (PercentDPS < -.5)
                    {
                        return "--";
                    }
                    else
                    {
                        return string.Format("{0:0.0}", PercentDPS) + "%";
                    }
                }

            }
        }

        string FormatNumber(float input)
        {
            int num = (int)Math.Round(input);

            if (num >= 100000000)
            {
                return (num / 1000000D).ToString("0.#M");
            }

            if (num >= 1000000)
            {
                return (num / 1000000D).ToString("0.##M");
            }

            if (num >= 100000)
            {
                return (num / 1000D).ToString("0.#K");
            }

            if (num >= 10000)
            {
                return (num / 1000D).ToString("0.##K");
            }

            return num.ToString("#,0");
        }

        public string DamageReadout
        {
            get
            {
                return Damage.ToString("N0");
            }
        }

        public Combatant(string id, string name)
        {
            ID = id;
            Name = name;
            Damage = 0;
            Healing = 0;
            MaxHitNum = 0;
            MaxHitID = "none";
            DPS = 0;
            PercentDPS = -1;
            Attacks = new List<Attack>();

            if(name=="YOU") //special cases whee
            {
                Color=(Color)ColorConverter.ConvertFromString("#FF205020");//pulled from bindings, made fully opaque
            } else if(name=="Zanverse") {
                Color=Color.FromRgb(255,255,255);
            } else {
                int i = 0;
                foreach(char c in name) i+=c;
                Color=colors[i%colors.Length];
            }
        }
    }
}