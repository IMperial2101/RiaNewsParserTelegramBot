﻿using NewsPropertyBot.NewClass;
using NewsPropertyBot.TelegramBotClass;
using RiaNewsParserTelegramBot.MyNewConstrucorBlock.PhotoConstructorBlock;
using RiaNewsParserTelegramBot.MyNewConstrucorBlock.PhotoConstructorBlock.Strategies;
using RiaNewsParserTelegramBot.PropertiesClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiaNewsParserTelegramBot.TelegramBotClass
{
    public class PhotoConstructorForSendler
    {

        public string pathToImages = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images");
        PhotoConstructor photoConstructor = new PhotoConstructor();
        public async Task MakePhoto(MyNew myNew, IPhotoConstructorStrategy constructorStrategy,ColorVariationsEnum colorsVariation)
        {
            await photoConstructor.MakePhoto(myNew, constructorStrategy, colorsVariation);
        }
        protected string MakeDescriptionToSend(MyNew myNew)
        {
            Random random = new Random();
            StringBuilder stringBuilder = new StringBuilder();

            int abzatcCount = random.Next(MyPropertiesStatic.minParagraphCount, MyPropertiesStatic.maxParagraphCount);

            if (abzatcCount > myNew.description.Count)
                abzatcCount = myNew.description.Count;
            for (int i = 0; i < abzatcCount; i++)
            {
                if (stringBuilder.Length > MyPropertiesStatic.maxDescripSymbCount)
                    break;
                stringBuilder.Append($"{MyPropertiesStatic.abzatcSmile}  ");
                stringBuilder.AppendLine(myNew.description[i]);
                stringBuilder.AppendLine();
                
            }

            return stringBuilder.ToString();
        }
    }
}