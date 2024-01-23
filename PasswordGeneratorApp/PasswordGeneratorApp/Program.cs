using System;
using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Tracing;
using System.IO;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Threading.Channels;
using System.Xml;


namespace passwordGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("This is a password generator program, enter some memorable words and the program will generate and save a password.\n\n");
            options();

        }

        static void options()
        {          
            Console.WriteLine("1: Words\n2: Passwords \n3: Information \n4: Amount of guesses \n5: Quit");
            string input = Console.ReadLine();

            //Words
            if (input == "1")
            {
                Console.Clear();
                wordControl();
            }

            //Passwords
            else if (input == "2")
            {
                Console.Clear();
                passwordControl();
            }

            //Info
            else if (input == "3")
            {
                Console.Clear();
                Console.WriteLine("This is a program which allows you to input " +
                                  "words which are memorable to you and generates\n" +
                                  "passwords with a custom amount of words. Words " +
                                  "and passwords are saved to a textfile.\n ");
                options();
            }

            //Guesses
            else if (input == "4")
            {
                combinations();
            }

            //Quit
            else if (input == "5")
            {
                Console.Clear();
                Console.WriteLine("\nGoodbye");
                Thread.Sleep(5000);
                Environment.Exit(0);

            }
            
            //Validation
            else
            {
                Console.Clear();
                Console.WriteLine("\nInvalid Input");
                options();
            }
        }

        static void wordControl()
        {
            //setup
            var wordPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\MemorableWords.txt"));
            StreamWriter wordW = new StreamWriter(wordPath, append: true);
            Console.WriteLine("1: Add Word\n" + "2: View Words\n" + "3: Remove Word\n" + "4: Erase all Words\n" + "5: Go back");
            string input = Console.ReadLine();


            //add
            if (input == "1")
            {
                
                Console.Clear();
                Console.WriteLine("Please type the word you would like to add to your dictionary:");

                string word = Console.ReadLine();
                wordW.WriteLine(word);
                wordW.Close();


                Console.WriteLine($"Word: '{word}' has been successfully added\n");

                options();
               
            }

            //view
            else if (input == "2")
            {

                Console.Clear();
                wordW.Close();


                var lines = File.ReadAllLines(wordPath);

                Console.WriteLine("Your current words are:\n");
                foreach (var line in lines)
                {
                    if ((line != null) && (line != "") && (line != " "))
                    {
                        Console.WriteLine(" " + line);
                    }


                    else if (File.ReadAllText(wordPath).Length == 0)
                    {
                        Console.Clear();
                        wordW.Close();

                        Console.Write("\nYou have no words, return to menu to add some");
                        wordControl();
                    }



                    else
                    {


                        continue;
                    }



                }

                Console.WriteLine("");
            }

            //remove
            else if (input == "3")
            {
                wordW.Close();
                Console.Clear();
                Console.WriteLine("Please enter the word you would like to remove:\n");
                var lines = File.ReadAllLines(wordPath);

                bool found = false;

                //check if null
                if (File.ReadAllText(wordPath).Length == 0)
                {
                    Console.Clear();
                    Console.Write("You have no words\n");
                    wordControl();
                }

                //view ported
                foreach (var line in lines)
                {
                    if ((line != null) && (line != "") && (line != " "))
                    {
                        Console.WriteLine(" " + line);
                    }
                    else if (File.ReadAllText(wordPath).Length == 0)
                    {
                        Console.Clear();
                        wordW.Close();

                        Console.Write("\nYou have no words, return to menu to add some");
                        wordControl();
                    }
                    else
                    {
                        continue;
                    }
                }

                string remove = Console.ReadLine();

                wordW.Close() ;
                if (remove == null || (remove.Contains(" ") == true))
                {
                    Console.Clear();
                    Console.WriteLine("Invalid Input, do not include spaces");
                    wordControl();
                }
                
                else
                {
                    string[] readText = File.ReadAllLines (wordPath);

                    //validation
                    if (readText.Contains(remove))
                    {
                        found = true;
                    }
                    else
                    {
                        found = false;
                    }

                    File.WriteAllText(wordPath, String.Empty);

                    using (StreamWriter writer = new StreamWriter(wordPath))
                    {
                        foreach (string s in readText)
                        {
                            if (!s.Equals(remove))
                            {
                                writer.WriteLine(s);
                            }
                        }
                    }


                }


                if (found == true)
                {
                    Console.WriteLine($"Word '{remove}' has been removed");
                }
                else
                {
                    Console.WriteLine("Not found\n");
                }

            }

            //Erase
            else if (input == "4")
            {
                wordW.Close ();
                Console.Clear();
                Console.WriteLine("You have selected erase all words, press 1 to confirm, press anything else to return");

                string confirm = Console.ReadLine();

                if (confirm == "1")
                {
                    wordW.Close();
                    Console.WriteLine("All memorable words have been erased\n");
                    File.WriteAllText(wordPath, String.Empty);
                }

                else
                {
                    wordControl();
                }

            }

            //Back
            else if (input == "5")
            {
                wordW.Close ();
                Console.Clear();
                options();
            }

            //validation
            else
            {
                wordW.Close () ;
                Console.Clear();
                Console.WriteLine("Invalid Input");
                wordControl();
            }

            
            options();

        }

        static void passwordControl()
        {
            //setup
            
            var passPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Passwords.txt"));
            StreamWriter passW = new StreamWriter(passPath, append: true);
            var words = File.ReadAllLines(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\\..\\..\\MemorableWords.txt")));
            Console.WriteLine("1: Create Password\n" + "2: View Passwords\n" + "3: Remove Password\n" + "4: Erase All Passwords\n" + "5: Go Back");
            var input = Console.ReadLine();

            //create
            if (input == "1") 
            {
                passW.Close ();
                Console.Clear();

                Console.WriteLine("What is this password for?");
                var platform = Console.ReadLine();




                Console.WriteLine("How many words in this passwords");
                Random r = new Random();

                string newPass = null;

                string num = Console.ReadLine();

                if (int.TryParse(num, out int value))
                {
                    int intNum = Int32.Parse(num);

                    for (int i = 0; i < intNum; i++)
                    {
                        newPass = newPass + (words[r.Next(0, words.Length)]);
                    }

                    Console.WriteLine($"Your new {platform} password is: " + newPass);
                    passW.WriteLine(platform + ": " + newPass);
                    passW.Close();

                    newPass = null;
                }

                else
                {
                    Console.WriteLine("\nInvalid Input\n");
                    passwordControl();
                }

            }

            //view
            else if (input == "2")
            {
                Console.Clear();
                passW.Close();
                
                var lines = File.ReadAllLines(passPath);

                Console.WriteLine("Your current words are:\n");


                if (File.ReadAllText(passPath).Length == 0)
                {
                    Console.Clear();
                    Console.Write("You have no passwords\n");
                    passwordControl();
                }

                else
                {
                    foreach (var line in lines)
                    {
                        if ((line != null) && (line != "") && (line != " "))
                        {
                            Console.WriteLine(line);
                        }

                        else
                        {
                            continue;
                        }


                    }
                }

                Console.WriteLine("\n");
            }

            //remove
            else if (input == "3")
            {
               
                passW.Close ();
                Console.Clear();
                var lines = File.ReadAllLines(passPath);
                Console.WriteLine("Please type out which of the following platform and password you wish to erase:\nTry to be as specific as possible\n");

                //check if null
                if (File.ReadAllText(passPath).Length == 0)
                {
                    Console.Clear();
                    Console.Write("You have no passwords\n");
                    passwordControl();
                }

                //view ported
                foreach (var line in lines)
                {
                    if ((line != null) && (line != "") && (line != " "))
                    {
                        Console.WriteLine(" " + line);
                    }
                    else if (File.ReadAllText(passPath).Length == 0)
                    {
                        Console.Clear();
                        passW.Close();

                        Console.Write("\nYou have no words, return to menu to add some");
                        wordControl();
                    }
                    else
                    {
                        continue;
                    }
                }

                string remove = Console.ReadLine();

                if (remove == null || (remove.Contains(" ") == true))
                {
                    Console.Clear();
                    Console.WriteLine("Invalid Input, do not include spaces");
                    wordControl();
                }

                else
                {
                    string[] readText = File.ReadAllLines(passPath);

                   

                    File.WriteAllText(passPath, String.Empty);

                    using (StreamWriter writer = new StreamWriter(passPath))
                    {
                        foreach (string s in readText)
                        {
                            if (!s.Contains(remove))
                            {
                                writer.WriteLine(s);
                            }
                        }
                    }

                }

                Console.WriteLine("Password removed\n");
            }
            
            //Erase
            else if(input == "4")
            {
                Console.Clear();    
                passW.Close();
                Console.WriteLine("You have selected erase all passwords press 1 to confirm, press anything else to return");

                string confirm = Console.ReadLine();

                if (confirm == "1")
                {
                    Console.Clear() ;
                    passW.Close();
                    Console.WriteLine("All memorable words have been erased\n");
                    File.WriteAllText(passPath, String.Empty);
                }

                else
                {                  
                    passwordControl();
                }
            }

            //Back
            else if (input == "5")
            {
                passW.Close();
                Console.Clear();
                options();
            }

            else
            {
                passW.Close (); 
                Console.Clear();
                Console.WriteLine("Invalid Input");
                passwordControl();
                

            }
            
            options();
        }

         static void combinations()
        {
            Console.WriteLine("\nEnter your password you would like to test:\n");

            string password = Console.ReadLine();

          
           
           
            int r = password.Length;          
         
            double computerSpeed = Math.Pow(2, 9);

            double trys = Math.Pow(95, r);

            double computertime = ((trys / computerSpeed)) / 31536000;

            Console.WriteLine(password.Length);

            Console.WriteLine($"There are {trys} possible combinations given the length of your password.\n" +
         
            $"For the average computer of 2Ghz it would take {computertime} years.\n\n" +
            $"However brute force programs use more advanced methods such as using dictionarys\n" +
            $"and therefore guess much faster.");
          
            options();
        }
    }
}









