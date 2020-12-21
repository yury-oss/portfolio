using System;
using System.Collections.Generic;
using System.IO;

namespace BuildUtility
{
    class Program
    {
        string myfile = @" ";
        string Testfile = @" ";
        public List<Target> listOfTargets = new List<Target>(); //список задач
        public List<string> targetsNames = new List<string>();
        Target t;
        string path;
        bool mypath =false;
        
        static void Main(string[] args)
        {
            Program pg = new Program();
            pg.Readfile();
        }
        public void Readfile()
        {
                Console.WriteLine("Введите 0 для запуска моего файла иначе что-то другое");
            if (Console.ReadLine() == "0")
            {
                mypath = true;
                path = myfile;
            } else { path = Testfile; }
            try
            {
                using (StreamReader sw = new StreamReader(path, System.Text.Encoding.Default))
                {
                    string text;
                    //прохлжу по makefile чтобы найти задачи и их активности
                        int indexofTarget = -1;
                    int indexforactivity = -1;
                    while ((text = sw.ReadLine()) != null) // читаю файл
                    {
                        Console.WriteLine("read.............");
                        string text2 = text;
                        if (!text.StartsWith(" ")) //если начинается не с пробела  создаю задачу и добавляю в список
                         {
                            indexofTarget++;
                            indexforactivity++;
                               string[] splitedtext = text.Split(':');
                                
                            if (!targetsNames.Contains(splitedtext[0]) )
                            {
                               t = new Target(); 
                               t.targetName = splitedtext[0];
                               listOfTargets.Add(t);
                                targetsNames.Add(splitedtext[0]);
                                
                            }
                                string[] splitedtext2 = text2.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            if (splitedtext2.Length > 1)
                            {
                                for (int i = 1; i < splitedtext2.Length; i++)
                                {
                                        if (!targetsNames.Contains(splitedtext2[i]))
                                        {   
                                            targetsNames.Add(splitedtext2[i]);
                                        t = new Target();
                                        t.targetName = splitedtext2[i];
                                        listOfTargets.Add(t);
                                        }
                                    if (!listOfTargets[indexofTarget].Dependency.Contains(t))
                                    {
                                        listOfTargets[indexofTarget].Dependency.Add(t);

                                    }
                                    
                                        
                                }
                            }
                        }
                            else // если с пробела то это активность , добавляю ее  список активностей задачи
                             {
                            listOfTargets[indexforactivity].Activities.Add(text.ToString());
                            Console.WriteLine(indexforactivity + " added" + text.ToString());
                        }
                        Console.Clear();
                        if (listOfTargets.Count >= 1000)
                        {
                            Console.WriteLine("воизбежание переполнения стека чтение отановлено, для продолжения демонстрации ");
                            break;
                        }
                       
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            if (mypath)Showmenu(); // просто отображает список всех задач и ждет ввода строки 
            Console.WriteLine(listOfTargets[0].targetName  + "  нажмите любую клавишу чтобы собрать" );
            Console.ReadLine();
            listOfTargets[0].MakeActivity(true);
            Console.ReadLine();

        }
        
        public void Showmenu()
        {
            Console.WriteLine("   All targets:" + '\n');
            foreach (Target item in listOfTargets)
            {
                Console.WriteLine(">>>>>>>>>>>>" +  item.targetName );
                foreach (Target item2 in item.Dependency)
                {
                Console.WriteLine(item2.targetName);

                }
                foreach (string act in item.Activities)
                {
                Console.WriteLine("             " + act);

                }
            }
            while (true)
            {

            Console.WriteLine("enter target name");
                foreach (Target item in listOfTargets)
                {
                    item.done = false; 
                }
            // Console.WriteLine(listOfTargets[1].targetName + " " + listOfTargets[1].Dependency.Count +" "+ listOfTargets[1].Dependency[0].targetName + " ");
            string entertargetname =  Console.ReadLine();
            for (int i = 0; i < listOfTargets.Count; i++)
            {

                if (listOfTargets[i].targetName == entertargetname) listOfTargets[i].MakeActivity(false); // выполняю задачу 
            }
            }
           
            
        }
    }
     class Target
    {
        public string targetName;
        public List<Target> Dependency = new List<Target>();
        public List<string> Activities = new List<string>();
        public bool done ;
        
        public void MakeActivity(bool bigfile)
        {
            
                
            if (done) return;
            done = true;
            if (Dependency !=null)
            {
                for (int i = 0; i < Dependency.Count; i++)
                {
                    Dependency[i].MakeActivity(bigfile);

                }
                
                    
                
            }
            for (int i = 0; i < Activities.Count; i++)
            {
                 Console.WriteLine(Activities[i].ToString());

            }
            if (bigfile) Console.WriteLine(" activity from " + targetName);

        }
    }
}
