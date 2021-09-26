using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using org.mariuszgromada.math.mxparser;

namespace Lab1
{
    public partial class Метод : Form
    {
        public Метод()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;  // Вывод формы по центру экрана
        }

        private void очиститьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            int length = textBox1.Text.Length;
            if (length == 0 && ch == ',' && ch == '-')  // Исключаем запятую в начале и минус
            {
                e.Handled = true;
            }
            if (!Char.IsDigit(ch) && ch != 8 && (ch != ',' || textBox1.Text.Contains(",")) && ((ch != '-' || textBox1.Text.Contains("-")))) // Если число, BACKSPACE запятая или минус, то вводим
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            int length = textBox2.Text.Length;
            if (length == 0 && ch == ',' && ch == '-')  // Исключаем запятую в начале и минус
            {
                e.Handled = true;
            }
            if (!Char.IsDigit(ch) && ch != 8 && (ch != ',' || textBox2.Text.Contains(",")) && ((ch != '-' || textBox2.Text.Contains("-")))) // Если число, BACKSPACE запятая или минус, то вводим
            {
                e.Handled = true;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            int length = textBox3.Text.Length;
            if (length == 0 && ch == ',' && ch == '-')  // Исключаем запятую в начале и минус
            {
                e.Handled = true;
            }
            if (!Char.IsDigit(ch) && ch != 8 && (ch != ',' || textBox3.Text.Contains(",")) && ((ch != '-' || textBox3.Text.Contains("-")))) // Если число, BACKSPACE запятая или минус, то вводим
            {
                e.Handled = true;
            }
        }

        private void рассчитатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "")
            {
                MessageBox.Show("Заполните все поля!", "Ошибка");
                return;
            }

            try
            {
                string a, b, formula;
                a = Convert.ToString(textBox1.Text);
                b = Convert.ToString(textBox2.Text);

                formula = Convert.ToString(textBox4.Text);
                Function f = new Function($"f(x)={textBox4.Text}"); // Сама формула здесь
                Function noF = new Function(textBox4.Text); // Сама формула здесь

                //string fa = $"f({a})";
                //string fb = $"f({b})";
                //Expression exp = new Expression(fa, f);
                //Expression exp2 = new Expression(fb, f);
                //label6.Text = Convert.ToString(exp.calculate());

                if (Convert.ToDouble(a) == Convert.ToDouble(b) || Convert.ToDouble(a) > Convert.ToDouble(b)) // Проверка, больше ли b хотя бы на 1
                {
                    MessageBox.Show("\"b\" должно быть больше \"a\" хотя бы на 1", "Ошибка");
                    return;
                }

                double borderA = Convert.ToDouble(textBox1.Text); // Граница A
                double borderB = Convert.ToDouble(textBox2.Text); // Граница B
                double Step = 0.5; // Шаг
                int n = (int)Math.Ceiling((borderB - borderA) / Step) + 1;

                double[] x1 = new double[n]; // Массив значений X – общий для обоих графиков
                double[] y1 = new double[n]; // Два массива Y – по одному для каждого графика

                for (int i = 0; i < n; ++i) // Расчитываем точки для графиков функции
                {
                    x1[i] = borderA + Step * i; // Вычисляем значение X
                    string d = $"f({x1[i]})";
                    string replacement = d.Replace(",", ".");

                    Expression graph = new Expression(replacement, f);
                    y1[i] = graph.calculate();

                }

                chart1.ChartAreas[0].AxisX.Minimum = borderA; // Оси графика
                chart1.ChartAreas[0].AxisX.Maximum = borderB;

                chart1.ChartAreas[0].AxisX.MajorGrid.Interval = Step; // Шаг сетки
                chart1.Series[0].Points.DataBindXY(x1, y1); // Добавление значений в график


                double f1(double x) // Возвращение функции
                {
                    Argument x_arg = new Argument("x");
                    Expression fx = new Expression(textBox4.Text, x_arg);
                    x_arg.setArgumentValue(x);
                    return fx.calculate();
                }
                /*double dixit(double a1, double b1, double eps1) // Метод дихотомии
                {
                    double c1;
                    do
                    {
                        c1 = (a1 + b1) / 2;
                        if (f1(a1) * f1(c1) < 0) 
                            b1 = c1;
                        else if (f1(b1) * f1(c1) < 0) 
                            a1 = c1;
                        else
                        {
                            MessageBox.Show("Нет корня", "Ошибка");
                            break;
                        }
                    } while (Math.Abs(b1 - a1) > eps1);
                        return c1;
                }
                double a11 = Convert.ToDouble(a);
                double b11 = Convert.ToDouble(b);
                double eps = Convert.ToDouble(textBox3.Text);

                double result = dixit(a11, b11, eps);
                label6.Text = Convert.ToString(result);*/

                double dixit(double a1, double b1, double e1)  // Метод дихотомии
                {
                    double c1;
                    while (b1 - a1 > e1)
                    {
                        c1 = (a1 + b1) / 2;

                        if (f1(c1 - e1) < f1(c1 + e1))
                            b1 = c1;
                        else
                            a1 = c1;
                    }
                    c1 = (a1 + b1) / 2;
                    return c1;
                }
                double a11 = Convert.ToDouble(a);
                double b11 = Convert.ToDouble(b);
                double eps = Convert.ToDouble(textBox3.Text);
                double result = dixit(a11, b11, eps);
                label6.Text = Convert.ToString(result);

                //chart1.Series[0].Points.AddXY(Convert.ToDouble(result));
            }
            catch
            {
                MessageBox.Show("Введите правильно форму", "Ошибка");
            }
        }
    }
}
