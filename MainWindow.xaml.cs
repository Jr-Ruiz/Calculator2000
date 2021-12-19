using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Calculator2000
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Display.Focus();
        }

        static class Globales
        {
            public static string num_parcial = "";
            public static List<double> numeros = new List<double>(); //Lista para almacenar los números de la operación
            public static List<string> operadores = new List<string>(); //Lista par almancenar los operadores
            public static char operador;
        }

        private void escribir(object sender, RoutedEventArgs e)
        {
            Button boton = sender as Button;

            Display.Text = Display.Text + boton.Content.ToString();
            resultado.Content = Display.Text;
        }

        private void calcular(object sender, RoutedEventArgs e)
        {

            Button boton = sender as Button;

            char[] caracter = Display.Text.ToCharArray(); //convertimos el string del Display en un array de caracteres

            for (int i = 0; i < Display.Text.Length; i = i + 1)
            {
                Globales.operador = caracter[i];
                //Recorremos el array de caracteres
                switch (caracter[i])
                {
                    case 'x':
                        rellenar(caracter[i]);//llamamos la función que rellena las listas
                        break;
                    case '/':
                        rellenar(caracter[i]);
                        break;
                    case '%':
                        rellenar(caracter[i]);
                        break;
                    case '+':
                        rellenar(caracter[i]);
                        break;
                    case '-':
                        rellenar(caracter[i]);
                        break;
                    default:
                        Globales.num_parcial = Globales.num_parcial + caracter[i];
                        break;
                }
            }
            calculo();
        }

        private void calculo()
        {
            try
            {
                Globales.numeros.Add(Convert.ToDouble(Globales.num_parcial));
                Globales.num_parcial = "";
            }
            catch
            {
                resultado.Content = "EXPRESIÓN MAL FORMADA";
            }

            while (Globales.operadores.Contains("x") || Globales.operadores.Contains("/") || Globales.operadores.Contains("%"))
            {

                for (int i = 0; i < Globales.operadores.Count; i = i + 1)
                {
                    if (Globales.operadores[i] == "x")
                    {
                        Globales.numeros[i] = Globales.numeros[i] * Globales.numeros[i + 1];
                        Globales.numeros.RemoveAt(i + 1);
                        Globales.operadores.RemoveAt(i);
                        break;
                    }
                    else if (Globales.operadores[i] == "/")
                    {
                        Globales.numeros[i] = Globales.numeros[i] / Globales.numeros[i + 1];
                        Globales.numeros.RemoveAt(i + 1);
                        Globales.operadores.RemoveAt(i);
                        break;
                    }
                    else if (Globales.operadores[i] == "%")
                    {
                        Globales.numeros[i] = Globales.numeros[i] * Globales.numeros[i + 1] / 100;
                        Globales.numeros.RemoveAt(i + 1);
                        Globales.operadores.RemoveAt(i);
                    }

                }
            }
            while (Globales.operadores.Contains("+") || Globales.operadores.Contains("-"))
            {

                for (int i = 0; i < Globales.operadores.Count; i = i + 1)
                {
                    if (Globales.operadores[i] == "+")
                    {
                        Globales.numeros[i] = Globales.numeros[i] + Globales.numeros[i + 1];
                        Globales.numeros.RemoveAt(i + 1);
                        Globales.operadores.RemoveAt(i);
                        break;
                    }
                    else if (Globales.operadores[i] == "-")
                    {
                        Globales.numeros[i] = Globales.numeros[i] - Globales.numeros[i + 1];
                        Globales.numeros.RemoveAt(i + 1);
                        Globales.operadores.RemoveAt(i);
                        break;
                    }

                }
            }
;
            try
            {
                Pantalla.Items.Add(Display.Text + "=" + Globales.numeros[0]);
                resultado.Content = Globales.numeros[0];
                Display.Text = "";
                Globales.numeros.RemoveAt(0);
                //reiniciar();
            }
            catch
            {
                resultado.Content = "EXPRESIÓN MAL FORMADA";
            }
        }
        private void rellenar(char operador) //A partir dela información del Display crea las listas

        {
            if (Globales.num_parcial != "")
            {
                Globales.operadores.Add(operador.ToString());
                Globales.numeros.Add(Convert.ToDouble(Globales.num_parcial));
                Globales.num_parcial = "";
            }
            else
            {
                resultado.Content = "EXPRESIÓN MAL FORMADA";
            }
        }

        private void borrar(object sender, RoutedEventArgs e)
        {
            if (Display.Text.Length > 0)
            {
                Display.Text = Display.Text.Remove(Display.Text.Length - 1);
                resultado.Content = Display.Text;
            }
        }

        private void reiniciar(object sender, RoutedEventArgs e)
        {
            Display.Text = "";
            resultado.Content = Display.Text;
            Globales.num_parcial = "";
            Globales.numeros.Clear();
            Globales.operadores.Clear();
        }
    }
}
