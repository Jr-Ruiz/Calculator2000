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
            //al inicializar la calculadora se deshabilitan los operadores y coma
            Porcentaje.IsEnabled = false;
            Div.IsEnabled = false;
            Mult.IsEnabled = false;
            Resta.IsEnabled = false;
            Suma.IsEnabled = false;
            BComa.IsEnabled = false;
            Igual.IsEnabled = false;
        }

        static class Globales
        {
            public static string num_parcial = "";
            public static List<double> numeros = new List<double>(); //Lista para almacenar los números de la operación
            public static List<string> operadores = new List<string>(); //Lista para almancenar los operadores
            public static char operador;
        }

        private void escribir(object sender, RoutedEventArgs e)
        {
            Button boton = sender as Button;
            Display.Content += boton.Content.ToString();
            resultado.Content = Display.Content;

            //llamada al metodo para inhabilitar los operadores o rehabilitarlos 
            inhabilitarOperadores();

            //si el boton clicado es un operador, tambíen hay que desactivar los botones de operacion
            if (boton.Name == "Porcentaje" || boton.Name == "Div" || boton.Name == "Mult" ||
                boton.Name == "Resta" || boton.Name == "Suma" || boton.Name == "BComa")
            {
                Porcentaje.IsEnabled = false;
                Div.IsEnabled = false;
                Mult.IsEnabled = false;
                Resta.IsEnabled = false;
                Suma.IsEnabled = false;
                BComa.IsEnabled = false;
                Igual.IsEnabled = false;
            }
        }

        private void calcular(object sender, RoutedEventArgs e)
        {

            Button boton = sender as Button;

            //convertimos el string del Display en un array de caracteres
            char[] caracter = Display.Content.ToString().ToCharArray();

            //Recorremos el array de caracteres
            for (int i = 0; i < Display.Content.ToString().Length; i++)
                {
                Globales.operador = caracter[i];
                    
                switch (caracter[i])
                {
                    //llamamos la función que rellena las listas
                    case 'x':
                        rellenar(caracter[i]);
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
            Globales.numeros.Add(Convert.ToDouble(Globales.num_parcial));
            Globales.num_parcial = "";
          
            while (Globales.operadores.Contains("x") || Globales.operadores.Contains("/") || Globales.operadores.Contains("%"))
            {
                for (int i = 0; i < Globales.operadores.Count; i++)
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
                        try //este try-catch no es necesario porque el error no salta nunca
                        {
                            Globales.numeros[i] = Globales.numeros[i] * Globales.numeros[i + 1] / 100;
                            Globales.numeros.RemoveAt(i + 1);
                            Globales.operadores.RemoveAt(i);
                        }
                        catch (Exception ex)
                        {
                            resultado.Content = "ERROR: " + ex.ToString;
                        }
                    }
                }
            }

            while (Globales.operadores.Contains("+") || Globales.operadores.Contains("-"))
            {
                for (int i = 0; i < Globales.operadores.Count; i++)
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
            Pantalla.Items.Add(Display.Content + "=" + Globales.numeros[0]);
            resultado.Content = Globales.numeros[0];
            Display.Content = "";
            Globales.numeros.RemoveAt(0);
 
            //llamada al metodo para inhabilitar los operadores una vez que se envíe la operacion al item
            inhabilitarOperadores();
        }

        private void rellenar(char operador) //A partir de la información del Display crea las listas
        {
            Globales.operadores.Add(operador.ToString());
            Globales.numeros.Add(Convert.ToDouble(Globales.num_parcial));            
            Globales.num_parcial = "";
        }

        private void borrar(object sender, RoutedEventArgs e)
        {
            //try-catch para controlar la excepcion que saltaba al darle a borrar al inicializar la calculadora 
            try
            {
                if (Display.Content.ToString().Length > 0)
                {
                    Display.Content = Display.Content.ToString().Remove(Display.Content.ToString().Length - 1);
                    resultado.Content = Display.Content;
                }
            } catch (NullReferenceException ex) {
                //este mensaje no aparece porque aparece el del segundo try-catch dentro del metodoinhabilitarOperadores()
                resultado.Content = "ERROR: " + ex.ToString;
            }
             //llamada al metodo para inhabilitar los operadores una vez se borre contenido
             inhabilitarOperadores();
        }

        private void reiniciar(object sender, RoutedEventArgs e)
        {
            Display.Content = "";
            resultado.Content = Display.Content;
            Globales.num_parcial = "";
            Globales.numeros.Clear();
            Globales.operadores.Clear();
            //llamada al metodo para inhabilitar los operadores una vez se borre todo el contenido
            inhabilitarOperadores();
        }

        //metodo void para inhabilitar los operadores cuando no hay contenido en el Display
        private void inhabilitarOperadores()
        {
           //try-catch para controlar la excepcion que saltaba al darle a borrar al inicializar la calculadora 
            try
            {
                //si no hay contenido en el Display se inhabilitan los botones de operación
                if (Display.Content.ToString().Length == 0)
                {
                    Porcentaje.IsEnabled = false;
                    Div.IsEnabled = false;
                    Mult.IsEnabled = false;
                    Resta.IsEnabled = false;
                    Suma.IsEnabled = false;
                    BComa.IsEnabled = false;
                    Igual.IsEnabled = false;
                }
                else   //de lo contrario, se vuelven a habilitar
                {
                    Porcentaje.IsEnabled = true;
                    Div.IsEnabled = true;
                    Mult.IsEnabled = true;
                    Resta.IsEnabled = true;
                    Suma.IsEnabled = true;
                    BComa.IsEnabled = true;
                    Igual.IsEnabled = true;
                }
            } catch(NullReferenceException ex)
            {
                resultado.Content = "ERROR: No hay contenido que borrar!";
            }

        }
    }
}
