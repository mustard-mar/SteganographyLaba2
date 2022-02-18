using System;
using System.Collections.Generic;
using System.Text;

namespace SteganographyLaba2
{
    class Program
    {
        public static void Main(string[] args)
        {
            string sourceImage = @"C:\Users\mustard\source\repos\SteganographyLaba2\SteganographyLaba2\kodim03.bmp";
            string stegImage = @"C:\Users\mustard\source\repos\SteganographyLaba2\SteganographyLaba2\kodimSteg.bmp";
            string message = "TheCakeIsaLie";
            uint startIndex = 3;// индекс с которого начинается запись
            string endLabel = "ge@Q/0";//метка которая сигнализирует об оканчании нашего сообщения
                                        // добавляется к сообщению
            uint coefK = 2;//  обычный целый коэффициент, используется для расчета шага(функция step)
            HiddenMessage.hideMessage(sourceImage, stegImage, message, startIndex, endLabel, coefK);
            
            string mess = HiddenMessage.showMessage(stegImage, startIndex, endLabel, coefK);
            Console.WriteLine("Полученное сообщение: " + mess);
        }
    }
    class HiddenMessage
    {
        static public void hideMessage(string source,string steg,string mess,uint startInd, string endLabel, uint coefK)
        {
            Bmp sourceBmp = new Bmp(source);
            sourceBmp.ImageRead();
            Color[,] image = sourceBmp.image;
            //Bmp stegBmp = new Bmp(sourceBmp, steg);
            mess = "" + (char)(mess.Length * 8) + mess;// сохраняем информацию о количестве вставленных бит
            byte[] meEndLabel = createByteArray(mess, endLabel);
            uint count = startInd;
            for (int j = 0; j < meEndLabel.Length; j++)
            {
                for (int i = 0; i < 8; i++)
                {
                    count += step(count, coefK);
                    byte l = getBit( meEndLabel[j],i);
                    Color t = image[count / sourceBmp.ih.biWidth, count % sourceBmp.ih.biWidth];
                    if (l == 1)
                    {
                        t.blue = setBit(t.blue, 0);
                    }
                    else
                    {
                        t.blue = clearBit(t.blue,0);
                    }
                }     
            }
            sourceBmp.ImageWrite(steg);
        }

        private static uint step(uint count, uint coefK)
        {
            uint sum = 0; 
            while (count > 0)
            {
                if (0x01 == (count & 0x01)) sum++;
                count = count >> 1;
            }

            return coefK * sum;
        }

        private static byte[] createByteArray(string mess, string endLabel)
        {
            byte[] res = new byte[mess.Length + endLabel.Length];
            int count = 0;
            for (int i = 0; i < mess.Length; count++,i++)
            {
                res[count] = (byte)mess[i];
            }
            for (int i = 0; i < endLabel.Length; count++, i++)
            {
                res[count] = (byte)endLabel[i];
            }
            return res;
        }

        static public string showMessage(string source, uint startInd, string endLabel, uint coefK)
        {
            Bmp sourceBmp = new Bmp(source);
            string mess = "";
            sourceBmp.ImageRead();
            Color[,] image = sourceBmp.image;
            uint count = startInd;
            while(!mess.Contains(endLabel))
            {
                
                byte symb = 0;
                for (int i = 0; i < 8; i++)
                {
                    count += step(count, coefK);
                    Color t = image[count / sourceBmp.ih.biWidth, count % sourceBmp.ih.biWidth];
                    byte tmp = getBit(t.blue,0);
                    if (tmp == 1)
                    {
                        symb = setBit(symb, i);
                    }
                    else
                    {
                        symb = clearBit(symb, i);
                    }                    
                }
                mess = mess + (char)symb;
            }
            mess = mess.Substring(0, mess.Length - endLabel.Length);
            byte countOfBits =  (byte)mess[0];
            Console.WriteLine("Количество бит: " + countOfBits);
            mess = mess.Substring(1,mess.Length-1);
            return mess;
        }
        static public byte setBit(byte bt, int i)
        {
            return (byte)(bt | (1 << i));
        }
        static public byte clearBit(byte bt, int i)
        {
            return (byte)(bt & (~(1 << i)));
        }
        static public byte getBit(byte bt, int i)
        {
            return (byte)((bt & (1 << i))>>i);
        }
    }
    
}
