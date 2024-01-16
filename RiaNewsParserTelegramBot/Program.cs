﻿using NewsPropertyBot.NewClass;
using NewsPropertyBot.ParsingClasses;
using NewsPropertyBot.TelegramBotClass;
using Newtonsoft.Json;
using RiaNewsParserTelegramBot.MyNewConstrucorBlock.PhotoConstructorBlock;
using RiaNewsParserTelegramBot.MyNewConstrucorBlock.PhotoConstructorBlock.Strategies;
using RiaNewsParserTelegramBot.PropertiesClass;
using RiaNewsParserTelegramBot.TelegramBotClass;
using RiaNewsParserTelegramBot.TelegramBotClass.SendStrateges;

class Program
{
    static MyProperties properties = ReadLineProperties();
    static async Task Main()
    {


        Random random = new Random();
        MyPropertiesStatic.MakeStaticProperties(properties);
        MyTelegramBot telegramBot = new MyTelegramBot();
        Parser parser = new Parser(telegramBot);
        PhotoConstructor photoConstructor = new PhotoConstructor();
        MyNewTelegramSendler telegramSendler = new MyNewTelegramSendler(telegramBot);
        MakeImagesFolder();

        
        
        /*
        MyNew myNew;
        myNew = await parser.ParseOneNewAsync("https://ria.ru/20240114/kupyanskoe-1921269575.html");
        telegramSendler.SendNew(myNew, new Title());
        Console.ReadLine();
        */
        

        //await parser.FirstParseAddLinks();
        while (true)
        {
            Console.WriteLine("Начало парсинга");
            List<MyNew> newsList = await parser.ParseNews();
            
            foreach(var myNews in newsList)
            {
                await telegramSendler.SendNew(myNews, new PhotoWithTitleAndDescription());
            }

            Console.WriteLine($"Конец, ожидание {MyPropertiesStatic.timeBetweenMainParseMinutes} минут {DateTime.Now}\n");
            await Task.Delay(TimeSpan.FromMinutes(MyPropertiesStatic.timeBetweenMainParseMinutes));
        }
        Console.ReadLine();
     
    }

    static MyProperties ReadLineProperties()
    {
        string propertiesJSON = string.Empty;
        try{
            using (StreamReader streamReader = new StreamReader("properties.txt"))
                propertiesJSON = streamReader.ReadToEnd();

            if (!string.IsNullOrEmpty(propertiesJSON))
                return JsonConvert.DeserializeObject<MyProperties>(propertiesJSON);
            else
                Console.WriteLine("Файл JSON пуст или не найден.");
        }
        catch (Exception ex){
            Console.WriteLine("Ошибка чтения файла JSON: " + ex.Message);
        }
        return null;
    }   
    static void MakeImagesFolder()
    {
        if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images")))
        {
            Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images"));
        }
    }
    static string ChooseRandomSendStrategy(Dictionary<string, int> weightSendStrategies)
    {
        Random random = new Random();
        int totalWeight = 0;


        foreach (var el in weightSendStrategies)
            totalWeight += el.Value;

        foreach(var el in weightSendStrategies)
        {
            if (random.Next(totalWeight) <= el.Value)
                return el.Key;
            else
                totalWeight -= el.Value;
        }
        return null;
    }
    static ISendNew RandomSendStrategy(Dictionary<string, int> weightSendStrategies)
    {
        string strategy = ChooseRandomSendStrategy(weightSendStrategies);
        switch (strategy)
        {
            case "Title":
                {
                    return new Title();
                    break;
                }
            case "TitleDescription":
                {
                    return new TitleDescription();
                    break;
                }
            case "PhotoWithTitle":
                {
                    return new PhotoWithTitle();
                    break;
                }
        }
        return null;

    }





}
