using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Prorations
{
    public class RunDistrict
    {
        #region Attributes
        public string District { get; set; }
        public string Run { get; set; }
        #endregion
    }
    public class ProrationsProject
    {
        #region Main
        public static void Main(string[] args)
        {
            /* operatorFileName,leaseFileName, wellFileName,  */
            string readFileName = string.Empty,
                   yearMonth = string.Empty,
                   district = string.Empty,
                   userOption = string.Empty,
                   filePath = string.Empty;
            bool open;

            DisplayPurpose();
            userOption = GetUserFileOption();

            switch (userOption)
            {
                case "0":  // Do Nothing and Exit Program 
                    break;
                case "1":
                    readFileName = GetReadFileName();
                    if (string.IsNullOrEmpty(readFileName))
                    {
                        // User Attempts Exceeded
                        break;
                    }
                    Console.Clear();
                    yearMonth = GetYearMonth();
                    if (string.IsNullOrEmpty(yearMonth))
                    {
                        // User Attempts Exceeded
                        break;
                    }
                    Console.Clear();
                    district = GetDistrict();
                    if (string.IsNullOrEmpty(district))
                    {
                        // User Attempts Exceeded
                        break;
                    }

                    PleaseWait();

                    ProcessSingleFile(readFileName, yearMonth, district);

                    ExitMessage();
                    break;
                case "2": var districtsToSkip = GetUserDistrictSkipOptions();

                    yearMonth = GetYearMonth();
                    if (string.IsNullOrEmpty(yearMonth))
                    {
                        // User Attempts Exceeded
                        break;
                    }
                    filePath = GetProrationsFilePath();
                    if (string.IsNullOrEmpty(yearMonth))
                    {
                        // User Attempts Exceeded
                        break;
                    }

                    PleaseWait();

                    ProcessFilePath(yearMonth, filePath, districtsToSkip);

                    ExitMessage();
                    break;
                case "deafult": break;

            }

            return;
        }
        #endregion

        #region DisplayMessages
        public static void DisplayPurpose()
        {
            Console.WriteLine("*********************************************************************************");
            Console.WriteLine("* The purpose for this program is too extract data from the Oil Prorations Text *");
            Console.WriteLine("* Files from the Texas Railroad Commission into three output files for          *");
            Console.WriteLine("* Operators, Leases, Wells. Have options of running one file or multiple files  *");
            Console.WriteLine("*********************************************************************************");
            Console.WriteLine("Please press Enter to continue: ");
            Console.ReadLine();
        }
        public static void ExitMessage()
        {
            Console.Clear();
            Console.WriteLine("*********************************************************************************");
            Console.WriteLine("* Thanks for using the Prorations File DB Prep application                      *");
            Console.WriteLine("* Please Press Enter to Exit                                                    *");
            Console.WriteLine("*********************************************************************************");
            Console.ReadLine();
        }
        public static void PleaseWait()
        {
            Console.Clear();
            Console.WriteLine("*********************************************************************************");
            Console.WriteLine("* Please Wait while the Files are Created                                       *");
            Console.WriteLine("*********************************************************************************");
        }
        #endregion

        #region Get
        public static string GetUserFileOption()
        {
            string userOption = string.Empty;
            int userAttempts = 0;

            while (userAttempts < 3 && string.IsNullOrEmpty(userOption))
            {
                Console.Clear();
                Console.WriteLine("*********************************************************************************");
                Console.WriteLine("* Please choose from the following options:                                     *");
                Console.WriteLine("* Entry Number | User Option                                                    *");
                Console.WriteLine("*      1       | Single File Run                                                *");
                Console.WriteLine("*      2       | Multiple File Run                                              *");
                Console.WriteLine("*********************************************************************************");
                userOption = Console.ReadLine();

                if (userOption != "1" && userOption != "2")
                {
                    Console.WriteLine("*********************************************************************************");
                    Console.WriteLine("* User Input Error: Please Enter a valid Entry Number                           *");
                    Console.WriteLine("* Press Enter when ready to continue                                            *");
                    Console.WriteLine("*********************************************************************************");
                    Console.ReadLine();
                    userAttempts++;
                    userOption = string.Empty;
                }
            }

            if (userAttempts == 3)
            {
                Console.WriteLine("*********************************************************************************");
                Console.WriteLine("* Error: Number of user attempts to enter a valid entry number has been reached.*");
                Console.WriteLine("* Maxium Entries Allowed: 3                                                     *");
                Console.WriteLine("* User Attempts : 3                                                             *");
                Console.WriteLine("* Program is Exiting.                                                           *");
                Console.WriteLine("* Please Press Enter                                                            *");
                Console.WriteLine("*********************************************************************************");
                Console.ReadLine();

                userOption = "0";
            }

            return userOption;
        }

        public static string GetReadFileName()
        {
            string fileName = string.Empty;
            int userAttempts = 0;

            while(userAttempts != 3 && string.IsNullOrEmpty(fileName))
            {
                Console.Clear();
                Console.WriteLine("*********************************************************************************");
                Console.WriteLine("* Enter either the file path if the program is running under the same directory *");
                Console.WriteLine("* or enter the full file path with extension. Use the following examples        *");
                Console.WriteLine("* Same Directory Input: oilsch1.txt                                             *");
                Console.WriteLine("* Different Directory Input: C:\\ProrationFiles\\oilsch1.txt                    *");
                Console.WriteLine("*********************************************************************************");
                Console.WriteLine("Please enter the name of the file from the Texas RRC: Ex. (oilsch1.txt)");
                fileName = Console.ReadLine();

                if (!OpenProrationsFile(fileName))
                {
                    Console.WriteLine("*********************************************************************************");
                    Console.WriteLine("* User File Open Error: Please Enter a valid File Name                          *");
                    Console.WriteLine("* Press Enter when ready to continue                                            *");
                    Console.WriteLine("*********************************************************************************");
                    Console.ReadLine();
                    userAttempts++;
                    fileName = string.Empty;
                }
            }
            

            if (userAttempts == 3)
            {
                Console.WriteLine("*********************************************************************************");
                Console.WriteLine("* Error: Number of user attempts to enter a valid File Name has been reached.   *");
                Console.WriteLine("* Maxium Entries Allowed: 3                                                     *");
                Console.WriteLine("* User Attempts : 3                                                             *");
                Console.WriteLine("* Program is Exiting.                                                           *");
                Console.WriteLine("* Please Press Enter                                                            *");
                Console.WriteLine("*********************************************************************************");
                Console.ReadLine();

                fileName = string.Empty;
            }

            return fileName;
        }

        public static string GetYearMonth()
        {
            string yearMon = string.Empty;
            int userAttempts = 0;

            while(userAttempts != 3 && string.IsNullOrEmpty(yearMon))
            {
                Console.Clear();
                Console.WriteLine("*********************************************************************************");
                Console.WriteLine("* Please Enter the Year-Month for the file as the following example:            *");
                Console.WriteLine("* Example: 201705                                                               *");
                Console.WriteLine("*********************************************************************************");
                yearMon = Console.ReadLine();

                if (string.IsNullOrEmpty(yearMon) || yearMon == "")
                {
                    Console.WriteLine("*********************************************************************************");
                    Console.WriteLine("* User Input Error: Please Enter a valid Entry Number                           *");
                    Console.WriteLine("* Press Enter when ready to continue                                            *");
                    Console.WriteLine("*********************************************************************************");
                    Console.ReadLine();
                    userAttempts++;
                    yearMon = string.Empty;
                }
            }          

            if (userAttempts == 3)
            {
                Console.WriteLine("*********************************************************************************");
                Console.WriteLine("* Error: Number of user attempts to enter a valid Year-Month has been reached.  *");
                Console.WriteLine("* Maxium Entries Allowed: 3                                                     *");
                Console.WriteLine("* User Attempts : 3                                                             *");
                Console.WriteLine("* Program is Exiting.                                                           *");
                Console.WriteLine("* Please Press Enter                                                            *");
                Console.WriteLine("*********************************************************************************");
                Console.ReadLine();

                yearMon = string.Empty;
            }

            return yearMon;
        }

        public static string GetDistrict()
        {
            string district = string.Empty;
            int userAttempts = 0;

            while(userAttempts != 3 && string.IsNullOrEmpty(district))
            {
                Console.Clear();
                Console.WriteLine("*********************************************************************************");
                Console.WriteLine("* Please Enter the District for the file. Valid Districts Listed Below.         *");
                Console.WriteLine("* Districts: 1, 2, 3, 4, 5, 6, 6E, 7B, 7C, 8, 8A, 9, 10                         *");
                Console.WriteLine("*********************************************************************************");
                district = Console.ReadLine();

                if (!ValidDistrict(district))
                {
                    Console.WriteLine("*********************************************************************************");
                    Console.WriteLine("* User District Input Error: Please Enter a valid District                      *");
                    Console.WriteLine("* Press Enter when ready to continue                                            *");
                    Console.WriteLine("*********************************************************************************");
                    Console.ReadLine();
                    userAttempts++;
                    district = string.Empty;
                }
            }
            
            if (userAttempts == 3)
            {
                Console.WriteLine("*********************************************************************************");
                Console.WriteLine("* Error: Number of user attempts to enter a valid District has been reached.    *");
                Console.WriteLine("* Maxium Entries Allowed: 3                                                     *");
                Console.WriteLine("* User Attempts : 3                                                             *");
                Console.WriteLine("* Program is Exiting.                                                           *");
                Console.WriteLine("* Please Press Enter                                                            *");
                Console.WriteLine("*********************************************************************************");
                Console.ReadLine();

                district = string.Empty;
            }

            return district;
        }

        public static string GetProrationsFilePath()
        {
            string filePath = string.Empty;
            int userAttempts = 0;

            while(userAttempts != 3 && string.IsNullOrEmpty(filePath))
            {
                Console.Clear();
                Console.WriteLine("*********************************************************************************");
                Console.WriteLine("* Please Enter the File Path of the Oil Proration files. Follow the example:    *");
                Console.WriteLine("* Example: C:\\Users\\UserName\\Prorations                                      *");
                Console.WriteLine("*********************************************************************************");
                filePath = Console.ReadLine();

                if (string.IsNullOrEmpty(filePath) || filePath == "")
                {
                    Console.WriteLine("*********************************************************************************");
                    Console.WriteLine("* User File Path Error: Please Enter a valid File Path                          *");
                    Console.WriteLine("* Press Enter when ready to continue                                            *");
                    Console.WriteLine("*********************************************************************************");
                    Console.ReadLine();
                    userAttempts++;
                    filePath = string.Empty;
                }
            }
            
            if (userAttempts == 3)
            {
                Console.WriteLine("*********************************************************************************");
                Console.WriteLine("* Error: Number of user attempts to enter a valid File Name has been reached.   *");
                Console.WriteLine("* Maxium Entries Allowed: 3                                                     *");
                Console.WriteLine("* User Attempts : 3                                                             *");
                Console.WriteLine("* Program is Exiting.                                                           *");
                Console.WriteLine("* Please Press Enter                                                            *");
                Console.WriteLine("*********************************************************************************");
                Console.ReadLine();

                filePath = string.Empty;
            }

            return filePath;
        }

        public static List<string> GetUserDistrictSkipOptions()
        {
            string userSkipOption = string.Empty;
            List<string> districtsToSkip = new List<string>();
            string userContinue = string.Empty;

            Console.Clear();
            Console.WriteLine("*********************************************************************************");
            Console.WriteLine("* When running multiple files the default setting is to run all Districts for   *");
            Console.WriteLine("* the Year-Month. If you ld like to skip any please choose between the options  *");
            Console.WriteLine("* below. Also all district files to be run:                                     *");
            Console.WriteLine("* Districts: 1, 2, 3, 4, 5, 6, 6E, 7B, 7C, 8, 8A, 9, 10                         *");
            Console.WriteLine("* Entry Number | Explaination                                                   *");
            Console.WriteLine("*      1       | Yes I would like to enter Districts to skip over               *");
            Console.WriteLine("*      2       | No, Run all Districts for Year-Month                           *");
            Console.WriteLine("*********************************************************************************");
            userSkipOption = Console.ReadLine();

            while (userSkipOption != "1" && userSkipOption != "2")
            {
                Console.WriteLine("*********************************************************************************");
                Console.WriteLine("* User District Input Error: Please Enter a valid File Name                     *");
                Console.WriteLine("* Press Enter a valid Entry Number                                              *");
                Console.WriteLine("* Entry Number | Explaination                                                   *");
                Console.WriteLine("*      1       | Yes I would like to enter Districts to skip over               *");
                Console.WriteLine("*      2       | No, Run all Districts for Year-Month                           *");
                Console.WriteLine("*********************************************************************************");
                userSkipOption = Console.ReadLine();
            }

            if (userSkipOption == "1")
            {
                string userAddAnother = "1";

                while (userAddAnother == "1")
                {
                    string userDistrict = string.Empty;

                    Console.Clear();
                    Console.WriteLine("*********************************************************************************");
                    Console.WriteLine("* Please Enter the Districts to skip within the Year-Month                      *");
                    Console.WriteLine("* Districts: 1, 2, 3, 4, 5, 6, 6E, 7B, 7C, 8, 8A, 9, 10                         *");
                    Console.WriteLine("*********************************************************************************");
                    userDistrict = Console.ReadLine();

                    while (!ValidDistrict(userDistrict))
                    {
                        Console.Clear();
                        Console.WriteLine("*********************************************************************************");
                        Console.WriteLine("* User District Input Error: Please Enter a valid District Name                 *");
                        Console.WriteLine("* Districts: 1, 2, 3, 4, 5, 6, 6E, 7B, 7C, 8, 8A, 9, 10                         *");
                        Console.WriteLine("*********************************************************************************");
                        userDistrict = Console.ReadLine();
                    }

                    districtsToSkip.Add(userDistrict);

                    Console.Clear();
                    Console.WriteLine("*********************************************************************************");
                    Console.WriteLine("* Would you like to add another District to skip, Please Choose:                *");
                    Console.WriteLine("* Entry Number | Option                                                         *");
                    Console.WriteLine("*      1       | Yes                                                            *");
                    Console.WriteLine("*      2       | No                                                             *");
                    Console.WriteLine("*********************************************************************************");
                    userAddAnother = Console.ReadLine();

                    while (userSkipOption != "1" && userSkipOption != "2")
                    {
                        Console.Clear();
                        Console.WriteLine("*********************************************************************************");
                        Console.WriteLine("* User Input Error: Please Enter a valid File Name                              *");
                        Console.WriteLine("* Press Enter a valid Entry Number                                              *");
                        Console.WriteLine("* Entry Number | Explaination                                                   *");
                        Console.WriteLine("*      1       | Yes I would like to enter Districts to skip over               *");
                        Console.WriteLine("*      2       | No, Run all Districts for Year-Month                           *");
                        Console.WriteLine("*********************************************************************************");
                        userAddAnother = Console.ReadLine();
                    }
                }

            }

            return districtsToSkip;
        }

        public static List<RunDistrict> GetDistricts()
        {
            List<string> validDistricts = new List<string>
                { "1", "2", "3", "4", "5", "6", "6E", "7B", "7C", "8", "8A", "9", "10"};
            List<RunDistrict> districtsSetToRun = new List<RunDistrict>();

            foreach (var dist in validDistricts)
            {
                RunDistrict runDistrict = new RunDistrict { District = dist, Run = "Y" };

                districtsSetToRun.Add(runDistrict);
            }

            return districtsSetToRun;
        }
        #endregion

        #region ProcessFiles
.        public static bool OpenProrationsFile(string readFileName)
        {
            bool fileOpen;

            try {
                StreamReader read = new StreamReader(readFileName);
                read.ReadLine();
                read.Close();
                fileOpen = true;
            } catch (Exception) {
                fileOpen = false;
            }

            return fileOpen;
        }


        public static bool ValidDistrict(string userDistrict)
        {
            bool isValidDistrct = false;
            List<string> validDistricts = new List<string>
                { "1", "2", "3", "4", "5", "6", "6E", "7B", "7C", "8", "8A", "9", "10"};

            foreach (var district in validDistricts)
            {
                if (district == userDistrict.ToUpper())
                    isValidDistrct = true;
            }

            return isValidDistrct;
        }
                
        public static void ProcessSingleFile(string readFN, string yearMonth, string district)
        {
            string operatorFN = string.Empty,
                   leaseFN = string.Empty,
                   wellFN = string.Empty;


            StreamReader rawData = new StreamReader(readFN);

            operatorFN = "OP_" + district + "_" + yearMonth + ".txt";
            StreamWriter operators = new StreamWriter(operatorFN);

            leaseFN = "L_" + district + "_" + yearMonth + ".txt";
            StreamWriter leases = new StreamWriter(leaseFN);

            wellFN = "W_" + district + "_" + yearMonth + ".txt";
            StreamWriter wells = new StreamWriter(wellFN);

            CreateProrationFiles(yearMonth, district, rawData, operators, leases, wells);
        }

        public static void ProcessFilePath(string yearMonth, string filePath, List<string> districtsToSkip)
        {
            var validDistricts = GetDistricts();

            if (districtsToSkip.Count == 0)
            {
                foreach(var district in validDistricts)
                {
                    try
                    {
                        string operatorFileName = string.Empty,
                               leaseFileName = string.Empty,
                               wellFileName = string.Empty,
                               prorationRawData = string.Empty,
                               currentOperatorNumber = string.Empty,
                               currentLeaseNumber = string.Empty,
                               fileName = string.Empty;

                        fileName = filePath + "\\oilsch" + district.District + ".txt";
                        if(OpenProrationsFile(fileName))
                        {
                            StreamReader rawData = new StreamReader(fileName);

                            operatorFileName = filePath + "\\OP_" + district.District + "_" + yearMonth + ".txt";
                            StreamWriter operators = new StreamWriter(operatorFileName);

                            leaseFileName = filePath + "\\L_" + district.District + "_" + yearMonth + ".txt";
                            StreamWriter leases = new StreamWriter(leaseFileName);

                            wellFileName = filePath + "\\W_" + district.District + "_" + yearMonth + ".txt";
                            StreamWriter wells = new StreamWriter(wellFileName);

                            CreateProrationFiles(yearMonth, district.District, rawData, operators, leases, wells);
                        }                        
                    }
                    catch (Exception ex)
                    {
                        // Do Nothing for now
                        // Build log file late
                    }
                }
            }
            else
            {
                foreach(var distToSkip in districtsToSkip)
                {
                    foreach(var dist in validDistricts)
                    {
                        if(distToSkip == dist.District)
                        {
                            dist.Run = "N";
                        }
                    }
                }

                foreach(var district in validDistricts)
                {
                    if (district.Run == "Y")
                    {
                        string operatorFileName = string.Empty,
                               leaseFileName = string.Empty,
                               wellFileName = string.Empty,
                               prorationRawData = string.Empty,
                               currentOperatorNumber = string.Empty,
                               currentLeaseNumber = string.Empty,
                               fileName = string.Empty;

                        fileName = filePath + "\\oilsch" + district.District + ".txt";
                        StreamReader rawData = new StreamReader(fileName);

                        operatorFileName = filePath + "\\OP_" + district.District + "_" + yearMonth + ".txt";
                        StreamWriter operators = new StreamWriter(operatorFileName);

                        leaseFileName = filePath + "\\L_" + district.District + "_" + yearMonth + ".txt";
                        StreamWriter leases = new StreamWriter(leaseFileName);

                        wellFileName = filePath + "\\W_" + district.District + "_" + yearMonth + ".txt";
                        StreamWriter wells = new StreamWriter(wellFileName);

                        Regex operatorRgx = new Regex(@"^\d{6}$");
                        Regex leaseRgx = new Regex(@"^\d{5}$");
                        Regex apiRgx = new Regex(@"^\d{3}\-\d{5}$");

                        CreateProrationFiles(yearMonth, district.District, rawData, operators, leases, wells);
                    }
                }
            }
        }

        public static void CreateProrationFiles(string yearMonth, string district, StreamReader rawDataFile, 
            StreamWriter operators, StreamWriter leases, StreamWriter wells)
        {
            string currentOperatorNumber = string.Empty,
            currentLeaseNumber = string.Empty;

            Regex operatorRgx = new Regex(@"^\d{6}$");
            Regex leaseRgx = new Regex(@"^\d{5}$");
            Regex apiRgx = new Regex(@"^\d{3}\-\d{5}$");

            var prorationRawData = rawDataFile.ReadLine();

            while (prorationRawData != null)
            {
                try
                {
                    if (operatorRgx.IsMatch(prorationRawData.Substring(33, 6)))
                    {
                        if (prorationRawData.Substring(40, 3) == "P-5" || prorationRawData.Substring(40, 3) == "P5 ")
                        {
                            if ((!prorationRawData.Contains("(CONT)")) && (!prorationRawData.Contains("(CONTINUED)")))
                            {
                                var operatorName = prorationRawData.Substring(0, 33).Replace(":", "");
                                var operatorNumber = prorationRawData.Substring(33, 6);
                                var status = prorationRawData.Substring(40, 10);

                                currentOperatorNumber = operatorNumber;
                                var output = operatorName + "|" + operatorNumber + "|" + status + "|" + yearMonth + "|" + district + ":";

                                operators.WriteLine(output);
                            }
                        }
                    }
                    else if (leaseRgx.IsMatch(prorationRawData.Substring(1, 5)))
                    {
                        if ((!prorationRawData.Contains("(CONT)")) && (!prorationRawData.Contains("(CONTINUED)")))
                        {
                            var leaseNumber = prorationRawData.Substring(1, 5);
                            var leaseName = prorationRawData.Substring(8, 37).Replace(":", "");
                            var liqGath = prorationRawData.Substring(45, 5);
                            var gasGath = prorationRawData.Substring(52, 5);
                            var un1 = prorationRawData.Substring(59, 8);
                            var un2 = prorationRawData.Substring(68, 4);
                            var un3 = prorationRawData.Substring(74, 8);
                            var un4 = prorationRawData.Substring(82, 7);
                            var un5 = prorationRawData.Substring(90, 7);
                            var un6 = prorationRawData.Substring(98, 9);
                            var un7 = prorationRawData.Substring(108, 7);
                            var un8 = prorationRawData.Substring(116, 7);

                            currentLeaseNumber = leaseNumber;
                            var output = (leaseNumber + "|" + leaseName + "|" + liqGath + "|" + gasGath + "|" + un1 + "|" + un2
                                + "|" + un3 + "|" + un4 + "|" + un5 + "|" + un6 + "|" + un7 + "|" + un8 + "|" + currentOperatorNumber
                                + "|" + yearMonth + "|" + district + ":");

                            leases.WriteLine(output);
                        }
                    }
                    else if(apiRgx.IsMatch(prorationRawData.Substring(123, 9)))
                    {
                        if (district != "6E")
                        {
                            var wellNum = string.Empty;
                            if (prorationRawData.Substring(0, 1) == "*")
                                wellNum = prorationRawData.Substring(1, 6);
                            else
                                wellNum = prorationRawData.Substring(0, 7);

                            var depth = prorationRawData.Substring(8, 5);
                            var prodMethod = prorationRawData.Substring(14, 1);
                            var allBar = prorationRawData.Substring(15, 5);
                            var acres = prorationRawData.Substring(22, 5);
                            var gor = prorationRawData.Substring(28, 5);
                            var un2 = prorationRawData.Substring(35, 20);
                            var oil = prorationRawData.Substring(58, 8);
                            var gas = prorationRawData.Substring(65, 7);
                            if (gas.Substring(0, 1) == "T" || gas.Substring(0, 1) == "Y" || gas.Substring(0, 1) == "M" || gas.Substring(0, 1) == "0" ||
                                gas.Substring(0, 1) == "L" || gas.Substring(0, 1) == "P")
                            {
                                var firstLetter = gas.Substring(0, 1);
                                gas = gas.Replace(firstLetter, " ");
                            }
                            var remarks = prorationRawData.Substring(74, 15).Replace(":", " ");
                            var un5 = prorationRawData.Substring(90, 5);
                            var prodMethod2 = prorationRawData.Substring(96, 1);
                            var un6 = prorationRawData.Substring(98, 1);
                            var un7 = prorationRawData.Substring(100, 1);
                            var un8 = prorationRawData.Substring(102, 4);
                            var testDate = prorationRawData.Substring(107, 6);
                            var waterBbls = prorationRawData.Substring(115, 3);
                            var cnty = prorationRawData.Substring(119, 3);
                            var API = prorationRawData.Substring(123, 9);

                            var output = (wellNum + "|" + depth + "|" + prodMethod + "|" + allBar + "|" + acres + "|" + gor + "|" + un2
                                + "|" + oil + "|" + gas + "|" + remarks + "|" + un5 + "|" + prodMethod2 + "|" + un6 + "|" + un7
                                + "|" + un8 + "|" + testDate + "|" + waterBbls + "|" + cnty + "|" + API + "|" + currentOperatorNumber + "|"
                                + currentLeaseNumber + "|" + yearMonth + "|" + district + ":");

                            wells.WriteLine(output);
                        }
                        else
                        {
                            var wellNum = string.Empty;
                            if (prorationRawData.Substring(0, 1) == "*")
                                wellNum = prorationRawData.Substring(1, 6);
                            else
                                wellNum = prorationRawData.Substring(0, 7);

                            var depth = "3650";
                            var prodMethod = prorationRawData.Substring(8, 1);
                            var allBar = "0.0";
                            var acres = "5";
                            var gor = prorationRawData.Substring(31, 5);
                            var un2 = "";
                            var oil = prorationRawData.Substring(51, 6);
                            var gas = prorationRawData.Substring(70, 4);
                            var remarks = prorationRawData.Substring(74, 15).Replace(":", " ");
                            var un5 = prorationRawData.Substring(90, 3);
                            var prodMethod2 = prorationRawData.Substring(96, 1);
                            var un6 = prorationRawData.Substring(98, 1);
                            var un7 = prorationRawData.Substring(100, 1);
                            var un8 = prorationRawData.Substring(102, 4);
                            var testDate = prorationRawData.Substring(107, 6);
                            var waterBbls = prorationRawData.Substring(115, 3);
                            var cnty = prorationRawData.Substring(119, 3);
                            var API = prorationRawData.Substring(123, 9);

                            var output = (wellNum + "|" + depth + "|" + prodMethod + "|" + allBar + "|" + acres + "|" + gor + "|" + un2
                                + "|" + oil + "|" + gas + "|" + remarks + "|" + un5 + "|" + prodMethod2 + "|" + un6 + "|" + un7
                                + "|" + un8 + "|" + testDate + "|" + waterBbls + "|" + cnty + "|" + API + "|" + currentOperatorNumber + "|"
                                + currentLeaseNumber + "|" + yearMonth + "|" + district + ":");

                            wells.WriteLine(output);
                        }
                        
                    }
                    else
                    {
                        // Do Nothing with the Row
                    }
                }
                catch (Exception ex)
                {
                    // Do Nothing Currently
                    // BUild log file later? 
                }

                prorationRawData = rawDataFile.ReadLine();
            }

            // Close the Files
            operators.Close();
            leases.Close();
            wells.Close();
        }
        #endregion
    }
 }