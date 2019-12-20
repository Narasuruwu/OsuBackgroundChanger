using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsuReplaceBackground
{
    class Program
    {

        private string path;

        private string bgPath;

        private string[] files;

        private string un = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

        private string[] username;

        private string skinName = "Skin = _.NoSkin._";

        private string cfgPth;

        private bool checkSkinFolder = false;

        static void Main(string[] args)
        {
            var instance = new Program();
            instance.setPath();
        }

        public void setPath()
        {
            path = Environment.CurrentDirectory + @"\Data\bg\";

            if (File.Exists(path + "bg.png"))
            {
                bgPath = path + "bg.png";
            }
            else if
            (File.Exists(path + "bg.jpg"))
            {
                bgPath = path + "bg.jpg";
            }

            username = un.Split('\\');

            if (File.Exists(Environment.CurrentDirectory + @"\osu!." + username[username.Length - 1] + ".cfg"))
            {
                cfgPth = Environment.CurrentDirectory + @"\osu!." + username[username.Length - 1] + ".cfg";

                int count = 0;

                string[] settings = File.ReadAllLines(cfgPth);

                bool whilestatement = true;

                bool canProgress = false;

                while (whilestatement)
                {
                    

                    if (settings[count].Contains("#UseSkinBg"))
                    {
                        if (!settings[count].Contains("##"))
                        {
                            canProgress = true;
                        }
                    }
                    if(settings[count].Contains("Skin =") && canProgress)
                    {
                        if (!settings[count].Contains("Editor"))
                        {
                            if (!settings[count].Contains("Ignore"))
                            {
                                if (!settings[count].Contains("Samples"))
                                {
                                    if (!settings[count].Contains("Cursor"))
                                    {
                                        if (!settings[count].Contains("Taiko"))
                                        {
                                            skinName = settings[count];
                                            checkSkinFolder = true;
                                            whilestatement = false;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if(count >= settings.Length - 1)
                    {
                        whilestatement = false;
                    }

                    if (whilestatement)
                    {
                        count++;
                    }
                }
            }

            if (checkSkinFolder)
            {
                replaceSkin();
            }
            else
            {
                replace();
            }
        }

        public void replaceSkin()
        {
            files = Directory.GetFiles(path);

            if (checkSkinFolder)
            {
                if (File.Exists(Environment.CurrentDirectory + @"\Skins\" + skinName.Remove(0, 7) + @"\menu-background.jpg"))
                {
                    foundSkinBackground = true;
                    foreach (var name in files)
                    {
                        Console.WriteLine("skin background!");
                        if (name != bgPath)
                        {
                            File.Copy(Environment.CurrentDirectory + @"\Skins\" + skinName.Remove(0, 7) + @"\menu-background.jpg", name, true);
                        }
                    }
                }
            }

            replace();

        }

        public void replace()
        {
            files = Directory.GetFiles(path);

            if (!foundSkinBackground)
            {
                if (checkSkinFolder)
                {
                    Console.WriteLine("Failed to use background from skin: " + skinName.Remove(0,7));
                }
                foreach(var name in files)
                {
                    if(name != bgPath )
                    {
                        Console.WriteLine("replacing: " + name);
                        File.Copy(bgPath, name, true);
                    }
                }
            }
            Environment.Exit(1);
        }

        private bool foundSkinBackground = false;

    }
}
