using MyReaderAPI;
using MyReaderAPI.MyInterface;
using MyReaderAPI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;

//Version : 2.18.0 (C#)
namespace SampleCode
{
    class Program : IAsynchronousMessage
    {
        static String ConnID = "";

        static void Main(String[] args)
        {

            String readKey;
            Program example = new Program();

            #region Connect

            while (true)
            {
                Console.WriteLine("Please select connect type: \n 1.RS232\n 2.TCP\n 3.RS485\n 4.USB\n 5.Open TCP Server\n 6.Close TCP Server\n 7.Get Server Startup\n q.Quit\n");
               // readKey = Console.ReadLine();
                readKey = "4";
                if (readKey.Equals("1"))
                {
                    Console.WriteLine("Please input RS232 ConnID,Format: 'COM Number':'Baud Rate' like \"COM1:115200\" \n");
                    ConnID = Console.ReadLine();
                    if (My_Reader.CreateSerialConn(ConnID, example))
                    {
                        Console.WriteLine("Connect success!\n");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Connect failure!\n");
                        continue;
                    }
                }
                else if (readKey.Equals("2"))
                {
                    Console.WriteLine("Please input TCP ConnID,Format: 'IP Address':'Connect Port' like \"192.168.1.116:9090\" \n");
                    ConnID = Console.ReadLine();
                    if (My_Reader.CreateTcpConn(ConnID, example))
                    {
                        Console.WriteLine("Connect success!\n");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Connect failure!\n");
                        continue;
                    }
                }
                else if (readKey.Equals("3"))
                {
                    Console.WriteLine("Please input RS485 ConnID,Format: '485 Address':'COM Number':'Baud Rate' like \"1:COM1:115200\" \n");
                    ConnID = Console.ReadLine();
                    if (My_Reader.Create485Conn(ConnID, example))
                    {
                        Console.WriteLine("Connect success!\n");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Connect failure!\n");
                        continue;
                    }
                }
                else if (readKey.Equals("4"))
                {
                    Dictionary<string, string> dic_UsbDevicePath_Name = new Dictionary<string, string>();
                    List<string> listUsbDevicePath = My_Reader.GetUsbHidDeviceList();
                    Console.WriteLine("Please select USB connect parameter");
                    for (int i = 0; i < listUsbDevicePath.Count; i++)
                    {
                        string path = listUsbDevicePath[i];
                        dic_UsbDevicePath_Name.Add(path, "UHF READER " + (i + 1));
                        string name = dic_UsbDevicePath_Name[path].ToString();
                        Console.WriteLine(Convert.ToString(i + 1) + "." + name);
                    }

                    try
                    {
                        string num = Console.ReadLine();

                        if (Convert.ToInt32(num) - 1 < listUsbDevicePath.Count)
                        {
                            int deviceIndex = Convert.ToInt32(num) - 1;
                            IntPtr Handle = User32API.GetCurrentWindowHandle();

                            if (My_Reader.CreateUsbConn(listUsbDevicePath[deviceIndex], Handle, example))
                            {
                                ConnID = listUsbDevicePath[deviceIndex];
                                if (My_Reader.CheckConnect(ConnID))
                                {
                                    Console.WriteLine("Connect success!\n");
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Connect failure!\n");
                                    continue;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Connect failure!\n");
                                continue;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Connect parameter select error!\n");
                            continue;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else if (readKey.Equals("5"))
                {
                    Console.WriteLine("Please input ServerIP like \"192.168.1.1\"");
                    string ServerIP = Console.ReadLine();
                    Console.WriteLine("Please input ServerPort like \"9090\"");
                    string ServerPort = Console.ReadLine();
                    if (My_Reader.OpenTcpServer(ServerIP, ServerPort, example))
                    {
                        Console.WriteLine("Open Tcp Server Success!");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Open Tcp Server Failure!");
                    }
                }
                else if (readKey.Equals("6"))
                {
                    My_Reader.CloseTcpServer();
                    Console.WriteLine("OK");
                }
                else if (readKey.Equals("7"))
                {
                    if (My_Reader.GetServerStartUp())
                    {
                        Console.WriteLine("Yes");
                    }
                    else
                    {
                        Console.WriteLine("No");
                    }
                }
                else if (readKey.Equals("q"))
                {
                    My_Reader._Config.Stop(ConnID);
                    My_Reader.CloseConn(ConnID);
                    return;
                }
                else
                {
                    Console.WriteLine("Select Parameter Error!\n");
                }
            }

            #endregion

            try
            {
                while (ConnID.Equals(""))
                {
                    Thread.Sleep(3000);
                }


                while (true)
                {
                    Console.WriteLine("Current ConnID : " + ConnID + "\n");
                    Console.WriteLine("Please select operation:");
                    Console.WriteLine("1.Connect Setting");
                    Console.WriteLine("2.Reader Setting");
                    Console.WriteLine("3.RFID Setting");
                    Console.WriteLine("4.GPIO Setting");
                    Console.WriteLine("5.6C Tag");
                    Console.WriteLine("6.6B Tag");
                    Console.WriteLine("7.GB Tag");
                    Console.WriteLine("8.Break Point");
                    Console.WriteLine("q.Quit");

                    //readKey = Console.ReadLine();
                    readKey = "5";
                    #region Connect Setting

                    if (readKey.Equals("1"))
                    {
                        while (true)
                        {
                            Console.WriteLine("Current ConnID : " + ConnID + "\n");
                            Console.WriteLine("Please select operation:");
                            Console.WriteLine("1.Close Single Connect");
                            Console.WriteLine("2.Close All Connect");
                            Console.WriteLine("3.Set API Language Type");
                            Console.WriteLine("4.Get API Language Type");
                            Console.WriteLine("b.Back to Previous Menu");
                            Console.WriteLine("q.Quit");

                            readKey = Console.ReadLine();
                            if (readKey.Equals("1"))
                            {
                                Console.WriteLine("Please input ConnID");
                                readKey = Console.ReadLine();
                                My_Reader.CloseConn(readKey);
                                if (readKey.Equals(ConnID))
                                {
                                    return;
                                }
                            }
                            else if (readKey.Equals("2"))
                            {
                                My_Reader.CloseAllConnect();
                                return;
                            }
                            else if (readKey.Equals("3"))
                            {
                                Console.WriteLine("Please select operation:");
                                Console.WriteLine("1.eAPILanguage.Chinese");
                                Console.WriteLine("2.eAPILanguage.English");
                                readKey = Console.ReadLine();
                                if (readKey == "1")
                                {
                                    My_Reader.SetAPILanguageType(eAPILanguage.Chinese);
                                }
                                else
                                {
                                    My_Reader.SetAPILanguageType(eAPILanguage.English);
                                }
                                Console.WriteLine("OK");

                            }
                            else if (readKey.Equals("4"))
                            {
                                string Result = My_Reader.GetAPILanguageType();
                                Console.WriteLine(Result);
                            }
                            else if (readKey.Equals("b"))
                            {
                                break;
                            }
                            else if (readKey.Equals("q"))
                            {
                                My_Reader._Config.Stop(ConnID);
                                My_Reader.CloseConn(ConnID);
                                return;
                            }
                        }
                    }

                    #endregion

                    #region Reader Setting
                    else if (readKey.Equals("2"))
                    {
                        while (true)
                        {
                            Console.WriteLine("Current ConnID : " + ConnID);
                            Console.WriteLine("Please select operation:");
                            Console.WriteLine("1.Set IP Setting");
                            Console.WriteLine("2.Get IP Setting");
                            Console.WriteLine("3.Stop Read");
                            Console.WriteLine("4.Set Reader Time");
                            Console.WriteLine("5.Get Reader Time");
                            Console.WriteLine("6.Set Serial Port");
                            Console.WriteLine("7.Get Serial Port");
                            Console.WriteLine("8.Set MAC");
                            Console.WriteLine("9.Get MAC");
                            Console.WriteLine("10.Set RS485");
                            Console.WriteLine("11.Get RS485");
                            Console.WriteLine("12.Set Server/Client");
                            Console.WriteLine("13.Get Server/Client");
                            Console.WriteLine("14.Reader Information");
                            Console.WriteLine("15.Baseband Version");
                            Console.WriteLine("16.Get Antenna Standing Wave Ratio");
                            Console.WriteLine("17.Set DHCP");
                            Console.WriteLine("18.Get DHCP");
                            Console.WriteLine("b.Back to Previous Menu");
                            Console.WriteLine("q.Quit");

                            readKey = Console.ReadLine();
                            if (readKey.Equals("1"))
                            {
                                Console.WriteLine("Please Input IP like \"192.168.1.116\"");
                                String IP = Console.ReadLine();
                                Console.WriteLine("Please input MASK like \"255.255.255.0\"");
                                String Mask = Console.ReadLine();
                                Console.WriteLine("Please input Gateway like \"192.168.1.1\"");
                                String Gateway = Console.ReadLine();
                                if (My_Reader._Config.SetReaderNetworkPortParam(ConnID, IP, Mask, Gateway) == 0)
                                {
                                    Console.WriteLine("Success!");
                                    Console.WriteLine("It is must be restart console program!");
                                }
                                else
                                {
                                    Console.WriteLine("Failure!");
                                }
                            }
                            else if (readKey.Equals("2"))
                            {
                                String Result = My_Reader._Config.GetReaderNetworkPortParam(ConnID);
                                Console.WriteLine(Result);
                                Console.WriteLine("Tip:	'IP'|'Mask'|'Gateway'");
                            }
                            else if (readKey.Equals("3"))
                            {
                                if (My_Reader._Config.Stop(ConnID) == 0)
                                {
                                    Console.WriteLine("Success!");
                                }
                                else
                                {
                                    Console.WriteLine("Failure!");
                                }
                            }
                            else if (readKey.Equals("4"))
                            {
                                Console.WriteLine("Please input param: \n");
                                Console.WriteLine("Tip:	yyyy-MM-dd HH:mm:ss.SSS	\n");
                                Console.WriteLine("Example:	2017-04-05 12:34:56.789	 \n");
                                String param = Console.ReadLine();

                                if (My_Reader._Config.SetReaderUTC(ConnID, param) == 0)
                                {
                                    Console.WriteLine("Success!");
                                }
                                else
                                {
                                    Console.WriteLine("Failure!");
                                }


                            }
                            else if (readKey.Equals("5"))
                            {
                                String Result = My_Reader._Config.GetReaderUTC(ConnID);

                                Console.WriteLine(Result + "\n");
                            }
                            else if (readKey.Equals("6"))
                            {
                                Console.WriteLine("Please select operation:");
                                Console.WriteLine("1.eBaudrate._9600bps");
                                Console.WriteLine("2.eBaudrate._19200bps");
                                Console.WriteLine("3.eBaudrate._115200bps");
                                Console.WriteLine("4.eBaudrate._230400bps");
                                Console.WriteLine("5.eBaudrate._460800bps");
                                readKey = Console.ReadLine();
                                if (readKey.Equals("1"))
                                {
                                    if (My_Reader._Config.SetReaderSerialPortParam(ConnID, eBaudrate._9600bps) == 0)
                                    {
                                        Console.WriteLine("Success!");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Failure!");
                                    }
                                }
                                else if (readKey.Equals("2"))
                                {
                                    if (My_Reader._Config.SetReaderSerialPortParam(ConnID, eBaudrate._19200bps) == 0)
                                    {
                                        Console.WriteLine("Success!");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Failure!");
                                    }
                                }
                                else if (readKey.Equals("3"))
                                {
                                    if (My_Reader._Config.SetReaderSerialPortParam(ConnID, eBaudrate._115200bps) == 0)
                                    {
                                        Console.WriteLine("Success!");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Failure!");
                                    }
                                }
                                else if (readKey.Equals("4"))
                                {
                                    if (My_Reader._Config.SetReaderSerialPortParam(ConnID, eBaudrate._230400bps) == 0)
                                    {
                                        Console.WriteLine("Success!");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Failure!");
                                    }
                                }
                                else if (readKey.Equals("5"))
                                {
                                    if (My_Reader._Config.SetReaderSerialPortParam(ConnID, eBaudrate._460800bps) == 0)
                                    {
                                        Console.WriteLine("Success!");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Failure!");
                                    }
                                }
                            }
                            else if (readKey.Equals("7"))
                            {
                                Console.WriteLine(My_Reader._Config.GetReaderSerialPortParam2(ConnID));
                            }
                            else if (readKey.Equals("8"))
                            {
                                Console.WriteLine("Please input MAC:");
                                Console.WriteLine("Tip:	00-00-00-00-00-00");
                                String param = Console.ReadLine();
                                if (My_Reader._Config.SetReaderMacParam(ConnID, param) == 0)
                                {
                                    Console.WriteLine("Success!");
                                }
                                else
                                {
                                    Console.WriteLine("Failure!");
                                }
                            }
                            else if (readKey.Equals("9"))
                            {
                                String Result = My_Reader._Config.GetReaderMacParam(ConnID);
                                Console.WriteLine(Result);
                            }
                            else if (readKey.Equals("10"))
                            {
                                Console.WriteLine("Please input 485 address:");
                                Console.WriteLine("Tip:	'485 address'");
                                Console.WriteLine("Example:	1");
                                String param = Console.ReadLine();
                                if (My_Reader._Config.SetReader485(ConnID, param) == 0)
                                {
                                    Console.WriteLine("Success!");
                                }
                                else
                                {
                                    Console.WriteLine("Failure!");
                                }
                            }
                            else if (readKey.Equals("11"))
                            {
                                String Result = My_Reader._Config.GetReader485(ConnID);
                                Console.WriteLine(Result);
                                Console.WriteLine("Tip:	'485 address'");
                            }
                            else if (readKey.Equals("12"))
                            {
                                Console.WriteLine("Please select workMode:");
                                Console.WriteLine("1.eWorkMode.Server");
                                Console.WriteLine("2.eWorkMode.Client");
                                readKey = Console.ReadLine();
                                eWorkMode workMode;
                                if (readKey.Equals("1"))
                                {
                                    workMode = eWorkMode.Server;
                                }
                                else
                                {
                                    workMode = eWorkMode.Client;
                                }

                                Console.WriteLine("Please input IP:");
                                String IP = Console.ReadLine();
                                Console.WriteLine("Please input Port:");
                                String Port = Console.ReadLine();
                                if (My_Reader._Config.SetReaderServerOrClient(ConnID, workMode, IP, Port) == 0)
                                {
                                    Console.WriteLine("Success!");
                                }
                                else
                                {
                                    Console.WriteLine("Failure!");
                                }
                            }
                            else if (readKey.Equals("13"))
                            {
                                Console.WriteLine(My_Reader._Config.GetReaderServerOrClient(ConnID));
                                Console.WriteLine("Tips: Server|\"Server Port\"  or  Client|\"Client IP\"|\"Client Port\"");
                            }
                            else if (readKey.Equals("14"))
                            {
                                Console.WriteLine(My_Reader._Config.GetReaderInformation(ConnID));
                                Console.WriteLine("Format: 'Application Software Version'|'Reader Name'|'Reader Power On Time'");
                                Console.WriteLine("Tip: The unit of Reader Power On Time is second");
                            }
                            else if (readKey.Equals("15"))
                            {
                                String BasebandVersion = My_Reader._Config.GetReaderBaseBandSoftVersion(ConnID);
                                Console.WriteLine(BasebandVersion + "\n");
                            }
                            else if (readKey.Equals("16"))
                            {
                                eAntennaNo antennaNo = new eAntennaNo();
                                Console.WriteLine("Please input antenna number:");
                                readKey = Console.ReadLine();
                                if (readKey.Equals("1"))
                                {
                                    antennaNo = eAntennaNo._1;
                                }
                                else if (readKey.Equals("2"))
                                {
                                    antennaNo = eAntennaNo._2;
                                }
                                else if (readKey.Equals("3"))
                                {
                                    antennaNo = eAntennaNo._3;
                                }
                                else if (readKey.Equals("4"))
                                {
                                    antennaNo = eAntennaNo._4;
                                }
                                else if (readKey.Equals("5"))
                                {
                                    antennaNo = eAntennaNo._5;
                                }
                                else if (readKey.Equals("6"))
                                {
                                    antennaNo = eAntennaNo._6;
                                }
                                else if (readKey.Equals("7"))
                                {
                                    antennaNo = eAntennaNo._7;
                                }
                                else if (readKey.Equals("8"))
                                {
                                    antennaNo = eAntennaNo._8;
                                }
                                else if (readKey.Equals("9"))
                                {
                                    antennaNo = eAntennaNo._9;
                                }
                                else if (readKey.Equals("10"))
                                {
                                    antennaNo = eAntennaNo._10;
                                }
                                else if (readKey.Equals("11"))
                                {
                                    antennaNo = eAntennaNo._11;
                                }
                                else if (readKey.Equals("12"))
                                {
                                    antennaNo = eAntennaNo._12;
                                }
                                else if (readKey.Equals("13"))
                                {
                                    antennaNo = eAntennaNo._13;
                                }
                                else if (readKey.Equals("14"))
                                {
                                    antennaNo = eAntennaNo._14;
                                }
                                else if (readKey.Equals("15"))
                                {
                                    antennaNo = eAntennaNo._15;
                                }
                                else if (readKey.Equals("16"))
                                {
                                    antennaNo = eAntennaNo._16;
                                }
                                else if (readKey.Equals("17"))
                                {
                                    antennaNo = eAntennaNo._17;
                                }
                                else if (readKey.Equals("18"))
                                {
                                    antennaNo = eAntennaNo._18;
                                }
                                else if (readKey.Equals("19"))
                                {
                                    antennaNo = eAntennaNo._19;
                                }
                                else if (readKey.Equals("20"))
                                {
                                    antennaNo = eAntennaNo._20;
                                }
                                else if (readKey.Equals("21"))
                                {
                                    antennaNo = eAntennaNo._21;
                                }
                                else if (readKey.Equals("22"))
                                {
                                    antennaNo = eAntennaNo._22;
                                }
                                else if (readKey.Equals("23"))
                                {
                                    antennaNo = eAntennaNo._23;
                                }
                                else if (readKey.Equals("24"))
                                {
                                    antennaNo = eAntennaNo._24;
                                }
                                else
                                {
                                    Console.WriteLine("antenna number error");
                                    break;
                                }

                                String AntennaStandingWaveRatio = My_Reader._Config.GetAntennaStandingWaveRatio(ConnID, (int)antennaNo);
                                Console.WriteLine(AntennaStandingWaveRatio + "\n");
                            }
                            else if (readKey.Equals("17"))
                            {
                                Console.WriteLine("Please select operation:");
                                Console.WriteLine("1.Open");
                                Console.WriteLine("2.Close");
                                readKey = Console.ReadLine();
                                if (readKey.Equals("1"))
                                {
                                    Console.WriteLine(My_Reader._Config.SetDHCP(ConnID, true));
                                }
                                else if (readKey.Equals("2"))
                                {
                                    Console.WriteLine(My_Reader._Config.SetDHCP(ConnID, false));
                                }
                            }
                            else if (readKey.Equals("18"))
                            {
                                if (My_Reader._Config.GetDHCP(ConnID))
                                {
                                    Console.WriteLine("Open");
                                }
                                else
                                {
                                    Console.WriteLine("Close");
                                }
                            }
                            else if (readKey.Equals("b"))
                            {
                                break;
                            }
                            else if (readKey.Equals("q"))
                            {
                                My_Reader._Config.Stop(ConnID);
                                My_Reader.CloseConn(ConnID);
                                return;
                            }
                        }

                    }

                    #endregion  

                    #region RFID Setting
                    else if (readKey.Equals("3"))
                    {
                        while (true)
                        {
                            Console.WriteLine("Current ConnID : " + ConnID + "\n");
                            Console.WriteLine("Please select operation:");
                            Console.WriteLine("1.Set EPC Baseband");
                            Console.WriteLine("2.Get EPC Baseband");
                            Console.WriteLine("3.Set ANT Power");
                            Console.WriteLine("4.Get ANT Power");
                            Console.WriteLine("5.Set Tag Update");
                            Console.WriteLine("6.Get Tag Update");
                            Console.WriteLine("7.Get Reader Property");
                            Console.WriteLine("8.Set Reader RF");
                            Console.WriteLine("9.Get Reader RF");
                            Console.WriteLine("10.Set Reader AutoSleep");
                            Console.WriteLine("11.Get Reader AutoSleep");
                            Console.WriteLine("12.Set ANT Enable");
                            Console.WriteLine("13.Get ANT Enable");
                            Console.WriteLine("14.Set Reader RF Point");
                            Console.WriteLine("15.Get Reader RF Point");
                            Console.WriteLine("b.Back to Previous Menu");
                            Console.WriteLine("q.Quit");

                            readKey = Console.ReadLine();
                            if (readKey.Equals("1"))
                            {
                                Console.WriteLine("Please input basebandMode:");
                                Console.WriteLine("0:Tari=25us，FM0，LHF=40KHz");
                                Console.WriteLine("1:Tari=25us，Miller4，LHF=250KHz");
                                Console.WriteLine("2:Tari=25us，Miller4，LHF=300KH");
                                Console.WriteLine("3:Tari=6.25us，FM0，LHF=400KHz");
                                Console.WriteLine("255:Auto");
                                int basebandMode = Convert.ToInt32(Console.ReadLine());


                                Console.WriteLine("Please input qValue:");
                                Console.WriteLine("0~15");
                                int qValue = Convert.ToInt32(Console.ReadLine());

                                Console.WriteLine("Please input session:");
                                Console.WriteLine("0~3");
                                int session = Convert.ToInt32(Console.ReadLine());

                                Console.WriteLine("Please input SearchType:");
                                Console.WriteLine("0:Single Flag");
                                Console.WriteLine("1:Flag B");
                                Console.WriteLine("2:Flag A&B");
                                int SearchType = Convert.ToInt32(Console.ReadLine());

                                int Result = My_Reader._Config.SetEPCBaseBandParam(ConnID, basebandMode, qValue, session, SearchType);
                                if (Result == 0)
                                {
                                    Console.WriteLine("Success!");
                                }
                                else
                                {
                                    Console.WriteLine("Failure!");
                                }
                            }
                            else if (readKey.Equals("2"))
                            {
                                String Result = My_Reader._Config.GetEPCBaseBandParam(ConnID);
                                Console.WriteLine("basebandMode|qValue|session|searchType");
                                Console.WriteLine("basebandMode:");
                                Console.WriteLine("0:Tari=25us，FM0，LHF=40KHz");
                                Console.WriteLine("1:Tari=25us，Miller4，LHF=250KHz");
                                Console.WriteLine("2:Tari=25us，Miller4，LHF=300KH");
                                Console.WriteLine("3:Tari=6.25us，FM0，LHF=400KHz");
                                Console.WriteLine("255:Auto");
                                Console.WriteLine("searchType:");
                                Console.WriteLine("0:Single Flag");
                                Console.WriteLine("1:Flag B");
                                Console.WriteLine("2:Flag A&B");
                                Console.WriteLine(Result);
                            }
                            else if (readKey.Equals("3"))
                            {
                                Dictionary<Int32, Int32> AntPower = new Dictionary<Int32, Int32>();
                                while (true)
                                {
                                    Console.WriteLine("Please input antenna number:");
                                    int Num = Convert.ToInt32(Console.ReadLine());
                                    Console.WriteLine("Please input this antenna's power (dB) ");
                                    int Power = Convert.ToInt32(Console.ReadLine());

                                    AntPower.Add(Num, Power);

                                    Console.WriteLine("Setting another antenna power");
                                    Console.WriteLine("1.Yes");
                                    Console.WriteLine("2.No");

                                    readKey = Console.ReadLine();
                                    if (readKey.Equals("1"))
                                    {
                                        continue;
                                    }
                                    else if (readKey.Equals("2"))
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Parameter error ,please reset");
                                        AntPower = new Dictionary<Int32, Int32>();
                                    }
                                }

                                int Result = My_Reader._Config.SetANTPowerParam(ConnID, AntPower);
                                if (Result == 0)
                                {
                                    Console.WriteLine("Success!");
                                }
                                else
                                {
                                    Console.WriteLine("Failure!");
                                }
                            }
                            else if (readKey.Equals("4"))
                            {
                                Dictionary<Int32, Int32> Result = My_Reader._Config.GetANTPowerParam(ConnID);
                                string rt = string.Empty;
                                foreach (var item in Result)
                                {
                                    rt += item.Key + "," + item.Value + "&";
                                }
                                rt = rt.TrimEnd('&');
                                Console.WriteLine(rt);
                                Console.WriteLine("1,\"ANT1 Power\"&2,\"ANT2 Power\"&3,\"ANT3 Power\"&4,\"ANT4 Power\"&5,\"ANT5 Power\"&6,\"ANT6 Power\"&7,\"ANT7 Power\"&8,\"ANT8 Power\"");
                            }
                            else if (readKey.Equals("5"))
                            {
                                Console.WriteLine("Please input repeatTimeFilter:");
                                int repeatTimeFilter = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine("Please input RSSIFilter:");
                                int RSSIFilter = Convert.ToInt32(Console.ReadLine());
                                int Result = My_Reader._Config.SetTagUpdateParam(ConnID, repeatTimeFilter, RSSIFilter);
                                if (Result == 0)
                                {
                                    Console.WriteLine("Success!");
                                }
                                else
                                {
                                    Console.WriteLine("Failure!");
                                }
                            }
                            else if (readKey.Equals("6"))
                            {
                                String Result = My_Reader._Config.GetTagUpdateParam(ConnID);
                                Console.WriteLine(Result);
                                Console.WriteLine("repeatTimeFilter|RSSIFilter");
                                Console.WriteLine("the unit of repeatTimeFilter is 10ms");
                            }
                            else if (readKey.Equals("7"))
                            {
                                String Result = My_Reader._Config.GetReaderProperty(ConnID);
                                Console.WriteLine(Result);
                                Console.WriteLine("Minimum Power|Maximum Power|Ant Number|RF list|RFID protocol list");
                            }
                            else if (readKey.Equals("8"))
                            {
                                Console.WriteLine("Please select eRF_Range:");
                                Console.WriteLine("1.eRF_Range.GB_920_to_925MHz");
                                Console.WriteLine("2.eRF_Range.GB_840_to_845MHz");
                                Console.WriteLine("3.eRF_Range.GB_920_to_925MHz_and_GB_840_to_845MHz");
                                Console.WriteLine("4.eRF_Range.FCC_902_to_928MHz");
                                Console.WriteLine("5.eRF_Range.ETSI_866_to_868MHz");
                                readKey = Console.ReadLine();
                                if (readKey.Equals("1"))
                                {
                                    if (My_Reader._Config.SetReaderRF(ConnID, eRF_Range.GB_920_to_925MHz) == 0)
                                    {
                                        Console.WriteLine("Success!");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Failure!");
                                    }
                                }
                                else if (readKey.Equals("2"))
                                {
                                    if (My_Reader._Config.SetReaderRF(ConnID, eRF_Range.GB_840_to_845MHz) == 0)
                                    {
                                        Console.WriteLine("Success!");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Failure!");
                                    }
                                }
                                else if (readKey.Equals("3"))
                                {
                                    if (My_Reader._Config.SetReaderRF(ConnID, eRF_Range.GB_920_to_925MHz_and_GB_840_to_845MHz) == 0)
                                    {
                                        Console.WriteLine("Success!");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Failure!");
                                    }
                                }
                                else if (readKey.Equals("4"))
                                {
                                    if (My_Reader._Config.SetReaderRF(ConnID, eRF_Range.FCC_902_to_928MHz) == 0)
                                    {
                                        Console.WriteLine("Success!");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Failure!");
                                    }
                                }
                                else if (readKey.Equals("5"))
                                {
                                    if (My_Reader._Config.SetReaderRF(ConnID, eRF_Range.ETSI_866_to_868MHz) == 0)
                                    {
                                        Console.WriteLine("Success!");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Failure!");
                                    }
                                }
                            }
                            else if (readKey.Equals("9"))
                            {
                                Console.WriteLine(My_Reader._Config.GetReaderRF2(ConnID));
                            }
                            else if (readKey.Equals("10"))
                            {
                                Console.WriteLine("Please select switch: ");
                                Console.WriteLine("1.Open");
                                Console.WriteLine("2.Close");
                                readKey = Console.ReadLine();
                                Boolean Switch;
                                if (readKey.Equals("1"))
                                {
                                    Switch = true;
                                }
                                else if (readKey.Equals("2"))
                                {
                                    Switch = false;
                                }
                                else
                                {
                                    Console.WriteLine("Parameter error!");
                                    continue;
                                }

                                Console.WriteLine("Please input time: ");
                                Console.WriteLine("Unit:    10ms");
                                String time = Console.ReadLine();

                                if (My_Reader._Config.SetReaderAutoSleepParam(ConnID, Switch, time) == 0)
                                {
                                    Console.WriteLine("Success!");
                                }
                                else
                                {
                                    Console.WriteLine("Failure!");
                                }
                            }
                            else if (readKey.Equals("11"))
                            {
                                Console.WriteLine(My_Reader._Config.GetReaderAutoSleepParam(ConnID));
                                Console.WriteLine("Unit:    10ms");
                            }
                            else if (readKey.Equals("12"))
                            {
                                eAntennaNo antennaNo = new eAntennaNo();
                                while (true)
                                {
                                    Console.WriteLine("Please input antenna number:");
                                    readKey = Console.ReadLine();
                                    if (readKey.Equals("1"))
                                    {
                                        antennaNo |= eAntennaNo._1;
                                    }
                                    else if (readKey.Equals("2"))
                                    {
                                        antennaNo |= eAntennaNo._2;
                                    }
                                    else if (readKey.Equals("3"))
                                    {
                                        antennaNo |= eAntennaNo._3;
                                    }
                                    else if (readKey.Equals("4"))
                                    {
                                        antennaNo |= eAntennaNo._4;
                                    }
                                    else if (readKey.Equals("5"))
                                    {
                                        antennaNo |= eAntennaNo._5;
                                    }
                                    else if (readKey.Equals("6"))
                                    {
                                        antennaNo |= eAntennaNo._6;
                                    }
                                    else if (readKey.Equals("7"))
                                    {
                                        antennaNo |= eAntennaNo._7;
                                    }
                                    else if (readKey.Equals("8"))
                                    {
                                        antennaNo |= eAntennaNo._8;
                                    }
                                    else if (readKey.Equals("9"))
                                    {
                                        antennaNo |= eAntennaNo._9;
                                    }
                                    else if (readKey.Equals("10"))
                                    {
                                        antennaNo |= eAntennaNo._10;
                                    }
                                    else if (readKey.Equals("11"))
                                    {
                                        antennaNo |= eAntennaNo._11;
                                    }
                                    else if (readKey.Equals("12"))
                                    {
                                        antennaNo |= eAntennaNo._12;
                                    }
                                    else if (readKey.Equals("13"))
                                    {
                                        antennaNo |= eAntennaNo._13;
                                    }
                                    else if (readKey.Equals("14"))
                                    {
                                        antennaNo |= eAntennaNo._14;
                                    }
                                    else if (readKey.Equals("15"))
                                    {
                                        antennaNo |= eAntennaNo._15;
                                    }
                                    else if (readKey.Equals("16"))
                                    {
                                        antennaNo |= eAntennaNo._16;
                                    }
                                    else if (readKey.Equals("17"))
                                    {
                                        antennaNo |= eAntennaNo._17;
                                    }
                                    else if (readKey.Equals("18"))
                                    {
                                        antennaNo |= eAntennaNo._18;
                                    }
                                    else if (readKey.Equals("19"))
                                    {
                                        antennaNo |= eAntennaNo._19;
                                    }
                                    else if (readKey.Equals("20"))
                                    {
                                        antennaNo |= eAntennaNo._20;
                                    }
                                    else if (readKey.Equals("21"))
                                    {
                                        antennaNo |= eAntennaNo._21;
                                    }
                                    else if (readKey.Equals("22"))
                                    {
                                        antennaNo |= eAntennaNo._22;
                                    }
                                    else if (readKey.Equals("23"))
                                    {
                                        antennaNo |= eAntennaNo._23;
                                    }
                                    else if (readKey.Equals("24"))
                                    {
                                        antennaNo |= eAntennaNo._24;
                                    }
                                    else
                                    {
                                        Console.WriteLine("antenna number error");
                                        continue;
                                    }

                                    Console.WriteLine("Setting another antenna enable");
                                    Console.WriteLine("1.Yes");
                                    Console.WriteLine("2.No");
                                    readKey = Console.ReadLine();
                                    if (readKey.Equals("1"))
                                    {
                                        continue;
                                    }
                                    else if (readKey.Equals("2"))
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Parameter error ,please reset");
                                        antennaNo = 0;
                                    }
                                }

                                if (My_Reader._Config.SetReaderANT(ConnID, antennaNo) == 0)
                                {
                                    Console.WriteLine("Success!");
                                }
                                else
                                {
                                    Console.WriteLine("Failure!");
                                }
                            }
                            else if (readKey.Equals("13"))
                            {
                                Console.WriteLine(My_Reader._Config.GetReaderANT2(ConnID));
                            }
                            else if (readKey.Equals("14"))
                            {
                                int result = My_Reader._Config.SetReaderWorkFrequency(ConnID, eWF_Mode.Specified, new List<eETSI_866_to_868MHz>() { eETSI_866_to_868MHz._865_7f, eETSI_866_to_868MHz._866_3f });
                                if (result == 0)
                                {
                                    Console.WriteLine("Success!");
                                }
                                else
                                {
                                    Console.WriteLine("Failure!");
                                }
                            }
                            else if (readKey.Equals("15"))
                            {
                                Console.WriteLine(My_Reader._Config.GetReaderWorkFrequency(ConnID));
                            }
                            else if (readKey.Equals("b"))
                            {
                                break;
                            }
                            else if (readKey.Equals("q"))
                            {
                                My_Reader._Config.Stop(ConnID);
                                My_Reader.CloseConn(ConnID);
                                return;
                            }

                        }

                    }
                    #endregion

                    #region GPIO Setting
                    else if (readKey.Equals("4"))
                    {
                        while (true)
                        {
                            Console.WriteLine("Current ConnID : " + ConnID + "\n");
                            Console.WriteLine("Please select operation:");
                            Console.WriteLine("1.Set GPI");
                            Console.WriteLine("2.Get GPI");
                            Console.WriteLine("3.Get GPI State");
                            Console.WriteLine("4.Set GPO");
                            Console.WriteLine("5.Set Wiegand");
                            Console.WriteLine("6.Get Wiegand");
                            Console.WriteLine("b.Back to Previous Menu");
                            Console.WriteLine("q.Quit");

                            readKey = Console.ReadLine();
                            if (readKey.Equals("1"))
                            {
                                Console.WriteLine("Please select GPINum");
                                Console.WriteLine("1.eGPI._1");
                                Console.WriteLine("2.eGPI._2");
                                Console.WriteLine("3.eGPI._3");
                                Console.WriteLine("4.eGPI._4");
                                eGPI GPINum = new eGPI();

                                readKey = Console.ReadLine();
                                if (readKey.Equals("1"))
                                {
                                    GPINum = eGPI._1;
                                }
                                else if (readKey.Equals("2"))
                                {
                                    GPINum = eGPI._2;
                                }
                                else if (readKey.Equals("3"))
                                {
                                    GPINum = eGPI._3;
                                }
                                else if (readKey.Equals("4"))
                                {
                                    GPINum = eGPI._4;
                                }

                                Console.WriteLine("Please select triggerStart");
                                Console.WriteLine("1.eTriggerStart.OFF");
                                Console.WriteLine("2.eTriggerStart.Low_level");
                                Console.WriteLine("3.TriggerStart.High_level");
                                Console.WriteLine("4.eTriggerStart.Rising_edge");
                                Console.WriteLine("5.eTriggerStart.Falling_edge");
                                Console.WriteLine("6.eTriggerStart.Any_edge");
                                eTriggerStart triggerStart = new eTriggerStart();

                                readKey = Console.ReadLine();
                                if (readKey.Equals("1"))
                                {
                                    triggerStart = eTriggerStart.OFF;
                                }
                                else if (readKey.Equals("2"))
                                {
                                    triggerStart = eTriggerStart.Low_level;
                                }
                                else if (readKey.Equals("3"))
                                {
                                    triggerStart = eTriggerStart.High_level;
                                }
                                else if (readKey.Equals("4"))
                                {
                                    triggerStart = eTriggerStart.Rising_edge;
                                }
                                else if (readKey.Equals("5"))
                                {
                                    triggerStart = eTriggerStart.Falling_edge;
                                }
                                else if (readKey.Equals("6"))
                                {
                                    triggerStart = eTriggerStart.Any_edge;
                                }

                                Console.WriteLine("Please select triggerCode");
                                Console.WriteLine("1.triggerCode.Single_Antenna_read_EPC");
                                Console.WriteLine("2.triggerCode.Single_Antenna_read_EPC_and_TID");
                                Console.WriteLine("3.triggerCode.Double_Antenna_read_EPC");
                                Console.WriteLine("4.triggerCode.Double_Antenna_read_EPC_and_TID");
                                Console.WriteLine("5.triggerCode.Four_Antenna_read_EPC");
                                Console.WriteLine("6.triggerCode.Four_Antenna_read_EPC_and_TID");
                                Console.WriteLine("7.triggerCode.Eight_Antenna_read_EPC");
                                Console.WriteLine("8.triggerCode.Eight_Antenna_read_EPC_and_TID");
                                Console.WriteLine("9.triggerCode.Twelve_Antenna_read_EPC");
                                Console.WriteLine("10.triggerCode.Twelve_Antenna_read_EPC_and_TID");
                                Console.WriteLine("11.triggerCode.Twenty_four_Antenna_read_EPC");
                                Console.WriteLine("12.triggerCode.Twenty_four_Antenna_read_EPC_and_TID");
                                Console.WriteLine("13.triggerCode.Custom_Read_0");
                                Console.WriteLine("14.triggerCode.Custom_Read_1");
                                Console.WriteLine("15.triggerCode.Custom_Read_2");


                                eTriggerCode triggerCode = new eTriggerCode();

                                readKey = Console.ReadLine();
                                if (readKey.Equals("1"))
                                {
                                    triggerCode = eTriggerCode.Single_Antenna_read_EPC;
                                }
                                else if (readKey.Equals("2"))
                                {
                                    triggerCode = eTriggerCode.Single_Antenna_read_EPC_and_TID;
                                }
                                else if (readKey.Equals("3"))
                                {
                                    triggerCode = eTriggerCode.Double_Antenna_read_EPC;
                                }
                                else if (readKey.Equals("4"))
                                {
                                    triggerCode = eTriggerCode.Double_Antenna_read_EPC_and_TID;
                                }
                                else if (readKey.Equals("5"))
                                {
                                    triggerCode = eTriggerCode.Four_Antenna_read_EPC;
                                }
                                else if (readKey.Equals("6"))
                                {
                                    triggerCode = eTriggerCode.Four_Antenna_read_EPC_and_TID;
                                }
                                else if (readKey.Equals("7"))
                                {
                                    triggerCode = eTriggerCode.Eight_Antenna_read_EPC;
                                }
                                else if (readKey.Equals("8"))
                                {
                                    triggerCode = eTriggerCode.Eight_Antenna_read_EPC_and_TID;
                                }
                                else if (readKey.Equals("9"))
                                {
                                    triggerCode = eTriggerCode.Twelve_Antenna_read_EPC;
                                }
                                else if (readKey.Equals("10"))
                                {
                                    triggerCode = eTriggerCode.Twelve_Antenna_read_EPC_and_TID;
                                }
                                else if (readKey.Equals("11"))
                                {
                                    triggerCode = eTriggerCode.Twenty_four_Antenna_read_EPC;
                                }
                                else if (readKey.Equals("12"))
                                {
                                    triggerCode = eTriggerCode.Twenty_four_Antenna_read_EPC_and_TID;
                                }
                                else if (readKey.Equals("13"))
                                {
                                    triggerCode = eTriggerCode.Custom_Read_0;
                                }
                                else if (readKey.Equals("14"))
                                {
                                    triggerCode = eTriggerCode.Custom_Read_1;
                                }
                                else if (readKey.Equals("15"))
                                {
                                    triggerCode = eTriggerCode.Custom_Read_2;
                                }

                                Console.WriteLine("Please select triggerStop");
                                Console.WriteLine("1.eTriggerStart.OFF");
                                Console.WriteLine("2.eTriggerStart.Low_level");
                                Console.WriteLine("3.TriggerStart.High_level");
                                Console.WriteLine("4.eTriggerStart.Rising_edge");
                                Console.WriteLine("5.eTriggerStart.Falling_edge");
                                Console.WriteLine("6.eTriggerStart.Any_edge");
                                Console.WriteLine("7.eTriggerStart.Delay");
                                eTriggerStop triggerStop = new eTriggerStop();

                                readKey = Console.ReadLine();
                                if (readKey.Equals("1"))
                                {
                                    triggerStop = eTriggerStop.OFF;
                                }
                                else if (readKey.Equals("2"))
                                {
                                    triggerStop = eTriggerStop.Low_level;
                                }
                                else if (readKey.Equals("3"))
                                {
                                    triggerStop = eTriggerStop.High_level;
                                }
                                else if (readKey.Equals("4"))
                                {
                                    triggerStop = eTriggerStop.Rising_edge;
                                }
                                else if (readKey.Equals("5"))
                                {
                                    triggerStop = eTriggerStop.Falling_edge;
                                }
                                else if (readKey.Equals("6"))
                                {
                                    triggerStop = eTriggerStop.Any_edge;
                                }
                                else if (readKey.Equals("7"))
                                {
                                    triggerStop = eTriggerStop.Delay;
                                }


                                bool uploadSwitch = true;
                                if (triggerStop == eTriggerStop.OFF)
                                {
                                    Console.WriteLine("Please select upload switch");
                                    Console.WriteLine("1.Upload");
                                    Console.WriteLine("2.No upload");
                                    readKey = Console.ReadLine();
                                    if (readKey.Equals("1"))
                                    {
                                        uploadSwitch = true;
                                    }

                                    if (readKey.Equals("2"))
                                    {
                                        uploadSwitch = false;
                                    }

                                }

                                String DelayTime = "";
                                if (triggerStop == eTriggerStop.Delay)
                                {
                                    Console.WriteLine("Please input delaytime, unit:10ms");
                                    DelayTime = Console.ReadLine();
                                }

                                if (My_Reader._Config.SetReaderGPIParam(ConnID, GPINum, triggerStart, triggerCode, triggerStop, DelayTime, uploadSwitch) == 0)
                                {
                                    Console.WriteLine("Success!");
                                }
                                else
                                {
                                    Console.WriteLine("Failure!");
                                }

                            }
                            else if (readKey.Equals("2"))
                            {
                                Console.WriteLine("Please select GPINum");
                                Console.WriteLine("1.eGPI._1");
                                Console.WriteLine("2.eGPI._2");
                                Console.WriteLine("3.eGPI._3");
                                Console.WriteLine("4.eGPI._4");
                                eGPI GPINum;

                                readKey = Console.ReadLine();
                                if (readKey.Equals("1"))
                                {
                                    GPINum = eGPI._1;
                                }
                                else if (readKey.Equals("2"))
                                {
                                    GPINum = eGPI._2;
                                }
                                else if (readKey.Equals("3"))
                                {
                                    GPINum = eGPI._3;
                                }
                                else if (readKey.Equals("4"))
                                {
                                    GPINum = eGPI._4;
                                }
                                else
                                {
                                    return;
                                }

                                Console.WriteLine(My_Reader._Config.GetReaderGPIParam(ConnID, GPINum));
                                Console.WriteLine("TriggerStart|TriggerCode|TriggerStop|TimeDelay(unit:10ms)|the Switch of GPI State’s upload when Trigger not stop(Negligible)");
                            }
                            else if (readKey.Equals("3"))
                            {
                                Console.WriteLine(My_Reader._Config.GetReaderGPIState(ConnID));
                                Console.WriteLine("1,\"GPI1 State\" & 2,\"GPI2 State\"( & 3,\"GPI3 State\" & 4,\"GPI4 State\")");
                            }
                            else if (readKey.Equals("4"))
                            {
                                Dictionary<eGPO, eGPOState> dicState = new Dictionary<eGPO, eGPOState>();
                                while (true)
                                {
                                    Console.WriteLine("Please select GPO number:");
                                    Console.WriteLine("1.GPO._1");
                                    Console.WriteLine("2.GPO._2");
                                    Console.WriteLine("3.GPO._3");
                                    Console.WriteLine("4.GPO._4");

                                    eGPO GPO;

                                    readKey = Console.ReadLine();
                                    if (readKey.Equals("1"))
                                    {
                                        GPO = eGPO._1;
                                    }
                                    else if (readKey.Equals("2"))
                                    {
                                        GPO = eGPO._2;
                                    }
                                    else if (readKey.Equals("3"))
                                    {
                                        GPO = eGPO._3;
                                    }
                                    else if (readKey.Equals("4"))
                                    {
                                        GPO = eGPO._4;
                                    }
                                    else
                                    {
                                        return;
                                    }


                                    Console.WriteLine("Please select GPO state ");
                                    Console.WriteLine("1.eGPOState.Low");
                                    Console.WriteLine("2.eGPOState.High");

                                    eGPOState GPOState;

                                    readKey = Console.ReadLine();
                                    if (readKey.Equals("1"))
                                    {
                                        GPOState = eGPOState.Low;
                                    }
                                    else if (readKey.Equals("2"))
                                    {
                                        GPOState = eGPOState.High;
                                    }
                                    else
                                    {
                                        return;
                                    }

                                    dicState.Add(GPO, GPOState);

                                    Console.WriteLine("Setting another GPO state");
                                    Console.WriteLine("1.Yes");
                                    Console.WriteLine("2.No");
                                    readKey = Console.ReadLine();
                                    if (readKey.Equals("1"))
                                    {
                                        continue;
                                    }
                                    else if (readKey.Equals("2"))
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Parameter error ,please reset");
                                        dicState = new Dictionary<eGPO, eGPOState>();
                                    }
                                }

                                int Result = My_Reader._Config.SetReaderGPOState(ConnID, dicState);
                                if (Result == 0)
                                {
                                    Console.WriteLine("Success!");
                                }
                                else
                                {
                                    Console.WriteLine("Failure!");
                                }
                            }
                            else if (readKey.Equals("5"))
                            {
                                Console.WriteLine("Please select wiegandSwitch ");
                                Console.WriteLine("1.eWiegandSwitch.Close");
                                Console.WriteLine("2.eWiegandSwitch.Open");

                                eWiegandSwitch wiegandSwitch;

                                readKey = Console.ReadLine();
                                if (readKey.Equals("1"))
                                {
                                    wiegandSwitch = eWiegandSwitch.Close;
                                }
                                else if (readKey.Equals("2"))
                                {
                                    wiegandSwitch = eWiegandSwitch.Open;
                                }
                                else
                                {
                                    return;
                                }

                                Console.WriteLine("Please select wiegandFormat ");
                                Console.WriteLine("1.eWiegandFormat.Wiegand26");
                                Console.WriteLine("2.eWiegandFormat.Wiegand34");
                                Console.WriteLine("3.eWiegandFormat.Wiegand66");

                                eWiegandFormat wiegandFormat;
                                readKey = Console.ReadLine();
                                if (readKey.Equals("1"))
                                {
                                    wiegandFormat = eWiegandFormat.Wiegand26;
                                }
                                else if (readKey.Equals("2"))
                                {
                                    wiegandFormat = eWiegandFormat.Wiegand34;
                                }
                                else if (readKey.Equals("3"))
                                {
                                    wiegandFormat = eWiegandFormat.Wiegand66;
                                }
                                else
                                {
                                    return;
                                }

                                Console.WriteLine("Please select wiegandDetails ");
                                Console.WriteLine("1.eWiegandDetails.end_of_the_EPC_data");
                                Console.WriteLine("2.eWiegandDetails.end_of_the_TID_data");

                                eWiegandDetails wiegandDetails;

                                readKey = Console.ReadLine();
                                if (readKey.Equals("1"))
                                {
                                    wiegandDetails = eWiegandDetails.end_of_the_EPC_data;
                                }
                                else if (readKey.Equals("2"))
                                {
                                    wiegandDetails = eWiegandDetails.end_of_the_TID_data;
                                }
                                else
                                {
                                    return;
                                }

                                if (My_Reader._Config.SetReaderWG(ConnID, wiegandSwitch, wiegandFormat, wiegandDetails) == 0)
                                {
                                    Console.WriteLine("Success!");
                                }
                                else
                                {
                                    Console.WriteLine("Failure!");
                                }

                            }
                            else if (readKey.Equals("6"))
                            {
                                Console.WriteLine(My_Reader._Config.GetReaderWG(ConnID));
                            }
                            else if (readKey.Equals("b"))
                            {
                                break;
                            }
                            else if (readKey.Equals("q"))
                            {
                                My_Reader._Config.Stop(ConnID);
                                My_Reader.CloseConn(ConnID);
                                return;
                            }
                        }
                    }
                    #endregion

                    #region Read 6C Tag
                    else if (readKey.Equals("5"))
                    {
                        while (true)
                        {
                            Console.WriteLine("Current ConnID : " + ConnID + "\n");
                            Console.WriteLine("Please select operation:");
                            Console.WriteLine("1.Read 6C Tag");
                            Console.WriteLine("2.Write 6C Tag");
                            Console.WriteLine("3.Lock 6C Tag");
                            Console.WriteLine("4.Kill 6C Tag");
                            Console.WriteLine("5.Get QT Parameter");
                            Console.WriteLine("6.Set QT Parameter");
                            Console.WriteLine("b.Back to Previous Menu");
                            Console.WriteLine("s.Stop Read");
                            Console.WriteLine("q.Quit");

                           // readKey = Console.ReadLine();
                            readKey = "1";



                            if (readKey.Equals("1"))
                            {
                                while (true)
                                {
                                    Console.WriteLine("Please select method:");
                                    Console.WriteLine("1.GetEPC(String ConnID, eAntennaNo antNum, eReadType readType)");
                                    Console.WriteLine("2.GetEPC(String ConnID, eAntennaNo antNum, eReadType readType, String accessPassword)");
                                    Console.WriteLine("3.GetEPC_MatchEPC(String ConnID, eAntennaNo antNum, eReadType readType, String sEPC)");
                                    Console.WriteLine("4.GetEPC_MatchEPC(String ConnID, eAntennaNo antNum, eReadType readType, String sEPC, int matchWordStartIndex)");
                                    Console.WriteLine("5.GetEPC_MatchEPC(String ConnID, eAntennaNo antNum, eReadType readType, String sEPC, int matchWordStartIndex, String accessPassword)");
                                    Console.WriteLine("6.GetEPC_MatchTID(String ConnID, eAntennaNo antNum, eReadType readType, String sTID)");
                                    Console.WriteLine("7.GetEPC_MatchTID(String ConnID, eAntennaNo antNum, eReadType readType, String sTID, int matchWordStartIndex)");
                                    Console.WriteLine("8.GetEPC_MatchTID(String ConnID, eAntennaNo antNum, eReadType readType, String sTID, int matchWordStartIndex, String accessPassword)");
                                    Console.WriteLine("9.GetEPC_TID(String ConnID, eAntennaNo antNum, eReadType readType)");
                                    Console.WriteLine("10.GetEPC_TID(String ConnID, eAntennaNo antNum, eReadType readType, String accessPassword)");
                                    Console.WriteLine("11.GetEPC_TID_MatchEPC(String ConnID, eAntennaNo antNum, eReadType readType, String sEPC)");
                                    Console.WriteLine("12.GetEPC_TID_MatchEPC(String ConnID, eAntennaNo antNum, eReadType readType, String sEPC, int matchWordStartIndex)");
                                    Console.WriteLine("13.GetEPC_TID_MatchEPC(String ConnID, eAntennaNo antNum, eReadType readType, String sEPC, int matchWordStartIndex, String accessPassword)");
                                    Console.WriteLine("14.GetEPC_TID_MatchTID(String ConnID, eAntennaNo antNum, eReadType readType, String sTID)");
                                    Console.WriteLine("15.GetEPC_TID_MatchTID(String ConnID, eAntennaNo antNum, eReadType readType, String sTID, int matchWordStartIndex)");
                                    Console.WriteLine("16.GetEPC_TID_MatchTID(String ConnID, eAntennaNo antNum, eReadType readType, String sTID, int matchWordStartIndex, String accessPassword)");
                                    Console.WriteLine("17.GetEPC_TID_UserData(String ConnID, eAntennaNo antNum, eReadType readType,int readStart, int readLen)");
                                    Console.WriteLine("18.GetEPC_TID_UserData(String ConnID, eAntennaNo antNum, eReadType readType,int readStart, int readLen, String accessPassword)");
                                    Console.WriteLine("19.GetEPC_TID_UserData_MatchEPC(String ConnID, eAntennaNo antNum, eReadType readType,int readStart, int readLen, String sEPC)");
                                    Console.WriteLine("20.GetEPC_TID_UserData_MatchEPC(String ConnID, eAntennaNo antNum, eReadType readType,int readStart, int readLen, String sEPC, int matchWordStartIndex)");
                                    Console.WriteLine("21.GetEPC_TID_UserData_MatchEPC(String ConnID, eAntennaNo antNum, eReadType readType,int readStart, int readLen, String sEPC, int matchWordStartIndex, String accessPassword)");
                                    Console.WriteLine("22.GetEPC_TID_UserData_MatchTID(String ConnID, eAntennaNo antNum, eReadType readType,int readStart, int readLen, String sTID)");
                                    Console.WriteLine("23.GetEPC_TID_UserData_MatchTID(String ConnID, eAntennaNo antNum, eReadType readType,int readStart, int readLen, String sTID, int matchWordStartIndex)");
                                    Console.WriteLine("24.GetEPC_TID_UserData_MatchTID(String ConnID, eAntennaNo antNum, eReadType readType,int readStart, int readLen, String sTID, int matchWordStartIndex, String accessPassword)");
                                    Console.WriteLine("s.Stop Read");
                                    Console.WriteLine("b.Back to Previous Menu");
                                    Console.WriteLine("q.Quit");

                                    //readKey = Console.ReadLine();
                                    readKey = "1";

                                    if (readKey.Equals("1"))
                                    {
                                        
                                        eAntennaNo antNum = eAntennaNo._1| eAntennaNo._3| eAntennaNo._5;
                                        /*
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else if (readKey.Equals("9"))
                                            {
                                                antNum |= eAntennaNo._9;
                                            }
                                            else if (readKey.Equals("10"))
                                            {
                                                antNum |= eAntennaNo._10;
                                            }
                                            else if (readKey.Equals("11"))
                                            {
                                                antNum |= eAntennaNo._11;
                                            }
                                            else if (readKey.Equals("12"))
                                            {
                                                antNum |= eAntennaNo._12;
                                            }
                                            else if (readKey.Equals("13"))
                                            {
                                                antNum |= eAntennaNo._13;
                                            }
                                            else if (readKey.Equals("14"))
                                            {
                                                antNum |= eAntennaNo._14;
                                            }
                                            else if (readKey.Equals("15"))
                                            {
                                                antNum |= eAntennaNo._15;
                                            }
                                            else if (readKey.Equals("16"))
                                            {
                                                antNum |= eAntennaNo._16;
                                            }
                                            else if (readKey.Equals("17"))
                                            {
                                                antNum |= eAntennaNo._17;
                                            }
                                            else if (readKey.Equals("18"))
                                            {
                                                antNum |= eAntennaNo._18;
                                            }
                                            else if (readKey.Equals("19"))
                                            {
                                                antNum |= eAntennaNo._19;
                                            }
                                            else if (readKey.Equals("20"))
                                            {
                                                antNum |= eAntennaNo._20;
                                            }
                                            else if (readKey.Equals("21"))
                                            {
                                                antNum |= eAntennaNo._21;
                                            }
                                            else if (readKey.Equals("22"))
                                            {
                                                antNum |= eAntennaNo._22;
                                            }
                                            else if (readKey.Equals("23"))
                                            {
                                                antNum |= eAntennaNo._23;
                                            }
                                            else if (readKey.Equals("24"))
                                            {
                                                antNum |= eAntennaNo._24;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = 0;
                                            }
                                        }
                                        */
                                        eReadType readType;
                                        //antNum = 21;
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        //readKey = Console.ReadLine();
                                        readKey = "1";
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                            Console.WriteLine(antNum);


                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        if (My_Reader._Tag6C.GetEPC(ConnID, antNum, readType) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                            Console.WriteLine(antNum);

                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }

                                    }
                                    else if (readKey.Equals("2"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else if (readKey.Equals("9"))
                                            {
                                                antNum |= eAntennaNo._9;
                                            }
                                            else if (readKey.Equals("10"))
                                            {
                                                antNum |= eAntennaNo._10;
                                            }
                                            else if (readKey.Equals("11"))
                                            {
                                                antNum |= eAntennaNo._11;
                                            }
                                            else if (readKey.Equals("12"))
                                            {
                                                antNum |= eAntennaNo._12;
                                            }
                                            else if (readKey.Equals("13"))
                                            {
                                                antNum |= eAntennaNo._13;
                                            }
                                            else if (readKey.Equals("14"))
                                            {
                                                antNum |= eAntennaNo._14;
                                            }
                                            else if (readKey.Equals("15"))
                                            {
                                                antNum |= eAntennaNo._15;
                                            }
                                            else if (readKey.Equals("16"))
                                            {
                                                antNum |= eAntennaNo._16;
                                            }
                                            else if (readKey.Equals("17"))
                                            {
                                                antNum |= eAntennaNo._17;
                                            }
                                            else if (readKey.Equals("18"))
                                            {
                                                antNum |= eAntennaNo._18;
                                            }
                                            else if (readKey.Equals("19"))
                                            {
                                                antNum |= eAntennaNo._19;
                                            }
                                            else if (readKey.Equals("20"))
                                            {
                                                antNum |= eAntennaNo._20;
                                            }
                                            else if (readKey.Equals("21"))
                                            {
                                                antNum |= eAntennaNo._21;
                                            }
                                            else if (readKey.Equals("22"))
                                            {
                                                antNum |= eAntennaNo._22;
                                            }
                                            else if (readKey.Equals("23"))
                                            {
                                                antNum |= eAntennaNo._23;
                                            }
                                            else if (readKey.Equals("24"))
                                            {
                                                antNum |= eAntennaNo._24;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = 0;
                                            }
                                        }

                                        eReadType readType;
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input accessPassword:");
                                        String accessPassword = Console.ReadLine();

                                        if (My_Reader._Tag6C.GetEPC(ConnID, antNum, readType, accessPassword) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }

                                    }
                                    else if (readKey.Equals("3"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else if (readKey.Equals("9"))
                                            {
                                                antNum |= eAntennaNo._9;
                                            }
                                            else if (readKey.Equals("10"))
                                            {
                                                antNum |= eAntennaNo._10;
                                            }
                                            else if (readKey.Equals("11"))
                                            {
                                                antNum |= eAntennaNo._11;
                                            }
                                            else if (readKey.Equals("12"))
                                            {
                                                antNum |= eAntennaNo._12;
                                            }
                                            else if (readKey.Equals("13"))
                                            {
                                                antNum |= eAntennaNo._13;
                                            }
                                            else if (readKey.Equals("14"))
                                            {
                                                antNum |= eAntennaNo._14;
                                            }
                                            else if (readKey.Equals("15"))
                                            {
                                                antNum |= eAntennaNo._15;
                                            }
                                            else if (readKey.Equals("16"))
                                            {
                                                antNum |= eAntennaNo._16;
                                            }
                                            else if (readKey.Equals("17"))
                                            {
                                                antNum |= eAntennaNo._17;
                                            }
                                            else if (readKey.Equals("18"))
                                            {
                                                antNum |= eAntennaNo._18;
                                            }
                                            else if (readKey.Equals("19"))
                                            {
                                                antNum |= eAntennaNo._19;
                                            }
                                            else if (readKey.Equals("20"))
                                            {
                                                antNum |= eAntennaNo._20;
                                            }
                                            else if (readKey.Equals("21"))
                                            {
                                                antNum |= eAntennaNo._21;
                                            }
                                            else if (readKey.Equals("22"))
                                            {
                                                antNum |= eAntennaNo._22;
                                            }
                                            else if (readKey.Equals("23"))
                                            {
                                                antNum |= eAntennaNo._23;
                                            }
                                            else if (readKey.Equals("24"))
                                            {
                                                antNum |= eAntennaNo._24;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = 0;
                                            }
                                        }

                                        eReadType readType;
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input sEPC:");
                                        String sEPC = Console.ReadLine();

                                        if (My_Reader._Tag6C.GetEPC_MatchEPC(ConnID, antNum, readType, sEPC) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("4"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else if (readKey.Equals("9"))
                                            {
                                                antNum |= eAntennaNo._9;
                                            }
                                            else if (readKey.Equals("10"))
                                            {
                                                antNum |= eAntennaNo._10;
                                            }
                                            else if (readKey.Equals("11"))
                                            {
                                                antNum |= eAntennaNo._11;
                                            }
                                            else if (readKey.Equals("12"))
                                            {
                                                antNum |= eAntennaNo._12;
                                            }
                                            else if (readKey.Equals("13"))
                                            {
                                                antNum |= eAntennaNo._13;
                                            }
                                            else if (readKey.Equals("14"))
                                            {
                                                antNum |= eAntennaNo._14;
                                            }
                                            else if (readKey.Equals("15"))
                                            {
                                                antNum |= eAntennaNo._15;
                                            }
                                            else if (readKey.Equals("16"))
                                            {
                                                antNum |= eAntennaNo._16;
                                            }
                                            else if (readKey.Equals("17"))
                                            {
                                                antNum |= eAntennaNo._17;
                                            }
                                            else if (readKey.Equals("18"))
                                            {
                                                antNum |= eAntennaNo._18;
                                            }
                                            else if (readKey.Equals("19"))
                                            {
                                                antNum |= eAntennaNo._19;
                                            }
                                            else if (readKey.Equals("20"))
                                            {
                                                antNum |= eAntennaNo._20;
                                            }
                                            else if (readKey.Equals("21"))
                                            {
                                                antNum |= eAntennaNo._21;
                                            }
                                            else if (readKey.Equals("22"))
                                            {
                                                antNum |= eAntennaNo._22;
                                            }
                                            else if (readKey.Equals("23"))
                                            {
                                                antNum |= eAntennaNo._23;
                                            }
                                            else if (readKey.Equals("24"))
                                            {
                                                antNum |= eAntennaNo._24;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = 0;
                                            }
                                        }

                                        eReadType readType;
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input sEPC:");
                                        String sEPC = Console.ReadLine();

                                        Console.WriteLine("Please input matchWordStartIndex:");
                                        int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                        if (My_Reader._Tag6C.GetEPC_MatchEPC(ConnID, antNum, readType, sEPC, matchWordStartIndex) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("5"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else if (readKey.Equals("9"))
                                            {
                                                antNum |= eAntennaNo._9;
                                            }
                                            else if (readKey.Equals("10"))
                                            {
                                                antNum |= eAntennaNo._10;
                                            }
                                            else if (readKey.Equals("11"))
                                            {
                                                antNum |= eAntennaNo._11;
                                            }
                                            else if (readKey.Equals("12"))
                                            {
                                                antNum |= eAntennaNo._12;
                                            }
                                            else if (readKey.Equals("13"))
                                            {
                                                antNum |= eAntennaNo._13;
                                            }
                                            else if (readKey.Equals("14"))
                                            {
                                                antNum |= eAntennaNo._14;
                                            }
                                            else if (readKey.Equals("15"))
                                            {
                                                antNum |= eAntennaNo._15;
                                            }
                                            else if (readKey.Equals("16"))
                                            {
                                                antNum |= eAntennaNo._16;
                                            }
                                            else if (readKey.Equals("17"))
                                            {
                                                antNum |= eAntennaNo._17;
                                            }
                                            else if (readKey.Equals("18"))
                                            {
                                                antNum |= eAntennaNo._18;
                                            }
                                            else if (readKey.Equals("19"))
                                            {
                                                antNum |= eAntennaNo._19;
                                            }
                                            else if (readKey.Equals("20"))
                                            {
                                                antNum |= eAntennaNo._20;
                                            }
                                            else if (readKey.Equals("21"))
                                            {
                                                antNum |= eAntennaNo._21;
                                            }
                                            else if (readKey.Equals("22"))
                                            {
                                                antNum |= eAntennaNo._22;
                                            }
                                            else if (readKey.Equals("23"))
                                            {
                                                antNum |= eAntennaNo._23;
                                            }
                                            else if (readKey.Equals("24"))
                                            {
                                                antNum |= eAntennaNo._24;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = 0;
                                            }
                                        }

                                        eReadType readType;
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input sEPC:");
                                        String sEPC = Console.ReadLine();

                                        Console.WriteLine("Please input matchWordStartIndex:");
                                        int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input accessPassword:");
                                        String accessPassword = Console.ReadLine();

                                        if (My_Reader._Tag6C.GetEPC_MatchEPC(ConnID, antNum, readType, sEPC, matchWordStartIndex, accessPassword) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("6"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else if (readKey.Equals("9"))
                                            {
                                                antNum |= eAntennaNo._9;
                                            }
                                            else if (readKey.Equals("10"))
                                            {
                                                antNum |= eAntennaNo._10;
                                            }
                                            else if (readKey.Equals("11"))
                                            {
                                                antNum |= eAntennaNo._11;
                                            }
                                            else if (readKey.Equals("12"))
                                            {
                                                antNum |= eAntennaNo._12;
                                            }
                                            else if (readKey.Equals("13"))
                                            {
                                                antNum |= eAntennaNo._13;
                                            }
                                            else if (readKey.Equals("14"))
                                            {
                                                antNum |= eAntennaNo._14;
                                            }
                                            else if (readKey.Equals("15"))
                                            {
                                                antNum |= eAntennaNo._15;
                                            }
                                            else if (readKey.Equals("16"))
                                            {
                                                antNum |= eAntennaNo._16;
                                            }
                                            else if (readKey.Equals("17"))
                                            {
                                                antNum |= eAntennaNo._17;
                                            }
                                            else if (readKey.Equals("18"))
                                            {
                                                antNum |= eAntennaNo._18;
                                            }
                                            else if (readKey.Equals("19"))
                                            {
                                                antNum |= eAntennaNo._19;
                                            }
                                            else if (readKey.Equals("20"))
                                            {
                                                antNum |= eAntennaNo._20;
                                            }
                                            else if (readKey.Equals("21"))
                                            {
                                                antNum |= eAntennaNo._21;
                                            }
                                            else if (readKey.Equals("22"))
                                            {
                                                antNum |= eAntennaNo._22;
                                            }
                                            else if (readKey.Equals("23"))
                                            {
                                                antNum |= eAntennaNo._23;
                                            }
                                            else if (readKey.Equals("24"))
                                            {
                                                antNum |= eAntennaNo._24;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = 0;
                                            }
                                        }

                                        eReadType readType;
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input sTID:");
                                        String sTID = Console.ReadLine();

                                        if (My_Reader._Tag6C.GetEPC_MatchTID(ConnID, antNum, readType, sTID) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("7"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else if (readKey.Equals("9"))
                                            {
                                                antNum |= eAntennaNo._9;
                                            }
                                            else if (readKey.Equals("10"))
                                            {
                                                antNum |= eAntennaNo._10;
                                            }
                                            else if (readKey.Equals("11"))
                                            {
                                                antNum |= eAntennaNo._11;
                                            }
                                            else if (readKey.Equals("12"))
                                            {
                                                antNum |= eAntennaNo._12;
                                            }
                                            else if (readKey.Equals("13"))
                                            {
                                                antNum |= eAntennaNo._13;
                                            }
                                            else if (readKey.Equals("14"))
                                            {
                                                antNum |= eAntennaNo._14;
                                            }
                                            else if (readKey.Equals("15"))
                                            {
                                                antNum |= eAntennaNo._15;
                                            }
                                            else if (readKey.Equals("16"))
                                            {
                                                antNum |= eAntennaNo._16;
                                            }
                                            else if (readKey.Equals("17"))
                                            {
                                                antNum |= eAntennaNo._17;
                                            }
                                            else if (readKey.Equals("18"))
                                            {
                                                antNum |= eAntennaNo._18;
                                            }
                                            else if (readKey.Equals("19"))
                                            {
                                                antNum |= eAntennaNo._19;
                                            }
                                            else if (readKey.Equals("20"))
                                            {
                                                antNum |= eAntennaNo._20;
                                            }
                                            else if (readKey.Equals("21"))
                                            {
                                                antNum |= eAntennaNo._21;
                                            }
                                            else if (readKey.Equals("22"))
                                            {
                                                antNum |= eAntennaNo._22;
                                            }
                                            else if (readKey.Equals("23"))
                                            {
                                                antNum |= eAntennaNo._23;
                                            }
                                            else if (readKey.Equals("24"))
                                            {
                                                antNum |= eAntennaNo._24;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");

                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = 0;
                                            }
                                        }

                                        eReadType readType;
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input sTID:");
                                        String sTID = Console.ReadLine();

                                        Console.WriteLine("Please input matchWordStartIndex:");
                                        int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                        if (My_Reader._Tag6C.GetEPC_MatchTID(ConnID, antNum, readType, sTID, matchWordStartIndex) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("8"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else if (readKey.Equals("9"))
                                            {
                                                antNum |= eAntennaNo._9;
                                            }
                                            else if (readKey.Equals("10"))
                                            {
                                                antNum |= eAntennaNo._10;
                                            }
                                            else if (readKey.Equals("11"))
                                            {
                                                antNum |= eAntennaNo._11;
                                            }
                                            else if (readKey.Equals("12"))
                                            {
                                                antNum |= eAntennaNo._12;
                                            }
                                            else if (readKey.Equals("13"))
                                            {
                                                antNum |= eAntennaNo._13;
                                            }
                                            else if (readKey.Equals("14"))
                                            {
                                                antNum |= eAntennaNo._14;
                                            }
                                            else if (readKey.Equals("15"))
                                            {
                                                antNum |= eAntennaNo._15;
                                            }
                                            else if (readKey.Equals("16"))
                                            {
                                                antNum |= eAntennaNo._16;
                                            }
                                            else if (readKey.Equals("17"))
                                            {
                                                antNum |= eAntennaNo._17;
                                            }
                                            else if (readKey.Equals("18"))
                                            {
                                                antNum |= eAntennaNo._18;
                                            }
                                            else if (readKey.Equals("19"))
                                            {
                                                antNum |= eAntennaNo._19;
                                            }
                                            else if (readKey.Equals("20"))
                                            {
                                                antNum |= eAntennaNo._20;
                                            }
                                            else if (readKey.Equals("21"))
                                            {
                                                antNum |= eAntennaNo._21;
                                            }
                                            else if (readKey.Equals("22"))
                                            {
                                                antNum |= eAntennaNo._22;
                                            }
                                            else if (readKey.Equals("23"))
                                            {
                                                antNum |= eAntennaNo._23;
                                            }
                                            else if (readKey.Equals("24"))
                                            {
                                                antNum |= eAntennaNo._24;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = 0;
                                            }
                                        }

                                        eReadType readType;
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input sTID:");
                                        String sTID = Console.ReadLine();

                                        Console.WriteLine("Please input matchWordStartIndex:");
                                        int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input accessPassword:");
                                        String accessPassword = Console.ReadLine();

                                        if (My_Reader._Tag6C.GetEPC_MatchTID(ConnID, antNum, readType, sTID, matchWordStartIndex, accessPassword) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("9"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else if (readKey.Equals("9"))
                                            {
                                                antNum |= eAntennaNo._9;
                                            }
                                            else if (readKey.Equals("10"))
                                            {
                                                antNum |= eAntennaNo._10;
                                            }
                                            else if (readKey.Equals("11"))
                                            {
                                                antNum |= eAntennaNo._11;
                                            }
                                            else if (readKey.Equals("12"))
                                            {
                                                antNum |= eAntennaNo._12;
                                            }
                                            else if (readKey.Equals("13"))
                                            {
                                                antNum |= eAntennaNo._13;
                                            }
                                            else if (readKey.Equals("14"))
                                            {
                                                antNum |= eAntennaNo._14;
                                            }
                                            else if (readKey.Equals("15"))
                                            {
                                                antNum |= eAntennaNo._15;
                                            }
                                            else if (readKey.Equals("16"))
                                            {
                                                antNum |= eAntennaNo._16;
                                            }
                                            else if (readKey.Equals("17"))
                                            {
                                                antNum |= eAntennaNo._17;
                                            }
                                            else if (readKey.Equals("18"))
                                            {
                                                antNum |= eAntennaNo._18;
                                            }
                                            else if (readKey.Equals("19"))
                                            {
                                                antNum |= eAntennaNo._19;
                                            }
                                            else if (readKey.Equals("20"))
                                            {
                                                antNum |= eAntennaNo._20;
                                            }
                                            else if (readKey.Equals("21"))
                                            {
                                                antNum |= eAntennaNo._21;
                                            }
                                            else if (readKey.Equals("22"))
                                            {
                                                antNum |= eAntennaNo._22;
                                            }
                                            else if (readKey.Equals("23"))
                                            {
                                                antNum |= eAntennaNo._23;
                                            }
                                            else if (readKey.Equals("24"))
                                            {
                                                antNum |= eAntennaNo._24;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = 0;
                                            }
                                        }

                                        eReadType readType;
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        if (My_Reader._Tag6C.GetEPC_TID(ConnID, antNum, readType) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("10"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else if (readKey.Equals("9"))
                                            {
                                                antNum |= eAntennaNo._9;
                                            }
                                            else if (readKey.Equals("10"))
                                            {
                                                antNum |= eAntennaNo._10;
                                            }
                                            else if (readKey.Equals("11"))
                                            {
                                                antNum |= eAntennaNo._11;
                                            }
                                            else if (readKey.Equals("12"))
                                            {
                                                antNum |= eAntennaNo._12;
                                            }
                                            else if (readKey.Equals("13"))
                                            {
                                                antNum |= eAntennaNo._13;
                                            }
                                            else if (readKey.Equals("14"))
                                            {
                                                antNum |= eAntennaNo._14;
                                            }
                                            else if (readKey.Equals("15"))
                                            {
                                                antNum |= eAntennaNo._15;
                                            }
                                            else if (readKey.Equals("16"))
                                            {
                                                antNum |= eAntennaNo._16;
                                            }
                                            else if (readKey.Equals("17"))
                                            {
                                                antNum |= eAntennaNo._17;
                                            }
                                            else if (readKey.Equals("18"))
                                            {
                                                antNum |= eAntennaNo._18;
                                            }
                                            else if (readKey.Equals("19"))
                                            {
                                                antNum |= eAntennaNo._19;
                                            }
                                            else if (readKey.Equals("20"))
                                            {
                                                antNum |= eAntennaNo._20;
                                            }
                                            else if (readKey.Equals("21"))
                                            {
                                                antNum |= eAntennaNo._21;
                                            }
                                            else if (readKey.Equals("22"))
                                            {
                                                antNum |= eAntennaNo._22;
                                            }
                                            else if (readKey.Equals("23"))
                                            {
                                                antNum |= eAntennaNo._23;
                                            }
                                            else if (readKey.Equals("24"))
                                            {
                                                antNum |= eAntennaNo._24;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = 0;
                                            }
                                        }

                                        eReadType readType;
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input accessPassword:");
                                        String accessPassword = Console.ReadLine();

                                        if (My_Reader._Tag6C.GetEPC_TID(ConnID, antNum, readType, accessPassword) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("11"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else if (readKey.Equals("9"))
                                            {
                                                antNum |= eAntennaNo._9;
                                            }
                                            else if (readKey.Equals("10"))
                                            {
                                                antNum |= eAntennaNo._10;
                                            }
                                            else if (readKey.Equals("11"))
                                            {
                                                antNum |= eAntennaNo._11;
                                            }
                                            else if (readKey.Equals("12"))
                                            {
                                                antNum |= eAntennaNo._12;
                                            }
                                            else if (readKey.Equals("13"))
                                            {
                                                antNum |= eAntennaNo._13;
                                            }
                                            else if (readKey.Equals("14"))
                                            {
                                                antNum |= eAntennaNo._14;
                                            }
                                            else if (readKey.Equals("15"))
                                            {
                                                antNum |= eAntennaNo._15;
                                            }
                                            else if (readKey.Equals("16"))
                                            {
                                                antNum |= eAntennaNo._16;
                                            }
                                            else if (readKey.Equals("17"))
                                            {
                                                antNum |= eAntennaNo._17;
                                            }
                                            else if (readKey.Equals("18"))
                                            {
                                                antNum |= eAntennaNo._18;
                                            }
                                            else if (readKey.Equals("19"))
                                            {
                                                antNum |= eAntennaNo._19;
                                            }
                                            else if (readKey.Equals("20"))
                                            {
                                                antNum |= eAntennaNo._20;
                                            }
                                            else if (readKey.Equals("21"))
                                            {
                                                antNum |= eAntennaNo._21;
                                            }
                                            else if (readKey.Equals("22"))
                                            {
                                                antNum |= eAntennaNo._22;
                                            }
                                            else if (readKey.Equals("23"))
                                            {
                                                antNum |= eAntennaNo._23;
                                            }
                                            else if (readKey.Equals("24"))
                                            {
                                                antNum |= eAntennaNo._24;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = 0;
                                            }
                                        }

                                        eReadType readType;
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input sEPC:");
                                        String sEPC = Console.ReadLine();

                                        if (My_Reader._Tag6C.GetEPC_TID_MatchEPC(ConnID, antNum, readType, sEPC) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("12"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else if (readKey.Equals("9"))
                                            {
                                                antNum |= eAntennaNo._9;
                                            }
                                            else if (readKey.Equals("10"))
                                            {
                                                antNum |= eAntennaNo._10;
                                            }
                                            else if (readKey.Equals("11"))
                                            {
                                                antNum |= eAntennaNo._11;
                                            }
                                            else if (readKey.Equals("12"))
                                            {
                                                antNum |= eAntennaNo._12;
                                            }
                                            else if (readKey.Equals("13"))
                                            {
                                                antNum |= eAntennaNo._13;
                                            }
                                            else if (readKey.Equals("14"))
                                            {
                                                antNum |= eAntennaNo._14;
                                            }
                                            else if (readKey.Equals("15"))
                                            {
                                                antNum |= eAntennaNo._15;
                                            }
                                            else if (readKey.Equals("16"))
                                            {
                                                antNum |= eAntennaNo._16;
                                            }
                                            else if (readKey.Equals("17"))
                                            {
                                                antNum |= eAntennaNo._17;
                                            }
                                            else if (readKey.Equals("18"))
                                            {
                                                antNum |= eAntennaNo._18;
                                            }
                                            else if (readKey.Equals("19"))
                                            {
                                                antNum |= eAntennaNo._19;
                                            }
                                            else if (readKey.Equals("20"))
                                            {
                                                antNum |= eAntennaNo._20;
                                            }
                                            else if (readKey.Equals("21"))
                                            {
                                                antNum |= eAntennaNo._21;
                                            }
                                            else if (readKey.Equals("22"))
                                            {
                                                antNum |= eAntennaNo._22;
                                            }
                                            else if (readKey.Equals("23"))
                                            {
                                                antNum |= eAntennaNo._23;
                                            }
                                            else if (readKey.Equals("24"))
                                            {
                                                antNum |= eAntennaNo._24;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = 0;
                                            }
                                        }

                                        eReadType readType;
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input sEPC:");
                                        String sEPC = Console.ReadLine();

                                        Console.WriteLine("Please input matchWordStartIndex:");
                                        int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                        if (My_Reader._Tag6C.GetEPC_TID_MatchEPC(ConnID, antNum, readType, sEPC, matchWordStartIndex) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("13"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else if (readKey.Equals("9"))
                                            {
                                                antNum |= eAntennaNo._9;
                                            }
                                            else if (readKey.Equals("10"))
                                            {
                                                antNum |= eAntennaNo._10;
                                            }
                                            else if (readKey.Equals("11"))
                                            {
                                                antNum |= eAntennaNo._11;
                                            }
                                            else if (readKey.Equals("12"))
                                            {
                                                antNum |= eAntennaNo._12;
                                            }
                                            else if (readKey.Equals("13"))
                                            {
                                                antNum |= eAntennaNo._13;
                                            }
                                            else if (readKey.Equals("14"))
                                            {
                                                antNum |= eAntennaNo._14;
                                            }
                                            else if (readKey.Equals("15"))
                                            {
                                                antNum |= eAntennaNo._15;
                                            }
                                            else if (readKey.Equals("16"))
                                            {
                                                antNum |= eAntennaNo._16;
                                            }
                                            else if (readKey.Equals("17"))
                                            {
                                                antNum |= eAntennaNo._17;
                                            }
                                            else if (readKey.Equals("18"))
                                            {
                                                antNum |= eAntennaNo._18;
                                            }
                                            else if (readKey.Equals("19"))
                                            {
                                                antNum |= eAntennaNo._19;
                                            }
                                            else if (readKey.Equals("20"))
                                            {
                                                antNum |= eAntennaNo._20;
                                            }
                                            else if (readKey.Equals("21"))
                                            {
                                                antNum |= eAntennaNo._21;
                                            }
                                            else if (readKey.Equals("22"))
                                            {
                                                antNum |= eAntennaNo._22;
                                            }
                                            else if (readKey.Equals("23"))
                                            {
                                                antNum |= eAntennaNo._23;
                                            }
                                            else if (readKey.Equals("24"))
                                            {
                                                antNum |= eAntennaNo._24;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = 0;
                                            }
                                        }

                                        eReadType readType;
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input sEPC:");
                                        String sEPC = Console.ReadLine();

                                        Console.WriteLine("Please input matchWordStartIndex:");
                                        int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input accessPassword:");
                                        String accessPassword = Console.ReadLine();

                                        if (My_Reader._Tag6C.GetEPC_TID_MatchEPC(ConnID, antNum, readType, sEPC, matchWordStartIndex, accessPassword) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("14"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else if (readKey.Equals("9"))
                                            {
                                                antNum |= eAntennaNo._9;
                                            }
                                            else if (readKey.Equals("10"))
                                            {
                                                antNum |= eAntennaNo._10;
                                            }
                                            else if (readKey.Equals("11"))
                                            {
                                                antNum |= eAntennaNo._11;
                                            }
                                            else if (readKey.Equals("12"))
                                            {
                                                antNum |= eAntennaNo._12;
                                            }
                                            else if (readKey.Equals("13"))
                                            {
                                                antNum |= eAntennaNo._13;
                                            }
                                            else if (readKey.Equals("14"))
                                            {
                                                antNum |= eAntennaNo._14;
                                            }
                                            else if (readKey.Equals("15"))
                                            {
                                                antNum |= eAntennaNo._15;
                                            }
                                            else if (readKey.Equals("16"))
                                            {
                                                antNum |= eAntennaNo._16;
                                            }
                                            else if (readKey.Equals("17"))
                                            {
                                                antNum |= eAntennaNo._17;
                                            }
                                            else if (readKey.Equals("18"))
                                            {
                                                antNum |= eAntennaNo._18;
                                            }
                                            else if (readKey.Equals("19"))
                                            {
                                                antNum |= eAntennaNo._19;
                                            }
                                            else if (readKey.Equals("20"))
                                            {
                                                antNum |= eAntennaNo._20;
                                            }
                                            else if (readKey.Equals("21"))
                                            {
                                                antNum |= eAntennaNo._21;
                                            }
                                            else if (readKey.Equals("22"))
                                            {
                                                antNum |= eAntennaNo._22;
                                            }
                                            else if (readKey.Equals("23"))
                                            {
                                                antNum |= eAntennaNo._23;
                                            }
                                            else if (readKey.Equals("24"))
                                            {
                                                antNum |= eAntennaNo._24;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = 0;
                                            }
                                        }

                                        eReadType readType;
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input sTID:");
                                        String sTID = Console.ReadLine();

                                        if (My_Reader._Tag6C.GetEPC_TID_MatchTID(ConnID, antNum, readType, sTID) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("15"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else if (readKey.Equals("9"))
                                            {
                                                antNum |= eAntennaNo._9;
                                            }
                                            else if (readKey.Equals("10"))
                                            {
                                                antNum |= eAntennaNo._10;
                                            }
                                            else if (readKey.Equals("11"))
                                            {
                                                antNum |= eAntennaNo._11;
                                            }
                                            else if (readKey.Equals("12"))
                                            {
                                                antNum |= eAntennaNo._12;
                                            }
                                            else if (readKey.Equals("13"))
                                            {
                                                antNum |= eAntennaNo._13;
                                            }
                                            else if (readKey.Equals("14"))
                                            {
                                                antNum |= eAntennaNo._14;
                                            }
                                            else if (readKey.Equals("15"))
                                            {
                                                antNum |= eAntennaNo._15;
                                            }
                                            else if (readKey.Equals("16"))
                                            {
                                                antNum |= eAntennaNo._16;
                                            }
                                            else if (readKey.Equals("17"))
                                            {
                                                antNum |= eAntennaNo._17;
                                            }
                                            else if (readKey.Equals("18"))
                                            {
                                                antNum |= eAntennaNo._18;
                                            }
                                            else if (readKey.Equals("19"))
                                            {
                                                antNum |= eAntennaNo._19;
                                            }
                                            else if (readKey.Equals("20"))
                                            {
                                                antNum |= eAntennaNo._20;
                                            }
                                            else if (readKey.Equals("21"))
                                            {
                                                antNum |= eAntennaNo._21;
                                            }
                                            else if (readKey.Equals("22"))
                                            {
                                                antNum |= eAntennaNo._22;
                                            }
                                            else if (readKey.Equals("23"))
                                            {
                                                antNum |= eAntennaNo._23;
                                            }
                                            else if (readKey.Equals("24"))
                                            {
                                                antNum |= eAntennaNo._24;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = 0;
                                            }
                                        }

                                        eReadType readType;
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input sTID:");
                                        String sTID = Console.ReadLine();

                                        Console.WriteLine("Please input matchWordStartIndex:");
                                        int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                        if (My_Reader._Tag6C.GetEPC_TID_MatchTID(ConnID, antNum, readType, sTID, matchWordStartIndex) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }

                                    }
                                    else if (readKey.Equals("16"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else if (readKey.Equals("9"))
                                            {
                                                antNum |= eAntennaNo._9;
                                            }
                                            else if (readKey.Equals("10"))
                                            {
                                                antNum |= eAntennaNo._10;
                                            }
                                            else if (readKey.Equals("11"))
                                            {
                                                antNum |= eAntennaNo._11;
                                            }
                                            else if (readKey.Equals("12"))
                                            {
                                                antNum |= eAntennaNo._12;
                                            }
                                            else if (readKey.Equals("13"))
                                            {
                                                antNum |= eAntennaNo._13;
                                            }
                                            else if (readKey.Equals("14"))
                                            {
                                                antNum |= eAntennaNo._14;
                                            }
                                            else if (readKey.Equals("15"))
                                            {
                                                antNum |= eAntennaNo._15;
                                            }
                                            else if (readKey.Equals("16"))
                                            {
                                                antNum |= eAntennaNo._16;
                                            }
                                            else if (readKey.Equals("17"))
                                            {
                                                antNum |= eAntennaNo._17;
                                            }
                                            else if (readKey.Equals("18"))
                                            {
                                                antNum |= eAntennaNo._18;
                                            }
                                            else if (readKey.Equals("19"))
                                            {
                                                antNum |= eAntennaNo._19;
                                            }
                                            else if (readKey.Equals("20"))
                                            {
                                                antNum |= eAntennaNo._20;
                                            }
                                            else if (readKey.Equals("21"))
                                            {
                                                antNum |= eAntennaNo._21;
                                            }
                                            else if (readKey.Equals("22"))
                                            {
                                                antNum |= eAntennaNo._22;
                                            }
                                            else if (readKey.Equals("23"))
                                            {
                                                antNum |= eAntennaNo._23;
                                            }
                                            else if (readKey.Equals("24"))
                                            {
                                                antNum |= eAntennaNo._24;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = 0;
                                            }
                                        }

                                        eReadType readType;
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input sTID:");
                                        String sTID = Console.ReadLine();

                                        Console.WriteLine("Please input matchWordStartIndex:");
                                        int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input accessPassword:");
                                        String accessPassword = Console.ReadLine();

                                        if (My_Reader._Tag6C.GetEPC_TID_MatchTID(ConnID, antNum, readType, sTID, matchWordStartIndex, accessPassword) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("17"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else if (readKey.Equals("9"))
                                            {
                                                antNum |= eAntennaNo._9;
                                            }
                                            else if (readKey.Equals("10"))
                                            {
                                                antNum |= eAntennaNo._10;
                                            }
                                            else if (readKey.Equals("11"))
                                            {
                                                antNum |= eAntennaNo._11;
                                            }
                                            else if (readKey.Equals("12"))
                                            {
                                                antNum |= eAntennaNo._12;
                                            }
                                            else if (readKey.Equals("13"))
                                            {
                                                antNum |= eAntennaNo._13;
                                            }
                                            else if (readKey.Equals("14"))
                                            {
                                                antNum |= eAntennaNo._14;
                                            }
                                            else if (readKey.Equals("15"))
                                            {
                                                antNum |= eAntennaNo._15;
                                            }
                                            else if (readKey.Equals("16"))
                                            {
                                                antNum |= eAntennaNo._16;
                                            }
                                            else if (readKey.Equals("17"))
                                            {
                                                antNum |= eAntennaNo._17;
                                            }
                                            else if (readKey.Equals("18"))
                                            {
                                                antNum |= eAntennaNo._18;
                                            }
                                            else if (readKey.Equals("19"))
                                            {
                                                antNum |= eAntennaNo._19;
                                            }
                                            else if (readKey.Equals("20"))
                                            {
                                                antNum |= eAntennaNo._20;
                                            }
                                            else if (readKey.Equals("21"))
                                            {
                                                antNum |= eAntennaNo._21;
                                            }
                                            else if (readKey.Equals("22"))
                                            {
                                                antNum |= eAntennaNo._22;
                                            }
                                            else if (readKey.Equals("23"))
                                            {
                                                antNum |= eAntennaNo._23;
                                            }
                                            else if (readKey.Equals("24"))
                                            {
                                                antNum |= eAntennaNo._24;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = 0;
                                            }
                                        }

                                        eReadType readType;
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input readStart:");
                                        int readStart = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input readLen:");
                                        int readLen = Convert.ToInt32(Console.ReadLine());

                                        if (My_Reader._Tag6C.GetEPC_TID_UserData(ConnID, antNum, readType, readStart, readLen) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("18"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else if (readKey.Equals("9"))
                                            {
                                                antNum |= eAntennaNo._9;
                                            }
                                            else if (readKey.Equals("10"))
                                            {
                                                antNum |= eAntennaNo._10;
                                            }
                                            else if (readKey.Equals("11"))
                                            {
                                                antNum |= eAntennaNo._11;
                                            }
                                            else if (readKey.Equals("12"))
                                            {
                                                antNum |= eAntennaNo._12;
                                            }
                                            else if (readKey.Equals("13"))
                                            {
                                                antNum |= eAntennaNo._13;
                                            }
                                            else if (readKey.Equals("14"))
                                            {
                                                antNum |= eAntennaNo._14;
                                            }
                                            else if (readKey.Equals("15"))
                                            {
                                                antNum |= eAntennaNo._15;
                                            }
                                            else if (readKey.Equals("16"))
                                            {
                                                antNum |= eAntennaNo._16;
                                            }
                                            else if (readKey.Equals("17"))
                                            {
                                                antNum |= eAntennaNo._17;
                                            }
                                            else if (readKey.Equals("18"))
                                            {
                                                antNum |= eAntennaNo._18;
                                            }
                                            else if (readKey.Equals("19"))
                                            {
                                                antNum |= eAntennaNo._19;
                                            }
                                            else if (readKey.Equals("20"))
                                            {
                                                antNum |= eAntennaNo._20;
                                            }
                                            else if (readKey.Equals("21"))
                                            {
                                                antNum |= eAntennaNo._21;
                                            }
                                            else if (readKey.Equals("22"))
                                            {
                                                antNum |= eAntennaNo._22;
                                            }
                                            else if (readKey.Equals("23"))
                                            {
                                                antNum |= eAntennaNo._23;
                                            }
                                            else if (readKey.Equals("24"))
                                            {
                                                antNum |= eAntennaNo._24;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = 0;
                                            }
                                        }

                                        eReadType readType;
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input readStart:");
                                        int readStart = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input readLen:");
                                        int readLen = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input accessPassword:");
                                        String accessPassword = Console.ReadLine();

                                        if (My_Reader._Tag6C.GetEPC_TID_UserData(ConnID, antNum, readType, readStart, readLen, accessPassword) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("19"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else if (readKey.Equals("9"))
                                            {
                                                antNum |= eAntennaNo._9;
                                            }
                                            else if (readKey.Equals("10"))
                                            {
                                                antNum |= eAntennaNo._10;
                                            }
                                            else if (readKey.Equals("11"))
                                            {
                                                antNum |= eAntennaNo._11;
                                            }
                                            else if (readKey.Equals("12"))
                                            {
                                                antNum |= eAntennaNo._12;
                                            }
                                            else if (readKey.Equals("13"))
                                            {
                                                antNum |= eAntennaNo._13;
                                            }
                                            else if (readKey.Equals("14"))
                                            {
                                                antNum |= eAntennaNo._14;
                                            }
                                            else if (readKey.Equals("15"))
                                            {
                                                antNum |= eAntennaNo._15;
                                            }
                                            else if (readKey.Equals("16"))
                                            {
                                                antNum |= eAntennaNo._16;
                                            }
                                            else if (readKey.Equals("17"))
                                            {
                                                antNum |= eAntennaNo._17;
                                            }
                                            else if (readKey.Equals("18"))
                                            {
                                                antNum |= eAntennaNo._18;
                                            }
                                            else if (readKey.Equals("19"))
                                            {
                                                antNum |= eAntennaNo._19;
                                            }
                                            else if (readKey.Equals("20"))
                                            {
                                                antNum |= eAntennaNo._20;
                                            }
                                            else if (readKey.Equals("21"))
                                            {
                                                antNum |= eAntennaNo._21;
                                            }
                                            else if (readKey.Equals("22"))
                                            {
                                                antNum |= eAntennaNo._22;
                                            }
                                            else if (readKey.Equals("23"))
                                            {
                                                antNum |= eAntennaNo._23;
                                            }
                                            else if (readKey.Equals("24"))
                                            {
                                                antNum |= eAntennaNo._24;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = 0;
                                            }
                                        }

                                        eReadType readType;
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input readStart:");
                                        int readStart = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input readLen:");
                                        int readLen = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input sEPC:");
                                        String sEPC = Console.ReadLine();

                                        if (My_Reader._Tag6C.GetEPC_TID_UserData_MatchEPC(ConnID, antNum, readType, readStart, readLen, sEPC) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("20"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else if (readKey.Equals("9"))
                                            {
                                                antNum |= eAntennaNo._9;
                                            }
                                            else if (readKey.Equals("10"))
                                            {
                                                antNum |= eAntennaNo._10;
                                            }
                                            else if (readKey.Equals("11"))
                                            {
                                                antNum |= eAntennaNo._11;
                                            }
                                            else if (readKey.Equals("12"))
                                            {
                                                antNum |= eAntennaNo._12;
                                            }
                                            else if (readKey.Equals("13"))
                                            {
                                                antNum |= eAntennaNo._13;
                                            }
                                            else if (readKey.Equals("14"))
                                            {
                                                antNum |= eAntennaNo._14;
                                            }
                                            else if (readKey.Equals("15"))
                                            {
                                                antNum |= eAntennaNo._15;
                                            }
                                            else if (readKey.Equals("16"))
                                            {
                                                antNum |= eAntennaNo._16;
                                            }
                                            else if (readKey.Equals("17"))
                                            {
                                                antNum |= eAntennaNo._17;
                                            }
                                            else if (readKey.Equals("18"))
                                            {
                                                antNum |= eAntennaNo._18;
                                            }
                                            else if (readKey.Equals("19"))
                                            {
                                                antNum |= eAntennaNo._19;
                                            }
                                            else if (readKey.Equals("20"))
                                            {
                                                antNum |= eAntennaNo._20;
                                            }
                                            else if (readKey.Equals("21"))
                                            {
                                                antNum |= eAntennaNo._21;
                                            }
                                            else if (readKey.Equals("22"))
                                            {
                                                antNum |= eAntennaNo._22;
                                            }
                                            else if (readKey.Equals("23"))
                                            {
                                                antNum |= eAntennaNo._23;
                                            }
                                            else if (readKey.Equals("24"))
                                            {
                                                antNum |= eAntennaNo._24;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = 0;
                                            }
                                        }

                                        eReadType readType;
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input readStart:");
                                        int readStart = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input readLen:");
                                        int readLen = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input sEPC:");
                                        String sEPC = Console.ReadLine();

                                        Console.WriteLine("Please input matchWordStartIndex:");
                                        int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                        if (My_Reader._Tag6C.GetEPC_TID_UserData_MatchEPC(ConnID, antNum, readType, readStart, readLen, sEPC, matchWordStartIndex) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("21"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else if (readKey.Equals("9"))
                                            {
                                                antNum |= eAntennaNo._9;
                                            }
                                            else if (readKey.Equals("10"))
                                            {
                                                antNum |= eAntennaNo._10;
                                            }
                                            else if (readKey.Equals("11"))
                                            {
                                                antNum |= eAntennaNo._11;
                                            }
                                            else if (readKey.Equals("12"))
                                            {
                                                antNum |= eAntennaNo._12;
                                            }
                                            else if (readKey.Equals("13"))
                                            {
                                                antNum |= eAntennaNo._13;
                                            }
                                            else if (readKey.Equals("14"))
                                            {
                                                antNum |= eAntennaNo._14;
                                            }
                                            else if (readKey.Equals("15"))
                                            {
                                                antNum |= eAntennaNo._15;
                                            }
                                            else if (readKey.Equals("16"))
                                            {
                                                antNum |= eAntennaNo._16;
                                            }
                                            else if (readKey.Equals("17"))
                                            {
                                                antNum |= eAntennaNo._17;
                                            }
                                            else if (readKey.Equals("18"))
                                            {
                                                antNum |= eAntennaNo._18;
                                            }
                                            else if (readKey.Equals("19"))
                                            {
                                                antNum |= eAntennaNo._19;
                                            }
                                            else if (readKey.Equals("20"))
                                            {
                                                antNum |= eAntennaNo._20;
                                            }
                                            else if (readKey.Equals("21"))
                                            {
                                                antNum |= eAntennaNo._21;
                                            }
                                            else if (readKey.Equals("22"))
                                            {
                                                antNum |= eAntennaNo._22;
                                            }
                                            else if (readKey.Equals("23"))
                                            {
                                                antNum |= eAntennaNo._23;
                                            }
                                            else if (readKey.Equals("24"))
                                            {
                                                antNum |= eAntennaNo._24;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = 0;
                                            }
                                        }

                                        eReadType readType;
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input readStart:");
                                        int readStart = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input readLen:");
                                        int readLen = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input sEPC:");
                                        String sEPC = Console.ReadLine();

                                        Console.WriteLine("Please input matchWordStartIndex:");
                                        int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input accessPassword:");
                                        String accessPassword = Console.ReadLine();

                                        if (My_Reader._Tag6C.GetEPC_TID_UserData_MatchEPC(ConnID, antNum, readType, readStart, readLen, sEPC, matchWordStartIndex, accessPassword) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("22"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else if (readKey.Equals("9"))
                                            {
                                                antNum |= eAntennaNo._9;
                                            }
                                            else if (readKey.Equals("10"))
                                            {
                                                antNum |= eAntennaNo._10;
                                            }
                                            else if (readKey.Equals("11"))
                                            {
                                                antNum |= eAntennaNo._11;
                                            }
                                            else if (readKey.Equals("12"))
                                            {
                                                antNum |= eAntennaNo._12;
                                            }
                                            else if (readKey.Equals("13"))
                                            {
                                                antNum |= eAntennaNo._13;
                                            }
                                            else if (readKey.Equals("14"))
                                            {
                                                antNum |= eAntennaNo._14;
                                            }
                                            else if (readKey.Equals("15"))
                                            {
                                                antNum |= eAntennaNo._15;
                                            }
                                            else if (readKey.Equals("16"))
                                            {
                                                antNum |= eAntennaNo._16;
                                            }
                                            else if (readKey.Equals("17"))
                                            {
                                                antNum |= eAntennaNo._17;
                                            }
                                            else if (readKey.Equals("18"))
                                            {
                                                antNum |= eAntennaNo._18;
                                            }
                                            else if (readKey.Equals("19"))
                                            {
                                                antNum |= eAntennaNo._19;
                                            }
                                            else if (readKey.Equals("20"))
                                            {
                                                antNum |= eAntennaNo._20;
                                            }
                                            else if (readKey.Equals("21"))
                                            {
                                                antNum |= eAntennaNo._21;
                                            }
                                            else if (readKey.Equals("22"))
                                            {
                                                antNum |= eAntennaNo._22;
                                            }
                                            else if (readKey.Equals("23"))
                                            {
                                                antNum |= eAntennaNo._23;
                                            }
                                            else if (readKey.Equals("24"))
                                            {
                                                antNum |= eAntennaNo._24;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = 0;
                                            }
                                        }

                                        eReadType readType;
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input readStart:");
                                        int readStart = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input readLen:");
                                        int readLen = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input sTID:");
                                        String sTID = Console.ReadLine();

                                        if (My_Reader._Tag6C.GetEPC_TID_UserData_MatchTID(ConnID, antNum, readType, readStart, readLen, sTID) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("23"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else if (readKey.Equals("9"))
                                            {
                                                antNum |= eAntennaNo._9;
                                            }
                                            else if (readKey.Equals("10"))
                                            {
                                                antNum |= eAntennaNo._10;
                                            }
                                            else if (readKey.Equals("11"))
                                            {
                                                antNum |= eAntennaNo._11;
                                            }
                                            else if (readKey.Equals("12"))
                                            {
                                                antNum |= eAntennaNo._12;
                                            }
                                            else if (readKey.Equals("13"))
                                            {
                                                antNum |= eAntennaNo._13;
                                            }
                                            else if (readKey.Equals("14"))
                                            {
                                                antNum |= eAntennaNo._14;
                                            }
                                            else if (readKey.Equals("15"))
                                            {
                                                antNum |= eAntennaNo._15;
                                            }
                                            else if (readKey.Equals("16"))
                                            {
                                                antNum |= eAntennaNo._16;
                                            }
                                            else if (readKey.Equals("17"))
                                            {
                                                antNum |= eAntennaNo._17;
                                            }
                                            else if (readKey.Equals("18"))
                                            {
                                                antNum |= eAntennaNo._18;
                                            }
                                            else if (readKey.Equals("19"))
                                            {
                                                antNum |= eAntennaNo._19;
                                            }
                                            else if (readKey.Equals("20"))
                                            {
                                                antNum |= eAntennaNo._20;
                                            }
                                            else if (readKey.Equals("21"))
                                            {
                                                antNum |= eAntennaNo._21;
                                            }
                                            else if (readKey.Equals("22"))
                                            {
                                                antNum |= eAntennaNo._22;
                                            }
                                            else if (readKey.Equals("23"))
                                            {
                                                antNum |= eAntennaNo._23;
                                            }
                                            else if (readKey.Equals("24"))
                                            {
                                                antNum |= eAntennaNo._24;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = 0;
                                            }
                                        }

                                        eReadType readType;
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input readStart:");
                                        int readStart = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input readLen:");
                                        int readLen = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input sTID:");
                                        String sTID = Console.ReadLine();

                                        Console.WriteLine("Please input matchWordStartIndex:");
                                        int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                        if (My_Reader._Tag6C.GetEPC_TID_UserData_MatchTID(ConnID, antNum, readType, readStart, readLen, sTID, matchWordStartIndex) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("24"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else if (readKey.Equals("9"))
                                            {
                                                antNum |= eAntennaNo._9;
                                            }
                                            else if (readKey.Equals("10"))
                                            {
                                                antNum |= eAntennaNo._10;
                                            }
                                            else if (readKey.Equals("11"))
                                            {
                                                antNum |= eAntennaNo._11;
                                            }
                                            else if (readKey.Equals("12"))
                                            {
                                                antNum |= eAntennaNo._12;
                                            }
                                            else if (readKey.Equals("13"))
                                            {
                                                antNum |= eAntennaNo._13;
                                            }
                                            else if (readKey.Equals("14"))
                                            {
                                                antNum |= eAntennaNo._14;
                                            }
                                            else if (readKey.Equals("15"))
                                            {
                                                antNum |= eAntennaNo._15;
                                            }
                                            else if (readKey.Equals("16"))
                                            {
                                                antNum |= eAntennaNo._16;
                                            }
                                            else if (readKey.Equals("17"))
                                            {
                                                antNum |= eAntennaNo._17;
                                            }
                                            else if (readKey.Equals("18"))
                                            {
                                                antNum |= eAntennaNo._18;
                                            }
                                            else if (readKey.Equals("19"))
                                            {
                                                antNum |= eAntennaNo._19;
                                            }
                                            else if (readKey.Equals("20"))
                                            {
                                                antNum |= eAntennaNo._20;
                                            }
                                            else if (readKey.Equals("21"))
                                            {
                                                antNum |= eAntennaNo._21;
                                            }
                                            else if (readKey.Equals("22"))
                                            {
                                                antNum |= eAntennaNo._22;
                                            }
                                            else if (readKey.Equals("23"))
                                            {
                                                antNum |= eAntennaNo._23;
                                            }
                                            else if (readKey.Equals("24"))
                                            {
                                                antNum |= eAntennaNo._24;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = 0;
                                            }
                                        }

                                        eReadType readType;
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input readStart:");
                                        int readStart = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input readLen:");
                                        int readLen = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input sTID:");
                                        String sTID = Console.ReadLine();

                                        Console.WriteLine("Please input matchWordStartIndex:");
                                        int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input accessPassword:");
                                        String accessPassword = Console.ReadLine();

                                        if (My_Reader._Tag6C.GetEPC_TID_UserData_MatchTID(ConnID, antNum, readType, readStart, readLen, sTID, matchWordStartIndex, accessPassword) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }

                                    #region Get QT Parameter

                                    else if (readKey.Equals("5"))
                                    {
                                        while (true)
                                        {
                                            Console.WriteLine("Please select method");
                                            Console.WriteLine("1.GetQtTagOption (String ConnID, eAntennaNo antNum)");
                                            Console.WriteLine("2.GetQtTagOption (String ConnID, eAntennaNo antNum, String accessPassword)");
                                            Console.WriteLine("3.GetQtTagOption_MatchEPC (String ConnID, eAntennaNo antNum, String sMatchData, Int32 matchWordStartIndex, String accessPassword)");
                                            Console.WriteLine("4.GetQtTagOption_MatchTID (String ConnID, eAntennaNo antNum, String sMatchData, Int32 matchWordStartIndex, String accessPassword)");
                                            Console.WriteLine("b.Back to Previous Menu");
                                            Console.WriteLine("q.Quit");

                                            if (readKey.Equals("1"))
                                            {
                                                eAntennaNo antNum = new eAntennaNo();
                                                while (true)
                                                {
                                                    Console.WriteLine("Please input antenna number:");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        antNum |= eAntennaNo._1;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        antNum |= eAntennaNo._2;
                                                    }
                                                    else if (readKey.Equals("3"))
                                                    {
                                                        antNum |= eAntennaNo._3;
                                                    }
                                                    else if (readKey.Equals("4"))
                                                    {
                                                        antNum |= eAntennaNo._4;
                                                    }
                                                    else if (readKey.Equals("5"))
                                                    {
                                                        antNum |= eAntennaNo._5;
                                                    }
                                                    else if (readKey.Equals("6"))
                                                    {
                                                        antNum |= eAntennaNo._6;
                                                    }
                                                    else if (readKey.Equals("7"))
                                                    {
                                                        antNum |= eAntennaNo._7;
                                                    }
                                                    else if (readKey.Equals("8"))
                                                    {
                                                        antNum |= eAntennaNo._8;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("antenna number error");
                                                        continue;
                                                    }

                                                    Console.WriteLine("Setting another antenna");
                                                    Console.WriteLine("1.Yes");
                                                    Console.WriteLine("2.No");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        continue;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Parameter error ,please reset");
                                                        antNum = new eAntennaNo();
                                                    }
                                                }

                                                Console.WriteLine(My_Reader._Tag6C.GetQtTagOption(ConnID, antNum));
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                eAntennaNo antNum = new eAntennaNo();
                                                while (true)
                                                {
                                                    Console.WriteLine("Please input antenna number:");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        antNum |= eAntennaNo._1;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        antNum |= eAntennaNo._2;
                                                    }
                                                    else if (readKey.Equals("3"))
                                                    {
                                                        antNum |= eAntennaNo._3;
                                                    }
                                                    else if (readKey.Equals("4"))
                                                    {
                                                        antNum |= eAntennaNo._4;
                                                    }
                                                    else if (readKey.Equals("5"))
                                                    {
                                                        antNum |= eAntennaNo._5;
                                                    }
                                                    else if (readKey.Equals("6"))
                                                    {
                                                        antNum |= eAntennaNo._6;
                                                    }
                                                    else if (readKey.Equals("7"))
                                                    {
                                                        antNum |= eAntennaNo._7;
                                                    }
                                                    else if (readKey.Equals("8"))
                                                    {
                                                        antNum |= eAntennaNo._8;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("antenna number error");
                                                        continue;
                                                    }

                                                    Console.WriteLine("Setting another antenna");
                                                    Console.WriteLine("1.Yes");
                                                    Console.WriteLine("2.No");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        continue;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Parameter error ,please reset");
                                                        antNum = new eAntennaNo();
                                                    }
                                                }

                                                Console.WriteLine("Please input accessPassword:");
                                                string accessPassword = Console.ReadLine();

                                                Console.WriteLine(My_Reader._Tag6C.GetQtTagOption(ConnID, antNum, accessPassword));
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                eAntennaNo antNum = new eAntennaNo();
                                                while (true)
                                                {
                                                    Console.WriteLine("Please input antenna number:");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        antNum |= eAntennaNo._1;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        antNum |= eAntennaNo._2;
                                                    }
                                                    else if (readKey.Equals("3"))
                                                    {
                                                        antNum |= eAntennaNo._3;
                                                    }
                                                    else if (readKey.Equals("4"))
                                                    {
                                                        antNum |= eAntennaNo._4;
                                                    }
                                                    else if (readKey.Equals("5"))
                                                    {
                                                        antNum |= eAntennaNo._5;
                                                    }
                                                    else if (readKey.Equals("6"))
                                                    {
                                                        antNum |= eAntennaNo._6;
                                                    }
                                                    else if (readKey.Equals("7"))
                                                    {
                                                        antNum |= eAntennaNo._7;
                                                    }
                                                    else if (readKey.Equals("8"))
                                                    {
                                                        antNum |= eAntennaNo._8;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("antenna number error");
                                                        continue;
                                                    }

                                                    Console.WriteLine("Setting another antenna");
                                                    Console.WriteLine("1.Yes");
                                                    Console.WriteLine("2.No");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        continue;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Parameter error ,please reset");
                                                        antNum = new eAntennaNo();
                                                    }
                                                }

                                                Console.WriteLine("Please input sMatchData:");
                                                string sMatchData = Console.ReadLine();

                                                Console.WriteLine("Please input matchWordStartIndex:");
                                                int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                                Console.WriteLine("Please input accessPassword:");
                                                string accessPassword = Console.ReadLine();

                                                Console.WriteLine(My_Reader._Tag6C.GetQtTagOption_MatchEPC(ConnID, antNum, sMatchData, matchWordStartIndex, accessPassword));
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                eAntennaNo antNum = new eAntennaNo();
                                                while (true)
                                                {
                                                    Console.WriteLine("Please input antenna number:");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        antNum |= eAntennaNo._1;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        antNum |= eAntennaNo._2;
                                                    }
                                                    else if (readKey.Equals("3"))
                                                    {
                                                        antNum |= eAntennaNo._3;
                                                    }
                                                    else if (readKey.Equals("4"))
                                                    {
                                                        antNum |= eAntennaNo._4;
                                                    }
                                                    else if (readKey.Equals("5"))
                                                    {
                                                        antNum |= eAntennaNo._5;
                                                    }
                                                    else if (readKey.Equals("6"))
                                                    {
                                                        antNum |= eAntennaNo._6;
                                                    }
                                                    else if (readKey.Equals("7"))
                                                    {
                                                        antNum |= eAntennaNo._7;
                                                    }
                                                    else if (readKey.Equals("8"))
                                                    {
                                                        antNum |= eAntennaNo._8;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("antenna number error");
                                                        continue;
                                                    }

                                                    Console.WriteLine("Setting another antenna");
                                                    Console.WriteLine("1.Yes");
                                                    Console.WriteLine("2.No");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        continue;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Parameter error ,please reset");
                                                        antNum = new eAntennaNo();
                                                    }
                                                }

                                                Console.WriteLine("Please input sMatchData:");
                                                string sMatchData = Console.ReadLine();

                                                Console.WriteLine("Please input matchWordStartIndex:");
                                                int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                                Console.WriteLine("Please input accessPassword:");
                                                string accessPassword = Console.ReadLine();

                                                Console.WriteLine(My_Reader._Tag6C.GetQtTagOption_MatchTID(ConnID, antNum, sMatchData, matchWordStartIndex, accessPassword));

                                            }
                                            else if (readKey.Equals("b"))
                                            {
                                                break;
                                            }
                                            else if (readKey.Equals("q"))
                                            {
                                                return;
                                            }
                                        }
                                    }

                                    #endregion

                                    #region Set QT Parameter

                                    else if (readKey.Equals("6"))
                                    {
                                        while (true)
                                        {
                                            Console.WriteLine("Please select method");
                                            Console.WriteLine("1.SetQtTagOption (String ConnID, eAntennaNo antNum, bool IsNotReduceResponseDistance,bool IsPrivateMode)");
                                            Console.WriteLine("2.SetQtTagOption (String ConnID, eAntennaNo antNum, bool IsNotReduceResponseDistance, bool IsPrivateMode, String accessPassword)");
                                            Console.WriteLine("3.SetQtTagOption_MatchEPC (String ConnID, eAntennaNo antNum, String sMatchData, bool IsNotReduceResponseDistance, bool IsPrivateMode, Int32 matchWordStartIndex, String accessPassword)");
                                            Console.WriteLine("4.SetQtTagOption_MatchTID (String ConnID, eAntennaNo antNum, String sMatchData, bool IsNotReduceResponseDistance, bool IsPrivateMode, Int32 matchWordStartIndex, String accessPassword)");
                                            Console.WriteLine("b.Back to Previous Menu");
                                            Console.WriteLine("q.Quit");

                                            if (readKey.Equals("1"))
                                            {
                                                eAntennaNo antNum = new eAntennaNo();
                                                while (true)
                                                {
                                                    Console.WriteLine("Please input antenna number:");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        antNum |= eAntennaNo._1;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        antNum |= eAntennaNo._2;
                                                    }
                                                    else if (readKey.Equals("3"))
                                                    {
                                                        antNum |= eAntennaNo._3;
                                                    }
                                                    else if (readKey.Equals("4"))
                                                    {
                                                        antNum |= eAntennaNo._4;
                                                    }
                                                    else if (readKey.Equals("5"))
                                                    {
                                                        antNum |= eAntennaNo._5;
                                                    }
                                                    else if (readKey.Equals("6"))
                                                    {
                                                        antNum |= eAntennaNo._6;
                                                    }
                                                    else if (readKey.Equals("7"))
                                                    {
                                                        antNum |= eAntennaNo._7;
                                                    }
                                                    else if (readKey.Equals("8"))
                                                    {
                                                        antNum |= eAntennaNo._8;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("antenna number error");
                                                        continue;
                                                    }

                                                    Console.WriteLine("Setting another antenna");
                                                    Console.WriteLine("1.Yes");
                                                    Console.WriteLine("2.No");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        continue;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Parameter error ,please reset");
                                                        antNum = new eAntennaNo();
                                                    }
                                                }

                                                Console.WriteLine("Please select IsNotReduceResponseDistance");
                                                Console.WriteLine("1.true");
                                                Console.WriteLine("2.false");

                                                bool IsNotReduceResponseDistance;

                                                readKey = Console.ReadLine();
                                                if (readKey.Equals("1"))
                                                {
                                                    IsNotReduceResponseDistance = true;
                                                }
                                                else if (readKey.Equals("2"))
                                                {
                                                    IsNotReduceResponseDistance = false;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("IsNotReduceResponseDistance Error");
                                                    continue;
                                                }


                                                Console.WriteLine("Please select IsPrivateMode");
                                                Console.WriteLine("1.true");
                                                Console.WriteLine("2.false");

                                                bool IsPrivateMode;

                                                readKey = Console.ReadLine();
                                                if (readKey.Equals("1"))
                                                {
                                                    IsPrivateMode = true;
                                                }
                                                else if (readKey.Equals("2"))
                                                {
                                                    IsPrivateMode = false;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("IsPrivateMode Error");
                                                    continue;
                                                }

                                                Console.WriteLine(My_Reader._Tag6C.SetQtTagOption(ConnID, antNum, IsNotReduceResponseDistance, IsPrivateMode));
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                eAntennaNo antNum = new eAntennaNo();
                                                while (true)
                                                {
                                                    Console.WriteLine("Please input antenna number:");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        antNum |= eAntennaNo._1;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        antNum |= eAntennaNo._2;
                                                    }
                                                    else if (readKey.Equals("3"))
                                                    {
                                                        antNum |= eAntennaNo._3;
                                                    }
                                                    else if (readKey.Equals("4"))
                                                    {
                                                        antNum |= eAntennaNo._4;
                                                    }
                                                    else if (readKey.Equals("5"))
                                                    {
                                                        antNum |= eAntennaNo._5;
                                                    }
                                                    else if (readKey.Equals("6"))
                                                    {
                                                        antNum |= eAntennaNo._6;
                                                    }
                                                    else if (readKey.Equals("7"))
                                                    {
                                                        antNum |= eAntennaNo._7;
                                                    }
                                                    else if (readKey.Equals("8"))
                                                    {
                                                        antNum |= eAntennaNo._8;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("antenna number error");
                                                        continue;
                                                    }

                                                    Console.WriteLine("Setting another antenna");
                                                    Console.WriteLine("1.Yes");
                                                    Console.WriteLine("2.No");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        continue;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Parameter error ,please reset");
                                                        antNum = new eAntennaNo();
                                                    }
                                                }

                                                Console.WriteLine("Please select IsNotReduceResponseDistance");
                                                Console.WriteLine("1.true");
                                                Console.WriteLine("2.false");

                                                bool IsNotReduceResponseDistance;

                                                readKey = Console.ReadLine();
                                                if (readKey.Equals("1"))
                                                {
                                                    IsNotReduceResponseDistance = true;
                                                }
                                                else if (readKey.Equals("2"))
                                                {
                                                    IsNotReduceResponseDistance = false;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("IsNotReduceResponseDistance Error");
                                                    continue;
                                                }


                                                Console.WriteLine("Please select IsPrivateMode");
                                                Console.WriteLine("1.true");
                                                Console.WriteLine("2.false");

                                                bool IsPrivateMode;

                                                readKey = Console.ReadLine();
                                                if (readKey.Equals("1"))
                                                {
                                                    IsPrivateMode = true;
                                                }
                                                else if (readKey.Equals("2"))
                                                {
                                                    IsPrivateMode = false;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("IsPrivateMode Error");
                                                    continue;
                                                }

                                                Console.WriteLine("Please input accessPassword");
                                                string accessPassword = Console.ReadLine();

                                                Console.WriteLine(My_Reader._Tag6C.SetQtTagOption(ConnID, antNum, IsNotReduceResponseDistance, IsPrivateMode, accessPassword));
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                eAntennaNo antNum = new eAntennaNo();
                                                while (true)
                                                {
                                                    Console.WriteLine("Please input antenna number:");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        antNum |= eAntennaNo._1;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        antNum |= eAntennaNo._2;
                                                    }
                                                    else if (readKey.Equals("3"))
                                                    {
                                                        antNum |= eAntennaNo._3;
                                                    }
                                                    else if (readKey.Equals("4"))
                                                    {
                                                        antNum |= eAntennaNo._4;
                                                    }
                                                    else if (readKey.Equals("5"))
                                                    {
                                                        antNum |= eAntennaNo._5;
                                                    }
                                                    else if (readKey.Equals("6"))
                                                    {
                                                        antNum |= eAntennaNo._6;
                                                    }
                                                    else if (readKey.Equals("7"))
                                                    {
                                                        antNum |= eAntennaNo._7;
                                                    }
                                                    else if (readKey.Equals("8"))
                                                    {
                                                        antNum |= eAntennaNo._8;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("antenna number error");
                                                        continue;
                                                    }

                                                    Console.WriteLine("Setting another antenna");
                                                    Console.WriteLine("1.Yes");
                                                    Console.WriteLine("2.No");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        continue;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Parameter error ,please reset");
                                                        antNum = new eAntennaNo();
                                                    }
                                                }

                                                Console.WriteLine("Please input sMatchData");
                                                string sMatchData = Console.ReadLine();

                                                Console.WriteLine("Please input matchWordStartIndex");
                                                int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                                Console.WriteLine("Please select IsNotReduceResponseDistance");
                                                Console.WriteLine("1.true");
                                                Console.WriteLine("2.false");

                                                bool IsNotReduceResponseDistance;

                                                readKey = Console.ReadLine();
                                                if (readKey.Equals("1"))
                                                {
                                                    IsNotReduceResponseDistance = true;
                                                }
                                                else if (readKey.Equals("2"))
                                                {
                                                    IsNotReduceResponseDistance = false;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("IsNotReduceResponseDistance Error");
                                                    continue;
                                                }


                                                Console.WriteLine("Please select IsPrivateMode");
                                                Console.WriteLine("1.true");
                                                Console.WriteLine("2.false");

                                                bool IsPrivateMode;

                                                readKey = Console.ReadLine();
                                                if (readKey.Equals("1"))
                                                {
                                                    IsPrivateMode = true;
                                                }
                                                else if (readKey.Equals("2"))
                                                {
                                                    IsPrivateMode = false;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("IsPrivateMode Error");
                                                    continue;
                                                }

                                                Console.WriteLine("Please input accessPassword");
                                                string accessPassword = Console.ReadLine();

                                                Console.WriteLine(My_Reader._Tag6C.SetQtTagOption_MatchEPC(ConnID, antNum, sMatchData, matchWordStartIndex, IsNotReduceResponseDistance, IsPrivateMode, accessPassword));
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                eAntennaNo antNum = new eAntennaNo();
                                                while (true)
                                                {
                                                    Console.WriteLine("Please input antenna number:");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        antNum |= eAntennaNo._1;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        antNum |= eAntennaNo._2;
                                                    }
                                                    else if (readKey.Equals("3"))
                                                    {
                                                        antNum |= eAntennaNo._3;
                                                    }
                                                    else if (readKey.Equals("4"))
                                                    {
                                                        antNum |= eAntennaNo._4;
                                                    }
                                                    else if (readKey.Equals("5"))
                                                    {
                                                        antNum |= eAntennaNo._5;
                                                    }
                                                    else if (readKey.Equals("6"))
                                                    {
                                                        antNum |= eAntennaNo._6;
                                                    }
                                                    else if (readKey.Equals("7"))
                                                    {
                                                        antNum |= eAntennaNo._7;
                                                    }
                                                    else if (readKey.Equals("8"))
                                                    {
                                                        antNum |= eAntennaNo._8;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("antenna number error");
                                                        continue;
                                                    }

                                                    Console.WriteLine("Setting another antenna");
                                                    Console.WriteLine("1.Yes");
                                                    Console.WriteLine("2.No");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        continue;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Parameter error ,please reset");
                                                        antNum = new eAntennaNo();
                                                    }
                                                }

                                                Console.WriteLine("Please input sMatchData");
                                                string sMatchData = Console.ReadLine();

                                                Console.WriteLine("Please input matchWordStartIndex");
                                                int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                                Console.WriteLine("Please select IsNotReduceResponseDistance");
                                                Console.WriteLine("1.true");
                                                Console.WriteLine("2.false");

                                                bool IsNotReduceResponseDistance;

                                                readKey = Console.ReadLine();
                                                if (readKey.Equals("1"))
                                                {
                                                    IsNotReduceResponseDistance = true;
                                                }
                                                else if (readKey.Equals("2"))
                                                {
                                                    IsNotReduceResponseDistance = false;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("IsNotReduceResponseDistance Error");
                                                    continue;
                                                }


                                                Console.WriteLine("Please select IsPrivateMode");
                                                Console.WriteLine("1.true");
                                                Console.WriteLine("2.false");

                                                bool IsPrivateMode;

                                                readKey = Console.ReadLine();
                                                if (readKey.Equals("1"))
                                                {
                                                    IsPrivateMode = true;
                                                }
                                                else if (readKey.Equals("2"))
                                                {
                                                    IsPrivateMode = false;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("IsPrivateMode Error");
                                                    continue;
                                                }

                                                Console.WriteLine("Please input accessPassword");
                                                string accessPassword = Console.ReadLine();

                                                Console.WriteLine(My_Reader._Tag6C.SetQtTagOption_MatchTID(ConnID, antNum, sMatchData, matchWordStartIndex, IsNotReduceResponseDistance, IsPrivateMode, accessPassword));
                                            }
                                            else if (readKey.Equals("b"))
                                            {
                                                break;
                                            }
                                            else if (readKey.Equals("q"))
                                            {
                                                return;
                                            }
                                        }
                                    }

                                    #endregion


                                    else if (readKey.Equals("b"))
                                    {
                                        break;
                                    }
                                    else if (readKey.Equals("s"))
                                    {
                                        My_Reader._Config.Stop(ConnID);
                                    }
                                    else if (readKey.Equals("q"))
                                    {
                                        My_Reader._Config.Stop(ConnID);
                                        My_Reader.CloseConn(ConnID);
                                        return;
                                    }

                                }
                            }

                            #endregion

                    #region Write 6C Tag


                            else if (readKey.Equals("2"))
                            {


                                while (true)
                                {

                                    Console.WriteLine("Please select operation:");
                                    Console.WriteLine("1.Write EPC");
                                    Console.WriteLine("2.Write UserData");
                                    Console.WriteLine("3.Write Password");
                                    Console.WriteLine("b.Back to Previous Menu");
                                    Console.WriteLine("q.Quit");



                                    readKey = Console.ReadLine();
                                    if (readKey.Equals("1"))
                                    {
                                        while (true)
                                        {
                                            Console.WriteLine("Please select method:");
                                            Console.WriteLine("1.WriteEPC(String ConnID, eAntennaNo antNum, String sWriteData)");
                                            Console.WriteLine("2.WriteEPC_MatchEPC(String ConnID, eAntennaNo antNum, String sWriteData, String sMatchData, int matchWordStartIndex) ");
                                            Console.WriteLine("3.WriteEPC_MatchEPC(String ConnID, eAntennaNo antNum, String sWriteData, String sMatchData, int matchWordStartIndex, String accessPassword) ");
                                            Console.WriteLine("4.WriteEPC_MatchTID(String ConnID, eAntennaNo antNum, String sWriteData, String sMatchData, int matchWordStartIndex) ");
                                            Console.WriteLine("5.WriteEPC_MatchTID(String ConnID, eAntennaNo antNum, String sWriteData, String sMatchData, int matchWordStartIndex, String accessPassword) ");
                                            Console.WriteLine("b.Back to Previous Menu");
                                            Console.WriteLine("q.Quit");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                eAntennaNo antNum = new eAntennaNo();
                                                while (true)
                                                {
                                                    Console.WriteLine("Please input antenna number:");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        antNum |= eAntennaNo._1;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        antNum |= eAntennaNo._2;
                                                    }
                                                    else if (readKey.Equals("3"))
                                                    {
                                                        antNum |= eAntennaNo._3;
                                                    }
                                                    else if (readKey.Equals("4"))
                                                    {
                                                        antNum |= eAntennaNo._4;
                                                    }
                                                    else if (readKey.Equals("5"))
                                                    {
                                                        antNum |= eAntennaNo._5;
                                                    }
                                                    else if (readKey.Equals("6"))
                                                    {
                                                        antNum |= eAntennaNo._6;
                                                    }
                                                    else if (readKey.Equals("7"))
                                                    {
                                                        antNum |= eAntennaNo._7;
                                                    }
                                                    else if (readKey.Equals("8"))
                                                    {
                                                        antNum |= eAntennaNo._8;
                                                    }
                                                    else if (readKey.Equals("9"))
                                                    {
                                                        antNum |= eAntennaNo._9;
                                                    }
                                                    else if (readKey.Equals("10"))
                                                    {
                                                        antNum |= eAntennaNo._10;
                                                    }
                                                    else if (readKey.Equals("11"))
                                                    {
                                                        antNum |= eAntennaNo._11;
                                                    }
                                                    else if (readKey.Equals("12"))
                                                    {
                                                        antNum |= eAntennaNo._12;
                                                    }
                                                    else if (readKey.Equals("13"))
                                                    {
                                                        antNum |= eAntennaNo._13;
                                                    }
                                                    else if (readKey.Equals("14"))
                                                    {
                                                        antNum |= eAntennaNo._14;
                                                    }
                                                    else if (readKey.Equals("15"))
                                                    {
                                                        antNum |= eAntennaNo._15;
                                                    }
                                                    else if (readKey.Equals("16"))
                                                    {
                                                        antNum |= eAntennaNo._16;
                                                    }
                                                    else if (readKey.Equals("17"))
                                                    {
                                                        antNum |= eAntennaNo._17;
                                                    }
                                                    else if (readKey.Equals("18"))
                                                    {
                                                        antNum |= eAntennaNo._18;
                                                    }
                                                    else if (readKey.Equals("19"))
                                                    {
                                                        antNum |= eAntennaNo._19;
                                                    }
                                                    else if (readKey.Equals("20"))
                                                    {
                                                        antNum |= eAntennaNo._20;
                                                    }
                                                    else if (readKey.Equals("21"))
                                                    {
                                                        antNum |= eAntennaNo._21;
                                                    }
                                                    else if (readKey.Equals("22"))
                                                    {
                                                        antNum |= eAntennaNo._22;
                                                    }
                                                    else if (readKey.Equals("23"))
                                                    {
                                                        antNum |= eAntennaNo._23;
                                                    }
                                                    else if (readKey.Equals("24"))
                                                    {
                                                        antNum |= eAntennaNo._24;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("antenna number error");
                                                        continue;
                                                    }

                                                    Console.WriteLine("Setting another antenna");
                                                    Console.WriteLine("1.Yes");
                                                    Console.WriteLine("2.No");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        continue;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Parameter error ,please reset");
                                                        antNum = 0;
                                                    }
                                                }

                                                Console.WriteLine("Please input sWriteData:");
                                                String sWriteData = Console.ReadLine();


                                                if (My_Reader._Tag6C.WriteEPC(ConnID, antNum, sWriteData) == 0)
                                                {
                                                    Console.WriteLine("Success!");
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Failure!");
                                                }

                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                eAntennaNo antNum = new eAntennaNo();
                                                while (true)
                                                {
                                                    Console.WriteLine("Please input antenna number:");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        antNum |= eAntennaNo._1;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        antNum |= eAntennaNo._2;
                                                    }
                                                    else if (readKey.Equals("3"))
                                                    {
                                                        antNum |= eAntennaNo._3;
                                                    }
                                                    else if (readKey.Equals("4"))
                                                    {
                                                        antNum |= eAntennaNo._4;
                                                    }
                                                    else if (readKey.Equals("5"))
                                                    {
                                                        antNum |= eAntennaNo._5;
                                                    }
                                                    else if (readKey.Equals("6"))
                                                    {
                                                        antNum |= eAntennaNo._6;
                                                    }
                                                    else if (readKey.Equals("7"))
                                                    {
                                                        antNum |= eAntennaNo._7;
                                                    }
                                                    else if (readKey.Equals("8"))
                                                    {
                                                        antNum |= eAntennaNo._8;
                                                    }
                                                    else if (readKey.Equals("9"))
                                                    {
                                                        antNum |= eAntennaNo._9;
                                                    }
                                                    else if (readKey.Equals("10"))
                                                    {
                                                        antNum |= eAntennaNo._10;
                                                    }
                                                    else if (readKey.Equals("11"))
                                                    {
                                                        antNum |= eAntennaNo._11;
                                                    }
                                                    else if (readKey.Equals("12"))
                                                    {
                                                        antNum |= eAntennaNo._12;
                                                    }
                                                    else if (readKey.Equals("13"))
                                                    {
                                                        antNum |= eAntennaNo._13;
                                                    }
                                                    else if (readKey.Equals("14"))
                                                    {
                                                        antNum |= eAntennaNo._14;
                                                    }
                                                    else if (readKey.Equals("15"))
                                                    {
                                                        antNum |= eAntennaNo._15;
                                                    }
                                                    else if (readKey.Equals("16"))
                                                    {
                                                        antNum |= eAntennaNo._16;
                                                    }
                                                    else if (readKey.Equals("17"))
                                                    {
                                                        antNum |= eAntennaNo._17;
                                                    }
                                                    else if (readKey.Equals("18"))
                                                    {
                                                        antNum |= eAntennaNo._18;
                                                    }
                                                    else if (readKey.Equals("19"))
                                                    {
                                                        antNum |= eAntennaNo._19;
                                                    }
                                                    else if (readKey.Equals("20"))
                                                    {
                                                        antNum |= eAntennaNo._20;
                                                    }
                                                    else if (readKey.Equals("21"))
                                                    {
                                                        antNum |= eAntennaNo._21;
                                                    }
                                                    else if (readKey.Equals("22"))
                                                    {
                                                        antNum |= eAntennaNo._22;
                                                    }
                                                    else if (readKey.Equals("23"))
                                                    {
                                                        antNum |= eAntennaNo._23;
                                                    }
                                                    else if (readKey.Equals("24"))
                                                    {
                                                        antNum |= eAntennaNo._24;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("antenna number error");
                                                        continue;
                                                    }

                                                    Console.WriteLine("Setting another antenna");
                                                    Console.WriteLine("1.Yes");
                                                    Console.WriteLine("2.No");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        continue;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Parameter error ,please reset");
                                                        antNum = 0;
                                                    }
                                                }

                                                Console.WriteLine("Please input sWriteData:");
                                                String sWriteData = Console.ReadLine();

                                                Console.WriteLine("Please input sMatchData:");
                                                String sMatchData = Console.ReadLine();

                                                Console.WriteLine("Please input matchWordStartIndex:");
                                                int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                                if (My_Reader._Tag6C.WriteEPC_MatchEPC(ConnID, antNum, sWriteData, sMatchData, matchWordStartIndex) == 0)
                                                {
                                                    Console.WriteLine("Success!");
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Failure!");
                                                }
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                eAntennaNo antNum = new eAntennaNo();
                                                while (true)
                                                {
                                                    Console.WriteLine("Please input antenna number:");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        antNum |= eAntennaNo._1;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        antNum |= eAntennaNo._2;
                                                    }
                                                    else if (readKey.Equals("3"))
                                                    {
                                                        antNum |= eAntennaNo._3;
                                                    }
                                                    else if (readKey.Equals("4"))
                                                    {
                                                        antNum |= eAntennaNo._4;
                                                    }
                                                    else if (readKey.Equals("5"))
                                                    {
                                                        antNum |= eAntennaNo._5;
                                                    }
                                                    else if (readKey.Equals("6"))
                                                    {
                                                        antNum |= eAntennaNo._6;
                                                    }
                                                    else if (readKey.Equals("7"))
                                                    {
                                                        antNum |= eAntennaNo._7;
                                                    }
                                                    else if (readKey.Equals("8"))
                                                    {
                                                        antNum |= eAntennaNo._8;
                                                    }
                                                    else if (readKey.Equals("9"))
                                                    {
                                                        antNum |= eAntennaNo._9;
                                                    }
                                                    else if (readKey.Equals("10"))
                                                    {
                                                        antNum |= eAntennaNo._10;
                                                    }
                                                    else if (readKey.Equals("11"))
                                                    {
                                                        antNum |= eAntennaNo._11;
                                                    }
                                                    else if (readKey.Equals("12"))
                                                    {
                                                        antNum |= eAntennaNo._12;
                                                    }
                                                    else if (readKey.Equals("13"))
                                                    {
                                                        antNum |= eAntennaNo._13;
                                                    }
                                                    else if (readKey.Equals("14"))
                                                    {
                                                        antNum |= eAntennaNo._14;
                                                    }
                                                    else if (readKey.Equals("15"))
                                                    {
                                                        antNum |= eAntennaNo._15;
                                                    }
                                                    else if (readKey.Equals("16"))
                                                    {
                                                        antNum |= eAntennaNo._16;
                                                    }
                                                    else if (readKey.Equals("17"))
                                                    {
                                                        antNum |= eAntennaNo._17;
                                                    }
                                                    else if (readKey.Equals("18"))
                                                    {
                                                        antNum |= eAntennaNo._18;
                                                    }
                                                    else if (readKey.Equals("19"))
                                                    {
                                                        antNum |= eAntennaNo._19;
                                                    }
                                                    else if (readKey.Equals("20"))
                                                    {
                                                        antNum |= eAntennaNo._20;
                                                    }
                                                    else if (readKey.Equals("21"))
                                                    {
                                                        antNum |= eAntennaNo._21;
                                                    }
                                                    else if (readKey.Equals("22"))
                                                    {
                                                        antNum |= eAntennaNo._22;
                                                    }
                                                    else if (readKey.Equals("23"))
                                                    {
                                                        antNum |= eAntennaNo._23;
                                                    }
                                                    else if (readKey.Equals("24"))
                                                    {
                                                        antNum |= eAntennaNo._24;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("antenna number error");
                                                        continue;
                                                    }

                                                    Console.WriteLine("Setting another antenna");
                                                    Console.WriteLine("1.Yes");
                                                    Console.WriteLine("2.No");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        continue;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Parameter error ,please reset");
                                                        antNum = 0;
                                                    }
                                                }

                                                Console.WriteLine("Please input sWriteData:");
                                                String sWriteData = Console.ReadLine();

                                                Console.WriteLine("Please input sMatchData:");
                                                String sMatchData = Console.ReadLine();

                                                Console.WriteLine("Please input matchWordStartIndex:");
                                                int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                                Console.WriteLine("Please input accessPassword:");
                                                String accessPassword = Console.ReadLine();

                                                if (My_Reader._Tag6C.WriteEPC_MatchEPC(ConnID, antNum, sWriteData, sMatchData, matchWordStartIndex, accessPassword) == 0)
                                                {
                                                    Console.WriteLine("Success!");
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Failure!");
                                                }
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                eAntennaNo antNum = new eAntennaNo();
                                                while (true)
                                                {
                                                    Console.WriteLine("Please input antenna number:");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        antNum |= eAntennaNo._1;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        antNum |= eAntennaNo._2;
                                                    }
                                                    else if (readKey.Equals("3"))
                                                    {
                                                        antNum |= eAntennaNo._3;
                                                    }
                                                    else if (readKey.Equals("4"))
                                                    {
                                                        antNum |= eAntennaNo._4;
                                                    }
                                                    else if (readKey.Equals("5"))
                                                    {
                                                        antNum |= eAntennaNo._5;
                                                    }
                                                    else if (readKey.Equals("6"))
                                                    {
                                                        antNum |= eAntennaNo._6;
                                                    }
                                                    else if (readKey.Equals("7"))
                                                    {
                                                        antNum |= eAntennaNo._7;
                                                    }
                                                    else if (readKey.Equals("8"))
                                                    {
                                                        antNum |= eAntennaNo._8;
                                                    }
                                                    else if (readKey.Equals("9"))
                                                    {
                                                        antNum |= eAntennaNo._9;
                                                    }
                                                    else if (readKey.Equals("10"))
                                                    {
                                                        antNum |= eAntennaNo._10;
                                                    }
                                                    else if (readKey.Equals("11"))
                                                    {
                                                        antNum |= eAntennaNo._11;
                                                    }
                                                    else if (readKey.Equals("12"))
                                                    {
                                                        antNum |= eAntennaNo._12;
                                                    }
                                                    else if (readKey.Equals("13"))
                                                    {
                                                        antNum |= eAntennaNo._13;
                                                    }
                                                    else if (readKey.Equals("14"))
                                                    {
                                                        antNum |= eAntennaNo._14;
                                                    }
                                                    else if (readKey.Equals("15"))
                                                    {
                                                        antNum |= eAntennaNo._15;
                                                    }
                                                    else if (readKey.Equals("16"))
                                                    {
                                                        antNum |= eAntennaNo._16;
                                                    }
                                                    else if (readKey.Equals("17"))
                                                    {
                                                        antNum |= eAntennaNo._17;
                                                    }
                                                    else if (readKey.Equals("18"))
                                                    {
                                                        antNum |= eAntennaNo._18;
                                                    }
                                                    else if (readKey.Equals("19"))
                                                    {
                                                        antNum |= eAntennaNo._19;
                                                    }
                                                    else if (readKey.Equals("20"))
                                                    {
                                                        antNum |= eAntennaNo._20;
                                                    }
                                                    else if (readKey.Equals("21"))
                                                    {
                                                        antNum |= eAntennaNo._21;
                                                    }
                                                    else if (readKey.Equals("22"))
                                                    {
                                                        antNum |= eAntennaNo._22;
                                                    }
                                                    else if (readKey.Equals("23"))
                                                    {
                                                        antNum |= eAntennaNo._23;
                                                    }
                                                    else if (readKey.Equals("24"))
                                                    {
                                                        antNum |= eAntennaNo._24;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("antenna number error");
                                                        continue;
                                                    }

                                                    Console.WriteLine("Setting another antenna");
                                                    Console.WriteLine("1.Yes");
                                                    Console.WriteLine("2.No");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        continue;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Parameter error ,please reset");
                                                        antNum = 0;
                                                    }
                                                }

                                                Console.WriteLine("Please input sWriteData:");
                                                String sWriteData = Console.ReadLine();

                                                Console.WriteLine("Please input sMatchData:");
                                                String sMatchData = Console.ReadLine();

                                                Console.WriteLine("Please input matchWordStartIndex:");
                                                int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                                if (My_Reader._Tag6C.WriteEPC_MatchTID(ConnID, antNum, sWriteData, sMatchData, matchWordStartIndex) == 0)
                                                {
                                                    Console.WriteLine("Success!");
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Failure!");
                                                }
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                eAntennaNo antNum = new eAntennaNo();
                                                while (true)
                                                {
                                                    Console.WriteLine("Please input antenna number:");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        antNum |= eAntennaNo._1;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        antNum |= eAntennaNo._2;
                                                    }
                                                    else if (readKey.Equals("3"))
                                                    {
                                                        antNum |= eAntennaNo._3;
                                                    }
                                                    else if (readKey.Equals("4"))
                                                    {
                                                        antNum |= eAntennaNo._4;
                                                    }
                                                    else if (readKey.Equals("5"))
                                                    {
                                                        antNum |= eAntennaNo._5;
                                                    }
                                                    else if (readKey.Equals("6"))
                                                    {
                                                        antNum |= eAntennaNo._6;
                                                    }
                                                    else if (readKey.Equals("7"))
                                                    {
                                                        antNum |= eAntennaNo._7;
                                                    }
                                                    else if (readKey.Equals("8"))
                                                    {
                                                        antNum |= eAntennaNo._8;
                                                    }
                                                    else if (readKey.Equals("9"))
                                                    {
                                                        antNum |= eAntennaNo._9;
                                                    }
                                                    else if (readKey.Equals("10"))
                                                    {
                                                        antNum |= eAntennaNo._10;
                                                    }
                                                    else if (readKey.Equals("11"))
                                                    {
                                                        antNum |= eAntennaNo._11;
                                                    }
                                                    else if (readKey.Equals("12"))
                                                    {
                                                        antNum |= eAntennaNo._12;
                                                    }
                                                    else if (readKey.Equals("13"))
                                                    {
                                                        antNum |= eAntennaNo._13;
                                                    }
                                                    else if (readKey.Equals("14"))
                                                    {
                                                        antNum |= eAntennaNo._14;
                                                    }
                                                    else if (readKey.Equals("15"))
                                                    {
                                                        antNum |= eAntennaNo._15;
                                                    }
                                                    else if (readKey.Equals("16"))
                                                    {
                                                        antNum |= eAntennaNo._16;
                                                    }
                                                    else if (readKey.Equals("17"))
                                                    {
                                                        antNum |= eAntennaNo._17;
                                                    }
                                                    else if (readKey.Equals("18"))
                                                    {
                                                        antNum |= eAntennaNo._18;
                                                    }
                                                    else if (readKey.Equals("19"))
                                                    {
                                                        antNum |= eAntennaNo._19;
                                                    }
                                                    else if (readKey.Equals("20"))
                                                    {
                                                        antNum |= eAntennaNo._20;
                                                    }
                                                    else if (readKey.Equals("21"))
                                                    {
                                                        antNum |= eAntennaNo._21;
                                                    }
                                                    else if (readKey.Equals("22"))
                                                    {
                                                        antNum |= eAntennaNo._22;
                                                    }
                                                    else if (readKey.Equals("23"))
                                                    {
                                                        antNum |= eAntennaNo._23;
                                                    }
                                                    else if (readKey.Equals("24"))
                                                    {
                                                        antNum |= eAntennaNo._24;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("antenna number error");
                                                        continue;
                                                    }

                                                    Console.WriteLine("Setting another antenna");
                                                    Console.WriteLine("1.Yes");
                                                    Console.WriteLine("2.No");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        continue;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Parameter error ,please reset");
                                                        antNum = 0;
                                                    }
                                                }

                                                Console.WriteLine("Please input sWriteData:");
                                                String sWriteData = Console.ReadLine();

                                                Console.WriteLine("Please input sMatchData:");
                                                String sMatchData = Console.ReadLine();

                                                Console.WriteLine("Please input matchWordStartIndex:");
                                                int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                                Console.WriteLine("Please input accessPassword:");
                                                String accessPassword = Console.ReadLine();

                                                if (My_Reader._Tag6C.WriteEPC_MatchTID(ConnID, antNum, sWriteData, sMatchData, matchWordStartIndex, accessPassword) == 0)
                                                {
                                                    Console.WriteLine("Success!");
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Failure!");
                                                }
                                            }
                                            else if (readKey.Equals("b"))
                                            {
                                                break;
                                            }
                                            else if (readKey.Equals("q"))
                                            {
                                                My_Reader._Config.Stop(ConnID);
                                                My_Reader.CloseConn(ConnID);
                                                return;
                                            }
                                        }
                                    }




                                    else if (readKey.Equals("2"))
                                    {
                                        while (true)
                                        {
                                            Console.WriteLine("Please select method:");
                                            Console.WriteLine("1.WriteUserData(String ConnID, eAntennaNo antNum, String sWriteData)");
                                            Console.WriteLine("2.WriteUserData_MatchEPC(String ConnID, eAntennaNo antNum, String sWriteData, String sMatchData, int matchWordStartIndex) ");
                                            Console.WriteLine("3.WriteUserData_MatchEPC(String ConnID, eAntennaNo antNum, String sWriteData, String sMatchData, int matchWordStartIndex, String accessPassword) ");
                                            Console.WriteLine("4.WriteUserData_MatchTID(String ConnID, eAntennaNo antNum, String sWriteData, String sMatchData, int matchWordStartIndex) ");
                                            Console.WriteLine("5.WriteUserData_MatchTID(String ConnID, eAntennaNo antNum, String sWriteData, String sMatchData, int matchWordStartIndex, String accessPassword) ");
                                            Console.WriteLine("b.Back to Previous Menu");
                                            Console.WriteLine("q.Quit");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                eAntennaNo antNum = new eAntennaNo();
                                                while (true)
                                                {
                                                    Console.WriteLine("Please input antenna number:");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        antNum |= eAntennaNo._1;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        antNum |= eAntennaNo._2;
                                                    }
                                                    else if (readKey.Equals("3"))
                                                    {
                                                        antNum |= eAntennaNo._3;
                                                    }
                                                    else if (readKey.Equals("4"))
                                                    {
                                                        antNum |= eAntennaNo._4;
                                                    }
                                                    else if (readKey.Equals("5"))
                                                    {
                                                        antNum |= eAntennaNo._5;
                                                    }
                                                    else if (readKey.Equals("6"))
                                                    {
                                                        antNum |= eAntennaNo._6;
                                                    }
                                                    else if (readKey.Equals("7"))
                                                    {
                                                        antNum |= eAntennaNo._7;
                                                    }
                                                    else if (readKey.Equals("8"))
                                                    {
                                                        antNum |= eAntennaNo._8;
                                                    }
                                                    else if (readKey.Equals("9"))
                                                    {
                                                        antNum |= eAntennaNo._9;
                                                    }
                                                    else if (readKey.Equals("10"))
                                                    {
                                                        antNum |= eAntennaNo._10;
                                                    }
                                                    else if (readKey.Equals("11"))
                                                    {
                                                        antNum |= eAntennaNo._11;
                                                    }
                                                    else if (readKey.Equals("12"))
                                                    {
                                                        antNum |= eAntennaNo._12;
                                                    }
                                                    else if (readKey.Equals("13"))
                                                    {
                                                        antNum |= eAntennaNo._13;
                                                    }
                                                    else if (readKey.Equals("14"))
                                                    {
                                                        antNum |= eAntennaNo._14;
                                                    }
                                                    else if (readKey.Equals("15"))
                                                    {
                                                        antNum |= eAntennaNo._15;
                                                    }
                                                    else if (readKey.Equals("16"))
                                                    {
                                                        antNum |= eAntennaNo._16;
                                                    }
                                                    else if (readKey.Equals("17"))
                                                    {
                                                        antNum |= eAntennaNo._17;
                                                    }
                                                    else if (readKey.Equals("18"))
                                                    {
                                                        antNum |= eAntennaNo._18;
                                                    }
                                                    else if (readKey.Equals("19"))
                                                    {
                                                        antNum |= eAntennaNo._19;
                                                    }
                                                    else if (readKey.Equals("20"))
                                                    {
                                                        antNum |= eAntennaNo._20;
                                                    }
                                                    else if (readKey.Equals("21"))
                                                    {
                                                        antNum |= eAntennaNo._21;
                                                    }
                                                    else if (readKey.Equals("22"))
                                                    {
                                                        antNum |= eAntennaNo._22;
                                                    }
                                                    else if (readKey.Equals("23"))
                                                    {
                                                        antNum |= eAntennaNo._23;
                                                    }
                                                    else if (readKey.Equals("24"))
                                                    {
                                                        antNum |= eAntennaNo._24;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("antenna number error");
                                                        continue;
                                                    }

                                                    Console.WriteLine("Setting another antenna");
                                                    Console.WriteLine("1.Yes");
                                                    Console.WriteLine("2.No");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        continue;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Parameter error ,please reset");
                                                        antNum = 0;
                                                    }
                                                }

                                                Console.WriteLine("Please input sWriteData:");
                                                String sWriteData = Console.ReadLine();


                                                if (My_Reader._Tag6C.WriteUserData(ConnID, antNum, sWriteData, 0) == 0)
                                                {
                                                    Console.WriteLine("Success!");
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Failure!");
                                                }

                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                eAntennaNo antNum = new eAntennaNo();
                                                while (true)
                                                {
                                                    Console.WriteLine("Please input antenna number:");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        antNum |= eAntennaNo._1;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        antNum |= eAntennaNo._2;
                                                    }
                                                    else if (readKey.Equals("3"))
                                                    {
                                                        antNum |= eAntennaNo._3;
                                                    }
                                                    else if (readKey.Equals("4"))
                                                    {
                                                        antNum |= eAntennaNo._4;
                                                    }
                                                    else if (readKey.Equals("5"))
                                                    {
                                                        antNum |= eAntennaNo._5;
                                                    }
                                                    else if (readKey.Equals("6"))
                                                    {
                                                        antNum |= eAntennaNo._6;
                                                    }
                                                    else if (readKey.Equals("7"))
                                                    {
                                                        antNum |= eAntennaNo._7;
                                                    }
                                                    else if (readKey.Equals("8"))
                                                    {
                                                        antNum |= eAntennaNo._8;
                                                    }
                                                    else if (readKey.Equals("9"))
                                                    {
                                                        antNum |= eAntennaNo._9;
                                                    }
                                                    else if (readKey.Equals("10"))
                                                    {
                                                        antNum |= eAntennaNo._10;
                                                    }
                                                    else if (readKey.Equals("11"))
                                                    {
                                                        antNum |= eAntennaNo._11;
                                                    }
                                                    else if (readKey.Equals("12"))
                                                    {
                                                        antNum |= eAntennaNo._12;
                                                    }
                                                    else if (readKey.Equals("13"))
                                                    {
                                                        antNum |= eAntennaNo._13;
                                                    }
                                                    else if (readKey.Equals("14"))
                                                    {
                                                        antNum |= eAntennaNo._14;
                                                    }
                                                    else if (readKey.Equals("15"))
                                                    {
                                                        antNum |= eAntennaNo._15;
                                                    }
                                                    else if (readKey.Equals("16"))
                                                    {
                                                        antNum |= eAntennaNo._16;
                                                    }
                                                    else if (readKey.Equals("17"))
                                                    {
                                                        antNum |= eAntennaNo._17;
                                                    }
                                                    else if (readKey.Equals("18"))
                                                    {
                                                        antNum |= eAntennaNo._18;
                                                    }
                                                    else if (readKey.Equals("19"))
                                                    {
                                                        antNum |= eAntennaNo._19;
                                                    }
                                                    else if (readKey.Equals("20"))
                                                    {
                                                        antNum |= eAntennaNo._20;
                                                    }
                                                    else if (readKey.Equals("21"))
                                                    {
                                                        antNum |= eAntennaNo._21;
                                                    }
                                                    else if (readKey.Equals("22"))
                                                    {
                                                        antNum |= eAntennaNo._22;
                                                    }
                                                    else if (readKey.Equals("23"))
                                                    {
                                                        antNum |= eAntennaNo._23;
                                                    }
                                                    else if (readKey.Equals("24"))
                                                    {
                                                        antNum |= eAntennaNo._24;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("antenna number error");
                                                        continue;
                                                    }

                                                    Console.WriteLine("Setting another antenna");
                                                    Console.WriteLine("1.Yes");
                                                    Console.WriteLine("2.No");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        continue;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Parameter error ,please reset");
                                                        antNum = 0;
                                                    }
                                                }

                                                Console.WriteLine("Please input sWriteData:");
                                                String sWriteData = Console.ReadLine();

                                                Console.WriteLine("Please input sMatchData:");
                                                String sMatchData = Console.ReadLine();

                                                Console.WriteLine("Please input matchWordStartIndex:");
                                                int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                                if (My_Reader._Tag6C.WriteUserData_MatchEPC(ConnID, antNum, sWriteData, 0, sMatchData, matchWordStartIndex) == 0)
                                                {
                                                    Console.WriteLine("Success!");
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Failure!");
                                                }
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                eAntennaNo antNum = new eAntennaNo();
                                                while (true)
                                                {
                                                    Console.WriteLine("Please input antenna number:");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        antNum |= eAntennaNo._1;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        antNum |= eAntennaNo._2;
                                                    }
                                                    else if (readKey.Equals("3"))
                                                    {
                                                        antNum |= eAntennaNo._3;
                                                    }
                                                    else if (readKey.Equals("4"))
                                                    {
                                                        antNum |= eAntennaNo._4;
                                                    }
                                                    else if (readKey.Equals("5"))
                                                    {
                                                        antNum |= eAntennaNo._5;
                                                    }
                                                    else if (readKey.Equals("6"))
                                                    {
                                                        antNum |= eAntennaNo._6;
                                                    }
                                                    else if (readKey.Equals("7"))
                                                    {
                                                        antNum |= eAntennaNo._7;
                                                    }
                                                    else if (readKey.Equals("8"))
                                                    {
                                                        antNum |= eAntennaNo._8;
                                                    }
                                                    else if (readKey.Equals("9"))
                                                    {
                                                        antNum |= eAntennaNo._9;
                                                    }
                                                    else if (readKey.Equals("10"))
                                                    {
                                                        antNum |= eAntennaNo._10;
                                                    }
                                                    else if (readKey.Equals("11"))
                                                    {
                                                        antNum |= eAntennaNo._11;
                                                    }
                                                    else if (readKey.Equals("12"))
                                                    {
                                                        antNum |= eAntennaNo._12;
                                                    }
                                                    else if (readKey.Equals("13"))
                                                    {
                                                        antNum |= eAntennaNo._13;
                                                    }
                                                    else if (readKey.Equals("14"))
                                                    {
                                                        antNum |= eAntennaNo._14;
                                                    }
                                                    else if (readKey.Equals("15"))
                                                    {
                                                        antNum |= eAntennaNo._15;
                                                    }
                                                    else if (readKey.Equals("16"))
                                                    {
                                                        antNum |= eAntennaNo._16;
                                                    }
                                                    else if (readKey.Equals("17"))
                                                    {
                                                        antNum |= eAntennaNo._17;
                                                    }
                                                    else if (readKey.Equals("18"))
                                                    {
                                                        antNum |= eAntennaNo._18;
                                                    }
                                                    else if (readKey.Equals("19"))
                                                    {
                                                        antNum |= eAntennaNo._19;
                                                    }
                                                    else if (readKey.Equals("20"))
                                                    {
                                                        antNum |= eAntennaNo._20;
                                                    }
                                                    else if (readKey.Equals("21"))
                                                    {
                                                        antNum |= eAntennaNo._21;
                                                    }
                                                    else if (readKey.Equals("22"))
                                                    {
                                                        antNum |= eAntennaNo._22;
                                                    }
                                                    else if (readKey.Equals("23"))
                                                    {
                                                        antNum |= eAntennaNo._23;
                                                    }
                                                    else if (readKey.Equals("24"))
                                                    {
                                                        antNum |= eAntennaNo._24;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("antenna number error");
                                                        continue;
                                                    }

                                                    Console.WriteLine("Setting another antenna");
                                                    Console.WriteLine("1.Yes");
                                                    Console.WriteLine("2.No");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        continue;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Parameter error ,please reset");
                                                        antNum = 0;
                                                    }
                                                }

                                                Console.WriteLine("Please input sWriteData:");
                                                String sWriteData = Console.ReadLine();

                                                Console.WriteLine("Please input sMatchData:");
                                                String sMatchData = Console.ReadLine();

                                                Console.WriteLine("Please input matchWordStartIndex:");
                                                int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                                Console.WriteLine("Please input accessPassword:");
                                                String accessPassword = Console.ReadLine();

                                                if (My_Reader._Tag6C.WriteUserData_MatchEPC(ConnID, antNum, sWriteData, 0, sMatchData, matchWordStartIndex, accessPassword) == 0)
                                                {
                                                    Console.WriteLine("Success!");
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Failure!");
                                                }
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                eAntennaNo antNum = new eAntennaNo();
                                                while (true)
                                                {
                                                    Console.WriteLine("Please input antenna number:");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        antNum |= eAntennaNo._1;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        antNum |= eAntennaNo._2;
                                                    }
                                                    else if (readKey.Equals("3"))
                                                    {
                                                        antNum |= eAntennaNo._3;
                                                    }
                                                    else if (readKey.Equals("4"))
                                                    {
                                                        antNum |= eAntennaNo._4;
                                                    }
                                                    else if (readKey.Equals("5"))
                                                    {
                                                        antNum |= eAntennaNo._5;
                                                    }
                                                    else if (readKey.Equals("6"))
                                                    {
                                                        antNum |= eAntennaNo._6;
                                                    }
                                                    else if (readKey.Equals("7"))
                                                    {
                                                        antNum |= eAntennaNo._7;
                                                    }
                                                    else if (readKey.Equals("8"))
                                                    {
                                                        antNum |= eAntennaNo._8;
                                                    }
                                                    else if (readKey.Equals("9"))
                                                    {
                                                        antNum |= eAntennaNo._9;
                                                    }
                                                    else if (readKey.Equals("10"))
                                                    {
                                                        antNum |= eAntennaNo._10;
                                                    }
                                                    else if (readKey.Equals("11"))
                                                    {
                                                        antNum |= eAntennaNo._11;
                                                    }
                                                    else if (readKey.Equals("12"))
                                                    {
                                                        antNum |= eAntennaNo._12;
                                                    }
                                                    else if (readKey.Equals("13"))
                                                    {
                                                        antNum |= eAntennaNo._13;
                                                    }
                                                    else if (readKey.Equals("14"))
                                                    {
                                                        antNum |= eAntennaNo._14;
                                                    }
                                                    else if (readKey.Equals("15"))
                                                    {
                                                        antNum |= eAntennaNo._15;
                                                    }
                                                    else if (readKey.Equals("16"))
                                                    {
                                                        antNum |= eAntennaNo._16;
                                                    }
                                                    else if (readKey.Equals("17"))
                                                    {
                                                        antNum |= eAntennaNo._17;
                                                    }
                                                    else if (readKey.Equals("18"))
                                                    {
                                                        antNum |= eAntennaNo._18;
                                                    }
                                                    else if (readKey.Equals("19"))
                                                    {
                                                        antNum |= eAntennaNo._19;
                                                    }
                                                    else if (readKey.Equals("20"))
                                                    {
                                                        antNum |= eAntennaNo._20;
                                                    }
                                                    else if (readKey.Equals("21"))
                                                    {
                                                        antNum |= eAntennaNo._21;
                                                    }
                                                    else if (readKey.Equals("22"))
                                                    {
                                                        antNum |= eAntennaNo._22;
                                                    }
                                                    else if (readKey.Equals("23"))
                                                    {
                                                        antNum |= eAntennaNo._23;
                                                    }
                                                    else if (readKey.Equals("24"))
                                                    {
                                                        antNum |= eAntennaNo._24;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("antenna number error");
                                                        continue;
                                                    }

                                                    Console.WriteLine("Setting another antenna");
                                                    Console.WriteLine("1.Yes");
                                                    Console.WriteLine("2.No");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        continue;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Parameter error ,please reset");
                                                        antNum = 0;
                                                    }
                                                }

                                                Console.WriteLine("Please input sWriteData:");
                                                String sWriteData = Console.ReadLine();

                                                Console.WriteLine("Please input sMatchData:");
                                                String sMatchData = Console.ReadLine();

                                                Console.WriteLine("Please input matchWordStartIndex:");
                                                int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                                if (My_Reader._Tag6C.WriteUserData_MatchTID(ConnID, antNum, sWriteData, 0, sMatchData, matchWordStartIndex) == 0)
                                                {
                                                    Console.WriteLine("Success!");
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Failure!");
                                                }
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                eAntennaNo antNum = new eAntennaNo();
                                                while (true)
                                                {
                                                    Console.WriteLine("Please input antenna number:");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        antNum |= eAntennaNo._1;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        antNum |= eAntennaNo._2;
                                                    }
                                                    else if (readKey.Equals("3"))
                                                    {
                                                        antNum |= eAntennaNo._3;
                                                    }
                                                    else if (readKey.Equals("4"))
                                                    {
                                                        antNum |= eAntennaNo._4;
                                                    }
                                                    else if (readKey.Equals("5"))
                                                    {
                                                        antNum |= eAntennaNo._5;
                                                    }
                                                    else if (readKey.Equals("6"))
                                                    {
                                                        antNum |= eAntennaNo._6;
                                                    }
                                                    else if (readKey.Equals("7"))
                                                    {
                                                        antNum |= eAntennaNo._7;
                                                    }
                                                    else if (readKey.Equals("8"))
                                                    {
                                                        antNum |= eAntennaNo._8;
                                                    }
                                                    else if (readKey.Equals("9"))
                                                    {
                                                        antNum |= eAntennaNo._9;
                                                    }
                                                    else if (readKey.Equals("10"))
                                                    {
                                                        antNum |= eAntennaNo._10;
                                                    }
                                                    else if (readKey.Equals("11"))
                                                    {
                                                        antNum |= eAntennaNo._11;
                                                    }
                                                    else if (readKey.Equals("12"))
                                                    {
                                                        antNum |= eAntennaNo._12;
                                                    }
                                                    else if (readKey.Equals("13"))
                                                    {
                                                        antNum |= eAntennaNo._13;
                                                    }
                                                    else if (readKey.Equals("14"))
                                                    {
                                                        antNum |= eAntennaNo._14;
                                                    }
                                                    else if (readKey.Equals("15"))
                                                    {
                                                        antNum |= eAntennaNo._15;
                                                    }
                                                    else if (readKey.Equals("16"))
                                                    {
                                                        antNum |= eAntennaNo._16;
                                                    }
                                                    else if (readKey.Equals("17"))
                                                    {
                                                        antNum |= eAntennaNo._17;
                                                    }
                                                    else if (readKey.Equals("18"))
                                                    {
                                                        antNum |= eAntennaNo._18;
                                                    }
                                                    else if (readKey.Equals("19"))
                                                    {
                                                        antNum |= eAntennaNo._19;
                                                    }
                                                    else if (readKey.Equals("20"))
                                                    {
                                                        antNum |= eAntennaNo._20;
                                                    }
                                                    else if (readKey.Equals("21"))
                                                    {
                                                        antNum |= eAntennaNo._21;
                                                    }
                                                    else if (readKey.Equals("22"))
                                                    {
                                                        antNum |= eAntennaNo._22;
                                                    }
                                                    else if (readKey.Equals("23"))
                                                    {
                                                        antNum |= eAntennaNo._23;
                                                    }
                                                    else if (readKey.Equals("24"))
                                                    {
                                                        antNum |= eAntennaNo._24;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("antenna number error");
                                                        continue;
                                                    }

                                                    Console.WriteLine("Setting another antenna");
                                                    Console.WriteLine("1.Yes");
                                                    Console.WriteLine("2.No");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        continue;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Parameter error ,please reset");
                                                        antNum = 0;
                                                    }
                                                }

                                                Console.WriteLine("Please input sWriteData:");
                                                String sWriteData = Console.ReadLine();

                                                Console.WriteLine("Please input sMatchData:");
                                                String sMatchData = Console.ReadLine();

                                                Console.WriteLine("Please input matchWordStartIndex:");
                                                int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                                Console.WriteLine("Please input accessPassword:");
                                                String accessPassword = Console.ReadLine();

                                                if (My_Reader._Tag6C.WriteUserData_MatchTID(ConnID, antNum, sWriteData, 0, sMatchData, matchWordStartIndex, accessPassword) == 0)
                                                {
                                                    Console.WriteLine("Success!");
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Failure!");
                                                }
                                            }
                                            else if (readKey.Equals("b"))
                                            {
                                                break;
                                            }
                                            else if (readKey.Equals("q"))
                                            {
                                                My_Reader._Config.Stop(ConnID);
                                                My_Reader.CloseConn(ConnID);
                                                return;
                                            }
                                        }
                                    }





                                    else if (readKey.Equals("3"))
                                    {
                                        while (true)
                                        {
                                            Console.WriteLine("Please select method:");
                                            Console.WriteLine("1.WriteAccessPassWord(String ConnID, eAntennaNo antNum, String sWriteData) ");
                                            Console.WriteLine("2.WriteAccessPassWord(String ConnID, eAntennaNo antNum, String sWriteData, String accessPassword) ");
                                            Console.WriteLine("3.WriteAccessPassWord_MatchTID(String ConnID, eAntennaNo antNum, String sWriteData, String sMatchData, int matchWordStartIndex, String accessPassword)");
                                            Console.WriteLine("4.WriteDestroyPassWord(String ConnID, eAntennaNo antNum, String sWriteData) ");
                                            Console.WriteLine("5.WriteDestroyPassWord(String ConnID, eAntennaNo antNum, String sWriteData, String accessPassword) ");
                                            Console.WriteLine("6.WriteDestroyPassWord_MatchTID(String ConnID, eAntennaNo antNum, String sWriteData, String sMatchData, int matchWordStartIndex, String accessPassword)");
                                            Console.WriteLine("b.Back to Previous Menu");
                                            Console.WriteLine("q.Quit");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                eAntennaNo antNum = new eAntennaNo();
                                                while (true)
                                                {
                                                    Console.WriteLine("Please input antenna number:");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        antNum |= eAntennaNo._1;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        antNum |= eAntennaNo._2;
                                                    }
                                                    else if (readKey.Equals("3"))
                                                    {
                                                        antNum |= eAntennaNo._3;
                                                    }
                                                    else if (readKey.Equals("4"))
                                                    {
                                                        antNum |= eAntennaNo._4;
                                                    }
                                                    else if (readKey.Equals("5"))
                                                    {
                                                        antNum |= eAntennaNo._5;
                                                    }
                                                    else if (readKey.Equals("6"))
                                                    {
                                                        antNum |= eAntennaNo._6;
                                                    }
                                                    else if (readKey.Equals("7"))
                                                    {
                                                        antNum |= eAntennaNo._7;
                                                    }
                                                    else if (readKey.Equals("8"))
                                                    {
                                                        antNum |= eAntennaNo._8;
                                                    }
                                                    else if (readKey.Equals("9"))
                                                    {
                                                        antNum |= eAntennaNo._9;
                                                    }
                                                    else if (readKey.Equals("10"))
                                                    {
                                                        antNum |= eAntennaNo._10;
                                                    }
                                                    else if (readKey.Equals("11"))
                                                    {
                                                        antNum |= eAntennaNo._11;
                                                    }
                                                    else if (readKey.Equals("12"))
                                                    {
                                                        antNum |= eAntennaNo._12;
                                                    }
                                                    else if (readKey.Equals("13"))
                                                    {
                                                        antNum |= eAntennaNo._13;
                                                    }
                                                    else if (readKey.Equals("14"))
                                                    {
                                                        antNum |= eAntennaNo._14;
                                                    }
                                                    else if (readKey.Equals("15"))
                                                    {
                                                        antNum |= eAntennaNo._15;
                                                    }
                                                    else if (readKey.Equals("16"))
                                                    {
                                                        antNum |= eAntennaNo._16;
                                                    }
                                                    else if (readKey.Equals("17"))
                                                    {
                                                        antNum |= eAntennaNo._17;
                                                    }
                                                    else if (readKey.Equals("18"))
                                                    {
                                                        antNum |= eAntennaNo._18;
                                                    }
                                                    else if (readKey.Equals("19"))
                                                    {
                                                        antNum |= eAntennaNo._19;
                                                    }
                                                    else if (readKey.Equals("20"))
                                                    {
                                                        antNum |= eAntennaNo._20;
                                                    }
                                                    else if (readKey.Equals("21"))
                                                    {
                                                        antNum |= eAntennaNo._21;
                                                    }
                                                    else if (readKey.Equals("22"))
                                                    {
                                                        antNum |= eAntennaNo._22;
                                                    }
                                                    else if (readKey.Equals("23"))
                                                    {
                                                        antNum |= eAntennaNo._23;
                                                    }
                                                    else if (readKey.Equals("24"))
                                                    {
                                                        antNum |= eAntennaNo._24;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("antenna number error");
                                                        continue;
                                                    }

                                                    Console.WriteLine("Setting another antenna");
                                                    Console.WriteLine("1.Yes");
                                                    Console.WriteLine("2.No");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        continue;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Parameter error ,please reset");
                                                        antNum = 0;
                                                    }
                                                }

                                                Console.WriteLine("Please input sWriteData:");
                                                String sWriteData = Console.ReadLine();


                                                if (My_Reader._Tag6C.WriteAccessPassWord(ConnID, antNum, sWriteData) == 0)
                                                {
                                                    Console.WriteLine("Success!");
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Failure!");
                                                }
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                eAntennaNo antNum = new eAntennaNo();
                                                while (true)
                                                {
                                                    Console.WriteLine("Please input antenna number:");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        antNum |= eAntennaNo._1;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        antNum |= eAntennaNo._2;
                                                    }
                                                    else if (readKey.Equals("3"))
                                                    {
                                                        antNum |= eAntennaNo._3;
                                                    }
                                                    else if (readKey.Equals("4"))
                                                    {
                                                        antNum |= eAntennaNo._4;
                                                    }
                                                    else if (readKey.Equals("5"))
                                                    {
                                                        antNum |= eAntennaNo._5;
                                                    }
                                                    else if (readKey.Equals("6"))
                                                    {
                                                        antNum |= eAntennaNo._6;
                                                    }
                                                    else if (readKey.Equals("7"))
                                                    {
                                                        antNum |= eAntennaNo._7;
                                                    }
                                                    else if (readKey.Equals("8"))
                                                    {
                                                        antNum |= eAntennaNo._8;
                                                    }
                                                    else if (readKey.Equals("9"))
                                                    {
                                                        antNum |= eAntennaNo._9;
                                                    }
                                                    else if (readKey.Equals("10"))
                                                    {
                                                        antNum |= eAntennaNo._10;
                                                    }
                                                    else if (readKey.Equals("11"))
                                                    {
                                                        antNum |= eAntennaNo._11;
                                                    }
                                                    else if (readKey.Equals("12"))
                                                    {
                                                        antNum |= eAntennaNo._12;
                                                    }
                                                    else if (readKey.Equals("13"))
                                                    {
                                                        antNum |= eAntennaNo._13;
                                                    }
                                                    else if (readKey.Equals("14"))
                                                    {
                                                        antNum |= eAntennaNo._14;
                                                    }
                                                    else if (readKey.Equals("15"))
                                                    {
                                                        antNum |= eAntennaNo._15;
                                                    }
                                                    else if (readKey.Equals("16"))
                                                    {
                                                        antNum |= eAntennaNo._16;
                                                    }
                                                    else if (readKey.Equals("17"))
                                                    {
                                                        antNum |= eAntennaNo._17;
                                                    }
                                                    else if (readKey.Equals("18"))
                                                    {
                                                        antNum |= eAntennaNo._18;
                                                    }
                                                    else if (readKey.Equals("19"))
                                                    {
                                                        antNum |= eAntennaNo._19;
                                                    }
                                                    else if (readKey.Equals("20"))
                                                    {
                                                        antNum |= eAntennaNo._20;
                                                    }
                                                    else if (readKey.Equals("21"))
                                                    {
                                                        antNum |= eAntennaNo._21;
                                                    }
                                                    else if (readKey.Equals("22"))
                                                    {
                                                        antNum |= eAntennaNo._22;
                                                    }
                                                    else if (readKey.Equals("23"))
                                                    {
                                                        antNum |= eAntennaNo._23;
                                                    }
                                                    else if (readKey.Equals("24"))
                                                    {
                                                        antNum |= eAntennaNo._24;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("antenna number error");
                                                        continue;
                                                    }

                                                    Console.WriteLine("Setting another antenna");
                                                    Console.WriteLine("1.Yes");
                                                    Console.WriteLine("2.No");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        continue;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Parameter error ,please reset");
                                                        antNum = 0;
                                                    }
                                                }

                                                Console.WriteLine("Please input sWriteData:");
                                                String sWriteData = Console.ReadLine();

                                                Console.WriteLine("Please input accessPassword:");
                                                String accessPassword = Console.ReadLine();


                                                if (My_Reader._Tag6C.WriteAccessPassWord(ConnID, antNum, sWriteData, accessPassword) == 0)
                                                {
                                                    Console.WriteLine("Success!");
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Failure!");
                                                }
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                eAntennaNo antNum = new eAntennaNo();
                                                while (true)
                                                {
                                                    Console.WriteLine("Please input antenna number:");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        antNum |= eAntennaNo._1;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        antNum |= eAntennaNo._2;
                                                    }
                                                    else if (readKey.Equals("3"))
                                                    {
                                                        antNum |= eAntennaNo._3;
                                                    }
                                                    else if (readKey.Equals("4"))
                                                    {
                                                        antNum |= eAntennaNo._4;
                                                    }
                                                    else if (readKey.Equals("5"))
                                                    {
                                                        antNum |= eAntennaNo._5;
                                                    }
                                                    else if (readKey.Equals("6"))
                                                    {
                                                        antNum |= eAntennaNo._6;
                                                    }
                                                    else if (readKey.Equals("7"))
                                                    {
                                                        antNum |= eAntennaNo._7;
                                                    }
                                                    else if (readKey.Equals("8"))
                                                    {
                                                        antNum |= eAntennaNo._8;
                                                    }
                                                    else if (readKey.Equals("9"))
                                                    {
                                                        antNum |= eAntennaNo._9;
                                                    }
                                                    else if (readKey.Equals("10"))
                                                    {
                                                        antNum |= eAntennaNo._10;
                                                    }
                                                    else if (readKey.Equals("11"))
                                                    {
                                                        antNum |= eAntennaNo._11;
                                                    }
                                                    else if (readKey.Equals("12"))
                                                    {
                                                        antNum |= eAntennaNo._12;
                                                    }
                                                    else if (readKey.Equals("13"))
                                                    {
                                                        antNum |= eAntennaNo._13;
                                                    }
                                                    else if (readKey.Equals("14"))
                                                    {
                                                        antNum |= eAntennaNo._14;
                                                    }
                                                    else if (readKey.Equals("15"))
                                                    {
                                                        antNum |= eAntennaNo._15;
                                                    }
                                                    else if (readKey.Equals("16"))
                                                    {
                                                        antNum |= eAntennaNo._16;
                                                    }
                                                    else if (readKey.Equals("17"))
                                                    {
                                                        antNum |= eAntennaNo._17;
                                                    }
                                                    else if (readKey.Equals("18"))
                                                    {
                                                        antNum |= eAntennaNo._18;
                                                    }
                                                    else if (readKey.Equals("19"))
                                                    {
                                                        antNum |= eAntennaNo._19;
                                                    }
                                                    else if (readKey.Equals("20"))
                                                    {
                                                        antNum |= eAntennaNo._20;
                                                    }
                                                    else if (readKey.Equals("21"))
                                                    {
                                                        antNum |= eAntennaNo._21;
                                                    }
                                                    else if (readKey.Equals("22"))
                                                    {
                                                        antNum |= eAntennaNo._22;
                                                    }
                                                    else if (readKey.Equals("23"))
                                                    {
                                                        antNum |= eAntennaNo._23;
                                                    }
                                                    else if (readKey.Equals("24"))
                                                    {
                                                        antNum |= eAntennaNo._24;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("antenna number error");
                                                        continue;
                                                    }

                                                    Console.WriteLine("Setting another antenna");
                                                    Console.WriteLine("1.Yes");
                                                    Console.WriteLine("2.No");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        continue;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Parameter error ,please reset");
                                                        antNum = 0;
                                                    }
                                                }

                                                Console.WriteLine("Please input sWriteData:");
                                                String sWriteData = Console.ReadLine();

                                                Console.WriteLine("Please input sMatchData:");
                                                String sMatchData = Console.ReadLine();

                                                Console.WriteLine("Please input matchWordStartIndex:");
                                                int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                                Console.WriteLine("Please input accessPassword:");
                                                String accessPassword = Console.ReadLine();

                                                if (My_Reader._Tag6C.WriteAccessPassWord_MatchTID(ConnID, antNum, sWriteData, sMatchData, matchWordStartIndex, accessPassword) == 0)
                                                {
                                                    Console.WriteLine("Success!");
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Failure!");
                                                }
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                eAntennaNo antNum = new eAntennaNo();
                                                while (true)
                                                {
                                                    Console.WriteLine("Please input antenna number:");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        antNum |= eAntennaNo._1;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        antNum |= eAntennaNo._2;
                                                    }
                                                    else if (readKey.Equals("3"))
                                                    {
                                                        antNum |= eAntennaNo._3;
                                                    }
                                                    else if (readKey.Equals("4"))
                                                    {
                                                        antNum |= eAntennaNo._4;
                                                    }
                                                    else if (readKey.Equals("5"))
                                                    {
                                                        antNum |= eAntennaNo._5;
                                                    }
                                                    else if (readKey.Equals("6"))
                                                    {
                                                        antNum |= eAntennaNo._6;
                                                    }
                                                    else if (readKey.Equals("7"))
                                                    {
                                                        antNum |= eAntennaNo._7;
                                                    }
                                                    else if (readKey.Equals("8"))
                                                    {
                                                        antNum |= eAntennaNo._8;
                                                    }
                                                    else if (readKey.Equals("9"))
                                                    {
                                                        antNum |= eAntennaNo._9;
                                                    }
                                                    else if (readKey.Equals("10"))
                                                    {
                                                        antNum |= eAntennaNo._10;
                                                    }
                                                    else if (readKey.Equals("11"))
                                                    {
                                                        antNum |= eAntennaNo._11;
                                                    }
                                                    else if (readKey.Equals("12"))
                                                    {
                                                        antNum |= eAntennaNo._12;
                                                    }
                                                    else if (readKey.Equals("13"))
                                                    {
                                                        antNum |= eAntennaNo._13;
                                                    }
                                                    else if (readKey.Equals("14"))
                                                    {
                                                        antNum |= eAntennaNo._14;
                                                    }
                                                    else if (readKey.Equals("15"))
                                                    {
                                                        antNum |= eAntennaNo._15;
                                                    }
                                                    else if (readKey.Equals("16"))
                                                    {
                                                        antNum |= eAntennaNo._16;
                                                    }
                                                    else if (readKey.Equals("17"))
                                                    {
                                                        antNum |= eAntennaNo._17;
                                                    }
                                                    else if (readKey.Equals("18"))
                                                    {
                                                        antNum |= eAntennaNo._18;
                                                    }
                                                    else if (readKey.Equals("19"))
                                                    {
                                                        antNum |= eAntennaNo._19;
                                                    }
                                                    else if (readKey.Equals("20"))
                                                    {
                                                        antNum |= eAntennaNo._20;
                                                    }
                                                    else if (readKey.Equals("21"))
                                                    {
                                                        antNum |= eAntennaNo._21;
                                                    }
                                                    else if (readKey.Equals("22"))
                                                    {
                                                        antNum |= eAntennaNo._22;
                                                    }
                                                    else if (readKey.Equals("23"))
                                                    {
                                                        antNum |= eAntennaNo._23;
                                                    }
                                                    else if (readKey.Equals("24"))
                                                    {
                                                        antNum |= eAntennaNo._24;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("antenna number error");
                                                        continue;
                                                    }

                                                    Console.WriteLine("Setting another antenna");
                                                    Console.WriteLine("1.Yes");
                                                    Console.WriteLine("2.No");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        continue;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Parameter error ,please reset");
                                                        antNum = 0;
                                                    }
                                                }

                                                Console.WriteLine("Please input sWriteData:");
                                                String sWriteData = Console.ReadLine();


                                                if (My_Reader._Tag6C.WriteDestroyPassWord(ConnID, antNum, sWriteData) == 0)
                                                {
                                                    Console.WriteLine("Success!");
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Failure!");
                                                }
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                eAntennaNo antNum = new eAntennaNo();
                                                while (true)
                                                {
                                                    Console.WriteLine("Please input antenna number:");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        antNum |= eAntennaNo._1;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        antNum |= eAntennaNo._2;
                                                    }
                                                    else if (readKey.Equals("3"))
                                                    {
                                                        antNum |= eAntennaNo._3;
                                                    }
                                                    else if (readKey.Equals("4"))
                                                    {
                                                        antNum |= eAntennaNo._4;
                                                    }
                                                    else if (readKey.Equals("5"))
                                                    {
                                                        antNum |= eAntennaNo._5;
                                                    }
                                                    else if (readKey.Equals("6"))
                                                    {
                                                        antNum |= eAntennaNo._6;
                                                    }
                                                    else if (readKey.Equals("7"))
                                                    {
                                                        antNum |= eAntennaNo._7;
                                                    }
                                                    else if (readKey.Equals("8"))
                                                    {
                                                        antNum |= eAntennaNo._8;
                                                    }
                                                    else if (readKey.Equals("9"))
                                                    {
                                                        antNum |= eAntennaNo._9;
                                                    }
                                                    else if (readKey.Equals("10"))
                                                    {
                                                        antNum |= eAntennaNo._10;
                                                    }
                                                    else if (readKey.Equals("11"))
                                                    {
                                                        antNum |= eAntennaNo._11;
                                                    }
                                                    else if (readKey.Equals("12"))
                                                    {
                                                        antNum |= eAntennaNo._12;
                                                    }
                                                    else if (readKey.Equals("13"))
                                                    {
                                                        antNum |= eAntennaNo._13;
                                                    }
                                                    else if (readKey.Equals("14"))
                                                    {
                                                        antNum |= eAntennaNo._14;
                                                    }
                                                    else if (readKey.Equals("15"))
                                                    {
                                                        antNum |= eAntennaNo._15;
                                                    }
                                                    else if (readKey.Equals("16"))
                                                    {
                                                        antNum |= eAntennaNo._16;
                                                    }
                                                    else if (readKey.Equals("17"))
                                                    {
                                                        antNum |= eAntennaNo._17;
                                                    }
                                                    else if (readKey.Equals("18"))
                                                    {
                                                        antNum |= eAntennaNo._18;
                                                    }
                                                    else if (readKey.Equals("19"))
                                                    {
                                                        antNum |= eAntennaNo._19;
                                                    }
                                                    else if (readKey.Equals("20"))
                                                    {
                                                        antNum |= eAntennaNo._20;
                                                    }
                                                    else if (readKey.Equals("21"))
                                                    {
                                                        antNum |= eAntennaNo._21;
                                                    }
                                                    else if (readKey.Equals("22"))
                                                    {
                                                        antNum |= eAntennaNo._22;
                                                    }
                                                    else if (readKey.Equals("23"))
                                                    {
                                                        antNum |= eAntennaNo._23;
                                                    }
                                                    else if (readKey.Equals("24"))
                                                    {
                                                        antNum |= eAntennaNo._24;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("antenna number error");
                                                        continue;
                                                    }

                                                    Console.WriteLine("Setting another antenna");
                                                    Console.WriteLine("1.Yes");
                                                    Console.WriteLine("2.No");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        continue;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Parameter error ,please reset");
                                                        antNum = 0;
                                                    }
                                                }

                                                Console.WriteLine("Please input sWriteData:");
                                                String sWriteData = Console.ReadLine();

                                                Console.WriteLine("Please input accessPassword:");
                                                String accessPassword = Console.ReadLine();


                                                if (My_Reader._Tag6C.WriteDestroyPassWord(ConnID, antNum, sWriteData, accessPassword) == 0)
                                                {
                                                    Console.WriteLine("Success!");
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Failure!");
                                                }
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                eAntennaNo antNum = new eAntennaNo();
                                                while (true)
                                                {
                                                    Console.WriteLine("Please input antenna number:");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        antNum |= eAntennaNo._1;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        antNum |= eAntennaNo._2;
                                                    }
                                                    else if (readKey.Equals("3"))
                                                    {
                                                        antNum |= eAntennaNo._3;
                                                    }
                                                    else if (readKey.Equals("4"))
                                                    {
                                                        antNum |= eAntennaNo._4;
                                                    }
                                                    else if (readKey.Equals("5"))
                                                    {
                                                        antNum |= eAntennaNo._5;
                                                    }
                                                    else if (readKey.Equals("6"))
                                                    {
                                                        antNum |= eAntennaNo._6;
                                                    }
                                                    else if (readKey.Equals("7"))
                                                    {
                                                        antNum |= eAntennaNo._7;
                                                    }
                                                    else if (readKey.Equals("8"))
                                                    {
                                                        antNum |= eAntennaNo._8;
                                                    }
                                                    else if (readKey.Equals("9"))
                                                    {
                                                        antNum |= eAntennaNo._9;
                                                    }
                                                    else if (readKey.Equals("10"))
                                                    {
                                                        antNum |= eAntennaNo._10;
                                                    }
                                                    else if (readKey.Equals("11"))
                                                    {
                                                        antNum |= eAntennaNo._11;
                                                    }
                                                    else if (readKey.Equals("12"))
                                                    {
                                                        antNum |= eAntennaNo._12;
                                                    }
                                                    else if (readKey.Equals("13"))
                                                    {
                                                        antNum |= eAntennaNo._13;
                                                    }
                                                    else if (readKey.Equals("14"))
                                                    {
                                                        antNum |= eAntennaNo._14;
                                                    }
                                                    else if (readKey.Equals("15"))
                                                    {
                                                        antNum |= eAntennaNo._15;
                                                    }
                                                    else if (readKey.Equals("16"))
                                                    {
                                                        antNum |= eAntennaNo._16;
                                                    }
                                                    else if (readKey.Equals("17"))
                                                    {
                                                        antNum |= eAntennaNo._17;
                                                    }
                                                    else if (readKey.Equals("18"))
                                                    {
                                                        antNum |= eAntennaNo._18;
                                                    }
                                                    else if (readKey.Equals("19"))
                                                    {
                                                        antNum |= eAntennaNo._19;
                                                    }
                                                    else if (readKey.Equals("20"))
                                                    {
                                                        antNum |= eAntennaNo._20;
                                                    }
                                                    else if (readKey.Equals("21"))
                                                    {
                                                        antNum |= eAntennaNo._21;
                                                    }
                                                    else if (readKey.Equals("22"))
                                                    {
                                                        antNum |= eAntennaNo._22;
                                                    }
                                                    else if (readKey.Equals("23"))
                                                    {
                                                        antNum |= eAntennaNo._23;
                                                    }
                                                    else if (readKey.Equals("24"))
                                                    {
                                                        antNum |= eAntennaNo._24;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("antenna number error");
                                                        continue;
                                                    }

                                                    Console.WriteLine("Setting another antenna");
                                                    Console.WriteLine("1.Yes");
                                                    Console.WriteLine("2.No");
                                                    readKey = Console.ReadLine();
                                                    if (readKey.Equals("1"))
                                                    {
                                                        continue;
                                                    }
                                                    else if (readKey.Equals("2"))
                                                    {
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Parameter error ,please reset");
                                                        antNum = 0;
                                                    }
                                                }

                                                Console.WriteLine("Please input sWriteData:");
                                                String sWriteData = Console.ReadLine();

                                                Console.WriteLine("Please input sMatchData:");
                                                String sMatchData = Console.ReadLine();

                                                Console.WriteLine("Please input matchWordStartIndex:");
                                                int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                                Console.WriteLine("Please input accessPassword:");
                                                String accessPassword = Console.ReadLine();

                                                if (My_Reader._Tag6C.WriteDestroyPassWord_MatchTID(ConnID, antNum, sWriteData, sMatchData, matchWordStartIndex, accessPassword) == 0)
                                                {
                                                    Console.WriteLine("Success!");
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Failure!");
                                                }
                                            }
                                            else if (readKey.Equals("b"))
                                            {
                                                break;
                                            }
                                            else if (readKey.Equals("q"))
                                            {
                                                My_Reader._Config.Stop(ConnID);
                                                My_Reader.CloseConn(ConnID);
                                                return;
                                            }
                                        }
                                    }


                                    else if (readKey.Equals("b"))
                                    {
                                        break;
                                    }
                                    else if (readKey.Equals("q"))
                                    {
                                        My_Reader._Config.Stop(ConnID);
                                        My_Reader.CloseConn(ConnID);
                                        return;
                                    }
                                }
                            }

                    #endregion

                    #region Lock 6C Tag

                            else if (readKey.Equals("3"))
                            {
                                while (true)
                                {
                                    Console.WriteLine("Please select method:");
                                    Console.WriteLine("1.Lock(String ConnID, eAntennaNo antNum, eLockArea lockArea, eLockType lockType)");
                                    Console.WriteLine("2.Lock_MatchEPC(String ConnID, eAntennaNo antNum, eLockArea lockArea, eLockType lockType, String sMatchData, int matchWordStartIndex)");
                                    Console.WriteLine("3.Lock_MatchEPC(String ConnID, eAntennaNo antNum, eLockArea lockArea, eLockType lockType, String sMatchData, int matchWordStartIndex, String accessPassword)");
                                    Console.WriteLine("4.Lock_MatchTID(String ConnID, eAntennaNo antNum, eLockArea lockArea, eLockType lockType, String sMatchData, int matchWordStartIndex)");
                                    Console.WriteLine("5.Lock_MatchTID(String ConnID, eAntennaNo antNum, eLockArea lockArea, eLockType lockType, String sMatchData, int matchWordStartIndex, String accessPassword)");
                                    Console.WriteLine("b.Back to Previous Menu");
                                    Console.WriteLine("q.Quit");
                                    readKey = Console.ReadLine();

                                    if (readKey.Equals("1"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else if (readKey.Equals("9"))
                                            {
                                                antNum |= eAntennaNo._9;
                                            }
                                            else if (readKey.Equals("10"))
                                            {
                                                antNum |= eAntennaNo._10;
                                            }
                                            else if (readKey.Equals("11"))
                                            {
                                                antNum |= eAntennaNo._11;
                                            }
                                            else if (readKey.Equals("12"))
                                            {
                                                antNum |= eAntennaNo._12;
                                            }
                                            else if (readKey.Equals("13"))
                                            {
                                                antNum |= eAntennaNo._13;
                                            }
                                            else if (readKey.Equals("14"))
                                            {
                                                antNum |= eAntennaNo._14;
                                            }
                                            else if (readKey.Equals("15"))
                                            {
                                                antNum |= eAntennaNo._15;
                                            }
                                            else if (readKey.Equals("16"))
                                            {
                                                antNum |= eAntennaNo._16;
                                            }
                                            else if (readKey.Equals("17"))
                                            {
                                                antNum |= eAntennaNo._17;
                                            }
                                            else if (readKey.Equals("18"))
                                            {
                                                antNum |= eAntennaNo._18;
                                            }
                                            else if (readKey.Equals("19"))
                                            {
                                                antNum |= eAntennaNo._19;
                                            }
                                            else if (readKey.Equals("20"))
                                            {
                                                antNum |= eAntennaNo._20;
                                            }
                                            else if (readKey.Equals("21"))
                                            {
                                                antNum |= eAntennaNo._21;
                                            }
                                            else if (readKey.Equals("22"))
                                            {
                                                antNum |= eAntennaNo._22;
                                            }
                                            else if (readKey.Equals("23"))
                                            {
                                                antNum |= eAntennaNo._23;
                                            }
                                            else if (readKey.Equals("24"))
                                            {
                                                antNum |= eAntennaNo._24;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = 0;
                                            }
                                        }

                                        Console.WriteLine("Please select lockArea:");
                                        Console.WriteLine("1.eLockArea.epc");
                                        Console.WriteLine("2.eLockArea.tid");
                                        Console.WriteLine("3.eLockArea.userdata");
                                        Console.WriteLine("4.eLockArea.AccessPassword");
                                        Console.WriteLine("5.eLockArea.DestroyPassword");
                                        eLockArea lockArea;
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            lockArea = eLockArea.epc;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            lockArea = eLockArea.tid;
                                        }
                                        else if (readKey.Equals("3"))
                                        {
                                            lockArea = eLockArea.userdata;
                                        }
                                        else if (readKey.Equals("4"))
                                        {
                                            lockArea = eLockArea.AccessPassword;
                                        }
                                        else if (readKey.Equals("5"))
                                        {
                                            lockArea = eLockArea.DestroyPassword;
                                        }
                                        else
                                        {
                                            Console.WriteLine("lockArea error");
                                            continue;
                                        }


                                        Console.WriteLine("Please select lockType:");
                                        Console.WriteLine("1.eLockType.Lock;");
                                        Console.WriteLine("2.eLockType.Unlock;");
                                        Console.WriteLine("3.eLockType.PermanentLock");
                                        Console.WriteLine("4.eLockType.PermanentUnlock");
                                        eLockType lockType;
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            lockType = eLockType.Lock;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            lockType = eLockType.Unlock;
                                        }
                                        else if (readKey.Equals("3"))
                                        {
                                            lockType = eLockType.PermanentLock;
                                        }
                                        else if (readKey.Equals("4"))
                                        {
                                            lockType = eLockType.PermanentUnlock;
                                        }
                                        else
                                        {
                                            Console.WriteLine("lockType error");
                                            continue;
                                        }


                                        if (My_Reader._Tag6C.Lock(ConnID, antNum, lockArea, lockType) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }

                                    }
                                    else if (readKey.Equals("2"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else if (readKey.Equals("9"))
                                            {
                                                antNum |= eAntennaNo._9;
                                            }
                                            else if (readKey.Equals("10"))
                                            {
                                                antNum |= eAntennaNo._10;
                                            }
                                            else if (readKey.Equals("11"))
                                            {
                                                antNum |= eAntennaNo._11;
                                            }
                                            else if (readKey.Equals("12"))
                                            {
                                                antNum |= eAntennaNo._12;
                                            }
                                            else if (readKey.Equals("13"))
                                            {
                                                antNum |= eAntennaNo._13;
                                            }
                                            else if (readKey.Equals("14"))
                                            {
                                                antNum |= eAntennaNo._14;
                                            }
                                            else if (readKey.Equals("15"))
                                            {
                                                antNum |= eAntennaNo._15;
                                            }
                                            else if (readKey.Equals("16"))
                                            {
                                                antNum |= eAntennaNo._16;
                                            }
                                            else if (readKey.Equals("17"))
                                            {
                                                antNum |= eAntennaNo._17;
                                            }
                                            else if (readKey.Equals("18"))
                                            {
                                                antNum |= eAntennaNo._18;
                                            }
                                            else if (readKey.Equals("19"))
                                            {
                                                antNum |= eAntennaNo._19;
                                            }
                                            else if (readKey.Equals("20"))
                                            {
                                                antNum |= eAntennaNo._20;
                                            }
                                            else if (readKey.Equals("21"))
                                            {
                                                antNum |= eAntennaNo._21;
                                            }
                                            else if (readKey.Equals("22"))
                                            {
                                                antNum |= eAntennaNo._22;
                                            }
                                            else if (readKey.Equals("23"))
                                            {
                                                antNum |= eAntennaNo._23;
                                            }
                                            else if (readKey.Equals("24"))
                                            {
                                                antNum |= eAntennaNo._24;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = 0;
                                            }
                                        }

                                        Console.WriteLine("Please select lockArea:");
                                        Console.WriteLine("1.eLockArea.epc");
                                        Console.WriteLine("2.eLockArea.tid");
                                        Console.WriteLine("3.eLockArea.userdata");
                                        Console.WriteLine("4.eLockArea.AccessPassword");
                                        Console.WriteLine("5.eLockArea.DestroyPassword");
                                        eLockArea lockArea;
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            lockArea = eLockArea.epc;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            lockArea = eLockArea.tid;
                                        }
                                        else if (readKey.Equals("3"))
                                        {
                                            lockArea = eLockArea.userdata;
                                        }
                                        else if (readKey.Equals("4"))
                                        {
                                            lockArea = eLockArea.AccessPassword;
                                        }
                                        else if (readKey.Equals("5"))
                                        {
                                            lockArea = eLockArea.DestroyPassword;
                                        }
                                        else
                                        {
                                            Console.WriteLine("lockArea error");
                                            continue;
                                        }


                                        Console.WriteLine("Please select lockType:");
                                        Console.WriteLine("1.eLockType.Lock;");
                                        Console.WriteLine("2.eLockType.Unlock;");
                                        Console.WriteLine("3.eLockType.PermanentLock");
                                        Console.WriteLine("4.eLockType.PermanentUnlock");
                                        eLockType lockType;
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            lockType = eLockType.Lock;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            lockType = eLockType.Unlock;
                                        }
                                        else if (readKey.Equals("3"))
                                        {
                                            lockType = eLockType.PermanentLock;
                                        }
                                        else if (readKey.Equals("4"))
                                        {
                                            lockType = eLockType.PermanentUnlock;
                                        }
                                        else
                                        {
                                            Console.WriteLine("lockType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input sMatchData");
                                        String sMatchData = Console.ReadLine();

                                        Console.WriteLine("Please input matchWordStartIndex");
                                        int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                        if (My_Reader._Tag6C.Lock_MatchEPC(ConnID, antNum, lockArea, lockType, sMatchData, matchWordStartIndex) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("3"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else if (readKey.Equals("9"))
                                            {
                                                antNum |= eAntennaNo._9;
                                            }
                                            else if (readKey.Equals("10"))
                                            {
                                                antNum |= eAntennaNo._10;
                                            }
                                            else if (readKey.Equals("11"))
                                            {
                                                antNum |= eAntennaNo._11;
                                            }
                                            else if (readKey.Equals("12"))
                                            {
                                                antNum |= eAntennaNo._12;
                                            }
                                            else if (readKey.Equals("13"))
                                            {
                                                antNum |= eAntennaNo._13;
                                            }
                                            else if (readKey.Equals("14"))
                                            {
                                                antNum |= eAntennaNo._14;
                                            }
                                            else if (readKey.Equals("15"))
                                            {
                                                antNum |= eAntennaNo._15;
                                            }
                                            else if (readKey.Equals("16"))
                                            {
                                                antNum |= eAntennaNo._16;
                                            }
                                            else if (readKey.Equals("17"))
                                            {
                                                antNum |= eAntennaNo._17;
                                            }
                                            else if (readKey.Equals("18"))
                                            {
                                                antNum |= eAntennaNo._18;
                                            }
                                            else if (readKey.Equals("19"))
                                            {
                                                antNum |= eAntennaNo._19;
                                            }
                                            else if (readKey.Equals("20"))
                                            {
                                                antNum |= eAntennaNo._20;
                                            }
                                            else if (readKey.Equals("21"))
                                            {
                                                antNum |= eAntennaNo._21;
                                            }
                                            else if (readKey.Equals("22"))
                                            {
                                                antNum |= eAntennaNo._22;
                                            }
                                            else if (readKey.Equals("23"))
                                            {
                                                antNum |= eAntennaNo._23;
                                            }
                                            else if (readKey.Equals("24"))
                                            {
                                                antNum |= eAntennaNo._24;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = 0;
                                            }
                                        }

                                        Console.WriteLine("Please select lockArea:");
                                        Console.WriteLine("1.eLockArea.epc");
                                        Console.WriteLine("2.eLockArea.tid");
                                        Console.WriteLine("3.eLockArea.userdata");
                                        Console.WriteLine("4.eLockArea.AccessPassword");
                                        Console.WriteLine("5.eLockArea.DestroyPassword");
                                        eLockArea lockArea;
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            lockArea = eLockArea.epc;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            lockArea = eLockArea.tid;
                                        }
                                        else if (readKey.Equals("3"))
                                        {
                                            lockArea = eLockArea.userdata;
                                        }
                                        else if (readKey.Equals("4"))
                                        {
                                            lockArea = eLockArea.AccessPassword;
                                        }
                                        else if (readKey.Equals("5"))
                                        {
                                            lockArea = eLockArea.DestroyPassword;
                                        }
                                        else
                                        {
                                            Console.WriteLine("lockArea error");
                                            continue;
                                        }


                                        Console.WriteLine("Please select lockType:");
                                        Console.WriteLine("1.eLockType.Lock;");
                                        Console.WriteLine("2.eLockType.Unlock;");
                                        Console.WriteLine("3.eLockType.PermanentLock");
                                        Console.WriteLine("4.eLockType.PermanentUnlock");
                                        eLockType lockType;
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            lockType = eLockType.Lock;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            lockType = eLockType.Unlock;
                                        }
                                        else if (readKey.Equals("3"))
                                        {
                                            lockType = eLockType.PermanentLock;
                                        }
                                        else if (readKey.Equals("4"))
                                        {
                                            lockType = eLockType.PermanentUnlock;
                                        }
                                        else
                                        {
                                            Console.WriteLine("lockType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input sMatchData");
                                        String sMatchData = Console.ReadLine();

                                        Console.WriteLine("Please input matchWordStartIndex");
                                        int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input accessPassword");
                                        String accessPassword = Console.ReadLine();

                                        if (My_Reader._Tag6C.Lock_MatchEPC(ConnID, antNum, lockArea, lockType, sMatchData, matchWordStartIndex, accessPassword) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("4"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else if (readKey.Equals("9"))
                                            {
                                                antNum |= eAntennaNo._9;
                                            }
                                            else if (readKey.Equals("10"))
                                            {
                                                antNum |= eAntennaNo._10;
                                            }
                                            else if (readKey.Equals("11"))
                                            {
                                                antNum |= eAntennaNo._11;
                                            }
                                            else if (readKey.Equals("12"))
                                            {
                                                antNum |= eAntennaNo._12;
                                            }
                                            else if (readKey.Equals("13"))
                                            {
                                                antNum |= eAntennaNo._13;
                                            }
                                            else if (readKey.Equals("14"))
                                            {
                                                antNum |= eAntennaNo._14;
                                            }
                                            else if (readKey.Equals("15"))
                                            {
                                                antNum |= eAntennaNo._15;
                                            }
                                            else if (readKey.Equals("16"))
                                            {
                                                antNum |= eAntennaNo._16;
                                            }
                                            else if (readKey.Equals("17"))
                                            {
                                                antNum |= eAntennaNo._17;
                                            }
                                            else if (readKey.Equals("18"))
                                            {
                                                antNum |= eAntennaNo._18;
                                            }
                                            else if (readKey.Equals("19"))
                                            {
                                                antNum |= eAntennaNo._19;
                                            }
                                            else if (readKey.Equals("20"))
                                            {
                                                antNum |= eAntennaNo._20;
                                            }
                                            else if (readKey.Equals("21"))
                                            {
                                                antNum |= eAntennaNo._21;
                                            }
                                            else if (readKey.Equals("22"))
                                            {
                                                antNum |= eAntennaNo._22;
                                            }
                                            else if (readKey.Equals("23"))
                                            {
                                                antNum |= eAntennaNo._23;
                                            }
                                            else if (readKey.Equals("24"))
                                            {
                                                antNum |= eAntennaNo._24;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = 0;
                                            }
                                        }

                                        Console.WriteLine("Please select lockArea:");
                                        Console.WriteLine("1.eLockArea.epc");
                                        Console.WriteLine("2.eLockArea.tid");
                                        Console.WriteLine("3.eLockArea.userdata");
                                        Console.WriteLine("4.eLockArea.AccessPassword");
                                        Console.WriteLine("5.eLockArea.DestroyPassword");
                                        eLockArea lockArea;
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            lockArea = eLockArea.epc;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            lockArea = eLockArea.tid;
                                        }
                                        else if (readKey.Equals("3"))
                                        {
                                            lockArea = eLockArea.userdata;
                                        }
                                        else if (readKey.Equals("4"))
                                        {
                                            lockArea = eLockArea.AccessPassword;
                                        }
                                        else if (readKey.Equals("5"))
                                        {
                                            lockArea = eLockArea.DestroyPassword;
                                        }
                                        else
                                        {
                                            Console.WriteLine("lockArea error");
                                            continue;
                                        }


                                        Console.WriteLine("Please select lockType:");
                                        Console.WriteLine("1.eLockType.Lock;");
                                        Console.WriteLine("2.eLockType.Unlock;");
                                        Console.WriteLine("3.eLockType.PermanentLock");
                                        Console.WriteLine("4.eLockType.PermanentUnlock");
                                        eLockType lockType;
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            lockType = eLockType.Lock;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            lockType = eLockType.Unlock;
                                        }
                                        else if (readKey.Equals("3"))
                                        {
                                            lockType = eLockType.PermanentLock;
                                        }
                                        else if (readKey.Equals("4"))
                                        {
                                            lockType = eLockType.PermanentUnlock;
                                        }
                                        else
                                        {
                                            Console.WriteLine("lockType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input sMatchData");
                                        String sMatchData = Console.ReadLine();

                                        Console.WriteLine("Please input matchWordStartIndex");
                                        int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                        if (My_Reader._Tag6C.Lock_MatchEPC(ConnID, antNum, lockArea, lockType, sMatchData, matchWordStartIndex) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("5"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else if (readKey.Equals("9"))
                                            {
                                                antNum |= eAntennaNo._9;
                                            }
                                            else if (readKey.Equals("10"))
                                            {
                                                antNum |= eAntennaNo._10;
                                            }
                                            else if (readKey.Equals("11"))
                                            {
                                                antNum |= eAntennaNo._11;
                                            }
                                            else if (readKey.Equals("12"))
                                            {
                                                antNum |= eAntennaNo._12;
                                            }
                                            else if (readKey.Equals("13"))
                                            {
                                                antNum |= eAntennaNo._13;
                                            }
                                            else if (readKey.Equals("14"))
                                            {
                                                antNum |= eAntennaNo._14;
                                            }
                                            else if (readKey.Equals("15"))
                                            {
                                                antNum |= eAntennaNo._15;
                                            }
                                            else if (readKey.Equals("16"))
                                            {
                                                antNum |= eAntennaNo._16;
                                            }
                                            else if (readKey.Equals("17"))
                                            {
                                                antNum |= eAntennaNo._17;
                                            }
                                            else if (readKey.Equals("18"))
                                            {
                                                antNum |= eAntennaNo._18;
                                            }
                                            else if (readKey.Equals("19"))
                                            {
                                                antNum |= eAntennaNo._19;
                                            }
                                            else if (readKey.Equals("20"))
                                            {
                                                antNum |= eAntennaNo._20;
                                            }
                                            else if (readKey.Equals("21"))
                                            {
                                                antNum |= eAntennaNo._21;
                                            }
                                            else if (readKey.Equals("22"))
                                            {
                                                antNum |= eAntennaNo._22;
                                            }
                                            else if (readKey.Equals("23"))
                                            {
                                                antNum |= eAntennaNo._23;
                                            }
                                            else if (readKey.Equals("24"))
                                            {
                                                antNum |= eAntennaNo._24;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = 0;
                                            }
                                        }

                                        Console.WriteLine("Please select lockArea:");
                                        Console.WriteLine("1.eLockArea.epc");
                                        Console.WriteLine("2.eLockArea.tid");
                                        Console.WriteLine("3.eLockArea.userdata");
                                        Console.WriteLine("4.eLockArea.AccessPassword");
                                        Console.WriteLine("5.eLockArea.DestroyPassword");
                                        eLockArea lockArea;
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            lockArea = eLockArea.epc;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            lockArea = eLockArea.tid;
                                        }
                                        else if (readKey.Equals("3"))
                                        {
                                            lockArea = eLockArea.userdata;
                                        }
                                        else if (readKey.Equals("4"))
                                        {
                                            lockArea = eLockArea.AccessPassword;
                                        }
                                        else if (readKey.Equals("5"))
                                        {
                                            lockArea = eLockArea.DestroyPassword;
                                        }
                                        else
                                        {
                                            Console.WriteLine("lockArea error");
                                            continue;
                                        }


                                        Console.WriteLine("Please select lockType:");
                                        Console.WriteLine("1.eLockType.Lock;");
                                        Console.WriteLine("2.eLockType.Unlock;");
                                        Console.WriteLine("3.eLockType.PermanentLock");
                                        Console.WriteLine("4.eLockType.PermanentUnlock");
                                        eLockType lockType;
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            lockType = eLockType.Lock;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            lockType = eLockType.Unlock;
                                        }
                                        else if (readKey.Equals("3"))
                                        {
                                            lockType = eLockType.PermanentLock;
                                        }
                                        else if (readKey.Equals("4"))
                                        {
                                            lockType = eLockType.PermanentUnlock;
                                        }
                                        else
                                        {
                                            Console.WriteLine("lockType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input sMatchData");
                                        String sMatchData = Console.ReadLine();

                                        Console.WriteLine("Please input matchWordStartIndex");
                                        int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input accessPassword");
                                        String accessPassword = Console.ReadLine();

                                        if (My_Reader._Tag6C.Lock_MatchTID(ConnID, antNum, lockArea, lockType, sMatchData, matchWordStartIndex, accessPassword) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("b"))
                                    {
                                        break;
                                    }
                                    else if (readKey.Equals("q"))
                                    {
                                        My_Reader._Config.Stop(ConnID);
                                        My_Reader.CloseConn(ConnID);
                                        return;
                                    }

                                }
                            }


                            #endregion

                    #region Destory 6C Tag

                            else if (readKey.Equals("4"))
                            {
                                while (true)
                                {
                                    Console.WriteLine("Please select method:");
                                    Console.WriteLine("1.Destroy(String ConnID, eAntennaNo antNum, String destroyPassword)");
                                    Console.WriteLine("2.Destroy_MatchEPC(String ConnID, eAntennaNo antNum, String destroyPassword, String sMatchData, int matchWordStartIndex)");
                                    Console.WriteLine("3.Destroy_MatchTID(String ConnID, eAntennaNo antNum, String destroyPassword, String sMatchData, int matchWordStartIndex)");
                                    Console.WriteLine("b.Back to Previous Menu");
                                    Console.WriteLine("q.Quit");
                                    readKey = Console.ReadLine();
                                    if (readKey.Equals("1"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else if (readKey.Equals("9"))
                                            {
                                                antNum |= eAntennaNo._9;
                                            }
                                            else if (readKey.Equals("10"))
                                            {
                                                antNum |= eAntennaNo._10;
                                            }
                                            else if (readKey.Equals("11"))
                                            {
                                                antNum |= eAntennaNo._11;
                                            }
                                            else if (readKey.Equals("12"))
                                            {
                                                antNum |= eAntennaNo._12;
                                            }
                                            else if (readKey.Equals("13"))
                                            {
                                                antNum |= eAntennaNo._13;
                                            }
                                            else if (readKey.Equals("14"))
                                            {
                                                antNum |= eAntennaNo._14;
                                            }
                                            else if (readKey.Equals("15"))
                                            {
                                                antNum |= eAntennaNo._15;
                                            }
                                            else if (readKey.Equals("16"))
                                            {
                                                antNum |= eAntennaNo._16;
                                            }
                                            else if (readKey.Equals("17"))
                                            {
                                                antNum |= eAntennaNo._17;
                                            }
                                            else if (readKey.Equals("18"))
                                            {
                                                antNum |= eAntennaNo._18;
                                            }
                                            else if (readKey.Equals("19"))
                                            {
                                                antNum |= eAntennaNo._19;
                                            }
                                            else if (readKey.Equals("20"))
                                            {
                                                antNum |= eAntennaNo._20;
                                            }
                                            else if (readKey.Equals("21"))
                                            {
                                                antNum |= eAntennaNo._21;
                                            }
                                            else if (readKey.Equals("22"))
                                            {
                                                antNum |= eAntennaNo._22;
                                            }
                                            else if (readKey.Equals("23"))
                                            {
                                                antNum |= eAntennaNo._23;
                                            }
                                            else if (readKey.Equals("24"))
                                            {
                                                antNum |= eAntennaNo._24;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = 0;
                                            }
                                        }

                                        Console.WriteLine("Please input destroyPassword:");
                                        String destroyPassword = Console.ReadLine();


                                        if (My_Reader._Tag6C.Destroy(ConnID, antNum, destroyPassword) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }

                                    }
                                    else if (readKey.Equals("2"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else if (readKey.Equals("9"))
                                            {
                                                antNum |= eAntennaNo._9;
                                            }
                                            else if (readKey.Equals("10"))
                                            {
                                                antNum |= eAntennaNo._10;
                                            }
                                            else if (readKey.Equals("11"))
                                            {
                                                antNum |= eAntennaNo._11;
                                            }
                                            else if (readKey.Equals("12"))
                                            {
                                                antNum |= eAntennaNo._12;
                                            }
                                            else if (readKey.Equals("13"))
                                            {
                                                antNum |= eAntennaNo._13;
                                            }
                                            else if (readKey.Equals("14"))
                                            {
                                                antNum |= eAntennaNo._14;
                                            }
                                            else if (readKey.Equals("15"))
                                            {
                                                antNum |= eAntennaNo._15;
                                            }
                                            else if (readKey.Equals("16"))
                                            {
                                                antNum |= eAntennaNo._16;
                                            }
                                            else if (readKey.Equals("17"))
                                            {
                                                antNum |= eAntennaNo._17;
                                            }
                                            else if (readKey.Equals("18"))
                                            {
                                                antNum |= eAntennaNo._18;
                                            }
                                            else if (readKey.Equals("19"))
                                            {
                                                antNum |= eAntennaNo._19;
                                            }
                                            else if (readKey.Equals("20"))
                                            {
                                                antNum |= eAntennaNo._20;
                                            }
                                            else if (readKey.Equals("21"))
                                            {
                                                antNum |= eAntennaNo._21;
                                            }
                                            else if (readKey.Equals("22"))
                                            {
                                                antNum |= eAntennaNo._22;
                                            }
                                            else if (readKey.Equals("23"))
                                            {
                                                antNum |= eAntennaNo._23;
                                            }
                                            else if (readKey.Equals("24"))
                                            {
                                                antNum |= eAntennaNo._24;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = 0;
                                            }
                                        }

                                        Console.WriteLine("Please input destroyPassword:");
                                        String destroyPassword = Console.ReadLine();

                                        Console.WriteLine("Please input sMatchData:");
                                        String sMatchData = Console.ReadLine();

                                        Console.WriteLine("Please input matchWordStartIndex:");
                                        int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                        if (My_Reader._Tag6C.Destroy_MatchEPC(ConnID, antNum, destroyPassword, sMatchData, matchWordStartIndex) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("3"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else if (readKey.Equals("9"))
                                            {
                                                antNum |= eAntennaNo._9;
                                            }
                                            else if (readKey.Equals("10"))
                                            {
                                                antNum |= eAntennaNo._10;
                                            }
                                            else if (readKey.Equals("11"))
                                            {
                                                antNum |= eAntennaNo._11;
                                            }
                                            else if (readKey.Equals("12"))
                                            {
                                                antNum |= eAntennaNo._12;
                                            }
                                            else if (readKey.Equals("13"))
                                            {
                                                antNum |= eAntennaNo._13;
                                            }
                                            else if (readKey.Equals("14"))
                                            {
                                                antNum |= eAntennaNo._14;
                                            }
                                            else if (readKey.Equals("15"))
                                            {
                                                antNum |= eAntennaNo._15;
                                            }
                                            else if (readKey.Equals("16"))
                                            {
                                                antNum |= eAntennaNo._16;
                                            }
                                            else if (readKey.Equals("17"))
                                            {
                                                antNum |= eAntennaNo._17;
                                            }
                                            else if (readKey.Equals("18"))
                                            {
                                                antNum |= eAntennaNo._18;
                                            }
                                            else if (readKey.Equals("19"))
                                            {
                                                antNum |= eAntennaNo._19;
                                            }
                                            else if (readKey.Equals("20"))
                                            {
                                                antNum |= eAntennaNo._20;
                                            }
                                            else if (readKey.Equals("21"))
                                            {
                                                antNum |= eAntennaNo._21;
                                            }
                                            else if (readKey.Equals("22"))
                                            {
                                                antNum |= eAntennaNo._22;
                                            }
                                            else if (readKey.Equals("23"))
                                            {
                                                antNum |= eAntennaNo._23;
                                            }
                                            else if (readKey.Equals("24"))
                                            {
                                                antNum |= eAntennaNo._24;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = 0;
                                            }
                                        }

                                        Console.WriteLine("Please input destroyPassword:");
                                        String destroyPassword = Console.ReadLine();

                                        Console.WriteLine("Please input sMatchData:");
                                        String sMatchData = Console.ReadLine();

                                        Console.WriteLine("Please input matchWordStartIndex:");
                                        int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                        if (My_Reader._Tag6C.Destroy_MatchTID(ConnID, antNum, destroyPassword, sMatchData, matchWordStartIndex) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("b"))
                                    {
                                        break;
                                    }
                                    else if (readKey.Equals("q"))
                                    {
                                        My_Reader._Config.Stop(ConnID);
                                        My_Reader.CloseConn(ConnID);
                                        return;
                                    }
                                }
                            }

                            else if (readKey.Equals("s"))
                            {
                                My_Reader._Config.Stop(ConnID);
                            }
                            else if (readKey.Equals("b"))
                            {
                                break;
                            }
                            else if (readKey.Equals("s"))
                            {
                                My_Reader._Config.Stop(ConnID);
                            }
                            else if (readKey.Equals("q"))
                            {
                                My_Reader._Config.Stop(ConnID);
                                My_Reader.CloseConn(ConnID);
                                return;
                            }
                        }
                    }


                    #endregion

                    #region 6B Tag

                    else if (readKey.Equals("6"))
                    {
                        while (true)
                        {
                            Console.WriteLine("Current ConnID : " + ConnID + "\n");
                            Console.WriteLine("Please select operation:");
                            Console.WriteLine("1.Read 6B Tag");
                            Console.WriteLine("2.Write 6B Tag");
                            Console.WriteLine("3.Lock 6B Tag");
                            Console.WriteLine("4.Get 6B Tag LockState");
                            Console.WriteLine("b.Back to Previous Menu");
                            Console.WriteLine("q.Quit");

                            readKey = Console.ReadLine();

                            if (readKey.Equals("1"))
                            {
                                while (true)
                                {
                                    Console.WriteLine("Please select method:");
                                    Console.WriteLine("1.Get6B(String ConnID, eAntennaNo antNum, eReadType readType, e6BReaderContent readerContent)");
                                    Console.WriteLine("2.Get6B_UserData(String ConnID, eAntennaNo antNum, eReadType readType, e6BReaderContent readerContent, Int32 readStart, Int32 readLen)");
                                    Console.WriteLine("3.Get6B_UserData_MatchTID(String ConnID, eAntennaNo antNum, eReadType readType, e6BReaderContent readerContent, Int32 readStart, Int32 readLen, String sMatchData)");
                                    Console.WriteLine("s.Stop Read");
                                    Console.WriteLine("b.Back to Previous Menu");
                                    Console.WriteLine("q.Quit");

                                    readKey = Console.ReadLine();

                                    if (readKey.Equals("1"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = new eAntennaNo();
                                            }
                                        }

                                        eReadType readType = new eReadType();
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        e6BReaderContent readerContent = new e6BReaderContent();
                                        Console.WriteLine("Please select readerContent:");
                                        Console.WriteLine("e6BReaderContent.TID");
                                        Console.WriteLine("e6BReaderContent.UserData");
                                        Console.WriteLine("e6BReaderContent.TID_UserData");
                                        if (readKey.Equals("1"))
                                        {
                                            readerContent = e6BReaderContent.TID;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readerContent = e6BReaderContent.UserData;
                                        }
                                        else if (readKey.Equals("3"))
                                        {
                                            readerContent = e6BReaderContent.TID_UserData;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        if (My_Reader._Tag6B.Get6B(ConnID, antNum, readType, readerContent) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("2"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = new eAntennaNo();
                                            }
                                        }

                                        eReadType readType = new eReadType();
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        e6BReaderContent readerContent = new e6BReaderContent();
                                        Console.WriteLine("Please select readerContent:");
                                        Console.WriteLine("e6BReaderContent.TID");
                                        Console.WriteLine("e6BReaderContent.UserData");
                                        Console.WriteLine("e6BReaderContent.TID_UserData");
                                        if (readKey.Equals("1"))
                                        {
                                            readerContent = e6BReaderContent.TID;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readerContent = e6BReaderContent.UserData;
                                        }
                                        else if (readKey.Equals("3"))
                                        {
                                            readerContent = e6BReaderContent.TID_UserData;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input readerStart");
                                        int readerStart = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input readerLen");
                                        int readerLen = Convert.ToInt32(Console.ReadLine());

                                        if (My_Reader._Tag6B.Get6B_UserData(ConnID, antNum, readType, readerContent, readerStart, readerLen) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("3"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = new eAntennaNo();
                                            }
                                        }

                                        eReadType readType = new eReadType();
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        e6BReaderContent readerContent = new e6BReaderContent();
                                        Console.WriteLine("Please select readerContent:");
                                        Console.WriteLine("e6BReaderContent.TID");
                                        Console.WriteLine("e6BReaderContent.UserData");
                                        Console.WriteLine("e6BReaderContent.TID_UserData");
                                        if (readKey.Equals("1"))
                                        {
                                            readerContent = e6BReaderContent.TID;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readerContent = e6BReaderContent.UserData;
                                        }
                                        else if (readKey.Equals("3"))
                                        {
                                            readerContent = e6BReaderContent.TID_UserData;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input readerStart");
                                        int readerStart = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input readerLen");
                                        int readerLen = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input sMatchData");
                                        string sMatchData = Console.ReadLine();

                                        if (My_Reader._Tag6B.Get6B_UserData_MatchTID(ConnID, antNum, readType, readerContent, readerStart, readerLen, sMatchData) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("b"))
                                    {
                                        break;
                                    }
                                    else if (readKey.Equals("q"))
                                    {
                                        return;
                                    }
                                }
                            }
                            else if (readKey.Equals("2"))
                            {
                                eAntennaNo antNum = new eAntennaNo();
                                while (true)
                                {
                                    Console.WriteLine("Please input antenna number:");
                                    readKey = Console.ReadLine();
                                    if (readKey.Equals("1"))
                                    {
                                        antNum |= eAntennaNo._1;
                                    }
                                    else if (readKey.Equals("2"))
                                    {
                                        antNum |= eAntennaNo._2;
                                    }
                                    else if (readKey.Equals("3"))
                                    {
                                        antNum |= eAntennaNo._3;
                                    }
                                    else if (readKey.Equals("4"))
                                    {
                                        antNum |= eAntennaNo._4;
                                    }
                                    else if (readKey.Equals("5"))
                                    {
                                        antNum |= eAntennaNo._5;
                                    }
                                    else if (readKey.Equals("6"))
                                    {
                                        antNum |= eAntennaNo._6;
                                    }
                                    else if (readKey.Equals("7"))
                                    {
                                        antNum |= eAntennaNo._7;
                                    }
                                    else if (readKey.Equals("8"))
                                    {
                                        antNum |= eAntennaNo._8;
                                    }
                                    else
                                    {
                                        Console.WriteLine("antenna number error");
                                        continue;
                                    }

                                    Console.WriteLine("Setting another antenna");
                                    Console.WriteLine("1.Yes");
                                    Console.WriteLine("2.No");
                                    readKey = Console.ReadLine();
                                    if (readKey.Equals("1"))
                                    {
                                        continue;
                                    }
                                    else if (readKey.Equals("2"))
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Parameter error ,please reset");
                                        antNum = new eAntennaNo();
                                    }
                                }

                                Console.WriteLine("Please input sTID");
                                string sTID = Console.ReadLine();

                                Console.WriteLine("Please input startIndex");
                                int startIndex = Convert.ToInt32(Console.ReadLine());

                                Console.WriteLine("Please input sWriteData");
                                string sWriteData = Console.ReadLine();

                                if (My_Reader._Tag6B.Write6B(ConnID, antNum, sTID, startIndex, sWriteData) == 0)
                                {
                                    Console.WriteLine("Success!");
                                }
                                else
                                {
                                    Console.WriteLine("Failure!");
                                }
                            }
                            else if (readKey.Equals("3"))
                            {
                                eAntennaNo antNum = new eAntennaNo();
                                while (true)
                                {
                                    Console.WriteLine("Please input antenna number:");
                                    readKey = Console.ReadLine();
                                    if (readKey.Equals("1"))
                                    {
                                        antNum |= eAntennaNo._1;
                                    }
                                    else if (readKey.Equals("2"))
                                    {
                                        antNum |= eAntennaNo._2;
                                    }
                                    else if (readKey.Equals("3"))
                                    {
                                        antNum |= eAntennaNo._3;
                                    }
                                    else if (readKey.Equals("4"))
                                    {
                                        antNum |= eAntennaNo._4;
                                    }
                                    else if (readKey.Equals("5"))
                                    {
                                        antNum |= eAntennaNo._5;
                                    }
                                    else if (readKey.Equals("6"))
                                    {
                                        antNum |= eAntennaNo._6;
                                    }
                                    else if (readKey.Equals("7"))
                                    {
                                        antNum |= eAntennaNo._7;
                                    }
                                    else if (readKey.Equals("8"))
                                    {
                                        antNum |= eAntennaNo._8;
                                    }
                                    else
                                    {
                                        Console.WriteLine("antenna number error");
                                        continue;
                                    }

                                    Console.WriteLine("Setting another antenna");
                                    Console.WriteLine("1.Yes");
                                    Console.WriteLine("2.No");
                                    readKey = Console.ReadLine();
                                    if (readKey.Equals("1"))
                                    {
                                        continue;
                                    }
                                    else if (readKey.Equals("2"))
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Parameter error ,please reset");
                                        antNum = new eAntennaNo();
                                    }
                                }

                                Console.WriteLine("Please input sTID");
                                string sTID = Console.ReadLine();

                                Console.WriteLine("Please input lockIndex");
                                int lockIndex = Convert.ToInt32(Console.ReadLine());

                                if (My_Reader._Tag6B.Lock6B(ConnID, antNum, sTID, lockIndex) == 0)
                                {
                                    Console.WriteLine("Success!");
                                }
                                else
                                {
                                    Console.WriteLine("Failure!");
                                }
                            }
                            else if (readKey.Equals("4"))
                            {
                                eAntennaNo antNum = new eAntennaNo();
                                while (true)
                                {
                                    Console.WriteLine("Please input antenna number:");
                                    readKey = Console.ReadLine();
                                    if (readKey.Equals("1"))
                                    {
                                        antNum |= eAntennaNo._1;
                                    }
                                    else if (readKey.Equals("2"))
                                    {
                                        antNum |= eAntennaNo._2;
                                    }
                                    else if (readKey.Equals("3"))
                                    {
                                        antNum |= eAntennaNo._3;
                                    }
                                    else if (readKey.Equals("4"))
                                    {
                                        antNum |= eAntennaNo._4;
                                    }
                                    else if (readKey.Equals("5"))
                                    {
                                        antNum |= eAntennaNo._5;
                                    }
                                    else if (readKey.Equals("6"))
                                    {
                                        antNum |= eAntennaNo._6;
                                    }
                                    else if (readKey.Equals("7"))
                                    {
                                        antNum |= eAntennaNo._7;
                                    }
                                    else if (readKey.Equals("8"))
                                    {
                                        antNum |= eAntennaNo._8;
                                    }
                                    else
                                    {
                                        Console.WriteLine("antenna number error");
                                        continue;
                                    }

                                    Console.WriteLine("Setting another antenna");
                                    Console.WriteLine("1.Yes");
                                    Console.WriteLine("2.No");
                                    readKey = Console.ReadLine();
                                    if (readKey.Equals("1"))
                                    {
                                        continue;
                                    }
                                    else if (readKey.Equals("2"))
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Parameter error ,please reset");
                                        antNum = new eAntennaNo();
                                    }
                                }

                                Console.WriteLine("Please input sTID");
                                string sTID = Console.ReadLine();

                                Console.WriteLine("Please input lockIndex");
                                int lockIndex = Convert.ToInt32(Console.ReadLine());

                                Console.WriteLine(My_Reader._Tag6B.GetLock6B_State(ConnID, antNum, sTID, lockIndex));

                            }
                            else if (readKey.Equals("s"))
                            {
                                My_Reader._Config.Stop(ConnID);
                            }
                            else if (readKey.Equals("b"))
                            {
                                break;
                            }
                            else if (readKey.Equals("q"))
                            {
                                return;
                            }

                        }
                    }

                    #endregion

                    #region GB Tag

                    else if (readKey.Equals("7"))
                    {
                        while (true)
                        {
                            Console.WriteLine("Current ConnID : " + ConnID + "\n");
                            Console.WriteLine("Please select operation:");
                            Console.WriteLine("1.Read GB Tag");
                            Console.WriteLine("2.Write GB Tag");
                            Console.WriteLine("3.Lock GB Tag");
                            Console.WriteLine("4.Kill GB Tag");
                            Console.WriteLine("b.Back to Previous Menu");
                            Console.WriteLine("q.Quit");

                            readKey = Console.ReadLine();

                            #region Read GB

                            if (readKey.Equals("1"))
                            {
                                while (true)
                                {
                                    Console.WriteLine("Please select method:");
                                    Console.WriteLine("1.GetEPC(String ConnID, eAntennaNo antNum, eReadType readType)");
                                    Console.WriteLine("2.GetEPC(String ConnID, eAntennaNo antNum, eReadType readType, String accessPassword)");
                                    Console.WriteLine("3.GetEPC_MatchEPC(String ConnID, eAntennaNo antNum, eReadType readType, String sEPC)");
                                    Console.WriteLine("4.GetEPC_MatchEPC(String ConnID, eAntennaNo antNum, eReadType readType, String sEPC, Int32 matchWordStartIndex)");
                                    Console.WriteLine("5.GetEPC_MatchEPC(String ConnID, eAntennaNo antNum, eReadType readType, String sEPC, Int32 matchWordStartIndex, String accessPassword)");
                                    Console.WriteLine("6.GetEPC_MatchTID(String ConnID, eAntennaNo antNum, eReadType readType, String sTID)");
                                    Console.WriteLine("7.GetEPC_MatchTID(String ConnID, eAntennaNo antNum, eReadType readType, String sTID, Int32 matchWordStartIndex)");
                                    Console.WriteLine("8.GetEPC_MatchTID(String ConnID, eAntennaNo antNum, eReadType readType, String sTID, Int32 matchWordStartIndex, String accessPassword)");
                                    Console.WriteLine("9.GetEPC_TID(String ConnID, eAntennaNo antNum, eReadType readType)");
                                    Console.WriteLine("10.GetEPC_TID(String ConnID, eAntennaNo antNum, eReadType readType, String accessPassword)");
                                    Console.WriteLine("11.GetEPC_TID_MatchEPC(String ConnID, eAntennaNo antNum, eReadType readType, String sEPC)");
                                    Console.WriteLine("12.GetEPC_TID_MatchEPC(String ConnID, eAntennaNo antNum, eReadType readType, String sEPC, Int32 matchWordStartIndex)");
                                    Console.WriteLine("13.GetEPC_TID_MatchEPC(String ConnID, eAntennaNo antNum, eReadType readType, String sEPC, Int32 matchWordStartIndex, String accessPassword)");
                                    Console.WriteLine("14.GetEPC_TID_MatchTID(String ConnID, eAntennaNo antNum, eReadType readType, String sTID)");
                                    Console.WriteLine("15.GetEPC_TID_MatchTID(String ConnID, eAntennaNo antNum, eReadType readType, String sTID, Int32 matchWordStartIndex)");
                                    Console.WriteLine("16.GetEPC_TID_MatchTID(String ConnID, eAntennaNo antNum, eReadType readType, String sTID, Int32 matchWordStartIndex, String accessPassword)");
                                    Console.WriteLine("17.GetEPC_TID_UserData(String ConnID, eAntennaNo antNum, eReadType readType,Int32 readStart, Int32 readLen)");
                                    Console.WriteLine("18.GetEPC_TID_UserData(String ConnID, eAntennaNo antNum, eReadType readType,Int32 readStart, Int32 readLen, String accessPassword)");
                                    Console.WriteLine("19.GetEPC_TID_UserData_MatchEPC(String ConnID, eAntennaNo antNum, eReadType readType,Int32 readStart, Int32 readLen, String sEPC)");
                                    Console.WriteLine("20.GetEPC_TID_UserData_MatchEPC(String ConnID, eAntennaNo antNum, eReadType readType,Int32 readStart, Int32 readLen, String sEPC, Int32 matchWordStartIndex)");
                                    Console.WriteLine("21.GetEPC_TID_UserData_MatchEPC(String ConnID, eAntennaNo antNum, eReadType readType,Int32 readStart, Int32 readLen, String sEPC, Int32 matchWordStartIndex, String accessPassword)");
                                    Console.WriteLine("22.GetEPC_TID_UserData_MatchTID(String ConnID, eAntennaNo antNum, eReadType readType,Int32 readStart, Int32 readLen, String sTID)");
                                    Console.WriteLine("23.GetEPC_TID_UserData_MatchTID(String ConnID, eAntennaNo antNum, eReadType readType,Int32 readStart, Int32 readLen, String sTID, Int32 matchWordStartIndex)");
                                    Console.WriteLine("24.GetEPC_TID_UserData_MatchTID(String ConnID, eAntennaNo antNum, eReadType readType,Int32 readStart, Int32 readLen, String sTID, Int32 matchWordStartIndex, String accessPassword)");
                                    Console.WriteLine("s.Stop Read");
                                    Console.WriteLine("b.Back to Previous Menu");
                                    Console.WriteLine("q.Quit");

                                    readKey = Console.ReadLine();

                                    if (readKey.Equals("1"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = new eAntennaNo();
                                            }
                                        }

                                        eReadType readType = new eReadType();
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        if (My_Reader._TagGB.GetEPC(ConnID, antNum, readType) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }

                                    }
                                    else if (readKey.Equals("2"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = new eAntennaNo();
                                            }
                                        }

                                        eReadType readType = new eReadType();
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input accessPassword:");
                                        string accessPassword = Console.ReadLine();

                                        if (My_Reader._TagGB.GetEPC(ConnID, antNum, readType, accessPassword) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }

                                    }
                                    else if (readKey.Equals("3"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = new eAntennaNo();
                                            }
                                        }

                                        eReadType readType = new eReadType();
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input sEPC:");
                                        string sEPC = Console.ReadLine();

                                        if (My_Reader._TagGB.GetEPC_MatchEPC(ConnID, antNum, readType, sEPC) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("4"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = new eAntennaNo();
                                            }
                                        }

                                        eReadType readType = new eReadType();
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input sEPC:");
                                        string sEPC = Console.ReadLine();

                                        Console.WriteLine("Please input matchWordStartIndex:");
                                        int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                        if (My_Reader._TagGB.GetEPC_MatchEPC(ConnID, antNum, readType, sEPC, matchWordStartIndex) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("5"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = new eAntennaNo();
                                            }
                                        }

                                        eReadType readType = new eReadType();
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input sEPC:");
                                        string sEPC = Console.ReadLine();

                                        Console.WriteLine("Please input matchWordStartIndex:");
                                        int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input accessPassword:");
                                        string accessPassword = Console.ReadLine();

                                        if (My_Reader._TagGB.GetEPC_MatchEPC(ConnID, antNum, readType, sEPC, matchWordStartIndex, accessPassword) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("6"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = new eAntennaNo();
                                            }
                                        }

                                        eReadType readType = new eReadType();
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input sTID:");
                                        string sTID = Console.ReadLine();

                                        if (My_Reader._TagGB.GetEPC_MatchTID(ConnID, antNum, readType, sTID) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("7"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = new eAntennaNo();
                                            }
                                        }

                                        eReadType readType = new eReadType();
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input sTID:");
                                        string sTID = Console.ReadLine();

                                        Console.WriteLine("Please input matchWordStartIndex:");
                                        int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                        if (My_Reader._TagGB.GetEPC_MatchTID(ConnID, antNum, readType, sTID, matchWordStartIndex) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("8"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = new eAntennaNo();
                                            }
                                        }

                                        eReadType readType = new eReadType();
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input sTID:");
                                        string sTID = Console.ReadLine();

                                        Console.WriteLine("Please input matchWordStartIndex:");
                                        int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input accessPassword:");
                                        string accessPassword = Console.ReadLine();

                                        if (My_Reader._TagGB.GetEPC_MatchTID(ConnID, antNum, readType, sTID, matchWordStartIndex, accessPassword) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("9"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = new eAntennaNo();
                                            }
                                        }

                                        eReadType readType = new eReadType();
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        if (My_Reader._TagGB.GetEPC_TID(ConnID, antNum, readType) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("10"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = new eAntennaNo();
                                            }
                                        }

                                        eReadType readType = new eReadType();
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input accessPassword:");
                                        string accessPassword = Console.ReadLine();

                                        if (My_Reader._TagGB.GetEPC_TID(ConnID, antNum, readType, accessPassword) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("11"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = new eAntennaNo();
                                            }
                                        }

                                        eReadType readType = new eReadType();
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input sEPC:");
                                        string sEPC = Console.ReadLine();

                                        if (My_Reader._TagGB.GetEPC_TID_MatchEPC(ConnID, antNum, readType, sEPC) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("12"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = new eAntennaNo();
                                            }
                                        }

                                        eReadType readType = new eReadType();
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input sEPC:");
                                        string sEPC = Console.ReadLine();

                                        Console.WriteLine("Please input matchWordStartIndex:");
                                        int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                        if (My_Reader._TagGB.GetEPC_TID_MatchEPC(ConnID, antNum, readType, sEPC, matchWordStartIndex) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("13"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = new eAntennaNo();
                                            }
                                        }

                                        eReadType readType = new eReadType();
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input sEPC:");
                                        string sEPC = Console.ReadLine();

                                        Console.WriteLine("Please input matchWordStartIndex:");
                                        int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input accessPassword:");
                                        string accessPassword = Console.ReadLine();

                                        if (My_Reader._TagGB.GetEPC_TID_MatchEPC(ConnID, antNum, readType, sEPC, matchWordStartIndex, accessPassword) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("14"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = new eAntennaNo();
                                            }
                                        }

                                        eReadType readType = new eReadType();
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input sTID:");
                                        string sTID = Console.ReadLine();

                                        if (My_Reader._TagGB.GetEPC_TID_MatchTID(ConnID, antNum, readType, sTID) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("15"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = new eAntennaNo();
                                            }
                                        }

                                        eReadType readType = new eReadType();
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input sTID:");
                                        string sTID = Console.ReadLine();

                                        Console.WriteLine("Please input matchWordStartIndex:");
                                        int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                        if (My_Reader._TagGB.GetEPC_TID_MatchTID(ConnID, antNum, readType, sTID, matchWordStartIndex) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }

                                    }
                                    else if (readKey.Equals("16"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = new eAntennaNo();
                                            }
                                        }

                                        eReadType readType = new eReadType();
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input sTID:");
                                        string sTID = Console.ReadLine();

                                        Console.WriteLine("Please input matchWordStartIndex:");
                                        int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input accessPassword:");
                                        string accessPassword = Console.ReadLine();

                                        if (My_Reader._TagGB.GetEPC_TID_MatchTID(ConnID, antNum, readType, sTID, matchWordStartIndex, accessPassword) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("17"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = new eAntennaNo();
                                            }
                                        }

                                        eReadType readType = new eReadType();
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input readStart:");
                                        int readStart = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input readLen:");
                                        int readLen = Convert.ToInt32(Console.ReadLine());

                                        if (My_Reader._TagGB.GetEPC_TID_UserData(ConnID, antNum, readType, readStart, readLen) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("18"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = new eAntennaNo();
                                            }
                                        }

                                        eReadType readType = new eReadType();
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input readStart:");
                                        int readStart = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input readLen:");
                                        int readLen = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input accessPassword:");
                                        string accessPassword = Console.ReadLine();

                                        if (My_Reader._TagGB.GetEPC_TID_UserData(ConnID, antNum, readType, readStart, readLen, accessPassword) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("19"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = new eAntennaNo();
                                            }
                                        }

                                        eReadType readType = new eReadType();
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input readStart:");
                                        int readStart = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input readLen:");
                                        int readLen = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input sEPC:");
                                        string sEPC = Console.ReadLine();

                                        if (My_Reader._TagGB.GetEPC_TID_UserData_MatchEPC(ConnID, antNum, readType, readStart, readLen, sEPC) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("20"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = new eAntennaNo();
                                            }
                                        }

                                        eReadType readType = new eReadType();
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input readStart:");
                                        int readStart = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input readLen:");
                                        int readLen = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input sEPC:");
                                        string sEPC = Console.ReadLine();

                                        Console.WriteLine("Please input matchWordStartIndex:");
                                        int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                        if (My_Reader._TagGB.GetEPC_TID_UserData_MatchEPC(ConnID, antNum, readType, readStart, readLen, sEPC, matchWordStartIndex) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("21"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = new eAntennaNo();
                                            }
                                        }

                                        eReadType readType = new eReadType();
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input readStart:");
                                        int readStart = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input readLen:");
                                        int readLen = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input sEPC:");
                                        string sEPC = Console.ReadLine();

                                        Console.WriteLine("Please input matchWordStartIndex:");
                                        int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input accessPassword:");
                                        string accessPassword = Console.ReadLine();

                                        if (My_Reader._TagGB.GetEPC_TID_UserData_MatchEPC(ConnID, antNum, readType, readStart, readLen, sEPC, matchWordStartIndex, accessPassword) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("22"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = new eAntennaNo();
                                            }
                                        }

                                        eReadType readType = new eReadType();
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input readStart:");
                                        int readStart = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input readLen:");
                                        int readLen = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input sTID:");
                                        string sTID = Console.ReadLine();

                                        if (My_Reader._TagGB.GetEPC_TID_UserData_MatchTID(ConnID, antNum, readType, readStart, readLen, sTID) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("23"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = new eAntennaNo();
                                            }
                                        }

                                        eReadType readType = new eReadType();
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input readStart:");
                                        int readStart = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input readLen:");
                                        int readLen = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input sTID:");
                                        string sTID = Console.ReadLine();

                                        Console.WriteLine("Please input matchWordStartIndex:");
                                        int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                        if (My_Reader._TagGB.GetEPC_TID_UserData_MatchTID(ConnID, antNum, readType, readStart, readLen, sTID, matchWordStartIndex) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("24"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = new eAntennaNo();
                                            }
                                        }

                                        eReadType readType = new eReadType();
                                        Console.WriteLine("Please select ReadType:");
                                        Console.WriteLine("1.eReadType.Single");
                                        Console.WriteLine("2.eReadType.Inventory");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            readType = eReadType.Single;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            readType = eReadType.Inventory;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ReadType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input readStart:");
                                        int readStart = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input readLen:");
                                        int readLen = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input sTID:");
                                        string sTID = Console.ReadLine();

                                        Console.WriteLine("Please input matchWordStartIndex:");
                                        int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input accessPassword:");
                                        string accessPassword = Console.ReadLine();

                                        if (My_Reader._TagGB.GetEPC_TID_UserData_MatchTID(ConnID, antNum, readType, readStart, readLen, sTID, matchWordStartIndex, accessPassword) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("b"))
                                    {
                                        break;
                                    }
                                    else if (readKey.Equals("q"))
                                    {
                                        return;
                                    }
                                }
                            }

                            #endregion

                            #region Write GB
                            else if (readKey.Equals("2"))
                            {
                                while (true)
                                {
                                    Console.WriteLine("Please select operation:");
                                    Console.WriteLine("1.Write EPC");
                                    Console.WriteLine("2.Write UserData");
                                    Console.WriteLine("b.Back to Previous Menu");
                                    Console.WriteLine("q.Quit");

                                    readKey = Console.ReadLine();
                                    if (readKey.Equals("1"))
                                    {
                                        Console.WriteLine("Please select method:");
                                        Console.WriteLine("1.WriteEPC(String ConnID, eAntennaNo antNum, String sWriteData)");
                                        Console.WriteLine("2.WriteEPC_MatchEPC(String ConnID, eAntennaNo antNum, String sWriteData, String sMatchData, Int32 matchWordStartIndex) ");
                                        Console.WriteLine("3.WriteEPC_MatchEPC(String ConnID, eAntennaNo antNum, String sWriteData, String sMatchData, Int32 matchWordStartIndex, String accessPassword) ");
                                        Console.WriteLine("4.WriteEPC_MatchTID(String ConnID, eAntennaNo antNum, String sWriteData, String sMatchData, Int32 matchWordStartIndex) ");
                                        Console.WriteLine("5.WriteEPC_MatchTID(String ConnID, eAntennaNo antNum, String sWriteData, String sMatchData, Int32 matchWordStartIndex, String accessPassword) ");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            eAntennaNo antNum = new eAntennaNo();
                                            while (true)
                                            {
                                                Console.WriteLine("Please input antenna number:");
                                                readKey = Console.ReadLine();
                                                if (readKey.Equals("1"))
                                                {
                                                    antNum |= eAntennaNo._1;
                                                }
                                                else if (readKey.Equals("2"))
                                                {
                                                    antNum |= eAntennaNo._2;
                                                }
                                                else if (readKey.Equals("3"))
                                                {
                                                    antNum |= eAntennaNo._3;
                                                }
                                                else if (readKey.Equals("4"))
                                                {
                                                    antNum |= eAntennaNo._4;
                                                }
                                                else if (readKey.Equals("5"))
                                                {
                                                    antNum |= eAntennaNo._5;
                                                }
                                                else if (readKey.Equals("6"))
                                                {
                                                    antNum |= eAntennaNo._6;
                                                }
                                                else if (readKey.Equals("7"))
                                                {
                                                    antNum |= eAntennaNo._7;
                                                }
                                                else if (readKey.Equals("8"))
                                                {
                                                    antNum |= eAntennaNo._8;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("antenna number error");
                                                    continue;
                                                }

                                                Console.WriteLine("Setting another antenna");
                                                Console.WriteLine("1.Yes");
                                                Console.WriteLine("2.No");
                                                readKey = Console.ReadLine();
                                                if (readKey.Equals("1"))
                                                {
                                                    continue;
                                                }
                                                else if (readKey.Equals("2"))
                                                {
                                                    break;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Parameter error ,please reset");
                                                    antNum = new eAntennaNo();
                                                }
                                            }

                                            Console.WriteLine("Please input sWriteData:");
                                            string sWriteData = Console.ReadLine();


                                            if (My_Reader._TagGB.WriteEPC(ConnID, antNum, sWriteData) == 0)
                                            {
                                                Console.WriteLine("Success!");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Failure!");
                                            }

                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            eAntennaNo antNum = new eAntennaNo();
                                            while (true)
                                            {
                                                Console.WriteLine("Please input antenna number:");
                                                readKey = Console.ReadLine();
                                                if (readKey.Equals("1"))
                                                {
                                                    antNum |= eAntennaNo._1;
                                                }
                                                else if (readKey.Equals("2"))
                                                {
                                                    antNum |= eAntennaNo._2;
                                                }
                                                else if (readKey.Equals("3"))
                                                {
                                                    antNum |= eAntennaNo._3;
                                                }
                                                else if (readKey.Equals("4"))
                                                {
                                                    antNum |= eAntennaNo._4;
                                                }
                                                else if (readKey.Equals("5"))
                                                {
                                                    antNum |= eAntennaNo._5;
                                                }
                                                else if (readKey.Equals("6"))
                                                {
                                                    antNum |= eAntennaNo._6;
                                                }
                                                else if (readKey.Equals("7"))
                                                {
                                                    antNum |= eAntennaNo._7;
                                                }
                                                else if (readKey.Equals("8"))
                                                {
                                                    antNum |= eAntennaNo._8;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("antenna number error");
                                                    continue;
                                                }

                                                Console.WriteLine("Setting another antenna");
                                                Console.WriteLine("1.Yes");
                                                Console.WriteLine("2.No");
                                                readKey = Console.ReadLine();
                                                if (readKey.Equals("1"))
                                                {
                                                    continue;
                                                }
                                                else if (readKey.Equals("2"))
                                                {
                                                    break;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Parameter error ,please reset");
                                                    antNum = new eAntennaNo();
                                                }
                                            }

                                            Console.WriteLine("Please input sWriteData:");
                                            string sWriteData = Console.ReadLine();

                                            Console.WriteLine("Please input sMatchData:");
                                            string sMatchData = Console.ReadLine();

                                            Console.WriteLine("Please input matchWordStartIndex:");
                                            int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                            if (My_Reader._TagGB.WriteEPC_MatchEPC(ConnID, antNum, sWriteData, sMatchData, matchWordStartIndex) == 0)
                                            {
                                                Console.WriteLine("Success!");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Failure!");
                                            }
                                        }
                                        else if (readKey.Equals("3"))
                                        {
                                            eAntennaNo antNum = new eAntennaNo();
                                            while (true)
                                            {
                                                Console.WriteLine("Please input antenna number:");
                                                readKey = Console.ReadLine();
                                                if (readKey.Equals("1"))
                                                {
                                                    antNum |= eAntennaNo._1;
                                                }
                                                else if (readKey.Equals("2"))
                                                {
                                                    antNum |= eAntennaNo._2;
                                                }
                                                else if (readKey.Equals("3"))
                                                {
                                                    antNum |= eAntennaNo._3;
                                                }
                                                else if (readKey.Equals("4"))
                                                {
                                                    antNum |= eAntennaNo._4;
                                                }
                                                else if (readKey.Equals("5"))
                                                {
                                                    antNum |= eAntennaNo._5;
                                                }
                                                else if (readKey.Equals("6"))
                                                {
                                                    antNum |= eAntennaNo._6;
                                                }
                                                else if (readKey.Equals("7"))
                                                {
                                                    antNum |= eAntennaNo._7;
                                                }
                                                else if (readKey.Equals("8"))
                                                {
                                                    antNum |= eAntennaNo._8;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("antenna number error");
                                                    continue;
                                                }

                                                Console.WriteLine("Setting another antenna");
                                                Console.WriteLine("1.Yes");
                                                Console.WriteLine("2.No");
                                                readKey = Console.ReadLine();
                                                if (readKey.Equals("1"))
                                                {
                                                    continue;
                                                }
                                                else if (readKey.Equals("2"))
                                                {
                                                    break;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Parameter error ,please reset");
                                                    antNum = new eAntennaNo();
                                                }
                                            }

                                            Console.WriteLine("Please input sWriteData:");
                                            string sWriteData = Console.ReadLine();

                                            Console.WriteLine("Please input sMatchData:");
                                            string sMatchData = Console.ReadLine();

                                            Console.WriteLine("Please input matchWordStartIndex:");
                                            int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                            Console.WriteLine("Please input accessPassword:");
                                            string accessPassword = Console.ReadLine();

                                            if (My_Reader._TagGB.WriteEPC_MatchEPC(ConnID, antNum, sWriteData, sMatchData, matchWordStartIndex, accessPassword) == 0)
                                            {
                                                Console.WriteLine("Success!");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Failure!");
                                            }
                                        }
                                        else if (readKey.Equals("4"))
                                        {
                                            eAntennaNo antNum = new eAntennaNo();
                                            while (true)
                                            {
                                                Console.WriteLine("Please input antenna number:");
                                                readKey = Console.ReadLine();
                                                if (readKey.Equals("1"))
                                                {
                                                    antNum |= eAntennaNo._1;
                                                }
                                                else if (readKey.Equals("2"))
                                                {
                                                    antNum |= eAntennaNo._2;
                                                }
                                                else if (readKey.Equals("3"))
                                                {
                                                    antNum |= eAntennaNo._3;
                                                }
                                                else if (readKey.Equals("4"))
                                                {
                                                    antNum |= eAntennaNo._4;
                                                }
                                                else if (readKey.Equals("5"))
                                                {
                                                    antNum |= eAntennaNo._5;
                                                }
                                                else if (readKey.Equals("6"))
                                                {
                                                    antNum |= eAntennaNo._6;
                                                }
                                                else if (readKey.Equals("7"))
                                                {
                                                    antNum |= eAntennaNo._7;
                                                }
                                                else if (readKey.Equals("8"))
                                                {
                                                    antNum |= eAntennaNo._8;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("antenna number error");
                                                    continue;
                                                }

                                                Console.WriteLine("Setting another antenna");
                                                Console.WriteLine("1.Yes");
                                                Console.WriteLine("2.No");
                                                readKey = Console.ReadLine();
                                                if (readKey.Equals("1"))
                                                {
                                                    continue;
                                                }
                                                else if (readKey.Equals("2"))
                                                {
                                                    break;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Parameter error ,please reset");
                                                    antNum = new eAntennaNo();
                                                }
                                            }

                                            Console.WriteLine("Please input sWriteData:");
                                            string sWriteData = Console.ReadLine();

                                            Console.WriteLine("Please input sMatchData:");
                                            string sMatchData = Console.ReadLine();

                                            Console.WriteLine("Please input matchWordStartIndex:");
                                            int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                            if (My_Reader._TagGB.WriteEPC_MatchTID(ConnID, antNum, sWriteData, sMatchData, matchWordStartIndex) == 0)
                                            {
                                                Console.WriteLine("Success!");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Failure!");
                                            }
                                        }
                                        else if (readKey.Equals("5"))
                                        {
                                            eAntennaNo antNum = new eAntennaNo();
                                            while (true)
                                            {
                                                Console.WriteLine("Please input antenna number:");
                                                readKey = Console.ReadLine();
                                                if (readKey.Equals("1"))
                                                {
                                                    antNum |= eAntennaNo._1;
                                                }
                                                else if (readKey.Equals("2"))
                                                {
                                                    antNum |= eAntennaNo._2;
                                                }
                                                else if (readKey.Equals("3"))
                                                {
                                                    antNum |= eAntennaNo._3;
                                                }
                                                else if (readKey.Equals("4"))
                                                {
                                                    antNum |= eAntennaNo._4;
                                                }
                                                else if (readKey.Equals("5"))
                                                {
                                                    antNum |= eAntennaNo._5;
                                                }
                                                else if (readKey.Equals("6"))
                                                {
                                                    antNum |= eAntennaNo._6;
                                                }
                                                else if (readKey.Equals("7"))
                                                {
                                                    antNum |= eAntennaNo._7;
                                                }
                                                else if (readKey.Equals("8"))
                                                {
                                                    antNum |= eAntennaNo._8;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("antenna number error");
                                                    continue;
                                                }

                                                Console.WriteLine("Setting another antenna");
                                                Console.WriteLine("1.Yes");
                                                Console.WriteLine("2.No");
                                                readKey = Console.ReadLine();
                                                if (readKey.Equals("1"))
                                                {
                                                    continue;
                                                }
                                                else if (readKey.Equals("2"))
                                                {
                                                    break;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Parameter error ,please reset");
                                                    antNum = new eAntennaNo();
                                                }
                                            }

                                            Console.WriteLine("Please input sWriteData:");
                                            string sWriteData = Console.ReadLine();

                                            Console.WriteLine("Please input sMatchData:");
                                            string sMatchData = Console.ReadLine();

                                            Console.WriteLine("Please input matchWordStartIndex:");
                                            int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                            Console.WriteLine("Please input accessPassword:");
                                            string accessPassword = Console.ReadLine();

                                            if (My_Reader._TagGB.WriteEPC_MatchTID(ConnID, antNum, sWriteData, sMatchData, matchWordStartIndex, accessPassword) == 0)
                                            {
                                                Console.WriteLine("Success!");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Failure!");
                                            }
                                        }
                                    }
                                    else if (readKey.Equals("2"))
                                    {
                                        Console.WriteLine("Please select method:");
                                        Console.WriteLine("1.WriteUserData(String ConnID, eAntennaNo antNum, String sWriteData , Int32 offset)");
                                        Console.WriteLine("2.WriteUserData_MatchEPC(String ConnID, eAntennaNo antNum, String sWriteData, Int32 offset, String sMatchData, Int32 matchWordStartIndex) ");
                                        Console.WriteLine("3.WriteUserData_MatchEPC(String ConnID, eAntennaNo antNum, String sWriteData, Int32 offset, String sMatchData, Int32 matchWordStartIndex, String accessPassword) ");
                                        Console.WriteLine("4.WriteUserData_MatchTID(String ConnID, eAntennaNo antNum, String sWriteData, Int32 offset, String sMatchData, Int32 matchWordStartIndex) ");
                                        Console.WriteLine("5.WriteUserData_MatchTID(String ConnID, eAntennaNo antNum, String sWriteData, Int32 offset, String sMatchData, Int32 matchWordStartIndex, String accessPassword) ");
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            eAntennaNo antNum = new eAntennaNo();
                                            while (true)
                                            {
                                                Console.WriteLine("Please input antenna number:");
                                                readKey = Console.ReadLine();
                                                if (readKey.Equals("1"))
                                                {
                                                    antNum |= eAntennaNo._1;
                                                }
                                                else if (readKey.Equals("2"))
                                                {
                                                    antNum |= eAntennaNo._2;
                                                }
                                                else if (readKey.Equals("3"))
                                                {
                                                    antNum |= eAntennaNo._3;
                                                }
                                                else if (readKey.Equals("4"))
                                                {
                                                    antNum |= eAntennaNo._4;
                                                }
                                                else if (readKey.Equals("5"))
                                                {
                                                    antNum |= eAntennaNo._5;
                                                }
                                                else if (readKey.Equals("6"))
                                                {
                                                    antNum |= eAntennaNo._6;
                                                }
                                                else if (readKey.Equals("7"))
                                                {
                                                    antNum |= eAntennaNo._7;
                                                }
                                                else if (readKey.Equals("8"))
                                                {
                                                    antNum |= eAntennaNo._8;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("antenna number error");
                                                    continue;
                                                }

                                                Console.WriteLine("Setting another antenna");
                                                Console.WriteLine("1.Yes");
                                                Console.WriteLine("2.No");
                                                readKey = Console.ReadLine();
                                                if (readKey.Equals("1"))
                                                {
                                                    continue;
                                                }
                                                else if (readKey.Equals("2"))
                                                {
                                                    break;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Parameter error ,please reset");
                                                    antNum = new eAntennaNo();
                                                }
                                            }

                                            Console.WriteLine("Please input childAreaNum:");
                                            int childAreaNum = Convert.ToInt32(Console.ReadLine());

                                            Console.WriteLine("Please input sWriteData:");
                                            string sWriteData = Console.ReadLine();


                                            if (My_Reader._TagGB.WriteUserData(ConnID, antNum, childAreaNum, sWriteData, 0) == 0)
                                            {
                                                Console.WriteLine("Success!");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Failure!");
                                            }

                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            eAntennaNo antNum = new eAntennaNo();
                                            while (true)
                                            {
                                                Console.WriteLine("Please input antenna number:");
                                                readKey = Console.ReadLine();
                                                if (readKey.Equals("1"))
                                                {
                                                    antNum |= eAntennaNo._1;
                                                }
                                                else if (readKey.Equals("2"))
                                                {
                                                    antNum |= eAntennaNo._2;
                                                }
                                                else if (readKey.Equals("3"))
                                                {
                                                    antNum |= eAntennaNo._3;
                                                }
                                                else if (readKey.Equals("4"))
                                                {
                                                    antNum |= eAntennaNo._4;
                                                }
                                                else if (readKey.Equals("5"))
                                                {
                                                    antNum |= eAntennaNo._5;
                                                }
                                                else if (readKey.Equals("6"))
                                                {
                                                    antNum |= eAntennaNo._6;
                                                }
                                                else if (readKey.Equals("7"))
                                                {
                                                    antNum |= eAntennaNo._7;
                                                }
                                                else if (readKey.Equals("8"))
                                                {
                                                    antNum |= eAntennaNo._8;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("antenna number error");
                                                    continue;
                                                }

                                                Console.WriteLine("Setting another antenna");
                                                Console.WriteLine("1.Yes");
                                                Console.WriteLine("2.No");
                                                readKey = Console.ReadLine();
                                                if (readKey.Equals("1"))
                                                {
                                                    continue;
                                                }
                                                else if (readKey.Equals("2"))
                                                {
                                                    break;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Parameter error ,please reset");
                                                    antNum = new eAntennaNo();
                                                }
                                            }

                                            Console.WriteLine("Please input childAreaNum:");
                                            int childAreaNum = Convert.ToInt32(Console.ReadLine());

                                            Console.WriteLine("Please input sWriteData:");
                                            string sWriteData = Console.ReadLine();

                                            Console.WriteLine("Please input sMatchData:");
                                            string sMatchData = Console.ReadLine();

                                            Console.WriteLine("Please input matchWordStartIndex:");
                                            int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                            if (My_Reader._TagGB.WriteUserData_MatchEPC(ConnID, antNum, childAreaNum, sWriteData, 0, sMatchData, matchWordStartIndex) == 0)
                                            {
                                                Console.WriteLine("Success!");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Failure!");
                                            }
                                        }
                                        else if (readKey.Equals("3"))
                                        {
                                            eAntennaNo antNum = new eAntennaNo();
                                            while (true)
                                            {
                                                Console.WriteLine("Please input antenna number:");
                                                readKey = Console.ReadLine();
                                                if (readKey.Equals("1"))
                                                {
                                                    antNum |= eAntennaNo._1;
                                                }
                                                else if (readKey.Equals("2"))
                                                {
                                                    antNum |= eAntennaNo._2;
                                                }
                                                else if (readKey.Equals("3"))
                                                {
                                                    antNum |= eAntennaNo._3;
                                                }
                                                else if (readKey.Equals("4"))
                                                {
                                                    antNum |= eAntennaNo._4;
                                                }
                                                else if (readKey.Equals("5"))
                                                {
                                                    antNum |= eAntennaNo._5;
                                                }
                                                else if (readKey.Equals("6"))
                                                {
                                                    antNum |= eAntennaNo._6;
                                                }
                                                else if (readKey.Equals("7"))
                                                {
                                                    antNum |= eAntennaNo._7;
                                                }
                                                else if (readKey.Equals("8"))
                                                {
                                                    antNum |= eAntennaNo._8;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("antenna number error");
                                                    continue;
                                                }

                                                Console.WriteLine("Setting another antenna");
                                                Console.WriteLine("1.Yes");
                                                Console.WriteLine("2.No");
                                                readKey = Console.ReadLine();
                                                if (readKey.Equals("1"))
                                                {
                                                    continue;
                                                }
                                                else if (readKey.Equals("2"))
                                                {
                                                    break;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Parameter error ,please reset");
                                                    antNum = new eAntennaNo();
                                                }
                                            }

                                            Console.WriteLine("Please input childAreaNum:");
                                            int childAreaNum = Convert.ToInt32(Console.ReadLine());

                                            Console.WriteLine("Please input sWriteData:");
                                            string sWriteData = Console.ReadLine();

                                            Console.WriteLine("Please input sMatchData:");
                                            string sMatchData = Console.ReadLine();

                                            Console.WriteLine("Please input matchWordStartIndex:");
                                            int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                            Console.WriteLine("Please input accessPassword:");
                                            string accessPassword = Console.ReadLine();

                                            if (My_Reader._TagGB.WriteUserData_MatchEPC(ConnID, antNum, childAreaNum, sWriteData, 0, sMatchData, matchWordStartIndex, accessPassword) == 0)
                                            {
                                                Console.WriteLine("Success!");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Failure!");
                                            }
                                        }
                                        else if (readKey.Equals("4"))
                                        {
                                            eAntennaNo antNum = new eAntennaNo();
                                            while (true)
                                            {
                                                Console.WriteLine("Please input antenna number:");
                                                readKey = Console.ReadLine();
                                                if (readKey.Equals("1"))
                                                {
                                                    antNum |= eAntennaNo._1;
                                                }
                                                else if (readKey.Equals("2"))
                                                {
                                                    antNum |= eAntennaNo._2;
                                                }
                                                else if (readKey.Equals("3"))
                                                {
                                                    antNum |= eAntennaNo._3;
                                                }
                                                else if (readKey.Equals("4"))
                                                {
                                                    antNum |= eAntennaNo._4;
                                                }
                                                else if (readKey.Equals("5"))
                                                {
                                                    antNum |= eAntennaNo._5;
                                                }
                                                else if (readKey.Equals("6"))
                                                {
                                                    antNum |= eAntennaNo._6;
                                                }
                                                else if (readKey.Equals("7"))
                                                {
                                                    antNum |= eAntennaNo._7;
                                                }
                                                else if (readKey.Equals("8"))
                                                {
                                                    antNum |= eAntennaNo._8;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("antenna number error");
                                                    continue;
                                                }

                                                Console.WriteLine("Setting another antenna");
                                                Console.WriteLine("1.Yes");
                                                Console.WriteLine("2.No");
                                                readKey = Console.ReadLine();
                                                if (readKey.Equals("1"))
                                                {
                                                    continue;
                                                }
                                                else if (readKey.Equals("2"))
                                                {
                                                    break;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Parameter error ,please reset");
                                                    antNum = new eAntennaNo();
                                                }
                                            }

                                            Console.WriteLine("Please input childAreaNum:");
                                            int childAreaNum = Convert.ToInt32(Console.ReadLine());

                                            Console.WriteLine("Please input sWriteData:");
                                            string sWriteData = Console.ReadLine();

                                            Console.WriteLine("Please input sMatchData:");
                                            string sMatchData = Console.ReadLine();

                                            Console.WriteLine("Please input matchWordStartIndex:");
                                            int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                            if (My_Reader._TagGB.WriteUserData_MatchTID(ConnID, antNum, childAreaNum, sWriteData, 0, sMatchData, matchWordStartIndex) == 0)
                                            {
                                                Console.WriteLine("Success!");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Failure!");
                                            }
                                        }
                                        else if (readKey.Equals("5"))
                                        {
                                            eAntennaNo antNum = new eAntennaNo();
                                            while (true)
                                            {
                                                Console.WriteLine("Please input antenna number:");
                                                readKey = Console.ReadLine();
                                                if (readKey.Equals("1"))
                                                {
                                                    antNum |= eAntennaNo._1;
                                                }
                                                else if (readKey.Equals("2"))
                                                {
                                                    antNum |= eAntennaNo._2;
                                                }
                                                else if (readKey.Equals("3"))
                                                {
                                                    antNum |= eAntennaNo._3;
                                                }
                                                else if (readKey.Equals("4"))
                                                {
                                                    antNum |= eAntennaNo._4;
                                                }
                                                else if (readKey.Equals("5"))
                                                {
                                                    antNum |= eAntennaNo._5;
                                                }
                                                else if (readKey.Equals("6"))
                                                {
                                                    antNum |= eAntennaNo._6;
                                                }
                                                else if (readKey.Equals("7"))
                                                {
                                                    antNum |= eAntennaNo._7;
                                                }
                                                else if (readKey.Equals("8"))
                                                {
                                                    antNum |= eAntennaNo._8;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("antenna number error");
                                                    continue;
                                                }

                                                Console.WriteLine("Setting another antenna");
                                                Console.WriteLine("1.Yes");
                                                Console.WriteLine("2.No");
                                                readKey = Console.ReadLine();
                                                if (readKey.Equals("1"))
                                                {
                                                    continue;
                                                }
                                                else if (readKey.Equals("2"))
                                                {
                                                    break;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Parameter error ,please reset");
                                                    antNum = new eAntennaNo();
                                                }
                                            }

                                            Console.WriteLine("Please input childAreaNum:");
                                            int childAreaNum = Convert.ToInt32(Console.ReadLine());

                                            Console.WriteLine("Please input sWriteData:");
                                            string sWriteData = Console.ReadLine();

                                            Console.WriteLine("Please input sMatchData:");
                                            string sMatchData = Console.ReadLine();

                                            Console.WriteLine("Please input matchWordStartIndex:");
                                            int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                            Console.WriteLine("Please input accessPassword:");
                                            string accessPassword = Console.ReadLine();

                                            if (My_Reader._TagGB.WriteUserData_MatchTID(ConnID, antNum, childAreaNum, sWriteData, 0, sMatchData, matchWordStartIndex, accessPassword) == 0)
                                            {
                                                Console.WriteLine("Success!");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Failure!");
                                            }
                                        }
                                    }
                                    else if (readKey.Equals("b"))
                                    {
                                        break;
                                    }
                                    else if (readKey.Equals("q"))
                                    {
                                        return;
                                    }
                                }
                            }

                            #endregion

                            #region Lock GB

                            else if (readKey.Equals("3"))
                            {
                                while (true)
                                {
                                    Console.WriteLine("Please select method:");
                                    Console.WriteLine("1.Lock(String ConnID, eAntennaNo antNum, eLockArea lockArea, eLockType lockType)");
                                    Console.WriteLine("2.Lock_MatchEPC(String ConnID, eAntennaNo antNum, eLockArea lockArea, eLockType lockType, String sMatchData, Int32 matchWordStartIndex)");
                                    Console.WriteLine("3.Lock_MatchEPC(String ConnID, eAntennaNo antNum, eLockArea lockArea, eLockType lockType, String sMatchData, Int32 matchWordStartIndex, String accessPassword)");
                                    Console.WriteLine("4.Lock_MatchTID(String ConnID, eAntennaNo antNum, eLockArea lockArea, eLockType lockType, String sMatchData, Int32 matchWordStartIndex)");
                                    Console.WriteLine("5.Lock_MatchTID(String ConnID, eAntennaNo antNum, eLockArea lockArea, eLockType lockType, String sMatchData, Int32 matchWordStartIndex, String accessPassword)");
                                    Console.WriteLine("b.Back to Previous Menu");
                                    Console.WriteLine("q.Quit");
                                    readKey = Console.ReadLine();

                                    if (readKey.Equals("1"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = new eAntennaNo();
                                            }
                                        }

                                        Console.WriteLine("Please select lockArea:");
                                        Console.WriteLine("1.eLockAreaGB.epc");
                                        Console.WriteLine("2.eLockAreaGB.tid");
                                        Console.WriteLine("3.eLockAreaGB.Password");
                                        Console.WriteLine("4.eLockAreaGB.userdata00");
                                        Console.WriteLine("5.eLockAreaGB.userdata01");
                                        Console.WriteLine("6.eLockAreaGB.userdata02");
                                        Console.WriteLine("7.eLockAreaGB.userdata03");
                                        Console.WriteLine("8.eLockAreaGB.userdata04");
                                        Console.WriteLine("9.eLockAreaGB.userdata05");
                                        Console.WriteLine("10.eLockAreaGB.userdata06");
                                        Console.WriteLine("11.eLockAreaGB.userdata07");
                                        Console.WriteLine("12.eLockAreaGB.userdata08");
                                        Console.WriteLine("13.eLockAreaGB.userdata09");
                                        Console.WriteLine("14.eLockAreaGB.userdata10");
                                        Console.WriteLine("15.eLockAreaGB.userdata11");
                                        Console.WriteLine("16.eLockAreaGB.userdata12");
                                        Console.WriteLine("17.eLockAreaGB.userdata13");
                                        Console.WriteLine("18.eLockAreaGB.userdata14");
                                        Console.WriteLine("19.eLockAreaGB.userdata15");

                                        eLockAreaGB lockArea = new eLockAreaGB();
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            lockArea = eLockAreaGB.epc;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            lockArea = eLockAreaGB.tid;
                                        }
                                        else if (readKey.Equals("3"))
                                        {
                                            lockArea = eLockAreaGB.Password;
                                        }
                                        else if (readKey.Equals("4"))
                                        {
                                            lockArea = eLockAreaGB.userdata00;
                                        }
                                        else if (readKey.Equals("5"))
                                        {
                                            lockArea = eLockAreaGB.userdata01;
                                        }
                                        else if (readKey.Equals("6"))
                                        {
                                            lockArea = eLockAreaGB.userdata02;
                                        }
                                        else if (readKey.Equals("7"))
                                        {
                                            lockArea = eLockAreaGB.userdata03;
                                        }
                                        else if (readKey.Equals("8"))
                                        {
                                            lockArea = eLockAreaGB.userdata04;
                                        }
                                        else if (readKey.Equals("9"))
                                        {
                                            lockArea = eLockAreaGB.userdata05;
                                        }
                                        else if (readKey.Equals("10"))
                                        {
                                            lockArea = eLockAreaGB.userdata06;
                                        }
                                        else if (readKey.Equals("11"))
                                        {
                                            lockArea = eLockAreaGB.userdata07;
                                        }
                                        else if (readKey.Equals("12"))
                                        {
                                            lockArea = eLockAreaGB.userdata08;
                                        }
                                        else if (readKey.Equals("13"))
                                        {
                                            lockArea = eLockAreaGB.userdata09;
                                        }
                                        else if (readKey.Equals("14"))
                                        {
                                            lockArea = eLockAreaGB.userdata10;
                                        }
                                        else if (readKey.Equals("15"))
                                        {
                                            lockArea = eLockAreaGB.userdata11;
                                        }
                                        else if (readKey.Equals("16"))
                                        {
                                            lockArea = eLockAreaGB.userdata12;
                                        }
                                        else if (readKey.Equals("17"))
                                        {
                                            lockArea = eLockAreaGB.userdata13;
                                        }
                                        else if (readKey.Equals("18"))
                                        {
                                            lockArea = eLockAreaGB.userdata14;
                                        }
                                        else if (readKey.Equals("19"))
                                        {
                                            lockArea = eLockAreaGB.userdata15;
                                        }
                                        else
                                        {
                                            Console.WriteLine("lockArea error");
                                            continue;
                                        }


                                        Console.WriteLine("Please select lockType:");
                                        Console.WriteLine("1.eLockTypeGB.ReadOnly");
                                        Console.WriteLine("2.eLockTypeGB.WriteOnly");
                                        Console.WriteLine("3.eLockTypeGB.Readable_Writable");
                                        Console.WriteLine("4.eLockTypeGB.Unreadable_Unwritable");
                                        Console.WriteLine("5.eLockTypeGB.Safemode_0x11");
                                        Console.WriteLine("6.eLockTypeGB.Safemode_0x12");
                                        Console.WriteLine("7.eLockTypeGB.Safemode_0x13");
                                        eLockTypeGB lockType = new eLockTypeGB();
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            lockType = eLockTypeGB.ReadOnly;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            lockType = eLockTypeGB.WriteOnly;
                                        }
                                        else if (readKey.Equals("3"))
                                        {
                                            lockType = eLockTypeGB.Readable_Writable;
                                        }
                                        else if (readKey.Equals("4"))
                                        {
                                            lockType = eLockTypeGB.Unreadable_Unwritable;
                                        }
                                        else if (readKey.Equals("5"))
                                        {
                                            lockType = eLockTypeGB.Safemode_0x11;
                                        }
                                        else if (readKey.Equals("6"))
                                        {
                                            lockType = eLockTypeGB.Safemode_0x12;
                                        }
                                        else if (readKey.Equals("7"))
                                        {
                                            lockType = eLockTypeGB.Safemode_0x13;
                                        }
                                        else
                                        {
                                            Console.WriteLine("lockType error");
                                            continue;
                                        }


                                        if (My_Reader._TagGB.Lock(ConnID, antNum, lockArea, lockType) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }

                                    }
                                    else if (readKey.Equals("2"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = new eAntennaNo();
                                            }
                                        }

                                        Console.WriteLine("Please select lockArea:");
                                        Console.WriteLine("1.eLockAreaGB.epc");
                                        Console.WriteLine("2.eLockAreaGB.tid");
                                        Console.WriteLine("3.eLockAreaGB.Password");
                                        Console.WriteLine("4.eLockAreaGB.userdata00;");
                                        Console.WriteLine("5.eLockAreaGB.userdata01;");
                                        Console.WriteLine("6.eLockAreaGB.userdata02;");
                                        Console.WriteLine("7.eLockAreaGB.userdata03;");
                                        Console.WriteLine("8.eLockAreaGB.userdata04;");
                                        Console.WriteLine("9.eLockAreaGB.userdata05;");
                                        Console.WriteLine("10.eLockAreaGB.userdata06;");
                                        Console.WriteLine("11.eLockAreaGB.userdata07;");
                                        Console.WriteLine("12.eLockAreaGB.userdata08;");
                                        Console.WriteLine("13.eLockAreaGB.userdata09;");
                                        Console.WriteLine("14.eLockAreaGB.userdata10;");
                                        Console.WriteLine("15.eLockAreaGB.userdata11;");
                                        Console.WriteLine("16.eLockAreaGB.userdata12;");
                                        Console.WriteLine("17.eLockAreaGB.userdata13;");
                                        Console.WriteLine("18.eLockAreaGB.userdata14;");
                                        Console.WriteLine("19.eLockAreaGB.userdata15;");

                                        eLockAreaGB lockArea = new eLockAreaGB();
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            lockArea = eLockAreaGB.epc;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            lockArea = eLockAreaGB.tid;
                                        }
                                        else if (readKey.Equals("3"))
                                        {
                                            lockArea = eLockAreaGB.Password;
                                        }
                                        else if (readKey.Equals("4"))
                                        {
                                            lockArea = eLockAreaGB.userdata00;
                                        }
                                        else if (readKey.Equals("5"))
                                        {
                                            lockArea = eLockAreaGB.userdata01;
                                        }
                                        else if (readKey.Equals("6"))
                                        {
                                            lockArea = eLockAreaGB.userdata02;
                                        }
                                        else if (readKey.Equals("7"))
                                        {
                                            lockArea = eLockAreaGB.userdata03;
                                        }
                                        else if (readKey.Equals("8"))
                                        {
                                            lockArea = eLockAreaGB.userdata04;
                                        }
                                        else if (readKey.Equals("9"))
                                        {
                                            lockArea = eLockAreaGB.userdata05;
                                        }
                                        else if (readKey.Equals("10"))
                                        {
                                            lockArea = eLockAreaGB.userdata06;
                                        }
                                        else if (readKey.Equals("11"))
                                        {
                                            lockArea = eLockAreaGB.userdata07;
                                        }
                                        else if (readKey.Equals("12"))
                                        {
                                            lockArea = eLockAreaGB.userdata08;
                                        }
                                        else if (readKey.Equals("13"))
                                        {
                                            lockArea = eLockAreaGB.userdata09;
                                        }
                                        else if (readKey.Equals("14"))
                                        {
                                            lockArea = eLockAreaGB.userdata10;
                                        }
                                        else if (readKey.Equals("15"))
                                        {
                                            lockArea = eLockAreaGB.userdata11;
                                        }
                                        else if (readKey.Equals("16"))
                                        {
                                            lockArea = eLockAreaGB.userdata12;
                                        }
                                        else if (readKey.Equals("17"))
                                        {
                                            lockArea = eLockAreaGB.userdata13;
                                        }
                                        else if (readKey.Equals("18"))
                                        {
                                            lockArea = eLockAreaGB.userdata14;
                                        }
                                        else if (readKey.Equals("19"))
                                        {
                                            lockArea = eLockAreaGB.userdata15;
                                        }
                                        else
                                        {
                                            Console.WriteLine("lockArea error");
                                            continue;
                                        }


                                        Console.WriteLine("Please select lockType:");
                                        Console.WriteLine("1.eLockTypeGB.ReadOnly;");
                                        Console.WriteLine("2.eLockTypeGB.WriteOnly;");
                                        Console.WriteLine("3.eLockTypeGB.Readable_Writable");
                                        Console.WriteLine("4.eLockTypeGB.Unreadable_Unwritable");
                                        Console.WriteLine("5.eLockTypeGB.Safemode_0x11;");
                                        Console.WriteLine("6.eLockTypeGB.Safemode_0x12");
                                        Console.WriteLine("7.eLockTypeGB.Safemode_0x13");
                                        eLockTypeGB lockType = new eLockTypeGB();
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            lockType = eLockTypeGB.ReadOnly;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            lockType = eLockTypeGB.WriteOnly;
                                        }
                                        else if (readKey.Equals("3"))
                                        {
                                            lockType = eLockTypeGB.Readable_Writable;
                                        }
                                        else if (readKey.Equals("4"))
                                        {
                                            lockType = eLockTypeGB.Unreadable_Unwritable;
                                        }
                                        else if (readKey.Equals("5"))
                                        {
                                            lockType = eLockTypeGB.Safemode_0x11;
                                        }
                                        else if (readKey.Equals("6"))
                                        {
                                            lockType = eLockTypeGB.Safemode_0x12;
                                        }
                                        else if (readKey.Equals("7"))
                                        {
                                            lockType = eLockTypeGB.Safemode_0x13;
                                        }
                                        else
                                        {
                                            Console.WriteLine("lockType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input sMatchData");
                                        string sMatchData = Console.ReadLine();

                                        Console.WriteLine("Please input matchWordStartIndex");
                                        int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                        if (My_Reader._TagGB.Lock_MatchEPC(ConnID, antNum, lockArea, lockType, sMatchData, matchWordStartIndex) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("3"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = new eAntennaNo();
                                            }
                                        }

                                        Console.WriteLine("Please select lockArea:");
                                        Console.WriteLine("1.eLockAreaGB.epc");
                                        Console.WriteLine("2.eLockAreaGB.tid");
                                        Console.WriteLine("3.eLockAreaGB.Password");
                                        Console.WriteLine("4.eLockAreaGB.userdata00;");
                                        Console.WriteLine("5.eLockAreaGB.userdata01;");
                                        Console.WriteLine("6.eLockAreaGB.userdata02;");
                                        Console.WriteLine("7.eLockAreaGB.userdata03;");
                                        Console.WriteLine("8.eLockAreaGB.userdata04;");
                                        Console.WriteLine("9.eLockAreaGB.userdata05;");
                                        Console.WriteLine("10.eLockAreaGB.userdata06;");
                                        Console.WriteLine("11.eLockAreaGB.userdata07;");
                                        Console.WriteLine("12.eLockAreaGB.userdata08;");
                                        Console.WriteLine("13.eLockAreaGB.userdata09;");
                                        Console.WriteLine("14.eLockAreaGB.userdata10;");
                                        Console.WriteLine("15.eLockAreaGB.userdata11;");
                                        Console.WriteLine("16.eLockAreaGB.userdata12;");
                                        Console.WriteLine("17.eLockAreaGB.userdata13;");
                                        Console.WriteLine("18.eLockAreaGB.userdata14;");
                                        Console.WriteLine("19.eLockAreaGB.userdata15;");

                                        eLockAreaGB lockArea = new eLockAreaGB();
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            lockArea = eLockAreaGB.epc;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            lockArea = eLockAreaGB.tid;
                                        }
                                        else if (readKey.Equals("3"))
                                        {
                                            lockArea = eLockAreaGB.Password;
                                        }
                                        else if (readKey.Equals("4"))
                                        {
                                            lockArea = eLockAreaGB.userdata00;
                                        }
                                        else if (readKey.Equals("5"))
                                        {
                                            lockArea = eLockAreaGB.userdata01;
                                        }
                                        else if (readKey.Equals("6"))
                                        {
                                            lockArea = eLockAreaGB.userdata02;
                                        }
                                        else if (readKey.Equals("7"))
                                        {
                                            lockArea = eLockAreaGB.userdata03;
                                        }
                                        else if (readKey.Equals("8"))
                                        {
                                            lockArea = eLockAreaGB.userdata04;
                                        }
                                        else if (readKey.Equals("9"))
                                        {
                                            lockArea = eLockAreaGB.userdata05;
                                        }
                                        else if (readKey.Equals("10"))
                                        {
                                            lockArea = eLockAreaGB.userdata06;
                                        }
                                        else if (readKey.Equals("11"))
                                        {
                                            lockArea = eLockAreaGB.userdata07;
                                        }
                                        else if (readKey.Equals("12"))
                                        {
                                            lockArea = eLockAreaGB.userdata08;
                                        }
                                        else if (readKey.Equals("13"))
                                        {
                                            lockArea = eLockAreaGB.userdata09;
                                        }
                                        else if (readKey.Equals("14"))
                                        {
                                            lockArea = eLockAreaGB.userdata10;
                                        }
                                        else if (readKey.Equals("15"))
                                        {
                                            lockArea = eLockAreaGB.userdata11;
                                        }
                                        else if (readKey.Equals("16"))
                                        {
                                            lockArea = eLockAreaGB.userdata12;
                                        }
                                        else if (readKey.Equals("17"))
                                        {
                                            lockArea = eLockAreaGB.userdata13;
                                        }
                                        else if (readKey.Equals("18"))
                                        {
                                            lockArea = eLockAreaGB.userdata14;
                                        }
                                        else if (readKey.Equals("19"))
                                        {
                                            lockArea = eLockAreaGB.userdata15;
                                        }
                                        else
                                        {
                                            Console.WriteLine("lockArea error");
                                            continue;
                                        }


                                        Console.WriteLine("Please select lockType:");
                                        Console.WriteLine("1.eLockTypeGB.ReadOnly;");
                                        Console.WriteLine("2.eLockTypeGB.WriteOnly;");
                                        Console.WriteLine("3.eLockTypeGB.Readable_Writable");
                                        Console.WriteLine("4.eLockTypeGB.Unreadable_Unwritable");
                                        Console.WriteLine("5.eLockTypeGB.Safemode_0x11;");
                                        Console.WriteLine("6.eLockTypeGB.Safemode_0x12");
                                        Console.WriteLine("7.eLockTypeGB.Safemode_0x13");
                                        eLockTypeGB lockType = new eLockTypeGB();
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            lockType = eLockTypeGB.ReadOnly;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            lockType = eLockTypeGB.WriteOnly;
                                        }
                                        else if (readKey.Equals("3"))
                                        {
                                            lockType = eLockTypeGB.Readable_Writable;
                                        }
                                        else if (readKey.Equals("4"))
                                        {
                                            lockType = eLockTypeGB.Unreadable_Unwritable;
                                        }
                                        else if (readKey.Equals("5"))
                                        {
                                            lockType = eLockTypeGB.Safemode_0x11;
                                        }
                                        else if (readKey.Equals("6"))
                                        {
                                            lockType = eLockTypeGB.Safemode_0x12;
                                        }
                                        else if (readKey.Equals("7"))
                                        {
                                            lockType = eLockTypeGB.Safemode_0x13;
                                        }
                                        else
                                        {
                                            Console.WriteLine("lockType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input sMatchData");
                                        string sMatchData = Console.ReadLine();

                                        Console.WriteLine("Please input matchWordStartIndex");
                                        int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input accessPassword");
                                        string accessPassword = Console.ReadLine();

                                        if (My_Reader._TagGB.Lock_MatchEPC(ConnID, antNum, lockArea, lockType, sMatchData, matchWordStartIndex, accessPassword) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("4"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = new eAntennaNo();
                                            }
                                        }

                                        Console.WriteLine("Please select lockArea:");
                                        Console.WriteLine("1.eLockAreaGB.epc");
                                        Console.WriteLine("2.eLockAreaGB.tid");
                                        Console.WriteLine("3.eLockAreaGB.Password");
                                        Console.WriteLine("4.eLockAreaGB.userdata00;");
                                        Console.WriteLine("5.eLockAreaGB.userdata01;");
                                        Console.WriteLine("6.eLockAreaGB.userdata02;");
                                        Console.WriteLine("7.eLockAreaGB.userdata03;");
                                        Console.WriteLine("8.eLockAreaGB.userdata04;");
                                        Console.WriteLine("9.eLockAreaGB.userdata05;");
                                        Console.WriteLine("10.eLockAreaGB.userdata06;");
                                        Console.WriteLine("11.eLockAreaGB.userdata07;");
                                        Console.WriteLine("12.eLockAreaGB.userdata08;");
                                        Console.WriteLine("13.eLockAreaGB.userdata09;");
                                        Console.WriteLine("14.eLockAreaGB.userdata10;");
                                        Console.WriteLine("15.eLockAreaGB.userdata11;");
                                        Console.WriteLine("16.eLockAreaGB.userdata12;");
                                        Console.WriteLine("17.eLockAreaGB.userdata13;");
                                        Console.WriteLine("18.eLockAreaGB.userdata14;");
                                        Console.WriteLine("19.eLockAreaGB.userdata15;");

                                        eLockAreaGB lockArea = new eLockAreaGB();
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            lockArea = eLockAreaGB.epc;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            lockArea = eLockAreaGB.tid;
                                        }
                                        else if (readKey.Equals("3"))
                                        {
                                            lockArea = eLockAreaGB.Password;
                                        }
                                        else if (readKey.Equals("4"))
                                        {
                                            lockArea = eLockAreaGB.userdata00;
                                        }
                                        else if (readKey.Equals("5"))
                                        {
                                            lockArea = eLockAreaGB.userdata01;
                                        }
                                        else if (readKey.Equals("6"))
                                        {
                                            lockArea = eLockAreaGB.userdata02;
                                        }
                                        else if (readKey.Equals("7"))
                                        {
                                            lockArea = eLockAreaGB.userdata03;
                                        }
                                        else if (readKey.Equals("8"))
                                        {
                                            lockArea = eLockAreaGB.userdata04;
                                        }
                                        else if (readKey.Equals("9"))
                                        {
                                            lockArea = eLockAreaGB.userdata05;
                                        }
                                        else if (readKey.Equals("10"))
                                        {
                                            lockArea = eLockAreaGB.userdata06;
                                        }
                                        else if (readKey.Equals("11"))
                                        {
                                            lockArea = eLockAreaGB.userdata07;
                                        }
                                        else if (readKey.Equals("12"))
                                        {
                                            lockArea = eLockAreaGB.userdata08;
                                        }
                                        else if (readKey.Equals("13"))
                                        {
                                            lockArea = eLockAreaGB.userdata09;
                                        }
                                        else if (readKey.Equals("14"))
                                        {
                                            lockArea = eLockAreaGB.userdata10;
                                        }
                                        else if (readKey.Equals("15"))
                                        {
                                            lockArea = eLockAreaGB.userdata11;
                                        }
                                        else if (readKey.Equals("16"))
                                        {
                                            lockArea = eLockAreaGB.userdata12;
                                        }
                                        else if (readKey.Equals("17"))
                                        {
                                            lockArea = eLockAreaGB.userdata13;
                                        }
                                        else if (readKey.Equals("18"))
                                        {
                                            lockArea = eLockAreaGB.userdata14;
                                        }
                                        else if (readKey.Equals("19"))
                                        {
                                            lockArea = eLockAreaGB.userdata15;
                                        }
                                        else
                                        {
                                            Console.WriteLine("lockArea error");
                                            continue;
                                        }


                                        Console.WriteLine("Please select lockType:");
                                        Console.WriteLine("1.eLockTypeGB.ReadOnly;");
                                        Console.WriteLine("2.eLockTypeGB.WriteOnly;");
                                        Console.WriteLine("3.eLockTypeGB.Readable_Writable");
                                        Console.WriteLine("4.eLockTypeGB.Unreadable_Unwritable");
                                        Console.WriteLine("5.eLockTypeGB.Safemode_0x11;");
                                        Console.WriteLine("6.eLockTypeGB.Safemode_0x12");
                                        Console.WriteLine("7.eLockTypeGB.Safemode_0x13");
                                        eLockTypeGB lockType = new eLockTypeGB();
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            lockType = eLockTypeGB.ReadOnly;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            lockType = eLockTypeGB.WriteOnly;
                                        }
                                        else if (readKey.Equals("3"))
                                        {
                                            lockType = eLockTypeGB.Readable_Writable;
                                        }
                                        else if (readKey.Equals("4"))
                                        {
                                            lockType = eLockTypeGB.Unreadable_Unwritable;
                                        }
                                        else if (readKey.Equals("5"))
                                        {
                                            lockType = eLockTypeGB.Safemode_0x11;
                                        }
                                        else if (readKey.Equals("6"))
                                        {
                                            lockType = eLockTypeGB.Safemode_0x12;
                                        }
                                        else if (readKey.Equals("7"))
                                        {
                                            lockType = eLockTypeGB.Safemode_0x13;
                                        }
                                        else
                                        {
                                            Console.WriteLine("lockType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input sMatchData");
                                        string sMatchData = Console.ReadLine();

                                        Console.WriteLine("Please input matchWordStartIndex");
                                        int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                        if (My_Reader._TagGB.Lock_MatchEPC(ConnID, antNum, lockArea, lockType, sMatchData, matchWordStartIndex) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("5"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = new eAntennaNo();
                                            }
                                        }

                                        Console.WriteLine("Please select lockArea:");
                                        Console.WriteLine("1.eLockAreaGB.epc");
                                        Console.WriteLine("2.eLockAreaGB.tid");
                                        Console.WriteLine("3.eLockAreaGB.Password");
                                        Console.WriteLine("4.eLockAreaGB.userdata00;");
                                        Console.WriteLine("5.eLockAreaGB.userdata01;");
                                        Console.WriteLine("6.eLockAreaGB.userdata02;");
                                        Console.WriteLine("7.eLockAreaGB.userdata03;");
                                        Console.WriteLine("8.eLockAreaGB.userdata04;");
                                        Console.WriteLine("9.eLockAreaGB.userdata05;");
                                        Console.WriteLine("10.eLockAreaGB.userdata06;");
                                        Console.WriteLine("11.eLockAreaGB.userdata07;");
                                        Console.WriteLine("12.eLockAreaGB.userdata08;");
                                        Console.WriteLine("13.eLockAreaGB.userdata09;");
                                        Console.WriteLine("14.eLockAreaGB.userdata10;");
                                        Console.WriteLine("15.eLockAreaGB.userdata11;");
                                        Console.WriteLine("16.eLockAreaGB.userdata12;");
                                        Console.WriteLine("17.eLockAreaGB.userdata13;");
                                        Console.WriteLine("18.eLockAreaGB.userdata14;");
                                        Console.WriteLine("19.eLockAreaGB.userdata15;");

                                        eLockAreaGB lockArea = new eLockAreaGB();
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            lockArea = eLockAreaGB.epc;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            lockArea = eLockAreaGB.tid;
                                        }
                                        else if (readKey.Equals("3"))
                                        {
                                            lockArea = eLockAreaGB.Password;
                                        }
                                        else if (readKey.Equals("4"))
                                        {
                                            lockArea = eLockAreaGB.userdata00;
                                        }
                                        else if (readKey.Equals("5"))
                                        {
                                            lockArea = eLockAreaGB.userdata01;
                                        }
                                        else if (readKey.Equals("6"))
                                        {
                                            lockArea = eLockAreaGB.userdata02;
                                        }
                                        else if (readKey.Equals("7"))
                                        {
                                            lockArea = eLockAreaGB.userdata03;
                                        }
                                        else if (readKey.Equals("8"))
                                        {
                                            lockArea = eLockAreaGB.userdata04;
                                        }
                                        else if (readKey.Equals("9"))
                                        {
                                            lockArea = eLockAreaGB.userdata05;
                                        }
                                        else if (readKey.Equals("10"))
                                        {
                                            lockArea = eLockAreaGB.userdata06;
                                        }
                                        else if (readKey.Equals("11"))
                                        {
                                            lockArea = eLockAreaGB.userdata07;
                                        }
                                        else if (readKey.Equals("12"))
                                        {
                                            lockArea = eLockAreaGB.userdata08;
                                        }
                                        else if (readKey.Equals("13"))
                                        {
                                            lockArea = eLockAreaGB.userdata09;
                                        }
                                        else if (readKey.Equals("14"))
                                        {
                                            lockArea = eLockAreaGB.userdata10;
                                        }
                                        else if (readKey.Equals("15"))
                                        {
                                            lockArea = eLockAreaGB.userdata11;
                                        }
                                        else if (readKey.Equals("16"))
                                        {
                                            lockArea = eLockAreaGB.userdata12;
                                        }
                                        else if (readKey.Equals("17"))
                                        {
                                            lockArea = eLockAreaGB.userdata13;
                                        }
                                        else if (readKey.Equals("18"))
                                        {
                                            lockArea = eLockAreaGB.userdata14;
                                        }
                                        else if (readKey.Equals("19"))
                                        {
                                            lockArea = eLockAreaGB.userdata15;
                                        }
                                        else
                                        {
                                            Console.WriteLine("lockArea error");
                                            continue;
                                        }


                                        Console.WriteLine("Please select lockType:");
                                        Console.WriteLine("1.eLockTypeGB.ReadOnly;");
                                        Console.WriteLine("2.eLockTypeGB.WriteOnly;");
                                        Console.WriteLine("3.eLockTypeGB.Readable_Writable");
                                        Console.WriteLine("4.eLockTypeGB.Unreadable_Unwritable");
                                        Console.WriteLine("5.eLockTypeGB.Safemode_0x11;");
                                        Console.WriteLine("6.eLockTypeGB.Safemode_0x12");
                                        Console.WriteLine("7.eLockTypeGB.Safemode_0x13");
                                        eLockTypeGB lockType = new eLockTypeGB();
                                        readKey = Console.ReadLine();
                                        if (readKey.Equals("1"))
                                        {
                                            lockType = eLockTypeGB.ReadOnly;
                                        }
                                        else if (readKey.Equals("2"))
                                        {
                                            lockType = eLockTypeGB.WriteOnly;
                                        }
                                        else if (readKey.Equals("3"))
                                        {
                                            lockType = eLockTypeGB.Readable_Writable;
                                        }
                                        else if (readKey.Equals("4"))
                                        {
                                            lockType = eLockTypeGB.Unreadable_Unwritable;
                                        }
                                        else if (readKey.Equals("5"))
                                        {
                                            lockType = eLockTypeGB.Safemode_0x11;
                                        }
                                        else if (readKey.Equals("6"))
                                        {
                                            lockType = eLockTypeGB.Safemode_0x12;
                                        }
                                        else if (readKey.Equals("7"))
                                        {
                                            lockType = eLockTypeGB.Safemode_0x13;
                                        }
                                        else
                                        {
                                            Console.WriteLine("lockType error");
                                            continue;
                                        }

                                        Console.WriteLine("Please input sMatchData");
                                        string sMatchData = Console.ReadLine();

                                        Console.WriteLine("Please input matchWordStartIndex");
                                        int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                        Console.WriteLine("Please input accessPassword");
                                        string accessPassword = Console.ReadLine();

                                        if (My_Reader._TagGB.Lock_MatchTID(ConnID, antNum, lockArea, lockType, sMatchData, matchWordStartIndex, accessPassword) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("b"))
                                    {
                                        break;
                                    }
                                    else if (readKey.Equals("q"))
                                    {
                                        return;
                                    }
                                }
                            }

                            #endregion

                            #region Kill GB

                            else if (readKey.Equals("4"))
                            {
                                while (true)
                                {
                                    Console.WriteLine("Please select method:");
                                    Console.WriteLine("1.Destroy(String ConnID, eAntennaNo antNum, String destroyPassword)");
                                    Console.WriteLine("2.Destroy_MatchEPC(String ConnID, eAntennaNo antNum, String destroyPassword, String sMatchData, Int32 matchWordStartIndex)");
                                    Console.WriteLine("3.Destroy_MatchTID(String ConnID, eAntennaNo antNum, String destroyPassword, String sMatchData, Int32 matchWordStartIndex)");
                                    Console.WriteLine("b.Back to Previous Menu");
                                    Console.WriteLine("q.Quit");
                                    readKey = Console.ReadLine();
                                    if (readKey.Equals("1"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = new eAntennaNo();
                                            }
                                        }

                                        Console.WriteLine("Please input destroyPassword:");
                                        string destroyPassword = Console.ReadLine();


                                        if (My_Reader._TagGB.Destroy(ConnID, antNum, destroyPassword) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }

                                    }
                                    else if (readKey.Equals("2"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = new eAntennaNo();
                                            }
                                        }

                                        Console.WriteLine("Please input destroyPassword:");
                                        string destroyPassword = Console.ReadLine();

                                        Console.WriteLine("Please input sMatchData:");
                                        string sMatchData = Console.ReadLine();

                                        Console.WriteLine("Please input matchWordStartIndex:");
                                        int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                        if (My_Reader._TagGB.Destroy_MatchEPC(ConnID, antNum, destroyPassword, sMatchData, matchWordStartIndex) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("3"))
                                    {
                                        eAntennaNo antNum = new eAntennaNo();
                                        while (true)
                                        {
                                            Console.WriteLine("Please input antenna number:");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                antNum |= eAntennaNo._1;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                antNum |= eAntennaNo._2;
                                            }
                                            else if (readKey.Equals("3"))
                                            {
                                                antNum |= eAntennaNo._3;
                                            }
                                            else if (readKey.Equals("4"))
                                            {
                                                antNum |= eAntennaNo._4;
                                            }
                                            else if (readKey.Equals("5"))
                                            {
                                                antNum |= eAntennaNo._5;
                                            }
                                            else if (readKey.Equals("6"))
                                            {
                                                antNum |= eAntennaNo._6;
                                            }
                                            else if (readKey.Equals("7"))
                                            {
                                                antNum |= eAntennaNo._7;
                                            }
                                            else if (readKey.Equals("8"))
                                            {
                                                antNum |= eAntennaNo._8;
                                            }
                                            else
                                            {
                                                Console.WriteLine("antenna number error");
                                                continue;
                                            }

                                            Console.WriteLine("Setting another antenna");
                                            Console.WriteLine("1.Yes");
                                            Console.WriteLine("2.No");
                                            readKey = Console.ReadLine();
                                            if (readKey.Equals("1"))
                                            {
                                                continue;
                                            }
                                            else if (readKey.Equals("2"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Parameter error ,please reset");
                                                antNum = new eAntennaNo();
                                            }
                                        }

                                        Console.WriteLine("Please input destroyPassword:");
                                        string destroyPassword = Console.ReadLine();

                                        Console.WriteLine("Please input sMatchData:");
                                        string sMatchData = Console.ReadLine();

                                        Console.WriteLine("Please input matchWordStartIndex:");
                                        int matchWordStartIndex = Convert.ToInt32(Console.ReadLine());

                                        if (My_Reader._TagGB.Destroy_MatchTID(ConnID, antNum, destroyPassword, sMatchData, matchWordStartIndex) == 0)
                                        {
                                            Console.WriteLine("Success!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failure!");
                                        }
                                    }
                                    else if (readKey.Equals("b"))
                                    {
                                        break;
                                    }
                                    else if (readKey.Equals("q"))
                                    {
                                        return;
                                    }
                                }
                            }

                            #endregion

                            else if (readKey.Equals("s"))
                            {
                                My_Reader._Config.Stop(ConnID);
                            }
                            else if (readKey.Equals("b"))
                            {
                                break;
                            }
                            else if (readKey.Equals("q"))
                            {
                                return;
                            }
                        }
                    }

                    #endregion

                    #region Break Point






                    else if (readKey.Equals("8"))
                    {
                        while (true)
                        {
                            Console.WriteLine("Current ConnID : " + ConnID + "\n");
                            Console.WriteLine("Please select operation:");
                            Console.WriteLine("1.Set Break Point Upload");
                            Console.WriteLine("2.Get Break Point Upload");
                            Console.WriteLine("3.Get Break Point Cache");
                            Console.WriteLine("4.Clear Break Point Cache");
                            Console.WriteLine("b.Back to Previous Menu");
                            Console.WriteLine("q.Quit");

                            readKey = Console.ReadLine();
                            if (readKey.Equals("1"))
                            {
                                Console.WriteLine("Please select Switch");
                                Console.WriteLine("1.true");
                                Console.WriteLine("2.false");

                                Boolean Switch;

                                readKey = Console.ReadLine();
                                if (readKey.Equals("1"))
                                {
                                    Switch = true;
                                }
                                else if (readKey.Equals("2"))
                                {
                                    Switch = false;
                                }
                                else
                                {
                                    Console.WriteLine("Parameter error,please reset");
                                    continue;
                                }

                                if (My_Reader._Config.SetBreakPointUpload(ConnID, Switch) == 0)
                                {
                                    Console.WriteLine("Success!");
                                }
                                else
                                {
                                    Console.WriteLine("Failure!");
                                }
                            }
                            else if (readKey.Equals("2"))
                            {
                                Console.WriteLine(My_Reader._Config.GetBreakPointUpload(ConnID));
                            }
                            else if (readKey.Equals("3"))
                            {
                                Console.WriteLine(My_Reader._Config.GetBreakPointCacheTag(ConnID));
                            }
                            else if (readKey.Equals("4"))
                            {
                                if (My_Reader._Config.ClearBreakPointCache(ConnID) == 0)
                                {
                                    Console.WriteLine("Success!");
                                }
                                else
                                {
                                    Console.WriteLine("Failure!");
                                }
                            }
                            else if (readKey.Equals("b"))
                            {
                                break;
                            }
                            else if (readKey.Equals("q"))
                            {
                                My_Reader._Config.Stop(ConnID);
                                My_Reader.CloseConn(ConnID);
                                return;
                            }
                        }
                    }

                    #endregion
                                                                        
                    else if (readKey.Equals("q"))
                    {
                        My_Reader._Config.Stop(ConnID);
                        My_Reader.CloseConn(ConnID);
                        return;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #region Interface

        public void WriteDebugMsg(String msg) 
        {
	    	
	    }

	    public void WriteLog(String msg) 
        {
	    	
	    }
        
	    public void PortConnecting(String connID) 
        {
            Console.WriteLine(connID);            
            if (My_Reader.GetServerStartUp())
            {
                Console.WriteLine("A reader connected to this server: " + connID);
                ConnID = connID;
            }
	    }

	    public void PortClosing(String connID) 
        {
	    	
	    }

	    public void OutPutTags(Tag_Model tag) 
        {	    	
	    	Console.WriteLine("EPC："+ tag.EPC + " TID：" + tag.TID + " ReaderName：" + tag.ReaderName + " Time:" + tag.ReadTime);
            string fileName = "C:/Users/Danil/Desktop/MainLine_PCReaderC#version_V2.18.0_20180711/SampleCode/SampleCode/out.txt";
            FileStream aFile = new FileStream(fileName, FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(aFile);
            aFile.Seek(0, SeekOrigin.End);
            sw.WriteLine( tag.EPC);
            sw.Close();
        }

	    public void OutPutTagsOver() 
        {
	    	
	    }

	    public void GPIControlMsg(GPI_Model gpi_model) 
        {
            Console.WriteLine("gpiindex: " + gpi_model.GpiIndex + " gpistate: " + gpi_model.GpiState + " StartOrStop: " + gpi_model.StartOrStop + " Time: " + gpi_model.UtcTime);
        }

        #endregion
    }
}
