﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            int listNum;
            listNum = getNameList();

            getHtmlTableDat(listNum);

            textFilter();

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
                            //writeForFile(data);
                            writer.WriteLine(data);
                            //Console.WriteLine(data);
                            Console.WriteLine("已读取数量 ：" + numbersRead);
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

        public static void getHtmlTableDat(int listNum)
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
            for (int i = 0; i < listNum; i++)
            {
                //set website
                var html2 = @"http://wiki.joyme.com/blhx/" + nameList[i];

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
                                    //Console.WriteLine(cell.InnerText);
                                }
                            }
                            else
                            {
                                writer.WriteLine("-");
                                //Console.WriteLine("-");
                            }
                        }
                    }

                }
                Console.WriteLine(String.Format("{0,-15} | {1,-15} | {2,-15}", nameList[i], "资料已读取", listNum + "/" + (i + 1)));

            }
            //closs the write file
            writer.Close();
            //reformat file
            reFormatfile(file_demodata, file_demodatafixed);
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

        public static void textFilter()
        {
            using (StreamWriter sw2 = new StreamWriter(@"C:\Users\yihan\Desktop\demoDataFixed2.txt", false))
            {
                using (StreamReader sr2 = new StreamReader(@"C:\Users\yihan\Desktop\demoDataFixed.txt"))
                {
                    while (sr2.Peek() >= 0)
                    {
                        String strReadLine = sr2.ReadLine();
                        bool check = skippingWord(strReadLine);
                        if (strReadLine == "前排三破弹幕或后排专属弹幕")
                        {
                            break;
                        }
                        if (check == true)
                        {
                            sw2.WriteLine(strReadLine);
                        }
                    }
                }


            }
        }

        public static bool skippingWord(String word)
        {
            if (word != "编号" && word != "初始星级" && word != "类型" && word != "稀有度" && word != "阵营" && word != "耗时" && word != "掉落点" && word != "营养价值" && word != "退役收益" && word != "主分类" && word != "次分类" && word != "性能" && word != "canvas" && word != "炮击" && word != "耐久" && word != "防空" && word != "机动" && word != "航空" && word != "雷击" && word != "初始属性/满级满破满强化好感度爱属性(婚初始属性除以1.06再乘以1.09)" && word != "耐久" && word != "装甲" && word != "装填" && word != "消耗" && word != "航速" && word != "突破升星效果" && word != "防空" && word != "航空" && word != "突破升星效果" && word != "一阶" && word != "二阶" && word != "三阶" && word != "槽位/武器效率/初始装备/可装备" && word != "槽位" && word != "效率（初始/满破）" && word != "初始装备" && word != "可装备")
            {
                return true;
            }
            else
                return false;
        }
    }
}