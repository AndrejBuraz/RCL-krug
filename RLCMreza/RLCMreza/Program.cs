using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLCMreza
{

    public class Complex
    {
        public int imaginarni;
        public int stvarni;

        public int Stvarni { get => stvarni; set => stvarni = value; }
        public int Imaginarni { get => imaginarni; set => imaginarni = value; }

        public Complex()
        {

        }
        public Complex(int stvarni, int imaginarni)
        {
            this.Stvarni = stvarni;
            this.Imaginarni = imaginarni;
        }   
        public static Complex operator + (Complex prvi, Complex drugi)
        {
            return new Complex(prvi.Stvarni + drugi.Stvarni, prvi.Imaginarni + drugi.Imaginarni);
        }
        public static Complex operator - (Complex prvi, Complex drugi)
        {
            return new Complex(prvi.Stvarni - drugi.Stvarni, prvi.Imaginarni - drugi.imaginarni);
        }

        public override string ToString()
        {
            if(Imaginarni > 0)
            {
                return (String.Format("{0} + {1}i", Stvarni, Imaginarni));
            }
            else
            {
                Imaginarni *= -1;
                return (String.Format("{0} - {1}i", Stvarni, Imaginarni));
            }
            
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            int brojOtpornika, brojKondenzatora, brojZavojnica, i = 0, j = 0;

            //Unos broja komponenti

            unos: Console.WriteLine("Unesi broj otpornika u spoju: ");
            brojOtpornika = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Unesi broj kondenzatora u spoju: ");
            brojKondenzatora = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Unesi broj zavojnica u spoju: ");
            brojZavojnica = Convert.ToInt32(Console.ReadLine());
            if(brojKondenzatora > 99 || brojOtpornika > 99 ||brojZavojnica > 99 )
            {
                Console.WriteLine("U mreži ne može postojati više od 99 elemenata jedne komponente, ponovite unos!");
                goto unos;
            }

            //deklaracija i inicijalizacija izraza polja R,C,L
            Complex[] R = new Complex[brojOtpornika];
            Complex[] C = new Complex[brojKondenzatora];
            Complex[] L = new Complex[brojZavojnica];

            for (i = 0; i < brojOtpornika; i++)
            {
                R[i] = new Complex();
                Console.WriteLine("Unesi realnu vrijednost " + (i + 1) + ". otpornika: ");
                R[i].stvarni = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Unesi imaginarnu vrijednost " + (i + 1) + " otpornika: ");
                R[i].imaginarni = Convert.ToInt32(Console.ReadLine());
            }
            for (i = 0; i < brojKondenzatora; i++)
            {
                C[i] = new Complex();
                Console.WriteLine("Unesi realnu vrijednost " + (i + 1) + ". kondenzatora: ");
                C[i].stvarni = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Unesi imaginarnu vrijednost " + (i + 1) + ". kondenzatora: ");
                C[i].imaginarni = Convert.ToInt32(Console.ReadLine());
            }
            for (i = 0; i < brojZavojnica; i++)
            {
                L[i] = new Complex();
                Console.WriteLine("Unesi realnu vrijednost " + (i + 1) + ". zavojnice: ");
                L[i].stvarni = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Unesi imaginarnu vrijednost " + (i + 1) + ". zavojnice: ");
                L[i].imaginarni = Convert.ToInt32(Console.ReadLine());

            }

            // FREKVENCIJA
            Console.WriteLine("Unesi frekvenciju: ");
            string frekvencijaIzraz = Console.ReadLine();
            double frekvencija = 0;

            //PRETVORI BROJEVE IZ FREKVENCIJA STRINGA U INT
            for (i = 0; i < frekvencijaIzraz.Length; i++)
            {
                char znak = frekvencijaIzraz[i];
                if (Char.IsNumber(znak))
                {
                        frekvencija = (frekvencija * 10) + (frekvencijaIzraz[i] - 48); //ASCII 0-9 raspon: 48-57

                }
                
            }
            char potencija = frekvencijaIzraz[frekvencijaIzraz.Length - 1];
            switch(potencija)
            {
                case '0':
                    frekvencija = frekvencija * Math.Pow(10.0, 1.0);
                    break;

                case 'K':
                    frekvencija = frekvencija * Math.Pow(10.0, 3.0);
                    break;

                case 'M':
                    frekvencija = frekvencija * Math.Pow(10.0, 6.0);
                    break;

                case 'G':
                    frekvencija = frekvencija * Math.Pow(10.0, 9.0);
                    break;
            }
            Console.WriteLine("Unesi topologiju mreže: ");
            Complex[] vrijednosti = new Complex[(brojKondenzatora + brojOtpornika + brojZavojnica)];
            string[] operacija = new string[(brojKondenzatora + brojOtpornika + brojZavojnica - 1)];
            string topologija = Console.ReadLine();
            int brojOperacija = 0, brojVrijednosti = 0;
            for(i = 0; i < topologija.Length; i++)
            {
                vrijednosti[i] = new Complex();
                switch(topologija[i])
                {
                    case 'R':
                        if (Char.IsNumber(topologija[i+2]))
                        {
                            vrijednosti[brojVrijednosti] = R[Convert.ToInt32(topologija[i+1] + topologija[i+2])];
                            i+= 2;
                        }
                        else if(Char.IsNumber(topologija[i + 1]))
                        {
                            vrijednosti[brojVrijednosti] = R[Convert.ToInt32(topologija[i+1])];
                            i++;
                        }
                        brojVrijednosti++;
                        break;

                    case 'L':
                        if (Char.IsNumber(topologija[i + 2]))
                        {
                            vrijednosti[brojVrijednosti] = L[Convert.ToInt32(topologija[i + 1] + topologija[i + 2])];
                            i += 2;
                        }
                        else if (Char.IsNumber(topologija[i + 1]))
                        {
                            vrijednosti[brojVrijednosti] = L[Convert.ToInt32(topologija[i + 1])];
                            i++;
                        }
                        brojVrijednosti++;
                        break;

                    case 'C':
                        if (Char.IsNumber(topologija[i + 2]))
                        {
                            vrijednosti[brojVrijednosti] = C[Convert.ToInt32(topologija[i + 1] + topologija[i + 2])];
                            i += 2;
                        }
                        else if (Char.IsNumber(topologija[i + 1]))
                        {
                            vrijednosti[brojVrijednosti] = C[Convert.ToInt32(topologija[i + 1])];
                            i++;
                        }
                        brojVrijednosti++;
                        break;

                    case '+':
                        operacija[brojOperacija] = "+";
                        brojOperacija++;
                        break;

                    case '|':
                        operacija[brojOperacija] = "|";
                        brojOperacija++;
                        i++; 
                        break;
                }
            }


            Console.WriteLine(vrijednosti[0]);
            Console.ReadKey();


        }
    }
}
