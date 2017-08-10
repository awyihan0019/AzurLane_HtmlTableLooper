using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {

            int listNum, NumLines;
            listNum = getNameList();
            Console.WriteLine("Number of data Detected " + listNum);
            for (int i = 0; i < listNum; i++)
            {
                String nameGet = getHtmlTableDat(listNum, i);
                NumLines = textFilter();
                generateSQL(NumLines, getNo(listNum), i);
                saveImageFull(listNum, i, getNo(listNum));
                Console.WriteLine(String.Format("{0,-15} | {1,-15} | {2,-15}", nameGet, "资料已读取", listNum + "/" + (i + 1)));
            }
            saveImage(getNo(listNum));
            Console.WriteLine("\n\n资料已读取完成!!");
            Console.ReadKey();
        }

        public static int getNameList()
        {
            int count = 0;
            int numbersRead = 1;

            //set the file to read and write
            String file_demo = @"C:\Users\yihan\Desktop\demo.txt";
            String file_demoFixed = @"C:\Users\yihan\Desktop\demoFixed.txt";

            //set writer
            StreamWriter writer = new StreamWriter(file_demo);

            //skip firts row
            bool firstRowIsSkipped = false;


            //declare webside
            var html = @"http://wiki.joyme.com/blhx/%E6%89%93%E6%8D%9E%E5%88%97%E8%A1%A8";

            //create web
            HtmlWeb web = new HtmlWeb();

            //load the web
            var htmldoc = web.Load(html);

            //getting the node
            HtmlNodeCollection node = htmldoc.DocumentNode.SelectNodes("//table[@id='CardSelectTr']");


            foreach (HtmlNode table in node)
            {

                foreach (HtmlNode row in table.SelectNodes("tr"))
                {
                    count = table.SelectNodes("tr").Count;
                    count--;
                    if (firstRowIsSkipped == false)
                    {  //skip one row for the table header
                        firstRowIsSkipped = true;
                        continue;
                    }
                    HtmlNodeCollection cells = row.SelectNodes("th|td");

                    if (cells == null)
                    {
                        continue;
                    }
                    for (var k = 0; k < cells.Count; k++)
                    {
                        String data = cells[k].InnerText;
                        if (k == 1)
                        {
                            //writeForFile(data); :for testing output purpose
                            writer.WriteLine(data);
                            //Console.WriteLine(data); :for testing output purpose
                            numbersRead++;
                        }
                    }
                }
                //closs the write file
                writer.Close();

            }
            //reformat file
            reFormatfile(file_demo, file_demoFixed);
            return count;
        }

        public static String getHtmlTableDat(int listNum, int loopTime)
        {
            int count = 0;
            String[] nameList = new String[listNum];
            //set the file to read and write
            String file_demodata = @"C:\Users\yihan\Desktop\demoData.txt";
            String file_demodatafixed = @"C:\Users\yihan\Desktop\demoDataFixed.txt";
            String file_demoFixed = @"C:\Users\yihan\Desktop\demoFixed.txt";
            //

            using (StreamReader sr = new StreamReader(file_demoFixed))
            {
                while (sr.Peek() >= 0)
                {
                    var strReadLine = sr.ReadLine();
                    nameList[count] = strReadLine;
                    count++;
                }
            }

            //
            //set writer
            StreamWriter writer = new StreamWriter(file_demodata);

            String name = nameList[loopTime];
            //set website
            var html2 = @"http://wiki.joyme.com/blhx/" + name;

            HtmlWeb web2 = new HtmlWeb();

            var htmldoc2 = web2.Load(html2);

            foreach (HtmlNode nodeTF in htmldoc2.DocumentNode.SelectNodes("//table[@class='wikitable']"))
            {
                foreach (HtmlNode row in nodeTF.SelectNodes("tr"))
                {
                    HtmlNodeCollection cells = row.SelectNodes("th|td");

                    foreach (HtmlNode cell in cells)
                    {
                        if (cell != null && cell.InnerText != "\n")
                        {
                            if (cell.InnerText.IndexOf("canvas") == -1)
                            {
                                writer.WriteLine(cell.InnerText);
                                //Console.WriteLine(cell.InnerText); :for testing output purpose
                            }
                        }
                        else
                        {
                            writer.WriteLine("-");
                            //Console.WriteLine("-"); :for testing output purpose
                        }
                    }
                }

            }
            //closs the write file
            writer.Close();
            //reformat file
            reFormatfile(file_demodata, file_demodatafixed);
            return name;
        }

        public static void reFormatfile(String file_to_read, String file_to_write)
        {
            using (StreamWriter sw = new StreamWriter(file_to_write, false))
            {
                using (StreamReader sr = new StreamReader(file_to_read))
                {
                    while (sr.Peek() >= 0)
                    {
                        var strReadLine = sr.ReadLine().Trim();
                        if (!String.IsNullOrWhiteSpace(strReadLine))
                        {
                            sw.WriteLine(strReadLine);
                        }
                    }
                }
            }
        }

        public static int textFilter()
        {
            using (StreamWriter sw2 = new StreamWriter(@"C:\Users\yihan\Desktop\demoDataFixed2.txt", false))
            {
                using (StreamReader sr2 = new StreamReader(@"C:\Users\yihan\Desktop\demoDataFixed.txt"))
                {
                    int count = 0;
                    while (sr2.Peek() >= 0)
                    {
                        String strReadLine = sr2.ReadLine();
                        bool check = skippingWord(strReadLine);
                        if (strReadLine == "前排三破弹幕或后排专属弹幕")
                        {
                            return count;
                        }
                        if (check == true)
                        {
                            sw2.WriteLine(strReadLine);
                            count++;
                        }
                    }
                    return count;
                }


            }
        }

        public static bool skippingWord(String word)
        {
            if (word != "编号" && word != "初始星级" && word != "类型" && word != "稀有度" && word != "阵营" && word != "耗时" && word != "掉落点" && word != "营养价值" && word != "退役收益" && word != "主分类" && word != "次分类" && word != "性能" && word != "canvas" && word != "炮击" && word != "耐久" && word != "防空" && word != "机动" && word != "航空" && word != "雷击" && word != "初始属性/满级满破满强化好感度爱属性(婚初始属性除以1.06再乘以1.09)" && word != "耐久" && word != "装甲" && word != "装填" && word != "消耗" && word != "航速" && word != "突破升星效果" && word != "防空" && word != "航空" && word != "突破升星效果" && word != "一阶" && word != "二阶" && word != "三阶" && word != "槽位/武器效率/初始装备/可装备" && word != "槽位" && word != "效率（初始/满破）" && word != "初始装备" && word != "可装备" && word != "技能" && word != "装备说明")
            {
                return true;
            }
            else
                return false;
        }

        public static void generateSQL(int numLine, String[] no, int dataReacher)
        {
            using (StreamWriter sw2 = new StreamWriter(@"C:\Users\yihan\Desktop\database.txt", true))
            {
                using (StreamReader sr2 = new StreamReader(@"C:\Users\yihan\Desktop\demoDataFixed2.txt"))
                {
                    int line = 0;
                    string[] dataList = new string[numLine];
                    String space;
                    //Console.WriteLine(numLine.ToString());
                    for (int i = 0; i < numLine; i++)
                    {
                        if (i != 22 && i != 23 && i != 24 && i != 25 && i != 35 && i != 39 && i != 43 && i != 47 && i != 51 && i != 56)
                        {
                            dataList[line] = sr2.ReadLine();
                            line++;
                        }
                        else
                            space = sr2.ReadLine();
                    }
                    sw2.WriteLine("INSERT INTO KANTAI (name, No, lvl, type, rare, camp, buildTime, dropPoint, value, returnValue, main, sub, hp, amor, filling, atk, tAtk, agi, airDef, airAtk, compsum, speed, lvlAtk, lvlHp, lvlAirDef, lvlAgi, lvlAirAtk, lvlTAtk, star1, star2, star3, usage1, startEquip1, equipType1, usage2, startEquip2, equipType2, usage3, startEquip3, equipType3, usage4, startEquip4, equipType4, usage5, startEquip5, equipType5, skill1, skillEffect1, skill2, skillEffect2, skill3, skillEffect3) VALUES (\""
                        + dataList[0] + "\",\"" + dataList[1] + "\",\"" + dataList[2] + "\",\"" + dataList[3] + "\",\"" + dataList[4] + "\",\"" + dataList[5] + "\",\"" + dataList[6] + "\",\"" + dataList[7] + "\",\"" + dataList[8] + "\",\"" + dataList[9] + "\",\""
                        + dataList[10] + "\",\"" + dataList[11] + "\",\"" + dataList[12] + "\",\"" + dataList[13] + "\",\"" + dataList[14] + "\",\"" + dataList[15] + "\",\"" + dataList[16] + "\",\"" + dataList[17] + "\",\"" + dataList[18] + "\",\"" + dataList[19] + "\",\""
                        + dataList[20] + "\",\"" + dataList[21] + "\",\"" + dataList[22] + "\",\"" + dataList[23] + "\",\"" + dataList[24] + "\",\"" + dataList[25] + "\",\"" + dataList[26] + "\",\"" + dataList[27] + "\",\"" + dataList[28] + "\",\"" + dataList[29] + "\",\""
                        + dataList[30] + "\",\"" + dataList[31] + "\",\"" + dataList[32] + "\",\"" + dataList[33] + "\",\"" + dataList[34] + "\",\"" + dataList[35] + "\",\"" + dataList[36] + "\",\"" + dataList[37] + "\",\"" + dataList[38] + "\",\"" + dataList[39] + "\",\""
                        + dataList[40] + "\",\"" + dataList[41] + "\",\"" + dataList[42] + "\",\"" + dataList[43] + "\",\"" + dataList[44] + "\",\"" + dataList[45] + "\",\"" + dataList[46] + "\",\"" + dataList[47] + "\",\"" + dataList[48] + "\",\"" + dataList[49] + "\",\""
                        + dataList[50] + "\",\"" + dataList[51] + "\");");
                }
            }
        }

        public static void saveImage(String[] no)
        {
            //StreamWriter sw2 = new StreamWriter(@"C:\Users\yihan\Desktop\imgURL.txt", true);

            String url;
            String foulder = @"C:\Users\yihan\Desktop\ImageDemo\";

            //declare webside
            var html = @"http://wiki.joyme.com/blhx/%E6%89%93%E6%8D%9E%E5%88%97%E8%A1%A8";

            var htmlDoc2 = new HtmlDocument();

            String html2;

            //create web
            HtmlWeb web = new HtmlWeb();

            //load the web
            var htmldoc = web.Load(html);

            var htmlNodes = htmldoc.DocumentNode.SelectSingleNode("//table[@id='CardSelectTr']");
            //var htmlNodes = htmlDoc.DocumentNode.SelectNodes("//body/h1");
            //var html = node.OuterHtml();
            html2 = htmlNodes.InnerHtml;

            htmlDoc2.LoadHtml(html2);
            HtmlNodeCollection imageNode = htmlDoc2.DocumentNode.SelectNodes("//img");
            for (int i = 0; i < imageNode.Count; i++)
            {
                url = imageNode[i].GetAttributeValue("src", null);
                //Console.WriteLine(url);
                var client = new WebClient();
                client.DownloadFile(url, foulder + no[i] + ".jpg");
                Console.WriteLine("Download successfull : " + (i + 1));
            }
        }

        public static void saveImageFull(int listNum, int loopTime, String[] no)
        {
            int count = 0;
            String url;
            String foulder = @"C:\Users\yihan\Desktop\ImageFull\";

            var htmlDoc2 = new HtmlDocument();

            String html2;
            String[] nameList = new String[listNum];

            //set the file to read and write
            String file_demoFixed = @"C:\Users\yihan\Desktop\demoFixed.txt";

            //

            using (StreamReader sr = new StreamReader(file_demoFixed))
            {
                while (sr.Peek() >= 0)
                {
                    var strReadLine = sr.ReadLine();
                    nameList[count] = strReadLine;
                    count++;
                }
            }

            //

            String name = nameList[loopTime];
            //set website
            var html = @"http://wiki.joyme.com/blhx/" + name;

            HtmlWeb web2 = new HtmlWeb();

            var htmldoc = web2.Load(html);

            //Line----------------------------------------------------------------Save image below-------------------------------------

            var htmlNodes = htmldoc.DocumentNode.SelectSingleNode("//div[@id='con_1']");
            //var htmlNodes = htmlDoc.DocumentNode.SelectNodes("//body/h1");
            //var html = node.OuterHtml();
            html2 = htmlNodes.InnerHtml;

            htmlDoc2.LoadHtml(html2);
            HtmlNodeCollection imageNode = htmlDoc2.DocumentNode.SelectNodes("//img");
            for (int i = 0; i < imageNode.Count; i++)
            {
                url = imageNode[i].GetAttributeValue("src", null);
                //Console.WriteLine(url);
                var client = new WebClient();
                client.DownloadFile(url, foulder + no[loopTime] + "_full" + ".jpg");
                Console.WriteLine("Image download successfull!");
            }
        }

        public static String[] getNo(int names)
        {
            int count = 0;
            int listCount = 0;
            var client = new WebClient();
            String[] name = new string[names];

            //declare webside
            var html = @"http://wiki.joyme.com/blhx/%E6%89%93%E6%8D%9E%E5%88%97%E8%A1%A8";

            //create web
            HtmlWeb web = new HtmlWeb();

            //load the web
            var htmldoc = web.Load(html);

            //skip firts row
            bool firstRowIsSkipped = false;

            //getting the node
            HtmlNodeCollection node = htmldoc.DocumentNode.SelectNodes("//table[@id='CardSelectTr']");


            foreach (HtmlNode table in node)
            {

                foreach (HtmlNode row in table.SelectNodes("tr"))
                {
                    count = table.SelectNodes("tr").Count;
                    count--;
                    if (firstRowIsSkipped == false)
                    {  //skip one row for the table header
                        firstRowIsSkipped = true;
                        continue;
                    }
                    HtmlNodeCollection cells = row.SelectNodes("th|td");

                    if (cells == null)
                    {
                        continue;
                    }
                    for (var k = 0; k < cells.Count; k++)
                    {
                        String data = cells[k].InnerText;
                        if (k == 0)
                        {
                            name[listCount] = "NO."+data.Trim();
                            //Console.WriteLine(data);
                            listCount++;
                        }
                    }
                }
            }
            return name;
        }
    }
}
