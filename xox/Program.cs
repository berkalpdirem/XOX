using System.Security.Cryptography.X509Certificates;

namespace xox
{
    internal class Program
    {
        static void Main(string[] args)
        {
            restart:
            //3'e 3lük matrix'in oluşturulması
            char[,] arr = new char[3, 3] {{'-','-','-'},
                                          {'-','-','-'},
                                          {'-','-','-'}};

            List<char> drawList = new List<char>();
            // Oynanacak Sembolün  belirlenmesi
            sembolSelection:
            Console.WriteLine("Lütfen oynamak istediğiniz karekteri seçiniz(x veya o)");
            string selection = Console.ReadLine();

            char humanSembol;
            char yzSembol;
            if (selection == "x")
            {
                humanSembol = Convert.ToChar(selection);
                yzSembol = 'o';
            }
            else if (selection == "o")
            {
                humanSembol = Convert.ToChar(selection);
                yzSembol = 'x';
            }
            else
                goto sembolSelection;


            ayrac();
            visualization(arr);
            ayrac();
            for (int i = 0; i < 6; i++)
            {
                //İnsan Hamlesi
                if (drawCondition(arr, drawList))
                {
                    break;
                }
                
                charPlacementHuman(arr, humanSembol);
                visualization(arr);
                ayrac();
                if (winConditions(arr))
                {
                    Console.WriteLine("Kazandınız!!!");
                    break;
                }

                //YZ Hamlesi
                Console.WriteLine("YZ Hamlesi");
                if (drawCondition(arr, drawList))
                {
                    break;
                }
                charPlacementYZ(arr,humanSembol,yzSembol);
                visualization(arr);
                ayrac();
                if (winConditions(arr))
                {
                    Console.WriteLine("YZ Kazandı !!!");
                    break;
                }
            }
            //Yeni oyun yada programdan çıkış komutunun alınması
            ayrac();
            command:
            Console.WriteLine("Yeni Oyun İçin 'new' yazınız");
            Console.WriteLine("Programdan çıkmak için 'exit' yazınız ");
            string command = Console.ReadLine();

            if (command == "new")
            { 
                Console.Clear();
                goto restart;
            }
            else if (command =="exit")
            {
                goto cıkıs;
            }
            else
            {
                goto command;
            }

            cıkıs:
            Console.WriteLine("Güle Güle");
        }

        
        #region Metodlar
        /// <summary>
        /// Consolda çıktyılardan sonra ayraç kullanımı
        /// </summary>
        static void ayrac(char sembol = '/', int adet = 30)
        {
            Console.WriteLine(new string(sembol,adet));
        }

        /// <summary>
        /// Oluşturulan Matrixin Görselleştirilmesi
        /// </summary>
        /// <param name="arr"></param>
        static void visualization(char[,] arr)
        {
            int xBorder=arr.GetLength(0);
            int yBorder=arr.GetLength(1);
            arr.GetLength(1);
            for (int x = 0; x <xBorder ; x++)
            {
                for (int y = 0; y < yBorder; y++)
                {
                    Console.Write(arr[x, y] + " ");
                }
                Console.WriteLine();
            }
        }


        /// <summary>
        /// Oynanmak istenen alana seçilen sembolun yerleştirilmesi
        /// </summary>
        /// <param name="arr"></param>
        static void charPlacementHuman(char[,]arr , char sembol)
        {
            char[] subCoorList = {'0', '1', '2' };
            
            Console.WriteLine("Lütfen Oynamak İstediğiniz Alanın Koordinatlarını (satırNo),(SutunNo) formatında yazınız.");
            coorinatSelection:
            string input = Console.ReadLine();
            
            // İnputun İstenilen Formatta gelmesinin Kontrolü
            if (input.Length != 3)
            {
                Console.WriteLine("Lütfen y,x formatında değer giriniz");
                goto coorinatSelection;
            }
            else if (input[1] != ',')
            {
                Console.WriteLine("Lütfen y,x formatında değer giriniz");
                goto coorinatSelection;
            }
            else if (!subCoorList.Contains(input[0]) || !subCoorList.Contains(input[2]))
            {
                Console.WriteLine("Lütfen Girmiş Olduğunuz Koordinatlar Verilern Matrix Sınırları İçerisinde Olabilecek Koordinatlar Olsun");
                goto coorinatSelection;
            }

            int x = int.Parse(input[0].ToString());
            int y = int.Parse(input[2].ToString());

            // Boş alana hamle oynama kontrolü
            if (arr[x,y] == '-')
            {
                arr[x, y] =sembol;
            }
            else
            {
                Console.WriteLine("Belirtilen alan dolu Lütfen Yeni Bir Koordinat Giriniz");
                goto coorinatSelection;
            }

        }

        /// <summary>
        /// YZ'nin oynayacağı alanı belirlemesi
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="sembol"></param>
        static void charPlacementYZ(char[,]arr , char humanSembol, char yzSembol)
        {
            int x;
            int y;

            //Kazanma durumu var ise yapılacak hamle
            for (x = 0; x < 3; x++)
            {
                for (y = 0; y < 3; y++)
                {
                    if (arr[x, y] == '-')
                    {
                        arr[x, y] = yzSembol;
                        if (winConditions(arr))
                        {
                            return;
                        }
                        arr[x, y] = '-';
                    }
                    
                }
            }

            // Kaybetme durumu var ise ilgili durumu engelleyen hamle
            for (x= 0; x < 3; x++)
            {
                for (y= 0; y < 3; y++)
                {
                    if (arr[x,y] == '-')
                    {
                        arr[x, y] = humanSembol;
                        if (winConditions(arr))
                        {
                            arr[x, y] = yzSembol;
                            return;
                        }
                        arr[x, y] = '-';
                    }
                }
            }
            
            //Kazanma durumu yada kaybetme durumu yoksa random olarak hamle yap.
            repatRandomYZ:
            Random rnd = new Random();
            x = rnd.Next(0, 3);
            y = rnd.Next(0, 3);
            if (arr[x,y]== '-')
            {
                arr[x,y]= yzSembol;
            }
            else
            {
                 goto repatRandomYZ;
            }
        }

        /// <summary>
        /// Kazanma olasılıklarının hesaplanması
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        static bool winConditions(char[,]arr )
        {
            //Dikey Kontrol
            for (int x = 0; x < 3; x++)
            {
                if (arr[x, 0] != '-' && arr[x, 0] == arr[x, 1] && arr[x,0] == arr[x,2])
                {
                    return true;
                }
            }

            //Yatay Kontrol
            for (int y = 0; y < 3; y++)
            {
                if (arr[0, y] != '-' && arr[0, y] == arr[1, y] && arr[0,y] == arr[2,y])
                { 
                    return true;
                }
            }

            //Çapraz Kontrol
            if (arr[0,0] != '-' && arr[0,0] == arr[1,1] && arr[0,0] == arr[2,2])
            {   
                return true;
            }

            if (arr[0, 2] != '-' && arr[0, 2] == arr[1, 1] && arr[0, 2] == arr[2, 0])
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Beraberlik durumunu kontrol eder
        /// </summary>
        static bool drawCondition(char[,] arr , List<char>drawList)
        {
            drawList.Clear();
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    drawList.Add(arr[x,y]);
                }
            }

            if (!drawList.Contains('-'))
            {
                Console.WriteLine("Kazanan Olmadı,Berabere !!!");
                return true;
            }

            return false;
        }
        #endregion
    }
}