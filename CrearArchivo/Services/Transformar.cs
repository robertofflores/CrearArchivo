using CrearArchivo.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CrearArchivo.Services
{
    public class Transformar
    {
        string code;
        string mon;
        string fpag;
        string doc;
        string tipocre;
        string cuota;
        string codigolocal;
        string nombrelocal;
        string numlocal;
        string siglalocal;
        int registro;
        string linelocal;

        //public List<File> VariosArchivos()
        //{
        //    DirectoryInfo di = new DirectoryInfo(@"D:\Lee\Xime");
        //    //Console.WriteLine("Search pattern *2* returns:");
        //    var archivos = di.GetFiles("*.txt");
        //    List<FileSystemInfo> files = new List<FileSystemInfo>();
        //    foreach (var arch in archivos) {
        //        files.AddRange(arch);

        //            }

        //    return files;

            //foreach (var fi in di.GetFiles("*2*"))
            //{
            //    //Console.WriteLine(fi.Name);
            //}
       // }
        public List<Liquidacion2> LeerArchivo(string path)
        {
            int counter = 0;
            string line;


            //string path = "C:\\liquidaciones\\CREDITO.txt";

            // Read the file and display it line by line.  
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            var liquidacion = new List<Liquidacion2>();
            //var liquidacion2 = new List<Liquidacion2>();
            while ((line = file.ReadLine()) != null)
            {
                if (line.Substring(0, 1) == "1")
                {
                    code = line.Substring(15, 2);
                    switch (code)
                    {
                        case "08":
                            code = "AMERICAN EXPRESS";
                            break;
                        case "16":
                            code = "VISA";
                            break;
                        case "15":
                            code = "MASTERCARD";
                            break;
                        case "43":
                            code = "MAESTRO";
                            break;
                    }

                }
                if (line.Substring(0, 1) == "2")
                {
                    switch (line.Substring(1, 3))
                    {
                        case "USD":
                            mon = "DOLARES";
                            break;
                    }
                    switch (line.Substring(75, 4))
                    {
                        case "0101":
                            fpag = "Pago Total";
                            break;
                        case "0301":
                            fpag = "Pago 1 de 3";
                            break;
                        case "0302":
                            fpag = "Pago 2 de 3";
                            break;
                        case "0303":
                            fpag = "Pago 3 de 3";
                            break;
                    }
                    switch (line.Substring(93, 2))
                    {
                        case "00":
                            doc = "RECAP PAGADO";
                            break;
                        case "01":
                            doc = "NOTA DE CREDITO";
                            break;
                        case "02":
                            doc = "NOTA DE DEBITO";
                            break;
                        case "03":
                            doc = "DEPOSITO DIRECTO";
                            break;
                        case "04":
                            doc = "VOUCHER RECHAZADO";
                            break;
                        case "05":
                            doc = "VOUCHER ANULADO";
                            break;
                    }
                    switch (line.Substring(199, 2))
                    {
                        case "01":
                            tipocre = "CREDITO CORRIENTE";
                            cuota = "00";
                            break;
                        case "03":
                            tipocre = "DIFERIDO PROPIO PLAN ESPECIAL";
                            cuota = line.Substring(201, 2);
                            break;
                    }

                    if (int.Parse(line.Substring(99, 1)) > 0)
                    {
                        codigolocal = line.Substring(99, 6);
                    }
                    else
                    {
                        codigolocal = line.Substring(100, 5);
                    }

                    System.IO.StreamReader filelocal = new System.IO.StreamReader("\\\\192.168.2.123\\compartir\\localesx.txt");
                    while ((linelocal = filelocal.ReadLine()) != null)
                    {

                        if (codigolocal == linelocal.Substring(0, codigolocal.Length))
                        {
                            string resto = linelocal.Substring(codigolocal.Length + 1, linelocal.Length - codigolocal.Length - 1);
                            int posicion = resto.IndexOf(",");
                            nombrelocal = resto.Substring(0, posicion);
                            resto = resto.Substring(posicion + 1, resto.Length - posicion - 1);
                            posicion = resto.IndexOf(",");
                            numlocal = resto.Substring(0, posicion);
                            siglalocal = resto.Substring(posicion + 1, resto.Length - posicion - 1);
                            break;

                        }
                        registro++;
                    };


                    var newLiquidacion = new Liquidacion2
                    {
                        Codigo = code,
                        Moneda = mon,
                        Recap = line.Substring(4, 14),
                        Voucher = line.Substring(18, 14),
                        RecapCorregido = line.Substring(32, 14),
                        VoucherCorregido = line.Substring(46, 14),
                        Liquidado = line.Substring(60, 13) + "." + line.Substring(73, 2),
                        FormaPago = fpag,
                        Pago = line.Substring(85, 2) + "/" + line.Substring(83, 2) + "/" + line.Substring(79, 4),
                        Documento = doc,
                        Local = nombrelocal,
                        Numero = numlocal,
                        Sigla = siglalocal,
                        Tarjeta = line.Substring(105, 10) + " " + line.Substring(115, 9),
                        Consumo = line.Substring(124, 13) + "." + line.Substring(137, 2),
                        Comision = line.Substring(139, 13) + "." + line.Substring(152, 2),
                        Retencion = line.Substring(154, 13) + "." + line.Substring(167, 2),
                        Iva = line.Substring(169, 13) + "." + line.Substring(182, 2),
                        RetencionIva = line.Substring(184, 13) + "." + line.Substring(197, 2),
                        Credito = tipocre,
                        Cuotas = cuota,
                        ComisionIva = line.Substring(203, 13) + "." + line.Substring(216, 2)
                    };
                    liquidacion.Add(newLiquidacion);
                    counter++;
                }

                // System.Console.WriteLine(line);


                //file.Close();
                //System.Console.WriteLine("There were {0} lines.", counter);
                // Suspend the screen.  
                //System.Console.ReadLine();

            }

            file.Close();
            return liquidacion.ToList();
        }
    }
}
