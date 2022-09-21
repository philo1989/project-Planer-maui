using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics.Text;
using ProjektPlanerAndRessourcenManager.DbModels;
using ProjektPlanerAndRessourcenManager.Pages;

namespace ProjektPlanerAndRessourcenManager
{
    public class RandomColor
    {
        public bool Hello()
        {
            return true;
        }
       
        public Color RndRGBValue()
        {
            System.Random rnd = new System.Random();
            return new Color(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256));
        } 
        public Color GetColor()
        {
            System.Random rnd = new System.Random();
            rnd.Next(0, 256);
            return new Color(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256));
        }
        public Color TranslateDbColor(string color)
        {
            for (int i = 0; i < color.Length; i++)
            {
                //string tmpcolorR = color.Substring(i, 2);
                //string tmpcolorG = color.Substring(i, 2);
                //string tmpcolorB = color.Substring(i, 2);

            }
            string tmpcolorR = color.Substring(1,2);
            string tmpcolorG = color.Substring(3, 2);
            string tmpcolorB = color.Substring(5, 2);
            Color colorResult = new Color(int.Parse(tmpcolorR, NumberStyles.HexNumber), int.Parse(tmpcolorG, NumberStyles.HexNumber), int.Parse(tmpcolorB, NumberStyles.HexNumber));
            
            return colorResult;
        }
        public string HexString(int digitNumber)
        {
            string result = "";
            //int intCon = 0;
            System.Random rnd = new System.Random();
            for (int i = 0; i < digitNumber; i++) {
                //result += rnd.Next(0, 17).ToString();

                int tmpResult = rnd.Next(0, 16);
                if (tmpResult >= 10) {
                    result += tmpResult.ToString("X"); //X prmeter makes it to convert the int values to hex vlues nd returns them as string
                    /*result += ToHex(tmpResult);*/ }
                else { result += tmpResult; }

                
                //intCon += rnd.Next(17,34);

            }
            return result;
        }
        private char ToHex(int value)
        {
            char result = '?';
            if (value < 10 || value > 15) { return result; }
            else { 
            switch (value)
            {
                case 10:
                    result = 'A';
                    break;
                case 11:
                    result = 'B';
                    break;
                case 12:
                    result = 'C';
                    break;
                case 13:
                    result = 'D';
                    break;
                case 14:
                    result = 'E';
                    break;
                case 15:
                    result = 'F'; 
                    break;
                default:
                    result = '!';
                    break;
            }


            return result;
            }
        }
    }
}